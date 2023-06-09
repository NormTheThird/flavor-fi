using FlavorFi.Common.Models.DatabaseModels;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Services.DatabaseServices;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using System.IO;

namespace FlavorFi.API.Facebook.Controllers
{
    public class BaseController : ApiController
    {
        public ILoggingService LoggingService { get; set; }

        public BaseController()
        {
            LoggingService = new LoggingService();
        }

        internal void ProcessEntry(JObject jsonRequest, HttpRequest request)
        {
            try
            {
                if (!ValidateRequest(request))
                    return;

                var logFacebookWebhookModel = new FacebookWebhookLogModel
                {
                    EntryId = jsonRequest["entry"][0]["id"].ToString(),
                    Entry = jsonRequest["entry"][0]["changes"][0]["field"].ToString(),
                    Type = jsonRequest["object"].ToString(),
                    TimeSent = Convert.ToInt32(jsonRequest["entry"][0]["time"])
                };

                var response = new LoggingService().LogFacebookWebhook(new LogFacebookWebhookRequest { CompanySiteId = Guid.Parse("97948564-E3E5-4A8D-A20E-676832D0636E"),  FacebookWebhookLog = logFacebookWebhookModel });
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
            }

        }

        internal bool ValidateWebhook(HttpRequest request)
        {
            try
            {
                var token = HttpContext.Current.Request.QueryString["hub.verify_token"];
                return token == "cZlpSwbdqXSL0XcWH";
            }
            catch (Exception) { return false; }
        }

        private static bool ValidateRequest(HttpRequest request)
        {
            try
            {
                var expectedSignature = request.Headers["X-Hub-Signature"].Remove(0, 5);
                var secret = "4d012a3e30403e3c49e6ab14a9eb0c3a";
                using (var reader = new StreamReader(request.InputStream))
                {
                    var payload = reader.ReadToEnd();
                    var calculatedSignature = CalculateSignature(secret, payload);
                    return calculatedSignature == expectedSignature;
                }
            }
            catch (Exception) { return false; }
        }

        private static string CalculateSignature(string appSecret, string payload)
        {
            payload = EncodeNonAsciiCharacters(payload);
            var secretKey = Encoding.UTF8.GetBytes(appSecret);
            var hmac = new HMACSHA1(secretKey);
            hmac.Initialize();
            var bytes = Encoding.UTF8.GetBytes(payload);
            var rawHmac = hmac.ComputeHash(bytes);
            return BitConverter.ToString(rawHmac).Replace("-", "").ToLower();
        }

        private static string EncodeNonAsciiCharacters(string value)
        {
            var sb = new StringBuilder();
            foreach (char c in value)
            {
                if (c > 127)
                {
                    var encodedValue = "\\u" + ((int)c).ToString("x4");
                    sb.Append(encodedValue);
                }
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }
    }
}