using FlavorFi.Common.Enums;
using FlavorFi.Common.Helpers;
using System;
using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.DatabaseModels
{
    [DataContract]
    public class BaseModel
    {
        [DataMember(IsRequired = true)] public Guid Id { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public DateTimeOffset DateCreated { get; set; } = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
    }

    [DataContract]
    public class ActiveModel : BaseModel
    {
        [DataMember(IsRequired = true)] public bool IsActive { get; set; } = false;
    }

    [DataContract]
    public class BaseStatusModel
    {
        [DataMember(IsRequired = true)] public string Message { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public bool IsProcessComplete { get; set; } = false;
    }

    [DataContract]
    public class BaseSiteModel
    {
        [DataMember(IsRequired = true)] public Guid AccountId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public Guid SiteId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public string BaseUrl { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string BaseWebhookUrl { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string PublicApiKey { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string SecretApiKey { get; set; } = string.Empty;
    }
}