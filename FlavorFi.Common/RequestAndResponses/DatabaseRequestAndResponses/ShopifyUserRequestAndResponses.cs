using System.Runtime.Serialization;
using DatabaseModels = FlavorFi.Common.Models.DatabaseModels;
using ShopifyModels = FlavorFi.Common.Models.ShopifyModels;

namespace FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses
{
    [DataContract]
    public class SaveShopifyUserFromShopifyRequest : BaseRequest
    {
        [DataMember(IsRequired = true)] public ShopifyModels.ShopifyUserModel User { get; set; } = new ShopifyModels.ShopifyUserModel();
    }

    [DataContract]
    public class SaveShopifyUserFromShopifyResponse : BaseResponse { }
}