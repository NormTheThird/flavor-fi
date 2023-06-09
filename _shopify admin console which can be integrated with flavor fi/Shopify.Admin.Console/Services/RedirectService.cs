using ShopifySharp;
using System;
using System.Threading.Tasks;

namespace Shopify.Admin.Console.Services
{
    public class RedirectService
    {
        public ShopifyRedirectService ShopifyRedirectService { get; set; }

        public RedirectService(string url, string password)
        {
            try
            {
                this.ShopifyRedirectService = new ShopifyRedirectService(url, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ShopifyRedirect AddRedirect(string _value, bool _withHtml = false)
        {
            try
            {
                var redirect = new ShopifyRedirect { Path = "/" + _value + ".html", Target = "/products/" + _value};
                var task = Task.Run(async () => await this.ShopifyRedirectService.CreateAsync(redirect));
                return task.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ShopifyRedirect GetRedirects(string _value)
        {
            try
            {
                var redirect = new ShopifyRedirect { Path = "/" + _value, Target = "/products/" + _value };
                var task = Task.Run(async () => await this.ShopifyRedirectService.CreateAsync(redirect));
                return task.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
