using LexiFlow.Infrastructure.Data.Repositories.Base;
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
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LexiFlow.Infrastructure.Data.UnitOfWork
{
    /// <summary>
    /// Unit of Work implementation with EF Core - Optimized for .NET 9
    /// Removed unused models (Analytics, Gamification, Sync, Submission, System Logs)
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LexiFlowContext _context;
        private bool _disposed = false;

        #region User Management Repositories
        private IRepository<User>? _users;
        private IRepository<Role>? _roles;
        private IRepository<UserRole>? _userRoles;
        private IRepository<Permission>? _permissions;
        private IRepository<RolePermission>? _rolePermissions;
        private IRepository<Department>? _departments;
        private IRepository<Team>? _teams;
        private IRepository<UserTeam>? _userTeams;
        private IRepository<UserPermission>? _userPermissions;
        private IRepository<Group>? _groups;
        private IRepository<UserGroup>? _userGroups;
        private IRepository<GroupPermission>? _groupPermissions;
        private IRepository<PermissionGroup>? _permissionGroups;
        private IRepository<PermissionGroupMapping>? _permissionGroupMappings;
        private IRepository<UserProfile>? _userProfiles;
        private IRepository<UserLearningPreference>? _userLearningPreferences;
        private IRepository<UserNotificationSetting>? _userNotificationSettings;
        #endregion

        #region Learning Content Repositories
        private IRepository<Category>? _categories;
        private IRepository<VocabularyGroup>? _vocabularyGroups;
        private IRepository<Vocabulary>? _vocabularies;
        private IRepository<Definition>? _definitions;
        private IRepository<Example>? _examples;
        private IRepository<Translation>? _translations;
        private IRepository<Kanji>? _kanjis;
        private IRepository<KanjiVocabulary>? _kanjiVocabularies;
        private IRepository<KanjiMeaning>? _kanjiMeanings;
        private IRepository<KanjiExample>? _kanjiExamples;
        private IRepository<KanjiExampleMeaning>? _kanjiExampleMeanings;
        private IRepository<KanjiComponent>? _kanjiComponents;
        private IRepository<KanjiComponentMapping>? _kanjiComponentMappings;
        private IRepository<Grammar>? _grammars;
        private IRepository<GrammarDefinition>? _grammarDefinitions;
        private IRepository<GrammarExample>? _grammarExamples;
        private IRepository<GrammarTranslation>? _grammarTranslations;
        private IRepository<TechnicalTerm>? _technicalTerms;
        private IRepository<TermExample>? _termExamples;
        private IRepository<TermTranslation>? _termTranslations;
        private IRepository<TermRelation>? _termRelations;
        private IRepository<UserTechnicalTerm>? _userTechnicalTerms;
        private IRepository<GroupVocabularyRelation>? _vocabularyRelations;
        #endregion

        #region Media Repositories
        private IRepository<MediaFile>? _mediaFiles;
        private IRepository<MediaCategory>? _mediaCategories;
        private IRepository<MediaProcessingHistory>? _mediaProcessingHistories;
        #endregion

        #region Progress Tracking Repositories
        private IRepository<LearningProgress>? _learningProgresses;
        private IRepository<LearningSession>? _learningSessions;
        private IRepository<LearningSessionDetail>? _sessionDetails;
        private IRepository<UserKanjiProgress>? _userKanjiProgresses;
        private IRepository<UserGrammarProgress>? _userGrammarProgresses;
        private IRepository<GoalProgress>? _goalProgresses;
        private IRepository<PersonalWordList>? _personalWordLists;
        private IRepository<PersonalWordListItem>? _personalWordListItems;
        #endregion

        #region JLPT Exam Management Repositories
        private IRepository<JLPTLevel>? _jlptLevels;
        private IRepository<JLPTExam>? _jlptExams;
        private IRepository<JLPTSection>? _jlptSections;
        private IRepository<Question>? _questions;
        private IRepository<QuestionOption>? _questionOptions;
        #endregion

        #region Test and Practice Repositories - Simplified
        private IRepository<TestResult>? _testResults;
        private IRepository<TestDetail>? _testDetails;
        private IRepository<UserExam>? _userExams;
        private IRepository<UserAnswer>? _userAnswers;
        #endregion

        #region Study Planning Repositories
        private IRepository<StudyGoal>? _studyGoals;
        private IRepository<StudyPlan>? _studyPlans;
        private IRepository<StudySession>? _studySessions;
        private IRepository<StudyTask>? _studyTasks;
        private IRepository<StudyTopic>? _studyTopics;
        private IRepository<StudyPlanItem>? _studyPlanItems;
        private IRepository<StudyPlanProgress>? _studyPlanProgresses;
        private IRepository<TaskCompletion>? _taskCompletions;
        #endregion

        #region Notification System Repositories
        private IRepository<NotificationType>? _notificationTypes;
        private IRepository<NotificationPriority>? _notificationPriorities;
        private IRepository<Notification>? _notifications;
        private IRepository<NotificationStatus>? _notificationStatuses;
        private IRepository<NotificationRecipient>? _notificationRecipients;
        private IRepository<NotificationResponse>? _notificationResponses;
        #endregion

        #region Scheduling Repositories
        private IRepository<Schedule>? _schedules;
        private IRepository<ScheduleItem>? _scheduleItems;
        private IRepository<ScheduleItemType>? _scheduleItemTypes;
        private IRepository<ScheduleRecurrence>? _scheduleRecurrences;
        private IRepository<ScheduleItemParticipant>? _scheduleItemParticipants;
        private IRepository<ScheduleReminder>? _scheduleReminders;
        #endregion

        #region System Repositories - Minimal
        private IRepository<Setting>? _settings;
        #endregion

        public UnitOfWork(LexiFlowContext context)
        {
            _context = context;
        }

        #region User Management Properties
        public IRepository<User> Users => _users ??= new FullFeaturedRepository<User>(_context);
        public IRepository<Role> Roles => _roles ??= new ActivatableRepository<Role>(_context);
        public IRepository<UserRole> UserRoles => _userRoles ??= new BaseRepository<UserRole>(_context);
        public IRepository<Permission> Permissions => _permissions ??= new BaseRepository<Permission>(_context);
        public IRepository<RolePermission> RolePermissions => _rolePermissions ??= new BaseRepository<RolePermission>(_context);
        public IRepository<Department> Departments => _departments ??= new ActivatableRepository<Department>(_context);
        public IRepository<Team> Teams => _teams ??= new ActivatableRepository<Team>(_context);
        public IRepository<UserTeam> UserTeams => _userTeams ??= new BaseRepository<UserTeam>(_context);
        public IRepository<UserPermission> UserPermissions => _userPermissions ??= new BaseRepository<UserPermission>(_context);
        public IRepository<Group> Groups => _groups ??= new ActivatableRepository<Group>(_context);
        public IRepository<UserGroup> UserGroups => _userGroups ??= new BaseRepository<UserGroup>(_context);
        public IRepository<GroupPermission> GroupPermissions => _groupPermissions ??= new BaseRepository<GroupPermission>(_context);
        public IRepository<PermissionGroup> PermissionGroups => _permissionGroups ??= new ActivatableRepository<PermissionGroup>(_context);
        public IRepository<PermissionGroupMapping> PermissionGroupMappings => _permissionGroupMappings ??= new BaseRepository<PermissionGroupMapping>(_context);
        public IRepository<UserProfile> UserProfiles => _userProfiles ??= new BaseRepository<UserProfile>(_context);
        public IRepository<UserLearningPreference> UserLearningPreferences => _userLearningPreferences ??= new BaseRepository<UserLearningPreference>(_context);
        public IRepository<UserNotificationSetting> UserNotificationSettings => _userNotificationSettings ??= new BaseRepository<UserNotificationSetting>(_context);
        #endregion

        #region Learning Content Properties
        public IRepository<Category> Categories => _categories ??= new ActivatableRepository<Category>(_context);
        public IRepository<VocabularyGroup> VocabularyGroups => _vocabularyGroups ??= new ActivatableRepository<VocabularyGroup>(_context);
        public IRepository<Vocabulary> Vocabularies => _vocabularies ??= new SoftDeleteRepository<Vocabulary>(_context);
        public IRepository<Definition> Definitions => _definitions ??= new BaseRepository<Definition>(_context);
        public IRepository<Example> Examples => _examples ??= new BaseRepository<Example>(_context);
        public IRepository<Translation> Translations => _translations ??= new BaseRepository<Translation>(_context);
        public IRepository<Kanji> Kanjis => _kanjis ??= new SoftDeleteRepository<Kanji>(_context);
        public IRepository<KanjiVocabulary> KanjiVocabularies => _kanjiVocabularies ??= new BaseRepository<KanjiVocabulary>(_context);
        public IRepository<KanjiMeaning> KanjiMeanings => _kanjiMeanings ??= new BaseRepository<KanjiMeaning>(_context);
        public IRepository<KanjiExample> KanjiExamples => _kanjiExamples ??= new BaseRepository<KanjiExample>(_context);
        public IRepository<KanjiExampleMeaning> KanjiExampleMeanings => _kanjiExampleMeanings ??= new BaseRepository<KanjiExampleMeaning>(_context);
        public IRepository<KanjiComponent> KanjiComponents => _kanjiComponents ??= new BaseRepository<KanjiComponent>(_context);
        public IRepository<KanjiComponentMapping> KanjiComponentMappings => _kanjiComponentMappings ??= new BaseRepository<KanjiComponentMapping>(_context);
        public IRepository<Grammar> Grammars => _grammars ??= new SoftDeleteRepository<Grammar>(_context);
        public IRepository<GrammarDefinition> GrammarDefinitions => _grammarDefinitions ??= new BaseRepository<GrammarDefinition>(_context);
        public IRepository<GrammarExample> GrammarExamples => _grammarExamples ??= new BaseRepository<GrammarExample>(_context);
        public IRepository<GrammarTranslation> GrammarTranslations => _grammarTranslations ??= new BaseRepository<GrammarTranslation>(_context);
        public IRepository<TechnicalTerm> TechnicalTerms => _technicalTerms ??= new SoftDeleteRepository<TechnicalTerm>(_context);
        public IRepository<TermExample> TermExamples => _termExamples ??= new BaseRepository<TermExample>(_context);
        public IRepository<TermTranslation> TermTranslations => _termTranslations ??= new BaseRepository<TermTranslation>(_context);
        public IRepository<TermRelation> TermRelations => _termRelations ??= new BaseRepository<TermRelation>(_context);
        public IRepository<UserTechnicalTerm> UserTechnicalTerms => _userTechnicalTerms ??= new BaseRepository<UserTechnicalTerm>(_context);
        public IRepository<GroupVocabularyRelation> VocabularyRelations => _vocabularyRelations ??= new BaseRepository<GroupVocabularyRelation>(_context);
        #endregion

        #region Media Properties
        public IRepository<MediaFile> MediaFiles => _mediaFiles ??= new SoftDeleteRepository<MediaFile>(_context);
        public IRepository<MediaCategory> MediaCategories => _mediaCategories ??= new ActivatableRepository<MediaCategory>(_context);
        public IRepository<MediaProcessingHistory> MediaProcessingHistories => _mediaProcessingHistories ??= new BaseRepository<MediaProcessingHistory>(_context);
        #endregion

        #region Progress Tracking Properties
        public IRepository<LearningProgress> LearningProgresses => _learningProgresses ??= new BaseRepository<LearningProgress>(_context);
        public IRepository<LearningSession> LearningSessions => _learningSessions ??= new BaseRepository<LearningSession>(_context);
        public IRepository<LearningSessionDetail> SessionDetails => _sessionDetails ??= new BaseRepository<LearningSessionDetail>(_context);
        public IRepository<UserKanjiProgress> UserKanjiProgresses => _userKanjiProgresses ??= new BaseRepository<UserKanjiProgress>(_context);
        public IRepository<UserGrammarProgress> UserGrammarProgresses => _userGrammarProgresses ??= new BaseRepository<UserGrammarProgress>(_context);
        public IRepository<GoalProgress> GoalProgresses => _goalProgresses ??= new BaseRepository<GoalProgress>(_context);
        public IRepository<PersonalWordList> PersonalWordLists => _personalWordLists ??= new BaseRepository<PersonalWordList>(_context);
        public IRepository<PersonalWordListItem> PersonalWordListItems => _personalWordListItems ??= new BaseRepository<PersonalWordListItem>(_context);
        #endregion

        #region JLPT Exam Management Properties
        public IRepository<JLPTLevel> JLPTLevels => _jlptLevels ??= new BaseRepository<JLPTLevel>(_context);
        public IRepository<JLPTExam> JLPTExams => _jlptExams ??= new ActivatableRepository<JLPTExam>(_context);
        public IRepository<JLPTSection> JLPTSections => _jlptSections ??= new BaseRepository<JLPTSection>(_context);
        public IRepository<Question> Questions => _questions ??= new ActivatableRepository<Question>(_context);
        public IRepository<QuestionOption> QuestionOptions => _questionOptions ??= new BaseRepository<QuestionOption>(_context);
        #endregion

        #region Test and Practice Properties - Simplified
        public IRepository<TestResult> TestResults => _testResults ??= new BaseRepository<TestResult>(_context);
        public IRepository<TestDetail> TestDetails => _testDetails ??= new BaseRepository<TestDetail>(_context);
        public IRepository<UserExam> UserExams => _userExams ??= new BaseRepository<UserExam>(_context);
        public IRepository<UserAnswer> UserAnswers => _userAnswers ??= new BaseRepository<UserAnswer>(_context);
        #endregion

        #region Study Planning Properties
        public IRepository<StudyGoal> StudyGoals => _studyGoals ??= new BaseRepository<StudyGoal>(_context);
        public IRepository<StudyPlan> StudyPlans => _studyPlans ??= new ActivatableRepository<StudyPlan>(_context);
        public IRepository<StudySession> StudySessions => _studySessions ??= new BaseRepository<StudySession>(_context);
        public IRepository<StudyTask> StudyTasks => _studyTasks ??= new BaseRepository<StudyTask>(_context);
        public IRepository<StudyTopic> StudyTopics => _studyTopics ??= new BaseRepository<StudyTopic>(_context);
        public IRepository<StudyPlanItem> StudyPlanItems => _studyPlanItems ??= new BaseRepository<StudyPlanItem>(_context);
        public IRepository<StudyPlanProgress> StudyPlanProgresses => _studyPlanProgresses ??= new BaseRepository<StudyPlanProgress>(_context);
        public IRepository<TaskCompletion> TaskCompletions => _taskCompletions ??= new BaseRepository<TaskCompletion>(_context);
        #endregion

        #region Notification System Properties
        public IRepository<NotificationType> NotificationTypes => _notificationTypes ??= new BaseRepository<NotificationType>(_context);
        public IRepository<NotificationPriority> NotificationPriorities => _notificationPriorities ??= new BaseRepository<NotificationPriority>(_context);
        public IRepository<Notification> Notifications => _notifications ??= new BaseRepository<Notification>(_context);
        public IRepository<NotificationStatus> NotificationStatuses => _notificationStatuses ??= new BaseRepository<NotificationStatus>(_context);
        public IRepository<NotificationRecipient> NotificationRecipients => _notificationRecipients ??= new BaseRepository<NotificationRecipient>(_context);
        public IRepository<NotificationResponse> NotificationResponses => _notificationResponses ??= new BaseRepository<NotificationResponse>(_context);
        #endregion

        #region Scheduling Properties
        public IRepository<Schedule> Schedules => _schedules ??= new ActivatableRepository<Schedule>(_context);
        public IRepository<ScheduleItem> ScheduleItems => _scheduleItems ??= new ActivatableRepository<ScheduleItem>(_context);
        public IRepository<ScheduleItemType> ScheduleItemTypes => _scheduleItemTypes ??= new BaseRepository<ScheduleItemType>(_context);
        public IRepository<ScheduleRecurrence> ScheduleRecurrences => _scheduleRecurrences ??= new BaseRepository<ScheduleRecurrence>(_context);
        public IRepository<ScheduleItemParticipant> ScheduleItemParticipants => _scheduleItemParticipants ??= new BaseRepository<ScheduleItemParticipant>(_context);
        public IRepository<ScheduleReminder> ScheduleReminders => _scheduleReminders ??= new BaseRepository<ScheduleReminder>(_context);
        #endregion

        #region System Properties - Minimal
        public IRepository<Setting> Settings => _settings ??= new BaseRepository<Setting>(_context);
        #endregion

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }
    }
}