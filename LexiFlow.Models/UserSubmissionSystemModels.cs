using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.Models
{
    #region User Submission System Models

    /// <summary>
    /// Represents a submission status
    /// </summary>
    public class SubmissionStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusID { get; set; }

        [Required]
        [StringLength(50)]
        public string StatusName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(20)]
        public string ColorCode { get; set; }

        public int? DisplayOrder { get; set; }

        public bool IsTerminal { get; set; } = false;

        public bool IsDefault { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<StatusTransition> FromStatusTransitions { get; set; }
        public virtual ICollection<StatusTransition> ToStatusTransitions { get; set; }
        public virtual ICollection<UserVocabularySubmission> UserVocabularySubmissions { get; set; }
        public virtual ICollection<ApprovalHistory> FromStatusApprovalHistories { get; set; }
        public virtual ICollection<ApprovalHistory> ToStatusApprovalHistories { get; set; }
    }

    /// <summary>
    /// Represents a status transition
    /// </summary>
    public class StatusTransition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransitionID { get; set; }

        public int? FromStatusID { get; set; }

        public int? ToStatusID { get; set; }

        [StringLength(100)]
        public string TransitionName { get; set; }

        [StringLength(50)]
        public string RequiredRole { get; set; }

        public bool RequiresApproval { get; set; } = false;

        public bool RequiresNote { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("FromStatusID")]
        public virtual SubmissionStatus FromStatus { get; set; }

        [ForeignKey("ToStatusID")]
        public virtual SubmissionStatus ToStatus { get; set; }
    }

    /// <summary>
    /// Represents a user vocabulary submission
    /// </summary>
    public class UserVocabularySubmission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubmissionID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        [StringLength(200)]
        public string SubmissionTitle { get; set; }

        public string SubmissionDescription { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        public int? StatusID { get; set; }

        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;

        public int? LastUpdatedByUserID { get; set; }

        public bool IsUrgent { get; set; } = false;

        public int VocabularyCount { get; set; } = 0;

        public string SubmissionNotes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("StatusID")]
        public virtual SubmissionStatus Status { get; set; }

        [ForeignKey("LastUpdatedByUserID")]
        public virtual User LastUpdatedByUser { get; set; }

        public virtual ICollection<UserVocabularyDetail> UserVocabularyDetails { get; set; }
        public virtual ICollection<ApprovalHistory> ApprovalHistories { get; set; }
    }

    /// <summary>
    /// Represents user vocabulary details
    /// </summary>
    public class UserVocabularyDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetailID { get; set; }

        [Required]
        public int SubmissionID { get; set; }

        [StringLength(100)]
        public string Japanese { get; set; }

        [StringLength(100)]
        public string Kana { get; set; }

        [StringLength(100)]
        public string Romaji { get; set; }

        [StringLength(255)]
        public string Vietnamese { get; set; }

        [StringLength(255)]
        public string English { get; set; }

        public string Example { get; set; }

        public string Notes { get; set; }

        public int? CategoryID { get; set; }

        [StringLength(50)]
        public string PartOfSpeech { get; set; }

        [StringLength(10)]
        public string Level { get; set; }

        [StringLength(255)]
        public string Source { get; set; }

        [StringLength(255)]
        public string ImageURL { get; set; }

        [StringLength(255)]
        public string AudioURL { get; set; }

        public bool IsApproved { get; set; } = false;

        public string RejectionReason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("SubmissionID")]
        public virtual UserVocabularySubmission Submission { get; set; }

        [ForeignKey("CategoryID")]
        public virtual VocabularyCategory Category { get; set; }
    }

    /// <summary>
    /// Represents an approval history
    /// </summary>
    public class ApprovalHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HistoryID { get; set; }

        [Required]
        public int SubmissionID { get; set; }

        public int? ApproverUserID { get; set; }

        public int? FromStatusID { get; set; }

        public int? ToStatusID { get; set; }

        public string Comments { get; set; }

        public DateTime ApprovedAt { get; set; } = DateTime.Now;

        public string ChangeDetails { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("SubmissionID")]
        public virtual UserVocabularySubmission Submission { get; set; }

        [ForeignKey("ApproverUserID")]
        public virtual User ApproverUser { get; set; }

        [ForeignKey("FromStatusID")]
        public virtual SubmissionStatus FromStatus { get; set; }

        [ForeignKey("ToStatusID")]
        public virtual SubmissionStatus ToStatus { get; set; }
    }

    #endregion
}