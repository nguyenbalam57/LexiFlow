using LexiFlow.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Analytics
{
    /// <summary>
    /// Mục trong báo cáo học tập
    /// </summary>
    public class StudyReportItem : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        [Required]
        public int ReportId { get; set; }

        public int? GoalId { get; set; }

        [StringLength(100)]
        public string MetricName { get; set; }

        [StringLength(255)]
        public string MetricValue { get; set; }

        [StringLength(255)]
        public string Comparison { get; set; }

        [StringLength(50)]
        public string Trend { get; set; }

        // Cải tiến: Dữ liệu chi tiết
        public string ChartData { get; set; }
        public string DetailedAnalysis { get; set; }

        // Cải tiến: Phân loại và thứ tự
        [StringLength(50)]
        public string Category { get; set; }
        public int DisplayOrder { get; set; } = 0;

        // Cải tiến: Liên kết
        public int? RelatedEntityId { get; set; }
        [StringLength(50)]
        public string RelatedEntityType { get; set; }

        public string Notes { get; set; }

        // Navigation properties
        [ForeignKey("ReportId")]
        public virtual StudyReport Report { get; set; }

        [ForeignKey("GoalId")]
        public virtual Planning.StudyGoal Goal { get; set; }
    }
}
