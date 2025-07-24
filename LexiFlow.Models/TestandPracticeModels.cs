using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models
{
    #region Test and Practice Models

    /// <summary>
    /// Represents a test result
    /// </summary>
    public class TestResult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestResultID { get; set; }

        [Required]
        public int UserID { get; set; }

        public DateTime TestDate { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string TestType { get; set; }

        public int? TotalQuestions { get; set; }

        public int? CorrectAnswers { get; set; }

        public int? Score { get; set; }

        public int? Duration { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public virtual ICollection<TestDetail> TestDetails { get; set; }
    }

    /// <summary>
    /// Represents a test detail
    /// </summary>
    public class TestDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestDetailID { get; set; }

        [Required]
        public int TestResultID { get; set; }

        public int? VocabularyID { get; set; }

        public bool IsCorrect { get; set; } = false;

        public int? TimeSpent { get; set; }

        [StringLength(255)]
        public string UserAnswer { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("TestResultID")]
        public virtual TestResult TestResult { get; set; }

        [ForeignKey("VocabularyID")]
        public virtual Vocabulary Vocabulary { get; set; }
    }

    /// <summary>
    /// Represents a custom exam
    /// </summary>
    public class CustomExam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomExamID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        [StringLength(100)]
        public string ExamName { get; set; }

        [StringLength(10)]
        public string Level { get; set; }

        public string Description { get; set; }

        public int? TimeLimit { get; set; }

        public bool IsPublic { get; set; } = false;

        public bool IsFeatured { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public virtual ICollection<CustomExamQuestion> CustomExamQuestions { get; set; }
        public virtual ICollection<UserExam> UserExams { get; set; }
    }

    /// <summary>
    /// Represents a question in a custom exam
    /// </summary>
    public class CustomExamQuestion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomQuestionID { get; set; }

        [Required]
        public int CustomExamID { get; set; }

        [Required]
        public int QuestionID { get; set; }

        public int? OrderNumber { get; set; }

        public int? ScoreValue { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("CustomExamID")]
        public virtual CustomExam CustomExam { get; set; }

        [ForeignKey("QuestionID")]
        public virtual Question Question { get; set; }
    }

    /// <summary>
    /// Represents a user's exam attempt
    /// </summary>
    public class UserExam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserExamID { get; set; }

        [Required]
        public int UserID { get; set; }

        public int? ExamID { get; set; }

        public int? CustomExamID { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? Score { get; set; }

        public int? TotalQuestions { get; set; }

        public int? CorrectAnswers { get; set; }

        public bool IsCompleted { get; set; } = false;

        public string ExamFeedback { get; set; }

        public string UserNotes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("ExamID")]
        public virtual JLPTExam Exam { get; set; }

        [ForeignKey("CustomExamID")]
        public virtual CustomExam CustomExam { get; set; }

        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
        public virtual ICollection<ExamAnalytic> ExamAnalytics { get; set; }
    }

    /// <summary>
    /// Represents a user's answer to a question
    /// </summary>
    public class UserAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerID { get; set; }

        [Required]
        public int UserExamID { get; set; }

        [Required]
        public int QuestionID { get; set; }

        public int? SelectedOptionID { get; set; }

        public string UserInput { get; set; }

        public bool IsCorrect { get; set; } = false;

        public int? TimeSpent { get; set; }

        public int? Attempt { get; set; }

        public DateTime? AnsweredAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserExamID")]
        public virtual UserExam UserExam { get; set; }

        [ForeignKey("QuestionID")]
        public virtual Question Question { get; set; }

        [ForeignKey("SelectedOptionID")]
        public virtual QuestionOption SelectedOption { get; set; }
    }

    /// <summary>
    /// Represents a practice set
    /// </summary>
    public class PracticeSet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PracticeSetID { get; set; }

        [Required]
        [StringLength(100)]
        public string SetName { get; set; }

        [StringLength(50)]
        public string SetType { get; set; }

        [StringLength(10)]
        public string Level { get; set; }

        public string Description { get; set; }

        [StringLength(255)]
        public string Skills { get; set; }

        public int? ItemCount { get; set; }

        public bool IsPublic { get; set; } = false;

        public bool IsFeatured { get; set; } = false;

        public int? CreatedByUserID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("CreatedByUserID")]
        public virtual User CreatedByUser { get; set; }

        public virtual ICollection<PracticeSetItem> PracticeSetItems { get; set; }
        public virtual ICollection<UserPracticeSet> UserPracticeSets { get; set; }
    }

    /// <summary>
    /// Represents an item in a practice set
    /// </summary>
    public class PracticeSetItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemID { get; set; }

        [Required]
        public int PracticeSetID { get; set; }

        [Required]
        public int QuestionID { get; set; }

        public int? OrderNumber { get; set; }

        [StringLength(50)]
        public string PracticeMode { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("PracticeSetID")]
        public virtual PracticeSet PracticeSet { get; set; }

        [ForeignKey("QuestionID")]
        public virtual Question Question { get; set; }
    }

    /// <summary>
    /// Represents a user's practice set
    /// </summary>
    public class UserPracticeSet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserPracticeID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int PracticeSetID { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? LastPracticed { get; set; }

        public int CompletionPercentage { get; set; } = 0;

        public int CorrectPercentage { get; set; } = 0;

        public int TotalAttempts { get; set; } = 0;

        public string UserNotes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("PracticeSetID")]
        public virtual PracticeSet PracticeSet { get; set; }

        public virtual ICollection<UserPracticeAnswer> UserPracticeAnswers { get; set; }
        public virtual ICollection<PracticeAnalytic> PracticeAnalytics { get; set; }
    }

    /// <summary>
    /// Represents a user's answer in a practice set
    /// </summary>
    public class UserPracticeAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PracticeAnswerID { get; set; }

        [Required]
        public int UserPracticeID { get; set; }

        [Required]
        public int QuestionID { get; set; }

        public bool IsCorrect { get; set; } = false;

        public int? Attempt { get; set; }

        public DateTime? AnsweredAt { get; set; }

        public int? TimeTaken { get; set; }

        public int? MemoryStrength { get; set; }

        public DateTime? NextReviewDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserPracticeID")]
        public virtual UserPracticeSet UserPracticeSet { get; set; }

        [ForeignKey("QuestionID")]
        public virtual Question Question { get; set; }
    }

    #endregion
}
