using FlavorFi.Common.Models.DatabaseModels;
using System.Collections.Generic;

namespace FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses
{
    public class SaveShopifyCustomerRequest : BaseRequest
    {
        public SaveShopifyCustomerRequest()
        {
            this.Customer = new ShopifyCustomerModel();
        }

        public ShopifyCustomerModel Customer { get; set; }
    }

    public class SaveShopifyCustomerResponse : BaseResponse { }

    public class SaveShopifyCustomersRequest : BaseRequest
    {
        public SaveShopifyCustomersRequest()
        {
            this.Customers = new List<ShopifyCustomerModel>();
        }

        public List<ShopifyCustomerModel> Customers { get; set; }
    }

    public class SaveShopifyCustomersResponse : BaseResponse { }
}