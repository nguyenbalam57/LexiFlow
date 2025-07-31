using LexiFlow.Models.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Submission
{
    /// <summary>
    /// Lịch sử phê duyệt bài nộp
    /// </summary>
    [Index(nameof(SubmissionId), nameof(ActionDate), Name = "IX_ApprovalHistory_Submission_Date")]
    public class ApprovalHistory : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HistoryId { get; set; }

        [Required]
        public int SubmissionId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime ActionDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        public string Action { get; set; } // Approve, Reject, Request Changes, etc.

        public int? FromStatusId { get; set; }

        public int? ToStatusId { get; set; }

        public string Comments { get; set; }

        // Cải tiến: Chi tiết hành động
        [StringLength(50)]
        public string ActionType { get; set; } // Status Change, Comment, Attachment, etc.

        public string ActionDetails { get; set; } // Chi tiết hành động

        // Cải tiến: Tệp đính kèm và liên kết
        [StringLength(255)]
        public string AttachmentUrls { get; set; } // Tệp đính kèm

        public int? RelatedEntityId { get; set; } // ID thực thể liên quan

        [StringLength(50)]
        public string RelatedEntityType { get; set; } // Loại thực thể liên quan

        // Cải tiến: Thông tin hệ thống
        public bool IsSystemAction { get; set; } = false; // Hành động hệ thống

        [StringLength(45)]
        public string IPAddress { get; set; } // Địa chỉ IP

        [StringLength(255)]
        public string UserAgent { get; set; } // User Agent

        // Cải tiến: Trạng thái và thông báo
        public bool IsNotified { get; set; } = false; // Đã thông báo

        public DateTime? NotifiedAt { get; set; } // Thời điểm thông báo

        public bool RequiresAction { get; set; } = false; // Yêu cầu hành động

        // Navigation properties
        [ForeignKey("SubmissionId")]
        public virtual UserVocabularySubmission Submission { get; set; }

        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("FromStatusId")]
        public virtual SubmissionStatus FromStatus { get; set; }

        [ForeignKey("ToStatusId")]
        public virtual SubmissionStatus ToStatus { get; set; }
    }
}
