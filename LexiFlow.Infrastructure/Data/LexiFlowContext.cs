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
    /// Context cơ sở dữ liệu chính cho ứng dụng LexiFlow - Tối ưu cho .NET 9
    /// Khắc phục các lỗi Entity Framework Global Query Filter và Foreign Key conflicts
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

        /// <summary>
        /// Thiết lập ID user hiện tại cho audit tracking
        /// </summary>
        public void SetCurrentUserId(int userId)
        {
            _currentUserId = userId;
        }

        #region DbSets
        // Quản lý User
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

        // Nội dung học tập
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

        // Theo dõi tiến độ
        public DbSet<LearningProgress> LearningProgresses { get; set; }
        public DbSet<LearningSession> LearningSessions { get; set; }
        public DbSet<LearningSessionDetail> SessionDetails { get; set; }
        public DbSet<UserKanjiProgress> UserKanjiProgresses { get; set; }
        public DbSet<UserGrammarProgress> UserGrammarProgresses { get; set; }
        public DbSet<GoalProgress> GoalProgresses { get; set; }
        public DbSet<PersonalWordList> PersonalWordLists { get; set; }
        public DbSet<PersonalWordListItem> PersonalWordListItems { get; set; }

        // Lập kế hoạch học tập
        public DbSet<StudyGoal> StudyGoals { get; set; }
        public DbSet<StudyPlan> StudyPlans { get; set; }
        public DbSet<StudySession> StudySessions { get; set; }
        public DbSet<StudyTask> StudyTasks { get; set; }
        public DbSet<StudyTopic> StudyTopics { get; set; }
        public DbSet<StudyPlanItem> StudyPlanItems { get; set; }
        public DbSet<StudyPlanProgress> StudyPlanProgresses { get; set; }
        public DbSet<TaskCompletion> TaskCompletions { get; set; }

        // Luyện tập và thi cử
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<TestDetail> TestDetails { get; set; }
        public DbSet<JLPTExam> JLPTExams { get; set; }
        public DbSet<JLPTLevel> JLPTLevels { get; set; }
        public DbSet<JLPTSection> JLPTSections { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<UserExam> UserExams { get; set; }

        // Thông báo
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<NotificationPriority> NotificationPriorities { get; set; }
        public DbSet<NotificationStatus> NotificationStatuses { get; set; }
        public DbSet<NotificationRecipient> NotificationRecipients { get; set; }
        public DbSet<NotificationResponse> NotificationResponses { get; set; }

        // Lịch trình
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleItem> ScheduleItems { get; set; }
        public DbSet<ScheduleItemType> ScheduleItemTypes { get; set; }
        public DbSet<ScheduleRecurrence> ScheduleRecurrences { get; set; }
        public DbSet<ScheduleItemParticipant> ScheduleItemParticipants { get; set; }
        public DbSet<ScheduleReminder> ScheduleReminders { get; set; }

        // Hệ thống
        public DbSet<Setting> Settings { get; set; }
        public DbSet<DeletedItem> DeletedItems { get; set; }
        public DbSet<SyncMetadata> SyncMetadata { get; set; }
        public DbSet<SyncConflict> SyncConflicts { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Cấu hình để giảm warning log levels
            optionsBuilder.ConfigureWarnings(warnings =>
            {
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.ForeignKeyPropertiesMappedToUnrelatedTables);
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.AmbientTransactionWarning);
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.CoreEventId.NavigationBaseIncludeIgnored);
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TẮT TẤT CẢ CASCADE TOÀN CỤC ĐỂ TRÁNH XUNG ĐỘT - QUAN TRỌNG!
            DisableAllCascades(modelBuilder);

            // Cấu hình theo thứ tự ưu tiên
            ConfigureCoreEntities(modelBuilder);
            ConfigureGlobalQueryFilters(modelBuilder); // SỬA LỖI GLOBAL QUERY FILTER
            ConfigureUserEntities(modelBuilder);
            ConfigureVocabularyEntities(modelBuilder);
            ConfigureKanjiEntities(modelBuilder); // THÊM MỚI
            ConfigureGrammarEntities(modelBuilder);
            ConfigureMediaEntities(modelBuilder);
            ConfigureExamEntities(modelBuilder);
            ConfigureProgressEntities(modelBuilder);
            ConfigurePlanningEntities(modelBuilder);
            ConfigureTechnicalTermEntities(modelBuilder);
            ConfigureNotificationEntities(modelBuilder);
            ConfigureSchedulingEntities(modelBuilder);
            ConfigureSystemEntities(modelBuilder);
            ConfigureSyncEntities(modelBuilder); // THÊM MỚI

            // Tối ưu hiệu suất cho .NET 9
            ConfigurePerformanceIndexes(modelBuilder);

            // CHỈ BẬT LẠI CÁC CASCADE THIẾT YẾU SAU CÙNG
            ConfigureEssentialCascades(modelBuilder);

            // SỬA LỖI FOREIGN KEY SHADOW PROPERTIES
            FixForeignKeyShadowProperties(modelBuilder);
        }

        /// <summary>
        /// Tắt tất cả cascade toàn cục để tránh xung đột
        /// </summary>
        private void DisableAllCascades(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }
        }

        /// <summary>
        /// SỬA LỖI: Cấu hình Global Query Filters để khắc phục warning
        /// Áp dụng query filter nhất quán cho tất cả entities có soft delete
        /// </summary>
        private void ConfigureGlobalQueryFilters(ModelBuilder modelBuilder)
        {
            _logger?.LogInformation("Đang cấu hình Global Query Filters...");

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Kiểm tra nếu entity implement ISoftDeletable
                if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted));
                    var condition = Expression.Equal(property, Expression.Constant(false));
                    var lambda = Expression.Lambda(condition, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);

                    _logger?.LogDebug("Đã áp dụng soft delete filter cho entity: {EntityName}", entityType.ClrType.Name);
                }
            }
        }

        /// <summary>
        /// SỬA LỖI: Khắc phục Shadow Properties bị tạo do xung đột tên foreign key
        /// </summary>
        private void FixForeignKeyShadowProperties(ModelBuilder modelBuilder)
        {
            _logger?.LogInformation("Đang khắc phục Foreign Key Shadow Properties...");

            // MediaFile - Sửa các shadow properties bị tạo
            modelBuilder.Entity<MediaFile>(entity =>
            {
                // Đảm bảo các FK được map rõ ràng để tránh shadow properties
                entity.Property(m => m.ExampleId).HasColumnName("ExampleId");
                entity.Property(m => m.GrammarExampleId).HasColumnName("GrammarExampleId");
                entity.Property(m => m.GrammarId).HasColumnName("GrammarId");
                entity.Property(m => m.KanjiExampleId).HasColumnName("KanjiExampleId");
                entity.Property(m => m.QuestionId).HasColumnName("QuestionId");
                entity.Property(m => m.QuestionOptionId).HasColumnName("QuestionOptionId");
                entity.Property(m => m.TermExampleId).HasColumnName("TermExampleId");
                entity.Property(m => m.UserId).HasColumnName("UserId");
            });

            // NotificationRecipient - Sửa GroupId shadow property
            modelBuilder.Entity<NotificationRecipient>(entity =>
            {
                entity.Property(nr => nr.GroupId).HasColumnName("GroupId");
            });

            // LearningProgress - Sửa shadow properties
            modelBuilder.Entity<LearningProgress>(entity =>
            {
                entity.Property(lp => lp.UserId).HasColumnName("UserId");
                entity.Property(lp => lp.VocabularyId).HasColumnName("VocabularyId");
            });

            // Các entities khác có shadow properties
            FixSchedulingEntityShadowProperties(modelBuilder);
            FixPermissionEntityShadowProperties(modelBuilder);
            FixUserEntityShadowProperties(modelBuilder);
        }

        /// <summary>
        /// Sửa shadow properties cho scheduling entities
        /// </summary>
        private void FixSchedulingEntityShadowProperties(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ScheduleItem>(entity =>
            {
                entity.Property(si => si.ScheduleId).HasColumnName("ScheduleId");
                entity.Property(si => si.StudyTaskId).HasColumnName("StudyTaskId");
            });

            modelBuilder.Entity<ScheduleItemParticipant>(entity =>
            {
                entity.Property(sip => sip.GroupId).HasColumnName("GroupId");
            });
        }

        /// <summary>
        /// Sửa shadow properties cho permission entities
        /// </summary>
        private void FixPermissionEntityShadowProperties(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupPermission>(entity =>
            {
                entity.Property(gp => gp.GroupId).HasColumnName("GroupId");
            });

            modelBuilder.Entity<PermissionGroupMapping>(entity =>
            {
                entity.Property(pgm => pgm.PermissionGroupId).HasColumnName("PermissionGroupId");
                entity.Property(pgm => pgm.PermissionId).HasColumnName("PermissionId");
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.Property(rp => rp.PermissionId).HasColumnName("PermissionId");
                entity.Property(rp => rp.RoleId).HasColumnName("RoleId");
            });

            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.Property(up => up.PermissionId).HasColumnName("PermissionId");
            });
        }

        /// <summary>
        /// Sửa shadow properties cho user entities
        /// </summary>
        private void FixUserEntityShadowProperties(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.Property(ug => ug.GroupId).HasColumnName("GroupId");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.Property(ur => ur.RoleId).HasColumnName("RoleId");
            });

            modelBuilder.Entity<UserTeam>(entity =>
            {
                entity.Property(ut => ut.TeamId).HasColumnName("TeamId");
            });
        }

        /// <summary>
        /// Chỉ bật lại các cascade quan trọng và thiết yếu
        /// </summary>
        private void ConfigureEssentialCascades(ModelBuilder modelBuilder)
        {
            _logger?.LogInformation("Đang cấu hình Essential Cascades...");

            // Notification -> NotificationRecipient (bắt buộc)
            modelBuilder.Entity<NotificationRecipient>()
                .HasOne(nr => nr.Notification)
                .WithMany(n => n.NotificationRecipients)
                .HasForeignKey(nr => nr.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);

            // NotificationRecipient -> NotificationResponse (bắt buộc)
            modelBuilder.Entity<NotificationResponse>()
                .HasOne(nr => nr.Recipient)
                .WithMany(nrec => nrec.NotificationResponses)
                .HasForeignKey(nr => nr.RecipientId)
                .OnDelete(DeleteBehavior.Cascade);

            // StudyPlan -> StudyGoal (bắt buộc)
            modelBuilder.Entity<StudyGoal>()
                .HasOne(sg => sg.StudyPlan)
                .WithMany(sp => sp.StudyGoals)
                .HasForeignKey(sg => sg.PlanId)
                .OnDelete(DeleteBehavior.Cascade);

            // Schedule -> ScheduleItem (bắt buộc)
            modelBuilder.Entity<ScheduleItem>()
                .HasOne(si => si.Schedule)
                .WithMany()
                .HasForeignKey(si => si.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade);

            // ScheduleItem -> ScheduleItemParticipant (bắt buộc)
            modelBuilder.Entity<ScheduleItemParticipant>()
                .HasOne(sip => sip.Item)
                .WithMany()
                .HasForeignKey(sip => sip.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // ScheduleItem -> ScheduleReminder (bắt buộc)
            modelBuilder.Entity<ScheduleReminder>()
                .HasOne(sr => sr.Item)
                .WithMany()
                .HasForeignKey(sr => sr.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> UserProfile, UserLearningPreference, UserNotificationSetting (cascade cần thiết)
            modelBuilder.Entity<UserProfile>()
                .HasOne(up => up.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<UserProfile>(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserLearningPreference>()
                .HasOne(ulp => ulp.User)
                .WithOne(u => u.LearningPreference)
                .HasForeignKey<UserLearningPreference>(ulp => ulp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserNotificationSetting>()
                .HasOne(uns => uns.User)
                .WithOne(u => u.NotificationSetting)
                .HasForeignKey<UserNotificationSetting>(uns => uns.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // PersonalWordList -> PersonalWordListItem (bắt buộc)
            modelBuilder.Entity<PersonalWordListItem>()
                .HasOne(pwli => pwli.List)
                .WithMany(pwl => pwl.Items)
                .HasForeignKey(pwli => pwli.ListId)
                .OnDelete(DeleteBehavior.Cascade);

            _logger?.LogInformation("Đã hoàn thành cấu hình Essential Cascades");
        }

        /// <summary>
        /// Cấu hình các base entities và audit tracking
        /// </summary>
        private void ConfigureCoreEntities(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(BaseEntity).IsAssignableFrom(e.ClrType)))
            {
                // Cấu hình RowVersion cho concurrency control
                modelBuilder.Entity(entityType.ClrType)
                    .Property("RowVersion")
                    .IsRowVersion()
                    .IsConcurrencyToken();

                // Cấu hình default values cho timestamps
                modelBuilder.Entity(entityType.ClrType)
                    .Property("CreatedAt")
                    .HasDefaultValueSql("GETUTCDATE()");

                modelBuilder.Entity(entityType.ClrType)
                    .Property("UpdatedAt")
                    .HasDefaultValueSql("GETUTCDATE()");
            }

            ConfigureAuditableEntities(modelBuilder);
            ConfigureSoftDeletableEntities(modelBuilder);
        }

        /// <summary>
        /// Cấu hình entities có audit tracking
        /// </summary>
        private void ConfigureAuditableEntities(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(AuditableEntity).IsAssignableFrom(e.ClrType)))
            {
                // QUAN TRỌNG: Đặt tất cả User navigation thành NoAction để tránh warning
                modelBuilder.Entity(entityType.ClrType)
                    .HasOne("LexiFlow.Models.User.User", "CreatedByUser")
                    .WithMany()
                    .HasForeignKey("CreatedBy")
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired(false); // THAY ĐỔI: Không bắt buộc để tránh circular reference

                modelBuilder.Entity(entityType.ClrType)
                    .HasOne("LexiFlow.Models.User.User", "ModifiedByUser")
                    .WithMany()
                    .HasForeignKey("ModifiedBy")
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired(false);
            }
        }

        /// <summary>
        /// Cấu hình entities có soft delete
        /// </summary>
        private void ConfigureSoftDeletableEntities(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(ISoftDeletable).IsAssignableFrom(e.ClrType) &&
                           e.ClrType.GetProperty("DeletedBy") != null))
            {
                // QUAN TRỌNG: NoAction để tránh warning với User entity
                modelBuilder.Entity(entityType.ClrType)
                    .HasOne("LexiFlow.Models.User.User", "DeletedByUser")
                    .WithMany()
                    .HasForeignKey("DeletedBy")
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired(false);
            }
        }

        /// <summary>
        /// Cấu hình User entities và relationships
        /// </summary>
        private void ConfigureUserEntities(ModelBuilder modelBuilder)
        {
            // User entity - core configuration
            modelBuilder.Entity<User>(entity =>
            {
                // One-to-One relationships with cascades
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

            // Department entity
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasOne(d => d.Manager)
                    .WithMany()
                    .HasForeignKey(d => d.ManagerUserId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(d => d.ParentDepartment)
                    .WithMany(d => d.ChildDepartments)
                    .HasForeignKey(d => d.ParentDepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(d => d.Users)
                    .WithOne(u => u.Department)
                    .HasForeignKey(u => u.DepartmentId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(d => d.Teams)
                    .WithOne(t => t.Department)
                    .HasForeignKey(t => t.DepartmentId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            ConfigureUserJunctionTables(modelBuilder);
        }

        /// <summary>
        /// Cấu hình junction tables cho User relationships
        /// </summary>
        private void ConfigureUserJunctionTables(ModelBuilder modelBuilder)
        {
            // UserRole - Many-to-Many
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

            // UserPermission
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

            // RolePermission
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

            // Các junction tables khác...
            ConfigureAdditionalUserJunctions(modelBuilder);
        }

        /// <summary>
        /// Cấu hình thêm các junction tables
        /// </summary>
        private void ConfigureAdditionalUserJunctions(ModelBuilder modelBuilder)
        {
            // UserTeam
            modelBuilder.Entity<UserTeam>(entity =>
            {
                entity.HasKey(ut => new { ut.UserId, ut.TeamId });
                entity.HasOne(ut => ut.User).WithMany().HasForeignKey(ut => ut.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(ut => ut.Team).WithMany().HasForeignKey(ut => ut.TeamId).OnDelete(DeleteBehavior.Cascade);
            });

            // UserGroup
            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.HasKey(ug => new { ug.UserId, ug.GroupId });
                entity.HasOne(ug => ug.User).WithMany().HasForeignKey(ug => ug.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(ug => ug.Group).WithMany().HasForeignKey(ug => ug.GroupId).OnDelete(DeleteBehavior.Cascade);
            });

            // GroupPermission
            modelBuilder.Entity<GroupPermission>(entity =>
            {
                entity.HasKey(gp => new { gp.GroupId, gp.PermissionId });
                entity.HasOne(gp => gp.Group).WithMany().HasForeignKey(gp => gp.GroupId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(gp => gp.Permission).WithMany().HasForeignKey(gp => gp.PermissionId).OnDelete(DeleteBehavior.Cascade);
            });

            // PermissionGroupMapping với các relationships phức tạp
            modelBuilder.Entity<PermissionGroupMapping>(entity =>
            {
                entity.HasKey(pgm => new { pgm.PermissionGroupId, pgm.PermissionId });

                entity.HasOne(pgm => pgm.Permission)
                    .WithMany()
                    .HasForeignKey(pgm => pgm.PermissionId)
                    .OnDelete(DeleteBehavior.NoAction); // QUAN TRỌNG: NoAction để tránh circular reference

                entity.HasOne(pgm => pgm.PermissionGroup)
                    .WithMany()
                    .HasForeignKey(pgm => pgm.PermissionGroupId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pgm => pgm.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(pgm => pgm.CreatedByUserId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(pgm => pgm.DependsOnPermission)
                    .WithMany()
                    .HasForeignKey(pgm => pgm.DependsOnPermissionId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }

        /// <summary>
        /// Cấu hình Vocabulary entities và relationships
        /// </summary>
        private void ConfigureVocabularyEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vocabulary>(entity =>
            {
                // Soft delete navigation
                entity.HasOne(v => v.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(v => v.DeletedBy)
                    .OnDelete(DeleteBehavior.NoAction);

                // Category relationship
                entity.HasOne(v => v.Category)
                    .WithMany()
                    .HasForeignKey(v => v.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Collections
                entity.HasMany(v => v.Definitions)
                    .WithOne(d => d.Vocabulary)
                    .HasForeignKey(d => d.VocabularyId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(v => v.Examples)
                    .WithOne(e => e.Vocabulary)
                    .HasForeignKey(e => e.VocabularyId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(v => v.Translations)
                    .WithOne(t => t.Vocabulary)
                    .HasForeignKey(t => t.VocabularyId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(v => v.MediaFiles)
                    .WithOne(m => m.Vocabulary)
                    .HasForeignKey(m => m.VocabularyId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // GroupVocabularyRelation
            modelBuilder.Entity<GroupVocabularyRelation>(entity =>
            {
                entity.HasOne(gvr => gvr.Group)
                    .WithMany()
                    .HasForeignKey(gvr => gvr.GroupId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(gvr => gvr.Vocabulary)
                    .WithMany()
                    .HasForeignKey(gvr => gvr.VocabularyId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }

        /// <summary>
        /// THÊM MỚI: Cấu hình Kanji entities để khắc phục warning
        /// </summary>
        private void ConfigureKanjiEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kanji>(entity =>
            {
                // Foreign Key relationships
                entity.HasOne(k => k.Category)
                    .WithMany()
                    .HasForeignKey(k => k.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Soft delete - người xóa
                entity.HasOne(k => k.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(k => k.DeletedBy)
                    .OnDelete(DeleteBehavior.SetNull);

                // Người kiểm duyệt
                entity.HasOne(k => k.VerifiedByUser)
                    .WithMany()
                    .HasForeignKey(k => k.VerifiedBy)
                    .OnDelete(DeleteBehavior.SetNull);

                // Collections với Cascade để đảm bảo tính toàn vẹn dữ liệu
                entity.HasMany(k => k.Meanings)
                    .WithOne(km => km.Kanji)
                    .HasForeignKey(km => km.KanjiId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(k => k.Examples)
                    .WithOne(ke => ke.Kanji)
                    .HasForeignKey(ke => ke.KanjiId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(k => k.ComponentMappings)
                    .WithOne(kcm => kcm.Kanji)
                    .HasForeignKey(kcm => kcm.KanjiId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(k => k.KanjiVocabularies)
                    .WithOne(kv => kv.Kanji)
                    .HasForeignKey(kv => kv.KanjiId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Media files và User progress dùng SetNull vì có thể tồn tại độc lập
                entity.HasMany(k => k.MediaFiles)
                    .WithOne()
                    .HasForeignKey("KanjiId") // Assuming MediaFile has KanjiId property
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(k => k.UserProgresses)
                    .WithOne(up => up.Kanji)
                    .HasForeignKey(up => up.KanjiId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // KanjiMeaning configuration
            modelBuilder.Entity<KanjiMeaning>(entity =>
            {
                entity.HasOne(km => km.Kanji)
                    .WithMany(k => k.Meanings)
                    .HasForeignKey(km => km.KanjiId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // KanjiExample configuration
            modelBuilder.Entity<KanjiExample>(entity =>
            {
                entity.HasOne(ke => ke.Kanji)
                    .WithMany(k => k.Examples)
                    .HasForeignKey(ke => ke.KanjiId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // KanjiComponentMapping configuration
            modelBuilder.Entity<KanjiComponentMapping>(entity =>
            {
                entity.HasOne(kcm => kcm.Kanji)
                    .WithMany(k => k.ComponentMappings)
                    .HasForeignKey(kcm => kcm.KanjiId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(kcm => kcm.Component)
                    .WithMany()
                    .HasForeignKey(kcm => kcm.ComponentId)
                    .OnDelete(DeleteBehavior.Restrict); // Không cho xóa component nếu đang được sử dụng
            });

            // KanjiVocabulary configuration
            modelBuilder.Entity<KanjiVocabulary>(entity =>
            {
                entity.HasOne(kv => kv.Kanji)
                    .WithMany(k => k.KanjiVocabularies)
                    .HasForeignKey(kv => kv.KanjiId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        /// <summary>
        /// Cấu hình Grammar entities
        /// </summary>
        private void ConfigureGrammarEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Grammar>(entity =>
            {
                entity.HasOne(g => g.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(g => g.DeletedBy)
                    .OnDelete(DeleteBehavior.NoAction);

                // Collections
                entity.HasMany(g => g.Definitions)
                    .WithOne(gd => gd.Grammar)
                    .HasForeignKey(gd => gd.GrammarId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(g => g.Examples)
                    .WithOne(ge => ge.Grammar)
                    .HasForeignKey(ge => ge.GrammarId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(g => g.Translations)
                    .WithOne(gt => gt.Grammar)
                    .HasForeignKey(gt => gt.GrammarId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // UserGrammarProgress
            modelBuilder.Entity<UserGrammarProgress>(entity =>
            {
                entity.HasOne(ugp => ugp.User)
                    .WithMany()
                    .HasForeignKey(ugp => ugp.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(ugp => ugp.Grammar)
                    .WithMany()
                    .HasForeignKey(ugp => ugp.GrammarId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }

        /// <summary>
        /// Cấu hình Media entities với tất cả relationships
        /// </summary>
        private void ConfigureMediaEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MediaFile>(entity =>
            {
                // Basic relationships - tất cả NoAction để tránh warning
                entity.HasOne(m => m.User)
                    .WithMany()
                    .HasForeignKey(m => m.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(m => m.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(m => m.DeletedBy)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(m => m.Category)
                    .WithMany()
                    .HasForeignKey(m => m.CategoryId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Learning content relationships
                entity.HasOne(m => m.Vocabulary)
                    .WithMany(v => v.MediaFiles)
                    .HasForeignKey(m => m.VocabularyId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(m => m.Kanji)
                    .WithMany(k => k.MediaFiles)
                    .HasForeignKey(m => m.KanjiId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(m => m.Grammar)
                    .WithMany()
                    .HasForeignKey(m => m.GrammarId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(m => m.TechnicalTerm)
                    .WithMany(t => t.MediaFiles)
                    .HasForeignKey(m => m.TechnicalTermId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Example relationships
                entity.HasOne(m => m.Example)
                    .WithMany()
                    .HasForeignKey(m => m.ExampleId)
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

                // Question relationships
                entity.HasOne(m => m.Question)
                    .WithMany()
                    .HasForeignKey(m => m.QuestionId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(m => m.QuestionOption)
                    .WithMany()
                    .HasForeignKey(m => m.QuestionOptionId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // MediaProcessingHistory với MediaFile cascade
            modelBuilder.Entity<MediaProcessingHistory>(entity =>
            {
                entity.HasOne(mph => mph.MediaFile)
                    .WithMany()
                    .HasForeignKey(mph => mph.MediaId)
                    .OnDelete(DeleteBehavior.NoAction); // KHẮC PHỤC WARNING
            });
        }

        /// <summary>
        /// Cấu hình Exam entities
        /// </summary>
        private void ConfigureExamEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasOne(q => q.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(q => q.CreatedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(q => q.Section)
                    .WithMany()
                    .HasForeignKey(q => q.SectionId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<UserExam>(entity =>
            {
                entity.HasOne(ue => ue.User)
                    .WithMany()
                    .HasForeignKey(ue => ue.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(ue => ue.Exam)
                    .WithMany()
                    .HasForeignKey(ue => ue.ExamId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<JLPTExam>(entity =>
            {
                entity.HasOne(je => je.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(je => je.CreatedByUserId)
                    .OnDelete(DeleteBehavior.NoAction); // KHẮC PHỤC WARNING
            });
        }

        /// <summary>
        /// Cấu hình Progress entities
        /// </summary>
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

            modelBuilder.Entity<LearningSession>(entity =>
            {
                entity.HasOne(ls => ls.User)
                    .WithMany()
                    .HasForeignKey(ls => ls.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // PersonalWordList và items
            modelBuilder.Entity<PersonalWordList>(entity =>
            {
                entity.HasOne(pwl => pwl.User)
                    .WithMany()
                    .HasForeignKey(pwl => pwl.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(pwl => pwl.Items)
                    .WithOne(pwli => pwli.List)
                    .HasForeignKey(pwli => pwli.ListId)
                    .OnDelete(DeleteBehavior.Cascade); // Essential cascade
            });

            modelBuilder.Entity<PersonalWordListItem>(entity =>
            {
                entity.HasOne(pwli => pwli.List)
                    .WithMany(pwl => pwl.Items)
                    .HasForeignKey(pwli => pwli.ListId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pwli => pwli.Vocabulary)
                    .WithMany()
                    .HasForeignKey(pwli => pwli.VocabularyId)
                    .OnDelete(DeleteBehavior.NoAction); // KHẮC PHỤC WARNING
            });
        }

        /// <summary>
        /// Cấu hình Planning entities
        /// </summary>
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
                    .OnDelete(DeleteBehavior.Cascade); // Essential cascade
            });

            modelBuilder.Entity<StudyGoal>(entity =>
            {
                entity.HasOne(sg => sg.StudyPlan)
                    .WithMany(sp => sp.StudyGoals)
                    .HasForeignKey(sg => sg.PlanId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(sg => sg.ModifiedByUser)
                    .WithMany()
                    .HasForeignKey(sg => sg.ModifiedBy)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<TaskCompletion>(entity =>
            {
                entity.HasOne(tc => tc.User)
                    .WithMany()
                    .HasForeignKey(tc => tc.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(tc => tc.StudyTask)
                    .WithMany()
                    .HasForeignKey(tc => tc.TaskId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<StudySession>(entity =>
            {
                entity.HasOne(ss => ss.User)
                    .WithMany()
                    .HasForeignKey(ss => ss.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<StudyTopic>(entity =>
            {
                entity.HasOne(st => st.ModifiedByUser)
                    .WithMany()
                    .HasForeignKey(st => st.ModifiedBy)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<StudyPlanItem>(entity =>
            {
                entity.HasOne(spi => spi.ModifiedByUser)
                    .WithMany()
                    .HasForeignKey(spi => spi.ModifiedBy)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<StudyPlanProgress>(entity =>
            {
                entity.HasOne(spp => spp.User)
                    .WithMany()
                    .HasForeignKey(spp => spp.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }

        /// <summary>
        /// Cấu hình Technical Term entities
        /// </summary>
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

                entity.HasMany(t => t.MediaFiles)
                    .WithOne(m => m.TechnicalTerm)
                    .HasForeignKey(m => m.TechnicalTermId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(t => t.UserTechnicalTerms)
                    .WithOne(utt => utt.Term)
                    .HasForeignKey(utt => utt.TermId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<TermExample>(entity =>
            {
                entity.HasOne(te => te.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(te => te.CreatedBy)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<TermTranslation>(entity =>
            {
                entity.HasOne(tt => tt.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(tt => tt.CreatedBy)
                    .OnDelete(DeleteBehavior.NoAction);
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
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<UserTechnicalTerm>(entity =>
            {
                entity.HasOne(utt => utt.User)
                    .WithMany()
                    .HasForeignKey(utt => utt.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(utt => utt.Term)
                    .WithMany(t => t.UserTechnicalTerms)
                    .HasForeignKey(utt => utt.TermId)
                    .OnDelete(DeleteBehavior.NoAction); // KHẮC PHỤC WARNING
            });
        }

        /// <summary>
        /// Cấu hình Notification entities với cascades quan trọng
        /// </summary>
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

                // Essential cascade để cleanup notification recipients
                entity.HasMany(n => n.NotificationRecipients)
                    .WithOne(nr => nr.Notification)
                    .HasForeignKey(nr => nr.NotificationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<NotificationRecipient>(entity =>
            {
                // Essential cascade từ notification
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

                // Essential cascade cho responses
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

        /// <summary>
        /// Cấu hình Scheduling entities
        /// </summary>
        private void ConfigureSchedulingEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasOne(s => s.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(s => s.CreatedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(s => s.ModifiedByUser)
                    .WithMany()
                    .HasForeignKey(s => s.ModifiedBy)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<ScheduleItem>(entity =>
            {
                // Essential cascade từ schedule
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

                entity.HasOne(si => si.ModifiedByUser)
                    .WithMany()
                    .HasForeignKey(si => si.ModifiedBy)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<ScheduleItemParticipant>(entity =>
            {
                // Essential cascade từ schedule item
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
                // Essential cascade từ schedule item
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

        /// <summary>
        /// Cấu hình System entities
        /// </summary>
        private void ConfigureSystemEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasIndex(s => s.SettingKey)
                    .IsUnique();
            });
        }

        /// <summary>
        /// THÊM MỚI: Cấu hình Sync entities để khắc phục warnings
        /// </summary>
        private void ConfigureSyncEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SyncMetadata>(entity =>
            {
                entity.HasOne(sm => sm.User)
                    .WithMany()
                    .HasForeignKey(sm => sm.UserID)
                    .OnDelete(DeleteBehavior.NoAction); // KHẮC PHỤC WARNING
            });

            modelBuilder.Entity<SyncConflict>(entity =>
            {
                entity.HasOne(sc => sc.User)
                    .WithMany()
                    .HasForeignKey(sc => sc.UserID)
                    .OnDelete(DeleteBehavior.NoAction); // KHẮC PHỤC WARNING
            });
        }

        /// <summary>
        /// Cấu hình indexes hiệu suất cao cho .NET 9
        /// Tối ưu cho các truy vấn thường xuyên và quan trọng
        /// </summary>
        private void ConfigurePerformanceIndexes(ModelBuilder modelBuilder)
        {
            _logger?.LogInformation("Đang cấu hình Performance Indexes...");

            // INDEXES CẤP ĐỘ CAO - Authentication & Core Users
            ConfigureUserPerformanceIndexes(modelBuilder);

            // INDEXES CẤP ĐỘ CAO - Learning Content
            ConfigureVocabularyPerformanceIndexes(modelBuilder);
            ConfigureKanjiPerformanceIndexes(modelBuilder);
            ConfigureGrammarPerformanceIndexes(modelBuilder);

            // INDEXES CẤP ĐỘ TRUNG BÌNH - Progress & Analytics
            ConfigureProgressPerformanceIndexes(modelBuilder);

            // INDEXES CẤP ĐỘ THẤP - Administrative & Support
            ConfigureAdministrativeIndexes(modelBuilder);

            _logger?.LogInformation("Đã hoàn thành cấu hình Performance Indexes");
        }

        /// <summary>
        /// Indexes hiệu suất cao cho User entities
        /// </summary>
        private void ConfigureUserPerformanceIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Username)
                    .IsUnique()
                    .HasDatabaseName("IX_Users_Username_Unique");

                entity.HasIndex(u => u.Email)
                    .HasDatabaseName("IX_Users_Email");

                entity.HasIndex(u => new { u.IsActive, u.LastLoginAt })
                    .HasDatabaseName("IX_Users_Active_LastLogin");

                entity.HasIndex(u => u.DepartmentId)
                    .HasDatabaseName("IX_Users_Department");

                entity.HasIndex(u => new { u.IsActive, u.CreatedAt })
                    .HasDatabaseName("IX_Users_Active_Created");

                entity.HasIndex(u => new { u.IsDeleted, u.IsActive })
                    .HasDatabaseName("IX_Users_SoftDelete_Active");
            });

            // UserRole junction table performance
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasIndex(ur => ur.UserId)
                    .HasDatabaseName("IX_UserRoles_User");

                entity.HasIndex(ur => ur.RoleId)
                    .HasDatabaseName("IX_UserRoles_Role");
            });
        }

        /// <summary>
        /// Indexes hiệu suất cao cho Vocabulary entities
        /// </summary>
        private void ConfigureVocabularyPerformanceIndexes(ModelBuilder modelBuilder)
        {
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

            // Definition indexes cho tìm kiếm
            modelBuilder.Entity<Definition>(entity =>
            {
                entity.HasIndex(d => d.VocabularyId)
                    .HasDatabaseName("IX_Definitions_Vocabulary");

                entity.HasIndex(d => new { d.VocabularyId, d.LanguageCode })
                    .HasDatabaseName("IX_Definitions_Vocabulary_Language");

                entity.HasIndex(d => d.LanguageCode)
                    .HasDatabaseName("IX_Definitions_Language");
            });

            // Example indexes
            modelBuilder.Entity<Example>(entity =>
            {
                entity.HasIndex(e => e.VocabularyId)
                    .HasDatabaseName("IX_Examples_Vocabulary");

                entity.HasIndex(e => new { e.VocabularyId, e.DifficultyLevel })
                    .HasDatabaseName("IX_Examples_Vocabulary_Difficulty");
            });
        }

        /// <summary>
        /// Indexes hiệu suất cao cho Kanji entities
        /// </summary>
        private void ConfigureKanjiPerformanceIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kanji>(entity =>
            {
                entity.HasIndex(k => k.Character)
                    .IsUnique()
                    .HasDatabaseName("IX_Kanjis_Character_Unique");

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

                entity.HasIndex(k => new { k.IsDeleted, k.IsActive })
                    .HasDatabaseName("IX_Kanjis_SoftDelete_Active");
            });

            // KanjiMeaning indexes
            modelBuilder.Entity<KanjiMeaning>(entity =>
            {
                entity.HasIndex(km => km.KanjiId)
                    .HasDatabaseName("IX_KanjiMeanings_Kanji");

                entity.HasIndex(km => new { km.KanjiId, km.Language })
                    .HasDatabaseName("IX_KanjiMeanings_Kanji_Language");
            });

            // KanjiVocabulary mapping indexes
            modelBuilder.Entity<KanjiVocabulary>(entity =>
            {
                entity.HasIndex(kv => new { kv.KanjiId, kv.VocabularyId })
                    .IsUnique()
                    .HasDatabaseName("IX_KanjiVocabularies_Kanji_Vocabulary_Unique");

                entity.HasIndex(kv => kv.VocabularyId)
                    .HasDatabaseName("IX_KanjiVocabularies_Vocabulary");
            });
        }

        /// <summary>
        /// Indexes hiệu suất cho Grammar entities
        /// </summary>
        private void ConfigureGrammarPerformanceIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Grammar>(entity =>
            {
                entity.HasIndex(g => g.Pattern)
                    .HasDatabaseName("IX_Grammars_Pattern");

                entity.HasIndex(g => new { g.Level, g.IsActive })
                    .HasDatabaseName("IX_Grammars_Level_Active");

                entity.HasIndex(g => new { g.GrammarType, g.Level })
                    .HasDatabaseName("IX_Grammars_Type_Level");

                entity.HasIndex(g => new { g.IsDeleted, g.IsActive })
                    .HasDatabaseName("IX_Grammars_SoftDelete_Active");
            });

            // UserGrammarProgress indexes
            modelBuilder.Entity<UserGrammarProgress>(entity =>
            {
                entity.HasIndex(ugp => new { ugp.UserId, ugp.GrammarId })
                    .IsUnique()
                    .HasDatabaseName("IX_UserGrammarProgresses_User_Grammar_Unique");

                entity.HasIndex(ugp => ugp.UserId)
                    .HasDatabaseName("IX_UserGrammarProgresses_User");
            });
        }

        /// <summary>
        /// Indexes hiệu suất cho Progress entities
        /// </summary>
        private void ConfigureProgressPerformanceIndexes(ModelBuilder modelBuilder)
        {
            // LearningProgress - Quan trọng cho SRS system
            modelBuilder.Entity<LearningProgress>(entity =>
            {
                entity.HasIndex(lp => new { lp.UserId, lp.VocabularyId })
                    .IsUnique()
                    .HasDatabaseName("IX_LearningProgresses_User_Vocabulary_Unique");

                entity.HasIndex(lp => new { lp.UserId, lp.NextReviewDate })
                    .HasDatabaseName("IX_LearningProgresses_User_NextReview");

                entity.HasIndex(lp => lp.MasteryLevel)
                    .HasDatabaseName("IX_LearningProgresses_Mastery");

                entity.HasIndex(lp => new { lp.UserId, lp.MasteryLevel, lp.NextReviewDate })
                    .HasDatabaseName("IX_LearningProgresses_User_Mastery_NextReview");

                entity.HasIndex(lp => new { lp.CreatedAt, lp.MasteryLevel })
                    .HasDatabaseName("IX_LearningProgresses_Created_Mastery");
            });

            // UserKanjiProgress
            modelBuilder.Entity<UserKanjiProgress>(entity =>
            {
                entity.HasIndex(ukp => new { ukp.UserId, ukp.KanjiId })
                    .IsUnique()
                    .HasDatabaseName("IX_UserKanjiProgresses_User_Kanji_Unique");

                entity.HasIndex(ukp => new { ukp.UserId, ukp.NextReviewDate })
                    .HasDatabaseName("IX_UserKanjiProgresses_User_NextReview");

                entity.HasIndex(ukp => new { ukp.UserId, ukp.RecognitionLevel })
                    .HasDatabaseName("IX_UserKanjiProgresses_User_Recognition");
            });

            // LearningSession
            modelBuilder.Entity<LearningSession>(entity =>
            {
                entity.HasIndex(ls => new { ls.UserId, ls.StartTime })
                    .HasDatabaseName("IX_LearningSessions_User_StartTime");

                entity.HasIndex(ls => ls.SessionType)
                    .HasDatabaseName("IX_LearningSessions_Type");

                entity.HasIndex(ls => new { ls.UserId, ls.SessionType, ls.StartTime })
                    .HasDatabaseName("IX_LearningSessions_User_Type_Start");
            });

            // StudyPlan
            modelBuilder.Entity<StudyPlan>(entity =>
            {
                entity.HasIndex(sp => new { sp.UserId, sp.IsActive })
                    .HasDatabaseName("IX_StudyPlans_User_Active");

                entity.HasIndex(sp => new { sp.StartDate, sp.TargetDate })
                    .HasDatabaseName("IX_StudyPlans_DateRange");
            });

            // StudyGoal
            modelBuilder.Entity<StudyGoal>(entity =>
            {
                entity.HasIndex(sg => new { sg.PlanId, sg.IsCompleted })
                    .HasDatabaseName("IX_StudyGoals_Plan_Completed");

                entity.HasIndex(sg => new { sg.TargetDate, sg.IsCompleted })
                    .HasDatabaseName("IX_StudyGoals_TargetDate_Completed");

                entity.HasIndex(sg => new { sg.Status, sg.Priority })
                    .HasDatabaseName("IX_StudyGoals_Status_Priority");
            });

            // TaskCompletion
            modelBuilder.Entity<TaskCompletion>(entity =>
            {
                entity.HasIndex(tc => new { tc.UserId, tc.CompletionDate })
                    .HasDatabaseName("IX_TaskCompletions_User_Date");

                entity.HasIndex(tc => new { tc.TaskId, tc.CompletionDate })
                    .HasDatabaseName("IX_TaskCompletions_Task_Date");

                entity.HasIndex(tc => new { tc.UserId, tc.CompletionPercentage })
                    .HasDatabaseName("IX_TaskCompletions_User_Percentage");
            });
        }

        /// <summary>
        /// Indexes cho administrative và support entities
        /// </summary>
        private void ConfigureAdministrativeIndexes(ModelBuilder modelBuilder)
        {
            // MediaFile indexes
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

                entity.HasIndex(m => new { m.IsDeleted, m.MediaType })
                    .HasDatabaseName("IX_MediaFiles_SoftDelete_Type");
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

            // Question indexes - optimized for common queries
            modelBuilder.Entity<Question>(entity =>
            {
                // Composite indexes for common query patterns
                entity.HasIndex(q => new { q.QuestionType, q.Difficulty, q.IsActive })
                    .HasDatabaseName("IX_Questions_Type_Difficulty_Active");

                entity.HasIndex(q => new { q.SectionId, q.IsActive })
                    .HasDatabaseName("IX_Questions_Section_Active");

                entity.HasIndex(q => new { q.IsDeleted, q.IsActive, q.QuestionType })
                    .HasDatabaseName("IX_Questions_SoftDelete_Active_Type");

                entity.HasIndex(q => new { q.CreatedBy, q.CreatedAt })
                    .HasDatabaseName("IX_Questions_CreatedBy_Date");

                // Performance tracking indexes
                entity.HasIndex(q => q.UsageCount)
                    .HasDatabaseName("IX_Questions_UsageCount");

                entity.HasIndex(q => q.SuccessRate)
                    .HasDatabaseName("IX_Questions_SuccessRate");

                // Full-text search index (if using SQL Server)
                entity.HasIndex(q => q.SearchVector)
                    .HasDatabaseName("IX_Questions_SearchVector");

                // Configure properties
                entity.Property(q => q.QuestionText)
                    .HasMaxLength(4000);

                entity.Property(q => q.Explanation)
                    .HasMaxLength(2000);

                entity.Property(q => q.CorrectAnswer)
                    .HasMaxLength(1000);
            });

            // UserExam indexes - optimized for dashboard and reporting
            modelBuilder.Entity<UserExam>(entity =>
            {
                entity.HasIndex(ue => new { ue.UserId, ue.StartTime, ue.Status })
                    .HasDatabaseName("IX_UserExams_User_StartTime_Status");

                entity.HasIndex(ue => new { ue.UserId, ue.ExamId, ue.Status })
                    .HasDatabaseName("IX_UserExams_User_Exam_Status");

                entity.HasIndex(ue => new { ue.IsDeleted, ue.Status, ue.StartTime })
                    .HasDatabaseName("IX_UserExams_SoftDelete_Status_StartTime");

                entity.HasIndex(ue => new { ue.IsPassed, ue.ScorePercentage })
                    .HasDatabaseName("IX_UserExams_Passed_Score");

                // Analytics indexes
                entity.HasIndex(ue => new { ue.EndTime, ue.Status })
                    .HasDatabaseName("IX_UserExams_EndTime_Status");

                // Configure decimal precision
                entity.Property(ue => ue.ScorePercentage)
                    .HasPrecision(5, 2);
            });

            // TestResult indexes - optimized for analytics and reporting
            modelBuilder.Entity<TestResult>(entity =>
            {
                entity.HasIndex(tr => new { tr.UserId, tr.TestType, tr.TestDate, tr.IsDeleted })
                    .HasDatabaseName("IX_TestResults_User_Type_Date_SoftDelete");

                entity.HasIndex(tr => new { tr.TestType, tr.Level, tr.AccuracyRate })
                    .HasDatabaseName("IX_TestResults_Type_Level_Accuracy");

                entity.HasIndex(tr => new { tr.UserId, tr.AccuracyRate, tr.TestDate })
                    .HasDatabaseName("IX_TestResults_User_Accuracy_Date");

                entity.HasIndex(tr => new { tr.CategoryId, tr.TestDate, tr.AccuracyRate })
                    .HasDatabaseName("IX_TestResults_Category_Date_Accuracy");

                // Performance analysis indexes
                entity.HasIndex(tr => new { tr.RankPercentile, tr.TestType })
                    .HasDatabaseName("IX_TestResults_Rank_Type");

                // Configure decimal precision
                entity.Property(tr => tr.AccuracyRate)
                    .HasPrecision(5, 2);

                entity.Property(tr => tr.ImprovementRate)
                    .HasPrecision(5, 2);

                // Configure JSON columns (if using SQL Server 2016+)
                entity.Property(tr => tr.WeakAreas)
                    .HasMaxLength(2000);

                entity.Property(tr => tr.StrongAreas)
                    .HasMaxLength(2000);

                entity.Property(tr => tr.Recommendations)
                    .HasMaxLength(4000);
            });

            // Global query filters for soft delete
            modelBuilder.Entity<Question>()
                .HasQueryFilter(q => !q.IsDeleted);

            modelBuilder.Entity<UserExam>()
                .HasQueryFilter(ue => !ue.IsDeleted);

            modelBuilder.Entity<TestResult>()
                .HasQueryFilter(tr => !tr.IsDeleted);

            ConfigureNotificationIndexes(modelBuilder);
            ConfigureSchedulingIndexes(modelBuilder);
            ConfigureTechnicalTermIndexes(modelBuilder);
            ConfigureSystemIndexes(modelBuilder);
        }

        /// <summary>
        /// Notification system indexes
        /// </summary>
        private void ConfigureNotificationIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasIndex(n => new { n.CreatedAt, n.TypeId })
                    .HasDatabaseName("IX_Notifications_CreatedAt_Type");

                entity.HasIndex(n => new { n.PriorityId, n.CreatedAt })
                    .HasDatabaseName("IX_Notifications_Priority_Created");

                entity.HasIndex(n => new { n.SenderUserId, n.CreatedAt })
                    .HasDatabaseName("IX_Notifications_Sender_Created");
            });

            modelBuilder.Entity<NotificationRecipient>(entity =>
            {
                entity.HasIndex(nr => new { nr.UserId, nr.ReadAt })
                    .HasDatabaseName("IX_NotificationRecipients_User_ReadAt");

                entity.HasIndex(nr => new { nr.NotificationId, nr.UserId })
                    .IsUnique()
                    .HasDatabaseName("IX_NotificationRecipients_Notification_User_Unique");

                entity.HasIndex(nr => new { nr.UserId, nr.IsOpened })
                    .HasDatabaseName("IX_NotificationRecipients_User_Opened");

                entity.HasIndex(nr => new { nr.UserId, nr.StatusId })
                    .HasDatabaseName("IX_NotificationRecipients_User_Status");
            });
        }

        /// <summary>
        /// Scheduling system indexes
        /// </summary>
        private void ConfigureSchedulingIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasIndex(s => new { s.CreatedByUserId, s.IsActive })
                    .HasDatabaseName("IX_Schedules_CreatedBy_Active");

                entity.HasIndex(s => new { s.UserId, s.IsActive })
                    .HasDatabaseName("IX_Schedules_User_Active");

                entity.HasIndex(s => new { s.IsDeleted, s.IsActive })
                    .HasDatabaseName("IX_Schedules_SoftDelete_Active");
            });

            modelBuilder.Entity<ScheduleItem>(entity =>
            {
                entity.HasIndex(si => new { si.ScheduleId, si.StartTime })
                    .HasDatabaseName("IX_ScheduleItems_Schedule_StartTime");

                entity.HasIndex(si => new { si.StartTime, si.Status })
                    .HasDatabaseName("IX_ScheduleItems_StartTime_Status");

                entity.HasIndex(si => new { si.Status, si.StartTime })
                    .HasDatabaseName("IX_ScheduleItems_Status_StartTime");

                entity.HasIndex(si => new { si.UserId, si.StartTime })
                    .HasDatabaseName("IX_ScheduleItems_User_StartTime");

                entity.HasIndex(si => new { si.IsDeleted, si.Status })
                    .HasDatabaseName("IX_ScheduleItems_SoftDelete_Status");
            });
        }

        /// <summary>
        /// Technical Term indexes
        /// </summary>
        private void ConfigureTechnicalTermIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TechnicalTerm>(entity =>
            {
                entity.HasIndex(tt => tt.Term)
                    .HasDatabaseName("IX_TechnicalTerms_Term");

                entity.HasIndex(tt => new { tt.CategoryId, tt.Status })
                    .HasDatabaseName("IX_TechnicalTerms_Category_Status");

                entity.HasIndex(tt => tt.Frequency)
                    .HasDatabaseName("IX_TechnicalTerms_Frequency");

                entity.HasIndex(tt => new { tt.Status, tt.Frequency })
                    .HasDatabaseName("IX_TechnicalTerms_Status_Frequency");

                entity.HasIndex(tt => new { tt.IsDeleted, tt.Status })
                    .HasDatabaseName("IX_TechnicalTerms_SoftDelete_Status");
            });

            // PersonalWordList indexes
            modelBuilder.Entity<PersonalWordList>(entity =>
            {
                entity.HasIndex(pwl => new { pwl.UserId, pwl.IsActive })
                    .HasDatabaseName("IX_PersonalWordLists_User_Active");

                entity.HasIndex(pwl => new { pwl.UserId, pwl.CreatedAt })
                    .HasDatabaseName("IX_PersonalWordLists_User_Created");

                entity.HasIndex(pwl => new { pwl.IsDeleted, pwl.IsActive })
                    .HasDatabaseName("IX_PersonalWordLists_SoftDelete_Active");
            });

            modelBuilder.Entity<PersonalWordListItem>(entity =>
            {
                entity.HasIndex(pwli => new { pwli.ListId, pwli.VocabularyId })
                    .IsUnique()
                    .HasDatabaseName("IX_PersonalWordListItems_List_Vocabulary_Unique");

                entity.HasIndex(pwli => new { pwli.ListId, pwli.AddedAt })
                    .HasDatabaseName("IX_PersonalWordListItems_List_Added");

                entity.HasIndex(pwli => pwli.VocabularyId)
                    .HasDatabaseName("IX_PersonalWordListItems_Vocabulary");
            });
        }

        /// <summary>
        /// System settings và audit indexes
        /// </summary>
        private void ConfigureSystemIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasIndex(s => s.SettingKey)
                    .IsUnique()
                    .HasDatabaseName("IX_Settings_Key_Unique");

                entity.HasIndex(s => s.Group)
                    .HasDatabaseName("IX_Settings_Group");

                entity.HasIndex(s => new { s.Group, s.IsActive })
                    .HasDatabaseName("IX_Settings_Group_Active");

                entity.HasIndex(s => new { s.IsDeleted, s.IsActive })
                    .HasDatabaseName("IX_Settings_SoftDelete_Active");
            });

            modelBuilder.Entity<SyncMetadata>(entity =>
            {
                entity.HasIndex(sm => new { sm.UserId, sm.LastSyncAt })
                    .HasDatabaseName("IX_SyncMetadata_User_LastSync");

                entity.HasIndex(sm => sm.EntityType)
                    .HasDatabaseName("IX_SyncMetadata_EntityType");

                entity.HasIndex(sm => new { sm.IsDeleted, sm.EntityType })
                    .HasDatabaseName("IX_SyncMetadata_SoftDelete_EntityType");
            });

            modelBuilder.Entity<SyncConflict>(entity =>
            {
                entity.HasIndex(sc => new { sc.UserId, sc.ConflictDate })
                    .HasDatabaseName("IX_SyncConflicts_User_ConflictDate");

                entity.HasIndex(sc => sc.ConflictType)
                    .HasDatabaseName("IX_SyncConflicts_ConflictType");

                entity.HasIndex(sc => new { sc.IsResolved, sc.ConflictDate })
                    .HasDatabaseName("IX_SyncConflicts_Resolved_Date");

                entity.HasIndex(sc => new { sc.IsDeleted, sc.IsResolved })
                    .HasDatabaseName("IX_SyncConflicts_SoftDelete_Resolved");
            });
        }

        /// <summary>
        /// Override SaveChanges để tự động cập nhật audit fields
        /// </summary>
        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        /// <summary>
        /// Override SaveChangesAsync để tự động cập nhật audit fields
        /// </summary>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Cập nhật tự động các trường audit cho entities
        /// </summary>
        private void UpdateAuditFields()
        {
            var now = DateTime.UtcNow;

            var entries = ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified) &&
                            e.Entity is BaseEntity);

            foreach (var entry in entries)
            {
                // Cập nhật BaseEntity timestamps
                if (entry.Entity is BaseEntity entity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedAt = now;
                    }
                    entity.UpdatedAt = now;
                }

                // Cập nhật AuditableEntity user tracking
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

                        // Không cho phép thay đổi CreatedBy và CreatedAt
                        entry.Property("CreatedBy").IsModified = false;
                        entry.Property("CreatedAt").IsModified = false;
                    }
                }

                // Cập nhật SoftDelete tracking
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

        /// <summary>
        /// Method hỗ trợ để ignore global query filters khi cần thiết
        /// </summary>
        public IQueryable<T> IncludeDeleted<T>() where T : class, ISoftDeletable
        {
            return Set<T>().IgnoreQueryFilters();
        }

        /// <summary>
        /// Method hỗ trợ để lấy chỉ các bản ghi đã xóa
        /// </summary>
        public IQueryable<T> OnlyDeleted<T>() where T : class, ISoftDeletable
        {
            return Set<T>().IgnoreQueryFilters().Where(x => x.IsDeleted);
        }

        /// <summary>
        /// Method hỗ trợ để soft delete một entity
        /// </summary>
        public void SoftDelete<T>(T entity) where T : class, ISoftDeletable
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;

            if (entity is SoftDeletableEntity deletableEntity && _currentUserId.HasValue)
            {
                deletableEntity.DeletedBy = _currentUserId.Value;
            }

            Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Method hỗ trợ để restore một entity đã bị soft delete
        /// </summary>
        public void RestoreDeleted<T>(T entity) where T : class, ISoftDeletable
        {
            entity.IsDeleted = false;
            entity.DeletedAt = null;

            if (entity is SoftDeletableEntity deletableEntity)
            {
                deletableEntity.DeletedBy = null;
            }

            Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Cleanup method để dispose resources
        /// </summary>
        public override void Dispose()
        {
            _logger?.LogDebug("Disposing LexiFlowContext...");
            base.Dispose();
        }

        /// <summary>
        /// Async cleanup method
        /// </summary>
        public override ValueTask DisposeAsync()
        {
            _logger?.LogDebug("Disposing LexiFlowContext async...");
            return base.DisposeAsync();
        }
    }
}