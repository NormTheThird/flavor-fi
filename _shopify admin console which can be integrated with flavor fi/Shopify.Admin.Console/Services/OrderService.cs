using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Admin.Console.Services
{
    public class OrderService
    {
        public ShopifyOrderService ShopifyOrderService { get; set; }

        public OrderService(string url, string password)
        {
            try
            {
                this.ShopifyOrderService = new ShopifyOrderService(url, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ShopifyOrder GetOrder(long _id)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyOrderService.GetAsync(_id));
                return task.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ShopifyOrder> GetOrders()
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyOrderService.ListAsync());
                return task.Result.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ShopifyOrder> GetOrdersForCustomer(long _customerId)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyOrderService.ListForCustomerAsync(_customerId));
                return task.Result.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ShopifyOrder AddOrder(ShopifyOrder _order)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyOrderService.CreateAsync(_order));
                return task.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ShopifyOrder UpdateOrder(ShopifyOrder _order)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyOrderService.UpdateAsync(_order));
                return task.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool DeleteOrder(long _id)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyOrderService.DeleteAsync(_id));
                var fault = task.IsFaulted;
                return fault;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}