using FlavorFi.Common.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses
{
    [DataContract]
    public class GetCompaniesRequest : BaseActiveRequest { }

    [DataContract]
    public class GetCompaniesResponse : BaseResponse
    {
        public GetCompaniesResponse()
        {
            this.Companies = new List<CompanyModel>();
        }

        [DataMember(IsRequired = true)]
        public List<CompanyModel> Companies { get; set; }
    }

    [DataContract]
    public class GetCompanyRequest : BaseRequest
    {
        public GetCompanyRequest()
        {
            this.CompanyId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid CompanyId { get; set; }
    }

    [DataContract]
    public class GetCompanyResponse : BaseResponse
    {
        public GetCompanyResponse()
        {
            this.Company = new CompanyModel();
        }

        [DataMember(IsRequired = true)]
        public CompanyModel Company { get; set; }
    }

    [DataContract]
    public class GetCompanyAndSitesResponse : BaseResponse
    {
        public GetCompanyAndSitesResponse()
        {
            this.SelectedCompanySiteId = Guid.Empty;
            this.Company = new CompanyModel();
            this.CompanySites = new List<CompanySiteModel>();
        }

        [DataMember(IsRequired = true)]
        public Guid SelectedCompanySiteId { get; set; }
        [DataMember(IsRequired = true)]
        public CompanyModel Company { get; set; }
        [DataMember(IsRequired = true)]
        public List<CompanySiteModel> CompanySites { get; set; }
    }

    [DataContract]
    public class SaveCompanyRequest : BaseRequest
    {
        public SaveCompanyRequest()
        {
            this.Company = new CompanyModel();
        }

        [DataMember(IsRequired = true)]
        public CompanyModel Company { get; set; }
    }

    [DataContract]
    public class SaveCompanyResponse : BaseResponse { }



    [DataContract]
    public class GetCompanySitesRequest : BaseActiveRequest
    {
        public GetCompanySitesRequest()
        {
            this.CompanyId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid CompanyId { get; set; }
    }

    [DataContract]
    public class GetCompanySitesResponse : BaseResponse
    {
        public GetCompanySitesResponse()
        {
            this.CompanySites = new List<CompanySiteModel>();
        }

        [DataMember(IsRequired = true)]
        public List<CompanySiteModel> CompanySites { get; set; }
    }

    [DataContract]
    public class GetCompanySiteRequest : BaseRequest { }

    [DataContract]
    public class GetCompanySiteByDomainRequest
    {
        public GetCompanySiteByDomainRequest()
        {
            this.Domain = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public string Domain { get; set; }
    }

    [DataContract]
    public class GetCompanySiteResponse : BaseResponse
    {
        public GetCompanySiteResponse()
        {
            this.CompanySite = new CompanySiteModel();
        }

        [DataMember(IsRequired = true)]
        public CompanySiteModel CompanySite { get; set; }
    }



    [DataContract]
    public class GetCompanySiteApplicationsRequest : BaseRequest { }

    [DataContract]
    public class GetCompanySiteApplicationsResponse : BaseResponse
    {
        public GetCompanySiteApplicationsResponse()
        {
            this.CompanySiteApplications = new List<CompanySiteApplicationModel>();
        }

        [DataMember(IsRequired = true)]
        public List<CompanySiteApplicationModel> CompanySiteApplications { get; set; }
    }

    [DataContract]
    public class GetCompanySiteApplicationRequest : BaseRequest
    {
        public GetCompanySiteApplicationRequest()
        {
            this.CompanySiteApplicationId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid CompanySiteApplicationId { get; set; }
    }

    [DataContract]
    public class GetCompanySiteApplicationResponse : BaseResponse
    {
        public GetCompanySiteApplicationResponse()
        {
            this.CompanySiteApplication = new CompanySiteApplicationModel();
        }

        [DataMember(IsRequired = true)]
        public CompanySiteApplicationModel CompanySiteApplication { get; set; }
    }

    [DataContract]
    public class SaveCompanySiteApplicationRequest : BaseRequest
    {
        public SaveCompanySiteApplicationRequest()
        {
            this.CompanySiteApplication = new CompanySiteApplicationModel();
        }

        [DataMember(IsRequired = true)]
        public CompanySiteApplicationModel CompanySiteApplication { get; set; }
    }

    [DataContract]
    public class SaveCompanySiteApplicationResponse : BaseResponse { }



    [DataContract]
    public class SaveCompanySiteEmailRequest : BaseRequest
    {
        public SaveCompanySiteEmailRequest()
        {
            this.CompanySiteEmail = new CompanySiteEmailModel();
        }

        [DataMember(IsRequired = true)]
        public CompanySiteEmailModel CompanySiteEmail { get; set; }
    }

    [DataContract]
    public class SaveCompanySiteEmailResponse : BaseResponse { }



    [DataContract]
    public class GetCompanySiteSettingsRequest : BaseRequest { }

    [DataContract]
    public class GetCompanySiteSettingsResponse : BaseResponse
    {
        public GetCompanySiteSettingsResponse()
        {
            this.CompanySiteSettings = new List<CompanySiteSettingModel>();
        }

        [DataMember(IsRequired = true)]
        public List<CompanySiteSettingModel> CompanySiteSettings { get; set; }
    }

    [DataContract]
    public class GetCompanySiteSettingRequest : BaseRequest
    {
        public GetCompanySiteSettingRequest()
        {
            this.ComapnySettingId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid ComapnySettingId { get; set; }
    }

    [DataContract]
    public class GetCompanySiteSettingResponse : BaseResponse
    {
        public GetCompanySiteSettingResponse()
        {
            this.CompanySiteSetting = new CompanySiteSettingModel();
        }

        [DataMember(IsRequired = true)]
        public CompanySiteSettingModel CompanySiteSetting { get; set; }
    }

    [DataContract]
    public class SaveCompanySiteSettingRequest : BaseRequest
    {
        public SaveCompanySiteSettingRequest()
        {
            this.CompanySiteSetting = new CompanySiteSettingModel();
        }

        [DataMember(IsRequired = true)]
        public CompanySiteSettingModel CompanySiteSetting { get; set; }
    }

    [DataContract]
    public class SaveCompanySiteSettingResponse : BaseResponse { }
}