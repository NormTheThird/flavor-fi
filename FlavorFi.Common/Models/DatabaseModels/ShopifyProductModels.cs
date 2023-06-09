using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.DatabaseModels
{
    [DataContract]
    public class ShopifyProductModel : BaseModel
    {
        [DataMember(IsRequired = true)] public long OriginalShopifyId { get; set; } = 0;
        [DataMember(IsRequired = true)] public string Title { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Vendor { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string ProductType { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.MinValue;
        [DataMember(IsRequired = true)] public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.MinValue;
        [DataMember(IsRequired = true)] public DateTimeOffset? PublishedAt { get; set; } = null;
        [DataMember(IsRequired = true)] public List<ShopifyProductVariantModel> Variants { get; set; } = new List<ShopifyProductVariantModel>();
    }

    [DataContract]
    public class ShopifyProductVariantModel : BaseModel
    {
        [DataMember(IsRequired = true)] public Guid ShopifyProductId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public long OriginalShopifyId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long OriginalShopifyProductId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long OriginalShopifyImageId { get; set; } = 0;
        [DataMember(IsRequired = true)] public string Title { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Sku { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Barcode { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public int Position { get; set; } = 0;
        [DataMember(IsRequired = true)] public int InventoryQuantity { get; set; } = 0;
        [DataMember(IsRequired = true)] public int OldInventoryQuantity { get; set; } = 0;
        [DataMember(IsRequired = true)] public decimal Price { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public decimal CompareAtPrice { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public bool Taxable { get; set; } = false;
        [DataMember(IsRequired = true)] public bool RequiresShipping { get; set; } = false;
        [DataMember(IsRequired = true)] public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.MinValue;
        [DataMember(IsRequired = true)] public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.MinValue;
    }
}