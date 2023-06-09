using FlavorFi.Common.Enums;
using FlavorFi.Common.Helpers;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Data;
using System;
using System.Linq;

namespace FlavorFi.Services.DatabaseServices
{
    public interface IShopifyCustomerService
    {
        SaveShopifyCustomerResponse SaveShopifyCustomer(SaveShopifyCustomerRequest request);
        SaveShopifyCustomersResponse SaveCustomers(SaveShopifyCustomersRequest request);
    }

    public class ShopifyCustomerService : BaseService, IShopifyCustomerService
    {
        /// <summary>
        ///     Adds and updates a customer from shopify to the database
        /// </summary>
        /// <param name="request">SaveCustomerRequest</param>
        /// <returns>SaveCustomerResponse</returns>
        public SaveShopifyCustomerResponse SaveShopifyCustomer(SaveShopifyCustomerRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var response = new SaveShopifyCustomerResponse();
                using (var context = new FlavorFiEntities())
                {
                    var customer = context.ShopifyCustomers.FirstOrDefault(c => c.OriginalShopifyId.Equals(request.Customer.OriginalShopifyId));
                    if (customer == null)
                    {
                        customer = new ShopifyCustomer
                        {
                            Id = Guid.NewGuid(),
                            CompanySiteId = request.CompanySiteId,
                            OriginalShopifyId = request.Customer.OriginalShopifyId,
                            DateCreated = now
                        };
                        context.ShopifyCustomers.Add(customer);
                    }
                    MapperService.Map(request.Customer, customer);

                    foreach (var address in request.Customer.Addresses)
                    {
                        //var _address = context.ShopifyAddresses.FirstOrDefault(a => a.OriginalShopifyId.Equals(address.OriginalShopifyId));
                        //if (_address == null)
                        //{
                        //    _address = new ShopifyAddress
                        //    {
                        //        Id = Guid.NewGuid(),
                        //        CompanySiteId = customer.CompanySiteId,
                        //        ShopifyCustomerId = customer.Id,
                        //        OriginalShopifyId = address.OriginalShopifyId,
                        //        OriginalShopifyCustomerId = address.OriginalShopifyCustomerId,
                        //        DateCreated = address.DateCreated,
                        //    };
                        //    context.ShopifyAddresses.Add(_address);
                        //}
                        //MapperService.Map(address, _address);
                    }

                    context.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SaveShopifyCustomerResponse { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        ///     Adds and updates all customers from shopify to the database
        /// </summary>
        /// <param name="request">SaveCustomersRequest</param>
        /// <returns>SaveCustomersResponse</returns>
        public SaveShopifyCustomersResponse SaveCustomers(SaveShopifyCustomersRequest request)
        {
            try
            {
                var response = new SaveShopifyCustomersResponse();
                using (var context = new FlavorFiEntities())
                {

                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SaveShopifyCustomersResponse { ErrorMessage = ex.Message };
            }
        }
    }
}