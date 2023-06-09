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
    public static class ShopifyConsoleCustomerService
    {
        public static void LoadCustomers(ShopifyBaseSiteModel baseSite)
        {
            try
            {
                System.Console.WriteLine($"Getting all customers from shopify for {baseSite.BaseUrl}.");

                var perPage = 200;
                var shopifyCustomerRequest = new ShopifyRequestAndResponses.GetShopifyRecordsRequest { BaseSite = baseSite};
                var shopifyCustomerService = new ShopifyServices.ShopifyCustomerService();
                var shopifyCountResponse = shopifyCustomerService.GetShopifyRecordCount(shopifyCustomerRequest);
                var pageCount = shopifyCountResponse.Count / perPage;
                if (shopifyCountResponse.Count % perPage != 0) pageCount++;

                System.Console.WriteLine($"Synchronizing {shopifyCountResponse.Count} customers");
                System.Console.WriteLine($"{pageCount} pages at {DateTime.Now.ToLongTimeString()}");

                var databaseShopifyCustomerService = new DatabaseServices.ShopifyCustomerService();
                var mapperService = new DatabaseServices.MapperService();
                for (int i = 0; i <= pageCount; i++)
                {
                    var perPageRequest = new ShopifyRequestAndResponses.GetShopifyRecordsPerPageRequest { BaseSite = baseSite, Parameters = new Dictionary<string, string>() };
                    perPageRequest.Parameters.Add("limit", perPage.ToString());
                    perPageRequest.Parameters.Add("page", i.ToString());
                    var perPageResponse = shopifyCustomerService.GetShopifyCustomersPerPage(perPageRequest);
                    if (!perPageResponse.IsSuccess) throw new ApplicationException(perPageResponse.ErrorMessage);
                    foreach (var customer in perPageResponse.Customers)
                    {

                        var saveShopifyCustomerRequest = new DatabaseRequestAndResponses.SaveShopifyCustomerRequest { CompanySiteId = baseSite.SiteId };
                        mapperService.Map(customer, saveShopifyCustomerRequest.Customer);
                        var saveShopifyCustomerResponse = databaseShopifyCustomerService.SaveShopifyCustomer(saveShopifyCustomerRequest);
                        if (!saveShopifyCustomerResponse.IsSuccess)
                            System.Console.WriteLine($"Customer Error [Customer: {customer.Id}][Error: {saveShopifyCustomerResponse.ErrorMessage}]");
                        else
                        {
                            //var metafieldRequest = new ShopifyRequestAndResponses.GetShopifyRecordRequest
                            //{
                            //    RecordId = product.Id
                            //};
                            //var metafieldResponse = shopifyProductService.GetShopifyProductMetafields(metafieldRequest);
                            //if (!metafieldResponse.IsSuccess)
                            //    System.Console.WriteLine($"Product Get Metafield Error [Product: {product.Id}][Error: {metafieldResponse.ErrorMessage}]");
                            //else
                            //{
                            //    foreach (var metafield in metafieldResponse.Metafields)
                            //    {
                            //        var saveShopifyMetafieldRequest = new DatabaseRequestAndResponses.SaveShopifyMetafieldRequest { };
                            //        mapperService.Map(metafield, saveShopifyMetafieldRequest.Metafield);
                            //        var saveShopifyMetafieldResponse = databaseShopifyProductService.SaveShopifyProductMetafield(saveShopifyMetafieldRequest);
                            //        if (!saveShopifyMetafieldResponse.IsSuccess)
                            //            System.Console.WriteLine($"Product Save Metafield Error [Product: {product.Id}][Metafield: {metafield.Id}][Error: {metafieldResponse.ErrorMessage}]");
                            //    }
                            //}
                        }
                    }

                    System.Console.WriteLine($"finished with page {i} of {pageCount} at {DateTime.Now.ToLongTimeString()}");
                }

                System.Console.WriteLine($"Synchronization of customers in the database is complete at {DateTime.Now.ToLongTimeString()}");
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
