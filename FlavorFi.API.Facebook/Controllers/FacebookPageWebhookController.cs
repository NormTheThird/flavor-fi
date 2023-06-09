using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web;
using Newtonsoft.Json.Linq;

namespace FlavorFi.API.Facebook.Controllers
{
    [RoutePrefix("api/Webhook")]
    public class FacebookPageWebhookController : BaseController
    {
        [HttpGet]
        [Route("Facebook_Page")]
        public HttpResponseMessage Facebook_Page()
        {
            if (!this.ValidateWebhook(HttpContext.Current.Request))
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(HttpContext.Current.Request.QueryString["hub.challenge"]);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            return response;
        }

        [HttpPost]
        [Route("Facebook_Page")]
        public HttpResponseMessage Facebook_Page([FromBody]JObject jsonRequest)
        {
            var request = HttpContext.Current.Request;
            System.Threading.Tasks.Task.Factory.StartNew(() => this.ProcessEntry(jsonRequest, request));
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}