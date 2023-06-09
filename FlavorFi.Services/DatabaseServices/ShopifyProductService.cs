using FlavorFi.Common.Enums;
using FlavorFi.Common.Helpers;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Data;
using System;
using System.Linq;

namespace FlavorFi.Services.DatabaseServices
{
    public interface IShopifyProductService
    {
        SaveShopifyProductResponse SaveShopifyProduct(SaveShopifyProductRequest request);
        SaveShopifyMetafieldResponse SaveShopifyProductMetafield(SaveShopifyMetafieldRequest request);
    }

    public class ShopifyProductService : BaseService, IShopifyProductService
    {
        /// <summary>
        ///     Adds and updates a product from shopify to the database
        /// </summary>
        /// <param name="request">SaveShopifyProductRequest</param>
        /// <returns>SaveShopifyProductResponse</returns>
        public SaveShopifyProductResponse SaveShopifyProduct(SaveShopifyProductRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var response = new SaveShopifyProductResponse();
                using (var context = new FlavorFiEntities())
                {
                    var product = context.ShopifyProducts.FirstOrDefault(p => p.OriginalShopifyId.Equals(request.Product.OriginalShopifyId) && p.CompanySiteId == request.CompanySiteId);
                    if (product == null)
                    {
                        product = new ShopifyProduct
                        {
                            Id = Guid.NewGuid(),
                            CompanySiteId = request.CompanySiteId,
                            OriginalShopifyId = request.Product.OriginalShopifyId,
                            DateCreated = now
                        };
                        context.ShopifyProducts.Add(product);
                    }

                    MapperService.Map(request.Product, product);
                    foreach (var variant in request.Product.Variants)
                    {
                        var _variant = context.ShopifyProductVariants.FirstOrDefault(v => v.OriginalShopifyId.Equals(variant.OriginalShopifyId));
                        if (_variant == null)
                        {
                            _variant = new ShopifyProductVariant
                            {
                                Id = Guid.NewGuid(),
                                CompanySiteId = product.CompanySiteId,
                                ShopifyProductId = product.Id,
                                OriginalShopifyId = variant.OriginalShopifyId,
                                OriginalShopifyProductId = variant.OriginalShopifyProductId,
                                DateCreated = now
                            };
                            context.ShopifyProductVariants.Add(_variant);
                        }
                        MapperService.Map(variant, _variant);
                    }

                    context.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SaveShopifyProductResponse { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        ///     Adds and updates a product metafield from shopify to the database
        /// </summary>
        /// <param name="request">SaveShopifyMetafieldRequest</param>
        /// <returns>SaveShopifyMetafieldResponse</returns>
        public SaveShopifyMetafieldResponse SaveShopifyProductMetafield(SaveShopifyMetafieldRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var response = new SaveShopifyMetafieldResponse();
                using (var context = new FlavorFiEntities())
                {
                    var product = context.ShopifyProducts.FirstOrDefault(p => p.OriginalShopifyId.Equals(request.Metafield.OriginalShopifyOwnerId) && p.CompanySiteId == request.CompanySiteId);
                    if (product == null) return new SaveShopifyMetafieldResponse { ErrorMessage = "Unable to find product for id " + request.Metafield.OriginalShopifyOwnerId };
                    var metafield = context.ShopifyMetafields.FirstOrDefault(m => m.OriginalShopifyId.Equals(request.Metafield.OriginalShopifyId));
                    if (metafield == null)
                    {
                        metafield = new ShopifyMetafield
                        {
                            Id = Guid.NewGuid(),
                            ShopifyCustomCollectionId = null,
                            ShopifyCustomerId = null,
                            ShopifyProductId = product.Id,
                            OriginalShopifyId = request.Metafield.OriginalShopifyId,
                            OriginalShopifyOwnerId = request.Metafield.OriginalShopifyOwnerId,
                            OwnerResource = request.Metafield.OwnerResource,
                            DateCreated = now
                        };
                        context.ShopifyMetafields.Add(metafield);
                    }

                    MapperService.Map(request.Metafield, metafield);
                    context.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SaveShopifyMetafieldResponse { ErrorMessage = ex.Message };
            }
        }
    }
}