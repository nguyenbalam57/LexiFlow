using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models
{
    #region JLPT Exam Management Models

    /// <summary>
    /// Represents a JLPT level
    /// </summary>
    public class JLPTLevel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LevelID { get; set; }

        [Required]
        [StringLength(10)]
        public string LevelName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int? VocabularyCount { get; set; }

        public int? KanjiCount { get; set; }

        public int? GrammarPoints { get; set; }

        public int? PassingScore { get; set; }

        public string RequiredSkills { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<StudyGoal> StudyGoals { get; set; }
    }

    /// <summary>
    /// Represents a JLPT exam
    /// </summary>
    public class JLPTExam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExamID { get; set; }

        [Required]
        [StringLength(100)]
        public string ExamName { get; set; }

        [StringLength(10)]
        public string Level { get; set; }

        public int? Year { get; set; }

        [StringLength(20)]
        public string Month { get; set; }

        public int? TotalTime { get; set; }

        public int? TotalScore { get; set; }

        public int? TotalQuestions { get; set; }

        public string Description { get; set; }

        public bool IsOfficial { get; set; } = false;

        public bool IsActive { get; set; } = true;

        public int? CreatedByUserID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("CreatedByUserID")]
        public virtual User CreatedByUser { get; set; }

        public virtual ICollection<JLPTSection> Sections { get; set; }
        public virtual ICollection<UserExam> UserExams { get; set; }
    }

    /// <summary>
    /// Represents a section in a JLPT exam
    /// </summary>
    public class JLPTSection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SectionID { get; set; }

        [Required]
        public int ExamID { get; set; }

        [StringLength(100)]
        public string SectionName { get; set; }

        [StringLength(50)]
        public string SectionType { get; set; }

        public int? OrderNumber { get; set; }

        public int? TimeAllocation { get; set; }

        public int? ScoreAllocation { get; set; }

        public int? QuestionCount { get; set; }

        public string Instructions { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("ExamID")]
        public virtual JLPTExam Exam { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }

    /// <summary>
    /// Represents a question in an exam
    /// </summary>
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionID { get; set; }

        public int? SectionID { get; set; }

        [StringLength(50)]
        public string QuestionType { get; set; }

        public string QuestionText { get; set; }

        [StringLength(255)]
        public string QuestionImage { get; set; }

        [StringLength(255)]
        public string QuestionAudio { get; set; }

        [StringLength(255)]
        public string CorrectAnswer { get; set; }

        public string Explanation { get; set; }

        public int? Difficulty { get; set; }

        [StringLength(10)]
        public string JLPT_Level { get; set; }

        [StringLength(255)]
        public string Tags { get; set; }

        [StringLength(255)]
        public string Skills { get; set; }

        public bool IsVerified { get; set; } = false;

        public int? CreatedByUserID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("SectionID")]
        public virtual JLPTSection Section { get; set; }

        [ForeignKey("CreatedByUserID")]
        public virtual User CreatedByUser { get; set; }

        public virtual ICollection<QuestionOption> Options { get; set; }
        public virtual ICollection<CustomExamQuestion> CustomExamQuestions { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
        public virtual ICollection<PracticeSetItem> PracticeSetItems { get; set; }
        public virtual ICollection<UserPracticeAnswer> UserPracticeAnswers { get; set; }
    }

    /// <summary>
    /// Represents an option for a question
    /// </summary>
    public class QuestionOption
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OptionID { get; set; }

        [Required]
        public int QuestionID { get; set; }

        public string OptionText { get; set; }

        [StringLength(255)]
        public string OptionImage { get; set; }

        public bool IsCorrect { get; set; } = false;

        public int? DisplayOrder { get; set; }

        public string Explanation { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("QuestionID")]
        public virtual Question Question { get; set; }

        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }

    #endregion
}
