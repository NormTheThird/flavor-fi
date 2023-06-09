using FlavorFi.Common.Models.ShopifyModels;
using FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using FlavorFi.Common.RequestAndResponses.ShopWorksRequestAndResponses;
using FlavorFi.Services.ShopifyServices;
using FlavorFi.Services.ShopWorksServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlavorFi.ShopWorks.Service.Services
{
    public class ProductService : BaseService
    {
        public void SyncProducts()
        {
            try
            {
                this.ConsoleLog("Getting products from shopworks", true);
                var getShopWorksProductsResponse = ShopWorksProductService.GetShopWorksProducts(new GetShopWorksProductsRequest());
                if (!getShopWorksProductsResponse.IsSuccess) Console.WriteLine("[Get Products Error: " + getShopWorksProductsResponse.ErrorMessage + "]");
                foreach (var product in getShopWorksProductsResponse.Products)
                {
                    var getShopWorksProductVariantsRequest = new GetShopWorksProductVariantsRequest { Product = product };
                    var getShopWorksProductVariantsResponse = ShopWorksProductService.GetShopWorksProductVariants(getShopWorksProductVariantsRequest);
                    if (!getShopWorksProductVariantsResponse.IsSuccess) ConsoleLog("[Product: " + product.Title + "][Variant Error: " + getShopWorksProductVariantsResponse.ErrorMessage + "]");
                    else
                    {
                        var _product = getShopWorksProductVariantsResponse.Product;
                        if (_product.Variants.Count == 0) ConsoleLog("[Product: " + _product.Title + "][Not Imported, No Variants]");
                        else
                        {

                            var getProductRequest = new GetShopifyRecordByQueryRequest { CompanySiteId = CompanySiteId };
                            getProductRequest.SearchFields.Add("title", _product.Title);
                            var getProductResponse = ShopifyProductService.GetShopifyProductByQuery(getProductRequest);
                            if (getProductResponse.IsSuccess && getProductResponse.Product != null)
                            {
                                _product.Id = getProductResponse.Product.Id;
                                _product.Tags += getProductResponse.Product.Tags;
                                foreach (var variant in getProductResponse.Product.Variants)
                                {
                                    if(!string.IsNullOrEmpty(variant.Option1))
                                    {
                                        var option = product.Options.FirstOrDefault(o => o.Name.Equals("Size", StringComparison.CurrentCultureIgnoreCase));
                                        if (option == null) product.Options.Add(new ShopifyOptionModel { Name = "Size", Values = new List<string>() });
                                        if (option.Values.Contains(variant.Option1)) option.Values.Add(variant.Option1);
                                    }
                                    if (!string.IsNullOrEmpty(variant.Option2))
                                    {
                                        var option = product.Options.FirstOrDefault(o => o.Name.Equals("Color", StringComparison.CurrentCultureIgnoreCase));
                                        if (option == null) product.Options.Add(new ShopifyOptionModel { Name = "Color", Values = new List<string>() });
                                        if (option.Values.Contains(variant.Option2))option.Values.Add(variant.Option2);
                                    }

                                    var _variant = _product.Variants.FirstOrDefault(v => v.Size.Equals(variant.Option1, StringComparison.CurrentCultureIgnoreCase) &&
                                                                                         v.Color.Equals(variant.Option2, StringComparison.CurrentCultureIgnoreCase));
                                    if(_variant == null)
                                    {
                                        _variant = new ShopifyCreateProductVariantModel
                                        {
                                            Color = variant.Option2,
                                            Size = variant.Option1,
                                            Sku = variant.Sku,
                                            CompareAtPrice = variant.CompareAtPrice,
                                            Price = variant.Price,
                                            InventoryManagement = variant.InventoryManagement,
                                            InventoryQuantity = variant.InventoryQuantity,
                                            Taxable = variant.Taxable
                                        };
                                        _product.Variants.Add(_variant);
                                    }
                                }
                            }
                            
                            var saveShopifyProductRequest = new SaveShopifyProductRequest { Product = _product, CompanySiteId = CompanySiteId };
                            var saveShopifyProductResponse = ShopifyProductService.SaveShopifyProduct(saveShopifyProductRequest);
                            if (!saveShopifyProductResponse.IsSuccess) ConsoleLog("[Product: " + _product.Title + "][Product Error: " + saveShopifyProductResponse.ErrorMessage + "]");
                            else ConsoleLog("[Product: " + _product.Title + "][Success]");
                        }
                    }
                }
                this.ConsoleLog("Finished Getting products from shopworks", true);
                this.WriteFile();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SyncProduct()
        {
            try
            {
                var partNumber = "550";
                this.ConsoleLog("Getting product " + partNumber + " from shopworks", true);
                var getShopWorksProductResponse = ShopWorksProductService.GetShopWorksProduct(new GetShopWorksProductRequest { PartNumber = partNumber });
                if (!getShopWorksProductResponse.IsSuccess) Console.WriteLine("[Get Product Error: " + getShopWorksProductResponse.ErrorMessage + "]");

                var product = getShopWorksProductResponse.Product;
                var getShopWorksProductVariantsRequest = new GetShopWorksProductVariantsRequest { Product = product };
                var getShopWorksProductVariantsResponse = ShopWorksProductService.GetShopWorksProductVariants(getShopWorksProductVariantsRequest);
                if (!getShopWorksProductVariantsResponse.IsSuccess) ConsoleLog("[Product: " + product.Title + "][Variant Error: " + getShopWorksProductVariantsResponse.ErrorMessage + "]");
                else
                {
                    var _product = getShopWorksProductVariantsResponse.Product;
                    if (_product.Variants.Count == 0) ConsoleLog("[Product: " + _product.Title + "][Not Imported, No Variants]");
                    else
                    {
                        //var saveShopifyProductRequest = new SaveShopifyProductRequest { Product = _product, CompanySiteId = CompanySiteId };
                        //var saveShopifyProductResponse = ShopifyProductService.SaveShopifyProduct(saveShopifyProductRequest);
                        //if (!saveShopifyProductResponse.IsSuccess) ConsoleLog("[Product: " + _product.Title + "][Product Error: " + saveShopifyProductResponse.ErrorMessage + "]");
                        //else ConsoleLog("[Product: " + _product.Title + "][Success]");
                    }
                }
                this.ConsoleLog("Finished Getting product from shopworks", true);
                this.WriteFile();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
