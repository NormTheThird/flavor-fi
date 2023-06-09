using FlavorFi.Common.Models.ShopifyModels;
using System.Collections.Generic;

namespace FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses
{
    public class GetShopifyOrderResponse : ShopifyBaseResponse
    {
        public ShopifyOrderModel Order { get; set; } = new ShopifyOrderModel();
    }

    public class GetShopifyOrdersResponse : ShopifyBaseResponse
    {
        public List<ShopifyOrderModel> Orders { get; set; } = new List<ShopifyOrderModel>();
    }

    public class GetShopifyOrderTransactionsResponse : ShopifyBaseResponse
    {
        public List<ShopifyOrderTransactionModel> OrderTransactions { get; set; } = new List<ShopifyOrderTransactionModel>();
    }

    public class GetShopifyQualityCheckOrderResponse : ShopifyBaseResponse
    {
        public long OrderNumber { get; set; } = 0;
        public List<ShopifyQualityCheckOrderItemModel> OrderItems { get; set; } = new List<ShopifyQualityCheckOrderItemModel>();
        public ShopifyCustomerModel Customer { get; set; } = new ShopifyCustomerModel();
        public ShopifyAddressModel ShippingAddress { get; set; } = new ShopifyAddressModel();
        public bool IsTrendsetter { get; set; } = false;
    }
}