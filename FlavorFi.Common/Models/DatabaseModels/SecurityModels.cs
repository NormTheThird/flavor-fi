using System;
using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.DatabaseModels
{
    [DataContract]
    public class SecurityModel
    {
        [DataMember(IsRequired = true)] public Guid Id { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public Guid? CompanyId { get; set; } = null;
        [DataMember(IsRequired = true)] public Guid? CompanySiteId { get; set; } = null;
        [DataMember(IsRequired = true)] public string FirstName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string LastName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Email { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public bool IsCompanyAdmin { get; set; } = false;
        [DataMember(IsRequired = true)] public bool IsSystemAdmin { get; set; } = false;
    }
}