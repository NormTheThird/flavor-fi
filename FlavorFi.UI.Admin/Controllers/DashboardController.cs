using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Services.DatabaseServices;
using System.Web.Mvc;

namespace FlavorFi.UI.Admin.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IShopifyGiftCardService ShopifyGiftCardService { get; }
        public IUploadService UploadService { get; }

        public DashboardController()
        {
            this.ShopifyGiftCardService = new ShopifyGiftCardService();
            this.UploadService = new UploadService();
        }

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Company");
        }

        #region Gift Card Total Widget

        [HttpPost]
        public ActionResult GetShopifyGiftCardTotals(GetShopifyGiftCardTotalsRequest request)
        {
            var response = this.ShopifyGiftCardService.GetShopifyGiftCardTotals(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult GetShopifyGiftCardMonthlyTotals(GetShopifyGiftCardMonthlyTotalsRequest request)
        {
            var response = this.ShopifyGiftCardService.GetShopifyGiftCardMonthlyTotals(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult GetLatestsUploadDate(GetLatestsUploadDateRequest request)
        {
            var response = this.UploadService.GetLatestsUploadDate(request);
            return Json(response);
        }

        #endregion

    }
}