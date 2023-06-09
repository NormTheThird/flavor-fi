using Newtonsoft.Json;
using System;

namespace FlavorFi.Common.Models.ShopifyModels
{
    [JsonObject(MemberSerialization.OptIn, Title = "custom_collections")]
    public class ShopifyCustomCollectionModel
    {
        [JsonProperty(PropertyName = "admin_graphql_api_id")] public string AdminGraphqlApiId { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "body_html")] public string BodyHtml { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "handle")] public string Handle { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "image")] public string Image { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "id")] public long Id { get; set; } = 0;
        [JsonProperty(PropertyName = "published")] public bool Published { get; set; } = false;
        [JsonProperty(PropertyName = "published_at")] public DateTimeOffset? PublishedAt { get; set; } = null;
        [JsonProperty(PropertyName = "published_scope")] public string PublishedScope { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "sort_order")] public string SortOrder { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "template_suffix")] public string TemplateSuffix { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "title")] public string Title { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "updated_at")] public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.MinValue;
    }

    [JsonObject(MemberSerialization.OptIn, Title = "collect")]
    public class ShopifyCollectModel
    {
        [JsonProperty(PropertyName = "published")] public long CollectionId { get; set; } = 0;
        [JsonProperty(PropertyName = "created_at")] public DateTimeOffset CraetedAt { get; set; } = DateTimeOffset.MinValue;
        [JsonProperty(PropertyName = "featured")] public bool Featured { get; set; } = false;
        [JsonProperty(PropertyName = "id")] public long Id { get; set; } = 0;
        [JsonProperty(PropertyName = "position")] public int Position { get; set; } = 0;
        [JsonProperty(PropertyName = "product_id")] public long ProductId { get; set; } = 0;
        [JsonProperty(PropertyName = "sort_value")] public string SortValue { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "updated_at")] public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.MinValue;
    }

    [JsonObject(MemberSerialization.OptIn, Title = "image")]
    public class ShopifyImageModel
    {
        [JsonProperty(PropertyName = "src")] public string Src { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "alt")] public string Alt { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "width")] public int Width { get; set; } = 0;
        [JsonProperty(PropertyName = "height")] public int Height { get; set; } = 0;
        [JsonProperty(PropertyName = "created_at")] public DateTimeOffset CraetedAt { get; set; } = DateTimeOffset.MinValue;
    }
}