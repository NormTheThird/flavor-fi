using System.Runtime.Serialization;
using DatabaseModels = FlavorFi.Common.Models.DatabaseModels;
using ShopifyModels = FlavorFi.Common.Models.ShopifyModels;

namespace FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses
{
    [DataContract]
    public class SaveShopifyCustomCollectionFromShopifyRequest : BaseRequest
    {
        [DataMember(IsRequired = true)] public ShopifyModels.ShopifyCustomCollectionModel CustomCollection { get; set; } = new ShopifyModels.ShopifyCustomCollectionModel();
    }

    public class SaveShopifyCustomCollectionFromShopifyResponse : BaseResponse { }
}