using FlavorFi.Common.Models.ShopifyModels;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses
{
    public class GetShopifyCustomerResponse : ShopifyBaseResponse
    {
        public GetShopifyCustomerResponse()
        {
            this.Customer = new ShopifyCustomerModel();
        }

        public ShopifyCustomerModel Customer { get; set; }
    }

    public class GetShopifyCustomersResponse : ShopifyBaseResponse
    {
        public GetShopifyCustomersResponse()
        {
            this.Customers = new List<ShopifyCustomerModel>();
        }

        public List<ShopifyCustomerModel> Customers { get; set; }
    }

    public class GetShopifyCustomerMetafiledsResponse : ShopifyBaseResponse
    {
        public GetShopifyCustomerMetafiledsResponse()
        {
            this.Metafields = new List<ShopifyMetafieldModel>();
        }

        public List<ShopifyMetafieldModel> Metafields { get; set; }
    }

    public class SaveShopifyCustomerRequest : ShopifyBaseRequest
    {
        public SaveShopifyCustomerRequest()
        {
            this.Customer = new ShopifyCreateCustomerModel();
        }

        [JsonProperty(PropertyName = "customer")]
        public ShopifyCreateCustomerModel Customer { get; set; }
    }

    public class SaveShopifyCustomerResponse : ShopifyBaseResponse
    {
        public SaveShopifyCustomerResponse()
        {
            this.Customer = new ShopifyCustomerModel();
        }

        public ShopifyCustomerModel Customer { get; set; }
    }

    public class SaveShopifyCustomerMetafieldRequest : ShopifyBaseRequest
    {
        public SaveShopifyCustomerMetafieldRequest()
        {
            this.CustomerMetafield = new ShopifyCreateMetafieldModel();
            this.CustomerId = 0;
        }

        public long CustomerId { get; set; }
        [JsonProperty(PropertyName = "metafield")]
        public ShopifyCreateMetafieldModel CustomerMetafield { get; set; }
    }

    public class SaveShopifyCustomerMetafieldResponse : ShopifyBaseResponse
    {
        public SaveShopifyCustomerMetafieldResponse()
        {
            this.CustomerMetafield = new ShopifyMetafieldModel();
        }

        public ShopifyMetafieldModel CustomerMetafield { get; set; }
    }
}