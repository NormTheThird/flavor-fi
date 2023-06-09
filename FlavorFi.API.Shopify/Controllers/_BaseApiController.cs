using FlavorFi.Common.Models.DatabaseModels;
using FlavorFi.Common.Models.ShopifyModels;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using DatabaseService = FlavorFi.Services.DatabaseServices;
using ShopifyService = FlavorFi.Services.ShopifyServices;

namespace FlavorFi.API.Shopify.Controllers
{
    public class BaseApiController : ApiController
    {
        public DatabaseService.ICompanyService CompanyService { get; set; }
        public DatabaseService.ILoggingService LoggingService { get; set; }
        public ShopifyService.IShopifyWebhookService ShopifyWebhookService { get; set; }

        public BaseApiController()
        {
            this.CompanyService = new DatabaseService.CompanyService();
            this.LoggingService = new DatabaseService.LoggingService();
            this.ShopifyWebhookService = new ShopifyService.ShopifyWebhookService();
        }

        public (bool IsSuccess, JObject Data, CompanySiteModel companySite)  ProcessShopifyWebhook(HttpRequestMessage request)
        {
            try
            {
                var domain = request.Headers.GetValues("x-shopify-shop-domain").FirstOrDefault();
                var getCompanyResponse = CompanyService.GetCompanySiteByDomain(new GetCompanySiteByDomainRequest { Domain = domain });
                if (!getCompanyResponse.IsSuccess)
                    throw new ApplicationException($"Webhook was unable to get company [Domain: {domain}]");

                var domainCompanySite = getCompanyResponse.CompanySite;
                if (!this.ShopifyWebhookService.Validate(request, domainCompanySite.ShopifySharedSecret))
                    throw new ApplicationException($"Webhook was not verified [Domain: {domain}][HMAC: {request.Headers.GetValues("x-shopify-hmac-sha256").FirstOrDefault()}]");

                using (var reader = new StreamReader(request.Content.ReadAsStreamAsync().Result))
                    return (true, JObject.Parse(reader.ReadToEnd()), domainCompanySite);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return (false, JObject.Parse(new JavaScriptSerializer().Serialize(ex)), null);
            }
        }

        public ShopifyBaseSiteModel GetShopifyBaseSiteModel(CompanySiteModel companySiteModel, CompanySiteApplicationModel application)
        {
            return new ShopifyBaseSiteModel
            {
                SiteId = companySiteModel.Id,
                BaseUrl = companySiteModel.ShopifyUrl,
                BaseWebhookUrl = companySiteModel.ShopifyWebhookUrl,
                PublicApiKey = string.IsNullOrEmpty(application.AppApiPublicKey) ? companySiteModel.ShopifyApiPublicKey : application.AppApiPublicKey,
                SecretApiKey = string.IsNullOrEmpty(application.AppApiSecretKey) ? companySiteModel.ShopifyApiSecretKey : application.AppApiSecretKey
            };
        }
    }
}