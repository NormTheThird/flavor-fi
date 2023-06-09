using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using FlavorFi.Services.DatabaseServices;
using System;
using System.Web.Mvc;

namespace FlavorFi.UI.Admin.Controllers
{
    public class CompanyController : BaseController
    {
        public ICompanyService CompanyService { get; set; }

        public CompanyController()
        {
            this.CompanyService = new CompanyService();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetCompany(GetCompanyRequest request)
        {
            request.CompanyId = SecurityModels.CustomPrincipal.GetBaseSecurityModel().CompanyId ?? Guid.Empty;
            var response = this.CompanyService.GetCompany(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult GetCompanySites(GetCompanySitesRequest request)
        {
            var response = this. CompanyService.GetCompanySites(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult GetCompanySiteLocations(GetShopifyRecordsRequest request)
        {
            var companySiteId = SecurityModels.CustomPrincipal.GetBaseSecurityModel().CompanySiteId ?? Guid.Empty;
            var getCompanySiteResponse = CompanyService.GetCompanySite(new GetCompanySiteRequest { CompanySiteId = companySiteId });
            if (!getCompanySiteResponse.IsSuccess)
                return Json(getCompanySiteResponse);

            request.BaseSite = GetShopifyBaseSiteModel(getCompanySiteResponse.CompanySite);
            var response = new Services.ShopifyServices.ShopifyLocationService().GetShopifyLocations(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult SaveCompany(SaveCompanyRequest request)
        {
            var response = this.CompanyService.SaveCompany(request);
            return Json(response);
        }

    }
}