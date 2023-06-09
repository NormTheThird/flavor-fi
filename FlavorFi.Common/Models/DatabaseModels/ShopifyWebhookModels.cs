using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.DatabaseModels
{
    [DataContract]
    public class ShopifyWebhookModel : BaseModel
    {
        [DataMember(IsRequired = true)] public string Event { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Topic { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Format { get; set; } = string.Empty;
    }

    [DataContract]
    public class ShopifyWebhookActivityLogModel : BaseModel
    {
        [DataMember(IsRequired = true)] public string Topic { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Activity { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public long RecordId { get; set; } = 0;
    }
}
