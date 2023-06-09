using FlavorFi.Common.Models.ShopifyModels;
using System.Collections.Generic;

namespace FlavorFi.Common.RequestAndResponses.ShopWorksRequestAndResponses
{
    public class GetShopWorksCustomersRequest : ShopWorksBaseRequest { }

    public class GetShopWorksCustomersResponse : ShopWorksBaseResponse
    {
        public GetShopWorksCustomersResponse()
        {
            this.Customers = new List<ShopifyCreateCustomerModel>();
        }

        public List<ShopifyCreateCustomerModel> Customers { get; set; }
    }
}