using FlavorFi.Common.Models.DatabaseModels;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace FlavorFi.API.Shopify.Controllers
{
    [Authorize]
    [RoutePrefix("api/ShopifyProduct")]
    public class ShopifyProductController : BaseApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("Products")]
        public void Products()
        {
            var webhookResult = ProcessShopifyWebhook(Request);
            if (!webhookResult.IsSuccess)
                return;

            var topic = Request.Headers.GetValues("x-shopify-topic").FirstOrDefault();
            switch (topic.Trim())
            {
                case "products/create":
                    return;
                case "products/delete":
                    return;
                case "products/update":
                    Task.Factory.StartNew(() => Update(webhookResult.Data, webhookResult.companySite));
                    return;
                default:
                    return;
            }
        }

        private void Update(JObject data, CompanySiteModel companySite)
        {

        }
    }



    ///// <summary>
    /////     Process the shopify product webhook events
    ///// </summary>
    ///// <param name="_companySite">The company site of the webhook.</param>
    ///// <param name="_topic">The topic of the webhook.</param>
    ///// <param name="_data">The json data object from the webhook.</param>
    //private static void ProcessProductWebhookEvents(CompanySiteModel _companySite, string _topic, JObject _data)
    //{
    //    try
    //    {
    //        var userToken = AccountService.GetWebhookUserToken().UserToken;
    //        if (_topic.Equals("delete")) return;
    //        if (_topic.ToLower().Equals("create") || _topic.Equals("update"))
    //        {
    //            if (_topic.Equals("update")) Thread.Sleep(5000);
    //            var saveShopifyProductRequest = new SaveShopifyProductRequest();
    //            var shopifyProduct = Mappers.ShopifyMapper.Map<Common.Models.ShopifyModels.ShopifyProductModel>(_data);
    //            Mappers.MobMapper.Map(shopifyProduct, saveShopifyProductRequest.Product);
    //            saveShopifyProductRequest.Product.CompanySiteId = _companySite.Id;
    //            saveShopifyProductRequest.UserToken = AccountService.GetWebhookUserToken().UserToken;
    //            var saveShopifyProductResponse = ShopifyProductService.SaveShopifyProduct(saveShopifyProductRequest);
    //            if (saveShopifyProductResponse.IsSuccess)
    //            {
    //                var metafieldRequest = new Common.RequestAndResponses.ShopifyRequestAndResponses.GetShopifyRecordRequest { CompanySiteId = _companySite.Id, UserToken = userToken, RecordId = shopifyProduct.Id };
    //                var metafieldResponse = Services.ShopifyServices.ShopifyProductService.GetShopifyProductMetafields(metafieldRequest);
    //                if (metafieldResponse.IsSuccess)
    //                {
    //                    foreach (var metafield in metafieldResponse.Metafields)
    //                    {
    //                        var saveShopifyMetafieldRequest = new SaveShopifyMetafieldRequest { UserToken = userToken, CompanySiteId = _companySite.Id };
    //                        Mappers.MobMapper.Map(metafield, saveShopifyMetafieldRequest.Metafield);
    //                        var saveShopifyMetafieldResponse = ShopifyProductService.SaveShopifyProductMetafield(saveShopifyMetafieldRequest);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        LoggingService.LogError(new LogErrorRequest { ex = ex });
    //    }
    //}
}