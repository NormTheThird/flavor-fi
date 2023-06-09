using Newtonsoft.Json;

namespace FlavorFi.Common.Models.ShopifyModels
{
    public class ShopifyCreateMetafieldModel
    {
        public ShopifyCreateMetafieldModel()
        {
            this.Id = 0;
            this.Namespace = string.Empty;
            this.Key = string.Empty;
            this.Value = string.Empty;
            this.ValueType = string.Empty;
        }

        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }
        [JsonProperty(PropertyName = "namespace")]
        public string Namespace { get; set; }
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
        [JsonProperty(PropertyName = "value_type")]
        public string ValueType { get; set; }
    }

    public class ShopifyMetafieldModel : ShopifyBaseModel
    {

        public ShopifyMetafieldModel()
        {
            this.OwnerId = 0;
            this.Namespace = string.Empty;
            this.Key = string.Empty;
            this.Value = string.Empty;
            this.ValueType = string.Empty;
            this.Description = string.Empty;
            this.OwnerResource = string.Empty;
        }

        public long OwnerId { get; set; }
        public string Namespace { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
        public string Description { get; set; }
        public string OwnerResource { get; set; }
    }
}