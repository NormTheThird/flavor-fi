using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FlavorFi.Common.Models.ShopifyModels
{
    public class ShopifyCreateProductModel
    {
        [JsonProperty(PropertyName = "id")] public long Id { get; set; } = 0;
        [JsonIgnore] public long ExternalId { get; set; } = 0;
        [JsonProperty(PropertyName = "title")] public string Title { get; set; } = string.Empty;
        [JsonIgnore] public string PartNumber { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "body_html")] public string BodyHtml { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "vendor")] public string Vendor { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "product_type")] public string ProductType { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "tags")] public string Tags { get; set; } = string.Empty;
        [JsonIgnore] public List<ShopifyCreateMetafieldModel> Metafields { get; set; } = new List<ShopifyCreateMetafieldModel>();
        [JsonProperty(PropertyName = "options")] public List<ShopifyOptionModel> Options { get; set; } = new List<ShopifyOptionModel>();
        [JsonProperty(PropertyName = "images")] public List<ShopifyProductImageModel> Images { get; set; } = new List<ShopifyProductImageModel>();
        [JsonIgnore] public List<ShopifyCreateProductVariantModel> Variants { get; set; } = new List<ShopifyCreateProductVariantModel>();
        [JsonProperty(PropertyName = "variants")] public List<ShopifyCreateProductVariantOptionModel> VariantOptions { get; set; } = new List<ShopifyCreateProductVariantOptionModel>();
    }

    public class ShopifyCreateProductVariantModel
    {
        [JsonProperty(PropertyName = "id")] public long Id { get; set; } = 0;
        [JsonIgnore] public long ExternalId { get; set; } = 0;
        [JsonProperty(PropertyName = "sku")] public string Sku { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "barcode")] public string Barcode { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "option1")] public string Color { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "option2")] public string Size { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "inventory_management")] public string InventoryManagement { get; set; } = "shopify";
        [JsonProperty(PropertyName = "inventory_quantity")] public int InventoryQuantity { get; set; } = 0;
        [JsonProperty(PropertyName = "inventory_policy")] public string InventoryPolicy { get; set; } = "continue";
        [JsonProperty(PropertyName = "price")] public decimal Price { get; set; } = 0.0m;
        [JsonProperty(PropertyName = "taxable")] public bool Taxable { get; set; } = false;
    }

    public class ShopifyCreateProductVariantOptionModel
    {
        [JsonProperty(PropertyName = "option1")] public string Color { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "option2")] public string Size { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "sku")] public string Sku { get; set; } = string.Empty;
    }

    public class ShopifyProductModel : ShopifyBaseModel
    {
        [JsonProperty(PropertyName = "title")] public string Title { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "body_html")] public string BodyHtml { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "vendor")] public string Vendor { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "product_type")] public string ProductType { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "handle")] public string Handle { get; set; } = string.Empty;
        public string TemplateSuffix { get; set; } = string.Empty;
        public string PublishedScope { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "tags")] public string Tags { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "published_at")] public DateTimeOffset? PublishedAt { get; set; } = null;
        [JsonProperty(PropertyName = "variants")] public List<ShopifyProductVariantModel> Variants { get; set; } = new List<ShopifyProductVariantModel>();
        [JsonProperty(PropertyName = "image")] public ShopifyProductImageModel Image { get; set; } = new ShopifyProductImageModel();
    }

    public class ShopifyProductVariantModel : ShopifyBaseModel
    {
        public long ProductId { get; set; } = 0;
        public long ImageId { get; set; } = 0;
        public long InventoryItemId { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string InventoryPolicy { get; set; } = string.Empty;
        public string FulfillmentService { get; set; } = string.Empty;
        public string InventoryManagement { get; set; } = string.Empty;
        public string Option1 { get; set; } = string.Empty;
        public string Option2 { get; set; } = string.Empty;
        public string Option3 { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public string WeightUnit { get; set; } = string.Empty;
        public int Position { get; set; } = 0;
        public int Grams { get; set; } = 0;
        public int InventoryQuantity { get; set; } = 0;
        public int OldInventoryQuantity { get; set; } = 0;
        public decimal Price { get; set; } = 0.0m;
        public decimal CompareAtPrice { get; set; } = 0.0m;
        public decimal Weight { get; set; } = 0.0m;
        public bool Taxable { get; set; } = false;
        public bool RequiresShipping { get; set; } = false;
    }

    public class ShopifyProductImageModel : ShopifyBaseModel
    {
        [JsonProperty(PropertyName = "product_id")] public long ProductId { get; set; } = 0;
        [JsonProperty(PropertyName = "width")] public int Width { get; set; } = 0;
        [JsonProperty(PropertyName = "height")] public int Height { get; set; } = 0;
        [JsonProperty(PropertyName = "src")] public string Src { get; set; } = string.Empty;
    }

    public class ShopifyOptionModel
    {
        [JsonProperty(PropertyName = "name")] public string Name { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "values")] public List<string> Values { get; set; } = new List<string>();
    }
}