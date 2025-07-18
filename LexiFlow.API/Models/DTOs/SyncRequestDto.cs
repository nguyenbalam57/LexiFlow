namespace LexiFlow.API.Models.DTOs
{
    public class SyncRequestDto
    {
        public string AuthToken { get; set; }
        public DateTime? LastSyncTimestamp { get; set; }
        public List<PendingSyncItemDto>? PendingItems { get; set; }
    }

    public class PendingSyncItemDto
    {
        public string TableName { get; set; }
        public int? RecordId { get; set; }
        public string Operation { get; set; } // INSERT, UPDATE, DELETE
        public string Data { get; set; }
    }
}
