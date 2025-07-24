using LexiFlow.API.Data.Context;
using LexiFlow.Models;
using LexiFlow.API.Data.Repositories;

namespace LexiFlow.API.Data.UnitOfWork
{
    /// <summary>
    /// Unit of Work implementation for managing transactions and repositories
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LexiFlowContext _context;
        private bool _disposed = false;

        // User Management repositories
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

        // Vocabulary Management repositories
        private IRepository<Category>? _categories;
        private IRepository<VocabularyGroup>? _vocabularyGroups;
        private IRepository<Vocabulary>? _vocabularies;
        private IRepository<VocabularyCategory>? _vocabularyCategories;
        private IRepository<Kanji>? _kanjis;
        private IRepository<KanjiVocabulary>? _kanjiVocabularies;
        private IRepository<Grammar>? _grammars;
        private IRepository<GrammarExample>? _grammarExamples;
        private IRepository<TechnicalTerm>? _technicalTerms;
        private IRepository<UserPersonalVocabulary>? _userPersonalVocabularies;
        private IRepository<KanjiComponent>? _kanjiComponents;
        private IRepository<KanjiComponentMapping>? _kanjiComponentMappings;

        // Learning Progress repositories
        private IRepository<LearningProgress>? _learningProgresses;
        private IRepository<UserKanjiProgress>? _userKanjiProgresses;
        private IRepository<UserGrammarProgress>? _userGrammarProgresses;
        private IRepository<PersonalWordList>? _personalWordLists;
        private IRepository<PersonalWordListItem>? _personalWordListItems;

        // JLPT Exam Management repositories
        private IRepository<JLPTLevel>? _jlptLevels;
        private IRepository<JLPTExam>? _jlptExams;
        private IRepository<JLPTSection>? _jlptSections;
        private IRepository<Question>? _questions;
        private IRepository<QuestionOption>? _questionOptions;

        // Test and Practice repositories
        private IRepository<TestResult>? _testResults;
        private IRepository<TestDetail>? _testDetails;
        private IRepository<CustomExam>? _customExams;
        private IRepository<UserExam>? _userExams;
        private IRepository<UserAnswer>? _userAnswers;
        private IRepository<PracticeSet>? _practiceSets;
        private IRepository<PracticeSetItem>? _practiceSetItems;
        private IRepository<UserPracticeSet>? _userPracticeSets;
        private IRepository<UserPracticeAnswer>? _userPracticeAnswers;

        // Study Planning repositories
        private IRepository<StudyPlan>? _studyPlans;
        private IRepository<StudyTopic>? _studyTopics;
        private IRepository<StudyGoal>? _studyGoals;
        private IRepository<StudyTask>? _studyTasks;

        // Notification System repositories
        private IRepository<NotificationType>? _notificationTypes;
        private IRepository<NotificationPriority>? _notificationPriorities;
        private IRepository<Notification>? _notifications;
        private IRepository<NotificationStatuse>? _notificationStatuses;
        private IRepository<NotificationRecipient>? _notificationRecipients;
        private IRepository<NotificationResponse>? _notificationResponses;

        // Gamification repositories
        private IRepository<Level>? _levels;
        private IRepository<UserLevel>? _userLevels;
        private IRepository<Badge>? _badges;
        private IRepository<UserBadge>? _userBadges;
        private IRepository<Challenge>? _challenges;
        private IRepository<UserChallenge>? _userChallenges;
        private IRepository<Achievement>? _achievements;
        private IRepository<UserAchievement>? _userAchievements;
        private IRepository<UserStreak>? _userStreaks;

        public UnitOfWork(LexiFlowContext context)
        {
            _context = context;
        }

        // User Management repository properties
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

        // Vocabulary Management repository properties
        public IRepository<Category> Categories => _categories ??= new Repository<Category>(_context);
        public IRepository<VocabularyGroup> VocabularyGroups => _vocabularyGroups ??= new Repository<VocabularyGroup>(_context);
        public IRepository<Vocabulary> Vocabularies => _vocabularies ??= new Repository<Vocabulary>(_context);
        public IRepository<VocabularyCategory> VocabularyCategories => _vocabularyCategories ??= new Repository<VocabularyCategory>(_context);
        public IRepository<Kanji> Kanjis => _kanjis ??= new Repository<Kanji>(_context);
        public IRepository<KanjiVocabulary> KanjiVocabularies => _kanjiVocabularies ??= new Repository<KanjiVocabulary>(_context);
        public IRepository<Grammar> Grammars => _grammars ??= new Repository<Grammar>(_context);
        public IRepository<GrammarExample> GrammarExamples => _grammarExamples ??= new Repository<GrammarExample>(_context);
        public IRepository<TechnicalTerm> TechnicalTerms => _technicalTerms ??= new Repository<TechnicalTerm>(_context);
        public IRepository<UserPersonalVocabulary> UserPersonalVocabularies => _userPersonalVocabularies ??= new Repository<UserPersonalVocabulary>(_context);
        public IRepository<KanjiComponent> KanjiComponents => _kanjiComponents ??= new Repository<KanjiComponent>(_context);
        public IRepository<KanjiComponentMapping> KanjiComponentMappings => _kanjiComponentMappings ??= new Repository<KanjiComponentMapping>(_context);

        // Learning Progress repository properties
        public IRepository<LearningProgress> LearningProgresses => _learningProgresses ??= new Repository<LearningProgress>(_context);
        public IRepository<UserKanjiProgress> UserKanjiProgresses => _userKanjiProgresses ??= new Repository<UserKanjiProgress>(_context);
        public IRepository<UserGrammarProgress> UserGrammarProgresses => _userGrammarProgresses ??= new Repository<UserGrammarProgress>(_context);
        public IRepository<PersonalWordList> PersonalWordLists => _personalWordLists ??= new Repository<PersonalWordList>(_context);
        public IRepository<PersonalWordListItem> PersonalWordListItems => _personalWordListItems ??= new Repository<PersonalWordListItem>(_context);

        // JLPT Exam Management repository properties
        public IRepository<JLPTLevel> JLPTLevels => _jlptLevels ??= new Repository<JLPTLevel>(_context);
        public IRepository<JLPTExam> JLPTExams => _jlptExams ??= new Repository<JLPTExam>(_context);
        public IRepository<JLPTSection> JLPTSections => _jlptSections ??= new Repository<JLPTSection>(_context);
        public IRepository<Question> Questions => _questions ??= new Repository<Question>(_context);
        public IRepository<QuestionOption> QuestionOptions => _questionOptions ??= new Repository<QuestionOption>(_context);

        // Test and Practice repository properties
        public IRepository<TestResult> TestResults => _testResults ??= new Repository<TestResult>(_context);
        public IRepository<TestDetail> TestDetails => _testDetails ??= new Repository<TestDetail>(_context);
        public IRepository<CustomExam> CustomExams => _customExams ??= new Repository<CustomExam>(_context);
        public IRepository<UserExam> UserExams => _userExams ??= new Repository<UserExam>(_context);
        public IRepository<UserAnswer> UserAnswers => _userAnswers ??= new Repository<UserAnswer>(_context);
        public IRepository<PracticeSet> PracticeSets => _practiceSets ??= new Repository<PracticeSet>(_context);
        public IRepository<PracticeSetItem> PracticeSetItems => _practiceSetItems ??= new Repository<PracticeSetItem>(_context);
        public IRepository<UserPracticeSet> UserPracticeSets => _userPracticeSets ??= new Repository<UserPracticeSet>(_context);
        public IRepository<UserPracticeAnswer> UserPracticeAnswers => _userPracticeAnswers ??= new Repository<UserPracticeAnswer>(_context);

        // Study Planning repository properties
        public IRepository<StudyPlan> StudyPlans => _studyPlans ??= new Repository<StudyPlan>(_context);
        public IRepository<StudyTopic> StudyTopics => _studyTopics ??= new Repository<StudyTopic>(_context);
        public IRepository<StudyGoal> StudyGoals => _studyGoals ??= new Repository<StudyGoal>(_context);
        public IRepository<StudyTask> StudyTasks => _studyTasks ??= new Repository<StudyTask>(_context);

        // Notification System repository properties
        public IRepository<NotificationType> NotificationTypes => _notificationTypes ??= new Repository<NotificationType>(_context);
        public IRepository<NotificationPriority> NotificationPriorities => _notificationPriorities ??= new Repository<NotificationPriority>(_context);
        public IRepository<Notification> Notifications => _notifications ??= new Repository<Notification>(_context);
        public IRepository<NotificationStatus> NotificationStatuses => _notificationStatuses ??= new Repository<NotificationStatus>(_context);
        public IRepository<NotificationRecipient> NotificationRecipients => _notificationRecipients ??= new Repository<NotificationRecipient>(_context);
        public IRepository<NotificationResponse> NotificationResponses => _notificationResponses ??= new Repository<NotificationResponse>(_context);

        // Gamification repository properties
        public IRepository<Level> Levels => _levels ??= new Repository<Level>(_context);
        public IRepository<UserLevel> UserLevels => _userLevels ??= new Repository<UserLevel>(_context);
        public IRepository<Badge> Badges => _badges ??= new Repository<Badge>(_context);
        public IRepository<UserBadge> UserBadges => _userBadges ??= new Repository<UserBadge>(_context);
        public IRepository<Challenge> Challenges => _challenges ??= new Repository<Challenge>(_context);
        public IRepository<UserChallenge> UserChallenges => _userChallenges ??= new Repository<UserChallenge>(_context);
        public IRepository<Achievement> Achievements => _achievements ??= new Repository<Achievement>(_context);
        public IRepository<UserAchievement> UserAchievements => _userAchievements ??= new Repository<UserAchievement>(_context);
        public IRepository<UserStreak> UserStreaks => _userStreaks ??= new Repository<UserStreak>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}