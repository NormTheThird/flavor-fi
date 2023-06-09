using FlavorFi.Common.Models.DatabaseModels;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Data;
using System;
using System.Linq;
using System.Text;

namespace FlavorFi.Services.DatabaseServices
{
    //public class ReportService
    //{
    //    public static GetCostOfGoodsReportResponse GetCostOfGoodsReport(GetReportRequest _request)
    //    {
    //        try
    //        {
    //            if (TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time").IsDaylightSavingTime(_request.StartDate)) _request.StartDate = _request.StartDate.AddHours(5);
    //            else _request.StartDate = _request.StartDate.AddHours(6);
    //            if (TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time").IsDaylightSavingTime(_request.EndDate)) _request.EndDate = _request.EndDate.AddDays(1).AddHours(5);
    //            else _request.EndDate = _request.EndDate = _request.EndDate.AddDays(1).AddHours(6);
    //            var response = new GetCostOfGoodsReportResponse();
    //            using (var context = new FlavorFiEntities())
    //            {
    //                // TODO: TREY: 2018-11-2 Create a stored procedure for this.
    //                //context.Database.CommandTimeout = 600;
    //                //var items = context.ShopifyOrderLineItems.AsNoTracking().Where(l => l.ShopifyOrder.CreatedAt >= _request.StartDate && l.ShopifyOrder.CreatedAt < _request.EndDate 
    //                //                                                                 && !l.IsGiftCard && l.CompanySiteId == _request.CompanySiteId)
    //                //                                                        .Select(i => new CostOfGoodReportModel
    //                //                                                        {
    //                //                                                            OrderId = i.ShopifyOrder.Id,
    //                //                                                            ProductId = i.ShopifyProductId ?? Guid.Empty,
    //                //                                                            OriginalOrderId = i.ShopifyOrder.OriginalShopifyId,
    //                //                                                            OriginalProductId = i.ShopifyProduct == null ? 0 : i.ShopifyProduct.OriginalShopifyId,
    //                //                                                            Name = i.Name,
    //                //                                                            Sku = i.Sku,
    //                //                                                            Vendor = i.ShopifyProduct.Vendor,
    //                //                                                            Variant = i.VariantTitle,
    //                //                                                            Quantity = i.Quantity,
    //                //                                                            CostOfGood = i.ShopifyProduct.ShopifyMetafields.FirstOrDefault(m => m.Key.ToLower().Equals("mactualcost")) == null ? "" : i.ShopifyProduct.ShopifyMetafields.FirstOrDefault(m => m.Key.ToLower().Equals("mactualcost")).Value,
    //                //                                                            SalePrice = i.Price,
    //                //                                                            DateSold = i.ShopifyOrder.CreatedAt
    //                //                                                        });
    //                //response.CostOfGoods = items.ToList();
    //                //response.Discounts = context.ShopifyOrders.AsNoTracking().Where(o => o.CreatedAt >= _request.StartDate && o.CreatedAt < _request.EndDate).Select(o => o.TotalDiscounts).DefaultIfEmpty().Sum();
    //                //response.Refunds = context.ShopifyOrderTransactions.AsNoTracking().Where(t => t.ShopifyOrder.CreatedAt >= _request.StartDate && t.ShopifyOrder.CreatedAt < _request.EndDate && t.Kind.Equals("refund") && t.ShopifyGiftCardId == null).Select(t => t.Amount).DefaultIfEmpty().Sum();
    //                //response.Vendors = items.Select(o => o.Vendor).DefaultIfEmpty().Distinct().ToList();
    //                //response.IsSuccess = true;
    //                //return response;
    //                return new GetCostOfGoodsReportResponse { ErrorMessage = "Need to create a stored procedure for this call." };
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LoggingService.LogError(new LogErrorRequest { ex = ex });
    //            return new GetCostOfGoodsReportResponse { ErrorMessage = "Unable to get cost of goods report." };
    //        }
    //    }

    //    public static GetExportReportResponse GetExportCostOfGoodsReport(GetReportRequest _request)
    //    {
    //        try
    //        {
    //            var dataResponse = GetCostOfGoodsReport(_request);
    //            if (!dataResponse.IsSuccess) throw new ApplicationException(dataResponse.ErrorMessage);

    //            var response = new GetExportReportResponse();
    //            var csv = new StringBuilder();
    //            var header = "Product Id,Item Name,Sku,Vendor,Variant,Quantity,Cost Of Good,Sale Amount,Markup Amount,Date Sold";
    //            csv.AppendLine(header);
    //            foreach (var item in dataResponse.CostOfGoods)
    //            {
    //                if (string.IsNullOrEmpty(item.CostOfGood)) item.CostOfGood = "0";
    //                else if (item.CostOfGood.Trim().Equals(".")) item.CostOfGood = "0";
    //                else item.CostOfGood = item.CostOfGood.Replace("-", ".");
    //                csv.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", item.OriginalProductId.ToString(), item.Name,
    //                               item.Sku, item.Vendor, item.Variant, item.Quantity, item.CostOfGood, item.SalePrice.ToString("C"),
    //                               ((item.SalePrice - Convert.ToDecimal(item.CostOfGood)) * item.Quantity).ToString("C"), item.DateSold));
    //            }

    //            response.Report = Encoding.ASCII.GetBytes(csv.ToString());
    //            response.IsSuccess = true;
    //            return response;
    //        }
    //        catch (Exception ex)
    //        {
    //            LoggingService.LogError(new LogErrorRequest { ex = ex });
    //            return new GetExportReportResponse { ErrorMessage = "Unable to get cost of goods report export." };
    //        }
    //    }


    //    public static GetGiftCardReportResponse GetGiftCardReport(GetReportRequest _request)
    //    {
    //        try
    //        {
    //            if (TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time").IsDaylightSavingTime(_request.StartDate)) _request.StartDate = _request.StartDate.AddHours(5);
    //            else _request.StartDate = _request.StartDate.AddHours(6);
    //            if (TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time").IsDaylightSavingTime(_request.EndDate)) _request.EndDate = _request.EndDate.AddDays(1).AddHours(5);
    //            else _request.EndDate = _request.EndDate = _request.EndDate.AddDays(1).AddHours(6);
    //            var response = new GetGiftCardReportResponse();
    //            using (var context = new FlavorFiEntities())
    //            {
    //                // TODO: TREY: 2018-11-2 Create a stored procdure for this.
    //                //context.Database.CommandTimeout = 600;
    //                //var giftCards = context.ShopifyGiftCards.AsNoTracking().Where(gc => gc.CreatedAt >= _request.StartDate && gc.CreatedAt < _request.EndDate 
    //                //                                                                 && gc.DisabledAt == null && gc.CompanySiteId == _request.CompanySiteId)
    //                //                                 .Select(gc => new GiftCardReportModel
    //                //                                 {
    //                //                                     Id = gc.Id,
    //                //                                     LastFour = gc.LastFour,
    //                //                                     IssuedBy = "",
    //                //                                     Customer = gc.ShopifyCustomer == null ? "" : gc.ShopifyCustomer.FirstName + " " + gc.ShopifyCustomer.LastName,
    //                //                                     Note = gc.Note,
    //                //                                     Balance = gc.Balance,
    //                //                                     InitialValue = gc.InitialValue,
    //                //                                     DateCreated = gc.CreatedAt
    //                //                                 });

    //                //response.TotalAmountUsed = context.ShopifyOrderTransactions.Where(t => t.ShopifyOrder.CreatedAt >= _request.StartDate && t.ShopifyOrder.CreatedAt < _request.EndDate && t.ShopifyGiftCardId != null)
    //                //                                                           .Select(t => t.Amount).DefaultIfEmpty().Sum();

    //                //var totalGiftCards = context.ShopifyGiftCards.AsNoTracking().Where(gc => gc.DisabledAt == null).Select(gc => gc.InitialValue).DefaultIfEmpty().Sum();
    //                //var totalUsed = context.ShopifyOrderTransactions.Where(t => t.ShopifyGiftCardId != null).Select(t => t.Amount).DefaultIfEmpty().Sum();
    //                //response.TotalLiability = totalGiftCards - totalUsed;
    //                //response.GiftCards = giftCards.ToList();
    //                //response.IsSuccess = true;
    //                //return response;
    //                return new GetGiftCardReportResponse { ErrorMessage = "Need to create a stored procedure for this call." };
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LoggingService.LogError(new LogErrorRequest { ex = ex });
    //            return new GetGiftCardReportResponse { ErrorMessage = "Unable to get gift card report." };
    //        }
    //    }

    //    public static GetExportReportResponse GetExportGiftCardReport(GetReportRequest _request)
    //    {
    //        try
    //        {
    //            var dataResponse = GetGiftCardReport(_request);
    //            if (!dataResponse.IsSuccess) throw new ApplicationException(dataResponse.ErrorMessage);

    //            var response = new GetExportReportResponse();
    //            var csv = new StringBuilder();
    //            var header = "Card Last 4, Balance, Initial Value, Issued By, Customer, Creation Date, Notes";
    //            csv.AppendLine(header);
    //            foreach (var item in dataResponse.GiftCards)
    //                csv.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6}", item.LastFour.ToUpper(), item.Balance, item.InitialValue, item.IssuedBy, item.Customer, item.DateCreated, item.Note));
    //            response.Report = Encoding.ASCII.GetBytes(csv.ToString());
    //            response.IsSuccess = true;
    //            return response;
    //        }
    //        catch (Exception ex)
    //        {
    //            LoggingService.LogError(new LogErrorRequest { ex = ex });
    //            return new GetExportReportResponse { ErrorMessage = "Unable to get gift card report export." };
    //        }
    //    }


    //    public static GetSalesReportResponse GetSalesReport(GetReportRequest _request)
    //    {
    //        try
    //        {
    //            if (TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time").IsDaylightSavingTime(_request.StartDate)) _request.StartDate = _request.StartDate.AddHours(5);
    //            else _request.StartDate = _request.StartDate.AddHours(6);
    //            if (TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time").IsDaylightSavingTime(_request.EndDate)) _request.EndDate = _request.EndDate.AddDays(1).AddHours(5);
    //            else _request.EndDate = _request.EndDate = _request.EndDate.AddDays(1).AddHours(6);
    //            var response = new GetSalesReportResponse();
    //            using (var context = new FlavorFiEntities())
    //            {
    //                var orders = context.ShopifyOrders.AsNoTracking().Where(o => o.CreatedAt >= _request.StartDate && o.CreatedAt < _request.EndDate && o.CompanySiteId == _request.CompanySiteId);
    //                response.GrossSales = orders.SelectMany(o => o.ShopifyOrderLineItems).Where(l => !l.IsGiftCard).Select(l => l.Price * l.Quantity).DefaultIfEmpty().Sum();
    //                response.Discounts = orders.Select(o => o.TotalDiscounts).DefaultIfEmpty().Sum();
    //                response.Returns = 12.0m;
    //                response.SalesTax = 0.0m;
    //                response.IsSuccess = true;
    //                return response;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LoggingService.LogError(new LogErrorRequest { ex = ex });
    //            return new GetSalesReportResponse { ErrorMessage = "Unable to get sales report." };
    //        }
    //    }
    //}
}