using LexiFlow.Models;
using LexiFlow.Models.Analytics;
using LexiFlow.Models.Core;
using LexiFlow.Models.Exam;
using LexiFlow.Models.Gamification;
using LexiFlow.Models.Learning.Grammar;
using LexiFlow.Models.Learning.Kanji;
using LexiFlow.Models.Learning.TechnicalTerms;
using LexiFlow.Models.Learning.Vocabulary;
using LexiFlow.Models.Media;
using LexiFlow.Models.Notification;
using LexiFlow.Models.Planning;
using LexiFlow.Models.Practice;
using LexiFlow.Models.Progress;
using LexiFlow.Models.Scheduling;
using LexiFlow.Models.Submission;
using LexiFlow.Models.Sync;
using LexiFlow.Models.System;
using LexiFlow.Models.User;
using LexiFlow.Models.User.UserRelations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace LexiFlow.Infrastructure.Data
{
    /// <summary>
    /// Main database context for LexiFlow application
    /// </summary>
    public class LexiFlowContext : DbContext
    {
        private readonly ILogger<LexiFlowContext> _logger;
        private int? _currentUserId;

        /// <summary>
        /// Initialize a new instance of the LexiFlowContext / Bối cảnh cơ sở dữ liệu chính cho ứng dụng LexiFlow
        /// </summary>
        public LexiFlowContext(DbContextOptions<LexiFlowContext> options, ILogger<LexiFlowContext> logger)
            : base(options)
        {
            _logger = logger;
        }

        /// <summary>
        /// Set the current user ID for auditing purposes
        /// </summary>
        /// <param name="userId">The ID of the current user</param>
        public void SetCurrentUserId(int userId)
        {
            _currentUserId = userId;
        }

        #region User Management DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<UserTeam> UserTeams { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<PermissionGroup> PermissionGroups { get; set; }
        public DbSet<PermissionGroupMapping> PermissionGroupMappings { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserLearningPreference> UserLearningPreferences { get; set; }
        public DbSet<UserNotificationSetting> UserNotificationSettings { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<GroupPermission> GroupPermissions { get; set; }
        #endregion

        #region Learning Content DbSets
        public DbSet<Category> Categories { get; set; }
        public DbSet<VocabularyGroup> VocabularyGroups { get; set; }
        public DbSet<Vocabulary> Vocabularies { get; set; }
        public DbSet<Definition> Definitions { get; set; }
        public DbSet<Example> Examples { get; set; }
        public DbSet<Translation> Translations { get; set; }

        public DbSet<Kanji> Kanjis { get; set; }
        public DbSet<KanjiVocabulary> KanjiVocabularies { get; set; }
        public DbSet<KanjiMeaning> KanjiMeanings { get; set; }
        public DbSet<KanjiExample> KanjiExamples { get; set; }
        public DbSet<KanjiExampleMeaning> KanjiExampleMeanings { get; set; }
        public DbSet<KanjiComponent> KanjiComponents { get; set; }
        public DbSet<KanjiComponentMapping> KanjiComponentMappings { get; set; }

        public DbSet<Grammar> Grammars { get; set; }
        public DbSet<GrammarDefinition> GrammarDefinitions { get; set; }
        public DbSet<GrammarExample> GrammarExamples { get; set; }
        public DbSet<GrammarTranslation> GrammarTranslations { get; set; }

        public DbSet<TechnicalTerm> TechnicalTerms { get; set; }
        public DbSet<TermExample> TermExamples { get; set; }
        public DbSet<TermTranslation> TermTranslations { get; set; }
        public DbSet<TermRelation> TermRelations { get; set; }

        public DbSet<UserTechnicalTerm> UserTechnicalTerms { get; set; }
        public DbSet<GroupVocabularyRelation> VocabularyRelations { get; set; }
        #endregion

        #region Media DbSets
        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<MediaCategory> MediaCategories { get; set; }
        public DbSet<MediaProcessingHistory> MediaProcessingHistories { get; set; }
        #endregion

        #region Progress Tracking DbSets
        public DbSet<LearningProgress> LearningProgresses { get; set; }
        public DbSet<LearningSession> LearningSessions { get; set; }
        public DbSet<LearningSessionDetail> SessionDetails { get; set; }
        public DbSet<UserKanjiProgress> UserKanjiProgresses { get; set; }
        public DbSet<UserGrammarProgress> UserGrammarProgresses { get; set; }
        public DbSet<GoalProgress> GoalProgresses { get; set; }
        public DbSet<PersonalWordList> PersonalWordLists { get; set; }
        public DbSet<PersonalWordListItem> PersonalWordListItems { get; set; }
        #endregion

        #region Planning DbSets
        public DbSet<StudyGoal> StudyGoals { get; set; }
        public DbSet<StudyPlan> StudyPlans { get; set; }
        public DbSet<StudySession> StudySessions { get; set; }
        public DbSet<StudyTask> StudyTasks { get; set; }
        public DbSet<StudyTopic> StudyTopics { get; set; }
        public DbSet<StudyPlanItem> StudyPlanItems { get; set; }
        public DbSet<StudyPlanProgress> StudyPlanProgresses { get; set; }
        public DbSet<TaskCompletion> TaskCompletions { get; set; }
        #endregion

        #region Practice and Exam DbSets
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<TestDetail> TestDetails { get; set; }
        public DbSet<JLPTExam> JLPTExams { get; set; }
        public DbSet<JLPTLevel> JLPTLevels { get; set; }
        public DbSet<JLPTSection> JLPTSections { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<UserExam> UserExams { get; set; } 
        public DbSet<UserPracticeSet> UserPracticeSets { get; set; }
        public DbSet<CustomExam> CustomExams { get; set; }
        public DbSet<CustomExamQuestion> CustomExamQuestions { get; set; }
        public DbSet<PracticeSet> PracticeSets { get; set; }
        public DbSet<PracticeSetItem> PracticeSetItems { get; set; }
        public DbSet<UserPracticeAnswer> UserPracticeAnswers { get; set; }
        #endregion

        #region Submission DbSets
        public DbSet<UserVocabularySubmission> UserVocabularySubmissions { get; set; }
        public DbSet<UserVocabularyDetail> UserVocabularyDetails { get; set; }
        public DbSet<SubmissionStatus> SubmissionStatuses { get; set; }
        public DbSet<StatusTransition> StatusTransitions { get; set; }
        public DbSet<ApprovalHistory> ApprovalHistories { get; set; }
        #endregion

        #region Analytics DbSets
        public DbSet<StudyReport> StudyReports { get; set; }
        public DbSet<StudyReportItem> StudyReportItems { get; set; }
        public DbSet<ReportType> ReportTypes { get; set; }
        public DbSet<ExamAnalytic> ExamAnalytics { get; set; } 
        public DbSet<PracticeAnalytic> PracticeAnalytics { get; set; } 
        public DbSet<StrengthWeakness> StrengthWeaknesses { get; set; }
        #endregion

        #region Gamification DbSets
        public DbSet<Level> Levels { get; set; }
        public DbSet<UserLevel> UserLevels { get; set; }
        public DbSet<PointType> PointTypes { get; set; }
        public DbSet<UserPoint> UserPoints { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<UserBadge> UserBadges { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<ChallengeRequirement> ChallengeRequirements { get; set; }
        public DbSet<UserChallenge> UserChallenges { get; set; }
        public DbSet<DailyTask> DailyTasks { get; set; }
        public DbSet<DailyTaskRequirement> DailyTaskRequirements { get; set; }
        public DbSet<UserDailyTask> UserDailyTasks { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<AchievementRequirement> AchievementRequirements { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
        public DbSet<Leaderboard> Leaderboards { get; set; }
        public DbSet<LeaderboardEntry> LeaderboardEntries { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        public DbSet<UserGift> UserGifts { get; set; }
        public DbSet<UserStreak> UserStreaks { get; set; }
        #endregion

        #region Synchronization DbSets
        public DbSet<SyncMetadata> SyncMetadata { get; set; }
        public DbSet<SyncConflict> SyncConflicts { get; set; }
        public DbSet<DeletedItem> DeletedItems { get; set; }
        #endregion

        #region Notification DbSets
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<NotificationPriority> NotificationPriorities { get; set; }
        public DbSet<NotificationStatus> NotificationStatuses { get; set; }
        public DbSet<NotificationRecipient> NotificationRecipients { get; set; }
        public DbSet<NotificationResponse> NotificationResponses { get; set; }
        #endregion

        #region Scheduling DbSets
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleItem> ScheduleItems { get; set; }
        public DbSet<ScheduleItemType> ScheduleItemTypes { get; set; }
        public DbSet<ScheduleRecurrence> ScheduleRecurrences { get; set; }
        public DbSet<ScheduleItemParticipant> ScheduleItemParticipants { get; set; }
        public DbSet<ScheduleReminder> ScheduleReminders { get; set; }
        #endregion

        #region System DbSets
        public DbSet<Setting> Settings { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<SyncLog> SyncLogs { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<PerformanceLog> PerformanceLogs { get; set; }
        #endregion

        /// <summary>
        /// Configure the model
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure core behavior
            ConfigureCoreEntities(modelBuilder);

            // Configure entity mappings
            ConfigureUserEntities(modelBuilder);
            ConfigureVocabularyEntities(modelBuilder);
            ConfigureKanjiEntities(modelBuilder);
            ConfigureGrammarEntities(modelBuilder);
            ConfigureTechnicalTermEntities(modelBuilder);
            ConfigureMediaEntities(modelBuilder);
            ConfigureProgressEntities(modelBuilder);
            ConfigurePracticeEntities(modelBuilder);
            ConfigureExamEntities(modelBuilder);
            ConfigurePlanningEntities(modelBuilder);
            ConfigureSchedulingEntities(modelBuilder);
            ConfigureNotificationEntities(modelBuilder);
            ConfigureSubmissionEntities(modelBuilder);
            ConfigureReportEntities(modelBuilder);
            ConfigureAnalyticsEntities(modelBuilder);
            ConfigureGamificationEntities(modelBuilder);
            ConfigureSyncEntities(modelBuilder);
            ConfigureSystemEntities(modelBuilder);
            ConfigureIndexes(modelBuilder);
            ConfigureQueryFilters(modelBuilder);
        }

        /// <summary>
        /// Configure core entities behavior
        /// </summary>
        private void ConfigureCoreEntities(ModelBuilder modelBuilder)
        {
            // Configure all entities that inherit from BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(BaseEntity).IsAssignableFrom(e.ClrType)))
            {
                // Configure RowVersion as concurrency token
                modelBuilder.Entity(entityType.ClrType)
                    .Property("RowVersion")
                    .IsRowVersion()
                    .IsConcurrencyToken();

                // Configure CreatedAt and UpdatedAt to use database default values if not set
                modelBuilder.Entity(entityType.ClrType)
                    .Property("CreatedAt")
                    .HasDefaultValueSql("GETUTCDATE()");

                modelBuilder.Entity(entityType.ClrType)
                    .Property("UpdatedAt")
                    .HasDefaultValueSql("GETUTCDATE()");
            }

            // Configure audit fields for AuditableEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(AuditableEntity).IsAssignableFrom(e.ClrType)))
            {
                // Configure CreatedBy and ModifiedBy relationships
                modelBuilder.Entity(entityType.ClrType)
                    .HasOne("CreatedByUser")
                    .WithMany()
                    .HasForeignKey("CreatedBy")
                    .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity(entityType.ClrType)
                    .HasOne("ModifiedByUser")
                    .WithMany()
                    .HasForeignKey("ModifiedBy")
                    .OnDelete(DeleteBehavior.Restrict);
            }

            // Configure soft delete fields for SoftDeletableEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(SoftDeletableEntity).IsAssignableFrom(e.ClrType)))
            {
                // Configure DeletedBy relationship
                modelBuilder.Entity(entityType.ClrType)
                    .HasOne("DeletedByUser")
                    .WithMany()
                    .HasForeignKey("DeletedBy")
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }

        /// <summary>
        /// Configure global query filters and indexes
        /// </summary>
        private void ConfigureQueryFilters(ModelBuilder modelBuilder)
        {
            // Apply global query filter for soft delete
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Check if entity implements ISoftDeletable
                if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, "IsDeleted");
                    var condition = Expression.Equal(property, Expression.Constant(false));
                    var lambda = Expression.Lambda(condition, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }

                // Check if entity implements IActivatable
                else if (typeof(IActivatable).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, "IsActive");
                    var condition = Expression.Equal(property, Expression.Constant(true));
                    var lambda = Expression.Lambda(condition, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }

        /// <summary>
        /// Configure system-related entities
        /// </summary>
        private void ConfigureSystemEntities(ModelBuilder modelBuilder)
        {
            // Setting configuration
            modelBuilder.Entity<Setting>(entity =>
            {
                entity.ToTable("Settings");
                entity.HasKey(s => s.SettingId);

                // Add indexes
                entity.HasIndex(s => s.SettingKey)
                    .IsUnique()
                    .HasDatabaseName("IX_Setting_Key");
                entity.HasIndex(s => s.Group)
                    .HasDatabaseName("IX_Setting_Group");
                entity.HasIndex(s => s.IsGlobal)
                    .HasDatabaseName("IX_Setting_IsGlobal");
                entity.HasIndex(s => s.DisplayOrder)
                    .HasDatabaseName("IX_Setting_DisplayOrder");
                entity.HasIndex(s => s.IsEditable)
                    .HasDatabaseName("IX_Setting_IsEditable");
                entity.HasIndex(s => s.IsVisible)
                    .HasDatabaseName("IX_Setting_IsVisible");
                entity.HasIndex(s => s.AccessRoles)
                    .HasDatabaseName("IX_Setting_AccessRoles");
                entity.HasIndex(s => s.DataType)
                    .HasDatabaseName("IX_Setting_DataType");
            });

            // ActivityLog configuration
            modelBuilder.Entity<ActivityLog>(entity =>
            {
                entity.ToTable("ActivityLogs");
                entity.HasKey(al => al.LogId);

                // Add indexes
                entity.HasIndex(al => new { al.UserId, al.Timestamp })
                    .HasDatabaseName("IX_ActivityLog_User_Time");
                entity.HasIndex(al => new { al.Module, al.Action })
                    .HasDatabaseName("IX_ActivityLog_Module_Action");
                entity.HasIndex(al => al.Timestamp)
                    .HasDatabaseName("IX_ActivityLog_Timestamp");
                entity.HasIndex(al => al.Severity)
                    .HasDatabaseName("IX_ActivityLog_Severity");
                entity.HasIndex(al => al.EntityType)
                    .HasDatabaseName("IX_ActivityLog_EntityType");
                entity.HasIndex(al => new { al.EntityType, al.EntityId })
                    .HasDatabaseName("IX_ActivityLog_Entity");
                entity.HasIndex(al => al.RequiresAttention)
                    .HasDatabaseName("IX_ActivityLog_RequiresAttention");
                entity.HasIndex(al => al.IsSystem)
                    .HasDatabaseName("IX_ActivityLog_IsSystem");

                // Relationships
                entity.HasOne(al => al.User)
                    .WithMany()
                    .HasForeignKey(al => al.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // SyncLog configuration
            modelBuilder.Entity<SyncLog>(entity =>
            {
                entity.ToTable("SyncLogs");
                entity.HasKey(sl => sl.SyncId);

                // Add indexes
                entity.HasIndex(sl => new { sl.UserId, sl.TableName, sl.LastSyncAt })
                    .HasDatabaseName("IX_SyncLog_User_Table_Time");
                entity.HasIndex(sl => sl.Status)
                    .HasDatabaseName("IX_SyncLog_Status");
                entity.HasIndex(sl => sl.SyncDirection)
                    .HasDatabaseName("IX_SyncLog_Direction");
                entity.HasIndex(sl => sl.DeviceId)
                    .HasDatabaseName("IX_SyncLog_DeviceId");
                entity.HasIndex(sl => sl.DeviceType)
                    .HasDatabaseName("IX_SyncLog_DeviceType");
                entity.HasIndex(sl => sl.ErrorCount)
                    .HasDatabaseName("IX_SyncLog_ErrorCount");
                entity.HasIndex(sl => sl.ConflictCount)
                    .HasDatabaseName("IX_SyncLog_ConflictCount");
                entity.HasIndex(sl => sl.AppVersion)
                    .HasDatabaseName("IX_SyncLog_AppVersion");

                // Relationships
                entity.HasOne(sl => sl.User)
                    .WithMany()
                    .HasForeignKey(sl => sl.UserId);
            });

            // ErrorLog configuration
            modelBuilder.Entity<ErrorLog>(entity =>
            {
                entity.ToTable("ErrorLogs");
                entity.HasKey(el => el.ErrorId);

                // Add indexes
                entity.HasIndex(el => el.Timestamp)
                    .HasDatabaseName("IX_ErrorLog_Time");
                entity.HasIndex(el => el.ErrorCode)
                    .HasDatabaseName("IX_ErrorLog_Code");
                entity.HasIndex(el => el.ErrorType)
                    .HasDatabaseName("IX_ErrorLog_Type");
                entity.HasIndex(el => el.ErrorSeverity)
                    .HasDatabaseName("IX_ErrorLog_Severity");
                entity.HasIndex(el => el.UserId)
                    .HasDatabaseName("IX_ErrorLog_User");
                entity.HasIndex(el => el.Source)
                    .HasDatabaseName("IX_ErrorLog_Source");
                entity.HasIndex(el => el.IsHandled)
                    .HasDatabaseName("IX_ErrorLog_IsHandled");
                entity.HasIndex(el => el.IsResolved)
                    .HasDatabaseName("IX_ErrorLog_IsResolved");
                entity.HasIndex(el => el.ResolvedAt)
                    .HasDatabaseName("IX_ErrorLog_ResolvedAt");
                entity.HasIndex(el => el.ResolvedBy)
                    .HasDatabaseName("IX_ErrorLog_ResolvedBy");

                // Relationships
                entity.HasOne(el => el.User)
                    .WithMany()
                    .HasForeignKey(el => el.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(el => el.ResolvedByUser)
                    .WithMany()
                    .HasForeignKey(el => el.ResolvedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // PerformanceLog configuration
            modelBuilder.Entity<PerformanceLog>(entity =>
            {
                entity.ToTable("PerformanceLogs");
                entity.HasKey(pl => pl.LogId);

                // Add indexes
                entity.HasIndex(pl => pl.Timestamp)
                    .HasDatabaseName("IX_PerformanceLog_Time");
                entity.HasIndex(pl => pl.Operation)
                    .HasDatabaseName("IX_PerformanceLog_Operation");
                entity.HasIndex(pl => pl.Category)
                    .HasDatabaseName("IX_PerformanceLog_Category");
                entity.HasIndex(pl => pl.ExecutionTimeMs)
                    .HasDatabaseName("IX_PerformanceLog_ExecutionTime");
                entity.HasIndex(pl => pl.UserId)
                    .HasDatabaseName("IX_PerformanceLog_User");
                entity.HasIndex(pl => pl.IsSlowOperation)
                    .HasDatabaseName("IX_PerformanceLog_IsSlowOperation");
                entity.HasIndex(pl => pl.CpuUsagePercent)
                    .HasDatabaseName("IX_PerformanceLog_CpuUsage");
                entity.HasIndex(pl => pl.DbTimeMs)
                    .HasDatabaseName("IX_PerformanceLog_DbTime");

                // Relationships
                entity.HasOne(pl => pl.User)
                    .WithMany()
                    .HasForeignKey(pl => pl.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        /// <summary>
        /// Configure indexing for optimized queries
        /// </summary>
        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            // User related indexes
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email);

            // Vocabulary related indexes
            modelBuilder.Entity<Vocabulary>().HasIndex(v => v.Term);
            modelBuilder.Entity<Vocabulary>().HasIndex(v => v.Level);
            modelBuilder.Entity<Vocabulary>().HasIndex(v => v.CategoryId);

            // Indexes for full-text search
            modelBuilder.Entity<Vocabulary>()
                .HasIndex(v => new { v.Term, v.Reading, v.Level })
                .HasDatabaseName("IX_Vocabulary_Search");

            // Media indexing
            modelBuilder.Entity<MediaFile>().HasIndex(m => m.MediaType);
            modelBuilder.Entity<MediaFile>().HasIndex(m => m.VocabularyId);

            // Learning progress indexes
            modelBuilder.Entity<LearningProgress>().HasIndex(lp => new { lp.UserId, lp.VocabularyId }).IsUnique();
            modelBuilder.Entity<LearningProgress>().HasIndex(lp => lp.NextReviewDate);

            // Composite indexes for common query patterns
            modelBuilder.Entity<LearningProgress>()
                .HasIndex(lp => new { lp.UserId, lp.NextReviewDate })
                .HasDatabaseName("IX_LearningProgress_UserReview");

            // Analytics indexes
            modelBuilder.Entity<ExamAnalytic>().HasIndex(ea => ea.UserExamId).IsUnique();
            modelBuilder.Entity<PracticeAnalytic>().HasIndex(pa => pa.UserPracticeId).IsUnique();
            modelBuilder.Entity<StrengthWeakness>()
                .HasIndex(sw => new { sw.UserId, sw.SkillType, sw.SpecificSkill })
                .IsUnique()
                .HasDatabaseName("IX_StrengthWeakness_UserSkill");
        }

        /// <summary>
        /// Configure user-related entities
        /// </summary>
        private void ConfigureUserEntities(ModelBuilder modelBuilder)
        {
            // User table configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.UserId);

                // Add indexes
                entity.HasIndex(u => u.Username).IsUnique().HasDatabaseName("IX_User_Username");
                entity.HasIndex(u => u.Email).HasDatabaseName("IX_User_Email");
                entity.HasIndex(u => u.IsActive).HasDatabaseName("IX_User_IsActive");
                entity.HasIndex(u => u.LastLoginAt).HasDatabaseName("IX_User_LastLogin");
                entity.HasIndex(u => u.RefreshTokenExpiryTime).HasDatabaseName("IX_User_RefreshTokenExpiry");

                // User relationships
                entity.HasOne(u => u.Profile)
                    .WithOne(p => p.User)
                    .HasForeignKey<UserProfile>(p => p.UserId);

                entity.HasOne(u => u.LearningPreference)
                    .WithOne(lp => lp.User)
                    .HasForeignKey<UserLearningPreference>(lp => lp.UserId);

                entity.HasOne(u => u.NotificationSetting)
                    .WithOne(ns => ns.User)
                    .HasForeignKey<UserNotificationSetting>(ns => ns.UserId);

                entity.HasMany(u => u.UserRoles)
                    .WithOne(ur => ur.User)
                    .HasForeignKey(ur => ur.UserId);

                entity.HasMany(u => u.MediaFiles)
                    .WithOne(mf => mf.User)
                    .HasForeignKey(mf => mf.UserId);

                entity.HasMany(u => u.LearningProgresses)
                    .WithOne(lp => lp.User)
                    .HasForeignKey(lp => lp.UserId);

                entity.HasMany(u => u.LearningSessions)
                    .WithOne(ls => ls.User)
                    .HasForeignKey(ls => ls.UserId);

                entity.HasMany(u => u.Badges)
                    .WithOne(ub => ub.User)
                    .HasForeignKey(ub => ub.UserId);
            });

            // UserProfile configuration
            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.ToTable("UserProfiles");
                entity.HasKey(up => up.UserId);

                // Add indexes
                entity.HasIndex(up => up.DepartmentId).HasDatabaseName("IX_UserProfile_Department");
                entity.HasIndex(up => up.AvatarMediaId).HasDatabaseName("IX_UserProfile_Avatar");
                entity.HasIndex(up => up.DisplayName).HasDatabaseName("IX_UserProfile_DisplayName");

                // Configure relationships
                entity.HasOne(up => up.Department)
                    .WithMany()
                    .HasForeignKey(up => up.DepartmentId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(up => up.AvatarMedia)
                    .WithMany()
                    .HasForeignKey(up => up.AvatarMediaId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // UserLearningPreference configuration
            modelBuilder.Entity<UserLearningPreference>(entity =>
            {
                entity.ToTable("UserLearningPreferences");
                entity.HasKey(ulp => ulp.PreferenceId);

                // Add indexes
                entity.HasIndex(ulp => ulp.UserId).IsUnique().HasDatabaseName("IX_UserLearningPreference_User");
                entity.HasIndex(ulp => ulp.LearningStyle).HasDatabaseName("IX_UserLearningPreference_Style");
                entity.HasIndex(ulp => ulp.StudyFocus).HasDatabaseName("IX_UserLearningPreference_Focus");
                entity.HasIndex(ulp => ulp.PreferredDifficulty).HasDatabaseName("IX_UserLearningPreference_Difficulty");
            });

            // UserNotificationSetting configuration
            modelBuilder.Entity<UserNotificationSetting>(entity =>
            {
                entity.ToTable("UserNotificationSettings");
                entity.HasKey(uns => uns.SettingId);

                // Add indexes
                entity.HasIndex(uns => uns.UserId).IsUnique().HasDatabaseName("IX_UserNotificationSetting_User");
                entity.HasIndex(uns => uns.EmailNotificationsEnabled).HasDatabaseName("IX_UserNotificationSetting_EmailEnabled");
                entity.HasIndex(uns => uns.PushNotificationsEnabled).HasDatabaseName("IX_UserNotificationSetting_PushEnabled");
                entity.HasIndex(uns => uns.QuietHoursEnabled).HasDatabaseName("IX_UserNotificationSetting_QuietHours");
            });

            // Role configuration
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(r => r.RoleId);

                // Add indexes
                entity.HasIndex(r => r.RoleName).IsUnique().HasDatabaseName("IX_Role_Name");
                entity.HasIndex(r => r.ParentRoleId).HasDatabaseName("IX_Role_Parent");
                entity.HasIndex(r => r.IsSystemRole).HasDatabaseName("IX_Role_IsSystem");
                entity.HasIndex(r => r.IsActive).HasDatabaseName("IX_Role_IsActive");
                entity.HasIndex(r => r.Priority).HasDatabaseName("IX_Role_Priority");

                // Role relationships
                entity.HasOne(r => r.ParentRole)
                    .WithMany(r => r.ChildRoles)
                    .HasForeignKey(r => r.ParentRoleId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(r => r.UserRoles)
                    .WithOne(ur => ur.Role)
                    .HasForeignKey(ur => ur.RoleId);

                entity.HasMany(r => r.RolePermissions)
                    .WithOne(rp => rp.Role)
                    .HasForeignKey(rp => rp.RoleId);

                entity.HasMany(r => r.ChildRoles)
                    .WithOne(r => r.ParentRole)
                    .HasForeignKey(r => r.ParentRoleId);
            });

            // Permission configuration
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("Permissions");
                entity.HasKey(p => p.PermissionId);

                // Add indexes
                entity.HasIndex(p => p.PermissionName).IsUnique().HasDatabaseName("IX_Permission_Name");
                entity.HasIndex(p => p.Module).HasDatabaseName("IX_Permission_Module");
                entity.HasIndex(p => p.Category).HasDatabaseName("IX_Permission_Category");
                entity.HasIndex(p => p.ResourceType).HasDatabaseName("IX_Permission_ResourceType");
                entity.HasIndex(p => p.Action).HasDatabaseName("IX_Permission_Action");
                entity.HasIndex(p => p.IsSystemPermission).HasDatabaseName("IX_Permission_IsSystem");

                // Permission relationships
                entity.HasMany(p => p.RolePermissions)
                    .WithOne(rp => rp.Permission)
                    .HasForeignKey(rp => rp.PermissionId);

                entity.HasMany(p => p.UserPermissions)
                    .WithOne(up => up.Permission)
                    .HasForeignKey(up => up.PermissionId);

                entity.HasMany(p => p.PermissionGroupMappings)
                    .WithOne(pgm => pgm.Permission)
                    .HasForeignKey(pgm => pgm.PermissionId);
            });

            // User-Role relationship
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRoles");
                entity.HasKey(ur => ur.UserRoleId);

                // Add indexes
                entity.HasIndex(ur => new { ur.UserId, ur.RoleId }).IsUnique().HasDatabaseName("IX_UserRole_UserRole");
                entity.HasIndex(ur => ur.ExpiresAt).HasDatabaseName("IX_UserRole_ExpiresAt");
                entity.HasIndex(ur => ur.AssignedBy).HasDatabaseName("IX_UserRole_AssignedBy");
                entity.HasIndex(ur => ur.IsActive).HasDatabaseName("IX_UserRole_IsActive");
                entity.HasIndex(ur => ur.Scope).HasDatabaseName("IX_UserRole_Scope");

                // Relationships
                entity.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId);

                entity.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId);

                entity.HasOne(ur => ur.AssignedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.AssignedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // RolePermission configuration
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("RolePermissions");
                entity.HasKey(rp => rp.RolePermissionId);

                // Add indexes
                entity.HasIndex(rp => new { rp.RoleId, rp.PermissionId }).IsUnique().HasDatabaseName("IX_RolePermission_RolePermission");
                entity.HasIndex(rp => rp.IsOverride).HasDatabaseName("IX_RolePermission_IsOverride");
                entity.HasIndex(rp => rp.GrantedBy).HasDatabaseName("IX_RolePermission_GrantedBy");
                entity.HasIndex(rp => rp.Scope).HasDatabaseName("IX_RolePermission_Scope");

                // Relationships
                entity.HasOne(rp => rp.Role)
                    .WithMany(r => r.RolePermissions)
                    .HasForeignKey(rp => rp.RoleId);

                entity.HasOne(rp => rp.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(rp => rp.PermissionId);

                entity.HasOne(rp => rp.GrantedByUser)
                    .WithMany()
                    .HasForeignKey(rp => rp.GrantedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // UserPermission configuration
            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.ToTable("UserPermissions");
                entity.HasKey(up => up.UserPermissionId);

                // Add indexes
                entity.HasIndex(up => new { up.UserId, up.PermissionId }).IsUnique().HasDatabaseName("IX_UserPermission_UserPermission");
                entity.HasIndex(up => up.GrantedByUserId).HasDatabaseName("IX_UserPermission_GrantedBy");
                entity.HasIndex(up => up.ExpiresAt).HasDatabaseName("IX_UserPermission_ExpiresAt");
                entity.HasIndex(up => up.Scope).HasDatabaseName("IX_UserPermission_Scope");
                entity.HasIndex(up => up.IsOverride).HasDatabaseName("IX_UserPermission_IsOverride");
                entity.HasIndex(up => up.IsActive).HasDatabaseName("IX_UserPermission_IsActive");

                // Relationships
                entity.HasOne(up => up.User)
                    .WithMany()
                    .HasForeignKey(up => up.UserId);

                entity.HasOne(up => up.Permission)
                    .WithMany(p => p.UserPermissions)
                    .HasForeignKey(up => up.PermissionId);

                entity.HasOne(up => up.GrantedByUser)
                    .WithMany()
                    .HasForeignKey(up => up.GrantedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Department configuration
            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Departments");
                entity.HasKey(d => d.DepartmentId);

                // Add indexes
                entity.HasIndex(d => d.DepartmentName).HasDatabaseName("IX_Department_Name");
                entity.HasIndex(d => d.DepartmentCode).IsUnique().HasDatabaseName("IX_Department_Code");
                entity.HasIndex(d => d.ParentDepartmentId).HasDatabaseName("IX_Department_Parent");
                entity.HasIndex(d => d.ManagerUserId).HasDatabaseName("IX_Department_Manager");
                entity.HasIndex(d => d.DisplayOrder).HasDatabaseName("IX_Department_DisplayOrder");
                entity.HasIndex(d => d.IsActive).HasDatabaseName("IX_Department_IsActive");
                entity.HasIndex(d => d.CostCenter).HasDatabaseName("IX_Department_CostCenter");

                // Relationships
                entity.HasOne(d => d.ParentDepartment)
                    .WithMany(d => d.ChildDepartments)
                    .HasForeignKey(d => d.ParentDepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Manager)
                    .WithMany()
                    .HasForeignKey(d => d.ManagerUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(d => d.ChildDepartments)
                    .WithOne(d => d.ParentDepartment)
                    .HasForeignKey(d => d.ParentDepartmentId);

                entity.HasMany(d => d.Users)
                    .WithOne(up => up.Department)
                    .HasForeignKey(up => up.DepartmentId);

                entity.HasMany(d => d.Teams)
                    .WithOne(t => t.Department)
                    .HasForeignKey(t => t.DepartmentId);
            });

            // Team configuration
            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("Teams");
                entity.HasKey(t => t.TeamId);

                // Add indexes
                entity.HasIndex(t => t.TeamName).HasDatabaseName("IX_Team_Name");
                entity.HasIndex(t => t.DepartmentId).HasDatabaseName("IX_Team_Department");
                entity.HasIndex(t => t.LeaderUserId).HasDatabaseName("IX_Team_Leader");
                entity.HasIndex(t => t.TeamType).HasDatabaseName("IX_Team_Type");
                entity.HasIndex(t => t.FormationDate).HasDatabaseName("IX_Team_FormationDate");
                entity.HasIndex(t => t.DisbandDate).HasDatabaseName("IX_Team_DisbandDate");
                entity.HasIndex(t => t.IsActive).HasDatabaseName("IX_Team_IsActive");
                entity.HasIndex(t => t.Status).HasDatabaseName("IX_Team_Status");

                // Relationships
                entity.HasOne(t => t.Department)
                    .WithMany(d => d.Teams)
                    .HasForeignKey(t => t.DepartmentId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(t => t.Leader)
                    .WithMany()
                    .HasForeignKey(t => t.LeaderUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(t => t.UserTeams)
                    .WithOne(ut => ut.Team)
                    .HasForeignKey(ut => ut.TeamId);
            });

            // UserTeam configuration
            modelBuilder.Entity<UserTeam>(entity =>
            {
                entity.ToTable("UserTeams");
                entity.HasKey(ut => ut.UserTeamId);

                // Add indexes
                entity.HasIndex(ut => new { ut.UserId, ut.TeamId }).IsUnique().HasDatabaseName("IX_UserTeam_UserTeam");
                entity.HasIndex(ut => ut.Role).HasDatabaseName("IX_UserTeam_Role");
                entity.HasIndex(ut => ut.ExpiresAt).HasDatabaseName("IX_UserTeam_ExpiresAt");
                entity.HasIndex(ut => ut.AddedBy).HasDatabaseName("IX_UserTeam_AddedBy");
                entity.HasIndex(ut => ut.TimeCommitment).HasDatabaseName("IX_UserTeam_TimeCommitment");
                entity.HasIndex(ut => ut.AllocationPercentage).HasDatabaseName("IX_UserTeam_Allocation");
                entity.HasIndex(ut => ut.IsActive).HasDatabaseName("IX_UserTeam_IsActive");

                // Relationships
                entity.HasOne(ut => ut.User)
                    .WithMany()
                    .HasForeignKey(ut => ut.UserId);

                entity.HasOne(ut => ut.Team)
                    .WithMany(t => t.UserTeams)
                    .HasForeignKey(ut => ut.TeamId);

                entity.HasOne(ut => ut.AddedByUser)
                    .WithMany()
                    .HasForeignKey(ut => ut.AddedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Group configuration
            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Groups");
                entity.HasKey(g => g.GroupId);

                // Add indexes
                entity.HasIndex(g => g.GroupName).IsUnique().HasDatabaseName("IX_Group_Name");
                entity.HasIndex(g => g.CreatedByUserId).HasDatabaseName("IX_Group_CreatedBy");
                entity.HasIndex(g => g.GroupType).HasDatabaseName("IX_Group_Type");
                entity.HasIndex(g => g.DisplayOrder).HasDatabaseName("IX_Group_DisplayOrder");
                entity.HasIndex(g => g.JoinPolicy).HasDatabaseName("IX_Group_JoinPolicy");
                entity.HasIndex(g => g.ParentGroupId).HasDatabaseName("IX_Group_Parent");
                entity.HasIndex(g => g.IsSystemGroup).HasDatabaseName("IX_Group_IsSystem");
                entity.HasIndex(g => g.ExpiryDate).HasDatabaseName("IX_Group_ExpiryDate");
                entity.HasIndex(g => g.IsActive).HasDatabaseName("IX_Group_IsActive");
                entity.HasIndex(g => g.MemberCount).HasDatabaseName("IX_Group_MemberCount");

                // Relationships
                entity.HasOne(g => g.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(g => g.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(g => g.ParentGroup)
                    .WithMany(g => g.ChildGroups)
                    .HasForeignKey(g => g.ParentGroupId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(g => g.ChildGroups)
                    .WithOne(g => g.ParentGroup)
                    .HasForeignKey(g => g.ParentGroupId);

                entity.HasMany(g => g.UserGroups)
                    .WithOne(ug => ug.Group)
                    .HasForeignKey(ug => ug.GroupId);

                entity.HasMany(g => g.GroupPermissions)
                    .WithOne(gp => gp.Group)
                    .HasForeignKey(gp => gp.GroupId);

                entity.HasMany(g => g.PermissionGroupMappings)
                    .WithOne()
                    .HasForeignKey(pgm => pgm.PermissionGroupId);
            });

            // UserGroup configuration
            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.ToTable("UserGroups");
                entity.HasKey(ug => ug.UserGroupId);

                // Add indexes
                entity.HasIndex(ug => new { ug.UserId, ug.GroupId }).IsUnique().HasDatabaseName("IX_UserGroup_User_Group");
                entity.HasIndex(ug => ug.MemberRole).HasDatabaseName("IX_UserGroup_MemberRole");
                entity.HasIndex(ug => ug.IsAdmin).HasDatabaseName("IX_UserGroup_IsAdmin");
                entity.HasIndex(ug => ug.Status).HasDatabaseName("IX_UserGroup_Status");
                entity.HasIndex(ug => ug.ExpiryDate).HasDatabaseName("IX_UserGroup_ExpiryDate");
                entity.HasIndex(ug => ug.InvitedBy).HasDatabaseName("IX_UserGroup_InvitedBy");
                entity.HasIndex(ug => ug.LastActivity).HasDatabaseName("IX_UserGroup_LastActivity");
                entity.HasIndex(ug => ug.ContributionPoints).HasDatabaseName("IX_UserGroup_ContributionPoints");

                // Relationships
                entity.HasOne(ug => ug.User)
                    .WithMany()
                    .HasForeignKey(ug => ug.UserId);

                entity.HasOne(ug => ug.Group)
                    .WithMany(g => g.UserGroups)
                    .HasForeignKey(ug => ug.GroupId);

                entity.HasOne(ug => ug.InvitedByUser)
                    .WithMany()
                    .HasForeignKey(ug => ug.InvitedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // GroupPermission configuration
            modelBuilder.Entity<GroupPermission>(entity =>
            {
                entity.ToTable("GroupPermissions");
                entity.HasKey(gp => gp.GroupPermissionId);

                // Add indexes
                entity.HasIndex(gp => new { gp.GroupId, gp.PermissionId }).IsUnique().HasDatabaseName("IX_GroupPermission_Group_Permission");
                entity.HasIndex(gp => gp.GrantedByUserId).HasDatabaseName("IX_GroupPermission_GrantedBy");
                entity.HasIndex(gp => gp.GrantedAt).HasDatabaseName("IX_GroupPermission_GrantedAt");
                entity.HasIndex(gp => gp.ExpiresAt).HasDatabaseName("IX_GroupPermission_ExpiresAt");
                entity.HasIndex(gp => gp.Scope).HasDatabaseName("IX_GroupPermission_Scope");
                entity.HasIndex(gp => gp.IsActive).HasDatabaseName("IX_GroupPermission_IsActive");

                // Relationships
                entity.HasOne(gp => gp.Group)
                    .WithMany(g => g.GroupPermissions)
                    .HasForeignKey(gp => gp.GroupId);

                entity.HasOne(gp => gp.Permission)
                    .WithMany()
                    .HasForeignKey(gp => gp.PermissionId);

                entity.HasOne(gp => gp.GrantedByUser)
                    .WithMany()
                    .HasForeignKey(gp => gp.GrantedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // PermissionGroup configuration
            modelBuilder.Entity<PermissionGroup>(entity =>
            {
                entity.ToTable("PermissionGroups");
                entity.HasKey(pg => pg.PermissionGroupId);

                // Add indexes
                entity.HasIndex(pg => pg.GroupName).IsUnique().HasDatabaseName("IX_PermissionGroup_Name");
                entity.HasIndex(pg => pg.ModuleArea).HasDatabaseName("IX_PermissionGroup_ModuleArea");
                entity.HasIndex(pg => pg.DisplayOrder).HasDatabaseName("IX_PermissionGroup_DisplayOrder");
                entity.HasIndex(pg => pg.ParentGroupId).HasDatabaseName("IX_PermissionGroup_Parent");
                entity.HasIndex(pg => pg.IsSystemGroup).HasDatabaseName("IX_PermissionGroup_IsSystem");
                entity.HasIndex(pg => pg.IsActive).HasDatabaseName("IX_PermissionGroup_IsActive");

                // Relationships
                entity.HasOne(pg => pg.ParentGroup)
                    .WithMany(pg => pg.ChildGroups)
                    .HasForeignKey(pg => pg.ParentGroupId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(pg => pg.ChildGroups)
                    .WithOne(pg => pg.ParentGroup)
                    .HasForeignKey(pg => pg.ParentGroupId);

                entity.HasMany(pg => pg.PermissionMappings)
                    .WithOne(pgm => pgm.PermissionGroup)
                    .HasForeignKey(pgm => pgm.PermissionGroupId);
            });

            // PermissionGroupMapping configuration
            modelBuilder.Entity<PermissionGroupMapping>(entity =>
            {
                entity.ToTable("PermissionGroupMappings");
                entity.HasKey(pgm => pgm.MappingId);

                // Add indexes
                entity.HasIndex(pgm => new { pgm.PermissionGroupId, pgm.PermissionId }).IsUnique().HasDatabaseName("IX_PermissionGroupMapping_Group_Permission");
                entity.HasIndex(pgm => pgm.DisplayOrder).HasDatabaseName("IX_PermissionGroupMapping_DisplayOrder");
                entity.HasIndex(pgm => pgm.SubCategory).HasDatabaseName("IX_PermissionGroupMapping_SubCategory");
                entity.HasIndex(pgm => pgm.IsRequired).HasDatabaseName("IX_PermissionGroupMapping_IsRequired");
                entity.HasIndex(pgm => pgm.IsDefault).HasDatabaseName("IX_PermissionGroupMapping_IsDefault");
                entity.HasIndex(pgm => pgm.IsSystem).HasDatabaseName("IX_PermissionGroupMapping_IsSystem");
                entity.HasIndex(pgm => pgm.CreatedByUserId).HasDatabaseName("IX_PermissionGroupMapping_CreatedBy");
                entity.HasIndex(pgm => pgm.DependsOnPermissionId).HasDatabaseName("IX_PermissionGroupMapping_DependsOn");
                entity.HasIndex(pgm => pgm.IsActive).HasDatabaseName("IX_PermissionGroupMapping_IsActive");

                // Relationships
                entity.HasOne(pgm => pgm.PermissionGroup)
                    .WithMany(pg => pg.PermissionMappings)
                    .HasForeignKey(pgm => pgm.PermissionGroupId);

                entity.HasOne(pgm => pgm.Permission)
                    .WithMany(p => p.PermissionGroupMappings)
                    .HasForeignKey(pgm => pgm.PermissionId);

                entity.HasOne(pgm => pgm.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(pgm => pgm.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(pgm => pgm.DependsOnPermission)
                    .WithMany()
                    .HasForeignKey(pgm => pgm.DependsOnPermissionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // UserTechnicalTerm configuration
            modelBuilder.Entity<UserTechnicalTerm>(entity =>
            {
                entity.ToTable("UserTechnicalTerms");
                entity.HasKey(utt => utt.UserTermId);

                // Add indexes
                entity.HasIndex(utt => new { utt.UserId, utt.TermId }).IsUnique().HasDatabaseName("IX_UserTechnicalTerm_User_Term");
                entity.HasIndex(utt => utt.ProficiencyLevel).HasDatabaseName("IX_UserTechnicalTerm_Proficiency");
                entity.HasIndex(utt => utt.LastPracticed).HasDatabaseName("IX_UserTechnicalTerm_LastPracticed");
                entity.HasIndex(utt => utt.NextReviewDate).HasDatabaseName("IX_UserTechnicalTerm_NextReview");
                entity.HasIndex(utt => utt.MemoryStrength).HasDatabaseName("IX_UserTechnicalTerm_MemoryStrength");
                entity.HasIndex(utt => utt.Priority).HasDatabaseName("IX_UserTechnicalTerm_Priority");
                entity.HasIndex(utt => utt.IsBookmarked).HasDatabaseName("IX_UserTechnicalTerm_IsBookmarked");

                // Relationships
                entity.HasOne(utt => utt.User)
                    .WithMany()
                    .HasForeignKey(utt => utt.UserId);

                entity.HasOne(utt => utt.Term)
                    .WithMany(tt => tt.UserTechnicalTerms)
                    .HasForeignKey(utt => utt.TermId);
            });
        }

        /// <summary>
        /// Configure vocabulary-related entities
        /// </summary>
        private void ConfigureVocabularyEntities(ModelBuilder modelBuilder)
        {
            // Vocabulary configuration
            modelBuilder.Entity<LexiFlow.Models.Learning.Vocabulary.Vocabulary>(entity =>
            {
                entity.ToTable("Vocabulary");
                entity.HasKey(v => v.Id);

                // Add indexes for optimized queries
                entity.HasIndex(v => v.Term).HasDatabaseName("IX_Vocabulary_Term");
                entity.HasIndex(v => v.LanguageCode).HasDatabaseName("IX_Vocabulary_LanguageCode");
                entity.HasIndex(v => v.Level).HasDatabaseName("IX_Vocabulary_Level");
                entity.HasIndex(v => v.IsCommon).HasDatabaseName("IX_Vocabulary_IsCommon");

                // Composite index for full-text search
                entity.HasIndex(v => new { v.Term, v.Reading, v.SearchVector })
                    .HasDatabaseName("IX_Vocabulary_Search");

                // Configure soft delete behavior
                entity.HasQueryFilter(v => !v.IsDeleted);

                // Vocabulary relationships
                entity.HasOne(v => v.Category)
                    .WithMany(c => c.Vocabularies)
                    .HasForeignKey(v => v.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(v => v.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(v => v.DeletedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(v => v.Definitions)
                    .WithOne(d => d.Vocabulary)
                    .HasForeignKey(d => d.VocabularyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(v => v.Examples)
                    .WithOne(e => e.Vocabulary)
                    .HasForeignKey(e => e.VocabularyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(v => v.Translations)
                    .WithOne(t => t.Vocabulary)
                    .HasForeignKey(t => t.VocabularyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(v => v.MediaFiles)
                    .WithOne(m => m.Vocabulary)
                    .HasForeignKey(m => m.VocabularyId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(v => v.LearningProgresses)
                    .WithOne()
                    .HasForeignKey(lp => lp.VocabularyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(v => v.KanjiVocabularies)
                    .WithOne(kv => kv.Vocabulary)
                    .HasForeignKey(kv => kv.VocabularyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Definition configuration
            modelBuilder.Entity<Definition>(entity =>
            {
                entity.ToTable("Definitions");
                entity.HasKey(d => d.Id);

                // Add index for foreign key and part of speech
                entity.HasIndex(d => d.VocabularyId).HasDatabaseName("IX_Definition_VocabularyId");
                entity.HasIndex(d => d.PartOfSpeech).HasDatabaseName("IX_Definition_PartOfSpeech");
                entity.HasIndex(d => d.LanguageCode).HasDatabaseName("IX_Definition_LanguageCode");

                // Definition relationships
                entity.HasOne(d => d.Vocabulary)
                    .WithMany(v => v.Definitions)
                    .HasForeignKey(d => d.VocabularyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Example configuration
            modelBuilder.Entity<Example>(entity =>
            {
                entity.ToTable("Examples");
                entity.HasKey(e => e.Id);

                // Add indexes for foreign key and difficulty level
                entity.HasIndex(e => e.VocabularyId).HasDatabaseName("IX_Example_VocabularyId");
                entity.HasIndex(e => e.DifficultyLevel).HasDatabaseName("IX_Example_DifficultyLevel");
                entity.HasIndex(e => e.LanguageCode).HasDatabaseName("IX_Example_LanguageCode");
                entity.HasIndex(e => e.IsVerified).HasDatabaseName("IX_Example_IsVerified");

                // Example relationships
                entity.HasOne(e => e.Vocabulary)
                    .WithMany(v => v.Examples)
                    .HasForeignKey(e => e.VocabularyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.MediaFiles)
                    .WithOne(m => m.Example)
                    .HasForeignKey(m => m.ExampleId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Translation configuration
            modelBuilder.Entity<Translation>(entity =>
            {
                entity.ToTable("Translations");
                entity.HasKey(t => t.Id);

                // Add indexes for foreign key and language code
                entity.HasIndex(t => t.VocabularyId).HasDatabaseName("IX_Translation_VocabularyId");
                entity.HasIndex(t => t.LanguageCode).HasDatabaseName("IX_Translation_LanguageCode");
                entity.HasIndex(t => t.IsMachineTranslated).HasDatabaseName("IX_Translation_IsMachineTranslated");

                // Translation relationships
                entity.HasOne(t => t.Vocabulary)
                    .WithMany(v => v.Translations)
                    .HasForeignKey(t => t.VocabularyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(t => t.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(t => t.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Category configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(c => c.CategoryId);

                // Add indexes for name and level
                entity.HasIndex(c => c.CategoryName).HasDatabaseName("IX_Category_CategoryName");
                entity.HasIndex(c => c.Level).HasDatabaseName("IX_Category_Level");
                entity.HasIndex(c => c.IsActive).HasDatabaseName("IX_Category_IsActive");
                entity.HasIndex(c => c.CategoryType).HasDatabaseName("IX_Category_CategoryType");

                // Apply global query filter for active categories
                entity.HasQueryFilter(c => c.IsActive);

                // Category relationships
                entity.HasOne(c => c.ParentCategory)
                    .WithMany(c => c.ChildCategories)
                    .HasForeignKey(c => c.ParentCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(c => c.ChildCategories)
                    .WithOne(c => c.ParentCategory)
                    .HasForeignKey(c => c.ParentCategoryId);

                entity.HasMany(c => c.Vocabularies)
                    .WithOne(v => v.Category)
                    .HasForeignKey(v => v.CategoryId);

                entity.HasMany(c => c.VocabularyGroups)
                    .WithOne(vg => vg.Category)
                    .HasForeignKey(vg => vg.CategoryId);
            });

            // VocabularyGroup configuration
            modelBuilder.Entity<VocabularyGroup>(entity =>
            {
                entity.ToTable("VocabularyGroups");
                entity.HasKey(vg => vg.GroupId);

                // Add indexes for name and category
                entity.HasIndex(vg => vg.GroupName).HasDatabaseName("IX_VocabularyGroup_GroupName");
                entity.HasIndex(vg => vg.CategoryId).HasDatabaseName("IX_VocabularyGroup_CategoryId");
                entity.HasIndex(vg => vg.IsActive).HasDatabaseName("IX_VocabularyGroup_IsActive");
                entity.HasIndex(vg => vg.GroupType).HasDatabaseName("IX_VocabularyGroup_GroupType");
                entity.HasIndex(vg => vg.IsPublic).HasDatabaseName("IX_VocabularyGroup_IsPublic");

                // Apply global query filter for active groups
                entity.HasQueryFilter(vg => vg.IsActive);

                // VocabularyGroup relationships
                entity.HasOne(vg => vg.Category)
                    .WithMany(c => c.VocabularyGroups)
                    .HasForeignKey(vg => vg.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(vg => vg.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(vg => vg.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(vg => vg.VocabularyRelations)
                    .WithOne(gvr => gvr.Group)
                    .HasForeignKey(gvr => gvr.GroupId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(vg => vg.LearningSessions)
                    .WithOne()
                    .HasForeignKey(ls => ls.GroupId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // GroupVocabularyRelation configuration
            modelBuilder.Entity<GroupVocabularyRelation>(entity =>
            {
                entity.ToTable("GroupVocabularyRelations");
                entity.HasKey(gvr => gvr.RelationId);

                // Add indexes
                entity.HasIndex(gvr => gvr.GroupId).HasDatabaseName("IX_GroupVocabularyRelation_GroupId");
                entity.HasIndex(gvr => gvr.VocabularyId).HasDatabaseName("IX_GroupVocabularyRelation_VocabularyId");
                entity.HasIndex(gvr => gvr.RelationType).HasDatabaseName("IX_GroupVocabularyRelation_RelationType");
                entity.HasIndex(gvr => gvr.Importance).HasDatabaseName("IX_GroupVocabularyRelation_Importance");

                // Unique constraint for group and vocabulary
                entity.HasIndex(gvr => new { gvr.GroupId, gvr.VocabularyId })
                    .IsUnique()
                    .HasDatabaseName("IX_GroupVocabularyRelation_Group_Vocabulary");

                // GroupVocabularyRelation relationships
                entity.HasOne(gvr => gvr.Group)
                    .WithMany(vg => vg.VocabularyRelations)
                    .HasForeignKey(gvr => gvr.GroupId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(gvr => gvr.Vocabulary)
                    .WithMany()
                    .HasForeignKey(gvr => gvr.VocabularyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        /// <summary>
        /// Configure kanji-related entities
        /// </summary>
        private void ConfigureKanjiEntities(ModelBuilder modelBuilder)
        {
            // Kanji configuration
            modelBuilder.Entity<Kanji>(entity =>
            {
                entity.ToTable("Kanji");
                entity.HasKey(k => k.KanjiId);

                // Add indexes
                entity.HasIndex(k => k.Character)
                    .IsUnique()
                    .HasDatabaseName("IX_Kanji_Character");

                entity.HasIndex(k => k.JLPTLevel)
                    .HasDatabaseName("IX_Kanji_JLPT");

                // Configure relationships
                entity.HasOne(k => k.Category)
                    .WithMany()
                    .HasForeignKey(k => k.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(k => k.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(k => k.DeletedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                // Configure collections
                entity.HasMany(k => k.Meanings)
                    .WithOne(m => m.Kanji)
                    .HasForeignKey(m => m.KanjiId);

                entity.HasMany(k => k.Examples)
                    .WithOne(e => e.Kanji)
                    .HasForeignKey(e => e.KanjiId);

                entity.HasMany(k => k.ComponentMappings)
                    .WithOne(cm => cm.Kanji)
                    .HasForeignKey(cm => cm.KanjiId);

                entity.HasMany(k => k.KanjiVocabularies)
                    .WithOne(kv => kv.Kanji)
                    .HasForeignKey(kv => kv.KanjiId);

                entity.HasMany(k => k.MediaFiles)
                    .WithOne(m => m.Kanji)
                    .HasForeignKey(m => m.KanjiId);

                entity.HasMany(k => k.UserProgresses)
                    .WithOne(up => up.Kanji)
                    .HasForeignKey(up => up.KanjiId);
            });

            // KanjiComponent configuration
            modelBuilder.Entity<KanjiComponent>(entity =>
            {
                entity.ToTable("KanjiComponents");
                entity.HasKey(kc => kc.ComponentId);

                // Add unique index for Character
                entity.HasIndex(kc => kc.Character)
                    .IsUnique()
                    .HasDatabaseName("IX_KanjiComponent_Character");

                // Relationships
                entity.HasOne(kc => kc.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(kc => kc.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(kc => kc.ComponentMappings)
                    .WithOne(cm => cm.Component)
                    .HasForeignKey(cm => cm.ComponentId);
            });

            // KanjiComponentMapping configuration
            modelBuilder.Entity<KanjiComponentMapping>(entity =>
            {
                entity.ToTable("KanjiComponentMappings");
                entity.HasKey(kcm => kcm.MappingId);

                // Add unique index for KanjiId and ComponentId
                entity.HasIndex(kcm => new { kcm.KanjiId, kcm.ComponentId })
                    .IsUnique()
                    .HasDatabaseName("IX_KanjiComponentMapping_Kanji_Component");

                // Relationships
                entity.HasOne(kcm => kcm.Kanji)
                    .WithMany(k => k.ComponentMappings)
                    .HasForeignKey(kcm => kcm.KanjiId);

                entity.HasOne(kcm => kcm.Component)
                    .WithMany(kc => kc.ComponentMappings)
                    .HasForeignKey(kcm => kcm.ComponentId);
            });

            // KanjiMeaning configuration
            modelBuilder.Entity<KanjiMeaning>(entity =>
            {
                entity.ToTable("KanjiMeanings");
                entity.HasKey(km => km.MeaningId);

                // Add index for KanjiId and Language
                entity.HasIndex(km => new { km.KanjiId, km.Language })
                    .HasDatabaseName("IX_KanjiMeaning_Kanji_Lang");

                // Relationships
                entity.HasOne(km => km.Kanji)
                    .WithMany(k => k.Meanings)
                    .HasForeignKey(km => km.KanjiId);

                entity.HasOne(km => km.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(km => km.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(km => km.UpdatedByUser)
                    .WithMany()
                    .HasForeignKey(km => km.UpdatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // KanjiExample configuration
            modelBuilder.Entity<KanjiExample>(entity =>
            {
                entity.ToTable("KanjiExamples");
                entity.HasKey(ke => ke.KanjiExampleId);

                // Add index for KanjiId
                entity.HasIndex(ke => ke.KanjiId)
                    .HasDatabaseName("IX_KanjiExample_Kanji");

                // Relationships
                entity.HasOne(ke => ke.Kanji)
                    .WithMany(k => k.Examples)
                    .HasForeignKey(ke => ke.KanjiId);

                entity.HasOne(ke => ke.VerifiedByUser)
                    .WithMany()
                    .HasForeignKey(ke => ke.VerifiedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(ke => ke.Meanings)
                    .WithOne(kem => kem.Example)
                    .HasForeignKey(kem => kem.ExampleId);

                entity.HasMany(ke => ke.MediaFiles)
                    .WithOne(m => m.KanjiExample)
                    .HasForeignKey(m => m.KanjiExampleId);
            });

            // KanjiExampleMeaning configuration
            modelBuilder.Entity<KanjiExampleMeaning>(entity =>
            {
                entity.ToTable("KanjiExampleMeanings");
                entity.HasKey(kem => kem.MeaningId);

                // Add unique index for ExampleId and Language
                entity.HasIndex(kem => new { kem.ExampleId, kem.Language })
                    .IsUnique()
                    .HasDatabaseName("IX_KanjiExampleMeaning_Example_Lang");

                // Relationships
                entity.HasOne(kem => kem.Example)
                    .WithMany(ke => ke.Meanings)
                    .HasForeignKey(kem => kem.ExampleId);

                entity.HasOne(kem => kem.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(kem => kem.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // KanjiVocabulary configuration
            modelBuilder.Entity<KanjiVocabulary>(entity =>
            {
                entity.ToTable("KanjiVocabularies");
                entity.HasKey(kv => kv.KanjiVocabularyId);

                // Add unique index for KanjiId and VocabularyId
                entity.HasIndex(kv => new { kv.KanjiId, kv.VocabularyId })
                    .IsUnique()
                    .HasDatabaseName("IX_KanjiVocabulary_Kanji_Vocab");

                // Relationships
                entity.HasOne(kv => kv.Kanji)
                    .WithMany(k => k.KanjiVocabularies)
                    .HasForeignKey(kv => kv.KanjiId);

                entity.HasOne(kv => kv.Vocabulary)
                    .WithMany(v => v.KanjiVocabularies)
                    .HasForeignKey(kv => kv.VocabularyId);
            });
        }

        /// <summary>
        /// Configure grammar-related entities
        /// </summary>
        private void ConfigureGrammarEntities(ModelBuilder modelBuilder)
        {
            // Grammar configuration
            modelBuilder.Entity<Grammar>(entity =>
            {
                entity.ToTable("Grammar");
                entity.HasKey(g => g.Id);

                // Add indexes 
                entity.HasIndex(g => g.Pattern)
                    .IsUnique()
                    .HasDatabaseName("IX_Grammar_Pattern");

                entity.HasIndex(g => g.Level)
                    .HasDatabaseName("IX_Grammar_Level");

                // Configure relationships
                entity.HasOne(g => g.Category)
                    .WithMany()
                    .HasForeignKey(g => g.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(g => g.ParentGrammar)
                    .WithMany(g => g.ChildGrammars)
                    .HasForeignKey(g => g.ParentGrammarId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(g => g.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(g => g.DeletedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                // Configure collections
                entity.HasMany(g => g.ChildGrammars)
                    .WithOne(g => g.ParentGrammar)
                    .HasForeignKey(g => g.ParentGrammarId);

                entity.HasMany(g => g.Definitions)
                    .WithOne(gd => gd.Grammar)
                    .HasForeignKey(gd => gd.GrammarId);

                entity.HasMany(g => g.Examples)
                    .WithOne(ge => ge.Grammar)
                    .HasForeignKey(ge => ge.GrammarId);

                entity.HasMany(g => g.Translations)
                    .WithOne(gt => gt.Grammar)
                    .HasForeignKey(gt => gt.GrammarId);

                entity.HasMany(g => g.MediaFiles)
                    .WithOne(m => m.Grammar)
                    .HasForeignKey(m => m.GrammarId);

                entity.HasMany(g => g.UserProgresses)
                    .WithOne(up => up.Grammar)
                    .HasForeignKey(up => up.GrammarId);
            });

            // GrammarDefinition configuration
            modelBuilder.Entity<GrammarDefinition>(entity =>
            {
                entity.ToTable("GrammarDefinitions");
                entity.HasKey(gd => gd.Id);

                // Relationships
                entity.HasOne(gd => gd.Grammar)
                    .WithMany(g => g.Definitions)
                    .HasForeignKey(gd => gd.GrammarId);

                entity.HasOne(gd => gd.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(gd => gd.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(gd => gd.UpdatedByUser)
                    .WithMany()
                    .HasForeignKey(gd => gd.UpdatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // GrammarExample configuration
            modelBuilder.Entity<GrammarExample>(entity =>
            {
                entity.ToTable("GrammarExamples");
                entity.HasKey(ge => ge.GrammarExampleId);

                // Add index for GrammarId
                entity.HasIndex(ge => ge.GrammarId)
                    .HasDatabaseName("IX_GrammarExample_Grammar");

                // Relationships
                entity.HasOne(ge => ge.Grammar)
                    .WithMany(g => g.Examples)
                    .HasForeignKey(ge => ge.GrammarId);

                entity.HasOne(ge => ge.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ge => ge.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ge => ge.UpdatedByUser)
                    .WithMany()
                    .HasForeignKey(ge => ge.UpdatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(ge => ge.MediaFiles)
                    .WithOne(m => m.GrammarExample)
                    .HasForeignKey(m => m.GrammarExampleId);
            });

            // GrammarTranslation configuration
            modelBuilder.Entity<GrammarTranslation>(entity =>
            {
                entity.ToTable("GrammarTranslations");
                entity.HasKey(gt => gt.Id);

                // Add unique index for GrammarId and LanguageCode
                entity.HasIndex(gt => new { gt.GrammarId, gt.LanguageCode })
                    .IsUnique()
                    .HasDatabaseName("IX_GrammarTranslation_Grammar_Lang");

                // Relationships
                entity.HasOne(gt => gt.Grammar)
                    .WithMany(g => g.Translations)
                    .HasForeignKey(gt => gt.GrammarId);

                entity.HasOne(gt => gt.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(gt => gt.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(gt => gt.UpdatedByUser)
                    .WithMany()
                    .HasForeignKey(gt => gt.UpdatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(gt => gt.VerifiedByUser)
                    .WithMany()
                    .HasForeignKey(gt => gt.VerifiedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        /// <summary>
        /// Configure scheduling-related entities
        /// </summary>
        private void ConfigureSchedulingEntities(ModelBuilder modelBuilder)
        {
            // Schedule configuration
            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedules");
                entity.HasKey(s => s.ScheduleId);

                // Add indexes
                entity.HasIndex(s => s.ScheduleName)
                    .HasDatabaseName("IX_Schedule_Name");
                entity.HasIndex(s => s.CreatedByUserId)
                    .HasDatabaseName("IX_Schedule_CreatedBy");
                entity.HasIndex(s => s.ScheduleType)
                    .HasDatabaseName("IX_Schedule_Type");
                entity.HasIndex(s => s.StudyPlanId)
                    .HasDatabaseName("IX_Schedule_StudyPlan");
                entity.HasIndex(s => s.IsPublic)
                    .HasDatabaseName("IX_Schedule_IsPublic");
                entity.HasIndex(s => s.IsActive)
                    .HasDatabaseName("IX_Schedule_IsActive");
                entity.HasIndex(s => new { s.ValidFrom, s.ValidTo })
                    .HasDatabaseName("IX_Schedule_ValidPeriod");

                // Apply global query filter for active schedules
                entity.HasQueryFilter(s => s.IsActive);

                // Relationships
                entity.HasOne(s => s.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(s => s.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.StudyPlan)
                    .WithMany(sp => sp.Schedules)
                    .HasForeignKey(s => s.StudyPlanId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(s => s.ScheduleItems)
                    .WithOne(si => si.Schedule)
                    .HasForeignKey(si => si.ScheduleId);
            });

            // ScheduleItemType configuration
            modelBuilder.Entity<ScheduleItemType>(entity =>
            {
                entity.ToTable("ScheduleItemTypes");
                entity.HasKey(sit => sit.TypeId);

                // Add indexes
                entity.HasIndex(sit => sit.TypeName)
                    .IsUnique()
                    .HasDatabaseName("IX_ScheduleItemType_Name");
                entity.HasIndex(sit => sit.DefaultColor)
                    .HasDatabaseName("IX_ScheduleItemType_Color");
                entity.HasIndex(sit => sit.DefaultDurationMinutes)
                    .HasDatabaseName("IX_ScheduleItemType_Duration");

                // Relationships
                entity.HasMany(sit => sit.ScheduleItems)
                    .WithOne(si => si.Type)
                    .HasForeignKey(si => si.TypeId);
            });

            // ScheduleRecurrence configuration
            modelBuilder.Entity<ScheduleRecurrence>(entity =>
            {
                entity.ToTable("ScheduleRecurrences");
                entity.HasKey(sr => sr.RecurrenceId);

                // Add indexes
                entity.HasIndex(sr => sr.RecurrenceType)
                    .HasDatabaseName("IX_ScheduleRecurrence_Type");
                entity.HasIndex(sr => sr.RecurrenceEnd)
                    .HasDatabaseName("IX_ScheduleRecurrence_End");
                entity.HasIndex(sr => sr.MaxOccurrences)
                    .HasDatabaseName("IX_ScheduleRecurrence_MaxOccurrences");

                // Relationships
                entity.HasMany(sr => sr.ScheduleItems)
                    .WithOne(si => si.Recurrence)
                    .HasForeignKey(si => si.RecurrenceId);
            });

            // ScheduleItem configuration
            modelBuilder.Entity<ScheduleItem>(entity =>
            {
                entity.ToTable("ScheduleItems");
                entity.HasKey(si => si.ScheduleItemId);

                // Add indexes
                entity.HasIndex(si => si.ScheduleId)
                    .HasDatabaseName("IX_ScheduleItem_Schedule");
                entity.HasIndex(si => si.TypeId)
                    .HasDatabaseName("IX_ScheduleItem_Type");
                entity.HasIndex(si => si.RecurrenceId)
                    .HasDatabaseName("IX_ScheduleItem_Recurrence");
                entity.HasIndex(si => si.StudyTaskId)
                    .HasDatabaseName("IX_ScheduleItem_StudyTask");
                entity.HasIndex(si => si.StartTime)
                    .HasDatabaseName("IX_ScheduleItem_StartTime");
                entity.HasIndex(si => si.EndTime)
                    .HasDatabaseName("IX_ScheduleItem_EndTime");
                entity.HasIndex(si => si.Status)
                    .HasDatabaseName("IX_ScheduleItem_Status");
                entity.HasIndex(si => si.IsAllDay)
                    .HasDatabaseName("IX_ScheduleItem_IsAllDay");
                entity.HasIndex(si => si.IsCancelled)
                    .HasDatabaseName("IX_ScheduleItem_IsCancelled");
                entity.HasIndex(si => si.IsCompleted)
                    .HasDatabaseName("IX_ScheduleItem_IsCompleted");
                entity.HasIndex(si => si.IsActive)
                    .HasDatabaseName("IX_ScheduleItem_IsActive");
                entity.HasIndex(si => si.PriorityLevel)
                    .HasDatabaseName("IX_ScheduleItem_Priority");
                entity.HasIndex(si => new { si.RelatedEntityType, si.RelatedEntityId })
                    .HasDatabaseName("IX_ScheduleItem_RelatedEntity");
                entity.HasIndex(si => new { si.SourceType, si.ExternalId })
                    .HasDatabaseName("IX_ScheduleItem_ExternalSource");

                // Apply global query filter for active items
                entity.HasQueryFilter(si => si.IsActive);

                // Relationships
                entity.HasOne(si => si.Schedule)
                    .WithMany(s => s.ScheduleItems)
                    .HasForeignKey(si => si.ScheduleId);

                entity.HasOne(si => si.Type)
                    .WithMany(sit => sit.ScheduleItems)
                    .HasForeignKey(si => si.TypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(si => si.Recurrence)
                    .WithMany(sr => sr.ScheduleItems)
                    .HasForeignKey(si => si.RecurrenceId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(si => si.StudyTask)
                    .WithMany(st => st.ScheduleItems)
                    .HasForeignKey(si => si.StudyTaskId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(si => si.Participants)
                    .WithOne(sip => sip.Item)
                    .HasForeignKey(sip => sip.ItemId);

                entity.HasMany(si => si.Reminders)
                    .WithOne(sr => sr.Item)
                    .HasForeignKey(sr => sr.ItemId);
            });

            // ScheduleItemParticipant configuration
            modelBuilder.Entity<ScheduleItemParticipant>(entity =>
            {
                entity.ToTable("ScheduleItemParticipants");
                entity.HasKey(sip => sip.ParticipantId);

                // Add indexes
                entity.HasIndex(sip => sip.ItemId)
                    .HasDatabaseName("IX_ScheduleItemParticipant_Item");
                entity.HasIndex(sip => sip.UserId)
                    .HasDatabaseName("IX_ScheduleItemParticipant_User");
                entity.HasIndex(sip => sip.GroupId)
                    .HasDatabaseName("IX_ScheduleItemParticipant_Group");
                entity.HasIndex(sip => sip.ParticipantRole)
                    .HasDatabaseName("IX_ScheduleItemParticipant_Role");
                entity.HasIndex(sip => sip.AttendanceStatus)
                    .HasDatabaseName("IX_ScheduleItemParticipant_Attendance");
                entity.HasIndex(sip => sip.ResponseStatus)
                    .HasDatabaseName("IX_ScheduleItemParticipant_Response");
                entity.HasIndex(sip => sip.JoinedAt)
                    .HasDatabaseName("IX_ScheduleItemParticipant_JoinTime");
                entity.HasIndex(sip => new { sip.ItemId, sip.UserId })
                    .IsUnique()
                    .HasDatabaseName("IX_ScheduleItemParticipant_Item_User");

                // Relationships
                entity.HasOne(sip => sip.Item)
                    .WithMany(si => si.Participants)
                    .HasForeignKey(sip => sip.ItemId);

                entity.HasOne(sip => sip.User)
                    .WithMany()
                    .HasForeignKey(sip => sip.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sip => sip.Group)
                    .WithMany()
                    .HasForeignKey(sip => sip.GroupId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ScheduleReminder configuration
            modelBuilder.Entity<ScheduleReminder>(entity =>
            {
                entity.ToTable("ScheduleReminders");
                entity.HasKey(sr => sr.ReminderId);

                // Add indexes
                entity.HasIndex(sr => sr.ItemId)
                    .HasDatabaseName("IX_ScheduleReminder_Item");
                entity.HasIndex(sr => sr.UserId)
                    .HasDatabaseName("IX_ScheduleReminder_User");
                entity.HasIndex(sr => sr.ReminderUnit)
                    .HasDatabaseName("IX_ScheduleReminder_Unit");
                entity.HasIndex(sr => sr.IsSent)
                    .HasDatabaseName("IX_ScheduleReminder_IsSent");
                entity.HasIndex(sr => sr.SentAt)
                    .HasDatabaseName("IX_ScheduleReminder_SentAt");
                entity.HasIndex(sr => sr.IsAcknowledged)
                    .HasDatabaseName("IX_ScheduleReminder_IsAcknowledged");
                entity.HasIndex(sr => new { sr.ItemId, sr.UserId, sr.ReminderTime, sr.ReminderUnit })
                    .HasDatabaseName("IX_ScheduleReminder_ItemUserTime");

                // Relationships
                entity.HasOne(sr => sr.Item)
                    .WithMany(si => si.Reminders)
                    .HasForeignKey(sr => sr.ItemId);

                entity.HasOne(sr => sr.User)
                    .WithMany()
                    .HasForeignKey(sr => sr.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        /// <summary>
        /// Configure notification-related entities
        /// </summary>
        private void ConfigureNotificationEntities(ModelBuilder modelBuilder)
        {
            // NotificationType configuration
            modelBuilder.Entity<NotificationType>(entity =>
            {
                entity.ToTable("NotificationTypes");
                entity.HasKey(nt => nt.TypeId);

                // Add indexes
                entity.HasIndex(nt => nt.TypeName)
                    .IsUnique()
                    .HasDatabaseName("IX_NotificationType_Name");

                // Relationships
                entity.HasMany(nt => nt.Notifications)
                    .WithOne(n => n.Type)
                    .HasForeignKey(n => n.TypeId);
            });

            // NotificationPriority configuration
            modelBuilder.Entity<NotificationPriority>(entity =>
            {
                entity.ToTable("NotificationPriorities");
                entity.HasKey(np => np.PriorityId);

                // Add indexes
                entity.HasIndex(np => np.PriorityName)
                    .IsUnique()
                    .HasDatabaseName("IX_NotificationPriority_Name");

                // Relationships
                entity.HasMany(np => np.Notifications)
                    .WithOne(n => n.Priority)
                    .HasForeignKey(n => n.PriorityId);
            });

            // NotificationStatus configuration
            modelBuilder.Entity<NotificationStatus>(entity =>
            {
                entity.ToTable("NotificationStatuses");
                entity.HasKey(ns => ns.StatusId);

                // Add indexes
                entity.HasIndex(ns => ns.StatusName)
                    .IsUnique()
                    .HasDatabaseName("IX_NotificationStatus_Name");

                // Relationships
                entity.HasMany(ns => ns.NotificationRecipients)
                    .WithOne(nr => nr.Status)
                    .HasForeignKey(nr => nr.StatusId);
            });

            // Notification configuration
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notifications");
                entity.HasKey(n => n.NotificationId);

                // Add indexes
                entity.HasIndex(n => n.SenderUserId).HasDatabaseName("IX_Notification_Sender");
                entity.HasIndex(n => n.TypeId).HasDatabaseName("IX_Notification_Type");
                entity.HasIndex(n => n.PriorityId).HasDatabaseName("IX_Notification_Priority");
                entity.HasIndex(n => n.IsSystemGenerated).HasDatabaseName("IX_Notification_IsSystem");
                entity.HasIndex(n => n.IsScheduled).HasDatabaseName("IX_Notification_IsScheduled");
                entity.HasIndex(n => n.ScheduledFor).HasDatabaseName("IX_Notification_ScheduledFor");
                entity.HasIndex(n => n.RelatedEntityType).HasDatabaseName("IX_Notification_EntityType");
                entity.HasIndex(n => new { n.RelatedEntityType, n.RelatedEntityId })
                    .HasDatabaseName("IX_Notification_Entity");

                // Configure soft delete behavior
                entity.HasQueryFilter(n => !n.IsDeleted);

                // Relationships
                entity.HasOne(n => n.SenderUser)
                    .WithMany()
                    .HasForeignKey(n => n.SenderUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(n => n.Type)
                    .WithMany(nt => nt.Notifications)
                    .HasForeignKey(n => n.TypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(n => n.Priority)
                    .WithMany(np => np.Notifications)
                    .HasForeignKey(n => n.PriorityId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(n => n.NotificationRecipients)
                    .WithOne(nr => nr.Notification)
                    .HasForeignKey(nr => nr.NotificationId);
            });

            // NotificationRecipient configuration
            modelBuilder.Entity<NotificationRecipient>(entity =>
            {
                entity.ToTable("NotificationRecipients");
                entity.HasKey(nr => nr.RecipientId);

                // Add indexes
                entity.HasIndex(nr => nr.NotificationId).HasDatabaseName("IX_NotificationRecipient_Notification");
                entity.HasIndex(nr => nr.UserId).HasDatabaseName("IX_NotificationRecipient_User");
                entity.HasIndex(nr => nr.GroupId).HasDatabaseName("IX_NotificationRecipient_Group");
                entity.HasIndex(nr => nr.StatusId).HasDatabaseName("IX_NotificationRecipient_Status");
                entity.HasIndex(nr => nr.IsDelivered).HasDatabaseName("IX_NotificationRecipient_IsDelivered");
                entity.HasIndex(nr => nr.IsOpened).HasDatabaseName("IX_NotificationRecipient_IsOpened");
                entity.HasIndex(nr => nr.IsArchived).HasDatabaseName("IX_NotificationRecipient_IsArchived");
                entity.HasIndex(nr => new { nr.NotificationId, nr.UserId })
                    .IsUnique()
                    .HasDatabaseName("IX_NotificationRecipient_Notification_User");

                // Relationships
                entity.HasOne(nr => nr.Notification)
                    .WithMany(n => n.NotificationRecipients)
                    .HasForeignKey(nr => nr.NotificationId);

                entity.HasOne(nr => nr.User)
                    .WithMany()
                    .HasForeignKey(nr => nr.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(nr => nr.Group)
                    .WithMany()
                    .HasForeignKey(nr => nr.GroupId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(nr => nr.Status)
                    .WithMany(ns => ns.NotificationRecipients)
                    .HasForeignKey(nr => nr.StatusId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(nr => nr.NotificationResponses)
                    .WithOne(nrs => nrs.Recipient)
                    .HasForeignKey(nrs => nrs.RecipientId);
            });

            // NotificationResponse configuration
            modelBuilder.Entity<NotificationResponse>(entity =>
            {
                entity.ToTable("NotificationResponses");
                entity.HasKey(nr => nr.ResponseId);

                // Add indexes
                entity.HasIndex(nr => nr.RecipientId).HasDatabaseName("IX_NotificationResponse_Recipient");
                entity.HasIndex(nr => nr.ResponseType).HasDatabaseName("IX_NotificationResponse_Type");
                entity.HasIndex(nr => nr.RespondedByUserId).HasDatabaseName("IX_NotificationResponse_User");
                entity.HasIndex(nr => nr.ResponseTime).HasDatabaseName("IX_NotificationResponse_Time");
                entity.HasIndex(nr => nr.ActionType).HasDatabaseName("IX_NotificationResponse_ActionType");

                // Relationships
                entity.HasOne(nr => nr.Recipient)
                    .WithMany(r => r.NotificationResponses)
                    .HasForeignKey(nr => nr.RecipientId);

                entity.HasOne(nr => nr.RespondedByUser)
                    .WithMany()
                    .HasForeignKey(nr => nr.RespondedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        /// <summary>
        /// Configure technical term-related entities
        /// </summary>
        private void ConfigureTechnicalTermEntities(ModelBuilder modelBuilder)
        {
            // TechnicalTerm configuration
            modelBuilder.Entity<TechnicalTerm>(entity =>
            {
                entity.ToTable("TechnicalTerms");
                entity.HasKey(tt => tt.TechnicalTermId);

                // Add indexes
                entity.HasIndex(tt => new { tt.Term, tt.LanguageCode, tt.Field })
                    .HasDatabaseName("IX_TechnicalTerm_Term_Lang_Field");

                entity.HasIndex(tt => tt.Field)
                    .HasDatabaseName("IX_TechnicalTerm_Field");

                // Configure relationships
                entity.HasOne(tt => tt.Category)
                    .WithMany()
                    .HasForeignKey(tt => tt.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(tt => tt.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(tt => tt.DeletedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                // Configure collections
                entity.HasMany(tt => tt.Examples)
                    .WithOne(te => te.Term)
                    .HasForeignKey(te => te.TermId);

                entity.HasMany(tt => tt.Translations)
                    .WithOne(tt => tt.Term)
                    .HasForeignKey(tt => tt.TermId);

                entity.HasMany(tt => tt.Relations)
                    .WithOne(tr => tr.Term1)
                    .HasForeignKey(tr => tr.TermId1);

                entity.HasMany(tt => tt.MediaFiles)
                    .WithOne(mf => mf.TechnicalTerm)
                    .HasForeignKey(mf => mf.TechnicalTermId);

                entity.HasMany(tt => tt.UserTechnicalTerms)
                    .WithOne(utt => utt.Term)
                    .HasForeignKey(utt => utt.TermId);
            });

            // TermExample configuration
            modelBuilder.Entity<TermExample>(entity =>
            {
                entity.ToTable("TermExamples");
                entity.HasKey(te => te.TermExampleId);

                // Add index for TermId
                entity.HasIndex(te => te.TermId)
                    .HasDatabaseName("IX_TermExample_Term");

                // Configure relationships
                entity.HasOne(te => te.Term)
                    .WithMany(tt => tt.Examples)
                    .HasForeignKey(te => te.TermId);

                entity.HasOne(te => te.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(te => te.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(te => te.UpdatedByUser)
                    .WithMany()
                    .HasForeignKey(te => te.UpdatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(te => te.MediaFiles)
                    .WithOne(mf => mf.TermExample)
                    .HasForeignKey(mf => mf.TermExampleId);
            });

            // TermTranslation configuration
            modelBuilder.Entity<TermTranslation>(entity =>
            {
                entity.ToTable("TermTranslations");
                entity.HasKey(tt => tt.Id);

                // Add unique index for TermId and LanguageCode
                entity.HasIndex(tt => new { tt.TermId, tt.LanguageCode })
                    .IsUnique()
                    .HasDatabaseName("IX_TermTranslation_Term_Lang");

                // Configure relationships
                entity.HasOne(tt => tt.Term)
                    .WithMany(t => t.Translations)
                    .HasForeignKey(tt => tt.TermId);

                entity.HasOne(tt => tt.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(tt => tt.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(tt => tt.UpdatedByUser)
                    .WithMany()
                    .HasForeignKey(tt => tt.UpdatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(tt => tt.ApprovedByUser)
                    .WithMany()
                    .HasForeignKey(tt => tt.ApprovedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // TermRelation configuration
            modelBuilder.Entity<TermRelation>(entity =>
            {
                entity.ToTable("TermRelations");
                entity.HasKey(tr => tr.Id);

                // Add unique index for TermId1 and TermId2
                entity.HasIndex(tr => new { tr.TermId1, tr.TermId2 })
                    .IsUnique()
                    .HasDatabaseName("IX_TermRelation_Term1_Term2");

                // Configure relationships
                entity.HasOne(tr => tr.Term1)
                    .WithMany(tt => tt.Relations)
                    .HasForeignKey(tr => tr.TermId1);

                entity.HasOne(tr => tr.Term2)
                    .WithMany()
                    .HasForeignKey(tr => tr.TermId2);

                entity.HasOne(tr => tr.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(tr => tr.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(tr => tr.UpdatedByUser)
                    .WithMany()
                    .HasForeignKey(tr => tr.UpdatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(tr => tr.VerifiedByUser)
                    .WithMany()
                    .HasForeignKey(tr => tr.VerifiedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // UserTechnicalTerm configuration
            modelBuilder.Entity<UserTechnicalTerm>(entity =>
            {
                entity.ToTable("UserTechnicalTerms");
                entity.HasKey(utt => utt.UserTermId);

                // Add unique index for UserId and TermId
                entity.HasIndex(utt => new { utt.UserId, utt.TermId })
                    .IsUnique()
                    .HasDatabaseName("IX_UserTechnicalTerm_User_Term");

                // Configure relationships
                entity.HasOne(utt => utt.User)
                    .WithMany()
                    .HasForeignKey(utt => utt.UserId);

                entity.HasOne(utt => utt.Term)
                    .WithMany(tt => tt.UserTechnicalTerms)
                    .HasForeignKey(utt => utt.TermId);
            });
        }

        /// <summary>
        /// Configure media-related entities
        /// </summary>
        private void ConfigureMediaEntities(ModelBuilder modelBuilder)
        {
            // MediaFile configuration
            modelBuilder.Entity<MediaFile>(entity =>
            {
                entity.ToTable("MediaFiles");
                entity.HasKey(m => m.MediaId);

                // Configure JSON serialization for metadata
                entity.Property(m => m.MetadataJson).HasColumnName("Metadata");

                // Configure composite index for vocabulary media query optimization
                entity.HasIndex(m => new { m.VocabularyId, m.MediaType, m.IsPrimary })
                    .HasDatabaseName("IX_MediaFiles_Vocabulary");

                // Configure composite index for user media query optimization
                entity.HasIndex(m => new { m.UserId, m.MediaType, m.IsPrimary })
                    .HasDatabaseName("IX_MediaFiles_User");
            });

            // MediaCategory configuration
            modelBuilder.Entity<MediaCategory>(entity =>
            {
                entity.ToTable("MediaCategories");
                entity.HasKey(mc => mc.CategoryId);

                entity.HasOne(mc => mc.ParentCategory)
                    .WithMany(mc => mc.ChildCategories)
                    .HasForeignKey(mc => mc.ParentCategoryId);
            });
        }

        /// <summary>
        /// Configure progress-related entities
        /// </summary>
        private void ConfigureProgressEntities(ModelBuilder modelBuilder)
        {
            // LearningProgress configuration
            modelBuilder.Entity<LearningProgress>(entity =>
            {
                entity.ToTable("LearningProgresses");
                entity.HasKey(lp => lp.ProgressId);

                // Composite unique constraint for user and vocabulary
                entity.HasIndex(lp => new { lp.UserId, lp.VocabularyId })
                    .IsUnique()
                    .HasDatabaseName("IX_LearningProgress_UserVocabulary");

                // Add other indexes defined in the model
                entity.HasIndex(lp => new { lp.UserId, lp.NextReviewDate })
                    .HasDatabaseName("IX_User_NextReview");
                entity.HasIndex(lp => lp.LastStudied)
                    .HasDatabaseName("IX_LearningProgress_LastStudied");
                entity.HasIndex(lp => lp.NextReviewDate)
                    .HasDatabaseName("IX_LearningProgress_NextReviewDate");

                // Relationships
                entity.HasOne(lp => lp.User)
                    .WithMany()
                    .HasForeignKey(lp => lp.UserId);

                entity.HasOne(lp => lp.Vocabulary)
                    .WithMany(v => v.LearningProgresses)
                    .HasForeignKey(lp => lp.VocabularyId);
            });

            // LearningSession configuration
            modelBuilder.Entity<LearningSession>(entity =>
            {
                entity.ToTable("LearningSessions");
                entity.HasKey(ls => ls.SessionId);

                // Add index defined in the model
                entity.HasIndex(ls => new { ls.UserId, ls.StartTime })
                    .HasDatabaseName("IX_User_Session");

                // Relationships
                entity.HasOne(ls => ls.User)
                    .WithMany(u => u.LearningSessions)
                    .HasForeignKey(ls => ls.UserId);

                entity.HasOne(ls => ls.Category)
                    .WithMany()
                    .HasForeignKey(ls => ls.CategoryId);

                entity.HasOne(ls => ls.Goal)
                    .WithMany()
                    .HasForeignKey(ls => ls.GoalId);

                entity.HasOne(ls => ls.VocabularyGroup)
                    .WithMany(vg => vg.LearningSessions)
                    .HasForeignKey(ls => ls.GroupId);

                entity.HasMany(ls => ls.SessionDetails)
                    .WithOne(sd => sd.Session)
                    .HasForeignKey(sd => sd.SessionId);
            });

            // LearningSessionDetail configuration
            modelBuilder.Entity<LearningSessionDetail>(entity =>
            {
                entity.ToTable("LearningSessionDetails");
                entity.HasKey(lsd => lsd.DetailId);

                entity.HasOne(lsd => lsd.Session)
                    .WithMany(ls => ls.SessionDetails)
                    .HasForeignKey(lsd => lsd.SessionId);
            });

            // UserKanjiProgress configuration
            modelBuilder.Entity<UserKanjiProgress>(entity =>
            {
                entity.ToTable("UserKanjiProgresses");
                entity.HasKey(ukp => ukp.ProgressId);

                // Add unique index for User and Kanji
                entity.HasIndex(ukp => new { ukp.UserId, ukp.KanjiId })
                    .IsUnique()
                    .HasDatabaseName("IX_UserKanjiProgress_User_Kanji");

                // Relationships
                entity.HasOne(ukp => ukp.User)
                    .WithMany()
                    .HasForeignKey(ukp => ukp.UserId);

                entity.HasOne(ukp => ukp.Kanji)
                    .WithMany(k => k.UserProgresses)
                    .HasForeignKey(ukp => ukp.KanjiId);
            });

            // UserGrammarProgress configuration
            modelBuilder.Entity<UserGrammarProgress>(entity =>
            {
                entity.ToTable("UserGrammarProgresses");
                entity.HasKey(ugp => ugp.ProgressId);

                // Add unique index for User and Grammar
                entity.HasIndex(ugp => new { ugp.UserId, ugp.GrammarId })
                    .IsUnique()
                    .HasDatabaseName("IX_UserGrammarProgress_User_Grammar");

                // Relationships
                entity.HasOne(ugp => ugp.User)
                    .WithMany()
                    .HasForeignKey(ugp => ugp.UserId);

                entity.HasOne(ugp => ugp.Grammar)
                    .WithMany(g => g.UserProgresses)
                    .HasForeignKey(ugp => ugp.GrammarId);
            });

            // GoalProgress configuration
            modelBuilder.Entity<GoalProgress>(entity =>
            {
                entity.ToTable("GoalProgresses");
                entity.HasKey(gp => gp.ProgressId);

                // Relationships
                entity.HasOne(gp => gp.Goal)
                    .WithMany()
                    .HasForeignKey(gp => gp.GoalId);
            });

            // PersonalWordList configuration
            modelBuilder.Entity<PersonalWordList>(entity =>
            {
                entity.ToTable("PersonalWordLists");
                entity.HasKey(pwl => pwl.ListId);

                // Add unique index for User and ListName
                entity.HasIndex(pwl => new { pwl.UserId, pwl.ListName })
                    .IsUnique()
                    .HasDatabaseName("IX_PersonalWordList_User_Name");

                // Relationships
                entity.HasOne(pwl => pwl.User)
                    .WithMany()
                    .HasForeignKey(pwl => pwl.UserId);

                entity.HasMany(pwl => pwl.Items)
                    .WithOne(pwli => pwli.List)
                    .HasForeignKey(pwli => pwli.ListId);
            });

            // PersonalWordListItem configuration
            modelBuilder.Entity<PersonalWordListItem>(entity =>
            {
                entity.ToTable("PersonalWordListItems");
                entity.HasKey(pwli => pwli.ItemId);

                // Add unique index for ListId and VocabularyId
                entity.HasIndex(pwli => new { pwli.ListId, pwli.VocabularyId })
                    .IsUnique()
                    .HasDatabaseName("IX_PersonalWordListItem_List_Vocab");

                // Relationships
                entity.HasOne(pwli => pwli.List)
                    .WithMany(pwl => pwl.Items)
                    .HasForeignKey(pwli => pwli.ListId);

                entity.HasOne(pwli => pwli.Vocabulary)
                    .WithMany()
                    .HasForeignKey(pwli => pwli.VocabularyId);
            });

            // StudySession configuration
            modelBuilder.Entity<StudySession>(entity =>
            {
                entity.ToTable("StudySessions");
                entity.HasKey(ss => ss.SessionId);

                entity.HasOne(ss => ss.User)
                    .WithMany()
                    .HasForeignKey(ss => ss.UserId);
            });
        }
        /// <summary>
        /// Configure practice-related entities
        /// </summary>
        private void ConfigurePracticeEntities(ModelBuilder modelBuilder)
        {
            // PracticeSet configuration
            modelBuilder.Entity<PracticeSet>(entity =>
            {
                entity.ToTable("PracticeSets");
                entity.HasKey(ps => ps.PracticeSetId);

                // Add indexes
                entity.HasIndex(ps => ps.SetName)
                    .HasDatabaseName("IX_PracticeSet_Name");

                entity.HasIndex(ps => ps.CreatedByUserId)
                    .HasDatabaseName("IX_PracticeSet_CreatedBy");

                entity.HasIndex(ps => ps.CategoryId)
                    .HasDatabaseName("IX_PracticeSet_Category");

                entity.HasIndex(ps => ps.Level)
                    .HasDatabaseName("IX_PracticeSet_Level");

                entity.HasIndex(ps => ps.IsPublic)
                    .HasDatabaseName("IX_PracticeSet_IsPublic");

                entity.HasIndex(ps => ps.IsFeatured)
                    .HasDatabaseName("IX_PracticeSet_IsFeatured");

                entity.HasIndex(ps => ps.IsActive)
                    .HasDatabaseName("IX_PracticeSet_IsActive");

                // Apply global query filter for active practice sets
                entity.HasQueryFilter(ps => ps.IsActive);

                // Relationships
                entity.HasOne(ps => ps.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ps => ps.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ps => ps.Category)
                    .WithMany()
                    .HasForeignKey(ps => ps.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(ps => ps.PracticeSetItems)
                    .WithOne(psi => psi.PracticeSet)
                    .HasForeignKey(psi => psi.PracticeSetId);

                entity.HasMany(ps => ps.UserPracticeSets)
                    .WithOne(ups => ups.PracticeSet)
                    .HasForeignKey(ups => ups.PracticeSetId);
            });

            // PracticeSetItem configuration
            modelBuilder.Entity<PracticeSetItem>(entity =>
            {
                entity.ToTable("PracticeSetItems");
                entity.HasKey(psi => psi.ItemId);

                // Add unique index for PracticeSetId and QuestionId
                entity.HasIndex(psi => new { psi.PracticeSetId, psi.QuestionId })
                    .IsUnique()
                    .HasDatabaseName("IX_PracticeSetItem_Set_Question");

                entity.HasIndex(psi => psi.OrderNumber)
                    .HasDatabaseName("IX_PracticeSetItem_Order");

                entity.HasIndex(psi => psi.PracticeMode)
                    .HasDatabaseName("IX_PracticeSetItem_Mode");

                // Relationships
                entity.HasOne(psi => psi.PracticeSet)
                    .WithMany(ps => ps.PracticeSetItems)
                    .HasForeignKey(psi => psi.PracticeSetId);

                entity.HasOne(psi => psi.Question)
                    .WithMany(q => q.PracticeSetItems)
                    .HasForeignKey(psi => psi.QuestionId);
            });

            // UserPracticeSet configuration
            modelBuilder.Entity<UserPracticeSet>(entity =>
            {
                entity.ToTable("UserPracticeSets");
                entity.HasKey(ups => ups.UserPracticeId);

                // Add index for UserId and PracticeSetId
                entity.HasIndex(ups => new { ups.UserId, ups.PracticeSetId })
                    .HasDatabaseName("IX_UserPractice_User_Set");

                entity.HasIndex(ups => ups.LastPracticed)
                    .HasDatabaseName("IX_UserPractice_LastPracticed");

                entity.HasIndex(ups => ups.CompletionPercentage)
                    .HasDatabaseName("IX_UserPractice_CompletionPercentage");

                entity.HasIndex(ups => ups.CorrectPercentage)
                    .HasDatabaseName("IX_UserPractice_CorrectPercentage");

                entity.HasIndex(ups => ups.UserRating)
                    .HasDatabaseName("IX_UserPractice_Rating");

                // Relationships
                entity.HasOne(ups => ups.User)
                    .WithMany()
                    .HasForeignKey(ups => ups.UserId);

                entity.HasOne(ups => ups.PracticeSet)
                    .WithMany(ps => ps.UserPracticeSets)
                    .HasForeignKey(ups => ups.PracticeSetId);

                entity.HasMany(ups => ups.UserPracticeAnswers)
                    .WithOne(upa => upa.UserPracticeSet)
                    .HasForeignKey(upa => upa.UserPracticeId);

                entity.HasMany(ups => ups.PracticeAnalytics)
                    .WithOne(pa => pa.UserPracticeSet)
                    .HasForeignKey(pa => pa.UserPracticeId);
            });

            // UserPracticeAnswer configuration
            modelBuilder.Entity<UserPracticeAnswer>(entity =>
            {
                entity.ToTable("UserPracticeAnswers");
                entity.HasKey(upa => upa.PracticeAnswerId);

                // Add index for UserPracticeId and QuestionId
                entity.HasIndex(upa => new { upa.UserPracticeId, upa.QuestionId })
                    .HasDatabaseName("IX_UserPracticeAnswer_Practice_Question");

                entity.HasIndex(upa => upa.IsCorrect)
                    .HasDatabaseName("IX_UserPracticeAnswer_IsCorrect");

                entity.HasIndex(upa => upa.AnsweredAt)
                    .HasDatabaseName("IX_UserPracticeAnswer_AnsweredAt");

                entity.HasIndex(upa => upa.NextReviewDate)
                    .HasDatabaseName("IX_UserPracticeAnswer_NextReview");

                entity.HasIndex(upa => upa.Difficulty)
                    .HasDatabaseName("IX_UserPracticeAnswer_Difficulty");

                // Relationships
                entity.HasOne(upa => upa.UserPracticeSet)
                    .WithMany(ups => ups.UserPracticeAnswers)
                    .HasForeignKey(upa => upa.UserPracticeId);

                entity.HasOne(upa => upa.Question)
                    .WithMany(q => q.UserPracticeAnswers)
                    .HasForeignKey(upa => upa.QuestionId);

                entity.HasOne(upa => upa.SelectedOption)
                    .WithMany()
                    .HasForeignKey(upa => upa.SelectedOptionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // TestResult configuration
            modelBuilder.Entity<TestResult>(entity =>
            {
                entity.ToTable("TestResults");
                entity.HasKey(tr => tr.TestResultId);

                // Add index for UserId and TestDate
                entity.HasIndex(tr => new { tr.UserId, tr.TestDate })
                    .HasDatabaseName("IX_TestResult_User_Date");

                entity.HasIndex(tr => tr.TestType)
                    .HasDatabaseName("IX_TestResult_Type");

                entity.HasIndex(tr => tr.Level)
                    .HasDatabaseName("IX_TestResult_Level");

                entity.HasIndex(tr => tr.CategoryId)
                    .HasDatabaseName("IX_TestResult_Category");

                entity.HasIndex(tr => tr.Score)
                    .HasDatabaseName("IX_TestResult_Score");

                // Relationships
                entity.HasOne(tr => tr.User)
                    .WithMany()
                    .HasForeignKey(tr => tr.UserId);

                entity.HasOne(tr => tr.Category)
                    .WithMany()
                    .HasForeignKey(tr => tr.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(tr => tr.TestDetails)
                    .WithOne(td => td.TestResult)
                    .HasForeignKey(td => td.TestResultId);
            });

            // TestDetail configuration
            modelBuilder.Entity<TestDetail>(entity =>
            {
                entity.ToTable("TestDetails");
                entity.HasKey(td => td.TestDetailId);

                // Add index for TestResultId
                entity.HasIndex(td => td.TestResultId)
                    .HasDatabaseName("IX_TestDetail_TestResult");

                entity.HasIndex(td => td.VocabularyId)
                    .HasDatabaseName("IX_TestDetail_Vocabulary");

                entity.HasIndex(td => td.QuestionId)
                    .HasDatabaseName("IX_TestDetail_Question");

                entity.HasIndex(td => td.IsCorrect)
                    .HasDatabaseName("IX_TestDetail_IsCorrect");

                entity.HasIndex(td => td.Difficulty)
                    .HasDatabaseName("IX_TestDetail_Difficulty");

                // Relationships
                entity.HasOne(td => td.TestResult)
                    .WithMany(tr => tr.TestDetails)
                    .HasForeignKey(td => td.TestResultId);

                entity.HasOne(td => td.Vocabulary)
                    .WithMany()
                    .HasForeignKey(td => td.VocabularyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(td => td.Question)
                    .WithMany()
                    .HasForeignKey(td => td.QuestionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // CustomExam configuration
            modelBuilder.Entity<CustomExam>(entity =>
            {
                entity.ToTable("CustomExams");
                entity.HasKey(ce => ce.CustomExamId);

                // Add indexes for UserId and ExamName
                entity.HasIndex(ce => new { ce.UserId, ce.ExamName })
                    .HasDatabaseName("IX_CustomExam_User_Name");

                entity.HasIndex(ce => ce.Level)
                    .HasDatabaseName("IX_CustomExam_Level");

                entity.HasIndex(ce => ce.ExamType)
                    .HasDatabaseName("IX_CustomExam_Type");

                entity.HasIndex(ce => ce.IsPublic)
                    .HasDatabaseName("IX_CustomExam_IsPublic");

                entity.HasIndex(ce => ce.IsFeatured)
                    .HasDatabaseName("IX_CustomExam_IsFeatured");

                entity.HasIndex(ce => ce.IsActive)
                    .HasDatabaseName("IX_CustomExam_IsActive");

                // Apply global query filter for active custom exams
                entity.HasQueryFilter(ce => ce.IsActive);

                // Relationships
                entity.HasOne(ce => ce.User)
                    .WithMany()
                    .HasForeignKey(ce => ce.UserId);

                entity.HasMany(ce => ce.CustomExamQuestions)
                    .WithOne(ceq => ceq.CustomExam)
                    .HasForeignKey(ceq => ceq.CustomExamId);

                entity.HasMany(ce => ce.UserExams)
                    .WithOne(ue => ue.CustomExam)
                    .HasForeignKey(ue => ue.CustomExamId);
            });

            // CustomExamQuestion configuration
            modelBuilder.Entity<CustomExamQuestion>(entity =>
            {
                entity.ToTable("CustomExamQuestions");
                entity.HasKey(ceq => ceq.CustomQuestionId);

                // Add unique index for CustomExamId and QuestionId
                entity.HasIndex(ceq => new { ceq.CustomExamId, ceq.QuestionId })
                    .IsUnique()
                    .HasDatabaseName("IX_CustomExamQuestion_Exam_Question");

                entity.HasIndex(ceq => ceq.OrderNumber)
                    .HasDatabaseName("IX_CustomExamQuestion_Order");

                entity.HasIndex(ceq => ceq.ScoreValue)
                    .HasDatabaseName("IX_CustomExamQuestion_Score");

                // Relationships
                entity.HasOne(ceq => ceq.CustomExam)
                    .WithMany(ce => ce.CustomExamQuestions)
                    .HasForeignKey(ceq => ceq.CustomExamId);

                entity.HasOne(ceq => ceq.Question)
                    .WithMany(q => q.CustomExamQuestions)
                    .HasForeignKey(ceq => ceq.QuestionId);
            });
        }

        /// <summary>
        /// Configure planning-related entities
        /// </summary>
        private void ConfigurePlanningEntities(ModelBuilder modelBuilder)
        {
            // StudyPlan configuration
            modelBuilder.Entity<StudyPlan>(entity =>
            {
                entity.ToTable("StudyPlans");
                entity.HasKey(sp => sp.StudyPlanId);

                // Add indexes
                entity.HasIndex(sp => new { sp.UserId, sp.PlanName })
                    .IsUnique()
                    .HasDatabaseName("IX_StudyPlan_User_Name");
                entity.HasIndex(sp => sp.TargetLevel)
                    .HasDatabaseName("IX_StudyPlan_TargetLevel");
                entity.HasIndex(sp => sp.PlanType)
                    .HasDatabaseName("IX_StudyPlan_Type");
                entity.HasIndex(sp => sp.IsActive)
                    .HasDatabaseName("IX_StudyPlan_IsActive");
                entity.HasIndex(sp => sp.CurrentStatus)
                    .HasDatabaseName("IX_StudyPlan_Status");

                // Apply global query filter for active plans
                entity.HasQueryFilter(sp => sp.IsActive);

                // Relationships
                entity.HasOne(sp => sp.User)
                    .WithMany()
                    .HasForeignKey(sp => sp.UserId);

                entity.HasOne(sp => sp.JLPTLevel)
                    .WithMany()
                    .HasForeignKey(sp => sp.TargetLevel)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(sp => sp.StudyGoals)
                    .WithOne(sg => sg.Plan)
                    .HasForeignKey(sg => sg.PlanId);

                entity.HasMany(sp => sp.StudyPlanItems)
                    .WithOne(spi => spi.Plan)
                    .HasForeignKey(spi => spi.PlanId);

                entity.HasMany(sp => sp.Schedules)
                    .WithOne(p => p.StudyPlan)
                    .HasForeignKey(s => s.StudyPlanId);
            });

            // StudyGoal configuration
            modelBuilder.Entity<StudyGoal>(entity =>
            {
                entity.ToTable("StudyGoals");
                entity.HasKey(sg => sg.GoalId);

                // Add indexes
                entity.HasIndex(sg => new { sg.PlanId, sg.GoalName })
                    .IsUnique()
                    .HasDatabaseName("IX_StudyGoal_Plan_Name");
                entity.HasIndex(sg => sg.LevelId)
                    .HasDatabaseName("IX_StudyGoal_Level");
                entity.HasIndex(sg => sg.TopicId)
                    .HasDatabaseName("IX_StudyGoal_Topic");
                entity.HasIndex(sg => sg.GoalType)
                    .HasDatabaseName("IX_StudyGoal_Type");
                entity.HasIndex(sg => sg.Status)
                    .HasDatabaseName("IX_StudyGoal_Status");
                entity.HasIndex(sg => sg.IsCompleted)
                    .HasDatabaseName("IX_StudyGoal_IsCompleted");

                // Relationships
                entity.HasOne(sg => sg.Plan)
                    .WithMany(sp => sp.StudyGoals)
                    .HasForeignKey(sg => sg.PlanId);

                entity.HasOne(sg => sg.Level)
                    .WithMany(jl => jl.StudyGoals)
                    .HasForeignKey(sg => sg.LevelId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sg => sg.Topic)
                    .WithMany(st => st.StudyGoals)
                    .HasForeignKey(sg => sg.TopicId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sg => sg.ParentGoal)
                    .WithMany(sg => sg.ChildGoals)
                    .HasForeignKey(sg => sg.ParentGoalId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(sg => sg.StudyTasks)
                    .WithOne(st => st.Goal)
                    .HasForeignKey(st => st.GoalId);

                entity.HasMany(sg => sg.ChildGoals)
                    .WithOne(sg => sg.ParentGoal)
                    .HasForeignKey(sg => sg.ParentGoalId);

                entity.HasMany(sg => sg.StudyReportItems)
                    .WithOne(sri => sri.Goal)
                    .HasForeignKey(sri => sri.GoalId);

                entity.HasMany(sg => sg.StrengthWeaknesses)
                    .WithOne(sw => sw.RelatedGoal)
                    .HasForeignKey(sw => sw.RelatedGoalId);
            });

            // StudyTask configuration
            modelBuilder.Entity<StudyTask>(entity =>
            {
                entity.ToTable("StudyTasks");
                entity.HasKey(st => st.StudyTaskId);

                // Add indexes
                entity.HasIndex(st => new { st.GoalId, st.TaskName })
                    .HasDatabaseName("IX_StudyTask_Goal_Name");
                entity.HasIndex(st => st.ItemId)
                    .HasDatabaseName("IX_StudyTask_Item");
                entity.HasIndex(st => st.Priority)
                    .HasDatabaseName("IX_StudyTask_Priority");
                entity.HasIndex(st => st.TaskType)
                    .HasDatabaseName("IX_StudyTask_Type");
                entity.HasIndex(st => st.TaskCategory)
                    .HasDatabaseName("IX_StudyTask_Category");
                entity.HasIndex(st => st.Status)
                    .HasDatabaseName("IX_StudyTask_Status");
                entity.HasIndex(st => st.ScheduledDate)
                    .HasDatabaseName("IX_StudyTask_ScheduledDate");
                entity.HasIndex(st => st.DueDate)
                    .HasDatabaseName("IX_StudyTask_DueDate");
                entity.HasIndex(st => st.IsCompleted)
                    .HasDatabaseName("IX_StudyTask_IsCompleted");

                // Relationships
                entity.HasOne(st => st.Goal)
                    .WithMany(sg => sg.StudyTasks)
                    .HasForeignKey(st => st.GoalId);

                entity.HasOne(st => st.Item)
                    .WithMany(spi => spi.StudyTasks)
                    .HasForeignKey(st => st.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(st => st.TaskCompletions)
                    .WithOne(tc => tc.Task)
                    .HasForeignKey(tc => tc.TaskId);

                entity.HasMany(st => st.ScheduleItems)
                    .WithOne(ss => ss.StudyTask)
                    .HasForeignKey(si => si.StudyTaskId);
            });

            // StudyTopic configuration
            modelBuilder.Entity<StudyTopic>(entity =>
            {
                entity.ToTable("StudyTopics");
                entity.HasKey(st => st.TopicId);

                // Add indexes
                entity.HasIndex(st => new { st.TopicName, st.Category })
                    .IsUnique()
                    .HasDatabaseName("IX_StudyTopic_Name_Category");
                entity.HasIndex(st => st.Category)
                    .HasDatabaseName("IX_StudyTopic_Category");
                entity.HasIndex(st => st.JLPTLevel)
                    .HasDatabaseName("IX_StudyTopic_JLPT");
                entity.HasIndex(st => st.Difficulty)
                    .HasDatabaseName("IX_StudyTopic_Difficulty");
                entity.HasIndex(st => st.DisplayOrder)
                    .HasDatabaseName("IX_StudyTopic_DisplayOrder");

                // Relationships
                entity.HasOne(st => st.ParentTopic)
                    .WithMany(st => st.ChildTopics)
                    .HasForeignKey(st => st.ParentTopicId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(st => st.ChildTopics)
                    .WithOne(st => st.ParentTopic)
                    .HasForeignKey(st => st.ParentTopicId);

                entity.HasMany(st => st.StudyGoals)
                    .WithOne(sg => sg.Topic)
                    .HasForeignKey(sg => sg.TopicId);

                entity.HasMany(st => st.StudyPlanItems)
                    .WithOne(spi => spi.Topic)
                    .HasForeignKey(spi => spi.TopicId);
            });

            // StudyPlanItem configuration
            modelBuilder.Entity<StudyPlanItem>(entity =>
            {
                entity.ToTable("StudyPlanItems");
                entity.HasKey(spi => spi.ItemId);

                // Add indexes
                entity.HasIndex(spi => new { spi.PlanId, spi.ItemType })
                    .HasDatabaseName("IX_StudyPlanItem_Plan_Type");
                entity.HasIndex(spi => spi.TopicId)
                    .HasDatabaseName("IX_StudyPlanItem_Topic");
                entity.HasIndex(spi => spi.VocabularyGroupId)
                    .HasDatabaseName("IX_StudyPlanItem_VocabularyGroup");
                entity.HasIndex(spi => spi.GrammarId)
                    .HasDatabaseName("IX_StudyPlanItem_Grammar");
                entity.HasIndex(spi => spi.ExamId)
                    .HasDatabaseName("IX_StudyPlanItem_Exam");
                entity.HasIndex(spi => spi.ScheduledDate)
                    .HasDatabaseName("IX_StudyPlanItem_ScheduledDate");
                entity.HasIndex(spi => spi.Status)
                    .HasDatabaseName("IX_StudyPlanItem_Status");
                entity.HasIndex(spi => spi.Priority)
                    .HasDatabaseName("IX_StudyPlanItem_Priority");
                entity.HasIndex(spi => spi.IsRecurring)
                    .HasDatabaseName("IX_StudyPlanItem_IsRecurring");

                // Relationships
                entity.HasOne(spi => spi.Plan)
                    .WithMany(sp => sp.StudyPlanItems)
                    .HasForeignKey(spi => spi.PlanId);

                entity.HasOne(spi => spi.Topic)
                    .WithMany(st => st.StudyPlanItems)
                    .HasForeignKey(spi => spi.TopicId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(spi => spi.VocabularyGroup)
                    .WithMany()
                    .HasForeignKey(spi => spi.VocabularyGroupId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(spi => spi.Grammar)
                    .WithMany()
                    .HasForeignKey(spi => spi.GrammarId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(spi => spi.Exam)
                    .WithMany()
                    .HasForeignKey(spi => spi.ExamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(spi => spi.StudyPlanProgresses)
                    .WithOne(spp => spp.Item)
                    .HasForeignKey(spp => spp.ItemId);

                entity.HasMany(spi => spi.StudyTasks)
                    .WithOne(st => st.Item)
                    .HasForeignKey(st => st.ItemId);
            });

            // StudyPlanProgress configuration
            modelBuilder.Entity<StudyPlanProgress>(entity =>
            {
                entity.ToTable("StudyPlanProgresses");
                entity.HasKey(spp => spp.ProgressId);

                // Add indexes
                entity.HasIndex(spp => spp.ItemId)
                    .HasDatabaseName("IX_StudyPlanProgress_Item");
                entity.HasIndex(spp => spp.Status)
                    .HasDatabaseName("IX_StudyPlanProgress_Status");
                entity.HasIndex(spp => spp.CompletedDate)
                    .HasDatabaseName("IX_StudyPlanProgress_CompletedDate");

                // Relationships
                entity.HasOne(spp => spp.Item)
                    .WithMany(spi => spi.StudyPlanProgresses)
                    .HasForeignKey(spp => spp.ItemId);
            });

            // TaskCompletion configuration
            modelBuilder.Entity<TaskCompletion>(entity =>
            {
                entity.ToTable("TaskCompletions");
                entity.HasKey(tc => tc.CompletionId);

                // Add indexes
                entity.HasIndex(tc => new { tc.TaskId, tc.CompletionDate })
                    .HasDatabaseName("IX_TaskCompletion_Task_Date");
                entity.HasIndex(tc => tc.CompletedByUserId)
                    .HasDatabaseName("IX_TaskCompletion_CompletedBy");
                entity.HasIndex(tc => tc.CompletionStatus)
                    .HasDatabaseName("IX_TaskCompletion_Status");

                // Relationships
                entity.HasOne(tc => tc.Task)
                    .WithMany(st => st.TaskCompletions)
                    .HasForeignKey(tc => tc.TaskId);

                entity.HasOne(tc => tc.CompletedByUser)
                    .WithMany()
                    .HasForeignKey(tc => tc.CompletedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // StudySession configuration
            modelBuilder.Entity<StudySession>(entity =>
            {
                entity.ToTable("StudySessions");
                entity.HasKey(ss => ss.SessionId);

                // Add indexes
                entity.HasIndex(ss => new { ss.UserId, ss.StartTime })
                    .HasDatabaseName("IX_StudySession_User_StartTime");
                entity.HasIndex(ss => ss.SessionType)
                    .HasDatabaseName("IX_StudySession_Type");
                entity.HasIndex(ss => ss.ContentType)
                    .HasDatabaseName("IX_StudySession_ContentType");
                entity.HasIndex(ss => ss.CategoryId)
                    .HasDatabaseName("IX_StudySession_Category");
                entity.HasIndex(ss => ss.StudyPlanId)
                    .HasDatabaseName("IX_StudySession_Plan");
                entity.HasIndex(ss => ss.Platform)
                    .HasDatabaseName("IX_StudySession_Platform");
                entity.HasIndex(ss => ss.StudyMode)
                    .HasDatabaseName("IX_StudySession_Mode");

                // Relationships
                entity.HasOne(ss => ss.User)
                    .WithMany()
                    .HasForeignKey(ss => ss.UserId);

                entity.HasOne(ss => ss.Category)
                    .WithMany()
                    .HasForeignKey(ss => ss.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ss => ss.StudyPlan)
                    .WithMany()
                    .HasForeignKey(ss => ss.StudyPlanId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(ss => ss.SessionDetails)
                    .WithOne()
                    .HasForeignKey(sd => sd.SessionId);
            });
        }

        /// <summary>
        /// Configure exam-related entities
        /// </summary>
        private void ConfigureExamEntities(ModelBuilder modelBuilder)
        {
            // JLPTLevel configuration
            modelBuilder.Entity<JLPTLevel>(entity =>
            {
                entity.ToTable("JLPTLevels");
                entity.HasKey(jl => jl.LevelId);

                // Self-reference for prerequisites
                entity.HasOne(jl => jl.PrerequisiteLevel)
                    .WithMany()
                    .HasForeignKey(jl => jl.PrerequisiteLevelId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relationships with other entities
                entity.HasMany(jl => jl.StudyGoals)
                    .WithOne()
                    .HasForeignKey(sg => sg.LevelId);

                entity.HasMany(jl => jl.Exams)
                    .WithOne()
                    .HasForeignKey(e => e.Level);
            });

            // JLPTExam configuration
            modelBuilder.Entity<JLPTExam>(entity =>
            {
                entity.ToTable("JLPTExams");
                entity.HasKey(je => je.ExamId);

                // Create composite unique index for Level, Year, Month
                entity.HasIndex(je => new { je.Level, je.Year, je.Month })
                    .IsUnique()
                    .HasDatabaseName("IX_JLPTExam_Level_Date");

                // Create index for ExamName
                entity.HasIndex(je => je.ExamName)
                    .HasDatabaseName("IX_Exam_Name");

                // Relationships
                entity.HasOne(je => je.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(je => je.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(je => je.Sections)
                    .WithOne(js => js.Exam)
                    .HasForeignKey(js => js.ExamId);

                entity.HasMany(je => je.UserExams)
                    .WithOne(ue => ue.Exam)
                    .HasForeignKey(ue => ue.ExamId);
            });

            // JLPTSection configuration
            modelBuilder.Entity<JLPTSection>(entity =>
            {
                entity.ToTable("JLPTSections");
                entity.HasKey(js => js.SectionId);

                // Create composite unique index for ExamId and SectionName
                entity.HasIndex(js => new { js.ExamId, js.SectionName })
                    .IsUnique()
                    .HasDatabaseName("IX_Section_Exam_Name");

                // Relationships
                entity.HasOne(js => js.Exam)
                    .WithMany(je => je.Sections)
                    .HasForeignKey(js => js.ExamId);

                entity.HasMany(js => js.Questions)
                    .WithOne(q => q.Section)
                    .HasForeignKey(q => q.SectionId);
            });

            // Question configuration
            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Questions");
                entity.HasKey(q => q.QuestionId);

                // Create index for SectionId and QuestionType
                entity.HasIndex(q => new { q.SectionId, q.QuestionType })
                    .HasDatabaseName("IX_Question_Section_Type");

                // Create index for CreatedByUserId
                entity.HasIndex(q => q.CreatedByUserId)
                    .HasDatabaseName("IX_Question_CreatedBy");

                // Relationships
                entity.HasOne(q => q.Section)
                    .WithMany(js => js.Questions)
                    .HasForeignKey(q => q.SectionId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(q => q.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(q => q.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(q => q.Options)
                    .WithOne(qo => qo.Question)
                    .HasForeignKey(qo => qo.QuestionId);

                entity.HasMany(q => q.UserAnswers)
                    .WithOne(ua => ua.Question)
                    .HasForeignKey(ua => ua.QuestionId);

                entity.HasMany(q => q.CustomExamQuestions)
                    .WithOne()
                    .HasForeignKey(ceq => ceq.QuestionId);

                entity.HasMany(q => q.PracticeSetItems)
                    .WithOne()
                    .HasForeignKey(psi => psi.QuestionId);

                entity.HasMany(q => q.UserPracticeAnswers)
                    .WithOne()
                    .HasForeignKey(upa => upa.QuestionId);

                entity.HasMany(q => q.MediaFiles)
                    .WithOne()
                    .HasForeignKey(mf => mf.QuestionId);
            });

            // QuestionOption configuration
            modelBuilder.Entity<QuestionOption>(entity =>
            {
                entity.ToTable("QuestionOptions");
                entity.HasKey(qo => qo.QuestionOptionId);

                // Relationships
                entity.HasOne(qo => qo.Question)
                    .WithMany(q => q.Options)
                    .HasForeignKey(qo => qo.QuestionId);

                entity.HasMany(qo => qo.UserAnswers)
                    .WithOne(ua => ua.SelectedOption)
                    .HasForeignKey(ua => ua.SelectedOptionId);

                entity.HasMany(qo => qo.MediaFiles)
                    .WithOne()
                    .HasForeignKey(mf => mf.QuestionOptionId);
            });

            // UserExam configuration
            modelBuilder.Entity<UserExam>(entity =>
            {
                entity.ToTable("UserExams");
                entity.HasKey(ue => ue.UserExamId);

                // Create index for UserId and ExamId
                entity.HasIndex(ue => new { ue.UserId, ue.ExamId })
                    .HasDatabaseName("IX_UserExam_User_Exam");

                // Relationships
                entity.HasOne(ue => ue.User)
                    .WithMany()
                    .HasForeignKey(ue => ue.UserId);

                entity.HasOne(ue => ue.Exam)
                    .WithMany(je => je.UserExams)
                    .HasForeignKey(ue => ue.ExamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ue => ue.CustomExam)
                    .WithMany(ce => ce.UserExams)
                    .HasForeignKey(ue => ue.CustomExamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(ue => ue.UserAnswers)
                    .WithOne(ua => ua.UserExam)
                    .HasForeignKey(ua => ua.UserExamId);

                entity.HasMany(ue => ue.ExamAnalytics)
                    .WithOne()
                    .HasForeignKey(ea => ea.UserExamId);
            });

            // UserAnswer configuration
            modelBuilder.Entity<UserAnswer>(entity =>
            {
                entity.ToTable("UserAnswers");
                entity.HasKey(ua => ua.AnswerId);

                // Create composite unique index for UserExamId and QuestionId
                entity.HasIndex(ua => new { ua.UserExamId, ua.QuestionId })
                    .IsUnique()
                    .HasDatabaseName("IX_UserAnswer_Exam_Question");

                // Relationships
                entity.HasOne(ua => ua.UserExam)
                    .WithMany(ue => ue.UserAnswers)
                    .HasForeignKey(ua => ua.UserExamId);

                entity.HasOne(ua => ua.Question)
                    .WithMany(q => q.UserAnswers)
                    .HasForeignKey(ua => ua.QuestionId);

                entity.HasOne(ua => ua.SelectedOption)
                    .WithMany(qo => qo.UserAnswers)
                    .HasForeignKey(ua => ua.SelectedOptionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // CustomExam configuration
            modelBuilder.Entity<CustomExam>(entity =>
            {
                entity.ToTable("CustomExams");
                entity.HasKey(ce => ce.CustomExamId);

                // Create index for UserId and ExamName
                entity.HasIndex(ce => new { ce.UserId, ce.ExamName })
                    .HasDatabaseName("IX_CustomExam_User_Name");

                // Relationships
                entity.HasOne(ce => ce.User)
                    .WithMany()
                    .HasForeignKey(ce => ce.UserId);

                entity.HasMany(ce => ce.CustomExamQuestions)
                    .WithOne()
                    .HasForeignKey(ceq => ceq.CustomExamId);

                entity.HasMany(ce => ce.UserExams)
                    .WithOne(ue => ue.CustomExam)
                    .HasForeignKey(ue => ue.CustomExamId);
            });

            // CustomExamQuestion configuration
            modelBuilder.Entity<CustomExamQuestion>(entity =>
            {
                entity.ToTable("CustomExamQuestions");
                entity.HasKey(ceq => ceq.CustomQuestionId);

                // Relationships
                entity.HasOne(ceq => ceq.CustomExam)
                    .WithMany(ce => ce.CustomExamQuestions)
                    .HasForeignKey(ceq => ceq.CustomExamId);

                entity.HasOne(ceq => ceq.Question)
                    .WithMany(q => q.CustomExamQuestions)
                    .HasForeignKey(ceq => ceq.QuestionId);
            });

            // UserPracticeSet configuration
            modelBuilder.Entity<UserPracticeSet>(entity =>
            {
                entity.ToTable("UserPracticeSets");
                entity.HasKey(ups => ups.PracticeSetId);

                // Relationships
                entity.HasOne(ups => ups.User)
                    .WithMany()
                    .HasForeignKey(ups => ups.UserId);
            });
        }

        /// <summary>
        /// Configure report-related entities
        /// </summary>
        private void ConfigureReportEntities(ModelBuilder modelBuilder)
        {
            // StudyReport configuration
            modelBuilder.Entity<StudyReport>(entity =>
            {
                entity.ToTable("StudyReports");
                entity.HasKey(sr => sr.ReportId);

                entity.HasMany(sr => sr.StudyReportItems)
                    .WithOne(sri => sri.Report)
                    .HasForeignKey(sri => sri.ReportId);
            });
        }

        /// <summary>
        /// Configure submission-related entities
        /// </summary>
        private void ConfigureSubmissionEntities(ModelBuilder modelBuilder)
        {
            // SubmissionStatus configuration
            modelBuilder.Entity<SubmissionStatus>(entity =>
            {
                entity.ToTable("SubmissionStatuses");
                entity.HasKey(ss => ss.StatusId);

                // Add indexes
                entity.HasIndex(ss => ss.StatusName)
                    .IsUnique()
                    .HasDatabaseName("IX_SubmissionStatus_Name");
                entity.HasIndex(ss => ss.DisplayOrder)
                    .HasDatabaseName("IX_SubmissionStatus_DisplayOrder");
                entity.HasIndex(ss => ss.IsInitial)
                    .HasDatabaseName("IX_SubmissionStatus_IsInitial");
                entity.HasIndex(ss => ss.IsTerminal)
                    .HasDatabaseName("IX_SubmissionStatus_IsTerminal");
                entity.HasIndex(ss => ss.IsDefaultStatus)
                    .HasDatabaseName("IX_SubmissionStatus_IsDefault");

                // Relationships
                entity.HasOne(ss => ss.AutoTransitionToStatus)
                    .WithMany()
                    .HasForeignKey(ss => ss.AutoTransitionToStatusId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(ss => ss.FromTransitions)
                    .WithOne(st => st.FromStatus)
                    .HasForeignKey(st => st.FromStatusId);

                entity.HasMany(ss => ss.ToTransitions)
                    .WithOne(st => st.ToStatus)
                    .HasForeignKey(st => st.ToStatusId);

                entity.HasMany(ss => ss.Submissions)
                    .WithOne(uvs => uvs.Status)
                    .HasForeignKey(uvs => uvs.StatusId);
            });

            // StatusTransition configuration
            modelBuilder.Entity<StatusTransition>(entity =>
            {
                entity.ToTable("StatusTransitions");
                entity.HasKey(st => st.TransitionId);

                // Add unique index for FromStatusId and ToStatusId
                entity.HasIndex(st => new { st.FromStatusId, st.ToStatusId })
                    .IsUnique()
                    .HasDatabaseName("IX_StatusTransition_From_To");

                entity.HasIndex(st => st.DisplayOrder)
                    .HasDatabaseName("IX_StatusTransition_DisplayOrder");

                // Relationships
                entity.HasOne(st => st.FromStatus)
                    .WithMany(ss => ss.FromTransitions)
                    .HasForeignKey(st => st.FromStatusId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(st => st.ToStatus)
                    .WithMany(ss => ss.ToTransitions)
                    .HasForeignKey(st => st.ToStatusId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // UserVocabularySubmission configuration
            modelBuilder.Entity<UserVocabularySubmission>(entity =>
            {
                entity.ToTable("UserVocabularySubmissions");
                entity.HasKey(uvs => uvs.SubmissionId);

                // Add indexes
                entity.HasIndex(uvs => new { uvs.UserId, uvs.SubmissionDate })
                    .HasDatabaseName("IX_UserVocabularySubmission_User_Date");
                entity.HasIndex(uvs => uvs.StatusId)
                    .HasDatabaseName("IX_UserVocabularySubmission_Status");
                entity.HasIndex(uvs => uvs.SubmissionType)
                    .HasDatabaseName("IX_UserVocabularySubmission_Type");
                entity.HasIndex(uvs => uvs.SubmissionCategory)
                    .HasDatabaseName("IX_UserVocabularySubmission_Category");
                entity.HasIndex(uvs => uvs.JLPTLevel)
                    .HasDatabaseName("IX_UserVocabularySubmission_JLPTLevel");
                entity.HasIndex(uvs => uvs.ReviewerId)
                    .HasDatabaseName("IX_UserVocabularySubmission_Reviewer");
                entity.HasIndex(uvs => uvs.ReviewedAt)
                    .HasDatabaseName("IX_UserVocabularySubmission_ReviewedAt");
                entity.HasIndex(uvs => uvs.ReviewResult)
                    .HasDatabaseName("IX_UserVocabularySubmission_ReviewResult");
                entity.HasIndex(uvs => uvs.IsDeleted)
                    .HasDatabaseName("IX_UserVocabularySubmission_IsDeleted");

                // Configure soft delete behavior
                entity.HasQueryFilter(uvs => !uvs.IsDeleted);

                // Relationships
                entity.HasOne(uvs => uvs.User)
                    .WithMany()
                    .HasForeignKey(uvs => uvs.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(uvs => uvs.Status)
                    .WithMany(ss => ss.Submissions)
                    .HasForeignKey(uvs => uvs.StatusId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(uvs => uvs.Reviewer)
                    .WithMany()
                    .HasForeignKey(uvs => uvs.ReviewerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(uvs => uvs.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(uvs => uvs.DeletedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(uvs => uvs.VocabularyDetails)
                    .WithOne(uvd => uvd.Submission)
                    .HasForeignKey(uvd => uvd.SubmissionId);

                entity.HasMany(uvs => uvs.ApprovalHistories)
                    .WithOne(ah => ah.Submission)
                    .HasForeignKey(ah => ah.SubmissionId);
            });

            // UserVocabularyDetail configuration
            modelBuilder.Entity<UserVocabularyDetail>(entity =>
            {
                entity.ToTable("UserVocabularyDetails");
                entity.HasKey(uvd => uvd.UserVocabularyDetaillId);

                // Add indexes
                entity.HasIndex(uvd => new { uvd.SubmissionId, uvd.VocabularyId })
                    .HasDatabaseName("IX_UserVocabularyDetail_Submission_Vocab");
                entity.HasIndex(uvd => uvd.Word)
                    .HasDatabaseName("IX_UserVocabularyDetail_Word");
                entity.HasIndex(uvd => uvd.Reading)
                    .HasDatabaseName("IX_UserVocabularyDetail_Reading");
                entity.HasIndex(uvd => uvd.JLPTLevel)
                    .HasDatabaseName("IX_UserVocabularyDetail_JLPTLevel");
                entity.HasIndex(uvd => uvd.ReviewStatus)
                    .HasDatabaseName("IX_UserVocabularyDetail_ReviewStatus");
                entity.HasIndex(uvd => uvd.Accuracy)
                    .HasDatabaseName("IX_UserVocabularyDetail_Accuracy");

                // Relationships
                entity.HasOne(uvd => uvd.Submission)
                    .WithMany(uvs => uvs.VocabularyDetails)
                    .HasForeignKey(uvd => uvd.SubmissionId);

                entity.HasOne(uvd => uvd.Vocabulary)
                    .WithMany()
                    .HasForeignKey(uvd => uvd.VocabularyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(uvd => uvd.MediaFiles)
                    .WithOne(m => m.UserVocabularyDetail)
                    .HasForeignKey(mf => mf.UserVocabularyDetaillId);
            });

            // ApprovalHistory configuration
            modelBuilder.Entity<ApprovalHistory>(entity =>
            {
                entity.ToTable("ApprovalHistories");
                entity.HasKey(ah => ah.HistoryId);

                // Add indexes
                entity.HasIndex(ah => new { ah.SubmissionId, ah.ActionDate })
                    .HasDatabaseName("IX_ApprovalHistory_Submission_Date");
                entity.HasIndex(ah => ah.UserId)
                    .HasDatabaseName("IX_ApprovalHistory_User");
                entity.HasIndex(ah => ah.Action)
                    .HasDatabaseName("IX_ApprovalHistory_Action");
                entity.HasIndex(ah => ah.FromStatusId)
                    .HasDatabaseName("IX_ApprovalHistory_FromStatus");
                entity.HasIndex(ah => ah.ToStatusId)
                    .HasDatabaseName("IX_ApprovalHistory_ToStatus");
                entity.HasIndex(ah => ah.ActionType)
                    .HasDatabaseName("IX_ApprovalHistory_ActionType");
                entity.HasIndex(ah => ah.IsSystemAction)
                    .HasDatabaseName("IX_ApprovalHistory_IsSystemAction");
                entity.HasIndex(ah => ah.IsNotified)
                    .HasDatabaseName("IX_ApprovalHistory_IsNotified");
                entity.HasIndex(ah => ah.RequiresAction)
                    .HasDatabaseName("IX_ApprovalHistory_RequiresAction");

                // Relationships
                entity.HasOne(ah => ah.Submission)
                    .WithMany(uvs => uvs.ApprovalHistories)
                    .HasForeignKey(ah => ah.SubmissionId);

                entity.HasOne(ah => ah.User)
                    .WithMany()
                    .HasForeignKey(ah => ah.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ah => ah.FromStatus)
                    .WithMany()
                    .HasForeignKey(ah => ah.FromStatusId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ah => ah.ToStatus)
                    .WithMany()
                    .HasForeignKey(ah => ah.ToStatusId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        /// <summary>
        /// Configure analytics-related entities
        /// </summary>
        private void ConfigureAnalyticsEntities(ModelBuilder modelBuilder)
        {
            // ExamAnalytic configuration
            modelBuilder.Entity<ExamAnalytic>(entity =>
            {
                entity.ToTable("ExamAnalytics");
                entity.HasKey(ea => ea.AnalyticsId);

                entity.HasOne(ea => ea.UserExam)
                    .WithOne()
                    .HasForeignKey<ExamAnalytic>(ea => ea.UserExamId);

                entity.HasOne(ea => ea.PreviousExam)
                    .WithMany()
                    .HasForeignKey(ea => ea.PreviousExamId);

                entity.HasIndex(ea => ea.UserExamId)
                    .IsUnique()
                    .HasDatabaseName("IX_ExamAnalytic_UserExam");
            });

            // PracticeAnalytic configuration
            modelBuilder.Entity<PracticeAnalytic>(entity =>
            {
                entity.ToTable("PracticeAnalytics");
                entity.HasKey(pa => pa.AnalyticsId);

                entity.HasOne(pa => pa.UserPracticeSet)
                    .WithOne()
                    .HasForeignKey<PracticeAnalytic>(pa => pa.UserPracticeId);

                entity.HasIndex(pa => pa.UserPracticeId)
                    .IsUnique()
                    .HasDatabaseName("IX_PracticeAnalytic_UserPractice");
            });

            // StrengthWeakness configuration
            modelBuilder.Entity<StrengthWeakness>(entity =>
            {
                entity.ToTable("StrengthWeaknesses");
                entity.HasKey(sw => sw.SWId);

                entity.HasOne(sw => sw.User)
                    .WithMany()
                    .HasForeignKey(sw => sw.UserId);

                entity.HasOne(sw => sw.RelatedGoal)
                    .WithMany()
                    .HasForeignKey(sw => sw.RelatedGoalId);

                entity.HasIndex(sw => new { sw.UserId, sw.SkillType, sw.SpecificSkill })
                    .IsUnique()
                    .HasDatabaseName("IX_StrengthWeakness_User_Skill");
            });
        }

        /// <summary>
        /// Configure gamification-related entities
        /// </summary>
        private void ConfigureGamificationEntities(ModelBuilder modelBuilder)
        {
            // Level configuration
            modelBuilder.Entity<Level>(entity =>
            {
                entity.ToTable("Levels");
                entity.HasKey(l => l.LevelId);

                // Create unique index for LevelNumber
                entity.HasIndex(l => l.LevelNumber)
                    .IsUnique()
                    .HasDatabaseName("IX_Level_Number");

                // Relationships
                entity.HasOne(l => l.Badge)
                    .WithMany(b => b.Levels)
                    .HasForeignKey(l => l.BadgeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(l => l.UserLevels)
                    .WithOne(ul => ul.Level)
                    .HasForeignKey(ul => ul.LevelId);
            });

            // UserLevel configuration
            modelBuilder.Entity<UserLevel>(entity =>
            {
                entity.ToTable("UserLevels");
                entity.HasKey(ul => ul.UserLevelId);

                // Create unique index for UserId
                entity.HasIndex(ul => ul.UserId)
                    .IsUnique()
                    .HasDatabaseName("IX_UserLevel_User");

                // Relationships
                entity.HasOne(ul => ul.User)
                    .WithMany()
                    .HasForeignKey(ul => ul.UserId);

                entity.HasOne(ul => ul.Level)
                    .WithMany(l => l.UserLevels)
                    .HasForeignKey(ul => ul.LevelId);
            });

            // PointType configuration
            modelBuilder.Entity<PointType>(entity =>
            {
                entity.ToTable("PointTypes");
                entity.HasKey(pt => pt.PointTypeId);

                // Create unique index for TypeName
                entity.HasIndex(pt => pt.TypeName)
                    .IsUnique()
                    .HasDatabaseName("IX_PointType_Name");

                // Relationships
                entity.HasMany(pt => pt.UserPoints)
                    .WithOne(up => up.PointType)
                    .HasForeignKey(up => up.PointTypeId);
            });

            // UserPoint configuration
            modelBuilder.Entity<UserPoint>(entity =>
            {
                entity.ToTable("UserPoints");
                entity.HasKey(up => up.UserPointId);

                // Create index for UserId and PointTypeId
                entity.HasIndex(up => new { up.UserId, up.PointTypeId })
                    .HasDatabaseName("IX_UserPoint_User_Type");

                // Relationships
                entity.HasOne(up => up.User)
                    .WithMany()
                    .HasForeignKey(up => up.UserId);

                entity.HasOne(up => up.PointType)
                    .WithMany(pt => pt.UserPoints)
                    .HasForeignKey(up => up.PointTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Badge configuration
            modelBuilder.Entity<Badge>(entity =>
            {
                entity.ToTable("Badges");
                entity.HasKey(b => b.BadgeId);

                // Create unique index for BadgeName and index for BadgeCategory
                entity.HasIndex(b => b.BadgeName)
                    .IsUnique()
                    .HasDatabaseName("IX_Badge_Name");

                entity.HasIndex(b => b.BadgeCategory)
                    .HasDatabaseName("IX_Badge_Category");

                // Relationships
                entity.HasMany(b => b.UserBadges)
                    .WithOne(ub => ub.Badge)
                    .HasForeignKey(ub => ub.BadgeId);

                entity.HasMany(b => b.Challenges)
                    .WithOne(c => c.Badge)
                    .HasForeignKey(c => c.BadgeId);

                entity.HasMany(b => b.Achievements)
                    .WithOne(a => a.Badge)
                    .HasForeignKey(a => a.BadgeId);

                entity.HasMany(b => b.Events)
                    .WithOne(e => e.Badge)
                    .HasForeignKey(e => e.BadgeId);

                entity.HasMany(b => b.Levels)
                    .WithOne(l => l.Badge)
                    .HasForeignKey(l => l.BadgeId);
            });

            // UserBadge configuration
            modelBuilder.Entity<UserBadge>(entity =>
            {
                entity.ToTable("UserBadges");
                entity.HasKey(ub => ub.UserBadgeId);

                // Create unique index for UserId and BadgeId
                entity.HasIndex(ub => new { ub.UserId, ub.BadgeId })
                    .IsUnique()
                    .HasDatabaseName("IX_UserBadge_User_Badge");

                // Relationships
                entity.HasOne(ub => ub.User)
                    .WithMany()
                    .HasForeignKey(ub => ub.UserId);

                entity.HasOne(ub => ub.Badge)
                    .WithMany(b => b.UserBadges)
                    .HasForeignKey(ub => ub.BadgeId);
            });

            // Challenge configuration
            modelBuilder.Entity<Challenge>(entity =>
            {
                entity.ToTable("Challenges");
                entity.HasKey(c => c.ChallengeId);

                // Create index for ChallengeName
                entity.HasIndex(c => c.ChallengeName)
                    .HasDatabaseName("IX_Challenge_Name");

                // Relationships
                entity.HasOne(c => c.Badge)
                    .WithMany(b => b.Challenges)
                    .HasForeignKey(c => c.BadgeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Event)
                    .WithMany(e => e.Challenges)
                    .HasForeignKey(c => c.EventId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.PrerequisiteChallenge)
                    .WithMany(c => c.DependentChallenges)
                    .HasForeignKey(c => c.PrerequisiteChallengeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(c => c.ChallengeRequirements)
                    .WithOne(cr => cr.Challenge)
                    .HasForeignKey(cr => cr.ChallengeId);

                entity.HasMany(c => c.UserChallenges)
                    .WithOne(uc => uc.Challenge)
                    .HasForeignKey(uc => uc.ChallengeId);

                entity.HasMany(c => c.DependentChallenges)
                    .WithOne(c => c.PrerequisiteChallenge)
                    .HasForeignKey(c => c.PrerequisiteChallengeId);
            });

            // ChallengeRequirement configuration
            modelBuilder.Entity<ChallengeRequirement>(entity =>
            {
                entity.ToTable("ChallengeRequirements");
                entity.HasKey(cr => cr.RequirementId);

                // Relationships
                entity.HasOne(cr => cr.Challenge)
                    .WithMany(c => c.ChallengeRequirements)
                    .HasForeignKey(cr => cr.ChallengeId);
            });

            // UserChallenge configuration
            modelBuilder.Entity<UserChallenge>(entity =>
            {
                entity.ToTable("UserChallenges");
                entity.HasKey(uc => uc.UserChallengeId);

                // Create unique index for UserId and ChallengeId
                entity.HasIndex(uc => new { uc.UserId, uc.ChallengeId })
                    .IsUnique()
                    .HasDatabaseName("IX_UserChallenge_User_Challenge");

                // Relationships
                entity.HasOne(uc => uc.User)
                    .WithMany()
                    .HasForeignKey(uc => uc.UserId);

                entity.HasOne(uc => uc.Challenge)
                    .WithMany(c => c.UserChallenges)
                    .HasForeignKey(uc => uc.ChallengeId);
            });

            // DailyTask configuration
            modelBuilder.Entity<DailyTask>(entity =>
            {
                entity.ToTable("DailyTasks");
                entity.HasKey(dt => dt.TaskId);

                // Create index for TaskName
                entity.HasIndex(dt => dt.TaskName)
                    .HasDatabaseName("IX_DailyTask_Name");

                // Relationships
                entity.HasOne(dt => dt.Event)
                    .WithMany(e => e.DailyTasks)
                    .HasForeignKey(dt => dt.EventId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(dt => dt.DailyTaskRequirements)
                    .WithOne(dtr => dtr.Task)
                    .HasForeignKey(dtr => dtr.TaskId);

                entity.HasMany(dt => dt.UserDailyTasks)
                    .WithOne(udt => udt.Task)
                    .HasForeignKey(udt => udt.TaskId);
            });

            // DailyTaskRequirement configuration
            modelBuilder.Entity<DailyTaskRequirement>(entity =>
            {
                entity.ToTable("DailyTaskRequirements");
                entity.HasKey(dtr => dtr.RequirementId);

                // Relationships
                entity.HasOne(dtr => dtr.Task)
                    .WithMany(dt => dt.DailyTaskRequirements)
                    .HasForeignKey(dtr => dtr.TaskId);
            });

            // UserDailyTask configuration
            modelBuilder.Entity<UserDailyTask>(entity =>
            {
                entity.ToTable("UserDailyTasks");
                entity.HasKey(udt => udt.UserTaskId);

                // Create unique index for UserId, TaskId and TaskDate
                entity.HasIndex(udt => new { udt.UserId, udt.TaskId, udt.TaskDate })
                    .IsUnique()
                    .HasDatabaseName("IX_UserDailyTask_User_Task_Date");

                // Relationships
                entity.HasOne(udt => udt.User)
                    .WithMany()
                    .HasForeignKey(udt => udt.UserId);

                entity.HasOne(udt => udt.Task)
                    .WithMany(dt => dt.UserDailyTasks)
                    .HasForeignKey(udt => udt.TaskId);
            });

            // Achievement configuration
            modelBuilder.Entity<Achievement>(entity =>
            {
                entity.ToTable("Achievements");
                entity.HasKey(a => a.AchievementId);

                // Create unique index for AchievementName
                entity.HasIndex(a => a.AchievementName)
                    .IsUnique()
                    .HasDatabaseName("IX_Achievement_Name");

                // Relationships
                entity.HasOne(a => a.Badge)
                    .WithMany(b => b.Achievements)
                    .HasForeignKey(a => a.BadgeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.ParentAchievement)
                    .WithMany(a => a.ChildAchievements)
                    .HasForeignKey(a => a.ParentAchievementId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(a => a.AchievementRequirements)
                    .WithOne(ar => ar.Achievement)
                    .HasForeignKey(ar => ar.AchievementId);

                entity.HasMany(a => a.UserAchievements)
                    .WithOne(ua => ua.Achievement)
                    .HasForeignKey(ua => ua.AchievementId);

                entity.HasMany(a => a.ChildAchievements)
                    .WithOne(a => a.ParentAchievement)
                    .HasForeignKey(a => a.ParentAchievementId);
            });

            // AchievementRequirement configuration
            modelBuilder.Entity<AchievementRequirement>(entity =>
            {
                entity.ToTable("AchievementRequirements");
                entity.HasKey(ar => ar.RequirementId);

                // Relationships
                entity.HasOne(ar => ar.Achievement)
                    .WithMany(a => a.AchievementRequirements)
                    .HasForeignKey(ar => ar.AchievementId);
            });

            // UserAchievement configuration
            modelBuilder.Entity<UserAchievement>(entity =>
            {
                entity.ToTable("UserAchievements");
                entity.HasKey(ua => ua.UserAchievementId);

                // Create unique index for UserId and AchievementId
                entity.HasIndex(ua => new { ua.UserId, ua.AchievementId })
                    .IsUnique()
                    .HasDatabaseName("IX_UserAchievement_User_Achievement");

                // Relationships
                entity.HasOne(ua => ua.User)
                    .WithMany()
                    .HasForeignKey(ua => ua.UserId);

                entity.HasOne(ua => ua.Achievement)
                    .WithMany(a => a.UserAchievements)
                    .HasForeignKey(ua => ua.AchievementId);
            });

            // Leaderboard configuration
            modelBuilder.Entity<Leaderboard>(entity =>
            {
                entity.ToTable("Leaderboards");
                entity.HasKey(l => l.LeaderboardId);

                // Create unique index for LeaderboardName
                entity.HasIndex(l => l.LeaderboardName)
                    .IsUnique()
                    .HasDatabaseName("IX_Leaderboard_Name");

                // Relationships
                entity.HasOne(l => l.Event)
                    .WithMany(e => e.Leaderboards)
                    .HasForeignKey(l => l.EventId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(l => l.LeaderboardEntries)
                    .WithOne(le => le.Leaderboard)
                    .HasForeignKey(le => le.LeaderboardId);
            });

            // LeaderboardEntry configuration
            modelBuilder.Entity<LeaderboardEntry>(entity =>
            {
                entity.ToTable("LeaderboardEntries");
                entity.HasKey(le => le.EntryId);

                // Create unique index for LeaderboardId and UserId
                entity.HasIndex(le => new { le.LeaderboardId, le.UserId })
                    .IsUnique()
                    .HasDatabaseName("IX_LeaderboardEntry_Leaderboard_User");

                // Relationships
                entity.HasOne(le => le.Leaderboard)
                    .WithMany(l => l.LeaderboardEntries)
                    .HasForeignKey(le => le.LeaderboardId);

                entity.HasOne(le => le.User)
                    .WithMany()
                    .HasForeignKey(le => le.UserId);
            });

            // Event configuration
            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Events");
                entity.HasKey(e => e.EventId);

                // Create unique index for EventName
                entity.HasIndex(e => e.EventName)
                    .IsUnique()
                    .HasDatabaseName("IX_Event_Name");

                // Relationships
                entity.HasOne(e => e.Badge)
                    .WithMany(b => b.Events)
                    .HasForeignKey(e => e.BadgeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.OrganizerUser)
                    .WithMany()
                    .HasForeignKey(e => e.OrganizerUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.UserEvents)
                    .WithOne(ue => ue.Event)
                    .HasForeignKey(ue => ue.EventId);

                entity.HasMany(e => e.Challenges)
                    .WithOne(c => c.Event)
                    .HasForeignKey(c => c.EventId);

                entity.HasMany(e => e.Leaderboards)
                    .WithOne(l => l.Event)
                    .HasForeignKey(l => l.EventId);

                entity.HasMany(e => e.DailyTasks)
                    .WithOne(dt => dt.Event)
                    .HasForeignKey(dt => dt.EventId);
            });

            // UserEvent configuration
            modelBuilder.Entity<UserEvent>(entity =>
            {
                entity.ToTable("UserEvents");
                entity.HasKey(ue => ue.UserEventId);

                // Create unique index for EventId and UserId
                entity.HasIndex(ue => new { ue.EventId, ue.UserId })
                    .IsUnique()
                    .HasDatabaseName("IX_UserEvent_Event_User");

                // Relationships
                entity.HasOne(ue => ue.Event)
                    .WithMany(e => e.UserEvents)
                    .HasForeignKey(ue => ue.EventId);

                entity.HasOne(ue => ue.User)
                    .WithMany()
                    .HasForeignKey(ue => ue.UserId);
            });

            // UserGift configuration
            modelBuilder.Entity<UserGift>(entity =>
            {
                entity.ToTable("UserGifts");
                entity.HasKey(ug => ug.GiftId);

                // Create index for SenderUserId and ReceiverUserId
                entity.HasIndex(ug => new { ug.SenderUserId, ug.ReceiverUserId })
                    .HasDatabaseName("IX_UserGift_Sender_Receiver");

                // Relationships
                entity.HasOne(ug => ug.SenderUser)
                    .WithMany()
                    .HasForeignKey(ug => ug.SenderUserId);

                entity.HasOne(ug => ug.ReceiverUser)
                    .WithMany()
                    .HasForeignKey(ug => ug.ReceiverUserId);

                entity.HasOne(ug => ug.Event)
                    .WithMany()
                    .HasForeignKey(ug => ug.EventId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ug => ug.Challenge)
                    .WithMany()
                    .HasForeignKey(ug => ug.ChallengeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // UserStreak configuration
            modelBuilder.Entity<UserStreak>(entity =>
            {
                entity.ToTable("UserStreaks");
                entity.HasKey(us => us.StreakId);

                // Create unique index for UserId and StreakType
                entity.HasIndex(us => new { us.UserId, us.StreakType })
                    .IsUnique()
                    .HasDatabaseName("IX_UserStreak_User_Type");

                // Relationships
                entity.HasOne(us => us.User)
                    .WithMany()
                    .HasForeignKey(us => us.UserId);
            });
        }

        /// <summary>
        /// Configure synchronization-related entities
        /// </summary>
        private void ConfigureSyncEntities(ModelBuilder modelBuilder)
        {
            // SyncMetadata configuration
            modelBuilder.Entity<SyncMetadata>(entity =>
            {
                entity.ToTable("SyncMetadata");
                entity.HasKey(sm => sm.Id);

                // Add indexes for frequently queried properties
                entity.HasIndex(sm => sm.LastSyncTime)
                    .HasDatabaseName("IX_SyncMetadata_LastSyncTime");
                entity.HasIndex(sm => sm.SyncStatus)
                    .HasDatabaseName("IX_SyncMetadata_SyncStatus");
                entity.HasIndex(sm => sm.DeviceType)
                    .HasDatabaseName("IX_SyncMetadata_DeviceType");
                entity.HasIndex(sm => sm.SyncMode)
                    .HasDatabaseName("IX_SyncMetadata_SyncMode");
                entity.HasIndex(sm => sm.SyncScope)
                    .HasDatabaseName("IX_SyncMetadata_SyncScope");
                entity.HasIndex(sm => sm.ConnectionType)
                    .HasDatabaseName("IX_SyncMetadata_ConnectionType");
                entity.HasIndex(sm => sm.SyncTrigger)
                    .HasDatabaseName("IX_SyncMetadata_SyncTrigger");
                entity.HasIndex(sm => sm.IsConnected)
                    .HasDatabaseName("IX_SyncMetadata_IsConnected");

                // Create composite unique index for User and Device
                entity.HasIndex(sm => new { sm.UserId, sm.DeviceId })
                    .IsUnique()
                    .HasDatabaseName("IX_SyncMetadata_User_Device");

                // Relationships
                entity.HasOne(sm => sm.User)
                    .WithMany()
                    .HasForeignKey(sm => sm.UserId);
            });

            // SyncConflict configuration
            modelBuilder.Entity<SyncConflict>(entity =>
            {
                entity.ToTable("SyncConflicts");
                entity.HasKey(sc => sc.ConflictId);

                // Add indexes
                entity.HasIndex(sc => sc.DeviceId)
                    .HasDatabaseName("IX_SyncConflict_DeviceId");
                entity.HasIndex(sc => sc.EntityType)
                    .HasDatabaseName("IX_SyncConflict_EntityType");
                entity.HasIndex(sc => sc.ConflictType)
                    .HasDatabaseName("IX_SyncConflict_ConflictType");
                entity.HasIndex(sc => sc.ConflictStatus)
                    .HasDatabaseName("IX_SyncConflict_Status");
                entity.HasIndex(sc => sc.ResolutionStrategy)
                    .HasDatabaseName("IX_SyncConflict_ResolutionStrategy");
                entity.HasIndex(sc => sc.ResolvedAt)
                    .HasDatabaseName("IX_SyncConflict_ResolvedAt");
                entity.HasIndex(sc => sc.ConflictSeverity)
                    .HasDatabaseName("IX_SyncConflict_ConflictSeverity");
                entity.HasIndex(sc => sc.ConflictContext)
                    .HasDatabaseName("IX_SyncConflict_ConflictContext");
                entity.HasIndex(sc => sc.IsUserNotified)
                    .HasDatabaseName("IX_SyncConflict_IsUserNotified");
                entity.HasIndex(sc => sc.ResolutionAttempts)
                    .HasDatabaseName("IX_SyncConflict_ResolutionAttempts");

                // Create composite indexes
                entity.HasIndex(sc => new { sc.UserId, sc.EntityType, sc.EntityId })
                    .HasDatabaseName("IX_SyncConflict_User_Entity");
                entity.HasIndex(sc => new { sc.ClientModifiedAt, sc.ServerModifiedAt })
                    .HasDatabaseName("IX_SyncConflict_ModifiedAt");

                // Relationships
                entity.HasOne(sc => sc.User)
                    .WithMany()
                    .HasForeignKey(sc => sc.UserId);

                entity.HasOne(sc => sc.ResolvedByUser)
                    .WithMany()
                    .HasForeignKey(sc => sc.ResolvedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // DeletedItem configuration
            modelBuilder.Entity<DeletedItem>(entity =>
            {
                entity.ToTable("DeletedItems");
                entity.HasKey(di => di.Id);

                // Add indexes
                entity.HasIndex(di => di.DeletedAt)
                    .HasDatabaseName("IX_DeletedItem_DeletedAt");
                entity.HasIndex(di => di.EntityType)
                    .HasDatabaseName("IX_DeletedItem_EntityType");
                entity.HasIndex(di => di.UserID)
                    .HasDatabaseName("IX_DeletedItem_UserID");
                entity.HasIndex(di => di.DeletionContext)
                    .HasDatabaseName("IX_DeletedItem_DeletionContext");
                entity.HasIndex(di => di.DeviceId)
                    .HasDatabaseName("IX_DeletedItem_DeviceId");
                entity.HasIndex(di => di.IsPermanentDeletion)
                    .HasDatabaseName("IX_DeletedItem_IsPermanentDeletion");
                entity.HasIndex(di => di.RetentionExpiry)
                    .HasDatabaseName("IX_DeletedItem_RetentionExpiry");
                entity.HasIndex(di => di.IsArchived)
                    .HasDatabaseName("IX_DeletedItem_IsArchived");
                entity.HasIndex(di => di.DeletedItemType)
                    .HasDatabaseName("IX_DeletedItem_DeletedItemType");
                entity.HasIndex(di => di.Priority)
                    .HasDatabaseName("IX_DeletedItem_Priority");
                entity.HasIndex(di => di.IsSyncedToClients)
                    .HasDatabaseName("IX_DeletedItem_IsSyncedToClients");
                entity.HasIndex(di => di.SyncedAt)
                    .HasDatabaseName("IX_DeletedItem_SyncedAt");
                entity.HasIndex(di => di.IsRestored)
                    .HasDatabaseName("IX_DeletedItem_IsRestored");
                entity.HasIndex(di => di.RestoredAt)
                    .HasDatabaseName("IX_DeletedItem_RestoredAt");

                // Create composite unique index for Entity
                entity.HasIndex(di => new { di.EntityType, di.EntityId })
                    .IsUnique()
                    .HasDatabaseName("IX_DeletedItem_Entity");

                // Relationships
                entity.HasOne(di => di.User)
                    .WithMany()
                    .HasForeignKey(di => di.UserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(di => di.RestoredByUser)
                    .WithMany()
                    .HasForeignKey(di => di.RestoredBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        /// <summary>
        /// Override SaveChanges to automatically update audit fields
        /// </summary>
        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        /// <summary>
        /// Override SaveChangesAsync to automatically update audit fields
        /// </summary>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Update audit fields for entities being added or modified
        /// </summary>
        private void UpdateAuditFields()
        {
            var now = DateTime.UtcNow;

            // Get all added/modified entities
            var entries = ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified) &&
                            e.Entity is BaseEntity);

            foreach (var entry in entries)
            {
                if (entry.Entity is BaseEntity entity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedAt = now;
                    }

                    entity.UpdatedAt = now;
                }

                // Handle AuditableEntity properties
                if (entry.Entity is AuditableEntity auditableEntity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        if (_currentUserId.HasValue)
                        {
                            auditableEntity.CreatedBy = _currentUserId.Value;
                        }
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        if (_currentUserId.HasValue)
                        {
                            auditableEntity.ModifiedBy = _currentUserId.Value;
                        }

                        // Prevent changing of CreatedBy
                        entry.Property("CreatedBy").IsModified = false;
                        entry.Property("CreatedAt").IsModified = false;
                    }
                }

                // Handle SoftDeletableEntity properties
                if (entry.Entity is ISoftDeletable softDeletable &&
                    entry.Property("IsDeleted").IsModified &&
                    softDeletable.IsDeleted)
                {
                    softDeletable.DeletedAt = now;

                    // Set DeletedBy if entity has this property and current user ID is available
                    if (entry.Entity is SoftDeletableEntity softDeletableEntity && _currentUserId.HasValue)
                    {
                        softDeletableEntity.DeletedBy = _currentUserId.Value;
                    }
                }
            }
        }
    }
}
