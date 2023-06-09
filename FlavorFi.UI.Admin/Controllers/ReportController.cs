using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Services.DatabaseServices;
using System.Web.Mvc;

namespace FlavorFi.UI.Admin.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        public ActionResult CostOfGoods()
        {
            return View();
        }

        public ActionResult GiftCards()
        {
            return View();
        }

        public ActionResult Sales()
        {
            return View();
        }


        //[HttpPost]
        //public ActionResult RunCOGReport(GetReportRequest request)
        //{
        //    var response = ReportService.GetCostOfGoodsReport(request);
        //    var jsonResult = Json(response, JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;
        //}

        //[HttpPost]
        //public ActionResult ExportCOGReport(GetReportRequest request)
        //{
        //    var response = ReportService.GetExportCostOfGoodsReport(request);
        //    var jsonResult = Json(response, JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;
        //}



        //[HttpGet]
        //public ActionResult RunGiftCardReport(GetReportRequest request)
        //{
        //    var response = ReportService.GetGiftCardReport(request);
        //    var jsonResult = Json(response, JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;
        //}

        //[HttpPost]
        //public ActionResult ExportGiftCardReport(GetReportRequest request)
        //{
        //    var response = ReportService.GetExportGiftCardReport(request);
        //    var jsonResult = Json(response, JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;
        //}


        //[HttpGet]
        //public ActionResult GetSalesReport (GetReportRequest request)
        //{
        //    var response = ReportService.GetSalesReport(request);
        //    var jsonResult = Json(response, JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;
        //}
    }
}