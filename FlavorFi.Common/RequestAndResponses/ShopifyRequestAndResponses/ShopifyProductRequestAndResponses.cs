using FlavorFi.Common.Models.ShopifyModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses
{
    public class GetShopifyProductResponse : ShopifyBaseResponse
    {
        public GetShopifyProductResponse()
        {
            this.Product = new ShopifyProductModel();
        }

        public ShopifyProductModel Product { get; set; }
    }

    public class GetShopifyProductsResponse : ShopifyBaseResponse
    {
        public GetShopifyProductsResponse()
        {
            this.Products = new List<ShopifyProductModel>();
        }

        public List<ShopifyProductModel> Products { get; set; }
    }

    public class GetShopifyProductMetafiledsResponse : ShopifyBaseResponse
    {
        public GetShopifyProductMetafiledsResponse()
        {
            this.Metafields = new List<ShopifyMetafieldModel>();
        }

        public List<ShopifyMetafieldModel> Metafields { get; set; }
    }



    public class SaveShopifyProductRequest : ShopifyBaseRequest
    {
        public SaveShopifyProductRequest()
        {
            this.Product = new ShopifyCreateProductModel();
        }

        [JsonProperty(PropertyName = "product")]
        public ShopifyCreateProductModel Product { get; set; }
    }

    public class SaveShopifyProductResponse : ShopifyBaseResponse
    {
        public SaveShopifyProductResponse()
        {
            this.Product = new ShopifyProductModel();
        }

        public ShopifyProductModel Product { get; set; }
    }

    public class SaveShopifyProductVariantRequest : ShopifyBaseRequest
    {
        public SaveShopifyProductVariantRequest()
        {
            this.ProductVariant = new ShopifyCreateProductVariantModel();
            this.ProductId = 0;
        }

        public long ProductId { get; set; }
        [JsonProperty(PropertyName = "variant")]
        public ShopifyCreateProductVariantModel ProductVariant { get; set; }
    }

    public class SaveShopifyProductVariantResponse : ShopifyBaseResponse
    {
        public SaveShopifyProductVariantResponse()
        {
            this.ProductVariant = new ShopifyProductVariantModel();
        }

        public ShopifyProductVariantModel ProductVariant { get; set; }
    }

    public class SaveShopifyProductMetafieldRequest : ShopifyBaseRequest
    {
        public SaveShopifyProductMetafieldRequest()
        {
            this.ProductMetafield = new ShopifyCreateMetafieldModel();
            this.ProductId = 0;
        }

        public long ProductId { get; set; }
        [JsonProperty(PropertyName = "metafield")]
        public ShopifyCreateMetafieldModel ProductMetafield { get; set; }
    }

    public class SaveShopifyProductMetafieldResponse : ShopifyBaseResponse
    {
        public SaveShopifyProductMetafieldResponse()
        {
            this.ProductMetafield = new ShopifyMetafieldModel();
        }

        public ShopifyMetafieldModel ProductMetafield { get; set; }
    }
}