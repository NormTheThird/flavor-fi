using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.ShopifyModels
{
    [DataContract]
    public class ShopifyUserModel
    {
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "account_owner")] public bool AccountOwner { get; set; } = false;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "bio")] public string Bio { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "email")] public string Email { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "first_name")] public string FirstName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "id")] public long Id { get; set; } = 0;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "im")] public string IM { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "last_name")] public string LastName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "permissions")] public List<string> Permissions { get; set; } = new List<string>();
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "phone")] public string PhoneNumber { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "receive_announcements")] public int ReceiveAnnouncements { get; set; } = 0;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "screen_name")] public string ScreenName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "url")] public string Url { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "locale")] public string Locale { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] [JsonProperty(PropertyName = "user_type")] public string UserType { get; set; } = string.Empty;
    }
}