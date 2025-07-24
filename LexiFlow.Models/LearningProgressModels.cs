using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.Models
{
    #region Learning Progress Models

    /// <summary>
    /// Represents a user's learning progress for a vocabulary item
    /// </summary>
    public class LearningProgress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProgressID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int VocabularyID { get; set; }

        public int StudyCount { get; set; } = 0;

        public int CorrectCount { get; set; } = 0;

        public int IncorrectCount { get; set; } = 0;

        public DateTime? LastStudied { get; set; }

        public int MemoryStrength { get; set; } = 0;

        public DateTime? NextReviewDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("VocabularyID")]
        public virtual Vocabulary Vocabulary { get; set; }
    }

    /// <summary>
    /// Represents a user's personal word list
    /// </summary>
    public class PersonalWordList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ListID { get; set; }

        [Required]
        [StringLength(100)]
        public string ListName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        public int UserID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public virtual ICollection<PersonalWordListItem> Items { get; set; }
    }

    /// <summary>
    /// Represents an item in a personal word list
    /// </summary>
    public class PersonalWordListItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemID { get; set; }

        [Required]
        public int ListID { get; set; }

        [Required]
        public int VocabularyID { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.Now;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("ListID")]
        public virtual PersonalWordList List { get; set; }

        [ForeignKey("VocabularyID")]
        public virtual Vocabulary Vocabulary { get; set; }
    }

    #endregion
}
