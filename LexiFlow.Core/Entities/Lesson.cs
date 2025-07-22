using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.Core.Entities
{
    /// <summary>
    /// Represents a lesson in the system
    /// </summary>
    public class Lesson : BaseEntity
    {
        /// <summary>
        /// Lesson title
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Lesson description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Course ID this lesson belongs to
        /// </summary>
        public int? CourseId { get; set; }

        /// <summary>
        /// Navigation property for Course
        /// </summary>
        [ForeignKey("CourseId")]
        public virtual Course? Course { get; set; }

        /// <summary>
        /// Language code this lesson is taught in
        /// </summary>
        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; } = "en";

        /// <summary>
        /// Target language code being taught
        /// </summary>
        [Required]
        [StringLength(10)]
        public string TargetLanguageCode { get; set; } = string.Empty;

        /// <summary>
        /// Difficulty level (1-5)
        /// </summary>
        public int DifficultyLevel { get; set; } = 1;

        /// <summary>
        /// Estimated duration in minutes
        /// </summary>
        public int DurationMinutes { get; set; } = 15;

        /// <summary>
        /// Order of this lesson within its course
        /// </summary>
        public int Order { get; set; } = 0;

        /// <summary>
        /// Status of the lesson (Draft, Published, Archived)
        /// </summary>
        [StringLength(20)]
        public string Status { get; set; } = "Draft";

        /// <summary>
        /// Publication date
        /// </summary>
        public DateTime? PublishedAt { get; set; }

        /// <summary>
        /// Cover image URL
        /// </summary>
        [StringLength(255)]
        public string? CoverImageUrl { get; set; }

        /// <summary>
        /// Introduction or summary for this lesson
        /// </summary>
        public string? Introduction { get; set; }

        /// <summary>
        /// Tags for this lesson (comma-separated)
        /// </summary>
        [StringLength(255)]
        public string? Tags { get; set; }

        /// <summary>
        /// Flag for featured lessons
        /// </summary>
        public bool IsFeatured { get; set; } = false;

        /// <summary>
        /// Flag for premium content
        /// </summary>
        public bool IsPremium { get; set; } = false;

        /// <summary>
        /// View count
        /// </summary>
        public int ViewCount { get; set; } = 0;

        /// <summary>
        /// Average rating (1-5)
        /// </summary>
        public double? AverageRating { get; set; }

        /// <summary>
        /// Number of ratings
        /// </summary>
        public int RatingCount { get; set; } = 0;

        /// <summary>
        /// SEO-friendly URL slug
        /// </summary>
        [StringLength(250)]
        public string? Slug { get; set; }

        /// <summary>
        /// Navigation property for sections in this lesson
        /// </summary>
        public virtual ICollection<LessonSection> Sections { get; set; } = new List<LessonSection>();

        /// <summary>
        /// Navigation property for vocabulary items in this lesson
        /// </summary>
        public virtual ICollection<LessonVocabulary> VocabularyItems { get; set; } = new List<LessonVocabulary>();

        /// <summary>
        /// Navigation property for user progress
        /// </summary>
        public virtual ICollection<UserLessonProgress> UserProgress { get; set; } = new List<UserLessonProgress>();
    }

    /// <summary>
    /// Represents a section within a lesson
    /// </summary>
    public class LessonSection : BaseEntity
    {
        /// <summary>
        /// Lesson ID this section belongs to
        /// </summary>
        public int LessonId { get; set; }

        /// <summary>
        /// Navigation property for Lesson
        /// </summary>
        [ForeignKey("LessonId")]
        public virtual Lesson? Lesson { get; set; }

        /// <summary>
        /// Section title
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Section content (HTML or Markdown)
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// Content type (Text, Video, Audio, Quiz, Exercise)
        /// </summary>
        [StringLength(20)]
        public string ContentType { get; set; } = "Text";

        /// <summary>
        /// Media URL for video or audio content
        /// </summary>
        [StringLength(255)]
        public string? MediaUrl { get; set; }

        /// <summary>
        /// Order of this section within its lesson
        /// </summary>
        public int Order { get; set; } = 0;

        /// <summary>
        /// Estimated duration in minutes
        /// </summary>
        public int DurationMinutes { get; set; } = 5;

        /// <summary>
        /// Flag for optional sections
        /// </summary>
        public bool IsOptional { get; set; } = false;

        /// <summary>
        /// Quiz or exercise ID associated with this section
        /// </summary>
        public int? ExerciseId { get; set; }

        /// <summary>
        /// Navigation property for Exercise
        /// </summary>
        [ForeignKey("ExerciseId")]
        public virtual Exercise? Exercise { get; set; }
    }

    /// <summary>
    /// Represents the vocabulary items associated with a lesson
    /// </summary>
    public class LessonVocabulary : BaseEntity
    {
        /// <summary>
        /// Lesson ID
        /// </summary>
        public int LessonId { get; set; }

        /// <summary>
        /// Navigation property for Lesson
        /// </summary>
        [ForeignKey("LessonId")]
        public virtual Lesson? Lesson { get; set; }

        /// <summary>
        /// Vocabulary item ID
        /// </summary>
        public int VocabularyItemId { get; set; }

        /// <summary>
        /// Navigation property for VocabularyItem
        /// </summary>
        [ForeignKey("VocabularyItemId")]
        public virtual VocabularyItem? VocabularyItem { get; set; }

        /// <summary>
        /// Order of this vocabulary item within the lesson
        /// </summary>
        public int Order { get; set; } = 0;

        /// <summary>
        /// Notes specific to this vocabulary item in this lesson
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Custom example for this vocabulary item in this lesson context
        /// </summary>
        public string? CustomExample { get; set; }
    }

    /// <summary>
    /// Tracks user progress for lessons
    /// </summary>
    public class UserLessonProgress : BaseEntity
    {
        /// <summary>
        /// User ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Navigation property for User
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        /// <summary>
        /// Lesson ID
        /// </summary>
        public int LessonId { get; set; }

        /// <summary>
        /// Navigation property for Lesson
        /// </summary>
        [ForeignKey("LessonId")]
        public virtual Lesson? Lesson { get; set; }

        /// <summary>
        /// Completion status as percentage (0-100%)
        /// </summary>
        public int CompletionPercentage { get; set; } = 0;

        /// <summary>
        /// Status (Not Started, In Progress, Completed)
        /// </summary>
        [StringLength(20)]
        public string Status { get; set; } = "Not Started";

        /// <summary>
        /// Start date
        /// </summary>
        public DateTime? StartedAt { get; set; }

        /// <summary>
        /// Completion date
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// Time spent in minutes
        /// </summary>
        public int TimeSpentMinutes { get; set; } = 0;

        /// <summary>
        /// Last section ID viewed
        /// </summary>
        public int? LastSectionId { get; set; }

        /// <summary>
        /// User rating (1-5)
        /// </summary>
        public int? Rating { get; set; }

        /// <summary>
        /// User review/feedback
        /// </summary>
        public string? Review { get; set; }

        /// <summary>
        /// Flag for bookmarked lessons
        /// </summary>
        public bool IsBookmarked { get; set; } = false;

        /// <summary>
        /// User notes for this lesson
        /// </summary>
        public string? Notes { get; set; }
    }
}