using System;
using System.Web.Mvc;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using FlavorFi.Services.DatabaseServices;
using FlavorFi.Services.ShopifyServices;

namespace FlavorFi.UI.Admin.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {
        public IAccountService AccountService { get; }
        public IModuleService ModuleService { get; }
        public ISecurityService SecurityService { get; }

        public AdminController()
        {
            this.AccountService = new AccountService();
            this.ModuleService = new ModuleService();
            this.SecurityService = new SecurityService();
        }

        public ActionResult Account()
        {
            return View("~/Views/Admin/Account/Index.cshtml");
        }

        public ActionResult Company()
        {
            return View("~/Views/Admin/Company/Index.cshtml");
        }

        public ActionResult Module()
        {
            return View("~/Views/Admin/Module/Index.cshtml");
        }

        #region Account

        [HttpPost]
        public ActionResult GetAdminAccounts(GetAdminAccountsRequest request)
        {
            var response = this.AccountService.GetAdminAccounts(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult GetAccount(GetAccountRequest request)
        {
            var response = this.AccountService.GetAccount(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult ChangeAccountStatus(ChangeAccountStatusRequest request)
        {
            var response = this.AccountService.ChangeAccountStatus(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult SaveNewPassword(SaveNewPasswordRequest request)
        {
            var response = this.SecurityService.SaveNewPassword(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult PasswordReset(PasswordResetRequest request)
        {
            var passwordResetResponse = this.SecurityService.PasswordReset(request);
            if (!passwordResetResponse.IsSuccess)
                return Json(passwordResetResponse);

            var url = $"{request.BaseUrl}/Security/Index?resetId={passwordResetResponse.ResetId}";
            var sendResetEmailRequest = new SendResetEmailRequest { Recipient = request.Email, Url = url };
            var sendResetEmailResponse = this.MessagingService.SendResetEmail(sendResetEmailRequest);
            return Json(sendResetEmailResponse);
        }

        #endregion

        #region Companies

        [HttpPost]
        public ActionResult GetCompanies(GetCompaniesRequest request)
        {
            var response = this.CompanyService.GetCompanies(request);
            return Json(response);
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
            var response = this.CompanyService.GetCompanySites(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult SaveCompany(SaveCompanyRequest request)
        {
            var response = this.CompanyService.SaveCompany(request);
            return Json(response);
        }

        #endregion

        #region Modules

        [HttpPost]
        public ActionResult GetModules(GetModulesRequest request)
        {
            var response = this.ModuleService.GetModules(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult ChangeModuleStatus(ChangeModuleStatusRequest request)
        {
            var response = this.ModuleService.ChangeModuleStatus(request);
            return Json(response);
        }

        #endregion
    }
}