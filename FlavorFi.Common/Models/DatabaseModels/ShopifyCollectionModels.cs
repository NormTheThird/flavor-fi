using System;
using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.DatabaseModels
{
    [DataContract]
    public class ShopifyCustomCollectionModel : BaseModel
    {
        [DataMember(IsRequired = true)] public long OriginalShopifyId { get; set; } = 0;
        [DataMember(IsRequired = true)] public string Title { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Handle { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string SortOrder { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string AdminGraphqlApiId { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public bool Published { get; set; } = false;
        [DataMember(IsRequired = true)] public DateTimeOffset? PublishedAt { get; set; } = null;
        [DataMember(IsRequired = true)] public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.MinValue;
    }
}