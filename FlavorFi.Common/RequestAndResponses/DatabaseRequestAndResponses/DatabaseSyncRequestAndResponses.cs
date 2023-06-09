using FlavorFi.Common.Models.DatabaseModels;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses
{
    [DataContract]
    public class GetDatabaseSyncStatusRequest : BaseRequest
    {
        [DataMember(IsRequired = true)] public string Entity { get; set; } = string.Empty;
    }

    [DataContract]
    public class GetDatabaseSyncStatusResponse : BaseResponse
    {
        [DataMember(IsRequired = true)] public DatabaseSyncModel SyncRecord { get; set; } = null;
    }

    [DataContract]
    public class SaveDatabaseSyncStatusRequest : BaseRequest
    {
        [DataMember(IsRequired = true)] public DatabaseSyncModel SyncRecord { get; set; } = new DatabaseSyncModel();
    }

    [DataContract]
    public class SaveDatabaseSyncStatusResponse : BaseResponse
    {
        [DataMember(IsRequired = true)] public DatabaseSyncModel SyncRecord { get; set; } = new DatabaseSyncModel();
    }
}