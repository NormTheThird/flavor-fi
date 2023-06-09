using System;
using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.DatabaseModels
{
    [DataContract]
    public class CompanyModel : ActiveModel
    {
        [DataMember(IsRequired = true)] public string Name { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Address1 { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Address2 { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string City { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string State { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string ZipCode { get; set; } = string.Empty;
    }

    [DataContract]
    public class CompanySiteSettingModel : BaseModel
    {
        [DataMember(IsRequired = true)] public string Key { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Value { get; set; } = string.Empty;
    }

    [DataContract]
    public class CompanySiteModel : ActiveModel
    {
        [DataMember(IsRequired = true)] public Guid CompanyId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public string Name { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string ShopifyUrl { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string ShopifyWebhookUrl { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string ShopifyDomain { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string ShopifyApiPublicKey { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string ShopifyApiSecretKey { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string ShopifySharedSecret { get; set; } = string.Empty;
    }

    [DataContract]
    public class CompanySiteApplicationModel : BaseModel
    {
        [DataMember(IsRequired = true)] public Guid CompanySiteId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public Guid ApplicationId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public string ApplicationName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string AppApiPublicKey { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string AppApiSecretKey { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string AppSharedSecret { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Locations { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public bool IsEnabled { get; set; } = false;
    }

    [DataContract]
    public class CompanySiteEmailModel : BaseModel
    {
        [DataMember(IsRequired = true)] public Guid CompanySiteId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public string Category { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Name { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Email { get; set; } = string.Empty;
    }
}