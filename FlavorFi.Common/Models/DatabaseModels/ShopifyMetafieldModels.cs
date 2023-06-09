using System;
using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.DatabaseModels
{
    [DataContract]
    public class ShopifyMetafieldModel : BaseModel
    {
        [DataMember(IsRequired = true)] public Guid? ShopifyCustomCollectionId { get; set; } = null;
        [DataMember(IsRequired = true)] public Guid? ShopifyCustomerId { get; set; } = null;
        [DataMember(IsRequired = true)] public Guid? ShopifyProductId { get; set; } = null;
        [DataMember(IsRequired = true)] public long OriginalShopifyId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long OriginalShopifyOwnerId { get; set; } = 0;
        [DataMember(IsRequired = true)] public string OwnerResource { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Namespace { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Key { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Value { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string ValueType { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.MinValue;
        [DataMember(IsRequired = true)] public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.MinValue;
    }
}