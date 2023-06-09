using System;
using System.Runtime.Serialization;

namespace FlavorFi.API.Shopify.RequestAndResponses
{
    [DataContract]
    public class GetCompanySiteApplicationStatusRequest
    {
        public GetCompanySiteApplicationStatusRequest()
        {
            this.Domain = string.Empty;
            this.ApplicationName = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public string Domain { get; set; }
        [DataMember(IsRequired = true)]
        public string ApplicationName { get; set; }
    }

    [DataContract]
    public class GetCompanySiteApplicationStatusResponse : BaseResponse
    {
        public GetCompanySiteApplicationStatusResponse()
        {
            this.CompanySiteApplicationId = null;
            this.IsEnabled = false;
        }

        [DataMember(IsRequired = true)]
        public Guid? CompanySiteApplicationId { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsEnabled { get; set; }
    }

    [DataContract]
    public class ChangeCompanySiteApplicationStatusRequest
    {
        public ChangeCompanySiteApplicationStatusRequest()
        {
            this.CompanySiteApplicationId = null;
        }

        [DataMember(IsRequired = true)]
        public Guid? CompanySiteApplicationId { get; set; }
    }

    [DataContract]
    public class ChangeCompanySiteApplicationStatusResponse : BaseResponse
    {
        public ChangeCompanySiteApplicationStatusResponse()
        {
            this.IsEnabled = false;
        }

        [DataMember(IsRequired = true)]
        public bool IsEnabled { get; set; }
    }
}