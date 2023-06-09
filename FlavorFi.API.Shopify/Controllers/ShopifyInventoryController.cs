using FlavorFi.API.Shopify.RequestAndResponses;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using FlavorFi.Services.DatabaseServices;
using FlavorFi.Services.ShopifyServices;
using System.Linq;
using System.Web.Http;

namespace FlavorFi.API.Shopify.Controllers
{
    [Authorize]
    [RoutePrefix("api/ShopifyInventory")]
    public class ShopifyInventoryController : BaseApiController
    {
        public ICompanyService CompanyService { get; set; }
        public IShopifyInventoryService ShopifyInventoryService { get; set; }

        public ShopifyInventoryController()
        {
            this.CompanyService = new CompanyService();
            this.ShopifyInventoryService = new ShopifyInventoryService();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("GetInventoryLocations")]
        public IHttpActionResult GetInventoryLocations(GetInventoryLocationsRequest request)
        {
            // Get the current company site application.
            var getCompanySiteApplicationRequest = new GetCompanySiteApplicationRequest { CompanySiteApplicationId = request.CompanySiteApplicationId };
            var getCompanySiteApplicationResponse = this.CompanyService.GetCompanySiteApplication(getCompanySiteApplicationRequest);
            if (!getCompanySiteApplicationResponse.IsSuccess)
                return Json(new GetInventoryLocationsResponse { ErrorMessage = getCompanySiteApplicationResponse.ErrorMessage });

            // Get the current company site.
            var application = getCompanySiteApplicationResponse.CompanySiteApplication;
            var getCompanySiteRequest = new GetCompanySiteRequest { CompanySiteId = application.CompanySiteId };
            var getCompanySiteResponse = this.CompanyService.GetCompanySite(getCompanySiteRequest);
            if (!getCompanySiteResponse.IsSuccess)
                return Json(new GetInventoryLocationsResponse { ErrorMessage = getCompanySiteResponse.ErrorMessage });

            // Get the current inventory locations from shopify.
            var baseSite = GetShopifyBaseSiteModel(getCompanySiteResponse.CompanySite, application);
            var getShopifyInventoryLoactionsRequest = new GetShopifyRecordsRequest { BaseSite = baseSite };
            var getShopifyInventoryLoactionsResponse = this.ShopifyInventoryService.GetShopifyInventoryLoactions(getShopifyInventoryLoactionsRequest);
            if (!getShopifyInventoryLoactionsResponse.IsSuccess)
                return Json(new GetInventoryLocationsResponse { ErrorMessage = getShopifyInventoryLoactionsResponse.ErrorMessage });

            var response = new GetInventoryLocationsResponse { IsSuccess = true };
            foreach (var location in getShopifyInventoryLoactionsResponse.Locations)
            {
                response.Locations.Add(new InventoryLocationModel
                {
                    LocationId = location.Id,
                    Name = location.Name,
                    IsEnabled = application.Locations.Split('|').Contains(location.Id.ToString())
                });
            }

            return Json(response);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("SaveInventoryLocation")]
        public IHttpActionResult SaveInventoryLocation(SaveInventoryLocationRequest request)
        {
            // Get the current company site application.
            var getCompanySiteApplicationRequest = new GetCompanySiteApplicationRequest { CompanySiteApplicationId = request.CompanySiteApplicationId };
            var getCompanySiteApplicationResponse = this.CompanyService.GetCompanySiteApplication(getCompanySiteApplicationRequest);
            if (!getCompanySiteApplicationResponse.IsSuccess)
                return Json(new SaveInventoryLocationResponse { ErrorMessage = getCompanySiteApplicationResponse.ErrorMessage });

            // Loop through locations and add or remove.
            var application = getCompanySiteApplicationResponse.CompanySiteApplication;
            var locations = application.Locations.Split('|').Where<string>(s => !string.IsNullOrEmpty(s)).ToList();
            if (!locations.Contains(request.LocationId.ToString()) && request.IsEnabled)
                locations.Add(request.LocationId.ToString());
            else if (locations.Contains(request.LocationId.ToString()) && !request.IsEnabled)
                locations.Remove(request.LocationId.ToString());
            application.Locations = string.Join("|", locations.ToArray());

            // Save the new locations array to the company site application.
            var saveCompanySiteApplicationRequest = new SaveCompanySiteApplicationRequest { CompanySiteApplication = application };
            var saveCompanySiteApplicationResponse = this.CompanyService.SaveCompanySiteApplication(saveCompanySiteApplicationRequest);
            if (!saveCompanySiteApplicationResponse.IsSuccess)
                return Json(new SaveInventoryLocationResponse { ErrorMessage = saveCompanySiteApplicationResponse.ErrorMessage });
            return Json(new SaveInventoryLocationResponse { IsSuccess = true });
        }
    }
}