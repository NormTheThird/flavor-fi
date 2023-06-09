using FlavorFi.Common.Models.ShopifyModels;
using FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using System;
using FlavorFi.Common.Enums;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;

namespace FlavorFi.Services.ShopifyServices
{
    public interface IShopifyCollectionService
    {
        GetShopifyCustomCollectionsResponse GetShopifyCustomCollectionsPerPage(GetShopifyRecordsPerPageRequest request);
    }

    public class ShopifyCollectionService : ShopifyBaseService, IShopifyCollectionService
    {
        private const ShopifyResourceType _shopifyResourceType = ShopifyResourceType.custom_collections;

        public ShopifyCollectionService() : base(_shopifyResourceType) { }

        /// <summary>
        ///     Gets all of the shopify custom collections for a certain date range for a certain page.
        /// </summary>
        /// <param name="request">GetShopifyRecordsPerPageRequest</param>
        /// <returns>GetShopifyCustomCollectionsResponse</returns>
        public GetShopifyCustomCollectionsResponse GetShopifyCustomCollectionsPerPage(GetShopifyRecordsPerPageRequest request)
        {
            try
            {
                var response = new GetShopifyCustomCollectionsResponse();
                var baseResponse = GetShopifyRecordsPerPage(request);
                if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);

                response.CustomCollections = ShopifyMapperService.MapToList<ShopifyCustomCollectionModel>(baseResponse.Data, _shopifyResourceType);
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetShopifyCustomCollectionsResponse { ErrorMessage = ex.Message };
            }
        }
    }
}