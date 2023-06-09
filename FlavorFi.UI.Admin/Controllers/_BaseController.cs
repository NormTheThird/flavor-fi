using FlavorFi.Common.Models.DatabaseModels;
using FlavorFi.Common.Models.ShopifyModels;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using System;
using System.Web.Mvc;

namespace FlavorFi.UI.Admin.Controllers
{
    public class BaseController : Controller
    {
        public Services.DatabaseServices.ICompanyService CompanyService { get; private set; }
        public Services.DatabaseServices.ILoggingService LoggingService { get; private set; }
        public Services.DatabaseServices.IMessagingService MessagingService { get; private set; }
        public Services.DatabaseServices.IStandaloneApplicationService StandaloneApplicationService { get; private set; }

        public Services.DatabaseServices.IShopifyOrderService DatabaseShopifyOrderService { get; private set; }
        public Services.DatabaseServices.IShopifyWebhookService DatabaseShopifyWebhookService { get; }

        public Services.ShopifyServices.IShopifyOrderService ShopifyOrderService { get; private set; }
        public Services.ShopifyServices.IShopifyWebhookService ShopifyWebhookService { get; }

        public BaseController()
        {
            this.CompanyService = new Services.DatabaseServices.CompanyService();
            this.LoggingService = new Services.DatabaseServices.LoggingService();
            this.MessagingService = new Services.DatabaseServices.MessagingService();
            this.StandaloneApplicationService = new Services.DatabaseServices.StandaloneApplicationService();

            this.DatabaseShopifyOrderService = new Services.DatabaseServices.ShopifyOrderService();
            this.DatabaseShopifyWebhookService = new Services.DatabaseServices.ShopifyWebhookService();

            this.ShopifyOrderService = new Services.ShopifyServices.ShopifyOrderService();
            this.ShopifyWebhookService = new Services.ShopifyServices.ShopifyWebhookService();
        }

        public ShopifyBaseSiteModel GetShopifyBaseSiteModel(Guid companySiteId)
        {
            var response = this.CompanyService.GetCompanySite(new GetCompanySiteRequest { CompanySiteId = companySiteId });
            return GetShopifyBaseSiteModel(response.CompanySite);
        }

        public ShopifyBaseSiteModel GetShopifyBaseSiteModel(CompanySiteModel companySiteModel)
        {
            if (companySiteModel == null) return new ShopifyBaseSiteModel();
            return new ShopifyBaseSiteModel
            {
                SiteId = companySiteModel.Id,
                BaseUrl = companySiteModel.ShopifyUrl,
                BaseWebhookUrl = companySiteModel.ShopifyWebhookUrl,
                PublicApiKey = companySiteModel.ShopifyApiPublicKey,
                SecretApiKey = companySiteModel.ShopifyApiSecretKey
            };
        }

        public BaseSiteModel GetBaseSiteModel(CompanySiteModel companySiteModel)
        {
            var securityModel = SecurityModels.CustomPrincipal.GetBaseSecurityModel();
            return new BaseSiteModel
            {
                AccountId = securityModel.Id,
                SiteId = companySiteModel.Id,
                BaseUrl = companySiteModel.ShopifyUrl,
                BaseWebhookUrl = companySiteModel.ShopifyWebhookUrl,
                PublicApiKey = companySiteModel.ShopifyApiPublicKey,
                SecretApiKey = companySiteModel.ShopifyApiSecretKey
            };
        }

        public ShopifyBaseSiteModel GetLauriebellesShopifyBaseTestSiteModel()
        {
            return new ShopifyBaseSiteModel
            {
                SiteId = Guid.Parse("97948564-E3E5-4A8D-A20E-676832D0636E"),
                BaseUrl = "https://lauriebellesbtq.myshopify.com",
                BaseWebhookUrl = "https://flavor-fi-admin-test.azurewebsites.net/Webhook/ShopifyWebhook",
                PublicApiKey = "eefdb6adefb5e7d8a75bebfda1de3083",
                SecretApiKey = "351ac38559d6f537ab7e0e48a721b558",
            };
        }
    }
}