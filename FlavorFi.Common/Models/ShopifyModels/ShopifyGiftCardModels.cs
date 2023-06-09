using Newtonsoft.Json;
using System;

namespace FlavorFi.Common.Models.ShopifyModels
{
    [JsonObject(MemberSerialization.OptIn, Title = "gift_cards")]
    public class ShopifyGiftCardModel
    {
        [JsonProperty(PropertyName = "api_client_id")] public long ApiClientId { get; set; } = 0;
        [JsonProperty(PropertyName = "balance")] public decimal Balance { get; set; } = 0.0m;
        [JsonProperty(PropertyName = "created_at")] public DateTimeOffset CraetedAt { get; set; } = DateTimeOffset.MinValue;
        [JsonProperty(PropertyName = "currency")] public string Currency { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "customer_id")] public long CustomerId { get; set; } = 0;
        [JsonProperty(PropertyName = "disabled_at")] public DateTimeOffset? DisabledAt { get; set; } = null;
        [JsonProperty(PropertyName = "expires_on")] public DateTimeOffset? ExpiresOn { get; set; } = null;
        [JsonProperty(PropertyName = "id")] public long Id { get; set; } = 0;
        [JsonProperty(PropertyName = "initial_value")] public decimal InitialValue { get; set; } = 0.0m;
        [JsonProperty(PropertyName = "last_characters")] public string LastCharacters { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "line_item_id")] public long LineItemId { get; set; } = 0;
        [JsonProperty(PropertyName = "note")] public string Note { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "order_id")] public long OrderId { get; set; } = 0;
        [JsonProperty(PropertyName = "template_suffix")] public string TemplateSuffix { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "user_id")] public long UserId { get; set; } = 0;
        [JsonProperty(PropertyName = "updated_at")] public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.MinValue;
    }
}