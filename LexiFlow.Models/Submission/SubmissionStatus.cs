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
    /// Trạng thái của bài nộp
    /// </summary>
    [Index(nameof(StatusName), IsUnique = true, Name = "IX_SubmissionStatus_Name")]
    public class SubmissionStatus : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusId { get; set; }

        [Required]
        [StringLength(50)]
        public string StatusName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(20)]
        public string ColorCode { get; set; }

        public int DisplayOrder { get; set; } = 0;

        // Cải tiến: Thuộc tính trạng thái
        public bool IsInitial { get; set; } = false; // Trạng thái ban đầu

        public bool IsTerminal { get; set; } = false; // Trạng thái kết thúc

        public bool RequiresAction { get; set; } = false; // Yêu cầu hành động

        public bool IsDefaultStatus { get; set; } = false; // Trạng thái mặc định

        // Cải tiến: Xử lý và quy trình
        public bool AllowEditing { get; set; } = true; // Cho phép chỉnh sửa

        public bool NotifySubmitter { get; set; } = true; // Thông báo cho người nộp

        public string NotificationTemplate { get; set; } // Mẫu thông báo

        // Cải tiến: Phân quyền và hạn chế
        public string AllowedRoles { get; set; } // Vai trò được phép

        public string RequiredPermissions { get; set; } // Quyền yêu cầu

        // Cải tiến: Xử lý tự động
        public int? AutoTransitionMinutes { get; set; } // Tự động chuyển sau X phút

        public int? AutoTransitionToStatusId { get; set; } // Chuyển đến trạng thái

        public string TriggerActions { get; set; } // Các hành động kích hoạt

        // Navigation properties
        [ForeignKey("AutoTransitionToStatusId")]
        public virtual SubmissionStatus AutoTransitionToStatus { get; set; }

        public virtual ICollection<StatusTransition> FromTransitions { get; set; }
        public virtual ICollection<StatusTransition> ToTransitions { get; set; }
        public virtual ICollection<UserVocabularySubmission> Submissions { get; set; }
    }
}
