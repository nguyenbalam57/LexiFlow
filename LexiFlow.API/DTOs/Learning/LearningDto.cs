using LexiFlow.API.DTOs.Vocabulary;
using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Learning
{
    /// <summary>
    /// Learning progress DTO for responses
    /// </summary>
    public class LearningProgressDto : BaseDto
    {
        /// <summary>
        /// Progress ID
        /// </summary>
        public new int Id { get; set; }

        /// <summary>
        /// User ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Vocabulary ID
        /// </summary>
        public int VocabularyID { get; set; }

        /// <summary>
        /// Vocabulary details
        /// </summary>
        public VocabularyDto? Vocabulary { get; set; }

        /// <summary>
        /// Number of study sessions
        /// </summary>
        public int StudyCount { get; set; }

        /// <summary>
        /// Number of correct answers
        /// </summary>
        public int CorrectCount { get; set; }

        /// <summary>
        /// Number of incorrect answers
        /// </summary>
        public int IncorrectCount { get; set; }

        /// <summary>
        /// Last study timestamp
        /// </summary>
        public DateTime? LastStudied { get; set; }

        /// <summary>
        /// Memory strength (0-100)
        /// </summary>
        public int MemoryStrength { get; set; }

        /// <summary>
        /// Next review due date
        /// </summary>
        public DateTime? NextReviewDate { get; set; }

        /// <summary>
        /// Mastery percentage
        /// </summary>
        public int MasteryPercentage => CalculateMastery();

        private int CalculateMastery()
        {
            if (StudyCount == 0) return 0;
            var correctRate = (float)CorrectCount / (CorrectCount + IncorrectCount);
            return (int)(correctRate * MemoryStrength);
        }
    }

    /// <summary>
    /// Study session result DTO
    /// </summary>
    public class StudySessionResultDto
    {
        /// <summary>
        /// User ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// List of vocabulary study results
        /// </summary>
        public List<VocabularyStudyResultDto> Results { get; set; } = new List<VocabularyStudyResultDto>();
    }

    /// <summary>
    /// Vocabulary study result DTO
    /// </summary>
    public class VocabularyStudyResultDto
    {
        /// <summary>
        /// Vocabulary ID
        /// </summary>
        public int VocabularyID { get; set; }

        /// <summary>
        /// Whether the answer was correct
        /// </summary>
        public bool IsCorrect { get; set; }

        /// <summary>
        /// Time spent (seconds)
        /// </summary>
        public int? TimeSpent { get; set; }

        /// <summary>
        /// Difficulty rating (1-5)
        /// </summary>
        public int? DifficultyRating { get; set; }
    }

    /// <summary>
    /// Personal word list DTO for responses
    /// </summary>
    public class PersonalWordListDto : BaseDto
    {
        /// <summary>
        /// List ID
        /// </summary>
        public new int Id { get; set; }

        /// <summary>
        /// List name
        /// </summary>
        public string ListName { get; set; } = null!;

        /// <summary>
        /// List description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// User ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Number of items in the list
        /// </summary>
        public int ItemCount { get; set; }
    }

    /// <summary>
    /// Create personal word list request DTO
    /// </summary>
    public class CreatePersonalWordListDto
    {
        /// <summary>
        /// List name
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ListName { get; set; } = null!;

        /// <summary>
        /// List description
        /// </summary>
        [StringLength(255)]
        public string? Description { get; set; }
    }

    /// <summary>
    /// Update personal word list request DTO
    /// </summary>
    public class UpdatePersonalWordListDto
    {
        /// <summary>
        /// List name
        /// </summary>
        [StringLength(100)]
        public string? ListName { get; set; }

        /// <summary>
        /// List description
        /// </summary>
        [StringLength(255)]
        public string? Description { get; set; }
    }

    /// <summary>
    /// Personal word list item DTO for responses
    /// </summary>
    public class PersonalWordListItemDto
    {
        /// <summary>
        /// Item ID
        /// </summary>
        public int ItemID { get; set; }

        /// <summary>
        /// List ID
        /// </summary>
        public int ListID { get; set; }

        /// <summary>
        /// Vocabulary ID
        /// </summary>
        public int VocabularyID { get; set; }

        /// <summary>
        /// Vocabulary details
        /// </summary>
        public VocabularyDto? Vocabulary { get; set; }

        /// <summary>
        /// Added timestamp
        /// </summary>
        public DateTime AddedAt { get; set; }
    }

    /// <summary>
    /// Add word to list request DTO
    /// </summary>
    public class AddWordToListDto
    {
        /// <summary>
        /// Vocabulary ID
        /// </summary>
        [Required]
        public int VocabularyID { get; set; }
    }

    /// <summary>
    /// Study plan DTO for responses
    /// </summary>
    public class StudyPlanDto : BaseDto
    {
        /// <summary>
        /// Plan ID
        /// </summary>
        public new int Id { get; set; }

        /// <summary>
        /// User ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Plan name
        /// </summary>
        public string PlanName { get; set; } = null!;

        /// <summary>
        /// Target JLPT level
        /// </summary>
        public string? TargetLevel { get; set; }

        /// <summary>
        /// Start date
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Target completion date
        /// </summary>
        public DateTime? TargetDate { get; set; }

        /// <summary>
        /// Plan description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Minutes per day goal
        /// </summary>
        public int? MinutesPerDay { get; set; }

        /// <summary>
        /// Whether the plan is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Current status
        /// </summary>
        public string? CurrentStatus { get; set; }

        /// <summary>
        /// Completion percentage
        /// </summary>
        public float CompletionPercentage { get; set; }

        /// <summary>
        /// Last update timestamp
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Study goals count
        /// </summary>
        public int GoalsCount { get; set; }
    }

    /// <summary>
    /// Create study plan request DTO
    /// </summary>
    public class CreateStudyPlanDto
    {
        /// <summary>
        /// Plan name
        /// </summary>
        [Required]
        [StringLength(100)]
        public string PlanName { get; set; } = null!;

        /// <summary>
        /// Target JLPT level
        /// </summary>
        [StringLength(10)]
        public string? TargetLevel { get; set; }

        /// <summary>
        /// Start date
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Target completion date
        /// </summary>
        public DateTime? TargetDate { get; set; }

        /// <summary>
        /// Plan description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Minutes per day goal
        /// </summary>
        public int? MinutesPerDay { get; set; }
    }

    /// <summary>
    /// Study goal DTO for responses
    /// </summary>
    public class StudyGoalDto : BaseDto
    {
        /// <summary>
        /// Goal ID
        /// </summary>
        public new int Id { get; set; }

        /// <summary>
        /// Plan ID
        /// </summary>
        public int PlanID { get; set; }

        /// <summary>
        /// Goal name
        /// </summary>
        public string GoalName { get; set; } = null!;

        /// <summary>
        /// Goal description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// JLPT level ID
        /// </summary>
        public int? LevelID { get; set; }

        /// <summary>
        /// JLPT level name
        /// </summary>
        public string? LevelName { get; set; }

        /// <summary>
        /// Study topic ID
        /// </summary>
        public int? TopicID { get; set; }

        /// <summary>
        /// Study topic name
        /// </summary>
        public string? TopicName { get; set; }

        /// <summary>
        /// Target completion date
        /// </summary>
        public DateTime? TargetDate { get; set; }

        /// <summary>
        /// Importance (1-5)
        /// </summary>
        public int? Importance { get; set; }

        /// <summary>
        /// Difficulty (1-5)
        /// </summary>
        public int? Difficulty { get; set; }

        /// <summary>
        /// Whether the goal is completed
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Progress percentage
        /// </summary>
        public float ProgressPercentage { get; set; }

        /// <summary>
        /// Tasks count
        /// </summary>
        public int TasksCount { get; set; }

        /// <summary>
        /// Completed tasks count
        /// </summary>
        public int CompletedTasksCount { get; set; }
    }

    /// <summary>
    /// Create study goal request DTO
    /// </summary>
    public class CreateStudyGoalDto
    {
        /// <summary>
        /// Goal name
        /// </summary>
        [Required]
        [StringLength(100)]
        public string GoalName { get; set; } = null!;

        /// <summary>
        /// Goal description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// JLPT level ID
        /// </summary>
        public int? LevelID { get; set; }

        /// <summary>
        /// Study topic ID
        /// </summary>
        public int? TopicID { get; set; }

        /// <summary>
        /// Target completion date
        /// </summary>
        public DateTime? TargetDate { get; set; }

        /// <summary>
        /// Importance (1-5)
        /// </summary>
        [Range(1, 5)]
        public int? Importance { get; set; }

        /// <summary>
        /// Difficulty (1-5)
        /// </summary>
        [Range(1, 5)]
        public int? Difficulty { get; set; }
    }

    /// <summary>
    /// Update study goal request DTO
    /// </summary>
    public class UpdateStudyGoalDto
    {
        /// <summary>
        /// Goal name
        /// </summary>
        [StringLength(100)]
        public string? GoalName { get; set; }

        /// <summary>
        /// Goal description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// JLPT level ID
        /// </summary>
        public int? LevelID { get; set; }

        /// <summary>
        /// Study topic ID
        /// </summary>
        public int? TopicID { get; set; }

        /// <summary>
        /// Target completion date
        /// </summary>
        public DateTime? TargetDate { get; set; }

        /// <summary>
        /// Importance (1-5)
        /// </summary>
        [Range(1, 5)]
        public int? Importance { get; set; }

        /// <summary>
        /// Difficulty (1-5)
        /// </summary>
        [Range(1, 5)]
        public int? Difficulty { get; set; }

        /// <summary>
        /// Whether the goal is completed
        /// </summary>
        public bool? IsCompleted { get; set; }

        /// <summary>
        /// Progress percentage
        /// </summary>
        [Range(0, 100)]
        public float? ProgressPercentage { get; set; }
    }
}