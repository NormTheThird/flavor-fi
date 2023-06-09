using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.DatabaseModels
{
    [DataContract]
    public class ModuleModel : ActiveModel
    {
        [DataMember(IsRequired = true)] public string Name { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string DisplayName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Description { get; set; } = string.Empty;
    }
}