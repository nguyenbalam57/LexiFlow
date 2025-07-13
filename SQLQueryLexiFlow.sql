-- Kiểm tra bảng database LexiFlow
SELECT name, state_desc FROM sys.databases WHERE name = 'LexiFlow';

-- Tạo cơ sở dữ liệu
CREATE DATABASE LexiFlow;
GO

-- Sử dụng database
USE LexiFlow;
GO

-- ===================================
-- CORE USER MANAGEMENT TABLES / BẢNG QUẢN LÝ NGƯỜI DÙNG CỐT LÕI
-- ===================================

-- Users Table
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    Department NVARCHAR(100),
    Email NVARCHAR(255),
    PhoneNumber NVARCHAR(50),
    Position NVARCHAR(100),
    LastLogin DATETIME,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- Roles Table
CREATE TABLE Roles (
    RoleID INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(255),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- UserRoles Table
CREATE TABLE UserRoles (
    UserRoleID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    RoleID INT NOT NULL,
    AssignedAt DATETIME DEFAULT GETDATE(),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
);

-- Permissions Table
CREATE TABLE Permissions (
    PermissionID INT IDENTITY(1,1) PRIMARY KEY,
    PermissionName NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(255),
    Module NVARCHAR(50),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- RolePermissions Table Bảng quyền vai trò
CREATE TABLE RolePermissions (
    RolePermissionID INT IDENTITY(1,1) PRIMARY KEY,
    RoleID INT NOT NULL,
    PermissionID INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID),
    FOREIGN KEY (PermissionID) REFERENCES Permissions(PermissionID)
);

-- Groups Table (for notification groups) Bảng Nhóm (dành cho nhóm thông báo)
CREATE TABLE Groups (
    GroupID INT IDENTITY(1,1) PRIMARY KEY,
    GroupName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- ===================================
-- VOCABULARY MANAGEMENT TABLES / BẢNG QUẢN LÝ TỪ VỰNG
-- ===================================

-- Categories Table Bảng danh mục
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    Level NVARCHAR(20),
    DisplayOrder INT,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- VocabularyGroups Table Bảng nhóm từ vựng
CREATE TABLE VocabularyGroups (
    GroupID INT IDENTITY(1,1) PRIMARY KEY,
    GroupName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    CategoryID INT,
    CreatedByUserID INT,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID),
    FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID)
);

-- Vocabulary Table
CREATE TABLE Vocabulary (
    VocabularyID INT IDENTITY(1,1) PRIMARY KEY,
    Japanese NVARCHAR(100) NOT NULL,
    Kana NVARCHAR(100),
    Romaji NVARCHAR(100),
    Vietnamese NVARCHAR(255),
    English NVARCHAR(255),
    Example NVARCHAR(MAX),
    Notes NVARCHAR(MAX),
    GroupID INT,
    Level NVARCHAR(20),
    PartOfSpeech NVARCHAR(50),
    AudioFile NVARCHAR(255),
    CreatedByUserID INT,
    UpdatedByUserID INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (GroupID) REFERENCES VocabularyGroups(GroupID),
    FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
    FOREIGN KEY (UpdatedByUserID) REFERENCES Users(UserID)
);

-- VocabularyCategories Table
CREATE TABLE VocabularyCategories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL,
    ParentCategoryID INT,
    Description NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ParentCategoryID) REFERENCES VocabularyCategories(CategoryID)
);

-- Kanji Table
CREATE TABLE Kanji (
    KanjiID INT IDENTITY(1,1) PRIMARY KEY,
    Character NVARCHAR(10) NOT NULL UNIQUE,
    Onyomi NVARCHAR(100),
    Kunyomi NVARCHAR(100),
    Meaning NVARCHAR(255),
    StrokeCount INT,
    JLPTLevel NVARCHAR(10),
    Grade INT,
    Radicals NVARCHAR(100),
    Components NVARCHAR(100),
    Examples NVARCHAR(MAX),
    MnemonicHint NVARCHAR(MAX),
    WritingOrderImage NVARCHAR(255),
    CreatedByUserID INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID)
);

-- KanjiVocabulary Table (Many-to-Many relationship between Kanji and Vocabulary)
CREATE TABLE KanjiVocabulary (
    KanjiVocabularyID INT IDENTITY(1,1) PRIMARY KEY,
    KanjiID INT NOT NULL,
    VocabularyID INT NOT NULL,
    Position INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (KanjiID) REFERENCES Kanji(KanjiID),
    FOREIGN KEY (VocabularyID) REFERENCES Vocabulary(VocabularyID)
);

-- Grammar Table
CREATE TABLE Grammar (
    GrammarID INT IDENTITY(1,1) PRIMARY KEY,
    GrammarPoint NVARCHAR(100) NOT NULL,
    JLPTLevel NVARCHAR(10),
    Pattern NVARCHAR(255),
    Meaning NVARCHAR(MAX),
    Usage NVARCHAR(MAX),
    Examples NVARCHAR(MAX),
    Notes NVARCHAR(MAX),
    Conjugation NVARCHAR(MAX),
    RelatedGrammar NVARCHAR(255),
    CategoryID INT,
    CreatedByUserID INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID),
    FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID)
);

-- GrammarExamples Table
CREATE TABLE GrammarExamples (
    ExampleID INT IDENTITY(1,1) PRIMARY KEY,
    GrammarID INT NOT NULL,
    JapaneseSentence NVARCHAR(MAX),
    Romaji NVARCHAR(MAX),
    VietnameseTranslation NVARCHAR(MAX),
    EnglishTranslation NVARCHAR(MAX),
    Context NVARCHAR(255),
    AudioFile NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (GrammarID) REFERENCES Grammar(GrammarID)
);

-- TechnicalTerms Table (Kỹ thuật)
CREATE TABLE TechnicalTerms (
    TermID INT IDENTITY(1,1) PRIMARY KEY,
    Japanese NVARCHAR(100) NOT NULL,
    Kana NVARCHAR(100),
    Romaji NVARCHAR(100),
    Vietnamese NVARCHAR(255),
    English NVARCHAR(255),
    Field NVARCHAR(100), -- IT, Engineering, Medical, etc.
    SubField NVARCHAR(100), -- Database, Network, Frontend, etc.
    Definition NVARCHAR(MAX),
    Context NVARCHAR(MAX),
    Examples NVARCHAR(MAX),
    Abbreviation NVARCHAR(50),
    RelatedTerms NVARCHAR(255),
    Department NVARCHAR(100),
    CreatedByUserID INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID)
);

-- UserPersonalVocabulary Table (Từ vựng cá nhân của người dùng)
CREATE TABLE UserPersonalVocabulary (
    PersonalVocabID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    Japanese NVARCHAR(100) NOT NULL,
    Kana NVARCHAR(100),
    Romaji NVARCHAR(100),
    Vietnamese NVARCHAR(255),
    English NVARCHAR(255),
    PersonalNote NVARCHAR(MAX),
    Context NVARCHAR(MAX),
    Source NVARCHAR(255), -- Where user learned this word
    Importance INT DEFAULT 3, -- 1-5 scale
    Tags NVARCHAR(255),
    ImagePath NVARCHAR(255),
    AudioPath NVARCHAR(255),
    IsPublic BIT DEFAULT 0, -- Can share with others
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- UserKanjiProgress Table
CREATE TABLE UserKanjiProgress (
    ProgressID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    KanjiID INT NOT NULL,
    RecognitionLevel INT DEFAULT 0, -- 0-100
    WritingLevel INT DEFAULT 0, -- 0-100
    LastPracticed DATETIME,
    PracticeCount INT DEFAULT 0,
    CorrectCount INT DEFAULT 0,
    NextReviewDate DATETIME,
    Notes NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (KanjiID) REFERENCES Kanji(KanjiID)
);

-- UserGrammarProgress Table
CREATE TABLE UserGrammarProgress (
    ProgressID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    GrammarID INT NOT NULL,
    UnderstandingLevel INT DEFAULT 0, -- 0-100
    UsageLevel INT DEFAULT 0, -- 0-100
    LastStudied DATETIME,
    StudyCount INT DEFAULT 0,
    TestScore FLOAT,
    NextReviewDate DATETIME,
    PersonalNotes NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (GrammarID) REFERENCES Grammar(GrammarID)
);

-- UserTechnicalTerms Table (Từ vựng kỹ thuật của người dùng)
CREATE TABLE UserTechnicalTerms (
    UserTermID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    TermID INT NOT NULL,
    IsBookmarked BIT DEFAULT 0,
    UsageFrequency INT DEFAULT 0,
    LastUsed DATETIME,
    PersonalExample NVARCHAR(MAX),
    WorkContext NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (TermID) REFERENCES TechnicalTerms(TermID)
);

-- VocabularyRelations Table (Quan hệ giữa các từ vựng)
CREATE TABLE VocabularyRelations (
    RelationID INT IDENTITY(1,1) PRIMARY KEY,
    VocabularyID1 INT NOT NULL,
    VocabularyID2 INT NOT NULL,
    RelationType NVARCHAR(50), -- Synonym, Antonym, Related, etc.
    Description NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (VocabularyID1) REFERENCES Vocabulary(VocabularyID),
    FOREIGN KEY (VocabularyID2) REFERENCES Vocabulary(VocabularyID)
);

-- KanjiComponents Table (Bộ phận cấu tạo của Kanji)
CREATE TABLE KanjiComponents (
    ComponentID INT IDENTITY(1,1) PRIMARY KEY,
    ComponentName NVARCHAR(50) NOT NULL,
    Character NVARCHAR(10),
    Meaning NVARCHAR(255),
    Type NVARCHAR(50), -- Radical, Component
    StrokeCount INT,
    Position NVARCHAR(50), -- Top, Bottom, Left, Right, etc.
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- KanjiComponentMapping Table
CREATE TABLE KanjiComponentMapping (
    MappingID INT IDENTITY(1,1) PRIMARY KEY,
    KanjiID INT NOT NULL,
    ComponentID INT NOT NULL,
    Position NVARCHAR(50),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (KanjiID) REFERENCES Kanji(KanjiID),
    FOREIGN KEY (ComponentID) REFERENCES KanjiComponents(ComponentID)
);

-- DepartmentVocabulary Table (Từ vựng theo phòng ban)
CREATE TABLE DepartmentVocabulary (
    DeptVocabID INT IDENTITY(1,1) PRIMARY KEY,
    Department NVARCHAR(100) NOT NULL,
    VocabularyID INT,
    TechnicalTermID INT,
    Priority INT DEFAULT 3, -- 1-5
    IsRequired BIT DEFAULT 0,
    CreatedByUserID INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (VocabularyID) REFERENCES Vocabulary(VocabularyID),
    FOREIGN KEY (TechnicalTermID) REFERENCES TechnicalTerms(TermID),
    FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID)
);

-- ===================================
-- LEARNING PROGRESS TABLES / BẢNG TIẾN ĐỘ HỌC TẬP
-- ===================================

-- LearningProgress Table
CREATE TABLE LearningProgress (
    ProgressID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    VocabularyID INT NOT NULL,
    StudyCount INT DEFAULT 0,
    CorrectCount INT DEFAULT 0,
    IncorrectCount INT DEFAULT 0,
    LastStudied DATETIME,
    MemoryStrength INT DEFAULT 0,
    NextReviewDate DATETIME,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (VocabularyID) REFERENCES Vocabulary(VocabularyID)
);

-- PersonalWordLists Table
CREATE TABLE PersonalWordLists (
    ListID INT IDENTITY(1,1) PRIMARY KEY,
    ListName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    UserID INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- PersonalWordListItems Table
CREATE TABLE PersonalWordListItems (
    ItemID INT IDENTITY(1,1) PRIMARY KEY,
    ListID INT NOT NULL,
    VocabularyID INT NOT NULL,
    AddedAt DATETIME DEFAULT GETDATE(),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ListID) REFERENCES PersonalWordLists(ListID),
    FOREIGN KEY (VocabularyID) REFERENCES Vocabulary(VocabularyID)
);

-- ===================================
-- JLPT EXAM MANAGEMENT TABLES / BẢNG QUẢN LÝ KỲ THI JLPT
-- ===================================

-- JLPTLevels Table
CREATE TABLE JLPTLevels (
    LevelID INT IDENTITY(1,1) PRIMARY KEY,
    LevelName NVARCHAR(10) NOT NULL UNIQUE,
    Description NVARCHAR(255),
    VocabularyCount INT,
    KanjiCount INT,
    GrammarPoints INT,
    PassingScore INT,
    RequiredSkills NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- JLPTExams Table
CREATE TABLE JLPTExams (
    ExamID INT IDENTITY(1,1) PRIMARY KEY,
    ExamName NVARCHAR(100) NOT NULL,
    Level NVARCHAR(10),
    Year INT,
    Month NVARCHAR(20),
    TotalTime INT,
    TotalScore INT,
    TotalQuestions INT,
    Description NVARCHAR(MAX),
    IsOfficial BIT DEFAULT 0,
    IsActive BIT DEFAULT 1,
    CreatedByUserID INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID)
);

-- JLPTSections Table
CREATE TABLE JLPTSections (
    SectionID INT IDENTITY(1,1) PRIMARY KEY,
    ExamID INT NOT NULL,
    SectionName NVARCHAR(100),
    SectionType NVARCHAR(50),
    OrderNumber INT,
    TimeAllocation INT,
    ScoreAllocation INT,
    QuestionCount INT,
    Instructions NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ExamID) REFERENCES JLPTExams(ExamID)
);

-- Questions Table
CREATE TABLE Questions (
    QuestionID INT IDENTITY(1,1) PRIMARY KEY,
    SectionID INT,
    QuestionType NVARCHAR(50),
    QuestionText NVARCHAR(MAX),
    QuestionImage NVARCHAR(255),
    QuestionAudio NVARCHAR(255),
    CorrectAnswer NVARCHAR(255),
    Explanation NVARCHAR(MAX),
    Difficulty INT,
    JLPT_Level NVARCHAR(10),
    Tags NVARCHAR(255),
    Skills NVARCHAR(255),
    IsVerified BIT DEFAULT 0,
    CreatedByUserID INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (SectionID) REFERENCES JLPTSections(SectionID),
    FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID)
);

-- QuestionOptions Table
CREATE TABLE QuestionOptions (
    OptionID INT IDENTITY(1,1) PRIMARY KEY,
    QuestionID INT NOT NULL,
    OptionText NVARCHAR(MAX),
    OptionImage NVARCHAR(255),
    IsCorrect BIT DEFAULT 0,
    DisplayOrder INT,
    Explanation NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (QuestionID) REFERENCES Questions(QuestionID)
);

-- ===================================
-- TEST AND PRACTICE TABLES / BÀNG KIỂM TRA VÀ THỰC HÀNH
-- ===================================

-- TestResults Table
CREATE TABLE TestResults (
    TestResultID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    TestDate DATETIME DEFAULT GETDATE(),
    TestType NVARCHAR(50),
    TotalQuestions INT,
    CorrectAnswers INT,
    Score INT,
    Duration INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- TestDetails Table
CREATE TABLE TestDetails (
    TestDetailID INT IDENTITY(1,1) PRIMARY KEY,
    TestResultID INT NOT NULL,
    VocabularyID INT,
    IsCorrect BIT DEFAULT 0,
    TimeSpent INT,
    UserAnswer NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (TestResultID) REFERENCES TestResults(TestResultID),
    FOREIGN KEY (VocabularyID) REFERENCES Vocabulary(VocabularyID)
);

-- CustomExams Table
CREATE TABLE CustomExams (
    CustomExamID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    ExamName NVARCHAR(100) NOT NULL,
    Level NVARCHAR(10),
    Description NVARCHAR(MAX),
    TimeLimit INT,
    IsPublic BIT DEFAULT 0,
    IsFeatured BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- CustomExamQuestions Table
CREATE TABLE CustomExamQuestions (
    CustomQuestionID INT IDENTITY(1,1) PRIMARY KEY,
    CustomExamID INT NOT NULL,
    QuestionID INT NOT NULL,
    OrderNumber INT,
    ScoreValue INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CustomExamID) REFERENCES CustomExams(CustomExamID),
    FOREIGN KEY (QuestionID) REFERENCES Questions(QuestionID)
);

-- UserExams Table
CREATE TABLE UserExams (
    UserExamID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    ExamID INT,
    CustomExamID INT,
    StartTime DATETIME,
    EndTime DATETIME,
    Score INT,
    TotalQuestions INT,
    CorrectAnswers INT,
    IsCompleted BIT DEFAULT 0,
    ExamFeedback NVARCHAR(MAX),
    UserNotes NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (ExamID) REFERENCES JLPTExams(ExamID),
    FOREIGN KEY (CustomExamID) REFERENCES CustomExams(CustomExamID)
);

-- UserAnswers Table
CREATE TABLE UserAnswers (
    AnswerID INT IDENTITY(1,1) PRIMARY KEY,
    UserExamID INT NOT NULL,
    QuestionID INT NOT NULL,
    SelectedOptionID INT,
    UserInput NVARCHAR(MAX),
    IsCorrect BIT DEFAULT 0,
    TimeSpent INT,
    Attempt INT,
    AnsweredAt DATETIME,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserExamID) REFERENCES UserExams(UserExamID),
    FOREIGN KEY (QuestionID) REFERENCES Questions(QuestionID),
    FOREIGN KEY (SelectedOptionID) REFERENCES QuestionOptions(OptionID)
);

-- PracticeSets Table
CREATE TABLE PracticeSets (
    PracticeSetID INT IDENTITY(1,1) PRIMARY KEY,
    SetName NVARCHAR(100) NOT NULL,
    SetType NVARCHAR(50),
    Level NVARCHAR(10),
    Description NVARCHAR(MAX),
    Skills NVARCHAR(255),
    ItemCount INT,
    IsPublic BIT DEFAULT 0,
    IsFeatured BIT DEFAULT 0,
    CreatedByUserID INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID)
);

-- PracticeSetItems Table
CREATE TABLE PracticeSetItems (
    ItemID INT IDENTITY(1,1) PRIMARY KEY,
    PracticeSetID INT NOT NULL,
    QuestionID INT NOT NULL,
    OrderNumber INT,
    PracticeMode NVARCHAR(50),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (PracticeSetID) REFERENCES PracticeSets(PracticeSetID),
    FOREIGN KEY (QuestionID) REFERENCES Questions(QuestionID)
);

-- UserPracticeSets Table
CREATE TABLE UserPracticeSets (
    UserPracticeID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    PracticeSetID INT NOT NULL,
    StartDate DATETIME,
    LastPracticed DATETIME,
    CompletionPercentage INT DEFAULT 0,
    CorrectPercentage INT DEFAULT 0,
    TotalAttempts INT DEFAULT 0,
    UserNotes NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (PracticeSetID) REFERENCES PracticeSets(PracticeSetID)
);

-- UserPracticeAnswers Table
CREATE TABLE UserPracticeAnswers (
    PracticeAnswerID INT IDENTITY(1,1) PRIMARY KEY,
    UserPracticeID INT NOT NULL,
    QuestionID INT NOT NULL,
    IsCorrect BIT DEFAULT 0,
    Attempt INT,
    AnsweredAt DATETIME,
    TimeTaken INT,
    MemoryStrength INT,
    NextReviewDate DATETIME,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserPracticeID) REFERENCES UserPracticeSets(UserPracticeID),
    FOREIGN KEY (QuestionID) REFERENCES Questions(QuestionID)
);

-- ===================================
-- STUDY PLANNING TABLES / BẢNG KẾ HOẠCH HỌC TẬP
-- ===================================

-- StudyPlans Table
CREATE TABLE StudyPlans (
    PlanID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    PlanName NVARCHAR(100) NOT NULL,
    TargetLevel NVARCHAR(10),
    StartDate DATE,
    TargetDate DATE,
    Description NVARCHAR(MAX),
    MinutesPerDay INT,
    IsActive BIT DEFAULT 1,
    CurrentStatus NVARCHAR(50),
    CompletionPercentage FLOAT DEFAULT 0,
    LastUpdated DATETIME DEFAULT GETDATE(),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- StudyTopics Table
CREATE TABLE StudyTopics (
    TopicID INT IDENTITY(1,1) PRIMARY KEY,
    TopicName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    Category NVARCHAR(50),
    ParentTopicID INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ParentTopicID) REFERENCES StudyTopics(TopicID)
);

-- StudyGoals Table
CREATE TABLE StudyGoals (
    GoalID INT IDENTITY(1,1) PRIMARY KEY,
    PlanID INT NOT NULL,
    GoalName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    LevelID INT,
    TopicID INT,
    TargetDate DATETIME,
    Importance INT,
    Difficulty INT,
    IsCompleted BIT DEFAULT 0,
    ProgressPercentage FLOAT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (PlanID) REFERENCES StudyPlans(PlanID),
    FOREIGN KEY (LevelID) REFERENCES JLPTLevels(LevelID),
    FOREIGN KEY (TopicID) REFERENCES StudyTopics(TopicID)
);

-- StudyPlanItems Table
CREATE TABLE StudyPlanItems (
    ItemID INT IDENTITY(1,1) PRIMARY KEY,
    PlanID INT NOT NULL,
    ItemType NVARCHAR(50),
    Content NVARCHAR(MAX),
    ScheduledDate DATE,
    Priority INT,
    IsRequired BIT DEFAULT 1,
    EstimatedTime INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (PlanID) REFERENCES StudyPlans(PlanID)
);

-- StudyPlanProgress Table
CREATE TABLE StudyPlanProgress (
    ProgressID INT IDENTITY(1,1) PRIMARY KEY,
    ItemID INT NOT NULL,
    CompletionStatus INT,
    CompletedDate DATE,
    ActualTime INT,
    UserNotes NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ItemID) REFERENCES StudyPlanItems(ItemID)
);

-- StudyTasks Table
CREATE TABLE StudyTasks (
    TaskID INT IDENTITY(1,1) PRIMARY KEY,
    GoalID INT NOT NULL,
    TaskName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    EstimatedDuration INT,
    DurationUnit NVARCHAR(20),
    ItemID INT,
    Priority INT,
    IsRequired BIT DEFAULT 1,
    IsCompleted BIT DEFAULT 0,
    CompletedAt DATETIME,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (GoalID) REFERENCES StudyGoals(GoalID),
    FOREIGN KEY (ItemID) REFERENCES StudyPlanItems(ItemID)
);

-- TaskCompletions Table
CREATE TABLE TaskCompletions (
    CompletionID INT IDENTITY(1,1) PRIMARY KEY,
    TaskID INT NOT NULL,
    CompletionDate DATETIME,
    ActualDuration INT,
    EffortLevel INT,
    SatisfactionLevel INT,
    Notes NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (TaskID) REFERENCES StudyTasks(TaskID)
);

-- ===================================
-- ANALYTICS AND REPORTING TABLES / BẢNG PHÂN TÍCH VÀ BÁO CÁO
-- ===================================

-- ExamAnalytics Table
CREATE TABLE ExamAnalytics (
    AnalyticsID INT IDENTITY(1,1) PRIMARY KEY,
    UserExamID INT NOT NULL,
    VocabularyScore FLOAT,
    GrammarScore FLOAT,
    ReadingScore FLOAT,
    ListeningScore FLOAT,
    WeakAreas NVARCHAR(MAX),
    StrongAreas NVARCHAR(MAX),
    Recommendations NVARCHAR(MAX),
    GeneratedAt DATETIME DEFAULT GETDATE(),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserExamID) REFERENCES UserExams(UserExamID)
);

-- PracticeAnalytics Table
CREATE TABLE PracticeAnalytics (
    AnalyticsID INT IDENTITY(1,1) PRIMARY KEY,
    UserPracticeID INT NOT NULL,
    SkillsAnalysis NVARCHAR(MAX),
    LearningCurve NVARCHAR(MAX),
    ProblemAreas NVARCHAR(MAX),
    MasteryPercentage INT,
    Recommendations NVARCHAR(MAX),
    GeneratedAt DATETIME DEFAULT GETDATE(),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserPracticeID) REFERENCES UserPracticeSets(UserPracticeID)
);

-- StrengthWeakness Table
CREATE TABLE StrengthWeakness (
    SWID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    SkillType NVARCHAR(50),
    SpecificSkill NVARCHAR(100),
    ProficiencyLevel INT,
    RecommendedMaterials NVARCHAR(MAX),
    ImprovementNotes NVARCHAR(MAX),
    LastUpdated DATETIME DEFAULT GETDATE(),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- ReportTypes Table
CREATE TABLE ReportTypes (
    TypeID INT IDENTITY(1,1) PRIMARY KEY,
    TypeName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    Template NVARCHAR(MAX),
    FrequencyDays INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- StudyReports Table
CREATE TABLE StudyReports (
    ReportID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    ReportName NVARCHAR(100),
    TypeID INT,
    StartPeriod DATETIME,
    EndPeriod DATETIME,
    GeneratedAt DATETIME DEFAULT GETDATE(),
    Format NVARCHAR(20),
    AccessURL NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (TypeID) REFERENCES ReportTypes(TypeID)
);

-- StudyReportItems Table
CREATE TABLE StudyReportItems (
    ItemID INT IDENTITY(1,1) PRIMARY KEY,
    ReportID INT NOT NULL,
    GoalID INT,
    MetricName NVARCHAR(100),
    MetricValue NVARCHAR(255),
    Comparison NVARCHAR(255),
    Trend NVARCHAR(50),
    Notes NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ReportID) REFERENCES StudyReports(ReportID),
    FOREIGN KEY (GoalID) REFERENCES StudyGoals(GoalID)
);

-- ===================================
-- NOTIFICATION SYSTEM TABLES / BẢNG HỆ THỐNG THÔNG BÁO
-- ===================================

-- NotificationTypes Table
CREATE TABLE NotificationTypes (
    TypeID INT IDENTITY(1,1) PRIMARY KEY,
    TypeName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255),
    IconPath NVARCHAR(255),
    ColorCode NVARCHAR(20),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- NotificationPriorities Table
CREATE TABLE NotificationPriorities (
    PriorityID INT IDENTITY(1,1) PRIMARY KEY,
    PriorityName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255),
    DisplayOrder INT,
    ColorCode NVARCHAR(20),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- Notifications Table
CREATE TABLE Notifications (
    NotificationID INT IDENTITY(1,1) PRIMARY KEY,
    SenderUserID INT,
    Title NVARCHAR(200) NOT NULL,
    Content NVARCHAR(MAX),
    TypeID INT,
    PriorityID INT,
    AllowResponses BIT DEFAULT 0,
    ExpirationDate DATETIME,
    AttachmentURL NVARCHAR(255),
    IsSystemGenerated BIT DEFAULT 0,
    IsDeleted BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (SenderUserID) REFERENCES Users(UserID),
    FOREIGN KEY (TypeID) REFERENCES NotificationTypes(TypeID),
    FOREIGN KEY (PriorityID) REFERENCES NotificationPriorities(PriorityID)
);

-- NotificationStatuses Table
CREATE TABLE NotificationStatuses (
    StatusID INT IDENTITY(1,1) PRIMARY KEY,
    StatusName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255),
    ColorCode NVARCHAR(20),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- NotificationRecipients Table
CREATE TABLE NotificationRecipients (
    RecipientID INT IDENTITY(1,1) PRIMARY KEY,
    NotificationID INT NOT NULL,
    UserID INT,
    GroupID INT,
    StatusID INT,
    ReceivedAt DATETIME DEFAULT GETDATE(),
    ReadAt DATETIME,
    IsArchived BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (NotificationID) REFERENCES Notifications(NotificationID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (GroupID) REFERENCES Groups(GroupID),
    FOREIGN KEY (StatusID) REFERENCES NotificationStatuses(StatusID)
);

-- NotificationResponses Table
CREATE TABLE NotificationResponses (
    ResponseID INT IDENTITY(1,1) PRIMARY KEY,
    RecipientID INT NOT NULL,
    ResponseContent NVARCHAR(MAX),
    ResponseTypeID INT,
    ResponseTime DATETIME DEFAULT GETDATE(),
    AttachmentURL NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (RecipientID) REFERENCES NotificationRecipients(RecipientID)
);

-- ===================================
-- SCHEDULING SYSTEM TABLES / BẢNG HỆ THỐNG LỊCH TRÌNH
-- ===================================

-- Schedules Table
CREATE TABLE Schedules (
    ScheduleID INT IDENTITY(1,1) PRIMARY KEY,
    ScheduleName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    CreatedByUserID INT,
    IsPublic BIT DEFAULT 0,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID)
);

-- ScheduleItemTypes Table
CREATE TABLE ScheduleItemTypes (
    TypeID INT IDENTITY(1,1) PRIMARY KEY,
    TypeName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255),
    IconPath NVARCHAR(255),
    DefaultColor NVARCHAR(20),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- ScheduleRecurrences Table
CREATE TABLE ScheduleRecurrences (
    RecurrenceID INT IDENTITY(1,1) PRIMARY KEY,
    RecurrenceType NVARCHAR(50),
    Interval INT,
    DaysOfWeek NVARCHAR(20),
    DayOfMonth INT,
    MonthOfYear INT,
    RecurrenceEnd DATETIME,
    MaxOccurrences INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- ScheduleItems Table
CREATE TABLE ScheduleItems (
    ItemID INT IDENTITY(1,1) PRIMARY KEY,
    ScheduleID INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    StartTime DATETIME,
    EndTime DATETIME,
    TypeID INT,
    RecurrenceID INT,
    Location NVARCHAR(255),
    IsAllDay BIT DEFAULT 0,
    IsCancelled BIT DEFAULT 0,
    IsCompleted BIT DEFAULT 0,
    PriorityLevel INT,
    ColorCode NVARCHAR(20),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ScheduleID) REFERENCES Schedules(ScheduleID),
    FOREIGN KEY (TypeID) REFERENCES ScheduleItemTypes(TypeID),
    FOREIGN KEY (RecurrenceID) REFERENCES ScheduleRecurrences(RecurrenceID)
);

-- ScheduleItemParticipants Table
CREATE TABLE ScheduleItemParticipants (
    ParticipantID INT IDENTITY(1,1) PRIMARY KEY,
    ItemID INT NOT NULL,
    UserID INT,
    GroupID INT,
    ParticipantRole NVARCHAR(50),
    ResponseStatus INT,
    Notes NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ItemID) REFERENCES ScheduleItems(ItemID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (GroupID) REFERENCES Groups(GroupID)
);

-- ScheduleReminders Table
CREATE TABLE ScheduleReminders (
    ReminderID INT IDENTITY(1,1) PRIMARY KEY,
    ItemID INT NOT NULL,
    UserID INT,
    ReminderTime INT,
    ReminderUnit NVARCHAR(20),
    IsEmailReminder BIT DEFAULT 0,
    IsPopupReminder BIT DEFAULT 1,
    IsSent BIT DEFAULT 0,
    SentAt DATETIME,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ItemID) REFERENCES ScheduleItems(ItemID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- ===================================
-- USER SUBMISSION SYSTEM TABLES / BẢNG HỆ THỐNG GỬI NGƯỜI DÙNG
-- ===================================

-- SubmissionStatuses Table
CREATE TABLE SubmissionStatuses (
    StatusID INT IDENTITY(1,1) PRIMARY KEY,
    StatusName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255),
    ColorCode NVARCHAR(20),
    DisplayOrder INT,
    IsTerminal BIT DEFAULT 0,
    IsDefault BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- StatusTransitions Table
CREATE TABLE StatusTransitions (
    TransitionID INT IDENTITY(1,1) PRIMARY KEY,
    FromStatusID INT,
    ToStatusID INT,
    TransitionName NVARCHAR(100),
    RequiredRole NVARCHAR(50),
    RequiresApproval BIT DEFAULT 0,
    RequiresNote BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (FromStatusID) REFERENCES SubmissionStatuses(StatusID),
    FOREIGN KEY (ToStatusID) REFERENCES SubmissionStatuses(StatusID)
);

-- UserVocabularySubmissions Table
CREATE TABLE UserVocabularySubmissions (
    SubmissionID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    SubmissionTitle NVARCHAR(200) NOT NULL,
    SubmissionDescription NVARCHAR(MAX),
    SubmittedAt DATETIME DEFAULT GETDATE(),
    StatusID INT,
    LastUpdatedAt DATETIME DEFAULT GETDATE(),
    LastUpdatedByUserID INT,
    IsUrgent BIT DEFAULT 0,
    VocabularyCount INT DEFAULT 0,
    SubmissionNotes NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (StatusID) REFERENCES SubmissionStatuses(StatusID),
    FOREIGN KEY (LastUpdatedByUserID) REFERENCES Users(UserID)
);

-- UserVocabularyDetails Table
CREATE TABLE UserVocabularyDetails (
    DetailID INT IDENTITY(1,1) PRIMARY KEY,
    SubmissionID INT NOT NULL,
    Japanese NVARCHAR(100),
    Kana NVARCHAR(100),
    Romaji NVARCHAR(100),
    Vietnamese NVARCHAR(255),
    English NVARCHAR(255),
    Example NVARCHAR(MAX),
    Notes NVARCHAR(MAX),
    CategoryID INT,
    PartOfSpeech NVARCHAR(50),
    Level NVARCHAR(10),
    Source NVARCHAR(255),
    ImageURL NVARCHAR(255),
    AudioURL NVARCHAR(255),
    IsApproved BIT DEFAULT 0,
    RejectionReason NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (SubmissionID) REFERENCES UserVocabularySubmissions(SubmissionID),
    FOREIGN KEY (CategoryID) REFERENCES VocabularyCategories(CategoryID)
);

-- ApprovalHistories Table
CREATE TABLE ApprovalHistories (
    HistoryID INT IDENTITY(1,1) PRIMARY KEY,
    SubmissionID INT NOT NULL,
    ApproverUserID INT,
    FromStatusID INT,
    ToStatusID INT,
    Comments NVARCHAR(MAX),
    ApprovedAt DATETIME DEFAULT GETDATE(),
    ChangeDetails NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (SubmissionID) REFERENCES UserVocabularySubmissions(SubmissionID),
    FOREIGN KEY (ApproverUserID) REFERENCES Users(UserID),
    FOREIGN KEY (FromStatusID) REFERENCES SubmissionStatuses(StatusID),
    FOREIGN KEY (ToStatusID) REFERENCES SubmissionStatuses(StatusID)
);

-- ===================================
-- GAMIFICATION SYSTEM TABLES / BẢNG HỆ THỐNG TRÒ CHƠI HÓA
-- ===================================

-- Levels Table
CREATE TABLE Levels (
    LevelID INT IDENTITY(1,1) PRIMARY KEY,
    LevelName NVARCHAR(50) NOT NULL,
    LevelNumber INT NOT NULL UNIQUE,
    ExperienceRequired INT NOT NULL,
    IconPath NVARCHAR(255),
    Description NVARCHAR(255),
    Benefits NVARCHAR(MAX),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- UserLevels Table
CREATE TABLE UserLevels (
    UserLevelID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    LevelID INT NOT NULL,
    CurrentExperience INT DEFAULT 0,
    ExperienceToNextLevel INT,
    LevelUpDate DATETIME,
    TotalExperienceEarned INT DEFAULT 0,
    DaysAtCurrentLevel INT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (LevelID) REFERENCES Levels(LevelID)
);

-- PointTypes Table
CREATE TABLE PointTypes (
    PointTypeID INT IDENTITY(1,1) PRIMARY KEY,
    TypeName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255),
    IconPath NVARCHAR(255),
    Multiplier INT DEFAULT 1,
    IsActive BIT DEFAULT 1,
    DisplayOrder INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- UserPoints Table
CREATE TABLE UserPoints (
    UserPointID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    PointTypeID INT,
    Points INT NOT NULL,
    EarnedAt DATETIME DEFAULT GETDATE(),
    Source NVARCHAR(100),
    Description NVARCHAR(255),
    RelatedEntityID INT,
    RelatedEntityType NVARCHAR(50),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (PointTypeID) REFERENCES PointTypes(PointTypeID)
);

-- Badges Table
CREATE TABLE Badges (
    BadgeID INT IDENTITY(1,1) PRIMARY KEY,
    BadgeName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    IconPath NVARCHAR(255),
    UnlockCriteria NVARCHAR(MAX),
    RequiredPoints INT,
    BadgeCategory NVARCHAR(50),
    Rarity INT,
    IsActive BIT DEFAULT 1,
    IsHidden BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- UserBadges Table
CREATE TABLE UserBadges (
    UserBadgeID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    BadgeID INT NOT NULL,
    EarnedAt DATETIME DEFAULT GETDATE(),
    IsDisplayed BIT DEFAULT 1,
    IsFavorite BIT DEFAULT 0,
    EarnCount INT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (BadgeID) REFERENCES Badges(BadgeID)
);

-- Challenges Table
CREATE TABLE Challenges (
    ChallengeID INT IDENTITY(1,1) PRIMARY KEY,
    ChallengeName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    PointsReward INT,
    ExperienceReward INT,
    BadgeID INT,
    StartDate DATETIME,
    EndDate DATETIME,
    DurationDays INT,
    ChallengeType NVARCHAR(50),
    Difficulty NVARCHAR(20),
    IsActive BIT DEFAULT 1,
    IsRecurring BIT DEFAULT 0,
    RecurrencePattern NVARCHAR(100),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (BadgeID) REFERENCES Badges(BadgeID)
);

-- ChallengeRequirements Table
CREATE TABLE ChallengeRequirements (
    RequirementID INT IDENTITY(1,1) PRIMARY KEY,
    ChallengeID INT NOT NULL,
    RequirementType NVARCHAR(50),
    Description NVARCHAR(255),
    TargetValue INT,
    EntityType NVARCHAR(50),
    EntityID INT,
    IsMandatory BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ChallengeID) REFERENCES Challenges(ChallengeID)
);

-- UserChallenges Table
CREATE TABLE UserChallenges (
    UserChallengeID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    ChallengeID INT NOT NULL,
    StartedAt DATETIME DEFAULT GETDATE(),
    CompletedAt DATETIME,
    CurrentProgress INT DEFAULT 0,
    MaxProgress INT,
    IsCompleted BIT DEFAULT 0,
    IsRewarded BIT DEFAULT 0,
    IsAbandoned BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (ChallengeID) REFERENCES Challenges(ChallengeID)
);

-- DailyTasks Table
CREATE TABLE DailyTasks (
    TaskID INT IDENTITY(1,1) PRIMARY KEY,
    TaskName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    PointsReward INT,
    ExperienceReward INT,
    TaskCategory NVARCHAR(50),
    TaskType NVARCHAR(50),
    Difficulty INT,
    IsActive BIT DEFAULT 1,
    RecurrenceDays INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- DailyTaskRequirements Table
CREATE TABLE DailyTaskRequirements (
    RequirementID INT IDENTITY(1,1) PRIMARY KEY,
    TaskID INT NOT NULL,
    RequirementType NVARCHAR(50),
    Description NVARCHAR(255),
    TargetValue INT,
    EntityType NVARCHAR(50),
    EntityID INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (TaskID) REFERENCES DailyTasks(TaskID)
);

-- UserDailyTasks Table
CREATE TABLE UserDailyTasks (
    UserTaskID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    TaskID INT NOT NULL,
    TaskDate DATE NOT NULL,
    CompletedAt DATETIME,
    CurrentProgress INT DEFAULT 0,
    MaxProgress INT,
    IsCompleted BIT DEFAULT 0,
    IsRewarded BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (TaskID) REFERENCES DailyTasks(TaskID)
);

-- Achievements Table
CREATE TABLE Achievements (
    AchievementID INT IDENTITY(1,1) PRIMARY KEY,
    AchievementName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    IconPath NVARCHAR(255),
    PointsReward INT,
    ExperienceReward INT,
    BadgeID INT,
    Category NVARCHAR(50),
    Tier INT,
    IsActive BIT DEFAULT 1,
    IsSecret BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (BadgeID) REFERENCES Badges(BadgeID)
);

-- AchievementRequirements Table
CREATE TABLE AchievementRequirements (
    RequirementID INT IDENTITY(1,1) PRIMARY KEY,
    AchievementID INT NOT NULL,
    RequirementType NVARCHAR(50),
    Description NVARCHAR(255),
    TargetValue INT,
    EntityType NVARCHAR(50),
    EntityID INT,
    IsMandatory BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (AchievementID) REFERENCES Achievements(AchievementID)
);

-- UserAchievements Table
CREATE TABLE UserAchievements (
    UserAchievementID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    AchievementID INT NOT NULL,
    UnlockedAt DATETIME DEFAULT GETDATE(),
    CurrentTier INT DEFAULT 1,
    MaxTier INT,
    ProgressValue INT DEFAULT 0,
    TargetValue INT,
    IsDisplayed BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (AchievementID) REFERENCES Achievements(AchievementID)
);

-- UserStreaks Table
CREATE TABLE UserStreaks (
    StreakID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    StreakType NVARCHAR(50),
    CurrentCount INT DEFAULT 0,
    LongestCount INT DEFAULT 0,
    LastActivityDate DATE,
    IsActive BIT DEFAULT 1,
    StartedAt DATETIME DEFAULT GETDATE(),
    TotalStreakDays INT DEFAULT 0,
    StreakFreezes INT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- Leaderboards Table
CREATE TABLE Leaderboards (
    LeaderboardID INT IDENTITY(1,1) PRIMARY KEY,
    LeaderboardName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    LeaderboardType NVARCHAR(50),
    TimeFrame NVARCHAR(20),
    StartDate DATETIME,
    EndDate DATETIME,
    ResetPeriodDays INT,
    IsActive BIT DEFAULT 1,
    Scope NVARCHAR(50),
    EntityType NVARCHAR(50),
    EntityID INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- LeaderboardEntries Table
CREATE TABLE LeaderboardEntries (
    EntryID INT IDENTITY(1,1) PRIMARY KEY,
    LeaderboardID INT NOT NULL,
    UserID INT NOT NULL,
    Score INT NOT NULL,
    Rank INT,
    PreviousRank INT,
    RankChange INT,
    UpdatedAt DATETIME DEFAULT GETDATE(),
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (LeaderboardID) REFERENCES Leaderboards(LeaderboardID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- Events Table
CREATE TABLE Events (
    EventID INT IDENTITY(1,1) PRIMARY KEY,
    EventName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    StartDate DATETIME,
    EndDate DATETIME,
    EventType NVARCHAR(50),
    RewardType NVARCHAR(50),
    RewardValue INT,
    BadgeID INT,
    IsActive BIT DEFAULT 1,
    ParticipationType NVARCHAR(50),
    MaxParticipants INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (BadgeID) REFERENCES Badges(BadgeID)
);

-- UserEvents Table
CREATE TABLE UserEvents (
    UserEventID INT IDENTITY(1,1) PRIMARY KEY,
    EventID INT NOT NULL,
    UserID INT NOT NULL,
    JoinedAt DATETIME DEFAULT GETDATE(),
    Score INT DEFAULT 0,
    Rank INT,
    HasCompleted BIT DEFAULT 0,
    IsRewarded BIT DEFAULT 0,
    LastActivityAt DATETIME,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (EventID) REFERENCES Events(EventID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- UserGifts Table
CREATE TABLE UserGifts (
    GiftID INT IDENTITY(1,1) PRIMARY KEY,
    SenderUserID INT NOT NULL,
    ReceiverUserID INT NOT NULL,
    GiftType NVARCHAR(50),
    GiftValue INT,
    Message NVARCHAR(MAX),
    SentAt DATETIME DEFAULT GETDATE(),
    ReceivedAt DATETIME,
    IsUsed BIT DEFAULT 0,
    IsExpired BIT DEFAULT 0,
    ExpirationDate DATETIME,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (SenderUserID) REFERENCES Users(UserID),
    FOREIGN KEY (ReceiverUserID) REFERENCES Users(UserID)
);

-- ===================================
-- SYSTEM MANAGEMENT TABLES / BẢNG QUẢN LÝ HỆ THỐNG
-- ===================================

-- Settings Table
CREATE TABLE Settings (
    SettingID INT IDENTITY(1,1) PRIMARY KEY,
    SettingKey NVARCHAR(100) NOT NULL UNIQUE,
    SettingValue NVARCHAR(MAX),
    Description NVARCHAR(255),
    [Group] NVARCHAR(50),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- ActivityLog Table
CREATE TABLE ActivityLog (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT,
    Timestamp DATETIME DEFAULT GETDATE(),
    Action NVARCHAR(100),
    Module NVARCHAR(50),
    Details NVARCHAR(MAX),
    IPAddress NVARCHAR(45),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- ===================================
-- INDEXES FOR PERFORMANCE / CHỈ SỐ HIỆU SUẤT
-- ===================================

-- User-related indexes
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_Username ON Users(Username);
CREATE INDEX IX_UserRoles_UserID ON UserRoles(UserID);
CREATE INDEX IX_UserRoles_RoleID ON UserRoles(RoleID);

-- Vocabulary-related indexes
CREATE INDEX IX_Vocabulary_GroupID ON Vocabulary(GroupID);
CREATE INDEX IX_Vocabulary_Level ON Vocabulary(Level);
CREATE INDEX IX_Vocabulary_Japanese ON Vocabulary(Japanese);

-- Learning progress indexes
CREATE INDEX IX_LearningProgress_UserID ON LearningProgress(UserID);
CREATE INDEX IX_LearningProgress_VocabularyID ON LearningProgress(VocabularyID);
CREATE INDEX IX_LearningProgress_NextReviewDate ON LearningProgress(NextReviewDate);

-- Test and exam indexes
CREATE INDEX IX_UserExams_UserID ON UserExams(UserID);
CREATE INDEX IX_UserExams_ExamID ON UserExams(ExamID);
CREATE INDEX IX_UserAnswers_UserExamID ON UserAnswers(UserExamID);
CREATE INDEX IX_Questions_SectionID ON Questions(SectionID);

-- Notification indexes
CREATE INDEX IX_Notifications_SenderUserID ON Notifications(SenderUserID);
CREATE INDEX IX_NotificationRecipients_UserID ON NotificationRecipients(UserID);
CREATE INDEX IX_NotificationRecipients_NotificationID ON NotificationRecipients(NotificationID);

-- Gamification indexes
CREATE INDEX IX_UserPoints_UserID ON UserPoints(UserID);
CREATE INDEX IX_UserBadges_UserID ON UserBadges(UserID);
CREATE INDEX IX_LeaderboardEntries_UserID ON LeaderboardEntries(UserID);
CREATE INDEX IX_LeaderboardEntries_LeaderboardID ON LeaderboardEntries(LeaderboardID);

-- Activity log indexes
CREATE INDEX IX_ActivityLog_UserID ON ActivityLog(UserID);
CREATE INDEX IX_ActivityLog_Timestamp ON ActivityLog(Timestamp);

-- ===================================
-- INITIAL DATA INSERTION / CHÈN DỮ LIỆU BAN ĐẦU
-- ===================================

-- Insert default roles
INSERT INTO Roles (RoleName, Description, IsActive) VALUES
('Admin', 'System Administrator with full access', 1),
('Teacher', 'Can create and manage learning content', 1),
('Student', 'Can access learning materials and take tests', 1),
('Guest', 'Limited access to public content', 1);

-- Insert default permissions
INSERT INTO Permissions (PermissionName, Description, Module) VALUES
('user.create', 'Create new users', 'User Management'),
('user.edit', 'Edit user information', 'User Management'),
('user.delete', 'Delete users', 'User Management'),
('vocabulary.create', 'Create vocabulary entries', 'Vocabulary'),
('vocabulary.edit', 'Edit vocabulary entries', 'Vocabulary'),
('vocabulary.delete', 'Delete vocabulary entries', 'Vocabulary'),
('test.create', 'Create tests and exams', 'Testing'),
('test.grade', 'Grade tests', 'Testing'),
('report.view', 'View reports', 'Analytics'),
('report.generate', 'Generate reports', 'Analytics');

-- Insert JLPT Levels
INSERT INTO JLPTLevels (LevelName, Description, VocabularyCount, KanjiCount, PassingScore) VALUES
('N5', 'Beginner level - Basic Japanese', 800, 100, 80),
('N4', 'Elementary level', 1500, 300, 90),
('N3', 'Intermediate level', 3700, 650, 95),
('N2', 'Upper intermediate level', 6000, 1000, 90),
('N1', 'Advanced level', 10000, 2000, 100);

-- Insert notification types
INSERT INTO NotificationTypes (TypeName, Description) VALUES
('System', 'System-generated notifications'),
('Assignment', 'New assignment notifications'),
('Test', 'Test-related notifications'),
('Achievement', 'Achievement and badge notifications'),
('Message', 'Direct messages from users');

-- Insert notification priorities
INSERT INTO NotificationPriorities (PriorityName, Description, DisplayOrder) VALUES
('Low', 'Low priority notifications', 1),
('Normal', 'Normal priority notifications', 2),
('High', 'High priority notifications', 3),
('Urgent', 'Urgent notifications requiring immediate attention', 4);

-- Insert default settings
INSERT INTO Settings (SettingKey, SettingValue, Description, [Group]) VALUES
('app.name', 'LexiFlow', 'Application name', 'General'),
('app.version', '1.0.0', 'Application version', 'General'),
('session.timeout', '30', 'Session timeout in minutes', 'Security'),
('password.min_length', '8', 'Minimum password length', 'Security'),
('email.smtp_server', 'smtp.gmail.com', 'SMTP server for email', 'Email'),
('email.smtp_port', '587', 'SMTP port', 'Email');

GO

-- Create triggers for UpdatedAt columns 
-- / Tạo trình kích hoạt cho các cột UpdatedAt
-- This trigger will automatically update the UpdatedAt column when a row is modified
-- / Trình kích hoạt này sẽ tự động cập nhật cột UpdatedAt khi một hàng được sửa đổi

-- Example trigger for Users table (create similar triggers for all tables)
-- / Ví dụ về trình kích hoạt cho bảng Người dùng (tạo trình kích hoạt tương tự cho tất cả các bảng)
CREATE TRIGGER trg_Users_UpdatedAt
ON Users
AFTER UPDATE
AS
BEGIN
    UPDATE Users
    SET UpdatedAt = GETDATE()
    FROM Users u
    INNER JOIN inserted i ON u.UserID = i.UserID;
END;
GO

PRINT 'LexiFlow Database created successfully!';