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
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL,
    ApiKey NVARCHAR(255),
    ApiKeyExpiry DATETIME,
    DepartmentID INT
);

-- Roles Table
CREATE TABLE Roles (
    RoleID INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(255),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL
);

-- UserRoles Table
CREATE TABLE UserRoles (
    UserRoleID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    RoleID INT NOT NULL,
    AssignedAt DATETIME DEFAULT GETDATE(),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL,
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
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL
);

-- RolePermissions Table
CREATE TABLE RolePermissions (
    RolePermissionID INT IDENTITY(1,1) PRIMARY KEY,
    RoleID INT NOT NULL,
    PermissionID INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL,
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID),
    FOREIGN KEY (PermissionID) REFERENCES Permissions(PermissionID)
);

-- Groups Table (for notification groups)
CREATE TABLE Groups (
    GroupID INT IDENTITY(1,1) PRIMARY KEY,
    GroupName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL
);

-- Departments Table
CREATE TABLE Departments (
    DepartmentID INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName NVARCHAR(100) NOT NULL UNIQUE,
    DepartmentCode NVARCHAR(20),
    ParentDepartmentID INT,
    ManagerUserID INT,
    Description NVARCHAR(255),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL,
    FOREIGN KEY (ParentDepartmentID) REFERENCES Departments(DepartmentID),
    FOREIGN KEY (ManagerUserID) REFERENCES Users(UserID)
);

-- Add Foreign Key for DepartmentID in Users table
ALTER TABLE Users ADD CONSTRAINT FK_Users_Departments FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID);

-- Teams Table
CREATE TABLE Teams (
    TeamID INT IDENTITY(1,1) PRIMARY KEY,
    TeamName NVARCHAR(100) NOT NULL,
    DepartmentID INT,
    LeaderUserID INT,
    Description NVARCHAR(255),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL,
    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID),
    FOREIGN KEY (LeaderUserID) REFERENCES Users(UserID)
);

-- UserTeams Table
CREATE TABLE UserTeams (
    UserTeamID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    TeamID INT NOT NULL,
    JoinedAt DATETIME DEFAULT GETDATE(),
    Role NVARCHAR(50),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (TeamID) REFERENCES Teams(TeamID)
);

-- UserPermissions Table
CREATE TABLE UserPermissions (
    UserPermissionID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    PermissionID INT NOT NULL,
    GrantedByUserID INT,
    GrantedAt DATETIME DEFAULT GETDATE(),
    ExpiresAt DATETIME,
    IsActive BIT DEFAULT 1,
    Notes NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (PermissionID) REFERENCES Permissions(PermissionID),
    FOREIGN KEY (GrantedByUserID) REFERENCES Users(UserID)
);

-- PermissionGroups Table
CREATE TABLE PermissionGroups (
    GroupID INT IDENTITY(1,1) PRIMARY KEY,
    GroupName NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL
);

-- PermissionGroupMappings Table
CREATE TABLE PermissionGroupMappings (
    MappingID INT IDENTITY(1,1) PRIMARY KEY,
    GroupID INT NOT NULL,
    PermissionID INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL,
    FOREIGN KEY (GroupID) REFERENCES PermissionGroups(GroupID),
    FOREIGN KEY (PermissionID) REFERENCES Permissions(PermissionID)
);

-- ===================================
-- VOCABULARY MANAGEMENT TABLES / BẢNG QUẢN LÝ TỪ VỰNG
-- ===================================

-- Categories Table
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    Level NVARCHAR(20),
    DisplayOrder INT,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL
);

-- VocabularyGroups Table
CREATE TABLE VocabularyGroups (
    GroupID INT IDENTITY(1,1) PRIMARY KEY,
    GroupName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    CategoryID INT,
    CreatedByUserID INT,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
    LastModifiedAt DATETIME DEFAULT GETDATE(),
    LastModifiedBy INT,
    FOREIGN KEY (GroupID) REFERENCES VocabularyGroups(GroupID),
    FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
    FOREIGN KEY (UpdatedByUserID) REFERENCES Users(UserID),
    FOREIGN KEY (LastModifiedBy) REFERENCES Users(UserID)
);

-- VocabularyCategories Table
CREATE TABLE VocabularyCategories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL,
    ParentCategoryID INT,
    Description NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL
);

-- KanjiComponentMapping Table
CREATE TABLE KanjiComponentMapping (
    MappingID INT IDENTITY(1,1) PRIMARY KEY,
    KanjiID INT NOT NULL,
    ComponentID INT NOT NULL,
    Position NVARCHAR(50),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL
);

-- NotificationPriorities Table
CREATE TABLE NotificationPriorities (
    PriorityID INT IDENTITY(1,1) PRIMARY KEY,
    PriorityName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255),
    DisplayOrder INT,
    ColorCode NVARCHAR(20),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL
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
    RowVersion ROWVERSION NOT NULL,
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
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL
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
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL
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
    RowVersion ROWVERSION NOT NULL,
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
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL
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
    RowVersion ROWVERSION NOT NULL,
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
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    RowVersion ROWVERSION NOT NULL,
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
    UpdatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL
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
    RowVersion ROWVERSION NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- SyncLog Table
CREATE TABLE SyncLog (
    SyncID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    TableName NVARCHAR(100),
    LastSyncAt DATETIME,
    RecordsSynced INT,
    SyncDirection NVARCHAR(20), -- 'Upload', 'Download'
    Status NVARCHAR(20),
    ErrorMessage NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    RowVersion ROWVERSION NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- ===================================
-- INDEXES FOR PERFORMANCE / CHỈ SỐ HIỆU SUẤT
-- ===================================

-- ===================================
-- USER MANAGEMENT INDEXES
-- ===================================

-- User-related indexes
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_Username ON Users(Username);
CREATE INDEX IX_UserRoles_UserID ON UserRoles(UserID);
CREATE INDEX IX_UserRoles_RoleID ON UserRoles(RoleID);

-- Indexes for User Authentication and Profile Access
-- These improve login speed and user search operations
CREATE INDEX IX_Users_Email_Password ON Users(Email, PasswordHash);
CREATE INDEX IX_Users_Username_Password ON Users(Username, PasswordHash);
CREATE INDEX IX_Users_FullName ON Users(FullName);
CREATE INDEX IX_Users_IsActive ON Users(IsActive);
CREATE INDEX IX_Users_DepartmentID ON Users(DepartmentID);
CREATE INDEX IX_Users_LastLogin ON Users(LastLogin);

-- Covering index for commonly accessed user profile data
CREATE INDEX IX_Users_ProfileData ON Users(UserID, Username, FullName, Email, Department, Position, IsActive);

-- Permission-related indexes
CREATE INDEX IX_Permissions_Module ON Permissions(Module);
CREATE INDEX IX_RolePermissions_PermissionID ON RolePermissions(PermissionID);
CREATE INDEX IX_UserPermissions_PermissionID ON UserPermissions(PermissionID);
CREATE INDEX IX_UserPermissions_IsActive ON UserPermissions(IsActive, PermissionID);
CREATE INDEX IX_UserPermissions_ExpiresAt ON UserPermissions(ExpiresAt, IsActive);

-- Department and Team indexes
CREATE INDEX IX_Departments_ParentDepartmentID ON Departments(ParentDepartmentID);
CREATE INDEX IX_Departments_ManagerUserID ON Departments(ManagerUserID);
CREATE INDEX IX_Teams_DepartmentID ON Teams(DepartmentID);
CREATE INDEX IX_Teams_LeaderUserID ON Teams(LeaderUserID);
CREATE INDEX IX_UserTeams_TeamID ON UserTeams(TeamID);
CREATE INDEX IX_UserTeams_IsActive ON UserTeams(IsActive, UserID);

-- ===================================
-- VOCABULARY MANAGEMENT INDEXES
-- ===================================

-- Vocabulary-related indexes
CREATE INDEX IX_Vocabulary_GroupID ON Vocabulary(GroupID);
CREATE INDEX IX_Vocabulary_Level ON Vocabulary(Level);
CREATE INDEX IX_Vocabulary_Japanese ON Vocabulary(Japanese);

-- Enhanced Vocabulary Indexes for full-text and partial searches
CREATE INDEX IX_Vocabulary_Japanese_Kana ON Vocabulary(Japanese, Kana);
CREATE INDEX IX_Vocabulary_Vietnamese_English ON Vocabulary(Vietnamese, English);
CREATE INDEX IX_Vocabulary_Level_PartOfSpeech ON Vocabulary(Level, PartOfSpeech);

-- Covering index for vocabulary list views (improves list performance)
CREATE INDEX IX_Vocabulary_ListView ON Vocabulary(VocabularyID, Japanese, Kana, Vietnamese, English, Level, GroupID);

-- User-specific vocabulary tracking
CREATE INDEX IX_Vocabulary_CreatedByUserID ON Vocabulary(CreatedByUserID);
CREATE INDEX IX_Vocabulary_UpdatedByUserID ON Vocabulary(UpdatedByUserID);
CREATE INDEX IX_Vocabulary_LastModifiedBy ON Vocabulary(LastModifiedBy);

-- Indexes for vocabulary categorization
CREATE INDEX IX_VocabularyGroups_CategoryID ON VocabularyGroups(CategoryID);
CREATE INDEX IX_VocabularyGroups_CreatedByUserID ON VocabularyGroups(CreatedByUserID);
CREATE INDEX IX_VocabularyGroups_IsActive ON VocabularyGroups(IsActive);

-- Kanji-related indexes
CREATE INDEX IX_Kanji_JLPTLevel ON Kanji(JLPTLevel);
CREATE INDEX IX_Kanji_StrokeCount ON Kanji(StrokeCount);
CREATE INDEX IX_Kanji_Grade ON Kanji(Grade);
CREATE INDEX IX_KanjiVocabulary_KanjiID ON KanjiVocabulary(KanjiID);
CREATE INDEX IX_KanjiVocabulary_Position ON KanjiVocabulary(Position);

-- Grammar-related indexes
CREATE INDEX IX_Grammar_JLPTLevel ON Grammar(JLPTLevel);
CREATE INDEX IX_Grammar_CategoryID ON Grammar(CategoryID);
CREATE INDEX IX_Grammar_CreatedByUserID ON Grammar(CreatedByUserID);
CREATE INDEX IX_GrammarExamples_GrammarID ON GrammarExamples(GrammarID);

-- Technical terms indexes
CREATE INDEX IX_TechnicalTerms_Field ON TechnicalTerms(Field);
CREATE INDEX IX_TechnicalTerms_SubField ON TechnicalTerms(SubField);
CREATE INDEX IX_TechnicalTerms_Department ON TechnicalTerms(Department);
CREATE INDEX IX_TechnicalTerms_Japanese_Kana ON TechnicalTerms(Japanese, Kana);
CREATE INDEX IX_TechnicalTerms_Vietnamese_English ON TechnicalTerms(Vietnamese, English);

-- ===================================
-- LEARNING PROGRESS INDEXES
-- ===================================

-- Learning progress indexes
CREATE INDEX IX_LearningProgress_UserID ON LearningProgress(UserID);
CREATE INDEX IX_LearningProgress_VocabularyID ON LearningProgress(VocabularyID);
CREATE INDEX IX_LearningProgress_NextReviewDate ON LearningProgress(NextReviewDate);

-- Index for due vocabulary review - CRITICAL for performance
CREATE INDEX IX_LearningProgress_NextReviewDate_UserID ON LearningProgress(NextReviewDate, UserID);
CREATE INDEX IX_LearningProgress_MemoryStrength ON LearningProgress(UserID, MemoryStrength);
CREATE INDEX IX_LearningProgress_StudyCount ON LearningProgress(UserID, StudyCount);

-- Covering index for user's learning statistics
CREATE INDEX IX_LearningProgress_Stats ON LearningProgress(UserID, VocabularyID, StudyCount, CorrectCount, IncorrectCount, MemoryStrength);

-- Kanji/Grammar progress tracking
CREATE INDEX IX_UserKanjiProgress_UserID_LastPracticed ON UserKanjiProgress(UserID, LastPracticed);
CREATE INDEX IX_UserKanjiProgress_NextReviewDate ON UserKanjiProgress(UserID, NextReviewDate);
CREATE INDEX IX_UserGrammarProgress_UserID_LastStudied ON UserGrammarProgress(UserID, LastStudied);
CREATE INDEX IX_UserGrammarProgress_NextReviewDate ON UserGrammarProgress(UserID, NextReviewDate);

-- Personal vocabulary indexes
CREATE INDEX IX_UserPersonalVocabulary_UserID_Japanese ON UserPersonalVocabulary(UserID, Japanese);
CREATE INDEX IX_UserPersonalVocabulary_Importance ON UserPersonalVocabulary(UserID, Importance);
CREATE INDEX IX_UserPersonalVocabulary_IsPublic ON UserPersonalVocabulary(IsPublic, UserID);

-- Personal word lists
CREATE INDEX IX_PersonalWordLists_UserID ON PersonalWordLists(UserID);
CREATE INDEX IX_PersonalWordListItems_ListID ON PersonalWordListItems(ListID);
CREATE INDEX IX_PersonalWordListItems_VocabularyID ON PersonalWordListItems(VocabularyID);

-- ===================================
-- TEST AND EXAM INDEXES
-- ===================================

-- Test and exam indexes
CREATE INDEX IX_UserExams_UserID ON UserExams(UserID);
CREATE INDEX IX_UserExams_ExamID ON UserExams(ExamID);
CREATE INDEX IX_UserAnswers_UserExamID ON UserAnswers(UserExamID);
CREATE INDEX IX_Questions_SectionID ON Questions(SectionID);

-- JLPT-related indexes
CREATE INDEX IX_JLPTExams_Level_Year_Month ON JLPTExams(Level, Year, Month);
CREATE INDEX IX_JLPTExams_IsActive ON JLPTExams(IsActive);
CREATE INDEX IX_JLPTExams_IsOfficial ON JLPTExams(IsOfficial);

-- Question-related indexes
CREATE INDEX IX_Questions_JLPT_Level ON Questions(JLPT_Level);
CREATE INDEX IX_Questions_QuestionType ON Questions(QuestionType);
CREATE INDEX IX_Questions_Difficulty ON Questions(Difficulty);
CREATE INDEX IX_Questions_Skills ON Questions(Skills);
CREATE INDEX IX_Questions_IsVerified ON Questions(IsVerified);
CREATE INDEX IX_QuestionOptions_IsCorrect ON QuestionOptions(QuestionID, IsCorrect);

-- User exam results
CREATE INDEX IX_UserExams_UserID_ExamID ON UserExams(UserID, ExamID);
CREATE INDEX IX_UserExams_UserID_IsCompleted ON UserExams(UserID, IsCompleted);
CREATE INDEX IX_UserExams_CustomExamID ON UserExams(CustomExamID);
CREATE INDEX IX_UserExams_StartTime ON UserExams(UserID, StartTime);
CREATE INDEX IX_UserAnswers_QuestionID ON UserAnswers(QuestionID);
CREATE INDEX IX_UserAnswers_IsCorrect ON UserAnswers(UserExamID, IsCorrect);

-- Practice-related indexes
CREATE INDEX IX_PracticeSets_SetType_Level ON PracticeSets(SetType, Level);
CREATE INDEX IX_PracticeSets_IsPublic ON PracticeSets(IsPublic);
CREATE INDEX IX_PracticeSets_IsFeatured ON PracticeSets(IsFeatured);
CREATE INDEX IX_PracticeSetItems_QuestionID ON PracticeSetItems(QuestionID);
CREATE INDEX IX_UserPracticeSets_UserID_PracticeSetID ON UserPracticeSets(UserID, PracticeSetID);
CREATE INDEX IX_UserPracticeSets_LastPracticed ON UserPracticeSets(UserID, LastPracticed);
CREATE INDEX IX_UserPracticeAnswers_QuestionID ON UserPracticeAnswers(QuestionID);
CREATE INDEX IX_UserPracticeAnswers_IsCorrect ON UserPracticeAnswers(UserPracticeID, IsCorrect);
CREATE INDEX IX_UserPracticeAnswers_NextReviewDate ON UserPracticeAnswers(UserPracticeID, NextReviewDate);

-- ===================================
-- STUDY PLANNING INDEXES
-- ===================================

-- Study plan tracking
CREATE INDEX IX_StudyPlans_UserID_IsActive ON StudyPlans(UserID, IsActive);
CREATE INDEX IX_StudyPlans_TargetLevel ON StudyPlans(UserID, TargetLevel);
CREATE INDEX IX_StudyPlans_TargetDate ON StudyPlans(UserID, TargetDate);
CREATE INDEX IX_StudyGoals_PlanID_IsCompleted ON StudyGoals(PlanID, IsCompleted);
CREATE INDEX IX_StudyGoals_TargetDate ON StudyGoals(PlanID, TargetDate);
CREATE INDEX IX_StudyGoals_TopicID ON StudyGoals(TopicID);

-- Study tasks
CREATE INDEX IX_StudyPlanItems_PlanID_ScheduledDate ON StudyPlanItems(PlanID, ScheduledDate);
CREATE INDEX IX_StudyPlanItems_Priority ON StudyPlanItems(PlanID, Priority);
CREATE INDEX IX_StudyTasks_GoalID_IsCompleted ON StudyTasks(GoalID, IsCompleted);
CREATE INDEX IX_StudyTasks_Priority ON StudyTasks(GoalID, Priority);
CREATE INDEX IX_TaskCompletions_TaskID ON TaskCompletions(TaskID);
CREATE INDEX IX_TaskCompletions_CompletionDate ON TaskCompletions(TaskID, CompletionDate);

-- ===================================
-- NOTIFICATION SYSTEM INDEXES
-- ===================================

-- Notification indexes
CREATE INDEX IX_Notifications_SenderUserID ON Notifications(SenderUserID);
CREATE INDEX IX_NotificationRecipients_UserID ON NotificationRecipients(UserID);
CREATE INDEX IX_NotificationRecipients_NotificationID ON NotificationRecipients(NotificationID);

-- Notification indexes
CREATE INDEX IX_Notifications_SenderUserID_TypeID ON Notifications(SenderUserID, TypeID);
CREATE INDEX IX_Notifications_PriorityID ON Notifications(PriorityID);
CREATE INDEX IX_Notifications_ExpirationDate ON Notifications(ExpirationDate);
CREATE INDEX IX_Notifications_IsSystemGenerated ON Notifications(IsSystemGenerated);
CREATE INDEX IX_Notifications_IsDeleted ON Notifications(IsDeleted);

-- Notification recipient tracking
CREATE INDEX IX_NotificationRecipients_NotificationID_StatusID ON NotificationRecipients(NotificationID, StatusID);
CREATE INDEX IX_NotificationRecipients_GroupID ON NotificationRecipients(GroupID);
CREATE INDEX IX_NotificationRecipients_ReadAt ON NotificationRecipients(UserID, ReadAt);
CREATE INDEX IX_NotificationRecipients_IsArchived ON NotificationRecipients(UserID, IsArchived);
CREATE INDEX IX_NotificationResponses_RecipientID ON NotificationResponses(RecipientID);
CREATE INDEX IX_NotificationResponses_ResponseTime ON NotificationResponses(RecipientID, ResponseTime);

-- ===================================
-- SCHEDULING SYSTEM INDEXES
-- ===================================

-- Schedule-related indexes
CREATE INDEX IX_Schedules_CreatedByUserID ON Schedules(CreatedByUserID);
CREATE INDEX IX_Schedules_IsActive ON Schedules(IsActive);
CREATE INDEX IX_ScheduleItems_ScheduleID_StartTime ON ScheduleItems(ScheduleID, StartTime);
CREATE INDEX IX_ScheduleItems_EndTime ON ScheduleItems(ScheduleID, EndTime);
CREATE INDEX IX_ScheduleItems_IsAllDay ON ScheduleItems(ScheduleID, IsAllDay);
CREATE INDEX IX_ScheduleItems_IsCancelled ON ScheduleItems(ScheduleID, IsCancelled);
CREATE INDEX IX_ScheduleItems_IsCompleted ON ScheduleItems(ScheduleID, IsCompleted);
CREATE INDEX IX_ScheduleItems_TypeID ON ScheduleItems(TypeID);
CREATE INDEX IX_ScheduleItems_RecurrenceID ON ScheduleItems(RecurrenceID);
CREATE INDEX IX_ScheduleItemParticipants_UserID ON ScheduleItemParticipants(UserID);
CREATE INDEX IX_ScheduleItemParticipants_GroupID ON ScheduleItemParticipants(GroupID);
CREATE INDEX IX_ScheduleReminders_UserID ON ScheduleReminders(UserID);
CREATE INDEX IX_ScheduleReminders_IsSent ON ScheduleReminders(ItemID, IsSent);

-- ===================================
-- GAMIFICATION SYSTEM INDEXES
-- ===================================

-- Gamification indexes
CREATE INDEX IX_UserPoints_UserID ON UserPoints(UserID);
CREATE INDEX IX_UserBadges_UserID ON UserBadges(UserID);
CREATE INDEX IX_LeaderboardEntries_UserID ON LeaderboardEntries(UserID);
CREATE INDEX IX_LeaderboardEntries_LeaderboardID ON LeaderboardEntries(LeaderboardID);

-- User progress indexes
CREATE INDEX IX_UserLevels_UserID_LevelID ON UserLevels(UserID, LevelID);
CREATE INDEX IX_UserLevels_TotalExperienceEarned ON UserLevels(UserID, TotalExperienceEarned);
CREATE INDEX IX_UserPoints_UserID_PointTypeID ON UserPoints(UserID, PointTypeID);
CREATE INDEX IX_UserPoints_EarnedAt ON UserPoints(UserID, EarnedAt);
CREATE INDEX IX_UserBadges_UserID_BadgeID ON UserBadges(UserID, BadgeID);
CREATE INDEX IX_UserBadges_IsDisplayed ON UserBadges(UserID, IsDisplayed);
CREATE INDEX IX_UserBadges_IsFavorite ON UserBadges(UserID, IsFavorite);
CREATE INDEX IX_UserBadges_EarnCount ON UserBadges(UserID, EarnCount);

-- Challenge and achievement indexes
CREATE INDEX IX_Challenges_StartDate_EndDate ON Challenges(StartDate, EndDate);
CREATE INDEX IX_Challenges_IsActive ON Challenges(IsActive);
CREATE INDEX IX_Challenges_IsRecurring ON Challenges(IsRecurring);
CREATE INDEX IX_UserChallenges_UserID_ChallengeID ON UserChallenges(UserID, ChallengeID);
CREATE INDEX IX_UserChallenges_IsCompleted ON UserChallenges(UserID, IsCompleted);
CREATE INDEX IX_UserChallenges_StartedAt ON UserChallenges(UserID, StartedAt);
CREATE INDEX IX_Achievements_Category_Tier ON Achievements(Category, Tier);
CREATE INDEX IX_Achievements_IsSecret ON Achievements(IsSecret);
CREATE INDEX IX_UserAchievements_UserID_AchievementID ON UserAchievements(UserID, AchievementID);
CREATE INDEX IX_UserAchievements_IsDisplayed ON UserAchievements(UserID, IsDisplayed);
CREATE INDEX IX_UserAchievements_CurrentTier ON UserAchievements(UserID, CurrentTier);

-- Daily tasks and streaks
CREATE INDEX IX_DailyTasks_TaskCategory ON DailyTasks(TaskCategory);
CREATE INDEX IX_DailyTasks_Difficulty ON DailyTasks(Difficulty);
CREATE INDEX IX_UserDailyTasks_UserID_TaskDate ON UserDailyTasks(UserID, TaskDate);
CREATE INDEX IX_UserDailyTasks_IsCompleted ON UserDailyTasks(UserID, IsCompleted);
CREATE INDEX IX_UserStreaks_UserID_StreakType ON UserStreaks(UserID, StreakType);
CREATE INDEX IX_UserStreaks_IsActive ON UserStreaks(UserID, IsActive);
CREATE INDEX IX_UserStreaks_LastActivityDate ON UserStreaks(UserID, LastActivityDate);
CREATE INDEX IX_UserStreaks_CurrentCount ON UserStreaks(UserID, CurrentCount DESC);

-- Leaderboard indexes
CREATE INDEX IX_Leaderboards_LeaderboardType ON Leaderboards(LeaderboardType);
CREATE INDEX IX_Leaderboards_TimeFrame ON Leaderboards(TimeFrame);
CREATE INDEX IX_Leaderboards_StartDate_EndDate ON Leaderboards(StartDate, EndDate);
CREATE INDEX IX_Leaderboards_IsActive ON Leaderboards(IsActive);
CREATE INDEX IX_LeaderboardEntries_LeaderboardID_Score ON LeaderboardEntries(LeaderboardID, Score DESC);
CREATE INDEX IX_LeaderboardEntries_Rank ON LeaderboardEntries(LeaderboardID, Rank);

-- ===================================
-- USER SUBMISSION SYSTEM INDEXES
-- ===================================

-- Submission tracking
CREATE INDEX IX_UserVocabularySubmissions_UserID_StatusID ON UserVocabularySubmissions(UserID, StatusID);
CREATE INDEX IX_UserVocabularySubmissions_SubmittedAt ON UserVocabularySubmissions(UserID, SubmittedAt);
CREATE INDEX IX_UserVocabularySubmissions_IsUrgent ON UserVocabularySubmissions(IsUrgent, StatusID);
CREATE INDEX IX_UserVocabularyDetails_SubmissionID ON UserVocabularyDetails(SubmissionID);
CREATE INDEX IX_UserVocabularyDetails_IsApproved ON UserVocabularyDetails(SubmissionID, IsApproved);
CREATE INDEX IX_UserVocabularyDetails_Level ON UserVocabularyDetails(Level);
CREATE INDEX IX_ApprovalHistories_SubmissionID ON ApprovalHistories(SubmissionID);
CREATE INDEX IX_ApprovalHistories_ApproverUserID ON ApprovalHistories(ApproverUserID);
CREATE INDEX IX_ApprovalHistories_FromStatusID_ToStatusID ON ApprovalHistories(FromStatusID, ToStatusID);

-- ===================================
-- ANALYTICS AND REPORTING INDEXES
-- ===================================

-- Analytics indexes
CREATE INDEX IX_ExamAnalytics_UserExamID ON ExamAnalytics(UserExamID);
CREATE INDEX IX_ExamAnalytics_GeneratedAt ON ExamAnalytics(GeneratedAt);
CREATE INDEX IX_PracticeAnalytics_UserPracticeID ON PracticeAnalytics(UserPracticeID);
CREATE INDEX IX_PracticeAnalytics_MasteryPercentage ON PracticeAnalytics(UserPracticeID, MasteryPercentage);
CREATE INDEX IX_StrengthWeakness_UserID_SkillType ON StrengthWeakness(UserID, SkillType);
CREATE INDEX IX_StrengthWeakness_ProficiencyLevel ON StrengthWeakness(UserID, ProficiencyLevel);
CREATE INDEX IX_StrengthWeakness_LastUpdated ON StrengthWeakness(UserID, LastUpdated);

-- Report indexes
CREATE INDEX IX_StudyReports_UserID_TypeID ON StudyReports(UserID, TypeID);
CREATE INDEX IX_StudyReports_StartPeriod_EndPeriod ON StudyReports(UserID, StartPeriod, EndPeriod);
CREATE INDEX IX_StudyReports_GeneratedAt ON StudyReports(UserID, GeneratedAt);
CREATE INDEX IX_StudyReportItems_ReportID ON StudyReportItems(ReportID);
CREATE INDEX IX_StudyReportItems_GoalID ON StudyReportItems(GoalID);
CREATE INDEX IX_StudyReportItems_MetricName ON StudyReportItems(ReportID, MetricName);

-- ===================================
-- SYSTEM MANAGEMENT INDEXES
-- ===================================

-- Activity log indexes
CREATE INDEX IX_ActivityLog_UserID ON ActivityLog(UserID);
CREATE INDEX IX_ActivityLog_Timestamp ON ActivityLog(Timestamp);

-- Settings and activity log
CREATE INDEX IX_Settings_Group ON Settings([Group]);
CREATE INDEX IX_ActivityLog_UserID_Timestamp ON ActivityLog(UserID, Timestamp);
CREATE INDEX IX_ActivityLog_Module ON ActivityLog(Module);
CREATE INDEX IX_ActivityLog_Action ON ActivityLog(Action);

-- Sync system indexes
CREATE INDEX IX_SyncLog_UserID_TableName ON SyncLog(UserID, TableName);
CREATE INDEX IX_SyncLog_LastSyncAt ON SyncLog(UserID, LastSyncAt);
CREATE INDEX IX_SyncLog_Status ON SyncLog(Status);

-- ===================================
-- ROWVERSION AND SYNC TRACKING INDEXES
-- ===================================

-- Create indexes for optimized ROWVERSION-based synchronization
CREATE INDEX IX_Vocabulary_RowVersion ON Vocabulary(RowVersion);
CREATE INDEX IX_VocabularyGroups_RowVersion ON VocabularyGroups(RowVersion);
CREATE INDEX IX_LearningProgress_RowVersion ON LearningProgress(RowVersion);
CREATE INDEX IX_Kanji_RowVersion ON Kanji(RowVersion);
CREATE INDEX IX_Grammar_RowVersion ON Grammar(RowVersion);
CREATE INDEX IX_UserExams_RowVersion ON UserExams(RowVersion);
CREATE INDEX IX_UserAnswers_RowVersion ON UserAnswers(RowVersion);
CREATE INDEX IX_StudyPlans_RowVersion ON StudyPlans(RowVersion);
CREATE INDEX IX_UserChallenges_RowVersion ON UserChallenges(RowVersion);

-- Indexes for SyncMarkers table
--CREATE INDEX IX_SyncMarkers_UserID_DeviceID ON SyncMarkers(UserID, DeviceID);
--CREATE INDEX IX_SyncMarkers_TableName ON SyncMarkers(TableName);
--CREATE INDEX IX_SyncMarkers_LastSyncTime ON SyncMarkers(LastSyncTime);

-- Indexes for ChangeTracking table
--CREATE INDEX IX_ChangeTracking_TableName_RowID ON ChangeTracking(TableName, RowID);
--CREATE INDEX IX_ChangeTracking_ChangeType ON ChangeTracking(ChangeType);
--CREATE INDEX IX_ChangeTracking_ChangedAt_IsProcessed ON ChangeTracking(ChangedAt, IsProcessed);
--CREATE INDEX IX_ChangeTracking_ChangedBy ON ChangeTracking(ChangedBy);

-- Indexes for RowVersionAudit table
--CREATE INDEX IX_RowVersionAudit_TableName_RowID ON RowVersionAudit(TableName, RowID);
--CREATE INDEX IX_RowVersionAudit_ChangedAt ON RowVersionAudit(ChangedAt);
--CREATE INDEX IX_RowVersionAudit_ChangedBy ON RowVersionAudit(ChangedBy);

-- ===================================
-- FILTERED INDEXES FOR COMMON QUERIES
-- ===================================

-- Active users only (reduces index size, improves performance)
CREATE INDEX IX_Users_Active_FilteredLogin ON Users(Email, PasswordHash, LastLogin) 
WHERE IsActive = 1;

---- Active vocabulary only (for vocabulary browsing)
--CREATE INDEX IX_Vocabulary_Active_Filtered ON Vocabulary(Level, PartOfSpeech, GroupID) 
--INCLUDE (Japanese, Kana, Vietnamese, English)
--WHERE IsActive = 1;

-- Due vocabulary for review (critical for spaced repetition algorithm)
--CREATE INDEX IX_LearningProgress_DueForReview ON LearningProgress(UserID) 
--INCLUDE (VocabularyID, StudyCount, CorrectCount, MemoryStrength)
--WHERE NextReviewDate <= GETDATE();

-- Completed exams for reporting
CREATE INDEX IX_UserExams_Completed ON UserExams(UserID, ExamID, Score) 
INCLUDE (StartTime, EndTime, TotalQuestions, CorrectAnswers)
WHERE IsCompleted = 1;

-- Public vocabulary items (for sharing)
CREATE INDEX IX_UserPersonalVocabulary_Public ON UserPersonalVocabulary(UserID, Japanese, Vietnamese)
WHERE IsPublic = 1;

-- Active challenges (reduces filtering overhead)
CREATE INDEX IX_Challenges_Active ON Challenges(StartDate, EndDate, ChallengeName)
WHERE IsActive = 1;

-- Current user streaks (for gamification display)
CREATE INDEX IX_UserStreaks_Active ON UserStreaks(UserID, StreakType, CurrentCount, LongestCount)
WHERE IsActive = 1;

-- ===================================
-- COLUMNSTORE INDEXES FOR ANALYTICS
-- ===================================

-- Columnstore index for learning analytics
-- Great for aggregating large amounts of historical data
CREATE COLUMNSTORE INDEX CSIX_LearningProgress_Analytics ON LearningProgress
(UserID, VocabularyID, StudyCount, CorrectCount, IncorrectCount, 
 MemoryStrength, LastStudied, NextReviewDate);

-- Columnstore index for user activity log analytics
CREATE COLUMNSTORE INDEX CSIX_ActivityLog_Analytics ON ActivityLog
(UserID, Timestamp, Action, Module);

-- Columnstore index for exam result analytics
CREATE COLUMNSTORE INDEX CSIX_UserAnswers_Analytics ON UserAnswers
(UserExamID, QuestionID, IsCorrect, TimeSpent, Attempt, AnsweredAt);

-- ===================================
-- INDEX MAINTENANCE PROCEDURE
-- ===================================

-- Create procedure to perform index maintenance
--CREATE OR ALTER PROCEDURE sp_MaintainIndexes
--AS
--BEGIN
--    -- Rebuild fragmented indexes
--    DECLARE @SQL NVARCHAR(MAX) = '';
    
--    SELECT @SQL = @SQL + 
--        'ALTER INDEX ' + QUOTENAME(i.name) + 
--        ' ON ' + QUOTENAME(SCHEMA_NAME(o.schema_id)) + '.' + QUOTENAME(o.name) + 
--        CASE 
--            WHEN ips.avg_fragmentation_in_percent > 30 THEN ' REBUILD;'
--            WHEN ips.avg_fragmentation_in_percent > 10 THEN ' REORGANIZE;'
--            ELSE ''
--        END + CHAR(13) + CHAR(10)
--    FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'LIMITED') ips
--    INNER JOIN sys.indexes i ON ips.object_id = i.object_id AND ips.index_id = i.index_id
--    INNER JOIN sys.objects o ON i.object_id = o.object_id
--    WHERE ips.avg_fragmentation_in_percent > 10
--    AND i.name IS NOT NULL
--    AND o.is_ms_shipped = 0
--    ORDER BY ips.avg_fragmentation_in_percent DESC;
    
--    IF LEN(@SQL) > 0
--    BEGIN
--        EXEC sp_executesql @SQL;
--        PRINT 'Index maintenance completed.';
--    END
--    ELSE
--    BEGIN
--        PRINT 'No index maintenance required.';
--    END
    
--    -- Update statistics
--    EXEC sp_updatestats;
--END;
--GO

-- Schedule the index maintenance procedure (sample)
-- In production, this would be set up as a SQL Agent job
PRINT 'To schedule index maintenance, create a SQL Agent job that executes sp_MaintainIndexes weekly.';
GO

-- ===================================
-- INDEX USAGE STATISTICS PROCEDURE
-- ===================================

-- Create procedure to monitor index usage
CREATE OR ALTER PROCEDURE sp_IndexUsageStats
AS
BEGIN
    SELECT 
        OBJECT_NAME(i.object_id) AS TableName,
        i.name AS IndexName,
        i.type_desc AS IndexType,
        ius.user_seeks + ius.user_scans + ius.user_lookups AS TotalReads,
        ius.user_seeks AS Seeks,
        ius.user_scans AS Scans,
        ius.user_lookups AS Lookups,
        ius.user_updates AS Updates,
        CASE 
            WHEN ius.user_updates > 0 AND (ius.user_seeks + ius.user_scans + ius.user_lookups) > 0
            THEN CAST((ius.user_seeks + ius.user_scans + ius.user_lookups) / (1.0 * ius.user_updates) AS DECIMAL(18,2))
            ELSE 0 
        END AS ReadWriteRatio,
        ius.last_user_seek AS LastSeek,
        ius.last_user_scan AS LastScan,
        ius.last_user_lookup AS LastLookup,
        ius.last_user_update AS LastUpdate
    FROM sys.indexes i
    LEFT JOIN sys.dm_db_index_usage_stats ius ON i.object_id = ius.object_id AND i.index_id = ius.index_id
    WHERE OBJECTPROPERTY(i.object_id, 'IsUserTable') = 1
    ORDER BY TotalReads DESC;
END;
GO

-- ===================================
-- MISSING INDEXES MONITORING
-- ===================================

-- Create procedure to identify missing indexes
CREATE OR ALTER PROCEDURE sp_MissingIndexes
AS
BEGIN
    SELECT 
        DB_NAME(mid.database_id) AS DatabaseName,
        OBJECT_NAME(mid.object_id) AS TableName,
        migs.avg_total_user_cost * migs.avg_user_impact * (migs.user_seeks + migs.user_scans) AS ImprovementMeasure,
        migs.user_seeks + migs.user_scans AS UserReads,
        'CREATE INDEX IX_' + OBJECT_NAME(mid.object_id) + '_' +
        REPLACE(REPLACE(REPLACE(ISNULL(mid.equality_columns, '') + 
        CASE WHEN mid.equality_columns IS NOT NULL AND mid.inequality_columns IS NOT NULL THEN '_' ELSE '' END + 
        ISNULL(mid.inequality_columns, ''), '[', ''), ']', ''), ', ', '_') +
        ' ON ' + mid.statement + ' (' + 
        ISNULL(mid.equality_columns, '') + 
        CASE WHEN mid.equality_columns IS NOT NULL AND mid.inequality_columns IS NOT NULL THEN ', ' ELSE '' END + 
        ISNULL(mid.inequality_columns, '') + ')' + 
        ISNULL(' INCLUDE (' + mid.included_columns + ')', '') AS CreateIndexStatement
    FROM sys.dm_db_missing_index_group_stats migs
    INNER JOIN sys.dm_db_missing_index_groups mig ON migs.group_handle = mig.index_group_handle
    INNER JOIN sys.dm_db_missing_index_details mid ON mig.index_handle = mid.index_handle
    WHERE mid.database_id = DB_ID()
    ORDER BY ImprovementMeasure DESC;
END;
GO

-- ===================================
-- INITIAL DATA INSERTION / CHÈN DỮ LIỆU BAN ĐẦU
-- ===================================

-- Insert default roles
INSERT INTO Roles (RoleName, Description, IsActive) VALUES
('Admin', 'System Administrator with full access', 1),
('Teacher', 'Can create and manage learning content', 1),
('Student', 'Can access learning materials and take tests', 1),
('Guest', 'Limited access to public content', 1),
('Manager', 'Department Manager with extended permissions', 1),
('Leader', 'Team Leader with team management capabilities', 1),
('Employee', 'Regular employee with limited access', 1);

-- Insert default permissions
INSERT INTO Permissions (PermissionName, Description, Module) VALUES
-- Basic permissions
('user.create', 'Create new users', 'User Management'),
('user.edit', 'Edit user information', 'User Management'),
('user.delete', 'Delete users', 'User Management'),
('vocabulary.create', 'Create vocabulary entries', 'Vocabulary'),
('vocabulary.edit', 'Edit vocabulary entries', 'Vocabulary'),
('vocabulary.delete', 'Delete vocabulary entries', 'Vocabulary'),
('test.create', 'Create tests and exams', 'Testing'),
('test.grade', 'Grade tests', 'Testing'),
('report.view', 'View reports', 'Analytics'),
('report.generate', 'Generate reports', 'Analytics'),

-- User Management Permissions
('user.view', 'View user information', 'User Management'),
('user.view.own', 'View own user information only', 'User Management'),
('user.view.team', 'View team members information', 'User Management'),
('user.manage.team', 'Manage team members', 'User Management'),
('user.manage.all', 'Manage all users', 'User Management'),

-- Vocabulary Permissions
('vocabulary.view', 'View vocabulary entries', 'Vocabulary'),
('vocabulary.view.department', 'View department vocabulary', 'Vocabulary'),
('vocabulary.suggest', 'Suggest new vocabulary', 'Vocabulary'),
('vocabulary.approve', 'Approve vocabulary suggestions', 'Vocabulary'),
('vocabulary.manage', 'Full vocabulary management', 'Vocabulary'),

-- Learning Permissions
('learning.access', 'Access learning modules', 'Learning'),
('learning.track.own', 'Track own progress', 'Learning'),
('learning.track.team', 'Track team progress', 'Learning'),
('learning.track.all', 'Track all users progress', 'Learning'),
('learning.content.create', 'Create learning content', 'Learning'),
('learning.content.manage', 'Manage all learning content', 'Learning'),

-- Test and Exam Permissions
('test.take', 'Take tests and exams', 'Testing'),
('test.create.basic', 'Create basic tests', 'Testing'),
('test.create.official', 'Create official tests', 'Testing'),
('test.grade.own', 'View own grades', 'Testing'),
('test.grade.team', 'View and grade team tests', 'Testing'),
('test.grade.all', 'View and grade all tests', 'Testing'),

-- Report Permissions
('report.view.own', 'View own reports', 'Analytics'),
('report.view.team', 'View team reports', 'Analytics'),
('report.view.department', 'View department reports', 'Analytics'),
('report.view.all', 'View all reports', 'Analytics'),
('report.generate.basic', 'Generate basic reports', 'Analytics'),
('report.generate.advanced', 'Generate advanced reports', 'Analytics'),

-- Gamification Permissions
('gamification.participate', 'Participate in gamification', 'Gamification'),
('gamification.create.challenges', 'Create challenges', 'Gamification'),
('gamification.manage', 'Manage gamification system', 'Gamification'),

-- Social Features Permissions
('social.view', 'View social content', 'Social'),
('social.post', 'Create posts and comments', 'Social'),
('social.moderate', 'Moderate social content', 'Social'),

-- System Permissions
('system.settings.view', 'View system settings', 'System'),
('system.settings.manage', 'Manage system settings', 'System'),
('system.logs.view', 'View system logs', 'System'),
('system.backup', 'Perform system backup', 'System');

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

-- Insert sample departments
INSERT INTO Departments (DepartmentName, DepartmentCode, Description) VALUES
('IT Department', 'IT', 'Information Technology Department'),
('HR Department', 'HR', 'Human Resources Department'),
('Sales Department', 'SALES', 'Sales and Marketing Department'),
('Engineering Department', 'ENG', 'Engineering and Development Department');

-- Insert permission groups
INSERT INTO PermissionGroups (GroupName, Description) VALUES
('Basic User Access', 'Basic permissions for all users'),
('Content Management', 'Permissions for content creation and management'),
('Team Management', 'Permissions for team leaders'),
('Department Management', 'Permissions for department managers'),
('System Administration', 'Full system administration permissions');

GO

-- Create triggers for UpdatedAt columns
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

-- Create similar triggers for all tables with UpdatedAt column
-- Example for Vocabulary table
CREATE TRIGGER trg_Vocabulary_UpdatedAt
ON Vocabulary
AFTER UPDATE
AS
BEGIN
    UPDATE Vocabulary
    SET UpdatedAt = GETDATE(),
        LastModifiedAt = GETDATE()
    FROM Vocabulary v
    INNER JOIN inserted i ON v.VocabularyID = i.VocabularyID;
END;
GO

-- Note: Similar triggers should be created for all tables with UpdatedAt column

PRINT 'LexiFlow Database created successfully with ROWVERSION columns added to all tables!';