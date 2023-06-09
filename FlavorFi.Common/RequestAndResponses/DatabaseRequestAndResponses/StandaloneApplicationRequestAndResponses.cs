using FlavorFi.Common.Models.DatabaseModels;
using System;
using System.Runtime.Serialization;

namespace FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses
{
    [DataContract]
    public class GetStandaloneApplicationRequest : BaseActiveRequest
    {
        [DataMember(IsRequired = true)] public Guid StandaloneApplicationId { get; set; }
    }

    [DataContract]
    public class GetStandaloneApplicationByNameRequest : BaseActiveRequest
    {
        [DataMember(IsRequired = true)] public string StandaloneApplicationName { get; set; }
    }

    [DataContract]
    public class GetStandaloneApplicationResponse : BaseResponse
    {
        [DataMember(IsRequired = true)] public StandaloneApplicationModel StandaloneApplication { get; set; }
    }
}