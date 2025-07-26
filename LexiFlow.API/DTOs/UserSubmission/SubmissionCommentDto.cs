namespace LexiFlow.API.DTOs.UserSubmission
{
    public class SubmissionCommentDto
    {
        public int CommentID { get; set; }
        public int SubmissionID { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
