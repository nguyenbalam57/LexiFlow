using LexiFlow.API.Data.Repositories;
using LexiFlow.Models;

namespace LexiFlow.API.Data.UnitOfWork
{
    /// <summary>
    /// Unit of Work interface defining all repositories and save changes method
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        // User Management
        IRepository<User> Users { get; }
        IRepository<Role> Roles { get; }
        IRepository<UserRole> UserRoles { get; }
        IRepository<Permission> Permissions { get; }
        IRepository<RolePermission> RolePermissions { get; }
        IRepository<Department> Departments { get; }
        IRepository<Team> Teams { get; }
        IRepository<UserTeam> UserTeams { get; }
        IRepository<UserPermission> UserPermissions { get; }
        IRepository<Group> Groups { get; }

        // Vocabulary Management
        IRepository<Category> Categories { get; }
        IRepository<VocabularyGroup> VocabularyGroups { get; }
        IRepository<Vocabulary> Vocabularies { get; }
        IRepository<VocabularyCategory> VocabularyCategories { get; }
        IRepository<Kanji> Kanjis { get; }
        IRepository<KanjiVocabulary> KanjiVocabularies { get; }
        IRepository<Grammar> Grammars { get; }
        IRepository<GrammarExample> GrammarExamples { get; }
        IRepository<TechnicalTerm> TechnicalTerms { get; }
        IRepository<UserPersonalVocabulary> UserPersonalVocabularies { get; }
        IRepository<KanjiComponent> KanjiComponents { get; }
        IRepository<KanjiComponentMapping> KanjiComponentMappings { get; }

        // Learning Progress
        IRepository<LearningProgress> LearningProgresses { get; }
        IRepository<UserKanjiProgress> UserKanjiProgresses { get; }
        IRepository<UserGrammarProgress> UserGrammarProgresses { get; }
        IRepository<PersonalWordList> PersonalWordLists { get; }
        IRepository<PersonalWordListItem> PersonalWordListItems { get; }

        // JLPT Exam Management
        IRepository<JLPTLevel> JLPTLevels { get; }
        IRepository<JLPTExam> JLPTExams { get; }
        IRepository<JLPTSection> JLPTSections { get; }
        IRepository<Question> Questions { get; }
        IRepository<QuestionOption> QuestionOptions { get; }

        // Test and Practice
        IRepository<TestResult> TestResults { get; }
        IRepository<TestDetail> TestDetails { get; }
        IRepository<CustomExam> CustomExams { get; }
        IRepository<UserExam> UserExams { get; }
        IRepository<UserAnswer> UserAnswers { get; }
        IRepository<PracticeSet> PracticeSets { get; }
        IRepository<PracticeSetItem> PracticeSetItems { get; }
        IRepository<UserPracticeSet> UserPracticeSets { get; }
        IRepository<UserPracticeAnswer> UserPracticeAnswers { get; }

        // Study Planning
        IRepository<StudyPlan> StudyPlans { get; }
        IRepository<StudyTopic> StudyTopics { get; }
        IRepository<StudyGoal> StudyGoals { get; }
        IRepository<StudyTask> StudyTasks { get; }

        // Notification System
        IRepository<NotificationType> NotificationTypes { get; }
        IRepository<NotificationPriority> NotificationPriorities { get; }
        IRepository<Notification> Notifications { get; }
        IRepository<NotificationStatus> NotificationStatuses { get; }
        IRepository<NotificationRecipient> NotificationRecipients { get; }
        IRepository<NotificationResponse> NotificationResponses { get; }

        // Gamification
        IRepository<Level> Levels { get; }
        IRepository<UserLevel> UserLevels { get; }
        IRepository<Badge> Badges { get; }
        IRepository<UserBadge> UserBadges { get; }
        IRepository<Challenge> Challenges { get; }
        IRepository<UserChallenge> UserChallenges { get; }
        IRepository<Achievement> Achievements { get; }
        IRepository<UserAchievement> UserAchievements { get; }
        IRepository<UserStreak> UserStreaks { get; }

        /// <summary>
        /// Save all changes made through the repositories
        /// </summary>
        /// <returns>Number of affected rows</returns>
        Task<int> SaveChangesAsync();
    }
}