using FlavorFi.Common.Enums;
using FlavorFi.Common.Helpers;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Data;
using System;
using System.Linq;

namespace FlavorFi.Services.DatabaseServices
{
    public interface IShopifyCollectionService
    {
        SaveShopifyCustomCollectionFromShopifyResponse SaveShopifyCustomCollectionFromShopify(SaveShopifyCustomCollectionFromShopifyRequest request);
    }

    public class ShopifyCollectionService : BaseService, IShopifyCollectionService
    {
        /// <summary>
        ///     Adds and updates a custom collection from shopify to the database
        /// </summary>
        /// <param name="request">SaveShopifyCustomCollectionFromShopifyRequest</param>
        /// <returns>SaveShopifyCustomCollectionFromShopifyResponse</returns>
        public  SaveShopifyCustomCollectionFromShopifyResponse SaveShopifyCustomCollectionFromShopify(SaveShopifyCustomCollectionFromShopifyRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var response = new SaveShopifyCustomCollectionFromShopifyResponse();
                using (var context = new FlavorFiEntities())
                {
                    var shopifyCustomCollection = context.ShopifyCustomCollections.FirstOrDefault(_ => _.OriginalShopifyId.Equals(request.CustomCollection.Id));
                    if (shopifyCustomCollection == null)
                    {
                        shopifyCustomCollection = new ShopifyCustomCollection
                        {
                            Id = Guid.NewGuid(),
                            CompanySiteId = request.CompanySiteId,
                            OriginalShopifyId = request.CustomCollection.Id,
                            DateCreated = now
                        };
                        context.ShopifyCustomCollections.Add(shopifyCustomCollection);
                    }

                    shopifyCustomCollection.Title = request.CustomCollection.Title;
                   // shopifyCustomCollection.Handle = request.CustomCollection.Handle;
                    shopifyCustomCollection.SortOrder = request.CustomCollection.SortOrder;
                    //shopifyCustomCollection.AdminGraphqlApiId = request.CustomCollection.AdminGraphqlApiId;
                    shopifyCustomCollection.Published = request.CustomCollection.Published;
                    shopifyCustomCollection.PublishedAt = request.CustomCollection.PublishedAt;
                    shopifyCustomCollection.UpdatedAt = request.CustomCollection.UpdatedAt;
                    context.SaveChanges();

                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SaveShopifyCustomCollectionFromShopifyResponse { ErrorMessage = ex.Message };
            }
        }
    }
}