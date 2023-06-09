using FlavorFi.Common.Models.DatabaseModels;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace FlavorFi.API.Shopify.Controllers
{
    [Authorize]
    [RoutePrefix("api/ShopifyOrder")]
    public class ShopifyOrderController : BaseApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("GetCompanySiteApplicationStatus")]
        public void Orders()
        {
            var webhookResult = ProcessShopifyWebhook(Request);
            if (!webhookResult.IsSuccess)
                return;

            var topic = Request.Headers.GetValues("x-shopify-topic").FirstOrDefault();
            switch (topic.Trim())
            {
                case "orders/cancelled":
                    return;
                case "orders/create":
                    Task.Factory.StartNew(() => Create(webhookResult.Data, webhookResult.companySite));
                    return;
                case "orders/delete":
                    return;
                case "orders/fulfilled":
                    return;
                case "orders/paid":
                    return;
                case "orders/partially_fulfilled":
                    return;
                case "orders/updated":
                    return;
                default:
                    return;
            }
        }

        private void Create(JObject data, CompanySiteModel companySite)
        {

        }

        ///// <summary>
        /////     Process the shopify order webhook events
        ///// </summary>
        ///// <param name="_companySite">The company site of the webhook.</param>
        ///// <param name="_topic">The topic of the webhook.</param>
        ///// <param name="_data">The json data object from the webhook.</param>
        //private static void ProcessOrderWebhookEvents(CompanySiteModel _companySite, string _topic, JObject _data)
        //{
        //    try
        //    {
        //        if (_topic.Equals("cancelled") || _topic.Equals("fulfilled") || _topic.Equals("delete") || _topic.Equals("paid") || _topic.Equals("partially_fulfilled")) return;
        //        if (_topic.Equals("create") || _topic.Equals("updated"))
        //        {
        //            if (_topic.Equals("create")) DoCompanySpecificTask(_companySite, _data);
        //            Thread.Sleep(new Random().Next(1000, 5000));

        //            var saveShopifyOrderRequest = new SaveShopifyOrderRequest { CompanySiteId = _companySite.Id };
        //            var shopifyOrder = Mappers.ShopifyMapper.Map<Common.Models.ShopifyModels.ShopifyOrderModel>(_data);
        //            Mappers.MobMapper.Map(shopifyOrder, saveShopifyOrderRequest.Order);
        //            var saveShopifyOrderResponse = ShopifyOrderService.SaveShopifyOrder(saveShopifyOrderRequest);

        //            if (saveShopifyOrderResponse.IsSuccess)
        //            {
        //                var getTransactionsRequest = new Common.RequestAndResponses.ShopifyRequestAndResponses.GetShopifyRecordRequest { CompanySiteId = _companySite.Id, RecordId = shopifyOrder.Id };
        //                var getTransactionsResponse = new Services.ShopifyServices.ShopifyOrderService().GetShopifyOrderTransactions(getTransactionsRequest);
        //                foreach (var transaction in getTransactionsResponse.OrderTransactions)
        //                {
        //                    var saveShopifyOrderTransactionRequest = new SaveShopifyOrderTransactionRequest { CompanySiteId = _companySite.Id };
        //                    Mappers.MobMapper.Map(transaction, saveShopifyOrderTransactionRequest.OrderTransaction);
        //                    saveShopifyOrderTransactionRequest.OrderTransaction.ShopifyOrderId = saveShopifyOrderResponse.ShopifyOrderId;
        //                    var saveShopifyOrderTransactionResponse = ShopifyOrderService.SaveShopifyOrderTransaction(saveShopifyOrderTransactionRequest);
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
}