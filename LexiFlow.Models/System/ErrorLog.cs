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

namespace LexiFlow.Models.System
{
    /// <summary>
    /// Nhật ký lỗi
    /// </summary>
    [Index(nameof(Timestamp), Name = "IX_ErrorLog_Time")]
    [Index(nameof(ErrorCode), Name = "IX_ErrorLog_Code")]
    public class ErrorLog : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ErrorId { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [StringLength(50)]
        public string ErrorCode { get; set; }

        [StringLength(255)]
        public string ErrorMessage { get; set; }

        public string StackTrace { get; set; }

        [StringLength(255)]
        public string Source { get; set; }

        [StringLength(255)]
        public string TargetSite { get; set; }

        public int? UserId { get; set; }

        [StringLength(50)]
        public string UserAction { get; set; }

        [StringLength(255)]
        public string RequestUrl { get; set; }

        [StringLength(50)]
        public string RequestMethod { get; set; }

        public string RequestData { get; set; }

        [StringLength(255)]
        public string UserAgent { get; set; }

        [StringLength(45)]
        public string IPAddress { get; set; }

        // Cải tiến: Phân loại lỗi
        [StringLength(50)]
        public string ErrorType { get; set; } // Exception, Validation, Business, Authorization

        [StringLength(50)]
        public string ErrorSeverity { get; set; } // Low, Medium, High, Critical

        public bool IsHandled { get; set; } = false;

        // Cải tiến: Xử lý lỗi
        public bool IsResolved { get; set; } = false;
        public DateTime? ResolvedAt { get; set; }
        public int? ResolvedBy { get; set; }
        public string ResolutionNotes { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("ResolvedBy")]
        public virtual User.User ResolvedByUser { get; set; }
    }
}
