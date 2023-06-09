using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.FacebookModels
{
    [DataContract]
    public class FacebookAccountModel : FacebookBaseModel
    {
        [DataMember(IsRequired = true)] public string Id { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Name { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Email { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Locale { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string UserName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string FirstName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string LastName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Gender { get; set; } = string.Empty;
    }
}