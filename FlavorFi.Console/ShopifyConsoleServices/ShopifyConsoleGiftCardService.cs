using System;
using FlavorFi.Services.ImportServices;
using FlavorFi.Common.Models.ShopifyModels;
using FlavorFi.Services.DatabaseServices;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;

namespace FlavorFi.Console.ShopifyConsoleServices
{
    public static class ShopifyConsoleGiftCardService
    {
        public static void LoadGiftCardsFromCSVFile(ShopifyBaseSiteModel baseSite)
        {
            try
            {
                System.Console.WriteLine($"Getting all gift cards from csv file {baseSite.BaseUrl}.");
                var path = @"C:\Users\Development\Desktop\gift_cards_export.csv";
                var giftCardList = new CsvImportService().CreateModelListFromCsvFile(path);
                System.Console.WriteLine($"Importing {giftCardList.Count} gift cards from csv file at {DateTime.Now.ToLongTimeString()}");
                foreach (var giftCard in giftCardList)
                {
                    var index = giftCardList.IndexOf(giftCard);
                    if (index % 1000 == 0 && index > 0)
                        System.Console.WriteLine($"{index} gift cards have been synchronized at {DateTime.Now.ToLongTimeString()}");

                    var saveShopifyGiftCardReportRequest = new SaveShopifyGiftCardReportRequest { CompanySiteId = baseSite.SiteId, GiftCard = giftCard };
                    var saveShopifyGiftCardResponse = new ShopifyGiftCardService().SaveShopifyGiftCardReport(saveShopifyGiftCardReportRequest);
                    if (!saveShopifyGiftCardResponse.IsSuccess)
                        System.Console.WriteLine(saveShopifyGiftCardResponse.ErrorMessage);
                }

                System.Console.WriteLine($"Synchronization of gift cards in the database is complete at {DateTime.Now.ToLongTimeString()}");
            }
            catch (Exception ex) { throw ex; }
        }

    }
}