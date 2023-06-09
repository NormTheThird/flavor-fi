using System;
using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.DatabaseModels
{
    [DataContract]
    public class ShopifyUserModel : BaseModel
    {
        [DataMember(IsRequired = true)] public Guid? AccountId { get; set; } = null;
        [DataMember(IsRequired = true)] public long OriginalShopifyId { get; set; } = 0;
        [DataMember(IsRequired = true)] public string FirstName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string LastName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Email { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string PhoneNumber { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string ScreenName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string UserType { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public bool IsAccountOwner { get; set; } = false;
        [DataMember(IsRequired = true)] public bool ReceiveAnnouncements { get; set; } = false;
    }
}