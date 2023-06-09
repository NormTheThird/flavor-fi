using System;
using System.Web.Mvc;
using System.Web.Security;
using FlavorFi.UI.Security;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Services.DatabaseServices;

namespace FlavorFi.UI.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index(string page)
        {
            ViewBag.Page = page ?? "";
            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            HttpContext.Session.RemoveAll();
            Response.Cookies["UserToken"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Index", "Account");
        }
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(ValidateAccountRequest request)
        {
            var response = AccountService.ValidateAccount(request);
            if (response.IsSuccess)
            {
                FormsAuthentication.SetAuthCookie(request.Email, request.RememberMe);
                var customPrincipal = new CustomPrincipal(new CustomIdentity(request.Email, request.Password));
                var customIdentity = (CustomIdentity)customPrincipal.Identity;
                Response.Cookies.Add(Common.Helpers.Security.CreateCookie("UserToken", customIdentity.UserToken));
                HttpContext.User = customPrincipal;
            }
            return Json(response);
        }

        [HttpPost]
        public ActionResult Register(RegisterAccountRequest request)
        {
            var response = AccountService.RegisterAccount(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordRequest request)
        {
            var response = AccountService.PasswordReset(new PasswordResetRequest { Email = request.Email });
            if (response.IsSuccess)
            {
                //Send email with new reset ID.
            }
            return Json(response);
        }


        [HttpPost]
        public ActionResult ResetPassword(ValidatePasswordResetRequest request)
        {
            var response = AccountService.ValidatePasswordReset(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateCompanyId(Guid companyId)
        {
            var user = Common.Helpers.Security.DecryptUserToken(Common.Helpers.Security.ReadEncryptedCookie("UserToken").UserToken);
            var response = AccountService.UpdateCompanyId(new UpdateCompanyIdRequest { Account = user, CompanyId = companyId });
            Response.Cookies["UserToken"].Value = response.UserToken;
            return Json(response);
        }
    }
}