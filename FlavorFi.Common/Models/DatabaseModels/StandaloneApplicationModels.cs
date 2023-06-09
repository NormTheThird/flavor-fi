using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.DatabaseModels
{
    [DataContract]
    public class StandaloneApplicationModel : ActiveModel
    {
        [DataMember(IsRequired = true)] public string Name { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public Dictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();
    }
}