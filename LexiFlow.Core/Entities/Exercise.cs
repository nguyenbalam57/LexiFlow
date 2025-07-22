using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LexiFlow.Core.Entities
{
    /// <summary>
    /// Represents an exercise or quiz in the system
    /// </summary>
    public class Exercise : BaseEntity
    {
        /// <summary>
        /// Exercise title
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Exercise description or instructions
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Exercise type (MultipleChoice, FillInBlank, Matching, Flashcard, Dictation, etc.)
        /// </summary>
        [StringLength(50)]
        public string Type { get; set; } = "MultipleChoice";

        /// <summary>
        /// Language code this exercise is in
        /// </summary>
        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; } = "en";

        /// <summary>
        /// Target language code being tested
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
        public int DurationMinutes { get; set; } = 5;

        /// <summary>
        /// Passing score percentage (0-100%)
        /// </summary>
        public int PassingScore { get; set; } = 70;

        /// <summary>
        /// Maximum score possible
        /// </summary>
        public int MaxScore { get; set; } = 100;

        /// <summary>
        /// Status of the exercise (Draft, Published, Archived)
        /// </summary>
        [StringLength(20)]
        public string Status { get; set; } = "Draft";

        /// <summary>
        /// Tags for this exercise (comma-separated)
        /// </summary>
        [StringLength(255)]
        public string? Tags { get; set; }

        /// <summary>
        /// Category ID this exercise belongs to
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Navigation property for Category
        /// </summary>
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        /// <summary>
        /// Lesson ID this exercise belongs to
        /// </summary>
        public int? LessonId { get; set; }

        /// <summary>
        /// Navigation property for Lesson
        /// </summary>
        [ForeignKey("LessonId")]
        public virtual Lesson? Lesson { get; set; }

        /// <summary>
        /// Flag for randomizing question order
        /// </summary>
        public bool RandomizeQuestions { get; set; } = false;

        /// <summary>
        /// Flag for time limit
        /// </summary>
        public bool HasTimeLimit { get; set; } = false;

        /// <summary>
        /// Time limit in minutes (if HasTimeLimit is true)
        /// </summary>
        public int? TimeLimitMinutes { get; set; }

        /// <summary>
        /// Maximum attempts allowed (null for unlimited)
        /// </summary>
        public int? MaxAttempts { get; set; }

        /// <summary>
        /// Flag for showing correct answers after submission
        /// </summary>
        public bool ShowAnswersAfterSubmit { get; set; } = true;

        /// <summary>
        /// Flag for showing explanation after submission
        /// </summary>
        public bool ShowExplanationAfterSubmit { get; set; } = true;

        /// <summary>
        /// Flag for premium content
        /// </summary>
        public bool IsPremium { get; set; } = false;

        /// <summary>
        /// Navigation property for questions in this exercise
        /// </summary>
        public virtual ICollection<ExerciseQuestion> Questions { get; set; } = new List<ExerciseQuestion>();

        /// <summary>
        /// Navigation property for vocabulary items in this exercise
        /// </summary>
        public virtual ICollection<ExerciseVocabulary> VocabularyItems { get; set; } = new List<ExerciseVocabulary>();

        /// <summary>
        /// Navigation property for user attempts
        /// </summary>
        public virtual ICollection<UserExerciseAttempt> UserAttempts { get; set; } = new List<UserExerciseAttempt>();
    }

    /// <summary>
    /// Represents a question within an exercise
    /// </summary>
    public class ExerciseQuestion : BaseEntity
    {
        /// <summary>
        /// Exercise ID this question belongs to
        /// </summary>
        public int ExerciseId { get; set; }

        /// <summary>
        /// Navigation property for Exercise
        /// </summary>
        [ForeignKey("ExerciseId")]
        public virtual Exercise? Exercise { get; set; }

        /// <summary>
        /// Question text or prompt
        /// </summary>
        [Required]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Question type (MultipleChoice, FillInBlank, Matching, etc.)
        /// </summary>
        [StringLength(50)]
        public string Type { get; set; } = "MultipleChoice";

        /// <summary>
        /// Instructions specific to this question
        /// </summary>
        public string? Instructions { get; set; }

        /// <summary>
        /// Points for this question
        /// </summary>
        public int Points { get; set; } = 10;

        /// <summary>
        /// Order of this question within the exercise
        /// </summary>
        public int Order { get; set; } = 0;

        /// <summary>
        /// Media URL (image, audio, video)
        /// </summary>
        [StringLength(255)]
        public string? MediaUrl { get; set; }

        /// <summary>
        /// Media type (Image, Audio, Video)
        /// </summary>
        [StringLength(20)]
        public string? MediaType { get; set; }

        /// <summary>
        /// Difficulty level (1-5)
        /// </summary>
        public int DifficultyLevel { get; set; } = 1;

        /// <summary>
        /// Explanation or hint
        /// </summary>
        public string? Explanation { get; set; }

        /// <summary>
        /// Tags for this question (comma-separated)
        /// </summary>
        [StringLength(255)]
        public string? Tags { get; set; }

        /// <summary>
        /// Options for multiple choice questions (JSON array)
        /// </summary>
        public string? Options { get; set; }

        /// <summary>
        /// Correct answers (JSON format - depends on question type)
        /// </summary>
        public string? CorrectAnswers { get; set; }

        /// <summary>
        /// Deserialized options for multiple choice questions
        /// </summary>
        [NotMapped]
        public List<QuestionOption>? OptionsList
        {
            get
            {
                if (string.IsNullOrEmpty(Options))
                {
                    return new List<QuestionOption>();
                }

                try
                {
                    return JsonSerializer.Deserialize<List<QuestionOption>>(Options);
                }
                catch
                {
                    return new List<QuestionOption>();
                }
            }
            set
            {
                if (value == null)
                {
                    Options = null;
                }
                else
                {
                    Options = JsonSerializer.Serialize(value);
                }
            }
        }

        /// <summary>
        /// Gets the correct answer string
        /// </summary>
        public string GetCorrectAnswerDisplay()
        {
            if (string.IsNullOrEmpty(CorrectAnswers))
            {
                return string.Empty;
            }

            try
            {
                // Handle different question types
                switch (Type.ToLower())
                {
                    case "multiplechoice":
                    case "singlechoice":
                        var correctIndices = JsonSerializer.Deserialize<List<int>>(CorrectAnswers);
                        if (correctIndices == null || OptionsList == null)
                        {
                            return string.Empty;
                        }

                        var correctOptions = correctIndices
                            .Select(i => OptionsList.ElementAtOrDefault(i)?.Text ?? string.Empty)
                            .Where(t => !string.IsNullOrEmpty(t));

                        return string.Join(", ", correctOptions);

                    case "fillinblank":
                    case "shortanswer":
                        var answers = JsonSerializer.Deserialize<List<string>>(CorrectAnswers);
                        return answers != null ? string.Join(", ", answers) : string.Empty;

                    case "matching":
                        var matchPairs = JsonSerializer.Deserialize<Dictionary<string, string>>(CorrectAnswers);
                        if (matchPairs == null)
                        {
                            return string.Empty;
                        }

                        return string.Join("; ", matchPairs.Select(p => $"{p.Key} → {p.Value}"));

                    default:
                        return CorrectAnswers;
                }
            }
            catch
            {
                return CorrectAnswers ?? string.Empty;
            }
        }
    }

    /// <summary>
    /// Represents an option in a multiple choice question
    /// </summary>
    public class QuestionOption
    {
        /// <summary>
        /// Option text
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Flag for correct option
        /// </summary>
        public bool IsCorrect { get; set; } = false;

        /// <summary>
        /// Feedback for this option
        /// </summary>
        public string? Feedback { get; set; }

        /// <summary>
        /// Image URL for this option
        /// </summary>
        public string? ImageUrl { get; set; }
    }

    /// <summary>
    /// Represents the vocabulary items associated with an exercise
    /// </summary>
    public class ExerciseVocabulary : BaseEntity
    {
        /// <summary>
        /// Exercise ID
        /// </summary>
        public int ExerciseId { get; set; }

        /// <summary>
        /// Navigation property for Exercise
        /// </summary>
        [ForeignKey("ExerciseId")]
        public virtual Exercise? Exercise { get; set; }

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
        /// Order of this vocabulary item within the exercise
        /// </summary>
        public int Order { get; set; } = 0;

        /// <summary>
        /// Notes specific to this vocabulary item in this exercise
        /// </summary>
        public string? Notes { get; set; }
    }

    /// <summary>
    /// Tracks user attempts at exercises
    /// </summary>
    public class UserExerciseAttempt : BaseEntity
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
        /// Exercise ID
        /// </summary>
        public int ExerciseId { get; set; }

        /// <summary>
        /// Navigation property for Exercise
        /// </summary>
        [ForeignKey("ExerciseId")]
        public virtual Exercise? Exercise { get; set; }

        /// <summary>
        /// Start time of the attempt
        /// </summary>
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Submission time of the attempt
        /// </summary>
        public DateTime? SubmittedAt { get; set; }

        /// <summary>
        /// Time spent in seconds
        /// </summary>
        public int TimeSpentSeconds { get; set; } = 0;

        /// <summary>
        /// Score achieved (points)
        /// </summary>
        public int Score { get; set; } = 0;

        /// <summary>
        /// Maximum possible score
        /// </summary>
        public int MaxScore { get; set; } = 0;

        /// <summary>
        /// Score as percentage (0-100%)
        /// </summary>
        public int ScorePercentage { get; set; } = 0;

        /// <summary>
        /// Status (In Progress, Completed, Timed Out)
        /// </summary>
        [StringLength(20)]
        public string Status { get; set; } = "In Progress";

        /// <summary>
        /// Flag for passing attempt
        /// </summary>
        public bool IsPassed { get; set; } = false;

        /// <summary>
        /// Attempt number for this user on this exercise
        /// </summary>
        public int AttemptNumber { get; set; } = 1;

        /// <summary>
        /// User answers (JSON format - depends on exercise type)
        /// </summary>
        public string? UserAnswers { get; set; }

        /// <summary>
        /// Question-by-question results (JSON format)
        /// </summary>
        public string? QuestionResults { get; set; }

        /// <summary>
        /// Feedback from the system
        /// </summary>
        public string? SystemFeedback { get; set; }

        /// <summary>
        /// Feedback from an instructor
        /// </summary>
        public string? InstructorFeedback { get; set; }

        /// <summary>
        /// Feedback from the user
        /// </summary>
        public string? UserFeedback { get; set; }

        /// <summary>
        /// User rating of this exercise (1-5)
        /// </summary>
        public int? UserRating { get; set; }
    }
}