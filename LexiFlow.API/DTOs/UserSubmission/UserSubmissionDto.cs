namespace LexiFlow.API.DTOs.UserSubmission
{
    public class UserSubmissionDto
    {
        public int SubmissionID { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public string SubmissionType { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int? RelatedItemID { get; set; }
        public string RelatedItemType { get; set; }
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public List<SubmissionAttachmentDto> Attachments { get; set; } = new List<SubmissionAttachmentDto>();
        public List<SubmissionCommentDto> Comments { get; set; } = new List<SubmissionCommentDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
