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
    /// Nhật ký hiệu suất
    /// </summary>
    [Index(nameof(Timestamp), Name = "IX_PerformanceLog_Time")]
    public class PerformanceLog : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(100)]
        public string Operation { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        public int ExecutionTimeMs { get; set; }

        [StringLength(255)]
        public string RequestPath { get; set; }

        public int? UserId { get; set; }

        [StringLength(45)]
        public string IPAddress { get; set; }

        // Cải tiến: Thông tin chi tiết
        public string Parameters { get; set; }
        public int? ResultCount { get; set; }
        public int? CpuUsagePercent { get; set; }
        public int? MemoryUsageBytes { get; set; }
        public int? DbCallCount { get; set; }
        public int? DbTimeMs { get; set; }

        // Cải tiến: Phân tích hiệu suất
        public bool IsSlowOperation { get; set; } = false;
        public string BottleneckIdentified { get; set; }
        public string OptimizationSuggestion { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }
    }
}
