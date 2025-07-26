namespace LexiFlow.API.DTOs.UserSubmission
{
    public class SubmissionStatusDto
    {
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public string Description { get; set; }
        public string ColorCode { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsTerminal { get; set; }
        public bool IsDefault { get; set; }
    }
}
