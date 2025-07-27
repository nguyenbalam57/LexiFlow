using LexiFlow.API.Data.Context;
using LexiFlow.API.Data.Repositories;
using LexiFlow.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.DirectoryServices.ActiveDirectory;

namespace LexiFlow.API.Data.UnitOfWork
{
    /// <summary>
    /// Unit of Work implementation for managing transactions and repositories
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
        #endregion

        #region Vocabulary Management Repositories
        private IRepository<Category>? _categories;
        private IRepository<VocabularyGroup>? _vocabularyGroups;
        private IRepository<Vocabulary>? _vocabularies;
        private IRepository<Kanji>? _kanjis;
        private IRepository<KanjiVocabulary>? _kanjiVocabularies;
        private IRepository<Grammar>? _grammars;
        private IRepository<GrammarExample>? _grammarExamples;
        private IRepository<TechnicalTerm>? _technicalTerms;
        private IRepository<UserPersonalVocabulary>? _userPersonalVocabularies;
        private IRepository<KanjiComponent>? _kanjiComponents;
        private IRepository<KanjiComponentMapping>? _kanjiComponentMappings;
        private IRepository<KanjiMeaning>? _kanjiMeanings;
        private IRepository<KanjiExample>? _kanjiExamples;
        #endregion

        #region Learning Progress Repositories
        private IRepository<LearningProgress>? _learningProgresses;
        private IRepository<UserKanjiProgress>? _userKanjiProgresses;
        private IRepository<UserGrammarProgress>? _userGrammarProgresses;
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

        #region Test and Practice Repositories
        private IRepository<TestResult>? _testResults;
        private IRepository<TestDetail>? _testDetails;
        private IRepository<CustomExam>? _customExams;
        private IRepository<UserExam>? _userExams;
        private IRepository<UserAnswer>? _userAnswers;
        private IRepository<PracticeSet>? _practiceSets;
        private IRepository<PracticeSetItem>? _practiceSetItems;
        private IRepository<UserPracticeSet>? _userPracticeSets;
        private IRepository<UserPracticeAnswer>? _userPracticeAnswers;
        #endregion

        #region Study Planning Repositories
        private IRepository<StudyPlan>? _studyPlans;
        private IRepository<StudyTopic>? _studyTopics;
        private IRepository<StudyGoal>? _studyGoals;
        private IRepository<StudyTask>? _studyTasks;
        #endregion

        #region Notification System Repositories
        private IRepository<NotificationType>? _notificationTypes;
        private IRepository<NotificationPriority>? _notificationPriorities;
        private IRepository<Notification>? _notifications;
        private IRepository<NotificationStatuse>? _notificationStatuses;
        private IRepository<NotificationRecipient>? _notificationRecipients;
        private IRepository<NotificationResponse>? _notificationResponses;
        #endregion

        #region Gamification Repositories
        private IRepository<Level>? _levels;
        private IRepository<UserLevel>? _userLevels;
        private IRepository<Badge>? _badges;
        private IRepository<UserBadge>? _userBadges;
        private IRepository<Challenge>? _challenges;
        private IRepository<UserChallenge>? _userChallenges;
        private IRepository<Achievement>? _achievements;
        private IRepository<UserAchievement>? _userAchievements;
        private IRepository<UserStreak>? _userStreaks;
        #endregion

        public UnitOfWork(LexiFlowContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region User Management Properties
        public IRepository<User> Users => _users ??= new Repository<User>(_context);
        public IRepository<Role> Roles => _roles ??= new Repository<Role>(_context);
        public IRepository<UserRole> UserRoles => _userRoles ??= new Repository<UserRole>(_context);
        public IRepository<Permission> Permissions => _permissions ??= new Repository<Permission>(_context);
        public IRepository<RolePermission> RolePermissions => _rolePermissions ??= new Repository<RolePermission>(_context);
        public IRepository<Department> Departments => _departments ??= new Repository<Department>(_context);
        public IRepository<Team> Teams => _teams ??= new Repository<Team>(_context);
        public IRepository<UserTeam> UserTeams => _userTeams ??= new Repository<UserTeam>(_context);
        public IRepository<UserPermission> UserPermissions => _userPermissions ??= new Repository<UserPermission>(_context);
        public IRepository<Group> Groups => _groups ??= new Repository<Group>(_context);
        #endregion

        #region Vocabulary Management Properties
        public IRepository<Category> Categories => _categories ??= new Repository<Category>(_context);
        public IRepository<VocabularyGroup> VocabularyGroups => _vocabularyGroups ??= new Repository<VocabularyGroup>(_context);
        public IRepository<Vocabulary> Vocabularies => _vocabularies ??= new Repository<Vocabulary>(_context);
        public IRepository<Kanji> Kanjis => _kanjis ??= new Repository<Kanji>(_context);
        public IRepository<KanjiVocabulary> KanjiVocabularies => _kanjiVocabularies ??= new Repository<KanjiVocabulary>(_context);
        public IRepository<Grammar> Grammars => _grammars ??= new Repository<Grammar>(_context);
        public IRepository<GrammarExample> GrammarExamples => _grammarExamples ??= new Repository<GrammarExample>(_context);
        public IRepository<TechnicalTerm> TechnicalTerms => _technicalTerms ??= new Repository<TechnicalTerm>(_context);
        public IRepository<UserPersonalVocabulary> UserPersonalVocabularies => _userPersonalVocabularies ??= new Repository<UserPersonalVocabulary>(_context);
        public IRepository<KanjiComponent> KanjiComponents => _kanjiComponents ??= new Repository<KanjiComponent>(_context);
        public IRepository<KanjiComponentMapping> KanjiComponentMappings => _kanjiComponentMappings ??= new Repository<KanjiComponentMapping>(_context);
        public IRepository<KanjiMeaning> KanjiMeanings => _kanjiMeanings ??= new Repository<KanjiMeaning>(_context);
        public IRepository<KanjiExample> KanjiExamples => _kanjiExamples ??= new Repository<KanjiExample>(_context);
        #endregion

        #region Learning Progress Properties
        public IRepository<LearningProgress> LearningProgresses => _learningProgresses ??= new Repository<LearningProgress>(_context);
        public IRepository<UserKanjiProgress> UserKanjiProgresses => _userKanjiProgresses ??= new Repository<UserKanjiProgress>(_context);
        public IRepository<UserGrammarProgress> UserGrammarProgresses => _userGrammarProgresses ??= new Repository<UserGrammarProgress>(_context);
        public IRepository<PersonalWordList> PersonalWordLists => _personalWordLists ??= new Repository<PersonalWordList>(_context);
        public IRepository<PersonalWordListItem> PersonalWordListItems => _personalWordListItems ??= new Repository<PersonalWordListItem>(_context);
        #endregion

        #region JLPT Exam Management Properties
        public IRepository<JLPTLevel> JLPTLevels => _jlptLevels ??= new Repository<JLPTLevel>(_context);
        public IRepository<JLPTExam> JLPTExams => _jlptExams ??= new Repository<JLPTExam>(_context);
        public IRepository<JLPTSection> JLPTSections => _jlptSections ??= new Repository<JLPTSection>(_context);
        public IRepository<Question> Questions => _questions ??= new Repository<Question>(_context);
        public IRepository<QuestionOption> QuestionOptions => _questionOptions ??= new Repository<QuestionOption>(_context);
        #endregion

        #region Test and Practice Properties
        public IRepository<TestResult> TestResults => _testResults ??= new Repository<TestResult>(_context);
        public IRepository<TestDetail> TestDetails => _testDetails ??= new Repository<TestDetail>(_context);
        public IRepository<CustomExam> CustomExams => _customExams ??= new Repository<CustomExam>(_context);
        public IRepository<UserExam> UserExams => _userExams ??= new Repository<UserExam>(_context);
        public IRepository<UserAnswer> UserAnswers => _userAnswers ??= new Repository<UserAnswer>(_context);
        public IRepository<PracticeSet> PracticeSets => _practiceSets ??= new Repository<PracticeSet>(_context);
        public IRepository<PracticeSetItem> PracticeSetItems => _practiceSetItems ??= new Repository<PracticeSetItem>(_context);
        public IRepository<UserPracticeSet> UserPracticeSets => _userPracticeSets ??= new Repository<UserPracticeSet>(_context);
        public IRepository<UserPracticeAnswer> UserPracticeAnswers => _userPracticeAnswers ??= new Repository<UserPracticeAnswer>(_context);
        #endregion

        #region Study Planning Properties
        public IRepository<StudyPlan> StudyPlans => _studyPlans ??= new Repository<StudyPlan>(_context);
        public IRepository<StudyTopic> StudyTopics => _studyTopics ??= new Repository<StudyTopic>(_context);
        public IRepository<StudyGoal> StudyGoals => _studyGoals ??= new Repository<StudyGoal>(_context);
        public IRepository<StudyTask> StudyTasks => _studyTasks ??= new Repository<StudyTask>(_context);
        #endregion

        #region Notification System Properties
        public IRepository<NotificationType> NotificationTypes => _notificationTypes ??= new Repository<NotificationType>(_context);
        public IRepository<NotificationPriority> NotificationPriorities => _notificationPriorities ??= new Repository<NotificationPriority>(_context);
        public IRepository<Notification> Notifications => _notifications ??= new Repository<Notification>(_context);
        public IRepository<NotificationStatuse> NotificationStatuses => _notificationStatuses ??= new Repository<NotificationStatuse>(_context);
        public IRepository<NotificationRecipient> NotificationRecipients => _notificationRecipients ??= new Repository<NotificationRecipient>(_context);
        public IRepository<NotificationResponse> NotificationResponses => _notificationResponses ??= new Repository<NotificationResponse>(_context);
        #endregion

        #region Gamification Properties
        public IRepository<Level> Levels => _levels ??= new Repository<Level>(_context);
        public IRepository<UserLevel> UserLevels => _userLevels ??= new Repository<UserLevel>(_context);
        public IRepository<Badge> Badges => _badges ??= new Repository<Badge>(_context);
        public IRepository<UserBadge> UserBadges => _userBadges ??= new Repository<UserBadge>(_context);
        public IRepository<Challenge> Challenges => _challenges ??= new Repository<Challenge>(_context);
        public IRepository<UserChallenge> UserChallenges => _userChallenges ??= new Repository<UserChallenge>(_context);
        public IRepository<Achievement> Achievements => _achievements ??= new Repository<Achievement>(_context);
        public IRepository<UserAchievement> UserAchievements => _userAchievements ??= new Repository<UserAchievement>(_context);
        public IRepository<UserStreak> UserStreaks => _userStreaks ??= new Repository<UserStreak>(_context);
        #endregion

        /// <summary>
        /// Save all changes made through the repositories
        /// </summary>
        /// <returns>Number of affected rows</returns>
        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Log the concurrency exception
                throw new DbUpdateConcurrencyException("A concurrency error occurred while saving changes.", ex);
            }
            catch (DbUpdateException ex)
            {
                // Log the database update exception
                throw new DbUpdateException("An error occurred while saving changes to the database.", ex);
            }
            catch (Exception ex)
            {
                // Log the general exception
                throw new Exception("An error occurred while saving changes.", ex);
            }
        }

        /// <summary>
        /// Begin a new transaction
        /// </summary>
        /// <returns>Database transaction</returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Get a queryable collection of entities from the repository
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>Queryable collection</returns>
        public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>();
        }

        /// <summary>
        /// Dispose the context and resources
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        /// <summary>
        /// Dispose the context and resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}