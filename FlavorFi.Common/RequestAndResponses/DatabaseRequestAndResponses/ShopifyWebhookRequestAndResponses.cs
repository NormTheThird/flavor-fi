using FlavorFi.Common.Models.DatabaseModels;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses
{
    [DataContract]
    public class GetShopifyWebhooksRequest : BaseRequest { }

    [DataContract]
    public class GetShopifyWebhooksResponse : BaseResponse
    {
        public GetShopifyWebhooksResponse()
        {
            this.ShopifyWebhooks = new List<ShopifyWebhookModel>();
        }

        [DataMember(IsRequired = true)]
        public List<ShopifyWebhookModel> ShopifyWebhooks { get; set; }
    }

    [DataContract]
    public class GetShopifyWebhookActivityLogsRequest : BaseRequest { }

    [DataContract]
    public class GetShopifyWebhookActivityLogsResponse : BaseResponse
    {
        public GetShopifyWebhookActivityLogsResponse()
        {
            this.ShopifyWebhookActivityLogs = new List<ShopifyWebhookActivityLogModel>();
        }

        [DataMember(IsRequired = true)]
        public List<ShopifyWebhookActivityLogModel> ShopifyWebhookActivityLogs { get; set; }
    }
}