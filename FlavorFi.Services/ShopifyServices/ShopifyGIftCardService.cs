using FlavorFi.Common.Enums;
using FlavorFi.Common.Models.ShopifyModels;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using FlavorFi.Services.DatabaseServices;
using FlavorFi.Services.ShopifyServices;
using System;

namespace FlavorFi.Services.ShopifyServices
{
    //public static class ShopifyGIftCardService
    //{
    //    /// <summary>
    //    ///     Gets a shopify gift card for a specific order id.
    //    /// </summary>
    //    /// <param name="request">GetShopifyRecordRequest</param>
    //    /// <returns>GetShopifyGiftCardResponse</returns>
    //    public static GetShopifyGiftCardResponse GetShopifyGiftCard(GetShopifyRecordRequest request)
    //    {
    //        try
    //        {
    //            var response = new GetShopifyGiftCardResponse();
    //            var baseService = new ShopifyBaseService(request.CompanySiteId);
    //            request.ResourceType = ShopifyResourceType.gift_cards;
    //            var baseResponse = baseService.GetShopifyRecord(request);
    //            if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
    //            response.GiftCard = ShopifyMapper.Map<ShopifyGiftCardModel>(baseResponse.Data);
    //            response.IsSuccess = true;
    //            return response;
    //        }
    //        catch (Exception ex)
    //        {
    //            LoggingService.LogError(new LogErrorRequest { ex = ex });
    //            return new GetShopifyGiftCardResponse { ErrorMessage = ex.Message };
    //        }
    //    }

    //    /// <summary>
    //    ///     Gets all of the shopify gift cards for a certain date range.
    //    /// </summary>
    //    /// <param name="request">GetShopifyRecordsRequest</param>
    //    /// <returns>GetShopifyGiftCardsResponse</returns>
    //    public static GetShopifyGiftCardsResponse GetShopifyGiftCards(GetShopifyRecordsRequest request)
    //    {
    //        try
    //        {
    //            var response = new GetShopifyGiftCardsResponse();
    //            var baseService = new ShopifyBaseService(request.CompanySiteId);
    //            request.ResourceType = ShopifyResourceType.gift_cards;
    //            var baseResponse = baseService.GetShopifyRecords(request);
    //            if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
    //            response.GiftCards = ShopifyMapper.MapToList<ShopifyGiftCardModel>(baseResponse.Data, request.ResourceType);
    //            response.IsSuccess = true;
    //            return response;
    //        }
    //        catch (Exception ex)
    //        {
    //            LoggingService.LogError(new LogErrorRequest { ex = ex });
    //            return new GetShopifyGiftCardsResponse { ErrorMessage = ex.Message };
    //        }
    //    }

    //    /// <summary>
    //    ///     Gets all of the shopify gift cards for a certain date range for a certain page.
    //    /// </summary>
    //    /// <param name="request">GetShopifyRecordsPerPageRequest</param>
    //    /// <returns>GetShopifyGiftCardsResponse</returns>
    //    public static GetShopifyGiftCardsResponse GetShopifyGiftCardsPerPage(GetShopifyRecordsPerPageRequest request)
    //    {
    //        try
    //        {
    //            var response = new GetShopifyGiftCardsResponse();
    //            var baseService = new ShopifyBaseService(request.CompanySiteId);
    //            request.ResourceType = ShopifyResourceType.gift_cards;
    //            var baseResponse = baseService.GetShopifyRecordsPerPage(request);
    //            if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
    //            response.GiftCards = ShopifyMapper.MapToList<ShopifyGiftCardModel>(baseResponse.Data, request.ResourceType);
    //            response.IsSuccess = true;
    //            return response;
    //        }
    //        catch (Exception ex)
    //        {
    //            LoggingService.LogError(new LogErrorRequest { ex = ex });
    //            return new GetShopifyGiftCardsResponse { ErrorMessage = ex.Message };
    //        }
    //    }
    //}
}
