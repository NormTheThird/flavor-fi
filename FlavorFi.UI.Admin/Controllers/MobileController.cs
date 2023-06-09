using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Services.DatabaseServices;
using System.Web.Mvc;

namespace FlavorFi.UI.Controllers
{
    public class MobileController : Controller
    {
        public ActionResult PushNotifications()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetPushNotifications(GetPushNotificationsRequest request)
        {
            request.UserToken = Common.Helpers.Security.ReadEncryptedCookie("UserToken").UserToken;
            var response = MobileService.GetPushNotifications(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SendPushNotificationAsync(SendPushNotificationAsyncRequest request)
        {
            request.UserToken = Common.Helpers.Security.ReadEncryptedCookie("UserToken").UserToken;
            var response = MobileService.SendPushNotificationAsync(request);
            return Json(response);
        }

        [HttpPost]
        public ActionResult GetSendPushNotificationStatus(GetSendPushNotificationStatusRequest request)
        {
            request.UserToken = Common.Helpers.Security.ReadEncryptedCookie("UserToken").UserToken;
            var response = MobileService.GetGetSendPushNotificationStatus(request);
            return Json(response);
        }
    }
}