namespace LexiFlow.API.Services
{
    public interface ISyncService
    {
        Task<SyncResult> SyncVocabularyAsync(int userId, DateTime? lastSync, List<PendingSyncItem> pendingItems);
        Task<DateTime> GetLastSyncTimestampAsync(int userId, string tableName);
    }

    public class SyncResult
    {
        public bool Success { get; set; }
        public DateTime SyncedAt { get; set; }
        public int ItemsUploaded { get; set; }
        public int ItemsDownloaded { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class PendingSyncItem
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public int? RecordId { get; set; }
        public string Operation { get; set; }
        public string Data { get; set; }
    }
}
