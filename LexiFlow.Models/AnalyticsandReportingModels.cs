using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models
{
    #region Analytics and Reporting Models

    /// <summary>
    /// Represents exam analytics
    /// </summary>
    public class ExamAnalytic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnalyticsID { get; set; }

        [Required]
        public int UserExamID { get; set; }

        public float? VocabularyScore { get; set; }

        public float? GrammarScore { get; set; }

        public float? ReadingScore { get; set; }

        public float? ListeningScore { get; set; }

        public string WeakAreas { get; set; }

        public string StrongAreas { get; set; }

        public string Recommendations { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.Now;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserExamID")]
        public virtual UserExam UserExam { get; set; }
    }

    /// <summary>
    /// Represents practice analytics
    /// </summary>
    public class PracticeAnalytic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnalyticsID { get; set; }

        [Required]
        public int UserPracticeID { get; set; }

        public string SkillsAnalysis { get; set; }

        public string LearningCurve { get; set; }

        public string ProblemAreas { get; set; }

        public int? MasteryPercentage { get; set; }

        public string Recommendations { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.Now;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserPracticeID")]
        public virtual UserPracticeSet UserPracticeSet { get; set; }
    }

    /// <summary>
    /// Represents a user's strengths and weaknesses
    /// </summary>
    public class StrengthWeakness
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SWID { get; set; }

        [Required]
        public int UserID { get; set; }

        [StringLength(50)]
        public string SkillType { get; set; }

        [StringLength(100)]
        public string SpecificSkill { get; set; }

        public int? ProficiencyLevel { get; set; }

        public string RecommendedMaterials { get; set; }

        public string ImprovementNotes { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }

    /// <summary>
    /// Represents a report type
    /// </summary>
    public class ReportType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeID { get; set; }

        [Required]
        [StringLength(100)]
        public string TypeName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public string Template { get; set; }

        public int? FrequencyDays { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<StudyReport> StudyReports { get; set; }
    }

    /// <summary>
    /// Represents a study report
    /// </summary>
    public class StudyReport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportID { get; set; }

        [Required]
        public int UserID { get; set; }

        [StringLength(100)]
        public string ReportName { get; set; }

        public int? TypeID { get; set; }

        public DateTime? StartPeriod { get; set; }

        public DateTime? EndPeriod { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.Now;

        [StringLength(20)]
        public string Format { get; set; }

        [StringLength(255)]
        public string AccessURL { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("TypeID")]
        public virtual ReportType Type { get; set; }

        public virtual ICollection<StudyReportItem> StudyReportItems { get; set; }
    }

    /// <summary>
    /// Represents an item in a study report
    /// </summary>
    public class StudyReportItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemID { get; set; }

        [Required]
        public int ReportID { get; set; }

        public int? GoalID { get; set; }

        [StringLength(100)]
        public string MetricName { get; set; }

        [StringLength(255)]
        public string MetricValue { get; set; }

        [StringLength(255)]
        public string Comparison { get; set; }

        [StringLength(50)]
        public string Trend { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("ReportID")]
        public virtual StudyReport Report { get; set; }

        [ForeignKey("GoalID")]
        public virtual StudyGoal Goal { get; set; }
    }

    #endregion
}
