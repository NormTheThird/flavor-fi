using FlavorFi.Common.Models.DatabaseModels;
using System.Collections.Generic;

namespace FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses
{
    public class SaveShopifyProductRequest : BaseRequest
    {
        public SaveShopifyProductRequest()
        {
            this.Product = new ShopifyProductModel();
        }

        public ShopifyProductModel Product { get; set; }
    }

    public class SaveShopifyProductResponse : BaseResponse { }

    public class SaveShopifyProductsRequest : BaseRequest
    {
        public SaveShopifyProductsRequest()
        {
            this.Products = new List<ShopifyProductModel>();
        }

        public List<ShopifyProductModel> Products { get; set; }
    }

    public class SaveShopifyProductsResponse : BaseResponse { }
}