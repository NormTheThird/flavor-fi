using FlavorFi.Common.Models.ShopifyModels;
using System.Collections.Generic;

namespace FlavorFi.Common.RequestAndResponses.ShopWorksRequestAndResponses
{
    public class GetShopWorksProductsRequest : ShopWorksBaseRequest { }

    public class GetShopWorksProductsResponse : ShopWorksBaseResponse
    {
        public GetShopWorksProductsResponse()
        {
            this.Products = new List<ShopifyCreateProductModel>();
        }

        public List<ShopifyCreateProductModel> Products { get; set; }
    }

    public class GetShopWorksProductRequest : ShopWorksBaseRequest
    {
        public GetShopWorksProductRequest()
        {
            this.PartNumber = string.Empty;
        }

        public string PartNumber { get; set; }
    }

    public class GetShopWorksProductResponse : ShopWorksBaseResponse
    {
        public GetShopWorksProductResponse()
        {
            this.Products = new List<ShopifyCreateProductModel>();
        }

        public List<ShopifyCreateProductModel> Products { get; set; }
    }
}