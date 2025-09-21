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
using LexiFlow.Models.Users.UserRelations;
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
    /// Context c? s? d? li?u chính hoàn ch?nh cho ?ng d?ng LexiFlow - .NET 9
    /// Qu?n lý t?t c? entities và relationships ???c t?i ?u hóa cho Entity Framework Core
    /// H? tr? audit trail, soft delete, và các tính n?ng nâng cao khác
    /// </summary>
    public class LexiFlowContext : DbContext
    {
        private readonly ILogger<LexiFlowContext> _logger;
        private int? _currentUserId;

        public LexiFlowContext(DbContextOptions<LexiFlowContext> options, ILogger<LexiFlowContext> logger = null)
            : base(options)
        {
            _logger = logger;
        }

        /// <summary>
        /// Thi?t l?p ID ng??i dùng hi?n t?i cho audit tracking
        /// ???c s? d?ng ?? t? ??ng ghi nh?n thông tin ng??i t?o/ch?nh s? entity
        /// </summary>
        public void SetCurrentUserId(int userId)
        {
            _currentUserId = userId;
        }

        #region DbSets - Qu?n lý ng??i dùng và phân quy?n

        // Qu?n lý ng??i dùng c? b?n
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserLearningPreference> UserLearningPreferences { get; set; }
        public DbSet<UserNotificationSetting> UserNotificationSettings { get; set; }

        // Qu?n lý vai trò và quy?n h?n
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionGroup> PermissionGroups { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<PermissionGroupMapping> PermissionGroupMappings { get; set; }

        // Qu?n lý t? ch?c
        public DbSet<Department> Departments { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserTeam> UserTeams { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<GroupPermission> GroupPermissions { get; set; }

        #endregion

        #region DbSets - N?i dung h?c t?p

        // T? v?ng và danh m?c
        public DbSet<Category> Categories { get; set; }
        public DbSet<VocabularyGroup> VocabularyGroups { get; set; }
        public DbSet<Vocabulary> Vocabularies { get; set; }
        public DbSet<Definition> Definitions { get; set; }
        public DbSet<Example> Examples { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<GroupVocabularyRelation> VocabularyRelations { get; set; }

        // Kanji và các thành ph?n liên quan
        public DbSet<Kanji> Kanjis { get; set; }
        public DbSet<KanjiVocabulary> KanjiVocabularies { get; set; }
        public DbSet<KanjiMeaning> KanjiMeanings { get; set; }
        public DbSet<KanjiExample> KanjiExamples { get; set; }
        public DbSet<KanjiExampleMeaning> KanjiExampleMeanings { get; set; }
        public DbSet<KanjiComponent> KanjiComponents { get; set; }
        public DbSet<KanjiComponentMapping> KanjiComponentMappings { get; set; }

        // Ng? pháp
        public DbSet<Grammar> Grammars { get; set; }
        public DbSet<GrammarDefinition> GrammarDefinitions { get; set; }
        public DbSet<GrammarExample> GrammarExamples { get; set; }
        public DbSet<GrammarTranslation> GrammarTranslations { get; set; }

        // Thu?t ng? k? thu?t
        public DbSet<TechnicalTerm> TechnicalTerms { get; set; }
        public DbSet<TermExample> TermExamples { get; set; }
        public DbSet<TermTranslation> TermTranslations { get; set; }
        public DbSet<TermRelation> TermRelations { get; set; }
        public DbSet<UserTechnicalTerm> UserTechnicalTerms { get; set; }

        #endregion

        #region DbSets - Qu?n lý Media

        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<MediaCategory> MediaCategories { get; set; }
        public DbSet<MediaProcessingHistory> MediaProcessingHistories { get; set; }

        #endregion

        #region DbSets - Ti?n ?? và phiên h?c

        public DbSet<LearningProgress> LearningProgresses { get; set; }
        public DbSet<LearningSession> LearningSessions { get; set; }
        public DbSet<LearningSessionDetail> SessionDetails { get; set; }
        public DbSet<UserKanjiProgress> UserKanjiProgresses { get; set; }
        public DbSet<UserGrammarProgress> UserGrammarProgresses { get; set; }
        public DbSet<GoalProgress> GoalProgresses { get; set; }
        public DbSet<PersonalWordList> PersonalWordLists { get; set; }
        public DbSet<PersonalWordListItem> PersonalWordListItems { get; set; }

        #endregion

        #region DbSets - K? ho?ch h?c t?p

        public DbSet<StudyGoal> StudyGoals { get; set; }
        public DbSet<StudyPlan> StudyPlans { get; set; }
        public DbSet<StudySession> StudySessions { get; set; }
        public DbSet<StudyTask> StudyTasks { get; set; }
        public DbSet<StudyTopic> StudyTopics { get; set; }
        public DbSet<StudyPlanItem> StudyPlanItems { get; set; }
        public DbSet<StudyPlanProgress> StudyPlanProgresses { get; set; }
        public DbSet<TaskCompletion> TaskCompletions { get; set; }

        #endregion

        #region DbSets - Thi c? và luy?n t?p

        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<TestDetail> TestDetails { get; set; }
        public DbSet<JLPTExam> JLPTExams { get; set; }
        public DbSet<JLPTLevel> JLPTLevels { get; set; }
        public DbSet<JLPTSection> JLPTSections { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<UserExam> UserExams { get; set; }

        #endregion

        #region DbSets - Thông báo

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<NotificationPriority> NotificationPriorities { get; set; }
        public DbSet<NotificationStatus> NotificationStatuses { get; set; }
        public DbSet<NotificationRecipient> NotificationRecipients { get; set; }
        public DbSet<NotificationResponse> NotificationResponses { get; set; }

        #endregion

        #region DbSets - L?p l?ch

        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleItem> ScheduleItems { get; set; }
        public DbSet<ScheduleItemType> ScheduleItemTypes { get; set; }
        public DbSet<ScheduleRecurrence> ScheduleRecurrences { get; set; }
        public DbSet<ScheduleItemParticipant> ScheduleItemParticipants { get; set; }
        public DbSet<ScheduleReminder> ScheduleReminders { get; set; }

        #endregion

        #region DbSets - H? th?ng và ??ng b?

        public DbSet<Setting> Settings { get; set; }
        public DbSet<DeletedItem> DeletedItems { get; set; }
        public DbSet<SyncMetadata> SyncMetadata { get; set; }
        public DbSet<SyncConflict> SyncConflicts { get; set; }

        #endregion

        /// <summary>
        /// Ghi ?è ph??ng th?c SaveChanges ?? t? ??ng c?p nh?t audit fields
        /// T? ??ng set th?i gian t?o/c?p nh?t và thông tin ng??i dùng th?c hi?n thao tác
        /// </summary>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditableEntities();
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Ghi ?è ph??ng th?c SaveChanges ??ng b?
        /// </summary>
        public override int SaveChanges()
        {
            UpdateAuditableEntities();
            return base.SaveChanges();
        }

        /// <summary>
        /// C?p nh?t thông tin audit cho các entities tr??c khi l?u
        /// X? lý t? ??ng c?p nh?t CreatedAt, UpdatedAt, CreatedBy, ModifiedBy
        /// </summary>
        private void UpdateAuditableEntities()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified))
                .ToList();

            foreach (var entityEntry in entries)
            {
                var entity = (BaseEntity)entityEntry.Entity;
                var now = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreatedAt = now;
                    entity.UpdatedAt = now;

                    // C?p nh?t thông tin audit cho AuditableEntity
                    if (entity is AuditableEntity auditableEntity && _currentUserId.HasValue)
                    {
                        auditableEntity.CreatedBy = _currentUserId.Value;
                    }
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    entity.UpdatedAt = now;

                    // C?p nh?t thông tin ng??i ch?nh s?a cho AuditableEntity
                    if (entity is AuditableEntity auditableEntity && _currentUserId.HasValue)
                    {
                        auditableEntity.ModifiedBy = _currentUserId.Value;
                    }
                }
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // C?u hình ?? gi?m các warning log không c?n thi?t
            optionsBuilder.ConfigureWarnings(warnings =>
            {
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.ForeignKeyPropertiesMappedToUnrelatedTables);
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.AmbientTransactionWarning);
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.CoreEventId.NavigationBaseIncludeIgnored);
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning);
            });

            // B?t sensitive data logging và logging trong môi tr??ng development
#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.LogTo(message => _logger?.LogInformation(message));
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _logger?.LogInformation("Bắt đầu cấu hình model cho LexiFlow Database Context...");

            // Tắt cascade delete toàn cục để tránh xung đột trong relationships phức tạp
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }

            // Ignore navigation properties của AuditableEntity để tránh xung đột
            // Chúng ta sẽ chỉ sử dụng IDs, không cần navigation properties
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).Ignore("CreatedByUser");
                    modelBuilder.Entity(entityType.ClrType).Ignore("ModifiedByUser");  
                    modelBuilder.Entity(entityType.ClrType).Ignore("DeletedByUser");
                }
            }

            // Áp dụng cấu hình cho từng nhóm entities
            ConfigureUserManagement(modelBuilder);
            ConfigureLearningContent(modelBuilder);
            ConfigureMediaManagement(modelBuilder);
            ConfigureProgressTracking(modelBuilder);
            ConfigureStudyPlanning(modelBuilder);
            ConfigureExamsAndPractice(modelBuilder);
            ConfigureNotifications(modelBuilder);
            ConfigureScheduling(modelBuilder);
            ConfigureSystemAndSync(modelBuilder);

            _logger?.LogInformation("Hoàn thành cấu hình model cho LexiFlow Database Context");
        }

        /// <summary>
        /// Cấu hình audit relationships cho các entities kế thừa AuditableEntity
        /// </summary>
        private void ConfigureAuditableEntities(ModelBuilder modelBuilder)
        {
            // Lấy tất cả entity types kế thừa từ AuditableEntity
            var auditableEntityTypes = modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(AuditableEntity).IsAssignableFrom(e.ClrType))
                .ToList();

            foreach (var entityType in auditableEntityTypes)
            {
                // Skip nếu đã configure trong các methods khác
                var entityBuilder = modelBuilder.Entity(entityType.ClrType);

                // Configure CreatedByUser relationship nếu chưa có
                if (!entityType.GetNavigations().Any(n => n.Name == "CreatedByUser" && n.ForeignKey != null))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasOne(typeof(User), "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.NoAction);
                }

                // Configure ModifiedByUser relationship nếu chưa có
                if (!entityType.GetNavigations().Any(n => n.Name == "ModifiedByUser" && n.ForeignKey != null))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasOne(typeof(User), "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedBy")
                        .OnDelete(DeleteBehavior.NoAction);
                }

                // Configure DeletedByUser relationship nếu chưa có
                if (!entityType.GetNavigations().Any(n => n.Name == "DeletedByUser" && n.ForeignKey != null))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasOne(typeof(User), "DeletedByUser")
                        .WithMany()
                        .HasForeignKey("DeletedBy")
                        .OnDelete(DeleteBehavior.NoAction);
                }
            }
        }

        #region Các ph??ng th?c c?u hình Model

        /// <summary>
        /// C?u hình các entities liên quan ??n qu?n lý ng??i dùng và phân quy?n
        /// </summary>
        private void ConfigureUserManagement(ModelBuilder modelBuilder)
        {
            // Cấu hình User entity - bảng người dùng chính
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email);
                
                // Relationship 1-1 với UserProfile
                entity.HasOne(u => u.Profile)
                    .WithOne(p => p.User)
                    .HasForeignKey<UserProfile>(p => p.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Relationship 1-1 với UserLearningPreference
                entity.HasOne(u => u.LearningPreference)
                    .WithOne(lp => lp.User)
                    .HasForeignKey<UserLearningPreference>(lp => lp.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Relationship 1-1 với UserNotificationSetting
                entity.HasOne(u => u.NotificationSetting)
                    .WithOne(ns => ns.User)
                    .HasForeignKey<UserNotificationSetting>(ns => ns.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Relationship với Department (nhiều user thuộc 1 department)
                entity.HasOne(u => u.Department)
                    .WithMany(d => d.Users)
                    .HasForeignKey(u => u.DepartmentId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Cấu hình Department entity - phòng ban
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.DepartmentId);
                entity.HasIndex(d => d.DepartmentName);
                entity.HasIndex(d => d.DepartmentCode).IsUnique();

                // Self-reference relationship cho phòng ban cha-con
                entity.HasOne(d => d.ParentDepartment)
                    .WithMany(d => d.ChildDepartments)
                    .HasForeignKey(d => d.ParentDepartmentId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Manager của department
                entity.HasOne(d => d.Manager)
                    .WithMany()
                    .HasForeignKey(d => d.ManagerUserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Cấu hình UserRole - bảng many-to-many giữa User và Role
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(ur => ur.Role)
                    .WithMany()
                    .HasForeignKey(ur => ur.RoleId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Configuration cho AssignedByUser
                entity.HasOne(ur => ur.AssignedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.AssignedBy)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Cấu hình Permission system
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(p => p.PermissionId);
                entity.HasIndex(p => p.PermissionName).IsUnique();

                entity.HasMany(p => p.PermissionGroupMappings)
                    .WithOne(pgm => pgm.Permission)
                    .HasForeignKey(pgm => pgm.PermissionId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Cấu hình PermissionGroup
            modelBuilder.Entity<PermissionGroup>(entity =>
            {
                entity.HasKey(pg => pg.PermissionGroupId);
                entity.HasIndex(pg => pg.GroupName).IsUnique();

                // Self-reference relationship
                entity.HasOne(pg => pg.ParentGroup)
                    .WithMany(pg => pg.ChildGroups)
                    .HasForeignKey(pg => pg.ParentGroupId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(pg => pg.PermissionMappings)
                    .WithOne(pgm => pgm.PermissionGroup)
                    .HasForeignKey(pgm => pgm.PermissionGroupId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Cấu hình PermissionGroupMapping
            modelBuilder.Entity<PermissionGroupMapping>(entity =>
            {
                entity.HasKey(pgm => pgm.MappingId);
                entity.HasIndex(pgm => new { pgm.PermissionGroupId, pgm.PermissionId }).IsUnique();

                entity.HasOne(pgm => pgm.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(pgm => pgm.CreatedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(pgm => pgm.DependsOnPermission)
                    .WithMany()
                    .HasForeignKey(pgm => pgm.DependsOnPermissionId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }

        /// <summary>
        /// C?u hình các entities liên quan ??n n?i dung h?c t?p
        /// </summary>
        private void ConfigureLearningContent(ModelBuilder modelBuilder)
        {
            // Cấu hình Category - danh mục từ vựng
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.CategoryId);
                entity.HasIndex(c => c.CategoryName);

                // Self-reference cho danh mục cha-con
                entity.HasOne(c => c.ParentCategory)
                    .WithMany(c => c.ChildCategories)
                    .HasForeignKey(c => c.ParentCategoryId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Cấu hình Vocabulary - từ vựng
            modelBuilder.Entity<Vocabulary>(entity =>
            {
                // Sử dụng key được kế thừa từ AuditableEntity
                entity.HasIndex(v => new { v.Term, v.LanguageCode });
                entity.HasIndex(v => v.CategoryId);
                entity.HasIndex(v => v.Level);

                // Relationship với Category
                entity.HasOne(v => v.Category)
                    .WithMany(c => c.Vocabularies)
                    .HasForeignKey(v => v.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Cấu hình Definition - định nghĩa từ vựng
            modelBuilder.Entity<Definition>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.HasIndex(d => d.VocabularyId);

                entity.HasOne(d => d.Vocabulary)
                    .WithMany(v => v.Definitions)
                    .HasForeignKey(d => d.VocabularyId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Cấu hình Example - ví dụ từ vựng
            modelBuilder.Entity<Example>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.VocabularyId);

                entity.HasOne(e => e.Vocabulary)
                    .WithMany(v => v.Examples)
                    .HasForeignKey(e => e.VocabularyId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Cấu hình Kanji
            modelBuilder.Entity<Kanji>(entity =>
            {
                entity.HasIndex(k => k.Character).IsUnique();
                entity.HasIndex(k => k.JLPTLevel);
                entity.HasIndex(k => k.StrokeCount);
            });

            // Cấu hình Grammar - ngữ pháp
            modelBuilder.Entity<Grammar>(entity =>
            {
                entity.HasIndex(g => g.Pattern);
                entity.HasIndex(g => g.Level);
            });

            // Cấu hình TechnicalTerm - thuật ngữ kỹ thuật
            modelBuilder.Entity<TechnicalTerm>(entity =>
            {
                entity.HasKey(tt => tt.TechnicalTermId);
                entity.HasIndex(tt => new { tt.Term, tt.LanguageCode, tt.Field });
                entity.HasIndex(tt => tt.Field);
            });
        }

        /// <summary>
        /// C?u hình các entities liên quan ??n qu?n lý media
        /// </summary>
        private void ConfigureMediaManagement(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MediaFile>(entity =>
            {
                entity.HasKey(m => m.MediaId);
                entity.HasIndex(m => new { m.MediaType, m.IsPrimary });
                entity.HasIndex(m => m.UserId);

                // Relationship với User (owner của file)
                entity.HasOne(m => m.User)
                    .WithMany(u => u.MediaFiles)
                    .HasForeignKey(m => m.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Relationship với User (người xóa file)
                entity.HasOne(m => m.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(m => m.DeletedBy)
                    .OnDelete(DeleteBehavior.NoAction);

                // Relationship với MediaCategory
                entity.HasOne(m => m.Category)
                    .WithMany()
                    .HasForeignKey(m => m.CategoryId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<MediaCategory>(entity =>
            {
                entity.HasKey(mc => mc.CategoryId);
                entity.HasIndex(mc => mc.CategoryName).IsUnique();
            });
        }

        /// <summary>
        /// C?u hình các entities liên quan ??n theo dõi ti?n ?? h?c t?p
        /// </summary>
        private void ConfigureProgressTracking(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LearningProgress>(entity =>
            {
                entity.HasKey(lp => lp.ProgressId);
                entity.HasIndex(lp => new { lp.UserId, lp.VocabularyId }).IsUnique();
                entity.HasIndex(lp => new { lp.UserId, lp.NextReviewDate });

                entity.HasOne(lp => lp.User)
                    .WithMany(u => u.LearningProgresses)
                    .HasForeignKey(lp => lp.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(lp => lp.Vocabulary)
                    .WithMany()
                    .HasForeignKey(lp => lp.VocabularyId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<LearningSession>(entity =>
            {
                entity.HasKey(ls => ls.SessionId);
                entity.HasIndex(ls => new { ls.UserId, ls.StartTime });

                entity.HasOne(ls => ls.User)
                    .WithMany()
                    .HasForeignKey(ls => ls.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<PersonalWordList>(entity =>
            {
                entity.HasIndex(pwl => new { pwl.UserId, pwl.ListName }).IsUnique();

                entity.HasOne(pwl => pwl.User)
                    .WithMany()
                    .HasForeignKey(pwl => pwl.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }

        /// <summary>
        /// C?u hình các entities liên quan ??n k? ho?ch h?c t?p
        /// </summary>
        private void ConfigureStudyPlanning(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudyPlan>(entity =>
            {
                entity.HasKey(sp => sp.StudyPlanId);
                entity.HasIndex(sp => new { sp.UserId, sp.PlanName }).IsUnique();

                entity.HasOne(sp => sp.User)
                    .WithMany()
                    .HasForeignKey(sp => sp.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<StudyGoal>(entity =>
            {
                entity.HasKey(sg => sg.GoalId);
                entity.HasIndex(sg => new { sg.PlanId, sg.GoalName }).IsUnique();
            });

            modelBuilder.Entity<StudyTask>(entity =>
            {
                entity.HasIndex(st => new { st.GoalId, st.TaskName }).IsUnique();
            });
        }

        /// <summary>
        /// C?u hình các entities liên quan ??n thi c? và luy?n t?p
        /// </summary>
        private void ConfigureExamsAndPractice(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(q => q.QuestionId);
                entity.HasIndex(q => q.QuestionType);

                entity.HasMany(q => q.Options)
                    .WithOne(qo => qo.Question)
                    .HasForeignKey(qo => qo.QuestionId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(q => q.UserAnswers)
                    .WithOne(ua => ua.Question)
                    .HasForeignKey(ua => ua.QuestionId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<TestResult>(entity =>
            {
                entity.HasKey(tr => tr.TestResultId);
                entity.HasIndex(tr => new { tr.UserId, tr.TestDate });
                entity.HasIndex(tr => tr.TestType);
                entity.HasIndex(tr => tr.Score);

                entity.HasOne(tr => tr.User)
                    .WithMany()
                    .HasForeignKey(tr => tr.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(tr => tr.TestDetails)
                    .WithOne(td => td.TestResult)
                    .HasForeignKey(td => td.TestResultId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<JLPTExam>(entity =>
            {
                entity.HasKey(je => je.ExamId);

                entity.HasOne(je => je.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(je => je.CreatedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }

        /// <summary>
        /// C?u hình các entities liên quan ??n h? th?ng thông báo
        /// </summary>
        private void ConfigureNotifications(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(n => n.NotificationId);
                entity.HasIndex(n => n.CreatedAt);

                entity.HasMany(n => n.NotificationRecipients)
                    .WithOne(nr => nr.Notification)
                    .HasForeignKey(nr => nr.NotificationId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<NotificationRecipient>(entity =>
            {
                entity.HasKey(nr => nr.RecipientId);
                entity.HasIndex(nr => new { nr.NotificationId, nr.UserId }).IsUnique();

                entity.HasOne(nr => nr.User)
                    .WithMany()
                    .HasForeignKey(nr => nr.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }

        /// <summary>
        /// C?u hình các entities liên quan ??n l?p l?ch
        /// </summary>
        private void ConfigureScheduling(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasKey(s => s.ScheduleId);
                entity.HasIndex(s => s.ScheduleName);

                entity.HasOne(s => s.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(s => s.CreatedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(s => s.ScheduleItems)
                    .WithOne(si => si.Schedule)
                    .HasForeignKey(si => si.ScheduleId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<ScheduleItem>(entity =>
            {
                entity.HasKey(si => si.ScheduleItemId);
                entity.HasIndex(si => si.StartTime);
                entity.HasIndex(si => new { si.ScheduleId, si.StartTime });

                entity.HasOne(si => si.StudyTask)
                    .WithMany()
                    .HasForeignKey(si => si.StudyTaskId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }

        /// <summary>
        /// C?u hình các entities liên quan ??n h? th?ng và ??ng b?
        /// </summary>
        private void ConfigureSystemAndSync(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasKey(s => s.SettingId);
                entity.HasIndex(s => s.SettingKey).IsUnique();
            });

            modelBuilder.Entity<DeletedItem>(entity =>
            {
                entity.HasKey(di => di.DeletedItemID);
                entity.HasIndex(di => new { di.EntityType, di.EntityId });

                entity.HasOne(di => di.User)
                    .WithMany()
                    .HasForeignKey(di => di.UserID)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<SyncMetadata>(entity =>
            {
                entity.HasKey(sm => sm.SyncMetadataID);

                entity.HasOne(sm => sm.User)
                    .WithMany()
                    .HasForeignKey(sm => sm.UserID)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<SyncConflict>(entity =>
            {
                entity.HasKey(sc => sc.ConflictID);

                entity.HasOne(sc => sc.User)
                    .WithMany()
                    .HasForeignKey(sc => sc.UserID)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }

        #endregion

        /// <summary>
        /// Kh?i t?o d? li?u m?u cho h? th?ng
        /// T?o các role, category, và setting c? b?n c?n thi?t
        /// </summary>
        public async Task SeedDataAsync()
        {
            _logger?.LogInformation("B?t ??u kh?i t?o d? li?u m?u cho h? th?ng...");

            // Kh?i t?o các role c? b?n
            if (!await Roles.AnyAsync())
            {
                var adminRole = new Role { RoleName = "Administrator", Description = "Qu?n tr? viên h? th?ng" };
                var teacherRole = new Role { RoleName = "Teacher", Description = "Vai trò giáo viên" };
                var studentRole = new Role { RoleName = "Student", Description = "Vai trò h?c sinh" };

                await Roles.AddRangeAsync(adminRole, teacherRole, studentRole);
                await base.SaveChangesAsync();
            }

            // Kh?i t?o các category m?c ??nh
            if (!await Categories.AnyAsync())
            {
                var categories = new[]
                {
                    new Category { CategoryName = "JLPT N5", Description = "T? v?ng c? b?n cho k? thi JLPT N5", Level = "N5" },
                    new Category { CategoryName = "JLPT N4", Description = "T? v?ng trung c?p cho k? thi JLPT N4", Level = "N4" },
                    new Category { CategoryName = "H?i tho?i hàng ngày", Description = "Các c?m t? thông d?ng trong giao ti?p hàng ngày", Level = "C? b?n" },
                    new Category { CategoryName = "Ti?ng Nh?t th??ng m?i", Description = "T? v?ng và c?m t? liên quan ??n business", Level = "Nâng cao" }
                };

                await Categories.AddRangeAsync(categories);
                await base.SaveChangesAsync();
            }

            // Kh?i t?o các setting m?c ??nh cho h? th?ng
            if (!await Settings.AnyAsync())
            {
                var settings = new[]
                {
                    new Setting { SettingKey = "DefaultLanguage", SettingValue = "vi", Description = "Ngôn ng? m?c ??nh c?a h? th?ng" },
                    new Setting { SettingKey = "SessionTimeout", SettingValue = "30", Description = "Th?i gian timeout phiên làm vi?c (phút)" },
                    new Setting { SettingKey = "MaxFileSize", SettingValue = "10485760", Description = "Kích th??c file t?i ?a cho phép (bytes)" }, // 10MB
                    new Setting { SettingKey = "EnableRegistration", SettingValue = "true", Description = "Cho phép ??ng ký tài kho?n m?i" }
                };

                await Settings.AddRangeAsync(settings);
                await base.SaveChangesAsync();
            }

            _logger?.LogInformation("Hoàn thành kh?i t?o d? li?u m?u cho h? th?ng");
        }

        /// <summary>
        /// Ph??ng th?c helper ?? l?y danh sách entities ?ang ho?t ??ng (không b? xóa m?m)
        /// </summary>
        public IQueryable<T> GetActiveEntities<T>() where T : BaseEntity
        {
            return Set<T>().Where(e => !e.IsDeleted && e.IsActive);
        }

        /// <summary>
        /// Ph??ng th?c xóa m?m hàng lo?t theo ?i?u ki?n
        /// </summary>
        public async Task<int> BulkSoftDeleteAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity
        {
            var entities = await Set<T>().Where(predicate).ToListAsync();
            
            foreach (var entity in entities)
            {
                entity.SoftDelete();
            }
            
            return await base.SaveChangesAsync();
        }
    }
}
