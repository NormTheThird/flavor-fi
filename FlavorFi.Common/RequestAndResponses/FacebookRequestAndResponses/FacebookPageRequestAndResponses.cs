using FlavorFi.Common.Models.FacebookModels;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FlavorFi.Common.RequestAndResponses.FacebookRequestAndResponses
{
    [DataContract]
    public class GetSubscribedAppsRequest : BaseRequest
    {
        public GetSubscribedAppsRequest()
        {
            this.PageId = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public string PageId { get; set; }
    }

    [DataContract]
    public class GetSubscribedAppsResponse : BaseResponse
    {
        public GetSubscribedAppsResponse()
        {
            this.SubscribedApps = new List<SubscribedAppModel>();
        }

        [DataMember(IsRequired = true)]
        public List<SubscribedAppModel> SubscribedApps { get; set; }
    }
}