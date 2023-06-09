using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Services.DatabaseServices;
using System;
using FlavorFi.Common.Models.ShopifyModels;
using FlavorFi.Services.FacebookServices;
using FlavorFi.Common.RequestAndResponses.FacebookRequestAndResponses;

namespace FlavorFi.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var companySiteId = Guid.Parse("97948564-E3E5-4A8D-A20E-676832D0636E"); // Laurie Belle - Online Production
                var shopifyBaseSiteModel = GetShopifyBaseSiteModel(companySiteId);

                // Load shopify products and shopify product variantes
                // ShopifyConsoleServices.ShopifyConsoleProductSevice.LoadProducts(shopifyBaseSiteModel);

                // Load shopify customers
                // ShopifyConsoleServices.ShopifyConsoleCustomerService.LoadCustomers(shopifyBaseSiteModel);

                // Load shopify orders
                ShopifyConsoleServices.ShopifyConsoleOrderService.LoadOrders(shopifyBaseSiteModel);

                // Load shopify gift cards
                // ShopifyConsoleServices.ShopifyConsoleGiftCardService.LoadGiftCardsFromCSVFile(shopifyBaseSiteModel);

                //FacebookService();


                System.Console.WriteLine("Finished");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            finally
            {
                System.Console.ReadKey();
            }
        }

        private static ShopifyBaseSiteModel GetShopifyBaseSiteModel(Guid companySiteId)
        {
            var siteResponse = new CompanyService().GetCompanySite(new GetCompanySiteRequest { CompanySiteId = companySiteId });
            if (!siteResponse.IsSuccess)
                throw new ApplicationException($"Error getting company site: {siteResponse.ErrorMessage}");
            return new ShopifyBaseSiteModel
            {
                SiteId = siteResponse.CompanySite.Id,
                BaseUrl = siteResponse.CompanySite.ShopifyUrl,
                BaseWebhookUrl = siteResponse.CompanySite.ShopifyWebhookUrl,
                PublicApiKey = siteResponse.CompanySite.ShopifyApiPublicKey,
                SecretApiKey = siteResponse.CompanySite.ShopifyApiSecretKey
            };
        }

        public static void FacebookService()
        {
            try
            {
                //var appId = "253343078936352";
                //var appSecret = "4d012a3e30403e3c49e6ab14a9eb0c3a";
                //var accessToken = "EAADmagg57yABAPoHZCUtAwuicOL2mZChtuvZAArmeWZCRzANqKlxIoQyjR63KZAONkPnbkSZBoJ11t6KmWgKM5DZBjQtxY67fwZBq1XcdL0AvdVcXHxrtELF3LXTZBcbPcGeMxUMpJKXjSBkfvz0ZAMRyySOdezQQc2jVZCQzGHZBFIJpsgQGm76hsdl8ToTzSLcxX1tm01eTKcNNgZDZD";
                //var pageAccessToken = "EAADmagg57yABANyPfY36QGuceqCQJdGd4RuywKwaQHBmceZAHU1RANggnyRa8iHTeMxK8kJDNCJsdcjNubscQZBXMxddUnIGvA1mkBvot45ZAVIJWr0Yu3WVns54UNsml0LoVYlzPpn6ix6CBgjnrwfeyLwmDAnM2OhCiUxBKMZCpQW96GzXdZCSki9PCrJqAWdWZBRjy6wwZDZD";

                var facebookService = new FacebookPageService(new FacebookClient());
                var getSubscibedAppsResponse = facebookService.GetSubscribedApps(new GetSubscribedAppsRequest { PageId = "1002274386534987" });
                foreach (var app in getSubscibedAppsResponse.SubscribedApps)
                {
                    System.Console.WriteLine($"Id: {app.Id}\nName: {app.Name}\nLink: {app.Link}");
                    System.Console.WriteLine();
                }



                //var account = getAccountTask.Result;
                // System.Console.WriteLine($"{account.Id} {account.Name}");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"[ErrorMessage: {ex.Message}");
            }
        }
    }

}