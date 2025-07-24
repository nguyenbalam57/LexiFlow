using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.Models
{
    #region Gamification System Models

    /// <summary>
    /// Represents a level in the gamification system
    /// </summary>
    public class Level
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LevelID { get; set; }

        [Required]
        [StringLength(50)]
        public string LevelName { get; set; }

        [Required]
        public int LevelNumber { get; set; }

        [Required]
        public int ExperienceRequired { get; set; }

        [StringLength(255)]
        public string IconPath { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public string Benefits { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<UserLevel> UserLevels { get; set; }
    }

    /// <summary>
    /// Represents a user's level in the gamification system
    /// </summary>
    public class UserLevel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserLevelID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int LevelID { get; set; }

        public int CurrentExperience { get; set; } = 0;

        public int? ExperienceToNextLevel { get; set; }

        public DateTime? LevelUpDate { get; set; }

        public int TotalExperienceEarned { get; set; } = 0;

        public int DaysAtCurrentLevel { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("LevelID")]
        public virtual Level Level { get; set; }
    }

    /// <summary>
    /// Represents a point type in the gamification system
    /// </summary>
    public class PointType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PointTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(255)]
        public string IconPath { get; set; }

        public int Multiplier { get; set; } = 1;

        public bool IsActive { get; set; } = true;

        public int? DisplayOrder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<UserPoint> UserPoints { get; set; }
    }

    /// <summary>
    /// Represents a user's points in the gamification system
    /// </summary>
    public class UserPoint
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserPointID { get; set; }

        [Required]
        public int UserID { get; set; }

        public int? PointTypeID { get; set; }

        [Required]
        public int Points { get; set; }

        public DateTime EarnedAt { get; set; } = DateTime.Now;

        [StringLength(100)]
        public string Source { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int? RelatedEntityID { get; set; }

        [StringLength(50)]
        public string RelatedEntityType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("PointTypeID")]
        public virtual PointType PointType { get; set; }
    }

    /// <summary>
    /// Represents a badge in the gamification system
    /// </summary>
    public class Badge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BadgeID { get; set; }

        [Required]
        [StringLength(100)]
        public string BadgeName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(255)]
        public string IconPath { get; set; }

        public string UnlockCriteria { get; set; }

        public int? RequiredPoints { get; set; }

        [StringLength(50)]
        public string BadgeCategory { get; set; }

        public int? Rarity { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsHidden { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<UserBadge> UserBadges { get; set; }
        public virtual ICollection<Challenge> Challenges { get; set; }
        public virtual ICollection<Achievement> Achievements { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }

    /// <summary>
    /// Represents a user's badge in the gamification system
    /// </summary>
    public class UserBadge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserBadgeID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int BadgeID { get; set; }

        public DateTime EarnedAt { get; set; } = DateTime.Now;

        public bool IsDisplayed { get; set; } = true;

        public bool IsFavorite { get; set; } = false;

        public int EarnCount { get; set; } = 1;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("BadgeID")]
        public virtual Badge Badge { get; set; }
    }

    /// <summary>
    /// Represents a challenge in the gamification system
    /// </summary>
    public class Challenge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChallengeID { get; set; }

        [Required]
        [StringLength(100)]
        public string ChallengeName { get; set; }

        public string Description { get; set; }

        public int? PointsReward { get; set; }

        public int? ExperienceReward { get; set; }

        public int? BadgeID { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? DurationDays { get; set; }

        [StringLength(50)]
        public string ChallengeType { get; set; }

        [StringLength(20)]
        public string Difficulty { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsRecurring { get; set; } = false;

        [StringLength(100)]
        public string RecurrencePattern { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("BadgeID")]
        public virtual Badge Badge { get; set; }

        public virtual ICollection<ChallengeRequirement> ChallengeRequirements { get; set; }
        public virtual ICollection<UserChallenge> UserChallenges { get; set; }
    }

    /// <summary>
    /// Represents a challenge requirement in the gamification system
    /// </summary>
    public class ChallengeRequirement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequirementID { get; set; }

        [Required]
        public int ChallengeID { get; set; }

        [StringLength(50)]
        public string RequirementType { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int? TargetValue { get; set; }

        [StringLength(50)]
        public string EntityType { get; set; }

        public int? EntityID { get; set; }

        public bool IsMandatory { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("ChallengeID")]
        public virtual Challenge Challenge { get; set; }
    }

    /// <summary>
    /// Represents a user's challenge in the gamification system
    /// </summary>
    public class UserChallenge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserChallengeID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int ChallengeID { get; set; }

        public DateTime StartedAt { get; set; } = DateTime.Now;

        public DateTime? CompletedAt { get; set; }

        public int CurrentProgress { get; set; } = 0;

        public int? MaxProgress { get; set; }

        public bool IsCompleted { get; set; } = false;

        public bool IsRewarded { get; set; } = false;

        public bool IsAbandoned { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("ChallengeID")]
        public virtual Challenge Challenge { get; set; }
    }

    /// <summary>
    /// Represents a daily task in the gamification system
    /// </summary>
    public class DailyTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskID { get; set; }

        [Required]
        [StringLength(100)]
        public string TaskName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int? PointsReward { get; set; }

        public int? ExperienceReward { get; set; }

        [StringLength(50)]
        public string TaskCategory { get; set; }

        [StringLength(50)]
        public string TaskType { get; set; }

        public int? Difficulty { get; set; }

        public bool IsActive { get; set; } = true;

        public int? RecurrenceDays { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<DailyTaskRequirement> DailyTaskRequirements { get; set; }
        public virtual ICollection<UserDailyTask> UserDailyTasks { get; set; }
    }

    /// <summary>
    /// Represents a daily task requirement in the gamification system
    /// </summary>
    public class DailyTaskRequirement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequirementID { get; set; }

        [Required]
        public int TaskID { get; set; }

        [StringLength(50)]
        public string RequirementType { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int? TargetValue { get; set; }

        [StringLength(50)]
        public string EntityType { get; set; }

        public int? EntityID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("TaskID")]
        public virtual DailyTask Task { get; set; }
    }

    /// <summary>
    /// Represents a user's daily task in the gamification system
    /// </summary>
    public class UserDailyTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserTaskID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int TaskID { get; set; }

        [Required]
        public DateTime TaskDate { get; set; }

        public DateTime? CompletedAt { get; set; }

        public int CurrentProgress { get; set; } = 0;

        public int? MaxProgress { get; set; }

        public bool IsCompleted { get; set; } = false;

        public bool IsRewarded { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("TaskID")]
        public virtual DailyTask Task { get; set; }
    }

    /// <summary>
    /// Represents an achievement in the gamification system
    /// </summary>
    public class Achievement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AchievementID { get; set; }

        [Required]
        [StringLength(100)]
        public string AchievementName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(255)]
        public string IconPath { get; set; }

        public int? PointsReward { get; set; }

        public int? ExperienceReward { get; set; }

        public int? BadgeID { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        public int? Tier { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsSecret { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("BadgeID")]
        public virtual Badge Badge { get; set; }

        public virtual ICollection<AchievementRequirement> AchievementRequirements { get; set; }
        public virtual ICollection<UserAchievement> UserAchievements { get; set; }
    }

    /// <summary>
    /// Represents an achievement requirement in the gamification system
    /// </summary>
    public class AchievementRequirement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequirementID { get; set; }

        [Required]
        public int AchievementID { get; set; }

        [StringLength(50)]
        public string RequirementType { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int? TargetValue { get; set; }

        [StringLength(50)]
        public string EntityType { get; set; }

        public int? EntityID { get; set; }

        public bool IsMandatory { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("AchievementID")]
        public virtual Achievement Achievement { get; set; }
    }

    /// <summary>
    /// Represents a user's achievement in the gamification system
    /// </summary>
    public class UserAchievement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserAchievementID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int AchievementID { get; set; }

        public DateTime UnlockedAt { get; set; } = DateTime.Now;

        public int CurrentTier { get; set; } = 1;

        public int? MaxTier { get; set; }

        public int ProgressValue { get; set; } = 0;

        public int? TargetValue { get; set; }

        public bool IsDisplayed { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("AchievementID")]
        public virtual Achievement Achievement { get; set; }
    }

    /// <summary>
    /// Represents a user's streak in the gamification system
    /// </summary>
    public class UserStreak
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StreakID { get; set; }

        [Required]
        public int UserID { get; set; }

        [StringLength(50)]
        public string StreakType { get; set; }

        public int CurrentCount { get; set; } = 0;

        public int LongestCount { get; set; } = 0;

        public DateTime? LastActivityDate { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime StartedAt { get; set; } = DateTime.Now;

        public int TotalStreakDays { get; set; } = 0;

        public int StreakFreezes { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }

    /// <summary>
    /// Represents a leaderboard in the gamification system
    /// </summary>
    public class Leaderboard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeaderboardID { get; set; }

        [Required]
        [StringLength(100)]
        public string LeaderboardName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(50)]
        public string LeaderboardType { get; set; }

        [StringLength(20)]
        public string TimeFrame { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ResetPeriodDays { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(50)]
        public string Scope { get; set; }

        [StringLength(50)]
        public string EntityType { get; set; }

        public int? EntityID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<LeaderboardEntry> LeaderboardEntries { get; set; }
    }

    /// <summary>
    /// Represents a leaderboard entry in the gamification system
    /// </summary>
    public class LeaderboardEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EntryID { get; set; }

        [Required]
        public int LeaderboardID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int Score { get; set; }

        public int? Rank { get; set; }

        public int? PreviousRank { get; set; }

        public int? RankChange { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("LeaderboardID")]
        public virtual Leaderboard Leaderboard { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }

    /// <summary>
    /// Represents an event in the gamification system
    /// </summary>
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventID { get; set; }

        [Required]
        [StringLength(100)]
        public string EventName { get; set; }

        public string Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(50)]
        public string EventType { get; set; }

        [StringLength(50)]
        public string RewardType { get; set; }

        public int? RewardValue { get; set; }

        public int? BadgeID { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(50)]
        public string ParticipationType { get; set; }

        public int? MaxParticipants { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("BadgeID")]
        public virtual Badge Badge { get; set; }

        public virtual ICollection<UserEvent> UserEvents { get; set; }
    }

    /// <summary>
    /// Represents a user's event in the gamification system
    /// </summary>
    public class UserEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserEventID { get; set; }

        [Required]
        public int EventID { get; set; }

        [Required]
        public int UserID { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.Now;

        public int Score { get; set; } = 0;

        public int? Rank { get; set; }

        public bool HasCompleted { get; set; } = false;

        public bool IsRewarded { get; set; } = false;

        public DateTime? LastActivityAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("EventID")]
        public virtual Event Event { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }

    /// <summary>
    /// Represents a user gift in the gamification system
    /// </summary>
    public class UserGift
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GiftID { get; set; }

        [Required]
        public int SenderUserID { get; set; }

        [Required]
        public int ReceiverUserID { get; set; }

        [StringLength(50)]
        public string GiftType { get; set; }

        public int? GiftValue { get; set; }

        public string Message { get; set; }

        public DateTime SentAt { get; set; } = DateTime.Now;

        public DateTime? ReceivedAt { get; set; }

        public bool IsUsed { get; set; } = false;

        public bool IsExpired { get; set; } = false;

        public DateTime? ExpirationDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("SenderUserID")]
        public virtual User SenderUser { get; set; }

        [ForeignKey("ReceiverUserID")]
        public virtual User ReceiverUser { get; set; }
    }

    #endregion
}