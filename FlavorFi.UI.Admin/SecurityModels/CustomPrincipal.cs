using System;
using System.Security.Principal;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using FlavorFi.Common.Models.DatabaseModels;

namespace FlavorFi.UI.Admin.SecurityModels
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }

        public CustomPrincipal(CustomIdentity identity)
        {
            Identity = identity;
        }

        public bool IsInRole(string role)
        {
            return false;
        }

        public static bool IsSystemAdmin()=> GetBaseSecurityModel().IsSystemAdmin;

        public static bool IsCompanyAdmin() => GetBaseSecurityModel().IsCompanyAdmin;

        public static bool IsAssignedToCompany() => GetBaseSecurityModel().CompanyId != null;

        public static SecurityModel GetBaseSecurityModel()
        {
            var formsAuthCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (string.IsNullOrEmpty(formsAuthCookie?.Value))
                return new SecurityModel();

            var formsAuthTicket = FormsAuthentication.Decrypt(formsAuthCookie.Value);
            return formsAuthTicket == null
                ? new SecurityModel()
                : new JavaScriptSerializer().Deserialize<SecurityModel>(formsAuthTicket.UserData);
        }
    }
}