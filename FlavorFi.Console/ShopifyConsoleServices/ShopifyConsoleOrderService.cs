using FlavorFi.Common.Enums;
using System;
using DatabaseServices = FlavorFi.Services.DatabaseServices;
using ShopifyServices = FlavorFi.Services.ShopifyServices;
using DatabaseRequestAndResponses = FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using ShopifyRequestAndResponses = FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using FlavorFi.Common.Models.ShopifyModels;
using System.Collections.Generic;

namespace FlavorFi.Console.ShopifyConsoleServices
{
    public static class ShopifyConsoleOrderService
    {
        public static void LoadOrders(ShopifyBaseSiteModel baseSite)
        {
            try
            {
                System.Console.WriteLine($"Getting all orders from shopify for {baseSite.BaseUrl}.");   

                var perPage = 100;
                var shopifyOrderRequest = new ShopifyRequestAndResponses.GetShopifyRecordsRequest { BaseSite = baseSite, Parameters = new Dictionary<string, string>() };
                shopifyOrderRequest.Parameters.Add("status", "any");
                var shopifyOrderService = new ShopifyServices.ShopifyOrderService();
                var shopifyCountResponse = shopifyOrderService.GetShopifyRecordCount(shopifyOrderRequest);
                var pageCount = shopifyCountResponse.Count / perPage;
                if (shopifyCountResponse.Count % perPage != 0) pageCount++;

                System.Console.WriteLine($"Synchronizing {shopifyCountResponse.Count} orders");
                System.Console.WriteLine($"{pageCount} pages at {DateTime.Now.ToLongTimeString()}");

                var databaseShopifyOrderService = new DatabaseServices.ShopifyOrderService();
                var mapperService = new DatabaseServices.MapperService();
                for (int i = 1994; i <= pageCount; i++)
                {
                    var perPageRequest = new ShopifyRequestAndResponses.GetShopifyRecordsPerPageRequest { BaseSite = baseSite, Parameters = new Dictionary<string, string>() };
                    perPageRequest.Parameters.Add("status", "any");
                    perPageRequest.Parameters.Add("limit", perPage.ToString());
                    perPageRequest.Parameters.Add("page", i.ToString());
                    var perPageResponse = new ShopifyServices.ShopifyOrderService().GetShopifyOrdersPerPage(perPageRequest);
                    if (!perPageResponse.IsSuccess) throw new ApplicationException(perPageResponse.ErrorMessage);
                    DateTimeOffset? lastOrderDate = null;
                    foreach (var order in perPageResponse.Orders)
                    {
                        lastOrderDate = order.CreatedAt;
                        var saveShopifyOrderRequest = new DatabaseRequestAndResponses.SaveShopifyOrderRequest { CompanySiteId = baseSite.SiteId };
                        mapperService.Map(order, saveShopifyOrderRequest.Order);
                        var saveShopifyOrderResponse = databaseShopifyOrderService.SaveShopifyOrder(saveShopifyOrderRequest);
                        if (!saveShopifyOrderResponse.IsSuccess)
                            System.Console.WriteLine($"Shopify Order Error [Order: {order.Id}][Error: {saveShopifyOrderResponse.ErrorMessage}]");
                        else
                        {
                            AddShopifyOrderShippingLines(order, baseSite.SiteId, saveShopifyOrderResponse.ShopifyOrderId);
                        }
                    }

                    System.Console.WriteLine($"finished with page {i} of {pageCount} at {DateTime.Now.ToLongTimeString()} with last order date at {lastOrderDate}");
                }

                System.Console.WriteLine($"Synchronization of orders in the database is complete at {DateTime.Now.ToLongTimeString()}");
            }
            catch (Exception ex) { throw ex; }
        }

        private static void AddShopifyOrderShippingLines(ShopifyOrderModel order, Guid baseSiteId, Guid shopifyOrderId)
        {
            if (order.ShippingLines == null) return;
            foreach (var shippingLine in order.ShippingLines)
            {
                var saveShopifyOrderShippingLineFromShopifyRequest = new DatabaseRequestAndResponses.SaveShopifyOrderShippingLineFromShopifyRequest
                {
                    CompanySiteId = baseSiteId,
                    ShopfiyOrderId = shopifyOrderId,
                    OriginalShopifyOrderId = order.Id,
                    ShopifyOrderShippingLine = shippingLine
                };
                var saveShopifyOrderShippingLineFromShopifyResponse = new DatabaseServices.ShopifyOrderService().SaveShopifyOrderShippingLineFromShopify(saveShopifyOrderShippingLineFromShopifyRequest);
                if (!saveShopifyOrderShippingLineFromShopifyResponse.IsSuccess)
                    System.Console.WriteLine($"Shopify Order Shipping Line Error [Order: {order.Id}][Error: {saveShopifyOrderShippingLineFromShopifyResponse.ErrorMessage}]");
            }
        }
    }
}