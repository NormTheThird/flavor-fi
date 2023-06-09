using FlavorFi.Common.RequestAndResponses.ShopWorksRequestAndResponses;
using FlavorFi.Services.ShopWorksServices;
using System;

namespace FlavorFi.ShopWorks.Service.Services
{
    public class CustomerService : BaseService
    {
        public void SyncCustomers()
        {
            try
            {
                this.ConsoleLog("Getting customers from shopworks", true);
                var getShopWorksCustomersResponse = ShopWorksCustomerService.GetShopWorksCustomers(new GetShopWorksCustomersRequest());
                if (!getShopWorksCustomersResponse.IsSuccess) Console.WriteLine("[Get Customers Error: " + getShopWorksCustomersResponse.ErrorMessage + "]");
                //foreach (var customer in getShopWorksCustomersResponse.Products)
                //{
                //    var getShopWorksProductVariantsRequest = new GetShopWorksProductVariantsRequest { Product = product };
                //    var getShopWorksProductVariantsResponse = ShopWorksProductService.GetShopWorksProductVariants(getShopWorksProductVariantsRequest);
                //    if (!getShopWorksProductVariantsResponse.IsSuccess) ConsoleLog("[Product: " + product.Title + "][Variant Error: " + getShopWorksProductVariantsResponse.ErrorMessage + "]");
                //    else
                //    {
                //        var _product = getShopWorksProductVariantsResponse.Product;
                //        if (_product.Variants.Count == 0) ConsoleLog("[Product: " + _product.Title + "][Not Imported, No Variants]");
                //        else
                //        {
                //            var saveShopifyProductRequest = new SaveShopifyProductRequest { Product = _product, CompanySiteId = CompanySiteId };
                //            var saveShopifyProductResponse = ShopifyProductService.SaveShopifyProduct(saveShopifyProductRequest);
                //            if (!saveShopifyProductResponse.IsSuccess) ConsoleLog("[Product: " + _product.Title + "][Product Error: " + saveShopifyProductResponse.ErrorMessage + "]");
                //            else ConsoleLog("[Product: " + _product.Title + "][Success]");
                //        }
                //    }
                //}
                this.ConsoleLog("Finished Getting customers from shopworks", true);
                this.WriteFile();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
