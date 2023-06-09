using System;
using FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using System.Web.Mvc;
using FlavorFi.Services.ShopifyServices;

namespace FlavorFi.UI.Controllers
{
    public class QualityController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetShopifyOrder(GetShopifyRecordByQueryRequest request)
        {
            var user = FlavorFi.Common.Helpers.Security.DecryptUserToken(Common.Helpers.Security.ReadEncryptedCookie("UserToken").UserToken);
            request.CompanySiteId = Guid.Parse("97948564-E3E5-4A8D-A20E-676832D0636E");// user.CompanySiteId;
            var orderResponse = new ShopifyOrderService().GetShopifyOrderByQuery(request);
            var jsonResult = Json(orderResponse, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}