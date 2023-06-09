using System;
using FlavorFi.Common.Models.DatabaseModels;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses
{
    [DataContract]
    public class GetAccountsRequest : BaseActiveRequest { }

    [DataContract]
    public class GetAccountsResponse : BaseResponse
    {
        public GetAccountsResponse()
        {
            this.Accounts = new List<AccountModel>();
        }

        [DataMember(IsRequired = true)]
        public List<AccountModel> Accounts { get; set; }
    }

    [DataContract]
    public class GetAccountRequest : BaseRequest
    {
        public GetAccountRequest()
        {
            this.AccountId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid AccountId { get; set; }
    }

    [DataContract]
    public class GetAccountResponse : BaseResponse
    {
        public GetAccountResponse()
        {
            this.Account = new AccountModel();
        }

        [DataMember(IsRequired = true)]
        public AccountModel Account { get; set; }
    }

    [DataContract]
    public class SaveAccountRequest : BaseRequest
    {
        public SaveAccountRequest()
        {
            this.Account = new AccountModel();
        }

        [DataMember(IsRequired = true)]
        public AccountModel Account { get; set; }
    }

    [DataContract]
    public class SaveAccountResponse : BaseResponse { }

    [DataContract]
    public class GetAdminAccountsRequest : BaseRequest { }

    [DataContract]
    public class GetAdminAccountsResponse : BaseResponse
    {
        public GetAdminAccountsResponse()
        {
            this.Accounts = new List<AdminAccountListModel>();
        }

        [DataMember(IsRequired = true)]
        public List<AdminAccountListModel> Accounts { get; set; }
    }

    [DataContract]
    public class SaveAccountProfileImageRequest : BaseRequest
    {
        public SaveAccountProfileImageRequest()
        {
            this.AccountId = Guid.Empty;
            this.AWSProfileImageId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid AccountId { get; set; }
        [DataMember(IsRequired = true)]
        public Guid AWSProfileImageId { get; set; }
    }

    [DataContract]
    public class SaveAccountProfileImageResponse : BaseResponse { }

    [DataContract]
    public class ChangeAccountStatusRequest : BaseRequest
    {
        public ChangeAccountStatusRequest()
        {
            this.AccountId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid AccountId { get; set; }
    }

    [DataContract]
    public class ChangeAccountStatusResponse : BaseResponse { }

    [DataContract]
    public class DeleteAccountRequest : BaseRequest
    {
        public DeleteAccountRequest()
        {
            this.AccountId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid AccountId { get; set; }
    }

    [DataContract]
    public class DeleteAccountResponse : BaseResponse { }

    [DataContract]
    public class GetAccountActivityRequest : BaseRequest
    {
        public GetAccountActivityRequest()
        {
            this.AccountId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid AccountId { get; set; }
    }

    [DataContract]
    public class GetAccountActivityResponse : BaseResponse
    {
        public GetAccountActivityResponse()
        {
            this.AccountActivity = new List<AccountActivityModel>();
        }

        [DataMember(IsRequired = true)]
        public List<AccountActivityModel> AccountActivity { get; set; }
    }
}