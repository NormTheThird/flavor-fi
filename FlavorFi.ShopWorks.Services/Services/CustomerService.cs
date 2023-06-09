using FlavorFi.Common.Models.ShopifyModels;
using FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using FlavorFi.Common.RequestAndResponses.ShopWorksRequestAndResponses;
using FlavorFi.Services.ShopifyServices;
using FlavorFi.Services.ShopWorksServices;
using System;

namespace FlavorFi.ShopWorks.Services
{
    public class CustomerService : BaseService
    {
        private ShopWorksCustomerService ShopWorksCustomerService { get; set; }

        public CustomerService(Guid companySiteId) : base(companySiteId)
        {
            this.ShopWorksCustomerService = new ShopWorksCustomerService(this.CompanySite.CompanyId);
        }

        public void SyncCustomers()
        {
            try
            {
                this.ConsoleLog("Getting customers from shopworks", true);
                var getShopWorksCustomersResponse = this.ShopWorksCustomerService.GetShopWorksCustomers(new GetShopWorksCustomersRequest());
                if (!getShopWorksCustomersResponse.IsSuccess) throw new ApplicationException("[Get Customers Error: " + getShopWorksCustomersResponse.ErrorMessage + "]");

                this.ConsoleLog("Adding / updating " + getShopWorksCustomersResponse.Customers.Count + " customers from shopworks", true);
                foreach (var customer in getShopWorksCustomersResponse.Customers)
                    this.AddOrEditCustomer(customer);
                this.ConsoleLog("Finished Getting customers from shopworks", true);
                this.WriteFile("ShopWorks_SyncCustomers");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AddOrEditCustomer(ShopifyCreateCustomerModel customer)
        {
            try
            {
                if (string.IsNullOrEmpty(customer.Email.Trim())) return;
                if (!customer.Email.Contains("@")) return;

                var getCustomerRequest = new GetShopifyRecordByQueryRequest { CompanySiteId = this.CompanySite.Id };
                if (string.IsNullOrEmpty(customer.Email)) throw new ApplicationException("Customer email does not exist");
                else
                {
                    //getCustomerRequest.SearchFields.Add("email", customer.Email);
                    //var getCustomerResponse = ShopifyCustomerService.GetShopifyCustomerByQuery(getCustomerRequest);
                    //if (getCustomerResponse.IsSuccess && getCustomerResponse.Customer != null)
                    //    customer.Id = getCustomerResponse.Customer.Id;
                    //else
                    //    customer.SendEmailInvite = true;

                    var saveShopifyCustomerRequest = new SaveShopifyCustomerRequest { Customer = customer, CompanySiteId = this.CompanySite.Id };
                    var saveShopifyCustomerResponse = ShopifyCustomerService.SaveShopifyCustomer(saveShopifyCustomerRequest);
                    if (!saveShopifyCustomerResponse.IsSuccess) ConsoleLog("[Customer: " + customer.Email + "][Customer Error: " + saveShopifyCustomerResponse.ErrorMessage + "]");
                    else
                    {
                        ConsoleLog("[Customer: " + customer.Email + "][Success]");
                        customer.Id = saveShopifyCustomerResponse.Customer.Id;
                        foreach (var metafield in customer.Metafields)
                        {
                            var saveShopifyCustomerMetafieldRequest = new SaveShopifyCustomerMetafieldRequest { CustomerId = customer.Id, CustomerMetafield = metafield, CompanySiteId = this.CompanySite.Id };
                            var saveShopifyCustomerMetafieldResponse = ShopifyCustomerService.SaveShopifyCustomerMetafield(saveShopifyCustomerMetafieldRequest);
                            if (saveShopifyCustomerMetafieldResponse.IsSuccess) this.ConsoleLog("-- Customer metafield " + metafield.Key + " : " + metafield.Value + " saved successfully");
                            else this.ConsoleLog("-- Customer metafield " + metafield.Key + " : " + metafield.Value + " error: " + saveShopifyCustomerMetafieldResponse.ErrorMessage);
                        }
                    }
                }
            }
            catch (Exception ex) { this.ConsoleLog("[SaveCollect][Error: " + ex.Message + "]"); }
        }
    }
}
