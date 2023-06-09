using Newtonsoft.Json;

namespace FlavorFi.Common.Models.ShopifyModels
{
    public class ShopifyCreateAddressModel
    {
        [JsonProperty(PropertyName = "id")] public long Id { get; set; } = 0;
        [JsonProperty(PropertyName = "first_name")] public string FirstName { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "last_name")] public string LastName { get; set; } = string.Empty;
        //[JsonProperty(PropertyName = "phone")] public string PhoneNumber { get; set; }
        [JsonProperty(PropertyName = "address1")] public string Address1 { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "address2")] public string Address2 { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "city")] public string City { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "province")] public string Province { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "phone")] public string ZipCode { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "zip")] public string Country { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "addresses")] public bool IsDefault { get; set; } = false;
    }

    public class ShopifyAddressModel : ShopifyBaseModel
    {
        public long CustomerId { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address1 { get; set; } = string.Empty;
        public string Address2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string ProvinceCode { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public decimal Latitude { get; set; } = 0.0m;
        public decimal Longitude { get; set; } = 0.0m;
        public bool IsDefault { get; set; } = false;
    }
}