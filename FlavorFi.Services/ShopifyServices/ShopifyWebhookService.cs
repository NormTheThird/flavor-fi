using FlavorFi.Common.Models.ShopifyModels;
using FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using System;
using System.Collections.Generic;
using FlavorFi.Common.Enums;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;

namespace FlavorFi.Services.ShopifyServices
{
    public interface IShopifyWebhookService
    {
        GetShopifyWebhookResponse GetShopifyWebhook(GetShopifyRecordRequest request);
        GetShopifyWebhooksResponse GetShopifyWebhooks(GetShopifyRecordsRequest request);
        GetShopifyWebhooksResponse GetShopifyWebhooksPerPage(GetShopifyRecordsPerPageRequest request);
        ActivateShopifyWebhookResponse ActivateShopifyWebhook(ActivateShopifyWebhookRequest request);
        DeactivateShopifyWebhookResponse DeactivateShopifyWebhook(DeactivateShopifyWebhookRequest request);
        bool Validate(HttpRequestMessage request, string sharedSecret);
        List<ShopifyWebhookModel> GetShopifyWebhookList(string baseWebhookUrl);
    }

    public class ShopifyWebhookService : ShopifyBaseService, IShopifyWebhookService
    {
        // From shopify: The /admin/webhooks.json endpoint only returns the webhooks that you have registered 
        // with that API key. There is no way to retrieve admin-created webhooks using the API at this time.

        private const ShopifyResourceType _shopifyResourceType = ShopifyResourceType.webhooks;

        public ShopifyWebhookService() : base(_shopifyResourceType) { }

        /// <summary>
        ///     Gets a shopify webhook for a specific webhook id.
        /// </summary>
        /// <param name="request">GetShopifyRecordRequest</param>
        /// <returns>GetShopifyWebhookResponse</returns>
        public GetShopifyWebhookResponse GetShopifyWebhook(GetShopifyRecordRequest request)
        {
            try
            {
                var response = new GetShopifyWebhookResponse();
                var baseResponse = GetShopifyRecord(request);
                if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
                response.Webhook = ShopifyMapperService.Map<ShopifyWebhookModel>(baseResponse.Data);
                response.IsSuccess = true;
                return response;
            }
            catch (WebException wex)
            {
                var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                return new GetShopifyWebhookResponse { ErrorMessage = resp };
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new Common.RequestAndResponses.DatabaseRequestAndResponses.LogErrorRequest { ex = ex });
                return new GetShopifyWebhookResponse { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        ///     Gets all of the shopify webhooks for a certain date range.
        /// </summary>
        /// <param name="request">GetShopifyRecordsRequest</param>
        /// <returns>GetShopifyOrdersResponse</returns>
        public GetShopifyWebhooksResponse GetShopifyWebhooks(GetShopifyRecordsRequest request)
        {
            try
            {
                var response = new GetShopifyWebhooksResponse();
                var baseResponse = GetShopifyRecords(request);
                if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
                var webooks = GetShopifyWebhookList(request.BaseSite.BaseWebhookUrl);
                if (baseResponse.Data.Count != 0)
                {
                    var activeWebhooks = ShopifyMapperService.MapToList<ShopifyWebhookModel>(baseResponse.Data, _shopifyResourceType);
                    foreach (var webhook in activeWebhooks)
                    {
                        var existingWebhook = webooks.FirstOrDefault(w => w.Topic.Equals(webhook.Topic, StringComparison.CurrentCultureIgnoreCase));
                        if (existingWebhook == null) webooks.Add(webhook);
                        else
                        {
                            existingWebhook.Id = webhook.Id;
                            existingWebhook.Url = webhook.Url;
                            existingWebhook.Format = webhook.Format;
                            existingWebhook.IsActive = webhook.IsActive;
                            existingWebhook.CreatedAt = webhook.CreatedAt;
                            existingWebhook.UpdatedAt = webhook.UpdatedAt;
                        }
                    }
                }
                response.Webhooks = webooks.OrderByDescending(_ => _.IsActive).ThenBy(_ => _.Event).ThenBy(_ => _.Topic).ToList();
                response.IsSuccess = true;
                return response;
            }
            catch (WebException wex)
            {
                if(wex.Response == null)
                    return new GetShopifyWebhooksResponse { ErrorMessage = wex.Message };
                var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                return new GetShopifyWebhooksResponse { ErrorMessage = resp };
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new Common.RequestAndResponses.DatabaseRequestAndResponses.LogErrorRequest { ex = ex });
                return new GetShopifyWebhooksResponse { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        ///     Gets all of the shopify webhooks for a certain date range for a certain page.
        /// </summary>
        /// <param name="request">GetShopifyRecordsPerPageRequest</param>
        /// <returns>GetShopifyWebhooksResponse</returns>
        public GetShopifyWebhooksResponse GetShopifyWebhooksPerPage(GetShopifyRecordsPerPageRequest request)
        {
            try
            {
                var response = new GetShopifyWebhooksResponse();
                var baseResponse = GetShopifyRecordsPerPage(request);
                if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
                response.Webhooks = ShopifyMapperService.Map<List<ShopifyWebhookModel>>(baseResponse.Data);
                response.IsSuccess = true;
                return response;
            }
            catch (WebException wex)
            {
                var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                return new GetShopifyWebhooksResponse { ErrorMessage = resp };
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new Common.RequestAndResponses.DatabaseRequestAndResponses.LogErrorRequest { ex = ex });
                return new GetShopifyWebhooksResponse { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        ///     Activates a new webhook or updates and existing webhook.
        /// </summary>
        /// <param name="request">ActivateShopifyWebhookRequest</param>
        /// <returns>ActivateShopifyWebhookResponse</returns>
        public ActivateShopifyWebhookResponse ActivateShopifyWebhook(ActivateShopifyWebhookRequest request)
        {
            try
            {
                var response = new ActivateShopifyWebhookResponse();
                var baseRequest = new SaveShopifyRecordRequest { BaseSite = request.BaseSite };

                if (request.Webhook.Id == 0)
                {
                    baseRequest.PostData = JObject.Parse(JsonConvert.SerializeObject(request)).ToString();
                    var baseResponse = AddShopifyRecord(baseRequest);
                    if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
                    response.NewWebhookId = baseResponse.Data["webhook"]["id"].Value<long>();
                }
                else
                {
                    baseRequest.PostData = JsonConvert.SerializeObject(new { webhook = JObject.Parse(JsonConvert.SerializeObject(request)) });
                    baseRequest.RecordId = request.Webhook.Id;
                    var baseResponse = UpdateShopifyRecord(baseRequest);
                    if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
                    response.NewWebhookId = baseResponse.Data["webhook"]["id"].Value<long>();
                }

                response.IsSuccess = true;
                return response;
            }
            catch (WebException wex)
            {
                var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                return new ActivateShopifyWebhookResponse { ErrorMessage = resp };
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new Common.RequestAndResponses.DatabaseRequestAndResponses.LogErrorRequest { ex = ex });
                return new ActivateShopifyWebhookResponse { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        ///      Deactivats a specific webhook based on an id.
        /// </summary>
        /// <param name="request">DeactivateShopifyWebhookRequest</param>
        /// <returns>DeactivateShopifyWebhookResponse</returns>
        public DeactivateShopifyWebhookResponse DeactivateShopifyWebhook(DeactivateShopifyWebhookRequest request)
        {
            try
            {
                var response = new DeactivateShopifyWebhookResponse();
                var baseRequest = new DeleteShopifyRecordRequest
                {
                    BaseSite = request.BaseSite,
                    RecordId = request.WebhookId
                };
                var baseResponse = DeleteShopifyRecord(baseRequest);
                if (!baseResponse.IsSuccess) throw new ApplicationException(baseResponse.ErrorMessage);
                response.IsSuccess = true;
                return response;
            }
            catch (WebException wex)
            {
                var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                return new DeactivateShopifyWebhookResponse { ErrorMessage = resp };
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new Common.RequestAndResponses.DatabaseRequestAndResponses.LogErrorRequest { ex = ex });
                return new DeactivateShopifyWebhookResponse { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        ///     Validates the request with the shared secret to verify the webhook.
        /// </summary>
        /// <param name="_request">The request to verify.</param>
        /// <param name="_sharedSecret">The shared secret to use for verification.</param>
        /// <returns></returns>
        public bool Validate(HttpRequestMessage request, string sharedSecret)
        {
            var data = request.Content.ReadAsStringAsync().Result;
            var keyBytes = Encoding.UTF8.GetBytes(sharedSecret);
            var dataBytes = Encoding.UTF8.GetBytes(data);

            //use the SHA256Managed Class to compute the hash
            var hmac = new HMACSHA256(keyBytes);
            var hmacBytes = hmac.ComputeHash(dataBytes);

            //retun as base64 string. Compared with the signature passed in the header of the post request from Shopify. If they match, the call is verified.
            var hmacHeader = request.Headers.GetValues("x-shopify-hmac-sha256").FirstOrDefault();
            var createSignature = Convert.ToBase64String(hmacBytes);
            return hmacHeader == createSignature;
        }

        /// <summary>
        /// Gets a complete list of all the possible webhooks in shopify.
        /// </summary>
        /// <param name="baseWebhookUrl">The base webhook url to use.</param>
        /// <returns>List<ShopifyWebhookModel></returns>
        public List<ShopifyWebhookModel> GetShopifyWebhookList(string baseWebhookUrl)
        {
            try
            {
                var now = DateTimeOffset.Now;
                using (var context = new Data.FlavorFiEntities())
                {
                    var webhooks = context.ShopifyWebhooks.AsNoTracking().Select(w => new ShopifyWebhookModel
                    {
                        Url = baseWebhookUrl,
                        Event = w.Event,
                        Topic = w.Topic,
                        Format = w.Format,
                    });
                    return webhooks.ToList();
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }
}