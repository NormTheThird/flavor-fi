using FlavorFi.Common.Models.ShopifyModels;
using System.Collections.Generic;

namespace FlavorFi.Common.RequestAndResponses.ShopWorksRequestAndResponses
{
    public class CreateShopWorksOrderFileRequest : ShopWorksBaseRequest
    {
        public CreateShopWorksOrderFileRequest()
        {
            this.Order = new ShopifyOrderModel();
            this.CustomerMetafields = new List<ShopifyMetafieldModel>();

        }

        public ShopifyOrderModel Order { get; set; }
        public List<ShopifyMetafieldModel> CustomerMetafields { get; set; }
    }

    public class CreateShopWorksOrderFileResponse : ShopWorksBaseResponse
    {
        public CreateShopWorksOrderFileResponse()
        {
            this.FileName = string.Empty;
            this.File = null;
        }

        public string FileName { get; set; }
        public byte[] File { get; set; }

    }
}