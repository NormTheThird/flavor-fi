using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.DatabaseModels
{
    [DataContract]
    public class ShopifyCustomerModel : BaseModel
    {
        [DataMember(IsRequired = true)] public long OriginalShopifyId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long OriginalShopifyLastOrderId { get; set; } = 0;
        [DataMember(IsRequired = true)] public string FirstName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string LastName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Email { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public int OrderCount { get; set; } = 0;
        [DataMember(IsRequired = true)] public decimal TotalSpent { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public bool IsTaxExempt { get; set; } = false;
        [DataMember(IsRequired = true)] public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.MinValue;
        [DataMember(IsRequired = true)] public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.MinValue;
        [DataMember(IsRequired = true)] public List<ShopifyAddressModel> Addresses { get; set; } = new List<ShopifyAddressModel>();
        [DataMember(IsRequired = true)] public ShopifyAddressModel DefaultAddress { get; set; } = new ShopifyAddressModel();
    }
}