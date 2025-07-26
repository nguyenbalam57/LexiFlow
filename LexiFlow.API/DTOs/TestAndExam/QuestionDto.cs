namespace LexiFlow.API.DTOs.TestAndExam
{
    public class QuestionDto
    {
        public int QuestionID { get; set; }
        public int SectionID { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public int Points { get; set; }
        public int DisplayOrder { get; set; }
        public string DifficultyLevel { get; set; }
        public string MediaURL { get; set; }
        public List<QuestionOptionDto> Options { get; set; } = new List<QuestionOptionDto>();
    }

}
