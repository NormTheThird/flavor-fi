using System;
using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.DatabaseModels
{
    [DataContract]
    public class AccountModel : ActiveModel
    {
        [DataMember(IsRequired = true)] public Guid CompanyId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public Guid CompanySiteId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public Guid AddressId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public AddressModel Address { get; set; } = new AddressModel();
        [DataMember(IsRequired = true)] public Guid? AWSProfileImageId { get; set; } = null;
        [DataMember(IsRequired = true)] public string FirstName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string LastName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Email { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string PhoneNumber { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string AltPhoneNumber { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string AllowedOrigin { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public int RefreshTokenLifeTimeMinutes { get; set; } = 0;
        [DataMember(IsRequired = true)] public bool IsCompanyAdmin { get; set; } = false;
        [DataMember(IsRequired = true)] public DateTimeOffset? DateOfBirth { get; set; } = null;
    }

    [DataContract]
    public class AdminAccountListModel : ActiveModel
    {
        [DataMember(IsRequired = true)] public Guid CompanyId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public Guid? AWSProfileImageId { get; set; } = null;
        [DataMember(IsRequired = true)] public string FullName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string CompanyName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Email { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string PhoneNumber { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string AltPhoneNumber { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public bool IsCompanyAdmin { get; set; } = false;
    }

    [DataContract]
    public class AccountActivityModel : BaseModel
    {
        [DataMember(IsRequired = true)] public Guid AccountId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public string ActivityType { get; set; } = string.Empty;
    }
}