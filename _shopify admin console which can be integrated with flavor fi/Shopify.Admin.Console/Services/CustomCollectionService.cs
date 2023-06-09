using ShopifySharp;
using ShopifySharp.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Admin.Console.Services
{
    public class CustomCollectionService
    {
        public ShopifyCustomCollectionService ShopifyCustomCollectionService { get; set; }

        public CustomCollectionService(string url, string password)
        {
            try
            {
                this.ShopifyCustomCollectionService = new ShopifyCustomCollectionService(url, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ShopifyCustomCollection GetCustomCollection(long _id)
        {
            try
            {
                var task = Task.Run(async () => await ShopifyCustomCollectionService.GetAsync(_id));
                return task.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ShopifyCustomCollection> GetCustomCollections()
        {
            try
            {
                var filters = new ShopifyCustomCollectionFilter { Limit = 250 };
                var task = Task.Run(async () => await ShopifyCustomCollectionService.ListAsync(filters));
                return task.Result.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ShopifyCustomCollection AddCustomCollection(ShopifyCustomCollection _customCollection)
        {
            try
            {
                var task = Task.Run(async () => await ShopifyCustomCollectionService.CreateAsync(_customCollection));
                return task.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ShopifyCustomCollection UpdateCustomCollections(ShopifyCustomCollection _customCollection)
        {
            try
            {
                var task = Task.Run(async () => await ShopifyCustomCollectionService.UpdateAsync(_customCollection));
                return task.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool DeleteCustomCollection(long _id)
        {
            try
            {
                var task = Task.Run(async () => await ShopifyCustomCollectionService.DeleteAsync(_id));
                return task.IsFaulted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}