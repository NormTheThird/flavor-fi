using FlavorFi.Common.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses
{
    [DataContract]
    public class BaseRequest
    {
        [DataMember(IsRequired = true)] public Guid CompanySiteId { get; set; } = Guid.Empty;
    }

    [DataContract]
    public class BaseActiveRequest : BaseRequest
    {
        [DataMember(IsRequired = true)] public bool GetActiveAndInactive { get; set; } = false;
    }

    [DataContract]
    public class BaseResponse
    {
        [DataMember(IsRequired = true)] public bool IsSuccess { get; set; } = false;
        [DataMember(IsRequired = true)] public string ErrorMessage { get; set; } = string.Empty;
    }

    [DataContract]
    public class BaseSyncRequest
    {
        [DataMember(IsRequired = true)] public BaseSiteModel BaseSite { get; set; } = new BaseSiteModel();
    }

    [DataContract]
    public class BaseSyncResponse : BaseResponse
    {
        [DataMember(IsRequired = true)] public string SessionId { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Message { get; set; } = string.Empty;
    }

    [DataContract]
    public class BaseSyncStatusRequest : BaseSyncRequest
    {
        [DataMember(IsRequired = true)] public string SessionId { get; set; } = string.Empty;
    }

    [DataContract]
    public class BaseSyncStatusResponse : BaseSyncResponse
    {
        [DataMember(IsRequired = true)] public List<SynchronizeDatabaseStatusModel> Statuses { get; set; } = new List<SynchronizeDatabaseStatusModel>();
    }
}