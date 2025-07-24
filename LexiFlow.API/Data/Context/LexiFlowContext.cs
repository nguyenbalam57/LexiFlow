using Microsoft.EntityFrameworkCore;
using System.DirectoryServices.ActiveDirectory;
using LexiFlow.Models;

namespace LexiFlow.API.Data.Context
{
    public class LexiFlowContext : DbContext
    {
        public LexiFlowContext(DbContextOptions<LexiFlowContext> options) : base(options) { }

        // User Management
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<RolePermission> RolePermissions { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<Team> Teams { get; set; } = null!;
        public DbSet<UserTeam> UserTeams { get; set; } = null!;
        public DbSet<UserPermission> UserPermissions { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;

        // Vocabulary Management
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<VocabularyGroup> VocabularyGroups { get; set; } = null!;
        public DbSet<Vocabulary> Vocabularies { get; set; } = null!;
        public DbSet<VocabularyCategory> VocabularyCategories { get; set; } = null!;
        public DbSet<Kanji> Kanjis { get; set; } = null!;
        public DbSet<KanjiVocabulary> KanjiVocabularies { get; set; } = null!;
        public DbSet<Grammar> Grammars { get; set; } = null!;
        public DbSet<GrammarExample> GrammarExamples { get; set; } = null!;
        public DbSet<TechnicalTerm> TechnicalTerms { get; set; } = null!;
        public DbSet<UserPersonalVocabulary> UserPersonalVocabularies { get; set; } = null!;
        public DbSet<KanjiComponent> KanjiComponents { get; set; } = null!;
        public DbSet<KanjiComponentMapping> KanjiComponentMappings { get; set; } = null!;

        // Learning Progress
        public DbSet<LearningProgress> LearningProgresses { get; set; } = null!;
        public DbSet<UserKanjiProgress> UserKanjiProgresses { get; set; } = null!;
        public DbSet<UserGrammarProgress> UserGrammarProgresses { get; set; } = null!;
        public DbSet<PersonalWordList> PersonalWordLists { get; set; } = null!;
        public DbSet<PersonalWordListItem> PersonalWordListItems { get; set; } = null!;

        // JLPT Exam Management
        public DbSet<JLPTLevel> JLPTLevels { get; set; } = null!;
        public DbSet<JLPTExam> JLPTExams { get; set; } = null!;
        public DbSet<JLPTSection> JLPTSections { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<QuestionOption> QuestionOptions { get; set; } = null!;

        // Test and Practice
        public DbSet<TestResult> TestResults { get; set; } = null!;
        public DbSet<TestDetail> TestDetails { get; set; } = null!;
        public DbSet<CustomExam> CustomExams { get; set; } = null!;
        public DbSet<UserExam> UserExams { get; set; } = null!;
        public DbSet<UserAnswer> UserAnswers { get; set; } = null!;
        public DbSet<PracticeSet> PracticeSets { get; set; } = null!;
        public DbSet<PracticeSetItem> PracticeSetItems { get; set; } = null!;
        public DbSet<UserPracticeSet> UserPracticeSets { get; set; } = null!;
        public DbSet<UserPracticeAnswer> UserPracticeAnswers { get; set; } = null!;

        // Study Planning
        public DbSet<StudyPlan> StudyPlans { get; set; } = null!;
        public DbSet<StudyTopic> StudyTopics { get; set; } = null!;
        public DbSet<StudyGoal> StudyGoals { get; set; } = null!;
        public DbSet<StudyTask> StudyTasks { get; set; } = null!;

        // Notification System
        public DbSet<NotificationType> NotificationTypes { get; set; } = null!;
        public DbSet<NotificationPriority> NotificationPriorities { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<NotificationStatuse> NotificationStatuses { get; set; } = null!;
        public DbSet<NotificationRecipient> NotificationRecipients { get; set; } = null!;

        // Gamification
        public DbSet<Level> Levels { get; set; } = null!;
        public DbSet<UserLevel> UserLevels { get; set; } = null!;
        public DbSet<Badge> Badges { get; set; } = null!;
        public DbSet<UserBadge> UserBadges { get; set; } = null!;
        public DbSet<Challenge> Challenges { get; set; } = null!;
        public DbSet<UserChallenge> UserChallenges { get; set; } = null!;
        public DbSet<Achievement> Achievements { get; set; } = null!;
        public DbSet<UserAchievement> UserAchievements { get; set; } = null!;
        public DbSet<UserStreak> UserStreaks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Table names
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<Permission>().ToTable("Permissions");
            modelBuilder.Entity<RolePermission>().ToTable("RolePermissions");
            modelBuilder.Entity<Department>().ToTable("Departments");
            modelBuilder.Entity<Team>().ToTable("Teams");
            modelBuilder.Entity<UserTeam>().ToTable("UserTeams");
            modelBuilder.Entity<UserPermission>().ToTable("UserPermissions");
            modelBuilder.Entity<Group>().ToTable("Groups");

            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<VocabularyGroup>().ToTable("VocabularyGroups");
            modelBuilder.Entity<Vocabulary>().ToTable("Vocabulary");
            modelBuilder.Entity<VocabularyCategory>().ToTable("VocabularyCategories");
            modelBuilder.Entity<Kanji>().ToTable("Kanji");
            modelBuilder.Entity<KanjiVocabulary>().ToTable("KanjiVocabulary");
            modelBuilder.Entity<Grammar>().ToTable("Grammar");
            modelBuilder.Entity<GrammarExample>().ToTable("GrammarExamples");
            modelBuilder.Entity<TechnicalTerm>().ToTable("TechnicalTerms");
            modelBuilder.Entity<UserPersonalVocabulary>().ToTable("UserPersonalVocabulary");
            modelBuilder.Entity<KanjiComponent>().ToTable("KanjiComponents");
            modelBuilder.Entity<KanjiComponentMapping>().ToTable("KanjiComponentMapping");

            modelBuilder.Entity<LearningProgress>().ToTable("LearningProgress");
            modelBuilder.Entity<UserKanjiProgress>().ToTable("UserKanjiProgress");
            modelBuilder.Entity<UserGrammarProgress>().ToTable("UserGrammarProgress");
            modelBuilder.Entity<PersonalWordList>().ToTable("PersonalWordLists");
            modelBuilder.Entity<PersonalWordListItem>().ToTable("PersonalWordListItems");

            // Relationships example for User Management
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserID);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleID);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Department_Navigation)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentID);

            // Configure RowVersion concurrency token
            modelBuilder.Entity<User>().Property(u => u.RowVersion).IsRowVersion();
            modelBuilder.Entity<Role>().Property(r => r.RowVersion).IsRowVersion();
            modelBuilder.Entity<Vocabulary>().Property(v => v.RowVersion).IsRowVersion();
            // Similar config for other entities with RowVersion

            // Unique constraints
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<Role>().HasIndex(r => r.RoleName).IsUnique();
            modelBuilder.Entity<Permission>().HasIndex(p => p.PermissionName).IsUnique();
            modelBuilder.Entity<Kanji>().HasIndex(k => k.Character).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}