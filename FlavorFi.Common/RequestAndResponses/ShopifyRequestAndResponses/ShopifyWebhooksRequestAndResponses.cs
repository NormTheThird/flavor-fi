using FlavorFi.Common.Models.ShopifyModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses
{
    [DataContract]
    public class GetShopifyWebhookResponse : ShopifyBaseResponse
    {
        [DataMember(IsRequired = true)] public ShopifyWebhookModel Webhook { get; set; } = new ShopifyWebhookModel();
    }

    [DataContract]
    public class GetShopifyWebhooksResponse : ShopifyBaseResponse
    {
        [DataMember(IsRequired = true)] public List<ShopifyWebhookModel> Webhooks { get; set; } = new List<ShopifyWebhookModel>();
    }

    [DataContract]
    public class ActivateShopifyWebhookRequest : ShopifyBaseRequest
    {

        [JsonProperty(PropertyName = "webhook")] [DataMember(IsRequired = true)] public ShopifyWebhookModel Webhook { get; set; } = new ShopifyWebhookModel();
    }

    [DataContract]
    public class ActivateShopifyWebhookResponse : ShopifyBaseResponse
    {
        [DataMember(IsRequired = true)] public long NewWebhookId { get; set; } = 0;
    }

    [DataContract]
    public class DeactivateShopifyWebhookRequest : ShopifyBaseRequest
    {
        [DataMember(IsRequired = true)] public long WebhookId { get; set; } = 0;
    }

    [DataContract]
    public class DeactivateShopifyWebhookResponse : ShopifyBaseResponse { }
}