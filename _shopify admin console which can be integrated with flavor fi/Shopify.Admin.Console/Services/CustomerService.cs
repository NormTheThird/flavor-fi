using ShopifySharp;
using ShopifySharp.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Admin.Console.Services
{
    public class CustomerService
    {
        public ShopifyCustomerService ShopifyCustomerService { get; set; }

        public CustomerService(string url, string password)
        {
            try
            {
                this.ShopifyCustomerService = new ShopifyCustomerService(url, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ShopifyCustomer GetCustomer(long _id)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyCustomerService.GetAsync(_id));
                return task.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ShopifyCustomer> GetCustomers()
        {
            try
            {

                var task = Task.Run(async () => await this.ShopifyCustomerService.ListAsync());
                return task.Result.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ShopifyCustomer AddCustomer(ShopifyCustomer _customer)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyCustomerService.CreateAsync(_customer));
                return task.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ShopifyCustomer UpdateCustomer(ShopifyCustomer _customer)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyCustomerService.UpdateAsync(_customer));
                return task.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ShopifyCustomer GetCustomerWithEmail(string firstName, string _email)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyCustomerService.SearchAsync(firstName + " email:" + _email));
                return task.Result.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public int GetActiveCustomerCount()
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyCustomerService.SearchAsync("state:enabled"));
                var result = task.Result;
                return result.Count();
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public bool DeleteCollect(long _id)
        {
            try
            {
                var task = Task.Run(async () => await this.ShopifyCustomerService.DeleteAsync(_id));
                return task.IsFaulted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}