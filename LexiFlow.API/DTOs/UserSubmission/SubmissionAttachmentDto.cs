namespace LexiFlow.API.DTOs.UserSubmission
{
    public class SubmissionAttachmentDto
    {
        public int AttachmentID { get; set; }
        public int SubmissionID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
