using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.Models
{
    #region Core User Management Models

    /// <summary>
    /// Represents a user in the system
    /// </summary>
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50)]
        [Phone]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string Position { get; set; }

        public DateTime? LastLogin { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [StringLength(255)]
        public string ApiKey { get; set; }

        public DateTime? ApiKeyExpiry { get; set; }

        public int? DepartmentID { get; set; }

        [ForeignKey("DepartmentID")]
        public Department Department_Navigation { get; set; }

        // Navigation properties
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserPermission> UserPermissions { get; set; }
        public virtual ICollection<UserTeam> UserTeams { get; set; }
        public virtual ICollection<Team> LedTeams { get; set; }
        public virtual ICollection<Department> ManagedDepartments { get; set; }
        public virtual ICollection<Vocabulary> CreatedVocabularies { get; set; }
        public virtual ICollection<Vocabulary> UpdatedVocabularies { get; set; }
        public virtual ICollection<Vocabulary> LastModifiedVocabularies { get; set; }
        public virtual ICollection<UserPersonalVocabulary> PersonalVocabularies { get; set; }
        public virtual ICollection<UserKanjiProgress> KanjiProgresses { get; set; }
        public virtual ICollection<UserGrammarProgress> GrammarProgresses { get; set; }
        public virtual ICollection<UserTechnicalTerm> TechnicalTerms { get; set; }
        public virtual ICollection<LearningProgress> LearningProgresses { get; set; }
        public virtual ICollection<PersonalWordList> PersonalWordLists { get; set; }
        public virtual ICollection<TestResult> TestResults { get; set; }
        public virtual ICollection<CustomExam> CustomExams { get; set; }
        public virtual ICollection<UserExam> UserExams { get; set; }
        public virtual ICollection<UserLevel> UserLevels { get; set; }
        public virtual ICollection<UserPoint> UserPoints { get; set; }
        public virtual ICollection<UserBadge> UserBadges { get; set; }
        public virtual ICollection<UserChallenge> UserChallenges { get; set; }
        public virtual ICollection<UserDailyTask> UserDailyTasks { get; set; }
        public virtual ICollection<UserAchievement> UserAchievements { get; set; }
        public virtual ICollection<UserStreak> UserStreaks { get; set; }
        public virtual ICollection<LeaderboardEntry> LeaderboardEntries { get; set; }
        public virtual ICollection<UserEvent> UserEvents { get; set; }
        public virtual ICollection<UserGift> SentGifts { get; set; }
        public virtual ICollection<UserGift> ReceivedGifts { get; set; }
        public virtual ICollection<ActivityLog> ActivityLogs { get; set; }
        public virtual ICollection<SyncLog> SyncLogs { get; set; }
    }

    /// <summary>
    /// Represents a role in the system
    /// </summary>
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleID { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }

    /// <summary>
    /// Represents a user-role mapping
    /// </summary>
    public class UserRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserRoleID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int RoleID { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.Now;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("RoleID")]
        public virtual Role Role { get; set; }
    }

    /// <summary>
    /// Represents a permission in the system
    /// </summary>
    public class Permission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PermissionID { get; set; }

        [Required]
        [StringLength(100)]
        public string PermissionName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Module { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
        public virtual ICollection<UserPermission> UserPermissions { get; set; }
        public virtual ICollection<PermissionGroupMapping> PermissionGroupMappings { get; set; }
    }

    /// <summary>
    /// Represents a role-permission mapping
    /// </summary>
    public class RolePermission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RolePermissionID { get; set; }

        [Required]
        public int RoleID { get; set; }

        [Required]
        public int PermissionID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("RoleID")]
        public virtual Role Role { get; set; }

        [ForeignKey("PermissionID")]
        public virtual Permission Permission { get; set; }
    }

    /// <summary>
    /// Represents a group in the system
    /// </summary>
    public class Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupID { get; set; }

        [Required]
        [StringLength(100)]
        public string GroupName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<NotificationRecipient> NotificationRecipients { get; set; }
        public virtual ICollection<ScheduleItemParticipant> ScheduleItemParticipants { get; set; }
    }

    /// <summary>
    /// Represents a department in the system
    /// </summary>
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentID { get; set; }

        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; }

        [StringLength(20)]
        public string DepartmentCode { get; set; }

        public int? ParentDepartmentID { get; set; }

        public int? ManagerUserID { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("ParentDepartmentID")]
        public virtual Department ParentDepartment { get; set; }

        [ForeignKey("ManagerUserID")]
        public virtual User Manager { get; set; }

        public virtual ICollection<Department> ChildDepartments { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }

    /// <summary>
    /// Represents a team in the system
    /// </summary>
    public class Team
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamID { get; set; }

        [Required]
        [StringLength(100)]
        public string TeamName { get; set; }

        public int? DepartmentID { get; set; }

        public int? LeaderUserID { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("DepartmentID")]
        public virtual Department Department { get; set; }

        [ForeignKey("LeaderUserID")]
        public virtual User Leader { get; set; }

        public virtual ICollection<UserTeam> UserTeams { get; set; }
    }

    /// <summary>
    /// Represents a user-team mapping
    /// </summary>
    public class UserTeam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserTeamID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int TeamID { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string Role { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("TeamID")]
        public virtual Team Team { get; set; }
    }

    /// <summary>
    /// Represents a user-permission mapping
    /// </summary>
    public class UserPermission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserPermissionID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int PermissionID { get; set; }

        public int? GrantedByUserID { get; set; }

        public DateTime GrantedAt { get; set; } = DateTime.Now;

        public DateTime? ExpiresAt { get; set; }

        public bool IsActive { get; set; } = true;

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("PermissionID")]
        public virtual Permission Permission { get; set; }

        [ForeignKey("GrantedByUserID")]
        public virtual User GrantedByUser { get; set; }
    }

    /// <summary>
    /// Represents a permission group
    /// </summary>
    public class PermissionGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupID { get; set; }

        [Required]
        [StringLength(100)]
        public string GroupName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<PermissionGroupMapping> PermissionGroupMappings { get; set; }
    }

    /// <summary>
    /// Represents a permission group mapping
    /// </summary>
    public class PermissionGroupMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MappingID { get; set; }

        [Required]
        public int GroupID { get; set; }

        [Required]
        public int PermissionID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("GroupID")]
        public virtual PermissionGroup Group { get; set; }

        [ForeignKey("PermissionID")]
        public virtual Permission Permission { get; set; }
    }

    #endregion
}
