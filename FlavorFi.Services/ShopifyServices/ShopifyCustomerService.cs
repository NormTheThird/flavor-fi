using FlavorFi.Common.Enums;
using FlavorFi.Common.Models.ShopifyModels;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;

namespace FlavorFi.Services.ShopifyServices
{
    public interface IShopifyCustomerService
    {
        GetShopifyCustomerResponse GetShopifyCustomer(GetShopifyRecordRequest request);
        GetShopifyCustomerResponse GetShopifyCustomerByQuery(GetShopifyRecordByQueryRequest request);
        GetShopifyCustomersResponse GetShopifyCustomers(GetShopifyRecordsRequest request);
        GetShopifyCustomersResponse GetShopifyCustomersPerPage(GetShopifyRecordsPerPageRequest request);
        GetShopifyCustomerMetafiledsResponse GetShopifyCustomerMetafields(GetShopifyRecordRequest request);
        //SaveShopifyCustomerResponse SaveShopifyCustomer(SaveShopifyCustomerRequest request);
        //SaveShopifyCustomerMetafieldResponse SaveShopifyCustomerMetafield(SaveShopifyCustomerMetafieldRequest request);
    }

    public class ShopifyCustomerService : ShopifyBaseService, IShopifyCustomerService
    {
        private const ShopifyResourceType _shopifyResourceType = ShopifyResourceType.customers;

        public ShopifyCustomerService() : base(_shopifyResourceType) { }

        /// <summary>
        ///     Gets a shopify customer for a specific customer id.
        /// </summary>
        /// <param name="request">GetShopifyRecordRequest</param>
        /// <returns>GetShopifyCustomerResponse</returns>
        public GetShopifyCustomerResponse GetShopifyCustomer(GetShopifyRecordRequest request)
        {
            try
            {
                var response = new GetShopifyCustomerResponse();
                var baseResponse = GetShopifyRecord(request);
                if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);

                response.Customer = ShopifyMapperService.Map<ShopifyCustomerModel>(baseResponse.Data);
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetShopifyCustomerResponse { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        ///     Gets a shopify customer for specific values.
        /// </summary>
        /// <param name="request">GetShopifyRecordByQueryRequest</param>
        /// <returns>GetShopifyCustomerResponse</returns>
        public GetShopifyCustomerResponse GetShopifyCustomerByQuery(GetShopifyRecordByQueryRequest request)
        {
            try
            {
                var response = new GetShopifyCustomerResponse { Customer = null };
                var baseResponse = GetShopifyRecordByQuery(request);
                if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
                var customers = ShopifyMapperService.MapToList<ShopifyCustomerModel>(baseResponse.Data, _shopifyResourceType);

                response.Customer = customers[0];
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetShopifyCustomerResponse { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        ///     Gets all of the shopify customers for a certain date range.
        /// </summary>
        /// <param name="request">GetShopifyRecordsRequest</param>
        /// <returns>GetShopifyCustomersResponse</returns>
        public GetShopifyCustomersResponse GetShopifyCustomers(GetShopifyRecordsRequest request)
        {
            try
            {
                var response = new GetShopifyCustomersResponse();
                var baseResponse = GetShopifyRecords(request);
                if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);

                response.Customers = ShopifyMapperService.MapToList<ShopifyCustomerModel>(baseResponse.Data, _shopifyResourceType);
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetShopifyCustomersResponse { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        ///     Gets all of the shopify customers for a certain date range for a certain page.
        /// </summary>
        /// <param name="request">GetShopifyRecordsPerPageRequest</param>
        /// <returns>GetShopifyCustomersResponse</returns>
        public GetShopifyCustomersResponse GetShopifyCustomersPerPage(GetShopifyRecordsPerPageRequest request)
        {
            try
            {
                var response = new GetShopifyCustomersResponse();
                var baseResponse = GetShopifyRecordsPerPage(request);
                if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);

                response.Customers = ShopifyMapperService.MapToList<ShopifyCustomerModel>(baseResponse.Data, _shopifyResourceType);
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetShopifyCustomersResponse { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        ///     Gets a shopify metafields specific customer id.
        /// </summary>
        /// <param name="request">GetShopifyRecordRequest</param>
        /// <returns>GetShopifyCustomerMetafiledsResponse</returns>
        public GetShopifyCustomerMetafiledsResponse GetShopifyCustomerMetafields(GetShopifyRecordRequest request)
        {
            try
            {
                var response = new GetShopifyCustomerMetafiledsResponse();
                var baseResponse = GetShopifyMetafieldRecords(request);
                if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);

                response.Metafields = ShopifyMapperService.MapToList<ShopifyMetafieldModel>(baseResponse.Data, ShopifyResourceType.metafields);
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetShopifyCustomerMetafiledsResponse { ErrorMessage = ex.Message };
            }
        }

        ///// <summary>
        /////     Creates or updates a shopify customer.
        ///// </summary>
        ///// <param name="request">SaveShopifyCustomerRequest</param>
        ///// <returns>SaveShopifyCustomerResponse</returns>
        //public SaveShopifyCustomerResponse SaveShopifyCustomer(SaveShopifyCustomerRequest request)
        //{
        //    try
        //    {
        //        var response = new SaveShopifyCustomerResponse();
        //        var baseRequest = new SaveShopifyRecordRequest { BaseSite = request.BaseSite, ResourceType = ShopifyResourceType.customers };
        //        var jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        //        var jobject = JObject.Parse(JsonConvert.SerializeObject(request, Formatting.None, jsonSettings));

        //        baseRequest.PostData = jobject.ToString();
        //        if (request.Customer.Id == 0)
        //        {
        //            var baseResponse = AddShopifyRecord(baseRequest);
        //            if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
        //            response.Customer = ShopifyMapperService.Map<ShopifyCustomerModel>(baseResponse.Data["customer"]);
        //        }
        //        else
        //        { 
        //            baseRequest.RecordId = request.Customer.Id;
        //            var baseResponse = UpdateShopifyRecord(baseRequest);
        //            if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
        //            response.Customer = ShopifyMapperService.Map<ShopifyCustomerModel>(baseResponse.Data["customer"]);
        //        }

        //        response.IsSuccess = true;
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingService.LogError(new LogErrorRequest { ex = ex });
        //        return new SaveShopifyCustomerResponse { ErrorMessage = ex.Message };
        //    }
        //}

        ///// <summary>
        /////     Creates or updates a shopify customer metafield.
        ///// </summary>
        ///// <param name="request">SaveShopifyCustomerMetafieldRequest</param>
        ///// <returns>SaveShopifyCustomerMetafieldResponse</returns>
        //public SaveShopifyCustomerMetafieldResponse SaveShopifyCustomerMetafield(SaveShopifyCustomerMetafieldRequest request)
        //{
        //    try
        //    {
        //        var response = new SaveShopifyCustomerMetafieldResponse();
        //        var baseRequest = new SaveShopifyRecordRequest();
        //        baseRequest.ResourceType = ShopifyResourceType.customers;
        //        baseRequest.PostData = JObject.Parse(JsonConvert.SerializeObject(request)).ToString();

        //        if (request.CustomerMetafield.Id == 0)
        //        {
        //            baseRequest.ExtendedUrl = "/" + request.CustomerId + "/metafields";
        //            var baseResponse = AddShopifyRecord(baseRequest);
        //            if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
        //            response.CustomerMetafield = ShopifyMapperService.Map<ShopifyMetafieldModel>(baseResponse.Data["metafield"]);
        //        }
        //        else
        //        {
        //            baseRequest.RecordId = request.CustomerMetafield.Id;
        //            var baseResponse = UpdateShopifyRecord(baseRequest);
        //            if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
        //            response.CustomerMetafield = ShopifyMapperService.Map<ShopifyMetafieldModel>(baseResponse.Data["metafield"]);
        //        }

        //        response.IsSuccess = true;
        //        return response;
        //    }
        //    catch (WebException wex)
        //    {
        //        var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
        //        return new SaveShopifyCustomerMetafieldResponse { ErrorMessage = resp };
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingService.LogError(new LogErrorRequest { ex = ex });
        //        return new SaveShopifyCustomerMetafieldResponse { ErrorMessage = ex.Message };
        //    }
        //}
    }
}