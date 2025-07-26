namespace LexiFlow.API.DTOs.TestAndExam
{
    public class TestAnswerDto
    {
        public int AnswerID { get; set; }
        public int ResultID { get; set; }
        public int QuestionID { get; set; }
        public string QuestionText { get; set; }
        public string UserAnswer { get; set; }
        public string CorrectAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public int Points { get; set; }
        public string Feedback { get; set; }
    }
}
