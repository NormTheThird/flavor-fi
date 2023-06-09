using FlavorFi.Common.Models.DatabaseModels;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FlavorFi.UI.Admin.Controllers
{
    [Authorize]
    public class WebhookController : BaseController
    {
        public ActionResult Webhooks()
        {
            return View();
        }

        public ActionResult WebhookActivityLogs()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetShopifyWebhooks(GetShopifyRecordsRequest request)
        {
            var companySiteId = SecurityModels.CustomPrincipal.GetBaseSecurityModel().CompanySiteId ?? Guid.Empty;
            var getCompanySiteResponse = CompanyService.GetCompanySite(new GetCompanySiteRequest { CompanySiteId = companySiteId });
            if (!getCompanySiteResponse.IsSuccess)
                return Json(getCompanySiteResponse);

            request.BaseSite = GetShopifyBaseSiteModel(getCompanySiteResponse.CompanySite);
            var response = ShopifyWebhookService.GetShopifyWebhooks(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ActivateShopifyWebhook(ActivateShopifyWebhookRequest request)
        {
            var companySiteId = SecurityModels.CustomPrincipal.GetBaseSecurityModel().CompanySiteId ?? Guid.Empty;
            var getCompanySiteResponse = CompanyService.GetCompanySite(new GetCompanySiteRequest { CompanySiteId = companySiteId });
            if (!getCompanySiteResponse.IsSuccess)
                return Json(getCompanySiteResponse);

            request.BaseSite = GetShopifyBaseSiteModel(getCompanySiteResponse.CompanySite);
            var response = ShopifyWebhookService.ActivateShopifyWebhook(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult DeleteShopifyWebhook(DeactivateShopifyWebhookRequest request)
        {
            var companySiteId = SecurityModels.CustomPrincipal.GetBaseSecurityModel().CompanySiteId ?? Guid.Empty;
            var getCompanySiteResponse = CompanyService.GetCompanySite(new GetCompanySiteRequest { CompanySiteId = companySiteId });
            if (!getCompanySiteResponse.IsSuccess)
                return Json(getCompanySiteResponse);

            request.BaseSite = GetShopifyBaseSiteModel(getCompanySiteResponse.CompanySite);
            var response = ShopifyWebhookService.DeactivateShopifyWebhook(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult GetShopifyWebhookActivityLogs(GetShopifyWebhookActivityLogsRequest request)
        {
            var response = this.DatabaseShopifyWebhookService.GetShopifyWebhookActivityLogs(request);
            return Json(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public void ShopifyWebhook()
        {
            try
            {
                var domain = Request.Headers["x-shopify-shop-domain"].ToString();
                var getCompanyResponse = CompanyService.GetCompanySiteByDomain(new GetCompanySiteByDomainRequest { Domain = domain });
                if (!getCompanyResponse.IsSuccess)
                    throw new ApplicationException($"Webhook was unable to get company [Domain: {domain}]");
                var companySite = getCompanyResponse.CompanySite;
                var topic = Request.Headers["x-shopify-topic"].ToString();
                using (var reader = new StreamReader(Request.InputStream))
                {
                    var json = JObject.Parse(reader.ReadToEnd());
                    Task.Factory.StartNew(() => ProcessWebHook(topic, json, companySite));
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
            }
        }

        private void ProcessWebHook(string topic, JObject data, CompanySiteModel companySite)
        {
            try
            {
                var recordId = Convert.ToInt64(0);
                var logId = LogShopifyWebhook(companySite.Id, Convert.ToInt64(data["id"]), topic);
                switch (topic.Trim())
                {
                    case "orders/create":
                        OrderCreate(logId, data, companySite);
                        return;
                    case "orders/updated":
                        OrderUpdated(data, companySite);
                        return;
                    case "products/update":
                        // TREY: 11/5/2019 Per roger stop doing anything when a product is updated.
                        // ProductsUpdate(logId, data, companySite);
                        return;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
            }
        }

        private void OrderCreate(Guid logId, JObject data, CompanySiteModel companySite)
        {
            var productIds = new List<long>();
            var sourceName = data["source_name"]?.ToString();
            if (string.IsNullOrEmpty(sourceName)) return;
            else if (!sourceName.Equals("web", StringComparison.CurrentCultureIgnoreCase) &&
                     !sourceName.Equals("1520611", StringComparison.CurrentCultureIgnoreCase)) //1520611 is the sales channel for the tap-cart mobile app
                return;

            foreach (var item in data["line_items"])
            {
                var productIdString = item["product_id"]?.ToString();
                if (!string.IsNullOrEmpty(productIdString))
                    productIds.Add(Convert.ToInt64(productIdString));
            }

            foreach (var productId in productIds)
            {
                if (productId == 0) continue;
                var request = new GetShopifyRecordRequest { RecordId = productId, BaseSite = GetShopifyBaseSiteModel(companySite) };
                var getShopifyProductResponse = new Services.ShopifyServices.ShopifyProductService().GetShopifyProduct(request);

                if (!getShopifyProductResponse.IsSuccess)
                    LogShopifyWebhookActivity(logId, $"{{Success: false, Outcome: {getShopifyProductResponse.ErrorMessage}, ProductId: {productId}}}");
                else
                {
                    var product = getShopifyProductResponse.Product;
                    var outcome = "";
                    var inventory = GetProductInventory(product.Variants.Select(_ => _.InventoryItemId).ToArray(), companySite);
                    if (inventory == -9999) outcome = "Unable to get inventory count";
                    else if (inventory <= 0) outcome = product.Tags.ToLower().Contains("restock") ? "Product was not hidden with tag restock" : HideProduct(product, companySite);
                    else outcome = UnHideProduct(product, companySite);
                    LogShopifyWebhookActivity(logId, $"{{Success: {inventory != -9999}, Outcome: {outcome}, Inventory: {inventory}, Title: {product.Title}, ProductId: {productId}, Published: {product.PublishedAt}, Tags: {product.Tags}}}");
                }
            }
        }

        private void OrderUpdated(JObject data, CompanySiteModel companySite)
        {
            try
            {
                var shopifyOrder = new Services.ShopifyServices.ShopifyMapperService().Map<Common.Models.ShopifyModels.ShopifyOrderModel>(data);
                var saveShopifyOrderRequest = new SaveShopifyOrderRequest { CompanySiteId = companySite.Id };
                var order = new Services.DatabaseServices.MapperService().Map(shopifyOrder, saveShopifyOrderRequest.Order);
                var saveShopifyOrderResponse = new Services.DatabaseServices.ShopifyOrderService().SaveShopifyOrder(saveShopifyOrderRequest);
                if (saveShopifyOrderResponse.IsSuccess)
                {
                    AddShopifyOrderShippingLines(shopifyOrder, companySite.Id, saveShopifyOrderResponse.ShopifyOrderId);
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
            }
        }

        //private void ProductsUpdate(Guid logId, JObject data, CompanySiteModel companySite)
        //{
        //    var productId = Convert.ToInt64(data["id"]);
        //    var productTitle = data["title"].ToString();
        //    var published = data["published_at"]?.ToString();
        //    var isPublished = !string.IsNullOrEmpty(published);
        //    var tags = data["tags"].ToString().ToLower();
        //    if (tags.ToLower().Contains("hideit_restock"))
        //    {
        //        var outcome = UnHideProduct(productId, isPublished, companySite);
        //        LogShopifyWebhookActivity(logId, $"{{Success: true, Outcome: {outcome}, Flag: hideit_restock, Title: {productTitle}, ProductId: {productId}, Published: {published}, Tags: {tags}}}");
        //    }
        //    else if (tags.ToLower().Contains("hideit_no_show"))
        //    {
        //        var outcome = HideProduct(productId, isPublished, companySite);
        //        LogShopifyWebhookActivity(logId, $"{{Success: true, Outcome: {outcome}, Flag: hideit_no_show, Title: {productTitle}, ProductId: {productId}, Published: {published}, Tags: {tags}}}");
        //    }
        //    //TODO: TREY: 4/25/2019 REMOVE THIS WHEN NOT USING ZERO OUT NO SHOW ANY MORE
        //    else if (tags.ToLower().Contains("zero_out_no_show"))
        //    {
        //        var outcome = HideProduct(productId, isPublished, companySite);
        //        LogShopifyWebhookActivity(logId, $"{{Success: true, Outcome: {outcome}, Flag: zero_out_no_show, Title: {productTitle}, ProductId: {productId}, Published: {published}, Tags: {tags}}}");
        //    }
        //    else
        //    {
        //        var request = new GetShopifyRecordRequest { RecordId = productId, BaseSite = GetShopifyBaseSiteModel(companySite) };
        //        var getShopifyProductResponse = new Services.ShopifyServices.ShopifyProductService().GetShopifyProduct(request);
        //        if (!getShopifyProductResponse.IsSuccess)
        //            LogShopifyWebhookActivity(logId, $"{{Success: false, Outcome: {getShopifyProductResponse.ErrorMessage}, Title: {productTitle}, ProductId: {productId}, Published: {published}, Tags: {tags}}}");
        //        else
        //        {
        //            var outcome = "";
        //            var inventory = GetProductInventory(getShopifyProductResponse.Product, companySite);
        //            if (inventory <= 0) outcome = HideProduct(productId, isPublished, companySite);
        //            else outcome = UnHideProduct(productId, isPublished, companySite);
        //            LogShopifyWebhookActivity(logId, $"{{Success: true, Outcome: {outcome}, Inventory: {inventory}, Title: {productTitle}, ProductId: {productId}, Published: {published}, Tags: {tags}}}"); ;
        //        }
        //    }
        //}

        private Guid LogShopifyWebhook(Guid companySiteId, long recordId, string topic)
        {
            try
            {
                var response = DatabaseShopifyWebhookService.GetShopifyWebhooks(new GetShopifyWebhooksRequest());
                if (!response.IsSuccess)
                    return Guid.Empty;

                var request = new LogShopifyWebhookRequest
                {
                    CompanySiteId = companySiteId,
                    ShopifyWebhookId = response.ShopifyWebhooks.FirstOrDefault(w => w.Topic == topic).Id,
                    RecordId = recordId
                };
                return LoggingService.LogShopifyWebhook(request).ShopifyWebhookLogId;
            }
            catch (Exception) { return Guid.Empty; }
        }

        private void LogShopifyWebhookActivity(Guid shopifyWebhookLogId, string activity)
        {
            var request = new LogShopifyWebhookActivityRequest
            {
                ShopifyWebhookLogId = shopifyWebhookLogId,
                Activity = activity
            };
            LoggingService.LogShopifyWebhookActivity(request);
        }

        private int GetProductInventory(long[] inventoryItemIds, CompanySiteModel companySite)
        {
            try
            {
                // TODO: TREY: 11/5/2019 Store locations.
                // TEST URL
                var apiVersion = ConfigurationManager.AppSettings["ShopifyApiVersion"];
                //var url = $@"{companySite.ShopifyUrl}/admin/api/{apiVersion}/inventory_levels.json?inventory_item_ids=" + string.Join(",", inventoryItemIds) + "&location_ids=5980094579";
                // PRODUCTION URL
                var url = $@"{companySite.ShopifyUrl}/admin/api/{apiVersion}/inventory_levels.json?inventory_item_ids=" + string.Join(",", inventoryItemIds) + "&location_ids=33223489";
                var webRequest = WebRequest.Create(url);
                this.SetBasicAuthHeader(webRequest, "Get", companySite);
                using (var dataResponse = webRequest.GetResponse())
                using (var reader = new StreamReader(dataResponse.GetResponseStream()))
                {
                    var inventory = 0;
                    var inventoryData = JObject.Parse(reader.ReadToEnd());
                    foreach (var inventoryLevel in inventoryData["inventory_levels"])
                    {
                        var available = inventoryLevel["available"].ToString();
                        if (!string.IsNullOrEmpty(available))
                            inventory += Convert.ToInt32(available) < 0 ? 0 : Convert.ToInt32(available);
                    }
                    return inventory;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return -9999;
            }
        }

        private string HideProduct(Common.Models.ShopifyModels.ShopifyProductModel product, CompanySiteModel companySite)
        {
            try
            {
                var isPublished = product.PublishedAt != null;
                if (!isPublished) return "Product already hidden no action taken";
                if (product.Id == 405251313 || product.Id == 534302392366)
                    return "Product should not be hidden no action taken";
                string postData = "{\"product\": {\"id\": " + product.Id + ",\"published\": false }}";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                Thread.Sleep(500);
                var apiVersion = ConfigurationManager.AppSettings["ShopifyApiVersion"];
                var url = $@"{companySite.ShopifyUrl}/admin/api/{apiVersion}/products/" + product.Id.ToString() + ".json";
                var webRequest = WebRequest.Create(url);
                webRequest.ContentLength = byteArray.Length;
                this.SetBasicAuthHeader(webRequest, "Put", companySite);
                using (var dataStream = webRequest.GetRequestStream())
                    dataStream.Write(byteArray, 0, byteArray.Length);
                using (var updateResponse = webRequest.GetResponse())
                using (var reader = new StreamReader(updateResponse.GetResponseStream()))
                    return "Product was hidden";
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return ex.Message;
            }
        }

        private string UnHideProduct(Common.Models.ShopifyModels.ShopifyProductModel product, CompanySiteModel companySite)
        {
            try
            {
                var isPublished = product.PublishedAt != null;
                if (isPublished) return "Product already published no action taken";
                string postData = "{\"product\": {\"id\": " + product.Id + ",\"published\": true }}";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                Thread.Sleep(500);
                var apiVersion = ConfigurationManager.AppSettings["ShopifyApiVersion"];
                var url = $@"{companySite.ShopifyUrl}/admin/api/{apiVersion}/products/" + product.Id.ToString() + ".json";
                var webRequest = WebRequest.Create(url);
                webRequest.ContentLength = byteArray.Length;
                this.SetBasicAuthHeader(webRequest, "Put", companySite);
                using (var dataStream = webRequest.GetRequestStream())
                    dataStream.Write(byteArray, 0, byteArray.Length);
                using (var updateResponse = webRequest.GetResponse())
                using (var reader = new StreamReader(updateResponse.GetResponseStream()))
                    return "Product was published";
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return ex.Message;
            }
        }

        private void SetBasicAuthHeader(WebRequest request, string method, CompanySiteModel companySite)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var authInfo = $"{companySite.ShopifyApiPublicKey}:{companySite.ShopifyApiSecretKey}";
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = $"Basic {authInfo}";
            request.ContentType = "application/json";
            request.Method = method;
        }

        private void AddShopifyOrderShippingLines(Common.Models.ShopifyModels.ShopifyOrderModel order, Guid baseSiteId, Guid shopifyOrderId)
        {
            if (order.ShippingLines == null) return;
            foreach (var shippingLine in order.ShippingLines)
            {
                var saveShopifyOrderShippingLineFromShopifyRequest = new SaveShopifyOrderShippingLineFromShopifyRequest
                {
                    CompanySiteId = baseSiteId,
                    ShopfiyOrderId = shopifyOrderId,
                    OriginalShopifyOrderId = order.Id,
                    ShopifyOrderShippingLine = shippingLine
                };
                new Services.DatabaseServices.ShopifyOrderService().SaveShopifyOrderShippingLineFromShopify(saveShopifyOrderShippingLineFromShopifyRequest);
            }
        }
    }
}