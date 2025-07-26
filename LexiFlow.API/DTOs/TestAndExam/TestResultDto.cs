namespace LexiFlow.API.DTOs.TestAndExam
{
    public class TestResultDto
    {
        public int ResultID { get; set; }
        public int UserID { get; set; }
        public int ExamID { get; set; }
        public string ExamName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Score { get; set; }
        public int TotalPoints { get; set; }
        public float ScorePercentage { get; set; }
        public bool IsPassed { get; set; }
        public List<TestAnswerDto> Answers { get; set; } = new List<TestAnswerDto>();
    }
}
