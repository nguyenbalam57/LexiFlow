namespace LexiFlow.API.DTOs.Sync
{
    public class SyncItemDto
    {
        public int Id { get; set; }
        public string Action { get; set; } = string.Empty; // "Create", "Update", "Delete"
        public object? Data { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
