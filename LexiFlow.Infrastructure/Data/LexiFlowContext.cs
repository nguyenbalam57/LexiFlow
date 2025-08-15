using LexiFlow.Models.Core;
using LexiFlow.Models.Exam;
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
    /// Main database context for LexiFlow application - Optimized for .NET 9
    /// </summary>
    public class LexiFlowContext : DbContext
    {
        private readonly ILogger<LexiFlowContext> _logger;
        private int? _currentUserId;

        public LexiFlowContext(DbContextOptions<LexiFlowContext> options, ILogger<LexiFlowContext> logger)
            : base(options)
        {
            _logger = logger;
        }

        public void SetCurrentUserId(int userId)
        {
            _currentUserId = userId;
        }

        #region DbSets
        // User Management
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

        // Learning Content
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

        // Media
        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<MediaCategory> MediaCategories { get; set; }
        public DbSet<MediaProcessingHistory> MediaProcessingHistories { get; set; }

        // Progress Tracking
        public DbSet<LearningProgress> LearningProgresses { get; set; }
        public DbSet<LearningSession> LearningSessions { get; set; }
        public DbSet<LearningSessionDetail> SessionDetails { get; set; }
        public DbSet<UserKanjiProgress> UserKanjiProgresses { get; set; }
        public DbSet<UserGrammarProgress> UserGrammarProgresses { get; set; }
        public DbSet<GoalProgress> GoalProgresses { get; set; }
        public DbSet<PersonalWordList> PersonalWordLists { get; set; }
        public DbSet<PersonalWordListItem> PersonalWordListItems { get; set; }

        // Planning
        public DbSet<StudyGoal> StudyGoals { get; set; }
        public DbSet<StudyPlan> StudyPlans { get; set; }
        public DbSet<StudySession> StudySessions { get; set; }
        public DbSet<StudyTask> StudyTasks { get; set; }
        public DbSet<StudyTopic> StudyTopics { get; set; }
        public DbSet<StudyPlanItem> StudyPlanItems { get; set; }
        public DbSet<StudyPlanProgress> StudyPlanProgresses { get; set; }
        public DbSet<TaskCompletion> TaskCompletions { get; set; }

        // Practice and Exam
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<TestDetail> TestDetails { get; set; }
        public DbSet<JLPTExam> JLPTExams { get; set; }
        public DbSet<JLPTLevel> JLPTLevels { get; set; }
        public DbSet<JLPTSection> JLPTSections { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<UserExam> UserExams { get; set; }

        // Notification
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<NotificationPriority> NotificationPriorities { get; set; }
        public DbSet<NotificationStatus> NotificationStatuses { get; set; }
        public DbSet<NotificationRecipient> NotificationRecipients { get; set; }
        public DbSet<NotificationResponse> NotificationResponses { get; set; }

        // Scheduling
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleItem> ScheduleItems { get; set; }
        public DbSet<ScheduleItemType> ScheduleItemTypes { get; set; }
        public DbSet<ScheduleRecurrence> ScheduleRecurrences { get; set; }
        public DbSet<ScheduleItemParticipant> ScheduleItemParticipants { get; set; }
        public DbSet<ScheduleReminder> ScheduleReminders { get; set; }

        // System
        public DbSet<Setting> Settings { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // DISABLE ALL CASCADES GLOBALLY TO AVOID CONFLICTS
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }

            ConfigureCoreEntities(modelBuilder);
            ConfigureQueryFilters(modelBuilder);
            ConfigureUserEntities(modelBuilder);
            ConfigureVocabularyEntities(modelBuilder);
            ConfigureGrammarEntities(modelBuilder);
            ConfigureMediaEntities(modelBuilder);
            ConfigureExamEntities(modelBuilder);
            ConfigureProgressEntities(modelBuilder);
            ConfigurePlanningEntities(modelBuilder);
            ConfigureTechnicalTermEntities(modelBuilder);
            ConfigureNotificationEntities(modelBuilder);
            ConfigureSchedulingEntities(modelBuilder);
            ConfigureSystemEntities(modelBuilder);

            // Configure performance indexes for .NET 9 optimization
            ConfigurePerformanceIndexes(modelBuilder);

            // OVERRIDE ESSENTIAL CASCADES ONLY WHERE NEEDED
            ConfigureEssentialCascades(modelBuilder);
        }

        private void ConfigureEssentialCascades(ModelBuilder modelBuilder)
        {
            // Only keep essential parent-child cascades
            
            // Notification -> NotificationRecipient (essential)
            modelBuilder.Entity<NotificationRecipient>()
                .HasOne(nr => nr.Notification)
                .WithMany(n => n.NotificationRecipients)
                .HasForeignKey(nr => nr.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);

            // NotificationRecipient -> NotificationResponse (essential)
            modelBuilder.Entity<NotificationResponse>()
                .HasOne(nr => nr.Recipient)
                .WithMany(nrec => nrec.NotificationResponses)
                .HasForeignKey(nr => nr.RecipientId)
                .OnDelete(DeleteBehavior.Cascade);

            // StudyPlan -> StudyGoal (essential)
            modelBuilder.Entity<StudyGoal>()
                .HasOne(sg => sg.StudyPlan)
                .WithMany(sp => sp.StudyGoals)
                .HasForeignKey(sg => sg.PlanId)
                .OnDelete(DeleteBehavior.Cascade);

            // Schedule -> ScheduleItem (essential)
            modelBuilder.Entity<ScheduleItem>()
                .HasOne(si => si.Schedule)
                .WithMany()
                .HasForeignKey(si => si.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade);

            // ScheduleItem -> ScheduleItemParticipant (essential)
            modelBuilder.Entity<ScheduleItemParticipant>()
                .HasOne(sip => sip.Item)
                .WithMany()
                .HasForeignKey(sip => sip.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // ScheduleItem -> ScheduleReminder (essential)
            modelBuilder.Entity<ScheduleReminder>()
                .HasOne(sr => sr.Item)
                .WithMany()
                .HasForeignKey(sr => sr.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureCoreEntities(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(BaseEntity).IsAssignableFrom(e.ClrType)))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property("RowVersion")
                    .IsRowVersion()
                    .IsConcurrencyToken();

                modelBuilder.Entity(entityType.ClrType)
                    .Property("CreatedAt")
                    .HasDefaultValueSql("GETUTCDATE()");

                modelBuilder.Entity(entityType.ClrType)
                    .Property("UpdatedAt")
                    .HasDefaultValueSql("GETUTCDATE()");
            }

            // Configure AuditableEntity navigation properties
            ConfigureAuditableEntities(modelBuilder);

            // Configure SoftDeletableEntity navigation properties
            ConfigureSoftDeletableEntities(modelBuilder);
        }

        private void ConfigureAuditableEntities(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(AuditableEntity).IsAssignableFrom(e.ClrType)))
            {
                // Configure CreatedByUser navigation property
                modelBuilder.Entity(entityType.ClrType)
                    .HasOne("LexiFlow.Models.User.User", "CreatedByUser")
                    .WithMany()
                    .HasForeignKey("CreatedBy")
                    .OnDelete(DeleteBehavior.NoAction) // CHANGED: Was Restrict
                    .IsRequired();

                // Configure ModifiedByUser navigation property
                modelBuilder.Entity(entityType.ClrType)
                    .HasOne("LexiFlow.Models.User.User", "ModifiedByUser")
                    .WithMany()
                    .HasForeignKey("ModifiedBy")
                    .OnDelete(DeleteBehavior.NoAction) // CHANGED: Was Restrict
                    .IsRequired(false);
            }
        }

        private void ConfigureSoftDeletableEntities(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(ISoftDeletable).IsAssignableFrom(e.ClrType) &&
                           e.ClrType.GetProperty("DeletedBy") != null))
            {
                // Configure DeletedByUser navigation property for entities that have DeletedBy property
                modelBuilder.Entity(entityType.ClrType)
                    .HasOne("LexiFlow.Models.User.User", "DeletedByUser")
                    .WithMany()
                    .HasForeignKey("DeletedBy")
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired(false);
            }
        }

        private void ConfigureQueryFilters(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, "IsDeleted");
                    var condition = Expression.Equal(property, Expression.Constant(false));
                    var lambda = Expression.Lambda(condition, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }

        private void ConfigureUserEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(u => u.Profile)
                    .WithOne(p => p.User)
                    .HasForeignKey<UserProfile>(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(u => u.LearningPreference)
                    .WithOne(lp => lp.User)
                    .HasForeignKey<UserLearningPreference>(lp => lp.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(u => u.NotificationSetting)
                    .WithOne(ns => ns.User)
                    .HasForeignKey<UserNotificationSetting>(ns => ns.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Department entity and its relationships
            modelBuilder.Entity<Department>(entity =>
            {
                // Configure Manager relationship
                entity.HasOne(d => d.Manager)
                    .WithMany()
                    .HasForeignKey(d => d.ManagerUserId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Configure ParentDepartment relationship
                entity.HasOne(d => d.ParentDepartment)
                    .WithMany(d => d.ChildDepartments)
                    .HasForeignKey(d => d.ParentDepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Configure Users relationship
                entity.HasMany(d => d.Users)
                    .WithOne(u => u.Department)
                    .HasForeignKey(u => u.DepartmentId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Configure Teams relationship
                entity.HasMany(d => d.Teams)
                    .WithOne(t => t.Department)
                    .HasForeignKey(t => t.DepartmentId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Team entity relationships
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasOne(t => t.Department)
                    .WithMany(d => d.Teams)
                    .HasForeignKey(t => t.DepartmentId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure UserProfile relationships
            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasOne(up => up.Department)
                    .WithMany()
                    .HasForeignKey(up => up.DepartmentId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure UserRole relationships
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ur => ur.Role)
                    .WithMany()
                    .HasForeignKey(ur => ur.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure UserPermission relationships
            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.HasKey(up => new { up.UserId, up.PermissionId });

                entity.HasOne(up => up.User)
                    .WithMany()
                    .HasForeignKey(up => up.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(up => up.Permission)
                    .WithMany()
                    .HasForeignKey(up => up.PermissionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure RolePermission relationships
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(rp => new { rp.RoleId, rp.PermissionId });

                entity.HasOne(rp => rp.Role)
                    .WithMany()
                    .HasForeignKey(rp => rp.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(rp => rp.Permission)
                    .WithMany()
                    .HasForeignKey(rp => rp.PermissionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure UserTeam relationships
            modelBuilder.Entity<UserTeam>(entity =>
            {
                entity.HasKey(ut => new { ut.UserId, ut.TeamId });

                entity.HasOne(ut => ut.User)
                    .WithMany()
                    .HasForeignKey(ut => ut.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ut => ut.Team)
                    .WithMany()
                    .HasForeignKey(ut => ut.TeamId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure UserGroup relationships
            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.HasKey(ug => new { ug.UserId, ug.GroupId });

                entity.HasOne(ug => ug.User)
                    .WithMany()
                    .HasForeignKey(ug => ug.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ug => ug.Group)
                    .WithMany()
                    .HasForeignKey(ug => ug.GroupId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure GroupPermission relationships
            modelBuilder.Entity<GroupPermission>(entity =>
            {
                entity.HasKey(gp => new { gp.GroupId, gp.PermissionId });

                entity.HasOne(gp => gp.Group)
                    .WithMany()
                    .HasForeignKey(gp => gp.GroupId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(gp => gp.Permission)
                    .WithMany()
                    .HasForeignKey(gp => gp.PermissionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure PermissionGroupMapping relationships
            modelBuilder.Entity<PermissionGroupMapping>(entity =>
            {
                entity.HasKey(pgm => new { pgm.PermissionGroupId, pgm.PermissionId });

                entity.HasOne(pgm => pgm.Permission)
                    .WithMany()
                    .HasForeignKey(pgm => pgm.PermissionId)
                    .OnDelete(DeleteBehavior.NoAction); // FIXED: Changed from Cascade to NoAction

                entity.HasOne(pgm => pgm.PermissionGroup)
                    .WithMany()
                    .HasForeignKey(pgm => pgm.PermissionGroupId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Configure CreatedByUser relationship
                entity.HasOne(pgm => pgm.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(pgm => pgm.CreatedByUserId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Configure DependsOnPermission relationship
                entity.HasOne(pgm => pgm.DependsOnPermission)
                    .WithMany()
                    .HasForeignKey(pgm => pgm.DependsOnPermissionId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }

        private void ConfigureVocabularyEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vocabulary>(entity =>
            {
                entity.HasQueryFilter(v => !v.IsDeleted);

                entity.HasOne(v => v.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(v => v.DeletedBy)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }

        private void ConfigureGrammarEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Grammar>(entity =>
            {
                entity.HasOne(g => g.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(g => g.DeletedBy)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }

        private void ConfigureMediaEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MediaFile>(entity =>
            {
                entity.HasOne(m => m.User)
                    .WithMany()
                    .HasForeignKey(m => m.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(m => m.Vocabulary)
                    .WithMany(v => v.MediaFiles)
                    .HasForeignKey(m => m.VocabularyId)
                    .OnDelete(DeleteBehavior.NoAction);

                // FIX: Thêm c?u hình cho DeletedByUser navigation property
                entity.HasOne(m => m.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(m => m.DeletedBy)
                    .OnDelete(DeleteBehavior.NoAction);

                // FIX: C?u hình cho Category
                entity.HasOne(m => m.Category)
                    .WithMany()
                    .HasForeignKey(m => m.CategoryId)
                    .OnDelete(DeleteBehavior.NoAction);

                // FIX: C?u hình cho Kanji
                entity.HasOne(m => m.Kanji)
                    .WithMany(k => k.MediaFiles)
                    .HasForeignKey(m => m.KanjiId)
                    .OnDelete(DeleteBehavior.NoAction);

                // FIX: C?u hình cho Grammar
                entity.HasOne(m => m.Grammar)
                    .WithMany()
                    .HasForeignKey(m => m.GrammarId)
                    .OnDelete(DeleteBehavior.NoAction);

                // FIX: C?u hình cho TechnicalTerm
                entity.HasOne(m => m.TechnicalTerm)
                    .WithMany(t => t.MediaFiles)
                    .HasForeignKey(m => m.TechnicalTermId)
                    .OnDelete(DeleteBehavior.NoAction);

                // FIX: C?u hình cho các entities khác - ALL CHANGED TO NoAction
                entity.HasOne(m => m.Example)
                    .WithMany()
                    .HasForeignKey(m => m.ExampleId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(m => m.Question)
                    .WithMany()
                    .HasForeignKey(m => m.QuestionId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(m => m.QuestionOption)
                    .WithMany()
                    .HasForeignKey(m => m.QuestionOptionId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(m => m.GrammarExample)
                    .WithMany()
                    .HasForeignKey(m => m.GrammarExampleId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(m => m.KanjiExample)
                    .WithMany()
                    .HasForeignKey(m => m.KanjiExampleId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(m => m.TermExample)
                    .WithMany()
                    .HasForeignKey(m => m.TermExampleId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }

        private void ConfigureExamEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasOne(q => q.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(q => q.CreatedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<UserExam>(entity =>
            {
                entity.HasOne(ue => ue.User)
                    .WithMany()
                    .HasForeignKey(ue => ue.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }

        private void ConfigureProgressEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LearningProgress>(entity =>
            {
                entity.HasOne(lp => lp.User)
                    .WithMany()
                    .HasForeignKey(lp => lp.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(lp => lp.Vocabulary)
                    .WithMany()
                    .HasForeignKey(lp => lp.VocabularyId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<UserKanjiProgress>(entity =>
            {
                entity.HasOne(ukp => ukp.User)
                    .WithMany()
                    .HasForeignKey(ukp => ukp.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(ukp => ukp.Kanji)
                    .WithMany(k => k.UserProgresses)
                    .HasForeignKey(ukp => ukp.KanjiId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }

        private void ConfigurePlanningEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudyPlan>(entity =>
            {
                entity.HasOne(sp => sp.User)
                    .WithMany()
                    .HasForeignKey(sp => sp.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(sp => sp.StudyGoals)
                    .WithOne(sg => sg.StudyPlan)
                    .HasForeignKey(sg => sg.PlanId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TaskCompletion>(entity =>
            {
                entity.HasOne(tc => tc.User)
                    .WithMany()
                    .HasForeignKey(tc => tc.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }

        private void ConfigureTechnicalTermEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TechnicalTerm>(entity =>
            {
                entity.HasOne(t => t.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(t => t.DeletedBy)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(t => t.Category)
                    .WithMany()
                    .HasForeignKey(t => t.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<TermRelation>(entity =>
            {
                entity.HasOne(tr => tr.Term1)
                    .WithMany()
                    .HasForeignKey(tr => tr.TermId1)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(tr => tr.Term2)
                    .WithMany()
                    .HasForeignKey(tr => tr.TermId2)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(tr => tr.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(tr => tr.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(tr => tr.UpdatedByUser)
                    .WithMany()
                    .HasForeignKey(tr => tr.UpdatedBy)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(tr => tr.VerifiedByUser)
                    .WithMany()
                    .HasForeignKey(tr => tr.VerifiedBy)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<UserTechnicalTerm>(entity =>
            {
                entity.HasOne(utt => utt.User)
                    .WithMany()
                    .HasForeignKey(utt => utt.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(utt => utt.Term)
                    .WithMany(t => t.UserTechnicalTerms)
                    .HasForeignKey(utt => utt.TermId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureNotificationEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasOne(n => n.Type)
                    .WithMany()
                    .HasForeignKey(n => n.TypeId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(n => n.Priority)
                    .WithMany()
                    .HasForeignKey(n => n.PriorityId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(n => n.SenderUser)
                    .WithMany()
                    .HasForeignKey(n => n.SenderUserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(n => n.NotificationRecipients)
                    .WithOne(nr => nr.Notification)
                    .HasForeignKey(nr => nr.NotificationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<NotificationRecipient>(entity =>
            {
                entity.HasOne(nr => nr.Notification)
            .WithMany(n => n.NotificationRecipients)
            .HasForeignKey(nr => nr.NotificationId)
            .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(nr => nr.User)
                    .WithMany()
                    .HasForeignKey(nr => nr.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(nr => nr.Group)
                    .WithMany()
                    .HasForeignKey(nr => nr.GroupId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(nr => nr.Status)
                    .WithMany()
                    .HasForeignKey(nr => nr.StatusId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Configure the collection relationship
                entity.HasMany(nr => nr.NotificationResponses)
                    .WithOne(nresp => nresp.Recipient)
                    .HasForeignKey(nresp => nresp.RecipientId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<NotificationResponse>(entity =>
            {
                entity.HasOne(nr => nr.Recipient)
            .WithMany(nrec => nrec.NotificationResponses)
            .HasForeignKey(nr => nr.RecipientId)
            .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(nr => nr.RespondedByUser)
                    .WithMany()
                    .HasForeignKey(nr => nr.RespondedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }

        private void ConfigureSchedulingEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasOne(s => s.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(s => s.CreatedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<ScheduleItem>(entity =>
            {
                entity.HasOne(si => si.Schedule)
                    .WithMany()
                    .HasForeignKey(si => si.ScheduleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(si => si.Type)
                    .WithMany()
                    .HasForeignKey(si => si.TypeId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(si => si.StudyTask)
                    .WithMany()
                    .HasForeignKey(si => si.StudyTaskId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(si => si.Recurrence)
                    .WithMany()
                    .HasForeignKey(si => si.RecurrenceId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<ScheduleItemParticipant>(entity =>
            {
                // Remove composite key since the model has its own ParticipantId primary key
                // entity.HasKey(sip => new { sip.ItemId, sip.UserId });

                entity.HasOne(sip => sip.Item)
                    .WithMany()
                    .HasForeignKey(sip => sip.ItemId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(sip => sip.User)
                    .WithMany()
                    .HasForeignKey(sip => sip.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(sip => sip.Group)
                    .WithMany()
                    .HasForeignKey(sip => sip.GroupId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<ScheduleReminder>(entity =>
            {
                entity.HasOne(sr => sr.Item)
                    .WithMany()
                    .HasForeignKey(sr => sr.ItemId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(sr => sr.User)
                    .WithMany()
                    .HasForeignKey(sr => sr.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }

        private void ConfigureSystemEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasIndex(s => s.SettingKey)
                    .IsUnique();
            });
        }

        private void ConfigurePerformanceIndexes(ModelBuilder modelBuilder)
        {
            // ?? High Priority Performance Indexes

            // User entity indexes - Authentication & Authorization
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Username)
                    .IsUnique()
                    .HasDatabaseName("IX_Users_Username");

                entity.HasIndex(u => u.Email)
                    .HasDatabaseName("IX_Users_Email");

                entity.HasIndex(u => new { u.IsActive, u.LastLoginAt })
                    .HasDatabaseName("IX_Users_Active_LastLogin");

                entity.HasIndex(u => u.DepartmentId)
                    .HasDatabaseName("IX_Users_Department");

                entity.HasIndex(u => new { u.IsActive, u.CreatedAt })
                    .HasDatabaseName("IX_Users_Active_Created");
            });

            // Vocabulary entity indexes - Core Learning Content
            modelBuilder.Entity<Vocabulary>(entity =>
            {
                entity.HasIndex(v => v.Term)
                    .HasDatabaseName("IX_Vocabularies_Term");

                entity.HasIndex(v => v.Reading)
                    .HasDatabaseName("IX_Vocabularies_Reading");

                entity.HasIndex(v => new { v.Level, v.IsActive })
                    .HasDatabaseName("IX_Vocabularies_Level_Active");

                entity.HasIndex(v => new { v.CategoryId, v.IsActive })
                    .HasDatabaseName("IX_Vocabularies_Category_Active");

                entity.HasIndex(v => v.FrequencyRank)
                    .HasDatabaseName("IX_Vocabularies_FrequencyRank");

                entity.HasIndex(v => v.PopularityScore)
                    .HasDatabaseName("IX_Vocabularies_Popularity");

                entity.HasIndex(v => v.DifficultyLevel)
                    .HasDatabaseName("IX_Vocabularies_Difficulty");

                entity.HasIndex(v => new { v.IsCommon, v.Level })
                    .HasDatabaseName("IX_Vocabularies_Common_Level");

                entity.HasIndex(v => new { v.PartOfSpeech, v.Level })
                    .HasDatabaseName("IX_Vocabularies_PartOfSpeech_Level");

                entity.HasIndex(v => new { v.IsActive, v.IsDeleted, v.Level })
                    .HasDatabaseName("IX_Vocabularies_Active_Deleted_Level");
            });

            // Kanji entity indexes - Core Learning Content
            modelBuilder.Entity<Kanji>(entity =>
            {
                entity.HasIndex(k => k.Character)
                    .IsUnique()
                    .HasDatabaseName("IX_Kanjis_Character");

                entity.HasIndex(k => new { k.JLPTLevel, k.IsActive })
                    .HasDatabaseName("IX_Kanjis_JLPTLevel_Active");

                entity.HasIndex(k => k.StrokeCount)
                    .HasDatabaseName("IX_Kanjis_StrokeCount");

                entity.HasIndex(k => k.FrequencyRank)
                    .HasDatabaseName("IX_Kanjis_FrequencyRank");

                entity.HasIndex(k => k.PopularityScore)
                    .HasDatabaseName("IX_Kanjis_Popularity");

                entity.HasIndex(k => new { k.Grade, k.JLPTLevel })
                    .HasDatabaseName("IX_Kanjis_Grade_JLPT");

                entity.HasIndex(k => new { k.IsActive, k.JLPTLevel, k.StrokeCount })
                    .HasDatabaseName("IX_Kanjis_Active_JLPT_Stroke");
            });

            // Grammar entity indexes - FIXED: Removed non-existent properties
            modelBuilder.Entity<Grammar>(entity =>
            {
                entity.HasIndex(g => g.Pattern)
                    .HasDatabaseName("IX_Grammars_Pattern");

                entity.HasIndex(g => new { g.Level, g.IsActive })
                    .HasDatabaseName("IX_Grammars_Level_Active");

                entity.HasIndex(g => new { g.GrammarType, g.Level })
                    .HasDatabaseName("IX_Grammars_Type_Level");
            });

            // Learning Progress indexes - Critical for performance
            modelBuilder.Entity<LearningProgress>(entity =>
            {
                entity.HasIndex(lp => new { lp.UserId, lp.VocabularyId })
                    .IsUnique()
                    .HasDatabaseName("IX_LearningProgresses_User_Vocabulary");

                entity.HasIndex(lp => new { lp.UserId, lp.NextReviewDate })
                    .HasDatabaseName("IX_LearningProgresses_User_NextReview");

                entity.HasIndex(lp => lp.MasteryLevel)
                    .HasDatabaseName("IX_LearningProgresses_Mastery");

                entity.HasIndex(lp => new { lp.UserId, lp.MasteryLevel, lp.NextReviewDate })
                    .HasDatabaseName("IX_LearningProgresses_User_Mastery_NextReview");

                entity.HasIndex(lp => new { lp.CreatedAt, lp.MasteryLevel })
                    .HasDatabaseName("IX_LearningProgresses_Created_Mastery");
            });

            // User Kanji Progress indexes - FIXED: Removed MasteryLevel
            modelBuilder.Entity<UserKanjiProgress>(entity =>
            {
                entity.HasIndex(ukp => new { ukp.UserId, ukp.KanjiId })
                    .IsUnique()
                    .HasDatabaseName("IX_UserKanjiProgresses_User_Kanji");

                entity.HasIndex(ukp => new { ukp.UserId, ukp.NextReviewDate })
                    .HasDatabaseName("IX_UserKanjiProgresses_User_NextReview");

                entity.HasIndex(ukp => new { ukp.UserId, ukp.RecognitionLevel })
                    .HasDatabaseName("IX_UserKanjiProgresses_User_Recognition");
            });

            // Study Plan indexes - FIXED: Removed Status property
            modelBuilder.Entity<StudyPlan>(entity =>
            {
                entity.HasIndex(sp => new { sp.UserId, sp.IsActive })
                    .HasDatabaseName("IX_StudyPlans_User_Active");

                entity.HasIndex(sp => new { sp.StartDate, sp.TargetDate })
                    .HasDatabaseName("IX_StudyPlans_DateRange");

                entity.HasIndex(sp => new { sp.UserId, sp.IsActive })
                    .HasDatabaseName("IX_StudyPlans_User_Active2");
            });

            // Study Goal indexes
            modelBuilder.Entity<StudyGoal>(entity =>
            {
                entity.HasIndex(sg => new { sg.PlanId, sg.IsCompleted })
                    .HasDatabaseName("IX_StudyGoals_Plan_Completed");

                entity.HasIndex(sg => new { sg.TargetDate, sg.IsCompleted })
                    .HasDatabaseName("IX_StudyGoals_TargetDate_Completed");

                entity.HasIndex(sg => new { sg.Status, sg.Priority })
                    .HasDatabaseName("IX_StudyGoals_Status_Priority");
            });

            // Task Completion indexes - FIXED: Removed Status property
            modelBuilder.Entity<TaskCompletion>(entity =>
            {
                entity.HasIndex(tc => new { tc.UserId, tc.CompletionDate })
                    .HasDatabaseName("IX_TaskCompletions_User_Date");

                entity.HasIndex(tc => new { tc.TaskId, tc.CompletionDate })
                    .HasDatabaseName("IX_TaskCompletions_Task_Date");

                entity.HasIndex(tc => new { tc.UserId, tc.CompletionPercentage })
                    .HasDatabaseName("IX_TaskCompletions_User_Percentage");
            });

            // Test Results indexes - Exam Performance
            modelBuilder.Entity<TestResult>(entity =>
            {
                entity.HasIndex(tr => new { tr.UserId, tr.TestDate })
                    .HasDatabaseName("IX_TestResults_User_Date");

                entity.HasIndex(tr => tr.TestType)
                    .HasDatabaseName("IX_TestResults_Type");

                entity.HasIndex(tr => tr.Score)
                    .HasDatabaseName("IX_TestResults_Score");

                entity.HasIndex(tr => new { tr.UserId, tr.TestType, tr.TestDate })
                    .HasDatabaseName("IX_TestResults_User_Type_Date");
            });

            // Question indexes
            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasIndex(q => new { q.QuestionType, q.Difficulty })
                    .HasDatabaseName("IX_Questions_Type_Difficulty");

                entity.HasIndex(q => q.SectionId)
                    .HasDatabaseName("IX_Questions_Section");

                entity.HasIndex(q => new { q.IsActive, q.QuestionType })
                    .HasDatabaseName("IX_Questions_Active_Type");
            });

            // User Exam indexes
            modelBuilder.Entity<UserExam>(entity =>
            {
                entity.HasIndex(ue => new { ue.UserId, ue.StartTime })
                    .HasDatabaseName("IX_UserExams_User_StartTime");

                entity.HasIndex(ue => ue.Status)
                    .HasDatabaseName("IX_UserExams_Status");

                entity.HasIndex(ue => new { ue.UserId, ue.Status })
                    .HasDatabaseName("IX_UserExams_User_Status");
            });

            // Learning Session indexes
            modelBuilder.Entity<LearningSession>(entity =>
            {
                entity.HasIndex(ls => new { ls.UserId, ls.StartTime })
                    .HasDatabaseName("IX_LearningSessions_User_StartTime");

                entity.HasIndex(ls => ls.SessionType)
                    .HasDatabaseName("IX_LearningSessions_Type");

                entity.HasIndex(ls => new { ls.UserId, ls.SessionType, ls.StartTime })
                    .HasDatabaseName("IX_LearningSessions_User_Type_Start");
            });

            // Media File indexes
            modelBuilder.Entity<MediaFile>(entity =>
            {
                entity.HasIndex(m => new { m.MediaType, m.IsPrimary })
                    .HasDatabaseName("IX_MediaFiles_Type_Primary");

                entity.HasIndex(m => m.VocabularyId)
                    .HasDatabaseName("IX_MediaFiles_Vocabulary");

                entity.HasIndex(m => m.KanjiId)
                    .HasDatabaseName("IX_MediaFiles_Kanji");

                entity.HasIndex(m => m.UserId)
                    .HasDatabaseName("IX_MediaFiles_User");

                entity.HasIndex(m => new { m.IsPublic, m.MediaType })
                    .HasDatabaseName("IX_MediaFiles_Public_Type");
            });

            // Category indexes
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(c => c.CategoryName)
                    .HasDatabaseName("IX_Categories_Name");

                entity.HasIndex(c => new { c.ParentCategoryId, c.IsActive })
                    .HasDatabaseName("IX_Categories_Parent_Active");

                entity.HasIndex(c => new { c.CategoryType, c.IsActive })
                    .HasDatabaseName("IX_Categories_Type_Active");
            });

            // Definition indexes
            modelBuilder.Entity<Definition>(entity =>
            {
                entity.HasIndex(d => d.VocabularyId)
                    .HasDatabaseName("IX_Definitions_Vocabulary");

                entity.HasIndex(d => new { d.VocabularyId, d.LanguageCode })
                    .HasDatabaseName("IX_Definitions_Vocabulary_Language");
            });

            // Example indexes
            modelBuilder.Entity<Example>(entity =>
            {
                entity.HasIndex(e => e.VocabularyId)
                    .HasDatabaseName("IX_Examples_Vocabulary");

                entity.HasIndex(e => new { e.VocabularyId, e.DifficultyLevel })
                    .HasDatabaseName("IX_Examples_Vocabulary_Difficulty");
            });

            // Kanji Meaning indexes
            modelBuilder.Entity<KanjiMeaning>(entity =>
            {
                entity.HasIndex(km => km.KanjiId)
                    .HasDatabaseName("IX_KanjiMeanings_Kanji");

                entity.HasIndex(km => new { km.KanjiId, km.Language })
                    .HasDatabaseName("IX_KanjiMeanings_Kanji_Language");
            });

            // Kanji Vocabulary mapping indexes
            modelBuilder.Entity<KanjiVocabulary>(entity =>
            {
                entity.HasIndex(kv => new { kv.KanjiId, kv.VocabularyId })
                    .IsUnique()
                    .HasDatabaseName("IX_KanjiVocabularies_Kanji_Vocabulary");

                entity.HasIndex(kv => kv.VocabularyId)
                    .HasDatabaseName("IX_KanjiVocabularies_Vocabulary");
            });

            // Schedule indexes
            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasIndex(s => new { s.CreatedByUserId, s.IsActive })
                    .HasDatabaseName("IX_Schedules_CreatedBy_Active");
            });

            // Schedule Item indexes
            modelBuilder.Entity<ScheduleItem>(entity =>
            {
                entity.HasIndex(si => new { si.ScheduleId, si.StartTime })
                    .HasDatabaseName("IX_ScheduleItems_Schedule_StartTime");

                entity.HasIndex(si => new { si.StartTime, si.Status })
                    .HasDatabaseName("IX_ScheduleItems_StartTime_Status");

                entity.HasIndex(si => new { si.Status, si.StartTime })
                    .HasDatabaseName("IX_ScheduleItems_Status_StartTime");
            });

            // Notification indexes
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasIndex(n => new { n.CreatedAt, n.TypeId })
                    .HasDatabaseName("IX_Notifications_CreatedAt_Type");

                entity.HasIndex(n => new { n.PriorityId, n.CreatedAt })
                    .HasDatabaseName("IX_Notifications_Priority_Created");
            });

            // Notification Recipient indexes - FIXED: Removed IsRead property
            modelBuilder.Entity<NotificationRecipient>(entity =>
            {
                entity.HasIndex(nr => new { nr.UserId, nr.ReadAt })
                    .HasDatabaseName("IX_NotificationRecipients_User_ReadAt");

                entity.HasIndex(nr => new { nr.NotificationId, nr.UserId })
                    .IsUnique()
                    .HasDatabaseName("IX_NotificationRecipients_Notification_User");

                entity.HasIndex(nr => new { nr.UserId, nr.IsOpened })
                    .HasDatabaseName("IX_NotificationRecipients_User_Opened");
            });

            // System Settings indexes - FIXED: Group instead of SettingGroup
            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasIndex(s => s.SettingKey)
                    .IsUnique()
                    .HasDatabaseName("IX_Settings_Key");

                entity.HasIndex(s => s.Group)
                    .HasDatabaseName("IX_Settings_Group");
            });

            // Technical Term indexes - FIXED: Frequency instead of Difficulty
            modelBuilder.Entity<TechnicalTerm>(entity =>
            {
                entity.HasIndex(tt => tt.Term)
                    .HasDatabaseName("IX_TechnicalTerms_Term");

                entity.HasIndex(tt => new { tt.CategoryId, tt.Status })
                    .HasDatabaseName("IX_TechnicalTerms_Category_Status");

                entity.HasIndex(tt => tt.Frequency)
                    .HasDatabaseName("IX_TechnicalTerms_Frequency");
            });

            // Personal Word List indexes
            modelBuilder.Entity<PersonalWordList>(entity =>
            {
                entity.HasIndex(pwl => new { pwl.UserId, pwl.IsActive })
                    .HasDatabaseName("IX_PersonalWordLists_User_Active");

                entity.HasIndex(pwl => new { pwl.UserId, pwl.CreatedAt })
                    .HasDatabaseName("IX_PersonalWordLists_User_Created");
            });

            // Personal Word List Item indexes
            modelBuilder.Entity<PersonalWordListItem>(entity =>
            {
                entity.HasIndex(pwli => new { pwli.ListId, pwli.VocabularyId })
                    .IsUnique()
                    .HasDatabaseName("IX_PersonalWordListItems_List_Vocabulary");

                entity.HasIndex(pwli => new { pwli.ListId, pwli.AddedAt })
                    .HasDatabaseName("IX_PersonalWordListItems_List_Added");
            });
        }


        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields()
        {
            var now = DateTime.UtcNow;

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

                        entry.Property("CreatedBy").IsModified = false;
                        entry.Property("CreatedAt").IsModified = false;
                    }
                }

                if (entry.Entity is ISoftDeletable softDeletable &&
                    entry.Property("IsDeleted").IsModified &&
                    softDeletable.IsDeleted)
                {
                    softDeletable.DeletedAt = now;

                    if (entry.Entity is SoftDeletableEntity softDeletableEntity && _currentUserId.HasValue)
                    {
                        softDeletableEntity.DeletedBy = _currentUserId.Value;
                    }
                }
            }
        }
    }
}