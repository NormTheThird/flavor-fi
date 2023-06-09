using FlavorFi.Common.Models.ShopifyModels;
using System.Collections.Generic;

namespace FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses
{
    public class GetShopifyLocationsResponse : ShopifyBaseResponse
    {
        public List<ShopifyLocationModel> Locations { get; set; } = new List<ShopifyLocationModel>();
    }
}