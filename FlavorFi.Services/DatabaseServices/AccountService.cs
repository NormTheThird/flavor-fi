using System;
using System.Linq;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Common.Helpers;
using FlavorFi.Common.Models.DatabaseModels;
using FlavorFi.Data;

namespace FlavorFi.Services.DatabaseServices
{
    public interface IAccountService : IBaseService
    {
        GetAccountsResponse GetAccounts(GetAccountsRequest request);
        GetAdminAccountsResponse GetAdminAccounts(GetAdminAccountsRequest request);
        GetAccountResponse GetAccount(GetAccountRequest request);
        ChangeAccountStatusResponse ChangeAccountStatus(ChangeAccountStatusRequest request);
    }

    public class AccountService : BaseService, IAccountService
    {
        public GetAccountsResponse GetAccounts(GetAccountsRequest request)
        {
            try
            {
                var response = new GetAccountsResponse();
                using (var context = new FlavorFiEntities())
                {
                    var accounts = context.Accounts.AsNoTracking().Where(a => request.GetActiveAndInactive ? (a.IsActive || !a.IsActive) : a.IsActive)
                                                   .Select(a => new AccountModel
                                                   {
                                                       Id = a.Id,
                                                       CompanyId = a.CompanyId,
                                                       CompanySiteId = a.CompanySiteId,
                                                       AddressId = a.AddressId,
                                                       AWSProfileImageId = a.AWSProfileImageId,
                                                       FirstName = a.FirstName,
                                                       LastName = a.LastName,
                                                       Email = a.Email,
                                                       PhoneNumber = a.PhoneNumber,
                                                       AltPhoneNumber = a.AltPhoneNumber,
                                                       AllowedOrigin = a.AllowedOrigin,
                                                       RefreshTokenLifeTimeMinutes = a.RefreshTokenLifeTimeMinutes,
                                                       IsCompanyAdmin = a.IsCompanyAdmin,
                                                       IsActive = a.IsActive,
                                                       DateOfBirth = a.DateOfBirth,
                                                       DateCreated = a.DateCreated
                                                   });
                    response.Accounts = accounts.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetAccountsResponse { ErrorMessage = "Unable to get accounta." };
            }
        }

        public GetAdminAccountsResponse GetAdminAccounts(GetAdminAccountsRequest request)
        {
            try
            {
                var response = new GetAdminAccountsResponse();
                using (var context = new FlavorFiEntities())
                {
                    var accounts = context.Accounts.AsNoTracking()
                                                   .Select(a => new AdminAccountListModel
                                                   {
                                                       Id = a.Id,
                                                       CompanyId = a.CompanyId,
                                                       AWSProfileImageId = a.AWSProfileImageId,
                                                       FullName = a.FirstName + " " + a.LastName,
                                                       CompanyName = a.Company.Name,
                                                       Email = a.Email,
                                                       PhoneNumber = a.PhoneNumber,
                                                       AltPhoneNumber = a.AltPhoneNumber,
                                                       IsCompanyAdmin = a.IsCompanyAdmin,
                                                       IsActive = a.IsActive,
                                                       DateCreated = a.DateCreated
                                                   });
                    response.Accounts = accounts.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetAdminAccountsResponse { ErrorMessage = "Unable to get admin accounts." };
            }
        }

        public GetAccountResponse GetAccount(GetAccountRequest request)
        {
            try
            {
                var response = new GetAccountResponse();
                using (var context = new FlavorFiEntities())
                {
                    var account = context.Accounts.AsNoTracking().FirstOrDefault(a => a.Id.Equals(request.AccountId));
                    if (account == null) throw new ApplicationException("Account does not exist for id " + request.AccountId);
                    response.Account = MapperService.Map<Account, AccountModel>(account);
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetAccountResponse { ErrorMessage = "Unable to get account." };
            }
        }

        public ChangeAccountStatusResponse ChangeAccountStatus(ChangeAccountStatusRequest request)
        {
            try
            {
                var response = new ChangeAccountStatusResponse();
                using (var context = new FlavorFiEntities())
                {
                    var account = context.Accounts.FirstOrDefault(a => a.Id.Equals(request.AccountId));
                    if (account == null) throw new ApplicationException($"Account does not exist for id {request.AccountId}");
                    account.IsActive = !account.IsActive;
                    context.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new ChangeAccountStatusResponse { ErrorMessage = "Unable to get account." };
            }
        }
    }

    //    public static GetWebhookUserTokenResponse GetWebhookUserToken()
    //    {
    //        try
    //        {
    //            using (var context = new FlavorFiEntities())
    //            {
    //                var account = context.Accounts.AsNoTracking().FirstOrDefault(a => a.Email.ToLower().Trim().Equals("webhook@flavormob.com"));
    //                if (account == null) return new GetWebhookUserTokenResponse { ErrorMessage = "Webhook account does not exist" };
    //                var response = new GetWebhookUserTokenResponse();
    //                var securityModel = new SecurityModel
    //                {
    //                    Id = account.Id,
    //                    CompanyId = account.CompanyId ?? Guid.Empty,
    //                    FirstName = account.FirstName,
    //                    LastName = account.LastName,
    //                    Email = account.Email,
    //                    IsSystemAdmin = account.IsSystemAdmin
    //                };
    //                response.UserToken = Security.EncryptUserToken(securityModel);
    //                response.IsSuccess = true;
    //                return response;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LoggingService.LogError(new LogErrorRequest { ex = ex });
    //            return new GetWebhookUserTokenResponse { ErrorMessage = "Unable to get webhook user token." };
    //        }
    //    }
    //}
}