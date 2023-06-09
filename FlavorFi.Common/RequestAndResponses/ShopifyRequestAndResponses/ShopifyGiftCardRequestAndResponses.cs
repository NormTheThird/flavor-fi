using FlavorFi.Common.Models.ShopifyModels;
using System.Collections.Generic;

namespace FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses
{
    public class GetShopifyGiftCardResponse : ShopifyBaseResponse
    {
        public GetShopifyGiftCardResponse()
        {
            this.GiftCard = new ShopifyGiftCardModel();
        }

        public ShopifyGiftCardModel GiftCard { get; set; }
    }

    public class GetShopifyGiftCardsResponse : ShopifyBaseResponse
    {
        public GetShopifyGiftCardsResponse()
        {
            this.GiftCards = new List<ShopifyGiftCardModel>();
        }

        public List<ShopifyGiftCardModel> GiftCards { get; set; }
    }
}