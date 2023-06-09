using FlavorFi.Common.Models.DatabaseModels;
using System;
using System.Runtime.Serialization;

namespace FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses
{
    [DataContract]
    public class LogErrorRequest : BaseRequest
    {
        [DataMember(IsRequired = true)] public Exception ex { get; set; } = null;
    }

    [DataContract]
    public class LogErrorResponse : BaseResponse { }

    [DataContract]
    public class LogFacebookWebhookRequest : BaseRequest
    {
        [DataMember(IsRequired = true)] public FacebookWebhookLogModel FacebookWebhookLog { get; set; } = new FacebookWebhookLogModel();
    }

    [DataContract]
    public class LogFacebookWebhookResponse : BaseResponse { }

    [DataContract]
    public class LogShopifyWebhookRequest : BaseRequest
    {
        [DataMember(IsRequired = true)] public Guid ShopifyWebhookId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public long RecordId { get; set; } = 0;
    }

    [DataContract]
    public class LogShopifyWebhookResponse : BaseResponse
    {
        [DataMember(IsRequired = true)] public Guid ShopifyWebhookLogId { get; set; } = Guid.Empty;
    }

    [DataContract]
    public class LogShopifyWebhookActivityRequest : BaseRequest
    {
        [DataMember(IsRequired = true)] public Guid ShopifyWebhookLogId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public string Activity { get; set; } = string.Empty;
    }

    [DataContract]
    public class LogShopifyWebhookActivityResponse : BaseResponse { }
}