using ShopifySharp;
using ShopifySharp.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Admin.Console.Services
{
    public class CollectService
    {
        public ShopifyCollectService ShopifyCollectService { get; set; }

        public CollectService(string url, string password)
        {
            try
            {
                this.ShopifyCollectService = new ShopifyCollectService(url, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ShopifyCollect GetCollect(long _id)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyCollectService.GetAsync(_id));
                return task.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ShopifyCollect> GetCollects(long _productId)
        {
            try
            {
                var filters = new ShopifyCollectFilter { ProductId = _productId };
                var task = Task.Run(async () => await this.ShopifyCollectService.ListAsync(filters));
                return task.Result.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ShopifyCollect AddCollect(ShopifyCollect _collect)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyCollectService.CreateAsync(_collect));
                return task.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool DeleteCollect(long _id)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyCollectService.DeleteAsync(_id));
                return task.IsFaulted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}