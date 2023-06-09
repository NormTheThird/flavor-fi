using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.ShopifyModels
{
    [DataContract]
    [JsonObject(MemberSerialization.OptIn, Title = "webhook")]
    public class ShopifyWebhookModel : ShopifyBaseModel
    {
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "address")] public string Url { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Event { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "topic")] public string Topic { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "format")] public string Format { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public bool IsActive { get; set; } = false;
    }
}