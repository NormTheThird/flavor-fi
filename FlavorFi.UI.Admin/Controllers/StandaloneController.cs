using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FlavorFi.UI.Admin.Controllers
{
    public class StandaloneController : BaseController
    {
        public ActionResult OrderPickup()
        {
            return View("~/Views/Standalone/OrderPickup.cshtml");
        }

        public ActionResult QualityCheck()
        {

            //var getStandaloneApplicationRequest = new GetStandaloneApplicationByNameRequest { StandaloneApplicationName = "QualityCheck", CompanySiteId = Guid.Parse("") };
            //var getStandaloneApplicationResponse = this.StandaloneApplicationService.GetStandaloneApplicationByName(getStandaloneApplicationRequest);
            
            return View("~/Views/Standalone/QualityCheck.cshtml");
        }

        [HttpPost]
        public ActionResult GetShopifyOrdersForPickup(GetShopifyOrdersForPickupRequest request)
        {
            var response = this.DatabaseShopifyOrderService.GetShopifyOrdersForPickup(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult SaveShopifyOrderAsPickedup(SaveShopifyOrderAsPickedupRequest request)
        {
            var response = this.DatabaseShopifyOrderService.SaveShopifyOrderAsPickedup(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult GetShopifyOrder(string orderNumber)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("name", orderNumber);
            parameters.Add("order_number", orderNumber);
            parameters.Add("status", "any");
            var request = new GetShopifyRecordByQueryRequest
            {
                // TODO: TREY 10/27/2019 FOR OTHER COMPANIES TO USE THIS WE WOULD NEED TO STORE THIS FILE LOCALLY ON THE COMPUTER.
                BaseSite = this.GetShopifyBaseSiteModel(Guid.Parse("97948564-E3E5-4A8D-A20E-676832D0636E")),
                Parameters = parameters
            };
            var orderResponse = this.ShopifyOrderService.GetShopifyQualityCheckOrder(request);
            return Json(orderResponse);
        }
    }
}