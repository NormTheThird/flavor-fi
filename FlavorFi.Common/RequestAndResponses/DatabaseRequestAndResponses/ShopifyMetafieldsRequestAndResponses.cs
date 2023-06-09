using FlavorFi.Common.Models.DatabaseModels;

namespace FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses
{
    public class SaveShopifyMetafieldRequest : BaseRequest
    {
        public SaveShopifyMetafieldRequest()
        {
            this.Metafield = new ShopifyMetafieldModel();
        }

        public ShopifyMetafieldModel Metafield { get; set; }
    }

    public class SaveShopifyMetafieldResponse : BaseResponse { }
}