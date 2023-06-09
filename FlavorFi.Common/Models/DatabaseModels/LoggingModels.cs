using System;
using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.DatabaseModels
{
    [DataContract]
    public class ErrorLogModel : BaseModel
    {
        [DataMember(IsRequired = true)] public int HResult { get; set; } = 0;
        [DataMember(IsRequired = true)] public string Source { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string ExceptionType { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string ExceptionMessage { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string InnerExceptionMessage { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string StackTrace { get; set; } = string.Empty;
    }

    [DataContract]
    public class FacebookWebhookLogModel : BaseModel
    {
        [DataMember(IsRequired = true)] public string EntryId { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Entry { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Type { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public int TimeSent { get; set; } = 0;
    }

    [DataContract]
    public class ShopifyWebhookLogModel : BaseModel
    {
        [DataMember(IsRequired = true)] public Guid ShopifyWebhookId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public long RecordId { get; set; } = 0;
        [DataMember(IsRequired = true)] public bool Verified { get; set; } = false;
    }
}