using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.FacebookModels
{
    [DataContract]
    public class SubscribedAppModel
    {
        [DataMember(IsRequired = true)] public string Id { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Category { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Link { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Name { get; set; } = string.Empty;
        [DataMember(IsRequired = true, Name = "subscribed_fields")] public List<string> SubscribedFields { get; set; } = new List<string>();
    }
}