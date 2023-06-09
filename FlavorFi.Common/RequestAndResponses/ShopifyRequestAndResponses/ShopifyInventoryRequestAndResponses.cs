using FlavorFi.Common.Models.ShopifyModels;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses
{
    [DataContract]
    public class GetShopifyInventoryLocationsResponse : ShopifyBaseResponse
    {
        public GetShopifyInventoryLocationsResponse()
        {
            this.Locations = new List<ShopifyInvnetoryLocationModel>();
        }

        [DataMember(IsRequired = true)]
        public List<ShopifyInvnetoryLocationModel> Locations { get; set; }
    }
}