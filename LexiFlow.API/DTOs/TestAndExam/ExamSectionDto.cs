namespace LexiFlow.API.DTOs.TestAndExam
{
    public class ExamSectionDto
    {
        public int SectionID { get; set; }
        public int ExamID { get; set; }
        public string SectionName { get; set; }
        public string SectionType { get; set; }
        public int DisplayOrder { get; set; }
        public int DurationMinutes { get; set; }
        public int TotalPoints { get; set; }
        public string Instructions { get; set; }
        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
    }
}
