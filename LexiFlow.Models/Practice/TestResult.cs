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

namespace LexiFlow.Models.Practice
{
    /// <summary>
    /// Kết quả kiểm tra
    /// </summary>
    [Index(nameof(UserId), nameof(TestDate), Name = "IX_TestResult_User_Date")]
    public class TestResult : BaseEntity
    {
        public int? DurationMinutes;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestResultId { get; set; }

        [Required]
        public int UserId { get; set; }

        public DateTime TestDate { get; set; } = DateTime.UtcNow;

        [StringLength(50)]
        public string TestType { get; set; }

        public int? TotalQuestions { get; set; }

        public int? CorrectAnswers { get; set; }

        public int? Score { get; set; }

        public int? Duration { get; set; }

        // Cải tiến: Thông tin chi tiết
        public string TestName { get; set; }

        [StringLength(10)]
        public string Level { get; set; }

        public int? CategoryId { get; set; }

        // Cải tiến: Phân tích kết quả
        public string WeakAreas { get; set; }
        public string StrongAreas { get; set; }
        public string Recommendations { get; set; }

        // Cải tiến: Thống kê chuyên sâu
        public double? AccuracyRate { get; set; }
        public int? AverageResponseTime { get; set; }
        public int? MaxResponseTime { get; set; }
        public int? MinResponseTime { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Learning.Vocabulary.Category Category { get; set; }

        public virtual ICollection<TestDetail> TestDetails { get; set; }
    }
}
