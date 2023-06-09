using FlavorFi.Common.Models.ShopifyModels;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses
{
    [DataContract]
    public class GetShopifyCustomCollectionsResponse: ShopifyBaseResponse
    {
        [DataMember(IsRequired = true)] public List<ShopifyCustomCollectionModel> CustomCollections { get; set; } = new List<ShopifyCustomCollectionModel>();
    }
}