namespace LexiFlow.API.Models.DTOs
{
    public class SyncItemDto
    {
        public int Id { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty; // "Create", "Update", "Delete"
        public object? Data { get; set; }
        public DateTime Timestamp { get; set; }
        public string RowVersionString { get; set; } = string.Empty;
    }
}
