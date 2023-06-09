using System;
using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.DatabaseModels
{
    [DataContract]
    public class ShopifyAddressModel : BaseModel
    {
        [DataMember(IsRequired = true)] public Guid ShopifyCustomerId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public long OriginalShopifyId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long OriginalShopifyCustomerId { get; set; } = 0;
        [DataMember(IsRequired = true)] public bool IsDefault { get; set; } = false;
    }
}