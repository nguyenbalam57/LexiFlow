namespace LexiFlow.API.DTOs.Sync
{
    public class SyncStatusDto
    {
        public string EntityType { get; set; } = string.Empty;
        public DateTime LastSync { get; set; }
        public int LocalCount { get; set; }
        public int ServerCount { get; set; }
        public bool InSync => LocalCount == ServerCount;
    }
}
