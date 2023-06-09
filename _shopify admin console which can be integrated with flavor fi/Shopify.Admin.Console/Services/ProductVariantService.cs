using ShopifySharp;
using ShopifySharp.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Admin.Console.Services
{
    public class ProductVariantService
    {
        public ShopifyProductVariantService ShopifyProductVariantService { get; set; }

        public ProductVariantService(string url, string password)
        {
            try
            {
                this.ShopifyProductVariantService = new ShopifyProductVariantService(url, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ShopifyProductVariant GetProductVariant(long _id)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyProductVariantService.GetAsync(_id));
                return task.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ShopifyProductVariant> GetProductVariants(long _productId)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyProductVariantService.ListAsync(_productId));
                return task.Result.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ShopifyProductVariant AddProductVariant(long _productId, ShopifyProductVariant _productVariant)
        {
            try
            {
                // By default, creating a product will publish it. To create an unpublished product
                var task = Task.Run(async () => await this.ShopifyProductVariantService.CreateAsync(_productId, _productVariant));
                return task.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ShopifyProductVariant UpdateProductVariant(ShopifyProductVariant _productVariant)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyProductVariantService.UpdateAsync(_productVariant));
                return task.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool DeleteProductVariant(long _productId, long _productVariantId)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyProductVariantService.DeleteAsync(_productId, _productVariantId));
                return task.IsFaulted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}