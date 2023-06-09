using FlavorFi.Common.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses
{
    [DataContract]
    public class GetModulesRequest : BaseActiveRequest { }

    [DataContract]
    public class GetModulesResponse : BaseResponse
    {
        public GetModulesResponse()
        {
            this.Modules = new List<ModuleModel>();
        }

        [DataMember(IsRequired = true)]
        public List<ModuleModel> Modules { get; set; }
    }

    [DataContract]
    public class ChangeModuleStatusRequest : BaseRequest
    {
        public ChangeModuleStatusRequest()
        {
            this.ModuleId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid ModuleId { get; set; }
    }

    [DataContract]
    public class ChangeModuleStatusResponse : BaseResponse { }
}