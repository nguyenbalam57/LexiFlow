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
    /// Bài nộp từ vựng của người dùng
    /// </summary>
    [Index(nameof(UserId), nameof(SubmissionDate), Name = "IX_UserVocabularySubmission_User_Date")]
    public class UserVocabularySubmission : AuditableEntity, ISoftDeletable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubmissionId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string SubmissionTitle { get; set; }

        public string Description { get; set; }

        public int StatusId { get; set; }

        // Cải tiến: Phân loại bài nộp
        [StringLength(50)]
        public string SubmissionType { get; set; } // NewVocabulary, Correction, Translation

        [StringLength(50)]
        public string SubmissionCategory { get; set; } // General, Technical, Slang

        [StringLength(20)]
        public string JLPTLevel { get; set; } // N5, N4, etc.

        // Cải tiến: Quản lý và phê duyệt
        public int? ReviewerId { get; set; } // Người đánh giá

        public DateTime? ReviewedAt { get; set; } // Thời điểm đánh giá

        [StringLength(50)]
        public string ReviewResult { get; set; } // Approved, Rejected, NeedsChanges

        public string ReviewComments { get; set; } // Nhận xét đánh giá

        // Cải tiến: Tài liệu và tham khảo
        public string References { get; set; } // Tài liệu tham khảo

        public string Sources { get; set; } // Nguồn

        [StringLength(255)]
        public string AttachmentUrls { get; set; } // Tệp đính kèm

        // Cải tiến: Phản hồi và thông báo
        public bool NotifyOnStatusChange { get; set; } = true; // Thông báo khi thay đổi trạng thái

        public string UserNotes { get; set; } // Ghi chú của người dùng

        // Soft delete
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("StatusId")]
        public virtual SubmissionStatus Status { get; set; }

        [ForeignKey("ReviewerId")]
        public virtual User.User Reviewer { get; set; }

        [ForeignKey("DeletedBy")]
        public virtual User.User DeletedByUser { get; set; }

        public virtual ICollection<UserVocabularyDetail> VocabularyDetails { get; set; }
        public virtual ICollection<ApprovalHistory> ApprovalHistories { get; set; }
    }
}
