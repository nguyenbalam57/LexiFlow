using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.Core.Entities
{
    /// <summary>
    /// Represents a language learning course
    /// </summary>
    public class Course : BaseEntity
    {
        /// <summary>
        /// Course title
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Course subtitle
        /// </summary>
        [StringLength(500)]
        public string? Subtitle { get; set; }

        /// <summary>
        /// Course description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Syllabus or overview
        /// </summary>
        public string? Syllabus { get; set; }

        /// <summary>
        /// Language code this course is taught in
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
        /// Proficiency level (Beginner, Elementary, Intermediate, Advanced, Proficient)
        /// </summary>
        [StringLength(20)]
        public string Level { get; set; } = "Beginner";

        /// <summary>
        /// Difficulty level (1-5)
        /// </summary>
        public int DifficultyLevel { get; set; } = 1;

        /// <summary>
        /// Estimated duration in hours
        /// </summary>
        public double DurationHours { get; set; } = 10;

        /// <summary>
        /// Status of the course (Draft, Published, Archived)
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
        /// Promotional video URL
        /// </summary>
        [StringLength(255)]
        public string? PromoVideoUrl { get; set; }

        /// <summary>
        /// Category ID this course belongs to
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Navigation property for Category
        /// </summary>
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        /// <summary>
        /// Price in USD (0 for free courses)
        /// </summary>
        public decimal Price { get; set; } = 0;

        /// <summary>
        /// Discount price in USD
        /// </summary>
        public decimal? DiscountPrice { get; set; }

        /// <summary>
        /// Discount valid until
        /// </summary>
        public DateTime? DiscountValidUntil { get; set; }

        /// <summary>
        /// Prerequisites or requirements
        /// </summary>
        public string? Prerequisites { get; set; }

        /// <summary>
        /// What students will learn (bullet points)
        /// </summary>
        public string? LearningOutcomes { get; set; }

        /// <summary>
        /// Target audience
        /// </summary>
        public string? TargetAudience { get; set; }

        /// <summary>
        /// Tags for this course (comma-separated)
        /// </summary>
        [StringLength(255)]
        public string? Tags { get; set; }

        /// <summary>
        /// Flag for featured courses
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
        /// Enrollment count
        /// </summary>
        public int EnrollmentCount { get; set; } = 0;

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
        /// Certificate available upon completion
        /// </summary>
        public bool HasCertificate { get; set; } = false;

        /// <summary>
        /// Navigation property for lessons in this course
        /// </summary>
        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

        /// <summary>
        /// Navigation property for modules in this course
        /// </summary>
        public virtual ICollection<CourseModule> Modules { get; set; } = new List<CourseModule>();

        /// <summary>
        /// Navigation property for user enrollments
        /// </summary>
        public virtual ICollection<UserEnrollment> Enrollments { get; set; } = new List<UserEnrollment>();

        /// <summary>
        /// Instructor IDs (comma-separated)
        /// </summary>
        public string? InstructorIds { get; set; }
    }

    /// <summary>
    /// Represents a module within a course
    /// </summary>
    public class CourseModule : BaseEntity
    {
        /// <summary>
        /// Course ID this module belongs to
        /// </summary>
        public int CourseId { get; set; }

        /// <summary>
        /// Navigation property for Course
        /// </summary>
        [ForeignKey("CourseId")]
        public virtual Course? Course { get; set; }

        /// <summary>
        /// Module title
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Module description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Order of this module within its course
        /// </summary>
        public int Order { get; set; } = 0;

        /// <summary>
        /// Estimated duration in hours
        /// </summary>
        public double DurationHours { get; set; } = 1;

        /// <summary>
        /// Navigation property for lessons in this module
        /// </summary>
        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    }

    /// <summary>
    /// Tracks user enrollment in courses
    /// </summary>
    public class UserEnrollment : BaseEntity
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
        /// Course ID
        /// </summary>
        public int CourseId { get; set; }

        /// <summary>
        /// Navigation property for Course
        /// </summary>
        [ForeignKey("CourseId")]
        public virtual Course? Course { get; set; }

        /// <summary>
        /// Enrollment date
        /// </summary>
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

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
        /// Last lesson ID viewed
        /// </summary>
        public int? LastLessonId { get; set; }

        /// <summary>
        /// User rating (1-5)
        /// </summary>
        public int? Rating { get; set; }

        /// <summary>
        /// User review/feedback
        /// </summary>
        public string? Review { get; set; }

        /// <summary>
        /// Payment ID if purchased
        /// </summary>
        [StringLength(100)]
        public string? PaymentId { get; set; }

        /// <summary>
        /// Price paid
        /// </summary>
        public decimal? PricePaid { get; set; }

        /// <summary>
        /// Flag for free/scholarship enrollment
        /// </summary>
        public bool IsFreeEnrollment { get; set; } = false;

        /// <summary>
        /// Certificate issue date
        /// </summary>
        public DateTime? CertificateIssuedAt { get; set; }

        /// <summary>
        /// Certificate URL or ID
        /// </summary>
        [StringLength(255)]
        public string? CertificateUrl { get; set; }

        /// <summary>
        /// Expiration date for access
        /// </summary>
        public DateTime? ExpiresAt { get; set; }
    }
}