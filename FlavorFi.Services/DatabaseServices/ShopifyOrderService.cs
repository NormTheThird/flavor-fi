using System;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Common.Helpers;
using FlavorFi.Common.Enums;
using FlavorFi.Data;
using System.Linq;
using FlavorFi.Common.Models.DatabaseModels;

namespace FlavorFi.Services.DatabaseServices
{
    public interface IShopifyOrderService
    {
        GetShopifyOrdersForPickupResponse GetShopifyOrdersForPickup(GetShopifyOrdersForPickupRequest request);
        SaveShopifyOrderAsPickedupResponse SaveShopifyOrderAsPickedup(SaveShopifyOrderAsPickedupRequest request);
        SaveShopifyOrderResponse SaveShopifyOrder(SaveShopifyOrderRequest request);
        SaveShopifyOrderTransactionResponse SaveShopifyOrderTransaction(SaveShopifyOrderTransactionRequest request);
        SaveShopifyOrderResponse SaveShopifyOrderFromShopify(SaveShopifyOrderRequest request);
        SaveShopifyOrderShippingLineFromShopifyResponse SaveShopifyOrderShippingLineFromShopify(SaveShopifyOrderShippingLineFromShopifyRequest request);
    }

    public class ShopifyOrderService : BaseService, IShopifyOrderService
    {
        public GetShopifyOrdersForPickupResponse GetShopifyOrdersForPickup(GetShopifyOrdersForPickupRequest request)
        {
            try
            {
                var response = new GetShopifyOrdersForPickupResponse();
                using (var context = new FlavorFiEntities())
                {
                    var ordersForPickup = context.ShopifyOrderShippingLines.AsNoTracking()
                                                                           .Where(_ => _.Code.ToUpper().Contains("IN STORE PICKUP") && _.ShopifyOrder.DatePickedUp == null)
                                                                           .OrderByDescending(_ => _.ShopifyOrder.OrderNumber)
                                                                           .Select(_ => new ShopifyOrderForPickupModel
                                                                           {
                                                                               ShopifyOrderId = _.ShopifyOrderId,
                                                                               FullName = _.ShopifyOrder.ShopifyCustomer.FirstName + " " + _.ShopifyOrder.ShopifyCustomer.LastName,
                                                                               OrderNumber = _.ShopifyOrder.OrderNumber,
                                                                               OrderDate = _.ShopifyOrder.CreatedAt
                                                                           }).ToList();
                    response.OrdersForPickup = ordersForPickup;
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetShopifyOrdersForPickupResponse { ErrorMessage = "Unable to get shopify orders for pickup." };
            }
        }

        public SaveShopifyOrderAsPickedupResponse SaveShopifyOrderAsPickedup(SaveShopifyOrderAsPickedupRequest request)
        {
            try
            {
                var response = new SaveShopifyOrderAsPickedupResponse();
                using (var context = new FlavorFiEntities())
                {
                    var order = context.ShopifyOrders.FirstOrDefault(_ => _.Id.Equals(request.ShopifyOrderId));
                    if (order == null) throw new ApplicationException($"Unable to find order id {request.ShopifyOrderId}");
                    order.DatePickedUp = DateTimeOffset.Now;
                    context.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SaveShopifyOrderAsPickedupResponse { ErrorMessage = "Unable to save shopify order pickup date." };
            }
        }

        public SaveShopifyOrderResponse SaveShopifyOrder(SaveShopifyOrderRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTimeOffset(TimeZoneInfoId.CentralStandardTime);
                var response = new SaveShopifyOrderResponse();
                using (var context = new FlavorFiEntities())
                {
                    var order = context.ShopifyOrders.FirstOrDefault(o => o.OriginalShopifyId.Equals(request.Order.OriginalShopifyId) && o.CompanySiteId == request.CompanySiteId);
                    if (order == null)
                    {
                        order = new ShopifyOrder
                        {
                            Id = Guid.NewGuid(),
                            CompanySiteId = request.CompanySiteId,
                            OriginalShopifyId = request.Order.OriginalShopifyId,
                            ShopifyCustomerId = null,
                            DateCreated = now
                        };
                        context.ShopifyOrders.Add(order);
                    }
                    MapperService.Map(request.Order, order);
                    order.ShopifyCustomerId = null;

                    foreach (var lineItem in request.Order.LineItems)
                    {
                        var _lineItem = context.ShopifyOrderLineItems.FirstOrDefault(l => l.OriginalShopifyId.Equals(lineItem.OriginalShopifyId) && l.CompanySiteId == request.CompanySiteId);
                        if (_lineItem == null)
                        {
                            _lineItem = new ShopifyOrderLineItem
                            {
                                Id = Guid.NewGuid(),
                                CompanySiteId = request.CompanySiteId,
                                ShopifyOrderId = order.Id,
                                OriginalShopifyId = lineItem.OriginalShopifyId,
                                OriginalShopifyOrderId = order.OriginalShopifyId,
                                DateCreated = now,
                                ShopifyProductId = null,
                                ShopifyProductVariantId = null,
                            };
                            context.ShopifyOrderLineItems.Add(_lineItem);
                        }


                        //var product = context.ShopifyProducts.AsNoTracking().FirstOrDefault(p => p.OriginalShopifyId.Equals(lineItem.OriginalShopifyProductId) && p.CompanySiteId == request.CompanySiteId);
                        //if (product == null) lineItem.ShopifyProductId = null;
                        //else lineItem.ShopifyProductId = product.Id;

                        //var variant = context.ShopifyProductVariants.AsNoTracking().FirstOrDefault(pv => pv.OriginalShopifyId.Equals(lineItem.OriginalShopifyProductVariantId) && pv.CompanySiteId == request.CompanySiteId);
                        //if (variant == null) lineItem.ShopifyProductVariantId = null;
                        //else lineItem.ShopifyProductVariantId = variant.Id;

                        MapperService.Map(lineItem, _lineItem);
                    }

                    context.SaveChanges();
                    response.ShopifyOrderId = order.Id;
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SaveShopifyOrderResponse { ErrorMessage = "Unable to save shopify order." };
            }
        }

        public SaveShopifyOrderTransactionResponse SaveShopifyOrderTransaction(SaveShopifyOrderTransactionRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTimeOffset(TimeZoneInfoId.CentralStandardTime);
                var response = new SaveShopifyOrderTransactionResponse();
                using (var context = new FlavorFiEntities())
                {
                    var transaction = context.ShopifyOrderTransactions.FirstOrDefault(t => t.OriginalShopifyId.Equals(request.OrderTransaction.OriginalShopifyId) && t.CompanySiteId == request.CompanySiteId);
                    if (transaction == null)
                    {
                        transaction = new ShopifyOrderTransaction
                        {
                            Id = Guid.NewGuid(),
                            CompanySiteId = request.CompanySiteId,
                            ShopifyOrderId = request.OrderTransaction.ShopifyOrderId,
                            OriginalShopifyId = request.OrderTransaction.OriginalShopifyId,
                            OriginalShopifyOrderId = request.OrderTransaction.OriginalShopifyOrderId,
                            DateCreated = now
                        };
                        context.ShopifyOrderTransactions.Add(transaction);
                    }

                    MapperService.Map(request.OrderTransaction, transaction);
                    if (request.OrderTransaction.OriginalShopifyGiftCardId > 0)
                    {
                        var giftCard = context.ShopifyGiftCards.AsNoTracking().FirstOrDefault(g => g.OriginalShopifyId.Equals(request.OrderTransaction.OriginalShopifyGiftCardId) && g.CompanySiteId == request.CompanySiteId);
                        if (giftCard != null) transaction.ShopifyGiftCardId = giftCard.Id;
                    }

                    context.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SaveShopifyOrderTransactionResponse { ErrorMessage = ex.Message };

            }
        }

        public SaveShopifyOrderResponse SaveShopifyOrderFromShopify(SaveShopifyOrderRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTimeOffset(TimeZoneInfoId.CentralStandardTime);
                var response = new SaveShopifyOrderResponse();
                using (var context = new FlavorFiEntities())
                {
                    var order = context.ShopifyOrders.FirstOrDefault(o => o.OriginalShopifyId.Equals(request.Order.OriginalShopifyId) && o.CompanySiteId == request.CompanySiteId);
                    if (order == null)
                    {
                        order = new ShopifyOrder
                        {
                            Id = Guid.NewGuid(),
                            CompanySiteId = request.CompanySiteId,
                            OriginalShopifyId = request.Order.OriginalShopifyId,
                            ShopifyCustomerId = null,
                            DateCreated = now
                        };
                        context.ShopifyOrders.Add(order);
                    }
                    MapperService.Map(request.Order, order);
                    context.SaveChanges();

                    if (request.Order.Customer == null) order.ShopifyCustomerId = null;
                    else
                    {
                        var customer = context.ShopifyCustomers.FirstOrDefault(c => c.OriginalShopifyId.Equals(request.Order.Customer.OriginalShopifyId) && c.CompanySiteId == request.CompanySiteId);
                        if (customer == null)
                        {
                            customer = new ShopifyCustomer
                            {
                                Id = Guid.NewGuid(),
                                CompanySiteId = request.CompanySiteId,
                                OriginalShopifyId = request.Order.Customer.OriginalShopifyId,
                                DateCreated = now
                            };
                            context.ShopifyCustomers.Add(customer);
                        }
                        MapperService.Map(request.Order.Customer, customer);
                        order.ShopifyCustomerId = customer.Id;
                        order.OriginalShopifyCustomerId = customer.OriginalShopifyId;
                        context.SaveChanges();
                    }

                    foreach (var lineItem in request.Order.LineItems)
                    {
                        var _lineItem = context.ShopifyOrderLineItems.FirstOrDefault(l => l.OriginalShopifyId.Equals(lineItem.OriginalShopifyId) && l.CompanySiteId == request.CompanySiteId);
                        if (_lineItem == null)
                        {
                            _lineItem = new ShopifyOrderLineItem
                            {
                                Id = Guid.NewGuid(),
                                CompanySiteId = request.CompanySiteId,
                                ShopifyOrderId = order.Id,
                                OriginalShopifyId = lineItem.OriginalShopifyId,
                                OriginalShopifyOrderId = order.OriginalShopifyId,
                                DateCreated = now
                            };
                            context.ShopifyOrderLineItems.Add(_lineItem);
                        }

                        var product = context.ShopifyProducts.AsNoTracking().FirstOrDefault(p => p.OriginalShopifyId.Equals(lineItem.OriginalShopifyProductId) && p.CompanySiteId == request.CompanySiteId);
                        if (product == null) lineItem.ShopifyProductId = null;
                        else lineItem.ShopifyProductId = product.Id;

                        var variant = context.ShopifyProductVariants.AsNoTracking().FirstOrDefault(pv => pv.OriginalShopifyId.Equals(lineItem.OriginalShopifyProductVariantId) && pv.CompanySiteId == request.CompanySiteId);
                        if (variant == null) lineItem.ShopifyProductVariantId = null;
                        else lineItem.ShopifyProductVariantId = variant.Id;

                        MapperService.Map(lineItem, _lineItem);
                    }

                    context.SaveChanges();
                    response.ShopifyOrderId = order.Id;
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SaveShopifyOrderResponse { ErrorMessage = "Unable to save shopify order." };
            }
        }

        public SaveShopifyOrderShippingLineFromShopifyResponse SaveShopifyOrderShippingLineFromShopify(SaveShopifyOrderShippingLineFromShopifyRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTimeOffset(TimeZoneInfoId.CentralStandardTime);
                var response = new SaveShopifyOrderShippingLineFromShopifyResponse();
                using (var context = new FlavorFiEntities())
                {
                    var shopifyOrderShippingLine = context.ShopifyOrderShippingLines.FirstOrDefault(_ => _.OriginalShopifyId.Equals(request.ShopifyOrderShippingLine.Id));
                    if (shopifyOrderShippingLine == null)
                    {
                        shopifyOrderShippingLine = new ShopifyOrderShippingLine
                        {
                            Id = Guid.NewGuid(),
                            CompanySiteId = request.CompanySiteId,
                            ShopifyOrderId = request.ShopfiyOrderId,
                            OriginalShopifyId = request.ShopifyOrderShippingLine.Id,
                            OriginalShopifyOrderId = request.OriginalShopifyOrderId,
                            DateCreated = now
                        };
                        context.ShopifyOrderShippingLines.Add(shopifyOrderShippingLine);
                    }

                    shopifyOrderShippingLine.Title = request.ShopifyOrderShippingLine.Title ?? "";
                    shopifyOrderShippingLine.Code = request.ShopifyOrderShippingLine.Code ?? "";
                    shopifyOrderShippingLine.Source = request.ShopifyOrderShippingLine.Source ?? "";
                    shopifyOrderShippingLine.Price = string.IsNullOrEmpty(request.ShopifyOrderShippingLine.Price) ? 0.0m : Convert.ToDecimal(request.ShopifyOrderShippingLine.Price);
                    context.SaveChanges();

                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SaveShopifyOrderShippingLineFromShopifyResponse { ErrorMessage = "Unable to save shopify order shipping line from shopify." };
            }
        }
    }
}