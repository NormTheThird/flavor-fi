using Newtonsoft.Json;
using System.Collections.Generic;

namespace FlavorFi.Common.Models.ShopifyModels
{
    public class ShopifyCreateCustomerModel
    {
        [JsonProperty(PropertyName = "id")] public long Id { get; set; } = 0;
        [JsonProperty(PropertyName = "first_name")] public string FirstName { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "last_name")] public string LastName { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "email")] public string Email { get; set; } = string.Empty;
        [JsonIgnore] public string PhoneNumber { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "verified_email")] public bool HasVerifiedEmail { get; set; } = false;
        [JsonProperty(PropertyName = "send_email_invite")] public bool SendEmailInvite { get; set; } = false;
        [JsonIgnore] public List<ShopifyCreateAddressModel> Addresses { get; set; } = new List<ShopifyCreateAddressModel>();
        [JsonIgnore] public List<ShopifyCreateMetafieldModel> Metafields { get; set; } = new List<ShopifyCreateMetafieldModel>();
    }

    public class ShopifyCustomerBaseModel : ShopifyBaseModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class ShopifyCustomerModel : ShopifyCustomerBaseModel
    {
        public long LastOrderId { get; set; } = 0;
        public string LastOrderName { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string MultipassIdentifier { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public int OrderCount { get; set; } = 0;
        public decimal TotalSpent { get; set; } = 0.0m;
        public bool AcceptsMarketing { get; set; } = false;
        public bool HasVerifiedEmail { get; set; } = false;
        public bool IsTaxExempt { get; set; } = false;
        public List<ShopifyAddressModel> Addresses { get; set; } = new List<ShopifyAddressModel>();
        public ShopifyAddressModel DefaultAddress { get; set; } = new ShopifyAddressModel();
    }
}