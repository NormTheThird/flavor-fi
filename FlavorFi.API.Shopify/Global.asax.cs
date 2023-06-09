using System.Web;
using System.Web.Http;

namespace FlavorFi.API.Shopify
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
