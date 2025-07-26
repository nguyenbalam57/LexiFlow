namespace LexiFlow.API.DTOs.Learning
{
    public class ReviewSessionRequestDto
    {
        public int Count { get; set; } = 20;
        public bool IncludeOverdue { get; set; } = true;
        public bool IncludeNew { get; set; } = true;
        public int? MaxMemoryStrength { get; set; }
        public string LevelFilter { get; set; }
    }
}
