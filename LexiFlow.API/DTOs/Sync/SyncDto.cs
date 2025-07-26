using LexiFlow.API.Models.DTOs;

namespace LexiFlow.API.DTOs.Sync
{
    public class SyncBatchDto
    {
        public string EntityType { get; set; } = string.Empty;
        public DateTime? LastSyncTime { get; set; }
        public List<SyncItemDto> Changes { get; set; } = new();
        public string DeviceId { get; set; } = string.Empty;
    }

    public class SyncItemDto
    {
        public int Id { get; set; }
        public string Action { get; set; } = string.Empty; // "Create", "Update", "Delete"
        public object? Data { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class SyncResponseDto
    {
        public int Created { get; set; }
        public int Updated { get; set; }
        public int Deleted { get; set; }
        public List<string> Errors { get; set; } = new();
        public DateTime ServerTime { get; set; } = DateTime.UtcNow;
    }

    public class SyncStatusDto
    {
        public string EntityType { get; set; } = string.Empty;
        public DateTime LastSync { get; set; }
        public int LocalCount { get; set; }
        public int ServerCount { get; set; }
        public bool InSync => LocalCount == ServerCount;
    }

}
