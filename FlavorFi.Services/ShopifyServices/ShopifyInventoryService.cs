using FlavorFi.Common.Enums;
using FlavorFi.Common.Models.ShopifyModels;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using System;

namespace FlavorFi.Services.ShopifyServices
{
    public interface IShopifyInventoryService
    {
        GetShopifyInventoryLocationsResponse GetShopifyInventoryLoactions(GetShopifyRecordsRequest request);
    }

    public class ShopifyInventoryService : ShopifyBaseService, IShopifyInventoryService
    {
        private const ShopifyResourceType _shopifyResourceType = ShopifyResourceType.locations;

        public ShopifyInventoryService() : base(_shopifyResourceType) { }

        /// <summary>
        ///     Gets all of the shopify inventory locations.
        /// </summary>
        /// <param name="request">GetShopifyRecordsRequest</param>
        /// <returns>GetShopifyInventoryLocationsResponse</returns>
        public GetShopifyInventoryLocationsResponse GetShopifyInventoryLoactions(GetShopifyRecordsRequest request)
        {
            try
            {
                var response = new GetShopifyInventoryLocationsResponse();
                var baseResponse = GetShopifyRecords(request);
                if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);

                response.Locations = ShopifyMapperService.MapToList<ShopifyInvnetoryLocationModel>(baseResponse.Data, _shopifyResourceType);
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetShopifyInventoryLocationsResponse { ErrorMessage = ex.Message };
            }
        }

    }
}
