using Shopify.Admin.Console.Enums;
using Shopify.Admin.Console.Helpers;
using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Admin.Console.Services
{
    public class MetaFieldService
    {
        public ShopifyMetaFieldService ShopifyMetaFieldService { get; set; }

        public MetaFieldService(string url, string password)
        {
            try
            {
                this.ShopifyMetaFieldService = new ShopifyMetaFieldService(url, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ShopifyMetaField GetMetaField(long _id)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyMetaFieldService.GetAsync(_id));
                return task.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ShopifyMetaField> GetMetaFields(long _resourceId, MetaFieldResourceType _resourceType)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyMetaFieldService.ListAsync(_resourceId, _resourceType.GetDescription()));
                return task.Result.ToList();
            }
            catch (Exception ex)
            {
                return new List<ShopifyMetaField>();
            }
        }
        public ShopifyMetaField AddMetaField(ShopifyMetaField _metaField, long _resourceId, MetaFieldResourceType _resourceType)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyMetaFieldService.CreateAsync(_metaField, _resourceId, _resourceType.GetDescription()));
                return task.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool DeleteMetaField(long _id)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyMetaFieldService.DeleteAsync(_id));
                return task.IsFaulted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}