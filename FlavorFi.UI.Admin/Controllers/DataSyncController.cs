using System;
using System.Web.Mvc;
using FlavorFi.Common.Models.DatabaseModels;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Services.DatabaseServices;

namespace FlavorFi.UI.Admin.Controllers
{
    [Authorize]
    public class DataSyncController : BaseController
    {
        public IDatabaseSyncService DatabaseSyncService { get; }

        public DataSyncController()
        {
            this.DatabaseSyncService = new DatabaseSyncService();
        }

        public ActionResult DatabaseSync()
        {
            return View("~/Views/DataSync/DatabaseSync/Index.cshtml");
        }

        [HttpPost]
        public ActionResult GetDatabaseSyncStatus(GetDatabaseSyncStatusRequest request)
        {
            var response = this.DatabaseSyncService.GetDatabaseSyncStatus(request);
            return Json(response);
        }

        #region Custom Collection Sync

        [HttpPost]
        public ActionResult SynchronizeDatabaseCustomCollections(BaseSyncRequest request)
        {
            var companySiteId = SecurityModels.CustomPrincipal.GetBaseSecurityModel().CompanySiteId ?? Guid.Empty;
            var getCompanySiteResponse = CompanyService.GetCompanySite(new GetCompanySiteRequest { CompanySiteId = companySiteId });
            if (!getCompanySiteResponse.IsSuccess)
                return Json(getCompanySiteResponse);

            request.BaseSite = GetBaseSiteModel(getCompanySiteResponse.CompanySite);
            var response = this.DatabaseSyncService.SynchronizeDatabaseCustomCollections(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult SynchronizeDatabaseCustomCollectionsStatus(BaseSyncStatusRequest request)
        {
            var companySiteId = SecurityModels.CustomPrincipal.GetBaseSecurityModel().CompanySiteId ?? Guid.Empty;
            var getCompanySiteResponse = CompanyService.GetCompanySite(new GetCompanySiteRequest { CompanySiteId = companySiteId });
            if (!getCompanySiteResponse.IsSuccess)
                return Json(getCompanySiteResponse);

            request.BaseSite = GetBaseSiteModel(getCompanySiteResponse.CompanySite);
            var response = this.DatabaseSyncService.SynchronizeDatabaseCustomCollectionsStatus(request);
            return Json(response);
        }

        #endregion

        #region Customer Sync

        [HttpPost]
        public ActionResult SynchronizeDatabaseCustomers(BaseSyncRequest request)
        {
            var companySiteId = SecurityModels.CustomPrincipal.GetBaseSecurityModel().CompanySiteId ?? Guid.Empty;
            var getCompanySiteResponse = CompanyService.GetCompanySite(new GetCompanySiteRequest { CompanySiteId = companySiteId });
            if (!getCompanySiteResponse.IsSuccess)
                return Json(getCompanySiteResponse);

            request.BaseSite = GetBaseSiteModel(getCompanySiteResponse.CompanySite);
            var response = this.DatabaseSyncService.SynchronizeDatabaseCustomers(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult SynchronizeDatabaseCustomersStatus(BaseSyncStatusRequest request)
        {
            var companySiteId = SecurityModels.CustomPrincipal.GetBaseSecurityModel().CompanySiteId ?? Guid.Empty;
            var getCompanySiteResponse = CompanyService.GetCompanySite(new GetCompanySiteRequest { CompanySiteId = companySiteId });
            if (!getCompanySiteResponse.IsSuccess)
                return Json(getCompanySiteResponse);

            request.BaseSite = GetBaseSiteModel(getCompanySiteResponse.CompanySite);
            var response = this.DatabaseSyncService.SynchronizeDatabaseCustomersStatus(request);
            return Json(response);
        }

        #endregion

        #region Order Sync

        [HttpPost]
        public ActionResult SynchronizeDatabaseOrders(BaseSyncRequest request)
        {
            var companySiteId = SecurityModels.CustomPrincipal.GetBaseSecurityModel().CompanySiteId ?? Guid.Empty;
            var getCompanySiteResponse = CompanyService.GetCompanySite(new GetCompanySiteRequest { CompanySiteId = companySiteId });
            if (!getCompanySiteResponse.IsSuccess)
                return Json(getCompanySiteResponse);

            request.BaseSite = GetBaseSiteModel(getCompanySiteResponse.CompanySite);
            var response = this.DatabaseSyncService.SynchronizeDatabaseOrders(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult SynchronizeDatabaseOrdersStatus(BaseSyncStatusRequest request)
        {
            var companySiteId = SecurityModels.CustomPrincipal.GetBaseSecurityModel().CompanySiteId ?? Guid.Empty;
            var getCompanySiteResponse = CompanyService.GetCompanySite(new GetCompanySiteRequest { CompanySiteId = companySiteId });
            if (!getCompanySiteResponse.IsSuccess)
                return Json(getCompanySiteResponse);

            request.BaseSite = GetBaseSiteModel(getCompanySiteResponse.CompanySite);
            var response = this.DatabaseSyncService.SynchronizeDatabaseOrdersStatus(request);
            return Json(response);
        }

        #endregion

        #region Product Sync

        [HttpPost]
        public ActionResult SynchronizeDatabaseProducts(BaseSyncRequest request)
        {
            var companySiteId = SecurityModels.CustomPrincipal.GetBaseSecurityModel().CompanySiteId ?? Guid.Empty;
            var getCompanySiteResponse = CompanyService.GetCompanySite(new GetCompanySiteRequest { CompanySiteId = companySiteId });
            if (!getCompanySiteResponse.IsSuccess)
                return Json(getCompanySiteResponse);

            request.BaseSite = GetBaseSiteModel(getCompanySiteResponse.CompanySite);
            var response = this.DatabaseSyncService.SynchronizeDatabaseProducts(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult SynchronizeDatabaseProductsStatus(BaseSyncStatusRequest request)
        {
            var companySiteId = SecurityModels.CustomPrincipal.GetBaseSecurityModel().CompanySiteId ?? Guid.Empty;
            var getCompanySiteResponse = CompanyService.GetCompanySite(new GetCompanySiteRequest { CompanySiteId = companySiteId });
            if (!getCompanySiteResponse.IsSuccess)
                return Json(getCompanySiteResponse);

            request.BaseSite = GetBaseSiteModel(getCompanySiteResponse.CompanySite);
            var response = this.DatabaseSyncService.SynchronizeDatabaseProductsStatus(request);
            return Json(response);
        }

        #endregion

        #region User Sync

        [HttpPost]
        public ActionResult SynchronizeDatabaseUsers(BaseSyncRequest request)
        {
            var companySiteId = SecurityModels.CustomPrincipal.GetBaseSecurityModel().CompanySiteId ?? Guid.Empty;
            var getCompanySiteResponse = CompanyService.GetCompanySite(new GetCompanySiteRequest { CompanySiteId = companySiteId });
            if (!getCompanySiteResponse.IsSuccess)
                return Json(getCompanySiteResponse);

            request.BaseSite = GetBaseSiteModel(getCompanySiteResponse.CompanySite);
            var response = this.DatabaseSyncService.SynchronizeDatabaseUsers(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult SynchronizeDatabaseUsersStatus(BaseSyncStatusRequest request)
        {
            var companySiteId = SecurityModels.CustomPrincipal.GetBaseSecurityModel().CompanySiteId ?? Guid.Empty;
            var getCompanySiteResponse = CompanyService.GetCompanySite(new GetCompanySiteRequest { CompanySiteId = companySiteId });
            if (!getCompanySiteResponse.IsSuccess)
                return Json(getCompanySiteResponse);

            request.BaseSite = GetBaseSiteModel(getCompanySiteResponse.CompanySite);
            var response = this.DatabaseSyncService.SynchronizeDatabaseUsersStatus(request);
            return Json(response);
        }

        #endregion
    }
}