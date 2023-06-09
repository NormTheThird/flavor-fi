using FlavorFi.Common.Enums;
using FlavorFi.Common.Helpers;
using FlavorFi.Common.Models.DatabaseModels;
using FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using FlavorFi.Services.DatabaseServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace FlavorFi.Services.ShopifyServices
{
    public interface IShopifyBaseService
    {
        GetShopifyRecordsResponse GetShopifyRecords(GetShopifyRecordsRequest request);
        GetShopifyRecordsResponse GetShopifyRecordsPerPage(GetShopifyRecordsPerPageRequest request);
        GetShopifyRecordResponse GetShopifyRecord(GetShopifyRecordRequest request);
        GetShopifyRecordResponse GetShopifyRecordByQuery(GetShopifyRecordByQueryRequest request);
        GetShopifyRecordsResponse GetShopifyMetafieldRecords(GetShopifyRecordRequest request);
        GetShopifyRecordCountResponse GetShopifyRecordCount(GetShopifyRecordsRequest request);
        SaveShopifyRecordResponse AddShopifyRecord(SaveShopifyRecordRequest request);
        SaveShopifyRecordResponse UpdateShopifyRecord(SaveShopifyRecordRequest request);
        ShopifyBaseResponse DeleteShopifyRecord(DeleteShopifyRecordRequest request);
    }

    public class ShopifyBaseService : IShopifyBaseService
    {
        public ILoggingService LoggingService { get; private set; }
        public IShopifyMapperService ShopifyMapperService { get; private set; }
        public ShopifyResourceType ShopifyResourceType { get; private set; }

        public ShopifyBaseService(ShopifyResourceType shopifyResourceType)
        {
            this.LoggingService = new LoggingService();
            this.ShopifyMapperService = new ShopifyMapperService();
            this.ShopifyResourceType = shopifyResourceType;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        /// <summary>
        ///     Gets all the records for a specified shopify resource and a specified date range.
        /// </summary>
        /// <param name="request">GetShopifyRecordsRequest</param>
        /// <returns>GetShopifyRecordsResponse</returns>
        public GetShopifyRecordsResponse GetShopifyRecords(GetShopifyRecordsRequest request)
        {
            try
            {
                var response = new GetShopifyRecordsResponse();
                var recordCount = this.GetShopifyRecordCount(request).Count;
                if (recordCount == 0) return new GetShopifyRecordsResponse { IsSuccess = true, Data = new JObject() };

                var jsonObject = new JObject();
                var pageCount = recordCount / 50;
                if (recordCount % 50 != 0) pageCount++;
                if (request.Parameters == null)
                    request.Parameters = new Dictionary<string, string>();
                request.Parameters.Add("limit", "50");
                for (int i = 1; i <= pageCount; i++)
                {
                    //if(!request.Parameters.ContainsKey("page"))
                    //    request.Parameters.Add("page", i.ToString());
                    //request.Parameters["page"] = i.ToString();
                    string url = $"{this.CreateUrl(request.BaseSite.BaseUrl, ShopifyResourceType)}.json{BuildParameterString(request.Parameters)}";
                    var webRequest = WebRequest.Create(url);
                    this.SetBasicAuthHeader(webRequest, request.BaseSite.PublicApiKey, request.BaseSite.SecretApiKey, "GET");
                    using (var dataResponse = webRequest.GetResponse())
                    {
                        using (var reader = new StreamReader(dataResponse.GetResponseStream()))
                        {
                            var objText = reader.ReadToEnd();
                            jsonObject.Merge(JObject.Parse(objText), new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Union });
                            this.AdjustForNextCall(dataResponse);
                        }
                    }
                }

                response.Data = jsonObject;
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Gets all the records for a specified shopify resource and a specified date range and a specified page.
        /// </summary>
        /// <param name="request">GetShopifyRecordsPerPageRequest</param>
        /// <returns>GetShopifyRecordsResponse</returns>
        public GetShopifyRecordsResponse GetShopifyRecordsPerPage(GetShopifyRecordsPerPageRequest request)
        {
            try
            {
                var response = new GetShopifyRecordsResponse();
                var jsonObject = new JObject();

                string url = $"{this.CreateUrl(request.BaseSite.BaseUrl, ShopifyResourceType)}.json{BuildParameterString(request.Parameters)}";
                var webRequest = WebRequest.Create(url);
                this.SetBasicAuthHeader(webRequest, request.BaseSite.PublicApiKey, request.BaseSite.SecretApiKey, "GET");
                using (var dataResponse = webRequest.GetResponse())
                {
                    using (var reader = new StreamReader(dataResponse.GetResponseStream()))
                    {
                        var objText = reader.ReadToEnd();
                        jsonObject.Merge(JObject.Parse(objText), new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Union });
                        response.Data = jsonObject;
                        response.IsSuccess = true;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Gets a specific record from shopify.
        /// </summary>
        /// <param name="request">GetShopifyRecordRequest</param>
        /// <returns>GetShopifyRecordResponse</returns>
        public GetShopifyRecordResponse GetShopifyRecord(GetShopifyRecordRequest request)
        {
            try
            {
                var response = new GetShopifyRecordResponse();
                string url = $"{this.CreateUrl(request.BaseSite.BaseUrl, ShopifyResourceType)}/{request.RecordId}.json{BuildParameterString(request.Parameters)}";
                var webRequest = WebRequest.Create(url);
                this.SetBasicAuthHeader(webRequest, request.BaseSite.PublicApiKey, request.BaseSite.SecretApiKey, "GET");
                using (var dataResponse = webRequest.GetResponse())
                {
                    using (var reader = new StreamReader(dataResponse.GetResponseStream()))
                    {
                        this.AdjustForNextCall(dataResponse);
                        response.Data = JObject.Parse(reader.ReadToEnd());
                        response.IsSuccess = true;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Gets a specific record from shopify with certain values.
        /// </summary>
        /// <param name="request">GetShopifyRecordByQueryRequest</param>
        /// <returns>GetShopifyRecordResponse</returns>
        public GetShopifyRecordResponse GetShopifyRecordByQuery(GetShopifyRecordByQueryRequest request)
        {
            try
            {
                var response = new GetShopifyRecordResponse();
                string url = $"{this.CreateUrl(request.BaseSite.BaseUrl, ShopifyResourceType)}.json{BuildParameterString(request.Parameters)}";
                var webRequest = WebRequest.Create(url);
                this.SetBasicAuthHeader(webRequest, request.BaseSite.PublicApiKey, request.BaseSite.SecretApiKey, "GET");
                using (var dataResponse = webRequest.GetResponse())
                {
                    using (var reader = new StreamReader(dataResponse.GetResponseStream()))
                    {
                        this.AdjustForNextCall(dataResponse);
                        response.Data = JObject.Parse(reader.ReadToEnd());
                        response.IsSuccess = true;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Gets the metafields for a specific record from shopify.
        /// </summary>
        /// <param name="request">GetShopifyRecordRequest</param>
        /// <returns>GetShopifyRecordsResponse</returns>
        public GetShopifyRecordsResponse GetShopifyMetafieldRecords(GetShopifyRecordRequest request)
        {
            try
            {
                var response = new GetShopifyRecordsResponse();
                string url = $"{this.CreateUrl(request.BaseSite.BaseUrl, ShopifyResourceType)}/{request.RecordId}/metafields.json{BuildParameterString(request.Parameters)}";
                var webRequest = WebRequest.Create(url);
                this.SetBasicAuthHeader(webRequest, request.BaseSite.PublicApiKey, request.BaseSite.SecretApiKey, "GET");
                using (var dataResponse = webRequest.GetResponse())
                {
                    using (var reader = new StreamReader(dataResponse.GetResponseStream()))
                    {
                        this.AdjustForNextCall(dataResponse);
                        response.Data = JObject.Parse(reader.ReadToEnd());
                        response.IsSuccess = true;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Gets the number of records in the specified shopify resource.
        /// </summary>
        /// <param name="request">ShopifyBaseCountRequest</param>
        /// <returns>ShopifyBaseCountResponse</returns>
        public GetShopifyRecordCountResponse GetShopifyRecordCount(GetShopifyRecordsRequest request)
        {
            try
            {
                var response = new GetShopifyRecordCountResponse();
                string url = $"{this.CreateUrl(request.BaseSite.BaseUrl, ShopifyResourceType)}/count.json{BuildParameterString(request.Parameters)}";
                var webRequest = WebRequest.Create(url);
                this.SetBasicAuthHeader(webRequest, request.BaseSite.PublicApiKey, request.BaseSite.SecretApiKey, "GET");
                using (var countResponse = webRequest.GetResponse())
                {
                    using (var reader = new StreamReader(countResponse.GetResponseStream()))
                    {
                        this.AdjustForNextCall(countResponse);
                        response.Count = Convert.ToInt32(JToken.Parse(reader.ReadToEnd())["count"]);
                        response.IsSuccess = true;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Adds a new shopify record.
        /// </summary>
        /// <param name="request">SaveShopifyRecordRequest</param>
        /// <returns>SaveShopifyRecordResponse</returns>
        public SaveShopifyRecordResponse AddShopifyRecord(SaveShopifyRecordRequest request)
        {
            try
            {
                var response = new SaveShopifyRecordResponse();
                byte[] byteArray = Encoding.UTF8.GetBytes(request.PostData);

                var extendedUrl = string.IsNullOrEmpty(request.ExtendedUrl) ? "" : request.ExtendedUrl.Trim();
                string url = $"{this.CreateUrl(request.BaseSite.BaseUrl, ShopifyResourceType)}{extendedUrl}.json";
                var webRequest = WebRequest.Create(url);
                webRequest.ContentLength = byteArray.Length;
                this.SetBasicAuthHeader(webRequest, request.BaseSite.PublicApiKey, request.BaseSite.SecretApiKey, "POST");
                using (var dataStream = webRequest.GetRequestStream())
                    dataStream.Write(byteArray, 0, byteArray.Length);
                using (var addResponse = webRequest.GetResponse())
                {
                    using (var reader = new StreamReader(addResponse.GetResponseStream()))
                    {
                        this.AdjustForNextCall(addResponse);
                        response.Data = JObject.Parse(reader.ReadToEnd());
                        response.IsSuccess = true;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(request.PostData);
                throw ex;
            }
        }

        /// <summary>
        ///     Updates a current shopify record.
        /// </summary>
        /// <param name="request">SaveShopifyRecordRequest</param>
        /// <returns>SaveShopifyRecordResponse</returns>
        public SaveShopifyRecordResponse UpdateShopifyRecord(SaveShopifyRecordRequest request)
        {
            try
            {
                var response = new SaveShopifyRecordResponse();
                byte[] byteArray = Encoding.UTF8.GetBytes(request.PostData);

                string url = $"{this.CreateUrl(request.BaseSite.BaseUrl, ShopifyResourceType)}/{request.RecordId}.json";
                var webRequest = WebRequest.Create(url);
                webRequest.ContentLength = byteArray.Length;
                this.SetBasicAuthHeader(webRequest, request.BaseSite.PublicApiKey, request.BaseSite.SecretApiKey, "PUT");
                using (var dataStream = webRequest.GetRequestStream())
                    dataStream.Write(byteArray, 0, byteArray.Length);
                using (var updateResponse = webRequest.GetResponse())
                {
                    using (var reader = new StreamReader(updateResponse.GetResponseStream()))
                    {
                        this.AdjustForNextCall(updateResponse);
                        response.Data = JObject.Parse(reader.ReadToEnd());
                        response.IsSuccess = true;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Deletes a specific record base on an id.
        /// </summary>
        /// <param name="request">DeleteShopifyRecordRequest</param>
        /// <returns>ShopifyBaseResponse</returns>
        public ShopifyBaseResponse DeleteShopifyRecord(DeleteShopifyRecordRequest request)
        {
            try
            {
                var response = new ShopifyBaseResponse();
                string url = $"{this.CreateUrl(request.BaseSite.BaseUrl, ShopifyResourceType)}/{request.RecordId}.json";
                var webRequest = WebRequest.Create(url);
                this.SetBasicAuthHeader(webRequest, request.BaseSite.PublicApiKey, request.BaseSite.SecretApiKey, "DELETE");
                using (var deleteResponse = webRequest.GetResponse())
                {
                    using (var reader = new StreamReader(deleteResponse.GetResponseStream()))
                    {
                        this.AdjustForNextCall(deleteResponse);
                        response.IsSuccess = true;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Sets the authorization for the web request
        /// </summary>
        /// <param name="request">The request to add the auth information to</param>
        /// <param name="BaseSite.PublicApiKey">The company site model for the shopify api keys.</param>
        /// <param name="BaseSite.SecretApiKey">The company site model for the shopify api keys.</param>
        /// <param name="method">The webrequest method</param>
        private void SetBasicAuthHeader(WebRequest request, string publicApiKey, string secretApiKey, string method)
        {
            var authInfo = $"{publicApiKey}:{secretApiKey}";
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = $"Basic {authInfo}";
            request.ContentType = "application/json";
            request.Method = method;
        }

        /// <summary>
        ///     Slows down the api call to avoid the 429 error.
        /// </summary>
        /// <param name="response">The web response from the shopify call.</param>
        private void AdjustForNextCall(WebResponse response)
        {
            try
            {
                // Console.WriteLine(response.Headers["X-Shopify-Shop-Api-Call-Limit"]);
                var rates = response.Headers["X-Shopify-Shop-Api-Call-Limit"].Split('/');
                var current = int.Parse(rates[0]);
                var max = int.Parse(rates[1]);
                if (current + 10 > max) Thread.Sleep(100);
                if (current + 8 > max) Thread.Sleep(100);
                if (current + 5 > max) Thread.Sleep(100);
                if (current + 2 > max) Thread.Sleep(100);
            }
            catch (Exception)
            {
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        ///     Gets the url from the company;
        /// </summary>
        /// <param name="BaseSite.BaseUrl">The company site model for the shopify url.</param>
        /// <param name="resourceType">The resource type to be added to the url</param>
        /// <returns></returns>
        private string CreateUrl(string baseUrl, ShopifyResourceType resourceType)
        {
            try
            {
                var apiVersion = ConfigurationManager.AppSettings["ShopifyApiVersion"];
                return $"{baseUrl}/admin/api/{apiVersion}/{resourceType}";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Gets a date string to pass into the url shopify
        /// </summary>
        /// <param name="startDate">The updated at min date.</param>
        /// <param name="endDate">The updated at max date.</param>
        /// <returns></returns>
        private string GetDateString(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                if (startDate == null) return "";
                var formattedStartDate = "updated_at_min=" + ((DateTime)startDate).Year.ToString() + "-" + ((DateTime)startDate).Month.ToString() + "-" + ((DateTime)startDate).Day.ToString();
                if (endDate == null) endDate = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var formattedEndDate = "updated_at_max=" + ((DateTime)endDate).Year.ToString() + "-" + ((DateTime)endDate).Month.ToString() + "-" + ((DateTime)endDate).Day.ToString();
                return formattedStartDate + "&" + formattedEndDate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string BuildParameterString(Dictionary<string, string> paramerters)
        {
            if (paramerters == null) return "";
            var retval = "?";
            for (int i = 0; i < paramerters.Count; i++)
            {
                var item = paramerters.ElementAt(i);
                retval += $"{item.Key}={item.Value}";
                if (i < paramerters.Count - 1)
                    retval += "&";
            }
            return retval;
        }
    }
}