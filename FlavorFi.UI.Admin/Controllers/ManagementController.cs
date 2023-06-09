using FlavorFi.Common.Helpers;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Services.DatabaseServices;
using System;
using System.Web.Mvc;

namespace FlavorFi.UI.Controllers
{
    [Authorize]
    public class ManagementController : Controller
    {
        public ActionResult CompanyManagement()
        {
            return View("~/Views/Management/Company/Index.cshtml");
        }

        public ActionResult UserManagement()
        {
            return View("~/Views/Management/User/Index.cshtml");
        }


        // User Management

        [HttpGet]
        public ActionResult GetUsers(GetUsersRequest request)
        {
            request.UserToken = Common.Helpers.Security.ReadEncryptedCookie("UserToken").UserToken;
            var response = UserService.GetUsers(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUser(GetUserRequest request)
        {
            request.UserToken = Common.Helpers.Security.ReadEncryptedCookie("UserToken").UserToken;
            var response = UserService.GetUser(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveUser(SaveUserRequest request)
        {
            request.UserToken = Common.Helpers.Security.ReadEncryptedCookie("UserToken").UserToken;
            var response = UserService.SaveUser(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult SendPasswordResetLink(SendPasswordResetLinkRequest request)
        {
            var resetId = Guid.NewGuid();
            request.UserToken = Common.Helpers.Security.ReadEncryptedCookie("UserToken").UserToken;
            var response = AccountService.PasswordReset(new PasswordResetRequest { Email = request.Email, ResetId = resetId });
            if (response.IsSuccess)
            {
                var url = Request.Url.GetLeftPart(UriPartial.Authority).Trim() + "/Account/ResetPassword?id=" + resetId.ToString();
                var emailResponse = MessagingService.SendResetEmail(new SendResetEmailRequest { Recipient = request.Email, Url = Bitly.Shorten(url) });
                if (!emailResponse.IsSuccess) return Json(emailResponse);
            }
            return Json(response);
        }

        [HttpPost]
        public ActionResult SaveCompany ( SaveCompanyRequest request)
        {
            request.UserToken = Common.Helpers.Security.ReadEncryptedCookie("UserToken").UserToken;

            var response = CompanyService.SaveCompany(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}