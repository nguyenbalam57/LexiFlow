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

namespace LexiFlow.Models.Analytics
{
    /// <summary>
    /// Báo cáo học tập
    /// </summary>
    [Index(nameof(UserId), nameof(TypeId), nameof(StartPeriod), nameof(EndPeriod), Name = "IX_StudyReport_User_Type_Period")]
    public class StudyReport : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportId { get; set; }

        [Required]
        public int UserId { get; set; }

        [StringLength(100)]
        public string ReportName { get; set; }

        public int? TypeId { get; set; }

        public DateTime? StartPeriod { get; set; }

        public DateTime? EndPeriod { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        [StringLength(20)]
        public string Format { get; set; }

        [StringLength(255)]
        public string AccessUrl { get; set; }

        // Cải tiến: Thông tin báo cáo chi tiết
        public string ReportSummary { get; set; }
        public string OverallAnalysis { get; set; }
        public string Recommendations { get; set; }

        // Cải tiến: Cấu hình thông báo
        public bool IsShared { get; set; } = false;
        public string SharedWith { get; set; }
        public bool EmailNotificationSent { get; set; } = false;

        // Cải tiến: Dữ liệu tổng hợp
        public int? TotalStudyMinutes { get; set; }
        public int? ItemsStudied { get; set; }
        public int? ExamsCompleted { get; set; }
        public int? PracticesCompleted { get; set; }
        public float? AverageAccuracy { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("TypeId")]
        public virtual ReportType Type { get; set; }

        public virtual ICollection<StudyReportItem> StudyReportItems { get; set; }
    }
}
