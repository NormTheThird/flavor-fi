using System;
using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.DatabaseModels
{
    [DataContract]
    public class DatabaseSyncModel : BaseModel
    {
        [DataMember(IsRequired = true)] public Guid SycnedByAccountId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public string Entity { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Message { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public int NumberSynced { get; set; } = 0;
        [DataMember(IsRequired = true)] public bool CurrentlySyncing { get; set; } = false;
        [DataMember(IsRequired = true)] public bool HadError { get; set; } = false;
        [DataMember(IsRequired = true)] public DateTimeOffset DateStarted { get; set; } = DateTime.MinValue;
        [DataMember(IsRequired = true)] public DateTimeOffset? DateEnded { get; set; } = null;
    }

    [DataContract]
    public class SynchronizeDatabaseStatusModel
    {
        [DataMember(IsRequired = true)] public string Message { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public int TotalRecordsToSync { get; set; } = 0;
        [DataMember(IsRequired = true)] public int RecordsSynced { get; set; } = 0;
        [DataMember(IsRequired = true)] public bool IsProcessComplete { get; set; } = false;
    }
}