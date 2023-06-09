using FlavorFi.Common.Enums;
using FlavorFi.Common.Models.ShopifyModels;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using System;

namespace FlavorFi.Services.ShopifyServices
{
    public interface IShopifyUserService
    {
        GetShopifyUsersResponse GetShopifyUsers(GetShopifyRecordByQueryRequest request);
    }

    public class ShopifyUserService : ShopifyBaseService, IShopifyUserService
    {
        private const ShopifyResourceType _shopifyResourceType = ShopifyResourceType.users;

        public ShopifyUserService() : base(_shopifyResourceType) { }

        /// <summary>
        ///     Gets all of the shopify users.
        /// </summary>
        /// <param name="request">GetShopifyRecordsRequest</param>
        /// <returns>GetShopifyUsersResponse</returns>
        public GetShopifyUsersResponse GetShopifyUsers(GetShopifyRecordByQueryRequest request)
        {
            try
            {
                var response = new GetShopifyUsersResponse();
                var baseResponse = GetShopifyRecordByQuery(request);
                if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);

                response.Users = ShopifyMapperService.MapToList<ShopifyUserModel>(baseResponse.Data, _shopifyResourceType);
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetShopifyUsersResponse { ErrorMessage = ex.Message };
            }
        }
    }
}