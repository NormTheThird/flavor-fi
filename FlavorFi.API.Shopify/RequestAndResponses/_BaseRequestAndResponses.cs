using System;
using System.Runtime.Serialization;

namespace FlavorFi.API.Shopify.RequestAndResponses
{
    [DataContract]
    public class BaseRequest
    {
        public BaseRequest()
        {
            this.CompanySiteId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid CompanySiteId { get; set; }
    }

    [DataContract]
    public class BaseAppllicationRequest
    {
        public BaseAppllicationRequest()
        {
            this.CompanySiteApplicationId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid CompanySiteApplicationId { get; set; }
    }

    [DataContract]
    public class BaseResponse
    {
        public BaseResponse()
        {
            this.IsSuccess = false;
            this.ErrorMessage = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public bool IsSuccess { get; set; }
        [DataMember(IsRequired = true)]
        public string ErrorMessage { get; set; }
    }
}