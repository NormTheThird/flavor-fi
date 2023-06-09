using System;
using System.Linq;
using FlavorFi.Common.Enums;
using FlavorFi.Common.Helpers;
using FlavorFi.Common.Models.DatabaseModels;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Data;

namespace FlavorFi.Services.DatabaseServices
{
    public interface ISecurityService
    {
        RegisterAccountResponse RegisterAccount(RegisterAccountRequest request);
        ValidateAccountResponse ValidateAccount(ValidateAccountRequest request);
        PasswordResetResponse PasswordReset(PasswordResetRequest request);
        ValidatePasswordResetResponse ValidatePasswordReset(ValidatePasswordResetRequest request);
        GetSecurityModelResponse GetSecurityModel(GetSecurityModelRequest request);
        UpdateCompanyIdResponse UpdateCompanyId(UpdateCompanyIdRequest request);
        SaveNewPasswordResponse SaveNewPassword(SaveNewPasswordRequest request);
    }

    public class SecurityService : BaseService, ISecurityService
    {
        public RegisterAccountResponse RegisterAccount(RegisterAccountRequest request)
        {
            try
            {
                var now = DateTimeOffset.Now;
                var response = new RegisterAccountResponse();
                using (var context = new FlavorFiEntities())
                {
                    var account = context.Accounts.FirstOrDefault(a => a.Email.Trim().Equals(request.Email.Trim(), StringComparison.CurrentCultureIgnoreCase));
                    if (account != null) return new RegisterAccountResponse { ErrorMessage = "This account already exists." };
                    var companySite = context.CompanySites.FirstOrDefault(cs => cs.RegistrationCode.Equals(request.RegistrationCode));
                    if(companySite == null) return new RegisterAccountResponse { ErrorMessage = "User registration code is invalid." };
                    account = new Account
                    {
                        Id = Guid.NewGuid(),
                        CompanyId = companySite.CompanyId,
                        CompanySiteId = companySite.Id,
                        Address = new Address
                        {
                            Id = Guid.NewGuid(),
                            Address1 = "",
                            Address2 = "",
                            City = "",
                            State = "",
                            ZipCode = "",
                            Latitude = 0.0m,
                            Longitude = 0.0m,
                            DateCreated = now
                        },
                        AWSProfileImageId = null,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        PhoneNumber = "",
                        AltPhoneNumber = "",
                        AllowedOrigin = "*",
                        RefreshTokenLifeTimeMinutes = 525600,
                        Password = Security.HashPassword(request.Password),
                        IsSystemAdmin = false,
                        IsCompanyAdmin = false,
                        IsActive = true,
                        DateOfBirth = null,
                        DateCreated = now,
                    };

                    context.Accounts.Add(account);
                    context.SaveChanges();
                    response.NewAccountId = account.Id;
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new RegisterAccountResponse { ErrorMessage = "Unable to register new user, please contact us at support@unieksoftware.com for help." };
            }
        }

        public ValidateAccountResponse ValidateAccount(ValidateAccountRequest request)
        {
            try
            {
                using (var context = new FlavorFiEntities())
                {
                    var account = context.Accounts.FirstOrDefault(a => a.Email.Trim().Equals(request.Email.Trim(), StringComparison.CurrentCultureIgnoreCase));
                    if (account == null) return new ValidateAccountResponse { ErrorMessage = "Oops! Sorry that user cannot be found." };
                    if (!account.IsActive) return new ValidateAccountResponse { ErrorMessage = "Account is inactive, please contact support@unieksoftware.com for help." };
                    if (!Security.ValidatePassword(request.Password.Trim(), account.Password)) return new ValidateAccountResponse { ErrorMessage = "Invalid login for this account." };
                    var securityModel = new SecurityModel
                    {
                        Id = account.Id,
                        CompanyId = account.CompanyId,
                        CompanySiteId = account.CompanySiteId,
                        FirstName = account.FirstName,
                        LastName = account.LastName,
                        Email = account.Email,
                        IsCompanyAdmin = account.IsCompanyAdmin,
                        IsSystemAdmin = account.IsSystemAdmin
                    };
                    return new ValidateAccountResponse { IsSuccess = true, SecurityModel = securityModel };
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new ValidateAccountResponse { ErrorMessage = "Unable to validate user, please contact us at support@unieksoftware.com for help." };
            }
        }

        public ChangePasswordResponse ChangePassword(ChangePasswordRequest request)
        {
            try
            {
                using (var context = new FlavorFiEntities())
                {
                    var account = context.Accounts.FirstOrDefault(a => a.Email.Trim().Equals(request.Email.Trim(), StringComparison.CurrentCultureIgnoreCase));
                    if (account == null) return new ChangePasswordResponse { ErrorMessage = "Account does not exist." };
                    if (!Security.ValidatePassword(request.OldPassword.Trim(), account.Password)) return new ChangePasswordResponse { ErrorMessage = "Old Password is invalid" };
                    account.Password = Security.HashPassword(request.NewPassword.Trim());
                    context.SaveChanges();
                    return new ChangePasswordResponse { IsSuccess = true };
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new ChangePasswordResponse { ErrorMessage = "Unable to change the password for this user, please contact us at support@unieksoftware.com for help." };
            }
        }

        public PasswordResetResponse PasswordReset(PasswordResetRequest request)
        {
            try
            {
                var now = DateTimeOffset.Now;
                var response = new PasswordResetResponse();
                using (var context = new FlavorFiEntities())
                {
                    var user = context.Accounts.FirstOrDefault(a => a.Email.Trim().Equals(request.Email.Trim(), StringComparison.InvariantCultureIgnoreCase));
                    if (user == null) return new PasswordResetResponse { ErrorMessage = "Oops!  Sorry that email cannot be found." };
                    if (!user.IsActive) throw new ApplicationException("This account is set to inactive: " + request.Email.Trim());
                    var resets = user.PasswordResets.Where(r => r.IsActive);
                    foreach (var reset in resets)
                        reset.IsActive = false;
                    var newReset = new PasswordReset
                    {
                        Id = Guid.NewGuid(),
                        AccountId = user.Id,
                        IsActive = true,
                        DateCreated = now
                    };
                    context.PasswordResets.Add(newReset);
                    context.SaveChanges();
                    response.ResetId = newReset.Id;
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new PasswordResetResponse { ErrorMessage = "Unable to reset password, please contact us at support@unieksoftware.com for help." };
            }
        }

        public ValidatePasswordResetResponse ValidatePasswordReset(ValidatePasswordResetRequest request)
        {
            try
            {
                var now = DateTimeOffset.Now;
                var response = new ValidatePasswordResetResponse();
                using (var context = new FlavorFiEntities())
                {
                    var expired = now.AddMinutes(-15);
                    var reset = context.PasswordResets.FirstOrDefault(r => r.Id == request.ResetId && r.IsActive && r.DateCreated > expired);
                    if (reset == null) throw new ApplicationException($"The time for this link has expired for reset id {request.ResetId}");
                    reset.IsActive = false;
                    var account = context.Accounts.FirstOrDefault(a => a.Id == reset.AccountId);
                    if (account == null) throw new ApplicationException($"Account does not exist for reset id {request.ResetId}");
                    account.Password = Security.HashPassword(request.NewPassword.Trim());
                    context.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new ValidatePasswordResetResponse { ErrorMessage = "Unable to validate password reset, please contact us at support@unieksoftware.com for help." };
            }
        }

        public GetSecurityModelResponse GetSecurityModel(GetSecurityModelRequest request)
        {
            try
            {
                var response = new GetSecurityModelResponse();
                using (var context = new FlavorFiEntities())
                {
                    var account = context.Accounts.AsNoTracking().FirstOrDefault(a => a.Id == request.AccountId);
                    if (account == null) throw new ApplicationException($"Account does not exist for account id {request.AccountId}");
                    if (!account.IsActive) throw new ApplicationException($"Account is not active for account id {request.AccountId}");
                    response.SecurityModel = new SecurityModel
                    {
                        Id = account.Id,
                        CompanyId = account.CompanyId,
                        CompanySiteId = account.CompanySiteId,
                        FirstName = account.FirstName,
                        LastName = account.LastName,
                        Email = account.Email,
                        IsCompanyAdmin = account.IsCompanyAdmin,
                        IsSystemAdmin = account.IsSystemAdmin
                    };

                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetSecurityModelResponse { ErrorMessage = "Unable to get account, please contact us at support@unieksoftware.com for help." };
            }
        }

        public UpdateCompanyIdResponse UpdateCompanyId(UpdateCompanyIdRequest request)
        {
            try
            {
                var response = new UpdateCompanyIdResponse();
                using (var context = new FlavorFiEntities())
                {
                    //var account = context.Accounts.FirstOrDefault(a => a.Id == userToken.Id);
                    //if (account == null) return new UpdateCompanyIdResponse { ErrorMessage = "Account does not exist" };
                    //account.CompanyId = request.CompanyId;
                    //context.SaveChanges();

                    //request.Account.CompanyId = request.CompanyId;
                    //response.UserToken = Security.EncryptUserToken(request.Account);
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new UpdateCompanyIdResponse { ErrorMessage = "Unable to update company id for account." };
            }
        }

        public SaveNewPasswordResponse SaveNewPassword(SaveNewPasswordRequest request)
        {
            try
            {
                var response = new SaveNewPasswordResponse();
                using (var context = new FlavorFiEntities())
                {
                    var account = context.Accounts.FirstOrDefault(a => a.Id.Equals(request.AccountId));
                    if (account == null) throw new ApplicationException("Account does not exist for id " + request.AccountId);
                    account.Password = Security.HashPassword(request.Password);
                    context.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SaveNewPasswordResponse { ErrorMessage = "Unable to save new password" };
            }
        }
    }
}