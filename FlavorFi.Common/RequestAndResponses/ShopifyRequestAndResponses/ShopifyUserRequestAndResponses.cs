using FlavorFi.Common.Models.ShopifyModels;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses
{
    [DataContract]
    public class GetShopifyUsersResponse : ShopifyBaseResponse
    {
        [DataMember(IsRequired = true)] public List<ShopifyUserModel> Users { get; set; } = new List<ShopifyUserModel>();
    }
}