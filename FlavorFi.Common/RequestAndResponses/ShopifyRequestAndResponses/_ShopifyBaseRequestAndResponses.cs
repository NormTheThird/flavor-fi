using FlavorFi.Common.Models.ShopifyModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses
{
    [DataContract]
    public class ShopifyBaseRequest
    {
        [JsonIgnore] [DataMember(IsRequired = true)] public ShopifyBaseSiteModel BaseSite { get; set; } = new ShopifyBaseSiteModel();
        [JsonIgnore] [DataMember(IsRequired = true)] public Dictionary<string, string> Parameters { get; set; } = null;
    }

    [DataContract]
    public class ShopifyBaseResponse
    {
        [DataMember(IsRequired = true)] public bool IsSuccess { get; set; } = false;
        [DataMember(IsRequired = true)] public string ErrorMessage { get; set; } = string.Empty;
    }

    [DataContract]
    public class SaveShopifyRecordRequest : ShopifyBaseRequest
    {
        [JsonIgnore] [DataMember(IsRequired = true)] public long RecordId { get; set; } = 0;
        [JsonIgnore] [DataMember(IsRequired = true)] public string ExtendedUrl { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string PostData { get; set; } = null;
    }

    [DataContract]
    public class SaveShopifyRecordResponse : ShopifyBaseResponse
    {
        [DataMember(IsRequired = true)] public JObject Data { get; set; } = null;
    }

    [DataContract]
    public class DeleteShopifyRecordRequest : ShopifyBaseRequest
    {
        [DataMember(IsRequired = true)] public long RecordId { get; set; } = 0;
    }

    [DataContract]
    public class GetShopifyRecordRequest : ShopifyBaseRequest
    {
        [DataMember(IsRequired = true)] public long RecordId { get; set; } = 0;
    }

    [DataContract]
    public class GetShopifyRecordByQueryRequest : ShopifyBaseRequest
    {
        [DataMember(IsRequired = true)] public bool GroupItems { get; set; } = false;
    }

    [DataContract]
    public class GetShopifyRecordResponse : ShopifyBaseResponse
    {
        [DataMember(IsRequired = true)] public JObject Data { get; set; } = null;
    }

    [DataContract]
    public class GetShopifyRecordsRequest : ShopifyBaseRequest { }

    [DataContract]
    public class GetShopifyRecordsPerPageRequest : GetShopifyRecordsRequest { }

    [DataContract]
    public class GetShopifyRecordsResponse : ShopifyBaseResponse
    {
        [DataMember(IsRequired = true)] public JObject Data { get; set; } = null;
    }

    [DataContract]
    public class GetShopifyRecordCountResponse : ShopifyBaseResponse
    {
        [DataMember(IsRequired = true)] public int Count { get; set; } = 0;
    }
}