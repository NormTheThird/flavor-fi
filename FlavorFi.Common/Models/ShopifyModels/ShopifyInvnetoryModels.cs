using Newtonsoft.Json;

namespace FlavorFi.Common.Models.ShopifyModels
{
    [JsonObject(MemberSerialization.OptIn, Title = "location")]
    public class ShopifyInvnetoryLocationModel : ShopifyBaseModel
    {
        public ShopifyInvnetoryLocationModel()
        {
            this.Name = string.Empty;
            this.Address1 = string.Empty;
            this.Address2 = string.Empty;
            this.City = string.Empty;
            this.ZipCode = string.Empty;
            this.Province = string.Empty;
            this.Country = string.Empty;
            this.PhoneNumber = string.Empty;
            this.CountryCode = string.Empty;
            this.CountryName = string.Empty;
            this.ProvinceCode = string.Empty;
            this.IsLegacy = false;
            this.IsActive = true;
        }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "address1")]
        public string Address1 { get; set; }
        [JsonProperty(PropertyName = "address2")]
        public string Address2 { get; set; }
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }
        [JsonProperty(PropertyName = "zip")]
        public string ZipCode { get; set; }
        [JsonProperty(PropertyName = "province")]
        public string Province { get; set; }
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }
        [JsonProperty(PropertyName = "phone")]
        public string PhoneNumber { get; set; }
        [JsonProperty(PropertyName = "country_code")]
        public string CountryCode { get; set; }
        [JsonProperty(PropertyName = "country_name")]
        public string CountryName { get; set; }
        [JsonProperty(PropertyName = "province_code")]
        public string ProvinceCode { get; set; }
        [JsonProperty(PropertyName = "legacy")]
        public bool IsLegacy { get; set; }
        [JsonProperty(PropertyName = "active")]
        public bool IsActive { get; set; }
    }
}