using ShopifySharp;
using ShopifySharp.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Admin.Console.Services
{
    public class ProductService 
    {
        public ShopifyProductService ShopifyProductService { get; set; }

        public ProductService(string url, string password)
        {
            try
            {
                this.ShopifyProductService = new ShopifyProductService(url, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ShopifyProduct GetProduct(long _id)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyProductService.GetAsync(_id));
                return task.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ShopifyProduct> GetProducts()
        {
            try
            {     
                var task = Task.Run(async () => await this.ShopifyProductService.ListAsync());
                return task.Result.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ShopifyProduct> GetProductByTitle(string _title)
        {
            try
            {
                var filters = new ShopifyProductFilter { Title = _title.Trim() };
                var task = Task.Run(async () => await this.ShopifyProductService.ListAsync(filters));
                return task.Result.ToList();
            }
            catch (Exception ex)
            {
                return new List<ShopifyProduct>();
            }
        }
        public ShopifyProduct AddProduct(ShopifyProduct _product)
        {
            try
            {
                // By default, creating a product will publish it. To create an unpublished product
                var task = Task.Run(async () => await this.ShopifyProductService.CreateAsync(_product));
                return task.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ShopifyProduct UpdateProduct(ShopifyProduct _product)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyProductService.UpdateAsync(_product));
                return task.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool DeleteProduct(long _id)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyProductService.DeleteAsync(_id));
                return task.IsFaulted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetNumberOfProducts()
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyProductService.CountAsync());
                return task.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}