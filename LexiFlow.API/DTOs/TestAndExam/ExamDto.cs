namespace LexiFlow.API.DTOs.TestAndExam
{
    public class ExamDto
    {
        public int ExamID { get; set; }
        public string ExamName { get; set; }
        public string ExamType { get; set; }
        public string JLPTLevel { get; set; }
        public int DurationMinutes { get; set; }
        public int TotalPoints { get; set; }
        public int PassingScore { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ExamSectionDto> Sections { get; set; } = new List<ExamSectionDto>();
    }
}
