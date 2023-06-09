using FlavorFi.Common.Enums;
using FlavorFi.Common.Models.ShopifyModels;
using FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace FlavorFi.Services.ShopifyServices
{
    public interface IShopifyProductService
    {
        GetShopifyProductResponse GetShopifyProduct(GetShopifyRecordRequest request);
        GetShopifyProductResponse GetShopifyProductByQuery(GetShopifyRecordByQueryRequest request);
        GetShopifyProductsResponse GetShopifyProducts(GetShopifyRecordsRequest request);
        GetShopifyProductsResponse GetShopifyProductsPerPage(GetShopifyRecordsPerPageRequest request);
        GetShopifyProductMetafiledsResponse GetShopifyProductMetafields(GetShopifyRecordRequest request);
        //SaveShopifyProductResponse SaveShopifyProduct(SaveShopifyProductRequest request);
        //SaveShopifyProductVariantResponse SaveShopifyProductVariant(SaveShopifyProductVariantRequest request);
        //SaveShopifyProductMetafieldResponse SaveShopifyProductMetafield(SaveShopifyProductMetafieldRequest request);
    }

    public class ShopifyProductService : ShopifyBaseService, IShopifyProductService
    {
        private const ShopifyResourceType _shopifyResourceType = ShopifyResourceType.products;

        public ShopifyProductService() : base(_shopifyResourceType) { }

        /// <summary>
        ///     Gets a shopify product for a specific product id.
        /// </summary>
        /// <param name="request">GetShopifyRecordRequest</param>
        /// <returns>GetShopifyProductResponse</returns>
        public GetShopifyProductResponse GetShopifyProduct(GetShopifyRecordRequest request)
        {
            try
            {
                var response = new GetShopifyProductResponse();
                var baseResponse = GetShopifyRecord(request);
                if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);

                response.Product = ShopifyMapperService.Map<ShopifyProductModel>(baseResponse.Data);
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new Common.RequestAndResponses.DatabaseRequestAndResponses.LogErrorRequest { ex = ex });
                return new GetShopifyProductResponse { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        ///     Gets a shopify product for specific values.
        /// </summary>
        /// <param name="request">GetShopifyRecordByQueryRequest</param>
        /// <returns>GetShopifyProductResponse</returns>
        public GetShopifyProductResponse GetShopifyProductByQuery(GetShopifyRecordByQueryRequest request)
        {
            try
            {
                var response = new GetShopifyProductResponse { Product = null };
                var baseResponse = GetShopifyRecordByQuery(request);
                if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
                var products = ShopifyMapperService.MapToList<ShopifyProductModel>(baseResponse.Data, _shopifyResourceType);

                var titleSearch = request.Parameters.FirstOrDefault(s => s.Key.Equals("title", StringComparison.CurrentCultureIgnoreCase));

                var product = products.FirstOrDefault(p => p.Title.Equals(titleSearch.Value, StringComparison.CurrentCultureIgnoreCase));
                response.Product = product;
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new Common.RequestAndResponses.DatabaseRequestAndResponses.LogErrorRequest { ex = ex });
                return new GetShopifyProductResponse { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        ///     Gets all of the shopify products for a certain date range.
        /// </summary>
        /// <param name="request">GetShopifyRecordsRequest</param>
        /// <returns>GetShopifyProductsResponse</returns>
        public GetShopifyProductsResponse GetShopifyProducts(GetShopifyRecordsRequest request)
        {
            try
            {
                var response = new GetShopifyProductsResponse();
                var baseResponse = GetShopifyRecords(request);
                if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);

                response.Products = ShopifyMapperService.MapToList<ShopifyProductModel>(baseResponse.Data, _shopifyResourceType);
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new Common.RequestAndResponses.DatabaseRequestAndResponses.LogErrorRequest { ex = ex });
                return new GetShopifyProductsResponse { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        ///     Gets all of the shopify products for a certain date range for a certain page.
        /// </summary>
        /// <param name="request">GetShopifyRecordsPerPageRequest</param>
        /// <returns>GetShopifyProductsResponse</returns>
        public GetShopifyProductsResponse GetShopifyProductsPerPage(GetShopifyRecordsPerPageRequest request)
        {
            try
            {
                var response = new GetShopifyProductsResponse();
                var baseResponse = GetShopifyRecordsPerPage(request);
                if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);

                response.Products = ShopifyMapperService.MapToList<ShopifyProductModel>(baseResponse.Data, _shopifyResourceType);
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new Common.RequestAndResponses.DatabaseRequestAndResponses.LogErrorRequest { ex = ex });
                return new GetShopifyProductsResponse { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        ///     Gets a shopify product for a specific product id.
        /// </summary>
        /// <param name="request">GetShopifyRecordRequest</param>
        /// <returns>GetShopifyProductResponse</returns>
        public GetShopifyProductMetafiledsResponse GetShopifyProductMetafields(GetShopifyRecordRequest request)
        {
            try
            {
                var response = new GetShopifyProductMetafiledsResponse();
                var baseResponse = GetShopifyMetafieldRecords(request);
                if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);

                response.Metafields = ShopifyMapperService.MapToList<ShopifyMetafieldModel>(baseResponse.Data, ShopifyResourceType.metafields);
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new Common.RequestAndResponses.DatabaseRequestAndResponses.LogErrorRequest { ex = ex });
                return new GetShopifyProductMetafiledsResponse { ErrorMessage = ex.Message };
            }
        }

        ///// <summary>
        /////     Creates or updates a shopify product.
        ///// </summary>
        ///// <param name="request">SaveShopifyProductRequest</param>
        ///// <returns>SaveShopifyRecordResponse</returns>
        //public SaveShopifyProductResponse SaveShopifyProduct(SaveShopifyProductRequest request)
        //{
        //    try
        //    {
        //        var response = new SaveShopifyProductResponse();
        //        var baseRequest = new SaveShopifyRecordRequest { BaseSite = request.BaseSite};
        //        var jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        //        var jobject = JObject.Parse(JsonConvert.SerializeObject(request, Formatting.None, jsonSettings));
        //        if (jobject["product"]["images"].ToString() == "[]")
        //            jobject["product"]["images"].Parent.Remove();

        //        baseRequest.PostData = jobject.ToString();
        //        if (request.Product.Id == 0)
        //        {   
        //            var baseResponse = AddShopifyRecord(baseRequest);
        //            if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
        //            response.Product = ShopifyMapperService.Map<ShopifyProductModel>(baseResponse.Data["product"]);
        //        }
        //        else
        //        {
        //            baseRequest.RecordId = request.Product.Id;
        //            var baseResponse = UpdateShopifyRecord(baseRequest);
        //            if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
        //            response.Product = ShopifyMapperService.Map<ShopifyProductModel>(baseResponse.Data["product"]);
        //        }

        //        response.IsSuccess = true;
        //        return response;
        //    }
        //    catch (WebException wex)
        //    {
        //        var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
        //        return new SaveShopifyProductResponse { ErrorMessage = resp };
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingService.LogError(new Common.RequestAndResponses.DatabaseRequestAndResponses.LogErrorRequest { ex = ex });
        //        return new SaveShopifyProductResponse { ErrorMessage = ex.Message };
        //    }
        //}

        ///// <summary>
        /////     Creates or updates a shopify product variant.
        ///// </summary>
        ///// <param name="request">SaveShopifyProductVariantRequest</param>
        ///// <returns>SaveShopifyProductVariantResponse</returns>
        //public SaveShopifyProductVariantResponse SaveShopifyProductVariant(SaveShopifyProductVariantRequest request)
        //{
        //    try
        //    {
        //        var response = new SaveShopifyProductVariantResponse();
        //        var baseRequest = new SaveShopifyRecordRequest();

        //        if (request.ProductVariant.Id == 0)
        //        {
        //            baseRequest.ResourceType = ShopifyResourceType.products;
        //            baseRequest.PostData = JObject.Parse(JsonConvert.SerializeObject(request)).ToString();
        //            baseRequest.ExtendedUrl = "/" + request.ProductId + "/variants";
        //            var baseResponse = AddShopifyRecord(baseRequest);
        //            if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
        //            response.ProductVariant = ShopifyMapperService.Map<ShopifyProductVariantModel>(baseResponse.Data["variant"]);
        //        }
        //        else
        //        {
        //            baseRequest.ResourceType = ShopifyResourceType.variants;
        //            baseRequest.PostData = JObject.Parse(JsonConvert.SerializeObject(request)).ToString();
        //            baseRequest.RecordId = request.ProductVariant.Id;
        //            var baseResponse = UpdateShopifyRecord(baseRequest);
        //            if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
        //            response.ProductVariant = ShopifyMapperService.Map<ShopifyProductVariantModel>(baseResponse.Data["variant"]);
        //        }

        //        response.IsSuccess = true;
        //        return response;
        //    }
        //    catch (WebException wex)
        //    {
        //        var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
        //        return new SaveShopifyProductVariantResponse { ErrorMessage = resp };
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingService.LogError(new Common.RequestAndResponses.DatabaseRequestAndResponses.LogErrorRequest { ex = ex });
        //        return new SaveShopifyProductVariantResponse { ErrorMessage = ex.Message };
        //    }
        //}

        ///// <summary>
        /////     Creates or updates a shopify product metafield.
        ///// </summary>
        ///// <param name="request">SaveShopifyProductMetafieldRequest</param>
        ///// <returns>SaveShopifyProductMetafieldResponse</returns>
        //public SaveShopifyProductMetafieldResponse SaveShopifyProductMetafield(SaveShopifyProductMetafieldRequest request)
        //{
        //    try
        //    {
        //        var response = new SaveShopifyProductMetafieldResponse();
        //        var baseRequest = new SaveShopifyRecordRequest();

        //        if (request.ProductMetafield.Id == 0)
        //        {
        //            baseRequest.ResourceType = ShopifyResourceType.products;
        //            baseRequest.PostData = JObject.Parse(JsonConvert.SerializeObject(request)).ToString();
        //            baseRequest.ExtendedUrl = "/" + request.ProductId + "/metafields";
        //            var baseResponse = AddShopifyRecord(baseRequest);
        //            if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
        //            response.ProductMetafield = ShopifyMapperService.Map<ShopifyMetafieldModel>(baseResponse.Data["metafield"]);
        //        }
        //        else
        //        {
        //            baseRequest.ResourceType = ShopifyResourceType.variants;
        //            baseRequest.PostData = JObject.Parse(JsonConvert.SerializeObject(request)).ToString();
        //            baseRequest.RecordId = request.ProductMetafield.Id;
        //            var baseResponse = UpdateShopifyRecord(baseRequest);
        //            if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
        //            response.ProductMetafield = ShopifyMapperService.Map<ShopifyMetafieldModel>(baseResponse.Data["metafield"]);
        //        }

        //        response.IsSuccess = true;
        //        return response;
        //    }
        //    catch (WebException wex)
        //    {
        //        var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
        //        return new SaveShopifyProductMetafieldResponse { ErrorMessage = resp };
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingService.LogError(new Common.RequestAndResponses.DatabaseRequestAndResponses.LogErrorRequest { ex = ex });
        //        return new SaveShopifyProductMetafieldResponse { ErrorMessage = ex.Message };
        //    }
        //}
    }
}