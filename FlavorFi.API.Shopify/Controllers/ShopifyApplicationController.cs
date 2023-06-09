using FlavorFi.API.Shopify.RequestAndResponses;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using System.Linq;
using System.Web.Http;

namespace FlavorFi.API.Shopify.Controllers
{
    [Authorize]
    [RoutePrefix("api/ShopifyApplication")]
    public class ShopifyApplicationController : BaseApiController
    {

        [HttpPost]
        [AllowAnonymous]
        [Route("GetCompanySiteApplicationStatus")]
        public IHttpActionResult GetCompanySiteApplicationStatus(GetCompanySiteApplicationStatusRequest request)
        {
            if (request == null)
                return Json(new GetCompanySiteApplicationStatusResponse { ErrorMessage = "GetCompanySiteApplicationStatus request object is null" });

            var getCompanySiteByDomainRequest = new GetCompanySiteByDomainRequest { Domain = request.Domain };
            var getCompanySiteByDomainResponse = this.CompanyService.GetCompanySiteByDomain(getCompanySiteByDomainRequest);
            if (!getCompanySiteByDomainResponse.IsSuccess)
                return Json(new GetCompanySiteApplicationStatusResponse { ErrorMessage = getCompanySiteByDomainResponse.ErrorMessage });

            var companySite = getCompanySiteByDomainResponse.CompanySite;
            var getCompanySiteApplicationsRequest = new GetCompanySiteApplicationsRequest { CompanySiteId = companySite.Id };
            var getCompanySiteApplicationsResponse = this.CompanyService.GetCompanySiteApplications(getCompanySiteApplicationsRequest);
            if (!getCompanySiteApplicationsResponse.IsSuccess)
                return Json(new GetCompanySiteApplicationStatusResponse { ErrorMessage = getCompanySiteApplicationsResponse.ErrorMessage });

            var companySiteApplication = getCompanySiteApplicationsResponse.CompanySiteApplications.FirstOrDefault(_ => _.ApplicationName.Equals(request.ApplicationName));
            if (companySiteApplication == null)
                return Json(new GetCompanySiteApplicationStatusResponse { IsSuccess = true });
            return Json(new GetCompanySiteApplicationStatusResponse { IsSuccess = true, IsEnabled = companySiteApplication.IsEnabled, CompanySiteApplicationId = companySiteApplication.Id });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ChangeCompanySiteApplicationStatus")]
        public IHttpActionResult ChangeCompanySiteApplicationStatus(ChangeCompanySiteApplicationStatusRequest request)
        {
            return Json("Not Yet Implemented");
        }
    }
}