namespace FlavorFi.Common.Models.DataSyncModels
{
    public class SynchronizeDatabaseStatusModel
    {
        public SynchronizeDatabaseStatusModel()
        {
            this.Message = string.Empty;
            this.TotalRecordsToSync = 0;
            this.RecordsSynced = 0;
            this.IsProcessComplete = false;
        }

        public string Message { get; set; }
        public int TotalRecordsToSync { get; set; }
        public int RecordsSynced { get; set; }
        public bool IsProcessComplete { get; set; }
    }
}