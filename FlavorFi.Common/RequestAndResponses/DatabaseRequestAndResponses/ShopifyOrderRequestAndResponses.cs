using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DatabaseModels = FlavorFi.Common.Models.DatabaseModels;
using ShopifyModels = FlavorFi.Common.Models.ShopifyModels;

namespace FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses
{

    [DataContract]
    public class GetShopifyOrdersForPickupRequest : BaseRequest { }

    [DataContract]
    public class GetShopifyOrdersForPickupResponse : BaseResponse
    {
        [DataMember(IsRequired = true)] public List<DatabaseModels.ShopifyOrderForPickupModel> OrdersForPickup { get; set; } = new List<DatabaseModels.ShopifyOrderForPickupModel>();
    }

    [DataContract]
    public class SaveShopifyOrderAsPickedupRequest : BaseRequest
    {
        [DataMember(IsRequired = true)] public Guid ShopifyOrderId { get; set; } = Guid.Empty;
    }

    [DataContract]
    public class SaveShopifyOrderAsPickedupResponse : BaseResponse { }


    [DataContract]
    public class SaveShopifyOrderRequest : BaseRequest
    {
        [DataMember(IsRequired = true)] public DatabaseModels.ShopifyOrderModel Order { get; set; } = new DatabaseModels.ShopifyOrderModel();
    }

    [DataContract]
    public class SaveShopifyOrderResponse : BaseResponse
    {
        [DataMember(IsRequired = true)] public Guid ShopifyOrderId { get; set; } = Guid.Empty;
    }

    [DataContract]
    public class SaveShopifyOrderTransactionRequest : BaseRequest
    {
        [DataMember(IsRequired = true)] public DatabaseModels.ShopifyOrderTransactionModel OrderTransaction { get; set; } = new DatabaseModels.ShopifyOrderTransactionModel();
    }

    [DataContract]
    public class SaveShopifyOrderTransactionResponse : BaseResponse { }

    [DataContract]
    public class SaveShopifyOrderShippingLineFromShopifyRequest : BaseRequest
    {
        [DataMember(IsRequired = true)] public Guid ShopfiyOrderId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public long OriginalShopifyOrderId { get; set; } = 0;
        [DataMember(IsRequired = true)] public ShopifyModels.ShopifyOrderShippingLineModel ShopifyOrderShippingLine { get; set; } = new ShopifyModels.ShopifyOrderShippingLineModel();
    }

    [DataContract]
    public class SaveShopifyOrderShippingLineFromShopifyResponse : BaseResponse { }
}