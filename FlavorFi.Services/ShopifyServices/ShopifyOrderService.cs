using FlavorFi.Common.Enums;
using FlavorFi.Common.Models.ShopifyModels;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace FlavorFi.Services.ShopifyServices
{
    public interface IShopifyOrderService
    {
        GetShopifyOrderResponse GetShopifyOrder(GetShopifyRecordRequest request);
        GetShopifyOrderResponse GetShopifyOrderByQuery(GetShopifyRecordByQueryRequest request);
        //GetShopifyOrderTransactionsResponse GetShopifyOrderTransactions(GetShopifyRecordRequest request);
        GetShopifyOrdersResponse GetShopifyOrders(GetShopifyRecordsRequest request);
        GetShopifyOrdersResponse GetShopifyOrdersPerPage(GetShopifyRecordsPerPageRequest request);
        GetShopifyQualityCheckOrderResponse GetShopifyQualityCheckOrder(GetShopifyRecordByQueryRequest request);
    }

    public class ShopifyOrderService : ShopifyBaseService, IShopifyOrderService
    {
        private const ShopifyResourceType _shopifyResourceType = ShopifyResourceType.orders;

        public ShopifyOrderService() : base(_shopifyResourceType) { }

        /// <summary>
        ///     Gets a shopify order for a specific order id.
        /// </summary>
        /// <param name="request">GetShopifyRecordRequest</param>
        /// <returns>GetShopifyOrderResponse</returns>
        public GetShopifyOrderResponse GetShopifyOrder(GetShopifyRecordRequest request)
        {
            try
            {
                var response = new GetShopifyOrderResponse();
                var baseResponse = GetShopifyRecord(request);
                if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
                response.Order = ShopifyMapperService.Map<ShopifyOrderModel>(baseResponse.Data["order"]);
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetShopifyOrderResponse { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        ///     Gets a shopify order for specific values.
        /// </summary>
        /// <param name="request">GetShopifyRecordByQueryRequest</param>
        /// <returns>GetShopifyOrderResponse</returns>
        public GetShopifyOrderResponse GetShopifyOrderByQuery(GetShopifyRecordByQueryRequest request)
        {
            try
            {
                var response = new GetShopifyOrderResponse();
                var baseResponse = GetShopifyRecordByQuery(request);
                if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
                var orders = ShopifyMapperService.MapToList<ShopifyOrderModel>(baseResponse.Data, _shopifyResourceType);

                var orderNumber = Convert.ToInt64(request.Parameters.FirstOrDefault(sf => sf.Key.Equals("order_number", StringComparison.CurrentCultureIgnoreCase)).Value);
                response.Order = orders.FirstOrDefault(o => o.OrderNumber == orderNumber);
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetShopifyOrderResponse { ErrorMessage = ex.Message };
            }
        }

        ///// <summary>
        /////     Gets the transactions for a shopify order for a specific order id.
        ///// </summary>
        ///// <param name="request">GetShopifyRecordRequest</param>
        ///// <returns>GetShopifyOrderTransactionsResponse</returns>
        //public GetShopifyOrderTransactionsResponse GetShopifyOrderTransactions(GetShopifyRecordRequest request)
        //{
        //    try
        //    {
        //        var response = new GetShopifyOrderTransactionsResponse();
        //        request.ResourceType = ShopifyResourceType.orders;

        //        string url = CreateUrl(ShopifyResourceType.orders) + "/" + request.RecordId.ToString() + "/transactions.json?";
        //        var webRequest = WebRequest.Create(url);
        //        SetBasicAuthHeader(webRequest, "GET");
        //        using (var dataResponse = webRequest.GetResponse())
        //        {
        //            using (var reader = new StreamReader(dataResponse.GetResponseStream()))
        //            {
        //                baseService.AdjustForNextCall(dataResponse);
        //                response.OrderTransactions = ShopifyMapper.MapToList<ShopifyOrderTransactionModel>(JObject.Parse(reader.ReadToEnd()), ShopifyResourceType.transactions);
        //                response.IsSuccess = true;
        //                return response;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingService.LogError(new LogErrorRequest { ex = ex });
        //        return new GetShopifyOrderTransactionsResponse { ErrorMessage = ex.Message };
        //    }
        //}

        /// <summary>
        ///     Gets all of the shopify orders for a certain date range.
        /// </summary>
        /// <param name="request">GetShopifyRecordsRequest</param>
        /// <returns>GetShopifyOrdersResponse</returns>
        public GetShopifyOrdersResponse GetShopifyOrders(GetShopifyRecordsRequest request)
        {
            try
            {
                var response = new GetShopifyOrdersResponse();
                var baseResponse = GetShopifyRecords(request);
                if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);

                response.Orders = ShopifyMapperService.MapToList<ShopifyOrderModel>(baseResponse.Data, _shopifyResourceType);
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetShopifyOrdersResponse { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        ///     Gets all of the shopify orders for a certain date range for a certain page.
        /// </summary>
        /// <param name="request">GetShopifyRecordsPerPageRequest</param>
        /// <returns>GetShopifyOrdersResponse</returns>
        public GetShopifyOrdersResponse GetShopifyOrdersPerPage(GetShopifyRecordsPerPageRequest request)
        {
            try
            {
                var response = new GetShopifyOrdersResponse();
                var baseResponse = GetShopifyRecordsPerPage(request);
                if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);

                response.Orders = ShopifyMapperService.MapToList<ShopifyOrderModel>(baseResponse.Data, _shopifyResourceType);
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetShopifyOrdersResponse { ErrorMessage = ex.Message };
            }
        }

        public GetShopifyQualityCheckOrderResponse GetShopifyQualityCheckOrder(GetShopifyRecordByQueryRequest request)
        {
            try
            {
                var response = new GetShopifyQualityCheckOrderResponse();
                var getOrderResponse = this.GetShopifyOrderByQuery(request);
                response.IsTrendsetter = getOrderResponse.Order.Tags.ToLower().Contains("trendsetter");
                response.OrderNumber = getOrderResponse.Order.OrderNumber;
                response.Customer = getOrderResponse.Order.Customer;
                response.ShippingAddress = getOrderResponse.Order.ShippingAddress;
                foreach (var item in getOrderResponse.Order.LineItems)
                {
                    var imageSrc = "";
                    if (item.ProductId != 0)
                    {
                        var getProductResponse = new ShopifyProductService().GetShopifyProduct(new GetShopifyRecordRequest { BaseSite = request.BaseSite, RecordId = item.ProductId });
                        if (getProductResponse.IsSuccess)
                            imageSrc = getProductResponse.Product.Image?.Src ?? "";
                    }

                    // Group same items together or leave them as an individual item.
                    if (request.GroupItems)
                    {
                        response.OrderItems.Add(new ShopifyQualityCheckOrderItemModel
                        {
                            Id = (long)item.Id,
                            Name = item.Name,
                            Sku = item.Sku,
                            Image = imageSrc,
                            NumberOf = 0,
                            Quantity = (int)item.Quantity,
                            Price = Convert.ToDecimal(item.Price)
                        });
                    }
                    else
                    {
                        for (int i = 1; i <= item.Quantity; i++)
                        {
                            response.OrderItems.Add(new ShopifyQualityCheckOrderItemModel
                            {
                                Id = (long)item.Id,
                                Name = item.Name,
                                Sku = item.Sku,
                                Image = imageSrc,
                                NumberOf = i,
                                Quantity = (int)item.Quantity,
                                Price = Convert.ToDecimal(item.Price)
                            });
                        }
                    }
                }
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetShopifyQualityCheckOrderResponse { ErrorMessage = ex.Message };
            }
        }
    }
}