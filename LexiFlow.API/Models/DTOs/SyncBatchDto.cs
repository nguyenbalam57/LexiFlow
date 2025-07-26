namespace LexiFlow.API.Models.DTOs
{
    public class SyncBatchDto
    {
        public string EntityType { get; set; } = string.Empty;
        public DateTime? LastSyncTime { get; set; }
        public List<SyncItemDto> Changes { get; set; } = new();
        public string DeviceId { get; set; } = string.Empty;
    }
}
