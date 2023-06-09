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
    public static class ShopifyConsoleProductSevice
    {
        public static void LoadProducts(ShopifyBaseSiteModel baseSite)
        {
            try
            {
                System.Console.WriteLine($"Getting all products from shopify for {baseSite.BaseUrl}.");

                var perPage = 20;
                var shopifyProductRequest = new ShopifyRequestAndResponses.GetShopifyRecordsRequest { BaseSite = baseSite };
                var shopifyProductService = new ShopifyServices.ShopifyProductService();
                var shopifyCountResponse = shopifyProductService.GetShopifyRecordCount(shopifyProductRequest);
                var pageCount = shopifyCountResponse.Count / perPage;
                if (shopifyCountResponse.Count % perPage != 0) pageCount++;

                System.Console.WriteLine($"Synchronizing {shopifyCountResponse.Count} products");
                System.Console.WriteLine($"{pageCount} pages at {DateTime.Now.ToLongTimeString()}");

                var databaseShopifyProductService = new DatabaseServices.ShopifyProductService();
                var mapperService = new DatabaseServices.MapperService();
                for (int i = 0; i <= pageCount; i++)
                {
                    var perPageRequest = new ShopifyRequestAndResponses.GetShopifyRecordsPerPageRequest { BaseSite = baseSite, Parameters = new Dictionary<string, string>() };
                    perPageRequest.Parameters.Add("limit", perPage.ToString());
                    perPageRequest.Parameters.Add("page", i.ToString());
                    var perPageResponse = shopifyProductService.GetShopifyProductsPerPage(perPageRequest);
                    if (!perPageResponse.IsSuccess) throw new ApplicationException(perPageResponse.ErrorMessage);
                    foreach (var product in perPageResponse.Products)
                    {

                        var saveShopifyProductRequest = new DatabaseRequestAndResponses.SaveShopifyProductRequest { CompanySiteId = baseSite.SiteId };
                        mapperService.Map(product, saveShopifyProductRequest.Product);
                        var saveShopifyProductResponse = databaseShopifyProductService.SaveShopifyProduct(saveShopifyProductRequest);
                        if (!saveShopifyProductResponse.IsSuccess)
                            System.Console.WriteLine($"Product Error [Product: {product.Id}][Error: {saveShopifyProductResponse.ErrorMessage}]");
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

                System.Console.WriteLine($"Synchronization of products in the database is complete at {DateTime.Now.ToLongTimeString()}");
            }
            catch (Exception ex) { throw ex; }
        }
    }
}