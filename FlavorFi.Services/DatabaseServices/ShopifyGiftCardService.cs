using FlavorFi.Common.Enums;
using FlavorFi.Common.Helpers;
using FlavorFi.Common.Models.DatabaseModels;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlavorFi.Services.DatabaseServices
{
    public interface IShopifyGiftCardService
    {
        SaveShopifyGiftCardResponse SaveShopifyGiftCard(SaveShopifyGiftCardRequest request);
        GetShopifyGiftCardTotalsResponse GetShopifyGiftCardTotals(GetShopifyGiftCardTotalsRequest request);
        GetShopifyGiftCardMonthlyTotalsResponse GetShopifyGiftCardMonthlyTotals(GetShopifyGiftCardMonthlyTotalsRequest request);
    }

    public class ShopifyGiftCardService : BaseService, IShopifyGiftCardService
    {
        public SaveShopifyGiftCardResponse SaveShopifyGiftCard(SaveShopifyGiftCardRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTimeOffset(TimeZoneInfoId.CentralStandardTime);
                var response = new SaveShopifyGiftCardResponse();
                using (var context = new FlavorFiEntities())
                {
                    var giftCard = context.ShopifyGiftCards.FirstOrDefault(o => o.OriginalShopifyId.Equals(request.GiftCard.OriginalShopifyId));
                    if (giftCard == null)
                    {
                        giftCard = new ShopifyGiftCard
                        {
                            Id = Guid.NewGuid(),
                            CompanySiteId = request.CompanySiteId,
                            OriginalShopifyId = request.GiftCard.OriginalShopifyId,
                            DateCreated = now
                        };
                        context.ShopifyGiftCards.Add(giftCard);
                    }
                    giftCard.ShopifyUserId = request.GiftCard.ShopifyUserId;
                    giftCard.ShopifyCustomerId = request.GiftCard.ShopifyUserId;
                    giftCard.ShopifyOrderId = request.GiftCard.ShopifyUserId;
                    giftCard.OriginalShopifyUserId = request.GiftCard.OriginalShopifyUserId;
                    giftCard.OriginalShopifyCustomerId = request.GiftCard.OriginalShopifyCustomerId;
                    giftCard.OriginalShopifyOrderId = request.GiftCard.OriginalShopifyOrderId;
                    giftCard.OriginalShopifyLineItemId = request.GiftCard.OriginalShopifyLineItemId;
                    giftCard.OriginalShopifyApiClientId = request.GiftCard.OriginalShopifyApiClientId;
                    giftCard.LastFour = request.GiftCard.LastFour;
                    giftCard.Note = request.GiftCard.Note;
                    giftCard.Balance = request.GiftCard.Balance;
                    giftCard.InitialValue = request.GiftCard.InitialValue;
                    giftCard.CreatedAt = request.GiftCard.CreatedAt;
                    giftCard.UpdatedAt = request.GiftCard.UpdatedAt;
                    giftCard.DisabledAt = request.GiftCard.DisabledAt;
                    giftCard.ExpiresOn = request.GiftCard.ExpiresOn;

                    context.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SaveShopifyGiftCardResponse { ErrorMessage = "unable to save shopify gift card" };
            }
        }

        public SaveShopifyGiftCardResponse SaveShopifyGiftCardReport(SaveShopifyGiftCardReportRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTimeOffset(TimeZoneInfoId.CentralStandardTime);
                var response = new SaveShopifyGiftCardResponse();
                using (var context = new FlavorFiEntities())
                {
                    var giftCard = context.ShopifyGiftCards.FirstOrDefault(o => o.OriginalShopifyId.Equals(request.GiftCard.OriginalShopifyId));
                    if (giftCard == null)
                    {
                        giftCard = new ShopifyGiftCard
                        {
                            Id = Guid.NewGuid(),
                            CompanySiteId = request.CompanySiteId,
                            OriginalShopifyId = request.GiftCard.OriginalShopifyId,
                            DateCreated = now
                        };
                        context.ShopifyGiftCards.Add(giftCard);
                    }

                    var originalShopifyCustomerId = (long)0;
                    var shopifyCustomerId = (Guid?)null;
                    if (!string.IsNullOrEmpty(request.GiftCard.OrderName))
                    {
                        var customer = context.ShopifyCustomers.AsNoTracking().FirstOrDefault(o => o.Email.Equals(request.GiftCard.Email, StringComparison.CurrentCultureIgnoreCase));
                        if (customer != null)
                        {
                            originalShopifyCustomerId = customer.OriginalShopifyId;
                            shopifyCustomerId = customer.Id;
                        }
                    }

                    var originalShopifyOrderId = (long)0;
                    var shopifyOrderId = (Guid?)null;
                    if (!string.IsNullOrEmpty(request.GiftCard.OrderName))
                    {
                        var order = context.ShopifyOrders.AsNoTracking().FirstOrDefault(o => o.Name.Equals(request.GiftCard.OrderName, StringComparison.CurrentCultureIgnoreCase));
                        if (order != null)
                        {
                            originalShopifyOrderId = order.OriginalShopifyId;
                            shopifyOrderId = order.Id;
                        }
                    }

                    giftCard.ShopifyUserId = null;
                    giftCard.ShopifyCustomerId = shopifyCustomerId;
                    giftCard.ShopifyOrderId = shopifyOrderId;
                    giftCard.OriginalShopifyUserId = 0;
                    giftCard.OriginalShopifyCustomerId = originalShopifyCustomerId;
                    giftCard.OriginalShopifyOrderId = originalShopifyOrderId;
                    giftCard.OriginalShopifyLineItemId = 0;
                    giftCard.OriginalShopifyApiClientId = 0;
                    giftCard.LastFour = request.GiftCard.LastFour;
                    giftCard.Note = "";
                    giftCard.Balance = request.GiftCard.Balance;
                    giftCard.InitialValue = request.GiftCard.InitialValue;
                    giftCard.CreatedAt = request.GiftCard.CreatedAt;
                    giftCard.UpdatedAt = request.GiftCard.CreatedAt;
                    giftCard.DisabledAt = request.GiftCard.DisabledAt;
                    giftCard.ExpiresOn = request.GiftCard.ExpiresOn;

                    context.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SaveShopifyGiftCardResponse { ErrorMessage = "unable to save shopify gift card" };
            }
        }

        public GetShopifyGiftCardTotalsResponse GetShopifyGiftCardTotals(GetShopifyGiftCardTotalsRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTimeOffset(TimeZoneInfoId.CentralStandardTime);
                var response = new GetShopifyGiftCardTotalsResponse();
                using (var context = new FlavorFiEntities())
                {
                    //TODO: TREY: 1/3/2018 Get real data from stored proc
                    if (request.Year == 2018)
                    {
                        response.ShopifyGiftCardTotals = new ShopifyGiftCardTotalsModel
                        {
                            GiftCardTotalAmount = 125000.0m,
                            GiftCardAmountFromReturns = 25000.0m,
                            RemainingTotalBalance = 100000.0m,
                            RemainingReturnBalance = 5000.0m
                        };
                    }
                    else
                    {
                        response.ShopifyGiftCardTotals = new ShopifyGiftCardTotalsModel
                        {
                            GiftCardTotalAmount = 1000.0m,
                            GiftCardAmountFromReturns = 500.0m,
                            RemainingTotalBalance = 500.0m,
                            RemainingReturnBalance = 200.0m
                        };
                    }

                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetShopifyGiftCardTotalsResponse { ErrorMessage = "unable to get shopify gift card totals" };
            }
        }

        public GetShopifyGiftCardMonthlyTotalsResponse GetShopifyGiftCardMonthlyTotals(GetShopifyGiftCardMonthlyTotalsRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTimeOffset(TimeZoneInfoId.CentralStandardTime);
                var response = new GetShopifyGiftCardMonthlyTotalsResponse();
                using (var context = new FlavorFiEntities())
                {
                    //TODO: TREY: 1/3/2018 Get real data from stored proc
                    var totals = new List<ShopifyGiftCardMonthlyTotalsModel>();
                    if (request.Year == 2018)
                    {
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 0, GiftCardTotalAmount = 1200.0m, GiftCardAmountFromReturns = 300.0m });
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 1, GiftCardTotalAmount = 1000.0m, GiftCardAmountFromReturns = 300.0m });
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 2, GiftCardTotalAmount = 2500.0m, GiftCardAmountFromReturns = 300.0m });
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 3, GiftCardTotalAmount = 500.0m, GiftCardAmountFromReturns = 300.0m });
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 4, GiftCardTotalAmount = 700.0m, GiftCardAmountFromReturns = 300.0m });
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 5, GiftCardTotalAmount = 3000.0m, GiftCardAmountFromReturns = 300.0m });
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 6, GiftCardTotalAmount = 3200.0m, GiftCardAmountFromReturns = 300.0m });
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 7, GiftCardTotalAmount = 2400.0m, GiftCardAmountFromReturns = 300.0m });
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 8, GiftCardTotalAmount = 1800.0m, GiftCardAmountFromReturns = 300.0m });
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 9, GiftCardTotalAmount = 1200.0m, GiftCardAmountFromReturns = 300.0m });
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 10, GiftCardTotalAmount = 900.0m, GiftCardAmountFromReturns = 300.0m });
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 11, GiftCardTotalAmount = 1600.0m, GiftCardAmountFromReturns = 300.0m });
                    }
                    else
                    {
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 0, GiftCardTotalAmount = 11200.0m, GiftCardAmountFromReturns = 1300.0m });
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 1, GiftCardTotalAmount = 11000.0m, GiftCardAmountFromReturns = 1300.0m });
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 2, GiftCardTotalAmount = 12500.0m, GiftCardAmountFromReturns = 1300.0m });
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 3, GiftCardTotalAmount = 1500.0m, GiftCardAmountFromReturns = 1300.0m });
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 4, GiftCardTotalAmount = 1700.0m, GiftCardAmountFromReturns = 1300.0m });
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 5, GiftCardTotalAmount = 13000.0m, GiftCardAmountFromReturns = 1300.0m });
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 6, GiftCardTotalAmount = 13200.0m, GiftCardAmountFromReturns = 1300.0m });
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 7, GiftCardTotalAmount = 12400.0m, GiftCardAmountFromReturns = 1300.0m });
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 8, GiftCardTotalAmount = 11800.0m, GiftCardAmountFromReturns = 1300.0m });
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 9, GiftCardTotalAmount = 11200.0m, GiftCardAmountFromReturns = 1300.0m });
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 10, GiftCardTotalAmount = 1900.0m, GiftCardAmountFromReturns = 1300.0m });
                        totals.Add(new ShopifyGiftCardMonthlyTotalsModel { Month = 11, GiftCardTotalAmount = 11600.0m, GiftCardAmountFromReturns = 1300.0m });
                    }
                    response.ShopifyGiftCardMonthlyTotals = totals;

                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetShopifyGiftCardMonthlyTotalsResponse { ErrorMessage = "unable to get shopify gift card monthly totals" };
            }
        }
    }
}