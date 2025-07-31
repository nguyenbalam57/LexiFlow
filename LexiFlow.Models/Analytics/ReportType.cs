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
    /// Loại báo cáo học tập
    /// </summary>
    [Index(nameof(TypeName), IsUnique = true, Name = "IX_ReportType_Name")]
    public class ReportType : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string TypeName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public string Template { get; set; }

        public int? FrequencyDays { get; set; }

        // Cải tiến: Cấu hình báo cáo
        [StringLength(50)]
        public string ReportCategory { get; set; } // Daily, Weekly, Monthly, Progress, Assessment

        [StringLength(50)]
        public string DataSource { get; set; } // Practice, Exam, LearningProgress, etc.

        public bool IncludeCharts { get; set; } = true;
        public bool IncludeRecommendations { get; set; } = true;
        public bool IncludeComparisons { get; set; } = true;

        // Cải tiến: Cấu hình tạo báo cáo tự động
        public bool EnableAutoGeneration { get; set; } = false;
        [StringLength(50)]
        public string AutoSchedule { get; set; } // Daily, Weekly, Monthly, Quarterly

        // Navigation properties
        public virtual ICollection<StudyReport> StudyReports { get; set; }
    }
}
