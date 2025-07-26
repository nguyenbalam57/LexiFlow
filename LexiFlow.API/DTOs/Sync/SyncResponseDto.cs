namespace LexiFlow.API.DTOs.Sync
{
    public class SyncResponseDto
    {
        public int Created { get; set; }
        public int Updated { get; set; }
        public int Deleted { get; set; }
        public List<string> Errors { get; set; } = new();
        public DateTime ServerTime { get; set; } = DateTime.UtcNow;
    }
}
