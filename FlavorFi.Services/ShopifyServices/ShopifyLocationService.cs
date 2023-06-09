using FlavorFi.Common.Enums;
using FlavorFi.Common.Models.ShopifyModels;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using System;

namespace FlavorFi.Services.ShopifyServices
{
    public interface IShopifyLocationService
    {
        GetShopifyLocationsResponse GetShopifyLocations(GetShopifyRecordsRequest request);
    }

    public class ShopifyLocationService : ShopifyBaseService, IShopifyLocationService
    {
        private const ShopifyResourceType _shopifyResourceType = ShopifyResourceType.locations;

        public ShopifyLocationService() : base(_shopifyResourceType) { }

        public GetShopifyLocationsResponse GetShopifyLocations(GetShopifyRecordsRequest request)
        {
            try
            {
                var response = new GetShopifyLocationsResponse();
                var baseResponse = GetShopifyRecords(request);
                if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);

                response.Locations = ShopifyMapperService.MapToList<ShopifyLocationModel>(baseResponse.Data, _shopifyResourceType);
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetShopifyLocationsResponse { ErrorMessage = ex.Message };
            }
        }
    }
}