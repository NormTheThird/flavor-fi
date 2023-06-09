using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Services.DatabaseServices;
using System;
using System.Web.Mvc;

namespace FlavorFi.UI.Admin.Controllers
{
    [Authorize]
    public class NavigationController : Controller
    {
        public ICompanyService CompanyService { get; private set; }
        public ISecurityService SecurityService { get; private set; }

        public NavigationController()
        {
            this.CompanyService = new CompanyService();
            this.SecurityService = new SecurityService();
        }

        [HttpPost]
        public ActionResult GetCompanies(GetCompaniesRequest request)
        {
            var response = CompanyService.GetCompanies(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult GetCompanyAndSites(GetCompanyRequest request)
        {
            var getCompanyAndSitesResponse = new GetCompanyAndSitesResponse();
            var securityModel = SecurityModels.CustomPrincipal.GetBaseSecurityModel();
            request.CompanyId = securityModel.CompanyId ?? Guid.Empty;

            var getCompanyResponse = CompanyService.GetCompany(request);
            if (!getCompanyResponse.IsSuccess)
                return Json(getCompanyResponse);

            getCompanyAndSitesResponse.Company = getCompanyResponse.Company;
            var getCompanySitesRequest = new GetCompanySitesRequest { CompanyId = request.CompanyId };
            var getCompanySitesResponse = CompanyService.GetCompanySites(getCompanySitesRequest);
            if (!getCompanySitesResponse.IsSuccess)
                return Json(getCompanySitesResponse);

            getCompanyAndSitesResponse.SelectedCompanySiteId = securityModel.CompanySiteId ?? Guid.Empty;
            getCompanyAndSitesResponse.CompanySites = getCompanySitesResponse.CompanySites;
            getCompanyAndSitesResponse.IsSuccess = true;
            return Json(getCompanyAndSitesResponse);
        }

        [HttpPost]
        public ActionResult UpdateCompanyId(UpdateCompanyIdRequest request)
        {
            var response = SecurityService.UpdateCompanyId(request);
            return Json(response);
        }

    }
}