using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.ShopifyModels
{
    [DataContract]
    [JsonObject(MemberSerialization.OptIn, Title = "locations")]
    public class ShopifyLocationModel : ShopifyBaseModel
    {
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "name")] public string Name { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "address1")] public string Address1 { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "address2")] public string Address2 { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "city")] public string City { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "province")] public string State { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "zip")] public string ZipCode { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "active")] public bool IsActive { get; set; } = false;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "legacy")] public bool IsLegacy { get; set; } = false;
    }
}