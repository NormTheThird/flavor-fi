using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.ShopifyModels
{
    [DataContract]
    public class ShopifyBaseModel
    {
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "id")] public long Id { get; set; } = 0;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "updated_at")] public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.MinValue;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "created_at")] public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.MinValue;
    }

    [DataContract]
    public class ShopifyBaseSiteModel
    {
        [DataMember(IsRequired = true)] public Guid SiteId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public string BaseUrl { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string BaseWebhookUrl { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string PublicApiKey { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string SecretApiKey { get; set; } = string.Empty;
    }
}