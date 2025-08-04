using LexiFlow.Infrastructure.Data.Repositories.Base;
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
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Infrastructure.Data.UnitOfWork
{
    /// <summary>
    /// Unit of Work interface defining all repositories and save changes method
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        #region User Management
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
        IRepository<UserGroup> UserGroups { get; }
        IRepository<GroupPermission> GroupPermissions { get; }
        IRepository<PermissionGroup> PermissionGroups { get; }
        IRepository<PermissionGroupMapping> PermissionGroupMappings { get; }
        IRepository<UserProfile> UserProfiles { get; }
        IRepository<UserLearningPreference> UserLearningPreferences { get; }
        IRepository<UserNotificationSetting> UserNotificationSettings { get; }
        #endregion

        #region Learning Content
        IRepository<Category> Categories { get; }
        IRepository<VocabularyGroup> VocabularyGroups { get; }
        IRepository<Vocabulary> Vocabularies { get; }
        IRepository<Definition> Definitions { get; }
        IRepository<Example> Examples { get; }
        IRepository<Translation> Translations { get; }
        IRepository<Kanji> Kanjis { get; }
        IRepository<KanjiVocabulary> KanjiVocabularies { get; }
        IRepository<KanjiMeaning> KanjiMeanings { get; }
        IRepository<KanjiExample> KanjiExamples { get; }
        IRepository<KanjiExampleMeaning> KanjiExampleMeanings { get; }
        IRepository<KanjiComponent> KanjiComponents { get; }
        IRepository<KanjiComponentMapping> KanjiComponentMappings { get; }
        IRepository<Grammar> Grammars { get; }
        IRepository<GrammarDefinition> GrammarDefinitions { get; }
        IRepository<GrammarExample> GrammarExamples { get; }
        IRepository<GrammarTranslation> GrammarTranslations { get; }
        IRepository<TechnicalTerm> TechnicalTerms { get; }
        IRepository<TermExample> TermExamples { get; }
        IRepository<TermTranslation> TermTranslations { get; }
        IRepository<TermRelation> TermRelations { get; }
        IRepository<UserTechnicalTerm> UserTechnicalTerms { get; }
        IRepository<GroupVocabularyRelation> VocabularyRelations { get; }
        #endregion

        #region Media
        IRepository<MediaFile> MediaFiles { get; }
        IRepository<MediaCategory> MediaCategories { get; }
        IRepository<MediaProcessingHistory> MediaProcessingHistories { get; }
        #endregion

        #region Progress Tracking
        IRepository<LearningProgress> LearningProgresses { get; }
        IRepository<LearningSession> LearningSessions { get; }
        IRepository<LearningSessionDetail> SessionDetails { get; }
        IRepository<UserKanjiProgress> UserKanjiProgresses { get; }
        IRepository<UserGrammarProgress> UserGrammarProgresses { get; }
        IRepository<GoalProgress> GoalProgresses { get; }
        IRepository<PersonalWordList> PersonalWordLists { get; }
        IRepository<PersonalWordListItem> PersonalWordListItems { get; }
        #endregion

        #region JLPT Exam Management
        IRepository<JLPTLevel> JLPTLevels { get; }
        IRepository<JLPTExam> JLPTExams { get; }
        IRepository<JLPTSection> JLPTSections { get; }
        IRepository<Question> Questions { get; }
        IRepository<QuestionOption> QuestionOptions { get; }
        #endregion

        #region Test and Practice
        IRepository<TestResult> TestResults { get; }
        IRepository<TestDetail> TestDetails { get; }
        IRepository<CustomExam> CustomExams { get; }
        IRepository<CustomExamQuestion> CustomExamQuestions { get; }
        IRepository<UserExam> UserExams { get; }
        IRepository<UserAnswer> UserAnswers { get; }
        IRepository<PracticeSet> PracticeSets { get; }
        IRepository<PracticeSetItem> PracticeSetItems { get; }
        IRepository<UserPracticeSet> UserPracticeSets { get; }
        IRepository<UserPracticeAnswer> UserPracticeAnswers { get; }
        #endregion

        #region Study Planning
        IRepository<StudyGoal> StudyGoals { get; }
        IRepository<StudyPlan> StudyPlans { get; }
        IRepository<StudySession> StudySessions { get; }
        IRepository<StudyTask> StudyTasks { get; }
        IRepository<StudyTopic> StudyTopics { get; }
        IRepository<StudyPlanItem> StudyPlanItems { get; }
        IRepository<StudyPlanProgress> StudyPlanProgresses { get; }
        IRepository<TaskCompletion> TaskCompletions { get; }
        #endregion

        #region Notification System
        IRepository<NotificationType> NotificationTypes { get; }
        IRepository<NotificationPriority> NotificationPriorities { get; }
        IRepository<Notification> Notifications { get; }
        IRepository<NotificationStatus> NotificationStatuses { get; }
        IRepository<NotificationRecipient> NotificationRecipients { get; }
        IRepository<NotificationResponse> NotificationResponses { get; }
        #endregion

        #region Scheduling
        IRepository<Schedule> Schedules { get; }
        IRepository<ScheduleItem> ScheduleItems { get; }
        IRepository<ScheduleItemType> ScheduleItemTypes { get; }
        IRepository<ScheduleRecurrence> ScheduleRecurrences { get; }
        IRepository<ScheduleItemParticipant> ScheduleItemParticipants { get; }
        IRepository<ScheduleReminder> ScheduleReminders { get; }
        #endregion

        #region Analytics
        IRepository<StudyReport> StudyReports { get; }
        IRepository<StudyReportItem> StudyReportItems { get; }
        IRepository<ReportType> ReportTypes { get; }
        IRepository<ExamAnalytic> ExamAnalytics { get; }
        IRepository<PracticeAnalytic> PracticeAnalytics { get; }
        IRepository<StrengthWeakness> StrengthWeaknesses { get; }
        #endregion

        #region Gamification
        IRepository<Level> Levels { get; }
        IRepository<UserLevel> UserLevels { get; }
        IRepository<PointType> PointTypes { get; }
        IRepository<UserPoint> UserPoints { get; }
        IRepository<Badge> Badges { get; }
        IRepository<UserBadge> UserBadges { get; }
        IRepository<Challenge> Challenges { get; }
        IRepository<ChallengeRequirement> ChallengeRequirements { get; }
        IRepository<UserChallenge> UserChallenges { get; }
        IRepository<DailyTask> DailyTasks { get; }
        IRepository<DailyTaskRequirement> DailyTaskRequirements { get; }
        IRepository<UserDailyTask> UserDailyTasks { get; }
        IRepository<Achievement> Achievements { get; }
        IRepository<AchievementRequirement> AchievementRequirements { get; }
        IRepository<UserAchievement> UserAchievements { get; }
        IRepository<Leaderboard> Leaderboards { get; }
        IRepository<LeaderboardEntry> LeaderboardEntries { get; }
        IRepository<Event> Events { get; }
        IRepository<UserEvent> UserEvents { get; }
        IRepository<UserGift> UserGifts { get; }
        IRepository<UserStreak> UserStreaks { get; }
        #endregion

        #region Submission
        IRepository<UserVocabularySubmission> UserVocabularySubmissions { get; }
        IRepository<UserVocabularyDetail> UserVocabularyDetails { get; }
        IRepository<SubmissionStatus> SubmissionStatuses { get; }
        IRepository<StatusTransition> StatusTransitions { get; }
        IRepository<ApprovalHistory> ApprovalHistories { get; }
        #endregion

        #region Synchronization
        IRepository<SyncMetadata> SyncMetadata { get; }
        IRepository<SyncConflict> SyncConflicts { get; }
        IRepository<DeletedItem> DeletedItems { get; }
        #endregion

        #region System
        IRepository<Setting> Settings { get; }
        IRepository<ActivityLog> ActivityLogs { get; }
        IRepository<SyncLog> SyncLogs { get; }
        IRepository<ErrorLog> ErrorLogs { get; }
        IRepository<PerformanceLog> PerformanceLogs { get; }
        #endregion

        /// <summary>
        /// Save all changes made through the repositories
        /// </summary>
        /// <returns>Number of affected rows</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Begin a new transaction
        /// </summary>
        /// <returns>Database transaction</returns>
        Task<IDbContextTransaction> BeginTransactionAsync();

        /// <summary>
        /// Get a queryable collection of entities from the repository
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>Queryable collection</returns>
        IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class;
    }
}