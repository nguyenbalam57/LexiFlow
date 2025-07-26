namespace LexiFlow.API.DTOs.Learning
{
    public class ReviewSessionResultDto
    {
        public int TotalReviewed { get; set; }
        public int CorrectCount { get; set; }
        public int IncorrectCount { get; set; }
        public List<ReviewItemResultDto> ItemResults { get; set; } = new List<ReviewItemResultDto>();
    }
}
