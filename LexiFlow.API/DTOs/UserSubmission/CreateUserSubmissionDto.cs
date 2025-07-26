using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.UserSubmission
{
    public class CreateUserSubmissionDto
    {
        [Required]
        [StringLength(50)]
        public string SubmissionType { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public int? RelatedItemID { get; set; }

        public string RelatedItemType { get; set; }

        public List<CreateSubmissionAttachmentDto> Attachments { get; set; } = new List<CreateSubmissionAttachmentDto>();
    }
}
