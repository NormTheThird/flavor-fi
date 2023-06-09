using FlavorFi.Common.Models.DatabaseModels;
using System;
using System.Runtime.Serialization;

namespace FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses
{
    [DataContract]
    public class RegisterAccountRequest : BaseRequest
    {
        public RegisterAccountRequest()
        {
            this.RegistrationCode = string.Empty;
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.Email = string.Empty;
            this.Password = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public string RegistrationCode { get; set; }
        [DataMember(IsRequired = true)]
        public string FirstName { get; set; }
        [DataMember(IsRequired = true)]
        public string LastName { get; set; }
        [DataMember(IsRequired = true)]
        public string Email { get; set; }
        [DataMember(IsRequired = true)]
        public string Password { get; set; }
    }

    [DataContract]
    public class RegisterAccountResponse : BaseResponse
    {
        public RegisterAccountResponse()
        {
            this.NewAccountId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid NewAccountId { get; set; }
    }

    [DataContract]
    public class ValidateAccountRequest : BaseRequest
    {
        public ValidateAccountRequest()
        {
            this.Email = string.Empty;
            this.Password = string.Empty;
            this.RememberMe = false;
        }

        [DataMember(IsRequired = true)]
        public string Email { get; set; }
        [DataMember(IsRequired = true)]
        public string Password { get; set; }
        [DataMember(IsRequired = true)]
        public bool RememberMe { get; set; }
    }

    [DataContract]
    public class ValidateAccountResponse : BaseResponse
    {
        public ValidateAccountResponse()
        {
            this.SecurityModel = new SecurityModel();
        }

        [DataMember(IsRequired = true)]
        public SecurityModel SecurityModel { get; set; }
    }

    [DataContract]
    public class ChangePasswordRequest : BaseRequest
    {
        public ChangePasswordRequest()
        {
            this.Email = string.Empty;
            this.OldPassword = string.Empty;
            this.NewPassword = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public string Email { get; set; }
        [DataMember(IsRequired = true)]
        public string OldPassword { get; set; }
        [DataMember(IsRequired = true)]
        public string NewPassword { get; set; }
    }

    [DataContract]
    public class ChangePasswordResponse : BaseResponse { }

    [DataContract]
    public class PasswordResetRequest : BaseRequest
    {
        public PasswordResetRequest()
        {
            this.Email = string.Empty;
            this.BaseUrl = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public string Email { get; set; }
        [DataMember(IsRequired = true)]
        public string BaseUrl { get; set; }
    }

    [DataContract]
    public class PasswordResetResponse : BaseResponse
    {
        public PasswordResetResponse()
        {
            this.ResetId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid ResetId { get; set; }
    }

    [DataContract]
    public class ValidatePasswordResetRequest : BaseRequest
    {
        public ValidatePasswordResetRequest()
        {
            this.ResetId = Guid.Empty;
            this.NewPassword = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid ResetId { get; set; }
        [DataMember(IsRequired = true)]
        public string NewPassword { get; set; }
    }

    [DataContract]
    public class ValidatePasswordResetResponse : BaseResponse { }

    [DataContract]
    public class GetSecurityModelRequest : BaseRequest
    {
        public GetSecurityModelRequest()
        {
            this.AccountId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid AccountId { get; set; }
    }

    [DataContract]
    public class GetSecurityModelResponse : BaseResponse
    {
        public GetSecurityModelResponse()
        {
            this.SecurityModel = new SecurityModel();
        }

        [DataMember(IsRequired = true)]
        public SecurityModel SecurityModel { get; set; }
    }

    [DataContract]
    public class UpdateLastLoginRequest : BaseRequest
    {
        public UpdateLastLoginRequest()
        {
            this.AccountId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid AccountId { get; set; }
    }

    [DataContract]
    public class UpdateLastLoginResponse : BaseResponse { }

    [DataContract]
    public class UpdateCompanyIdRequest : BaseRequest
    {
        public UpdateCompanyIdRequest()
        {
            this.AccountId = Guid.Empty;
            this.CompanyId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid AccountId { get; set; }
        [DataMember(IsRequired = true)]
        public Guid CompanyId { get; set; }
    }

    [DataContract]
    public class UpdateCompanyIdResponse : BaseResponse { }

    [DataContract]
    public class SaveNewPasswordRequest : BaseRequest
    {
        public SaveNewPasswordRequest()
        {
            this.AccountId = Guid.Empty;
            this.Password = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid AccountId { get; set; }
        [DataMember(IsRequired = true)]
        public string Password { get; set; }
    }

    [DataContract]
    public class SaveNewPasswordResponse : BaseResponse { }



    [DataContract]
    public class GetRefreshTokenRequest : BaseRequest
    {
        public GetRefreshTokenRequest()
        {
            this.RefreshToken = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public string RefreshToken { get; set; }
    }

}