namespace LexiFlow.API.Models
{
    public class SyncMetadata
    {
        public int UserID { get; set; }
        public string TableName { get; set; }
        public DateTime LastSyncTimestamp { get; set; }
        public int LastSyncVersion { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
