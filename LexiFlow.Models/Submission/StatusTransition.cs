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
    /// Quy tắc chuyển đổi trạng thái bài nộp
    /// </summary>
    [Index(nameof(FromStatusId), nameof(ToStatusId), IsUnique = true, Name = "IX_StatusTransition_From_To")]
    public class StatusTransition : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransitionId { get; set; }

        [Required]
        public int FromStatusId { get; set; }

        [Required]
        public int ToStatusId { get; set; }

        [StringLength(100)]
        public string TransitionName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        // Cải tiến: Phân quyền và hạn chế
        public string AllowedRoles { get; set; } // Vai trò được phép

        public string RequiredPermissions { get; set; } // Quyền yêu cầu

        // Cải tiến: Xác thực và kiểm tra
        public bool RequiresApproval { get; set; } = false; // Yêu cầu phê duyệt

        public bool RequiresComment { get; set; } = false; // Yêu cầu nhận xét

        public bool RequiresAttachment { get; set; } = false; // Yêu cầu tệp đính kèm

        public string ValidationRules { get; set; } // Quy tắc xác thực

        // Cải tiến: Thông báo và hành động
        public bool NotifySubmitter { get; set; } = true; // Thông báo cho người nộp

        public bool NotifyApprovers { get; set; } = true; // Thông báo cho người phê duyệt

        public string NotificationTemplate { get; set; } // Mẫu thông báo

        public string TriggerActions { get; set; } // Các hành động kích hoạt

        // Cải tiến: Hiển thị và giao diện
        [StringLength(100)]
        public string ButtonLabel { get; set; } // Nhãn nút

        [StringLength(20)]
        public string ButtonColor { get; set; } // Màu nút

        public string ConfirmationMessage { get; set; } // Thông báo xác nhận

        public int DisplayOrder { get; set; } = 0; // Thứ tự hiển thị

        // Navigation properties
        [ForeignKey("FromStatusId")]
        public virtual SubmissionStatus FromStatus { get; set; }

        [ForeignKey("ToStatusId")]
        public virtual SubmissionStatus ToStatus { get; set; }
    }
}
