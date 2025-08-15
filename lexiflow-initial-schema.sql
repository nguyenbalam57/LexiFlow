-- ===============================================================================
-- LexiFlow Initial Migration Script - .NET 9 Optimized
-- ===============================================================================
-- T?o database schema ban ??u cho LexiFlow v?i t?i ?u hóa .NET 9
-- Bao g?m t?t c? tables, indexes, constraints và triggers c?n thi?t
-- ===============================================================================

USE [master]
GO

-- T?o database LexiFlow n?u ch?a t?n t?i
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'LexiFlow')
BEGIN
    CREATE DATABASE [LexiFlow]
    COLLATE SQL_Latin1_General_CP1_CI_AS
END
GO

USE [LexiFlow]
GO

-- ===============================================================================
-- ?? DATABASE CONFIGURATION
-- ===============================================================================

-- Enable snapshot isolation for better concurrency
ALTER DATABASE [LexiFlow] SET ALLOW_SNAPSHOT_ISOLATION ON
GO
ALTER DATABASE [LexiFlow] SET READ_COMMITTED_SNAPSHOT ON
GO

-- Performance optimizations
ALTER DATABASE [LexiFlow] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [LexiFlow] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [LexiFlow] SET AUTO_UPDATE_STATISTICS_ASYNC ON
GO

-- ===============================================================================
-- ?? CORE TABLES - User Management
-- ===============================================================================

-- Departments Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Departments' AND xtype='U')
CREATE TABLE [dbo].[Departments] (
    [DepartmentId] int IDENTITY(1,1) NOT NULL,
    [DepartmentName] nvarchar(100) NOT NULL,
    [Description] nvarchar(255) NULL,
    [IsActive] bit NOT NULL DEFAULT 1,
    [CreatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED ([DepartmentId])
)
GO

-- Teams Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Teams' AND xtype='U')
CREATE TABLE [dbo].[Teams] (
    [TeamId] int IDENTITY(1,1) NOT NULL,
    [TeamName] nvarchar(100) NOT NULL,
    [DepartmentId] int NULL,
    [Description] nvarchar(255) NULL,
    [IsActive] bit NOT NULL DEFAULT 1,
    [CreatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_Teams] PRIMARY KEY CLUSTERED ([TeamId]),
    CONSTRAINT [FK_Teams_Departments] FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[Departments] ([DepartmentId])
)
GO

-- Roles Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Roles' AND xtype='U')
CREATE TABLE [dbo].[Roles] (
    [RoleId] int IDENTITY(1,1) NOT NULL,
    [RoleName] nvarchar(50) NOT NULL,
    [Description] nvarchar(255) NULL,
    [IsActive] bit NOT NULL DEFAULT 1,
    [CreatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([RoleId])
)
GO

-- Permissions Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Permissions' AND xtype='U')
CREATE TABLE [dbo].[Permissions] (
    [PermissionId] int IDENTITY(1,1) NOT NULL,
    [PermissionName] nvarchar(100) NOT NULL,
    [Description] nvarchar(255) NULL,
    [IsActive] bit NOT NULL DEFAULT 1,
    [CreatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED ([PermissionId])
)
GO

-- Users Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
CREATE TABLE [dbo].[Users] (
    [UserId] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(50) NOT NULL,
    [Email] nvarchar(255) NOT NULL,
    [PasswordHash] nvarchar(255) NOT NULL,
    [DepartmentId] int NULL,
    [PreferredLanguage] nvarchar(10) NOT NULL DEFAULT 'en',
    [TimeZone] nvarchar(50) NOT NULL DEFAULT 'UTC',
    [LastLoginAt] datetime2(7) NULL,
    [LastLoginIP] nvarchar(45) NULL,
    [IsActive] bit NOT NULL DEFAULT 1,
    [CreatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId]),
    CONSTRAINT [FK_Users_Departments] FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[Departments] ([DepartmentId])
)
GO

-- UserProfiles Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserProfiles' AND xtype='U')
CREATE TABLE [dbo].[UserProfiles] (
    [ProfileId] int IDENTITY(1,1) NOT NULL,
    [UserId] int NOT NULL,
    [FirstName] nvarchar(50) NULL,
    [LastName] nvarchar(50) NULL,
    [DisplayName] nvarchar(100) NULL,
    [DateOfBirth] date NULL,
    [Gender] nvarchar(10) NULL,
    [PhoneNumber] nvarchar(20) NULL,
    [Address] nvarchar(500) NULL,
    [IsActive] bit NOT NULL DEFAULT 1,
    [CreatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_UserProfiles] PRIMARY KEY CLUSTERED ([ProfileId]),
    CONSTRAINT [FK_UserProfiles_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE CASCADE
)
GO

-- ===============================================================================
-- ?? LEARNING CONTENT TABLES
-- ===============================================================================

-- Categories Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Categories' AND xtype='U')
CREATE TABLE [dbo].[Categories] (
    [CategoryId] int IDENTITY(1,1) NOT NULL,
    [CategoryName] nvarchar(100) NOT NULL,
    [Description] nvarchar(255) NULL,
    [Level] nvarchar(20) NULL,
    [ParentCategoryId] int NULL,
    [CategoryType] nvarchar(50) NULL,
    [IsActive] bit NOT NULL DEFAULT 1,
    [CreatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([CategoryId]),
    CONSTRAINT [FK_Categories_ParentCategory] FOREIGN KEY ([ParentCategoryId]) REFERENCES [dbo].[Categories] ([CategoryId])
)
GO

-- Vocabularies Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Vocabularies' AND xtype='U')
CREATE TABLE [dbo].[Vocabularies] (
    [VocabularyId] int IDENTITY(1,1) NOT NULL,
    [Term] nvarchar(100) NOT NULL,
    [Reading] nvarchar(200) NULL,
    [LanguageCode] nvarchar(10) NOT NULL DEFAULT 'ja',
    [Level] nvarchar(20) NULL,
    [CategoryId] int NULL,
    [PartOfSpeech] nvarchar(50) NULL,
    [FrequencyRank] int NULL,
    [PopularityScore] float NULL,
    [DifficultyLevel] int NULL,
    [IsCommon] bit NOT NULL DEFAULT 0,
    [IsActive] bit NOT NULL DEFAULT 1,
    [IsDeleted] bit NOT NULL DEFAULT 0,
    [DeletedAt] datetime2(7) NULL,
    [DeletedBy] int NULL,
    [CreatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] int NULL,
    [ModifiedBy] int NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_Vocabularies] PRIMARY KEY CLUSTERED ([VocabularyId]),
    CONSTRAINT [FK_Vocabularies_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([CategoryId]),
    CONSTRAINT [FK_Vocabularies_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [FK_Vocabularies_ModifiedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [FK_Vocabularies_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[Users] ([UserId])
)
GO

-- Kanjis Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Kanjis' AND xtype='U')
CREATE TABLE [dbo].[Kanjis] (
    [KanjiId] int IDENTITY(1,1) NOT NULL,
    [Character] nvarchar(1) NOT NULL,
    [Meaning] nvarchar(500) NULL,
    [OnReading] nvarchar(200) NULL,
    [KunReading] nvarchar(200) NULL,
    [StrokeCount] int NULL,
    [JLPTLevel] nvarchar(10) NULL,
    [Grade] int NULL,
    [FrequencyRank] int NULL,
    [PopularityScore] float NULL,
    [IsActive] bit NOT NULL DEFAULT 1,
    [CreatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] int NULL,
    [ModifiedBy] int NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_Kanjis] PRIMARY KEY CLUSTERED ([KanjiId]),
    CONSTRAINT [FK_Kanjis_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [FK_Kanjis_ModifiedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[Users] ([UserId])
)
GO

-- ===============================================================================
-- ?? PROGRESS TRACKING TABLES
-- ===============================================================================

-- LearningProgresses Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='LearningProgresses' AND xtype='U')
CREATE TABLE [dbo].[LearningProgresses] (
    [ProgressId] int IDENTITY(1,1) NOT NULL,
    [UserId] int NOT NULL,
    [VocabularyId] int NOT NULL,
    [MasteryLevel] int NOT NULL DEFAULT 0,
    [NextReviewDate] datetime2(7) NULL,
    [LastReviewedAt] datetime2(7) NULL,
    [ReviewCount] int NOT NULL DEFAULT 0,
    [CorrectCount] int NOT NULL DEFAULT 0,
    [IncorrectCount] int NOT NULL DEFAULT 0,
    [IsActive] bit NOT NULL DEFAULT 1,
    [CreatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_LearningProgresses] PRIMARY KEY CLUSTERED ([ProgressId]),
    CONSTRAINT [FK_LearningProgresses_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [FK_LearningProgresses_Vocabularies] FOREIGN KEY ([VocabularyId]) REFERENCES [dbo].[Vocabularies] ([VocabularyId])
)
GO

-- UserKanjiProgresses Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserKanjiProgresses' AND xtype='U')
CREATE TABLE [dbo].[UserKanjiProgresses] (
    [ProgressId] int IDENTITY(1,1) NOT NULL,
    [UserId] int NOT NULL,
    [KanjiId] int NOT NULL,
    [RecognitionLevel] int NOT NULL DEFAULT 0,
    [WritingLevel] int NOT NULL DEFAULT 0,
    [NextReviewDate] datetime2(7) NULL,
    [LastPracticed] datetime2(7) NULL,
    [PracticeCount] int NOT NULL DEFAULT 0,
    [CorrectCount] int NOT NULL DEFAULT 0,
    [IsActive] bit NOT NULL DEFAULT 1,
    [CreatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_UserKanjiProgresses] PRIMARY KEY CLUSTERED ([ProgressId]),
    CONSTRAINT [FK_UserKanjiProgresses_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [FK_UserKanjiProgresses_Kanjis] FOREIGN KEY ([KanjiId]) REFERENCES [dbo].[Kanjis] ([KanjiId])
)
GO

-- ===============================================================================
-- ?? STUDY PLANNING TABLES
-- ===============================================================================

-- StudyPlans Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='StudyPlans' AND xtype='U')
CREATE TABLE [dbo].[StudyPlans] (
    [PlanId] int IDENTITY(1,1) NOT NULL,
    [UserId] int NOT NULL,
    [PlanName] nvarchar(100) NOT NULL,
    [Description] nvarchar(1000) NULL,
    [StartDate] datetime2(7) NOT NULL,
    [TargetDate] datetime2(7) NULL,
    [IsActive] bit NOT NULL DEFAULT 1,
    [CreatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] int NULL,
    [ModifiedBy] int NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_StudyPlans] PRIMARY KEY CLUSTERED ([PlanId]),
    CONSTRAINT [FK_StudyPlans_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [FK_StudyPlans_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [FK_StudyPlans_ModifiedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[Users] ([UserId])
)
GO

-- StudyGoals Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='StudyGoals' AND xtype='U')
CREATE TABLE [dbo].[StudyGoals] (
    [GoalId] int IDENTITY(1,1) NOT NULL,
    [PlanId] int NOT NULL,
    [GoalName] nvarchar(100) NOT NULL,
    [Description] nvarchar(1000) NULL,
    [TargetDate] datetime2(7) NULL,
    [IsCompleted] bit NOT NULL DEFAULT 0,
    [CompletedDate] datetime2(7) NULL,
    [ProgressPercentage] real NOT NULL DEFAULT 0,
    [Status] nvarchar(50) NOT NULL DEFAULT 'NotStarted',
    [Priority] int NOT NULL DEFAULT 1,
    [IsActive] bit NOT NULL DEFAULT 1,
    [CreatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] int NULL,
    [ModifiedBy] int NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_StudyGoals] PRIMARY KEY CLUSTERED ([GoalId]),
    CONSTRAINT [FK_StudyGoals_StudyPlans] FOREIGN KEY ([PlanId]) REFERENCES [dbo].[StudyPlans] ([PlanId]) ON DELETE CASCADE,
    CONSTRAINT [FK_StudyGoals_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [FK_StudyGoals_ModifiedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[Users] ([UserId])
)
GO

-- ===============================================================================
-- ?? PERFORMANCE INDEXES
-- ===============================================================================

-- User Authentication Indexes
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Username] ON [dbo].[Users] ([Username])
GO
CREATE NONCLUSTERED INDEX [IX_Users_Email] ON [dbo].[Users] ([Email])
GO
CREATE NONCLUSTERED INDEX [IX_Users_Active_LastLogin] ON [dbo].[Users] ([IsActive], [LastLoginAt])
GO
CREATE NONCLUSTERED INDEX [IX_Users_Department] ON [dbo].[Users] ([DepartmentId])
GO

-- Vocabulary Learning Indexes
CREATE NONCLUSTERED INDEX [IX_Vocabularies_Term] ON [dbo].[Vocabularies] ([Term])
GO
CREATE NONCLUSTERED INDEX [IX_Vocabularies_Reading] ON [dbo].[Vocabularies] ([Reading])
GO
CREATE NONCLUSTERED INDEX [IX_Vocabularies_Level_Active] ON [dbo].[Vocabularies] ([Level], [IsActive])
GO
CREATE NONCLUSTERED INDEX [IX_Vocabularies_Category_Active] ON [dbo].[Vocabularies] ([CategoryId], [IsActive])
GO
CREATE NONCLUSTERED INDEX [IX_Vocabularies_FrequencyRank] ON [dbo].[Vocabularies] ([FrequencyRank])
GO

-- Kanji Learning Indexes
CREATE UNIQUE NONCLUSTERED INDEX [IX_Kanjis_Character] ON [dbo].[Kanjis] ([Character])
GO
CREATE NONCLUSTERED INDEX [IX_Kanjis_JLPTLevel_Active] ON [dbo].[Kanjis] ([JLPTLevel], [IsActive])
GO
CREATE NONCLUSTERED INDEX [IX_Kanjis_StrokeCount] ON [dbo].[Kanjis] ([StrokeCount])
GO

-- Learning Progress Indexes
CREATE UNIQUE NONCLUSTERED INDEX [IX_LearningProgresses_User_Vocabulary] ON [dbo].[LearningProgresses] ([UserId], [VocabularyId])
GO
CREATE NONCLUSTERED INDEX [IX_LearningProgresses_User_NextReview] ON [dbo].[LearningProgresses] ([UserId], [NextReviewDate])
GO
CREATE NONCLUSTERED INDEX [IX_LearningProgresses_Mastery] ON [dbo].[LearningProgresses] ([MasteryLevel])
GO

-- Kanji Progress Indexes
CREATE UNIQUE NONCLUSTERED INDEX [IX_UserKanjiProgresses_User_Kanji] ON [dbo].[UserKanjiProgresses] ([UserId], [KanjiId])
GO
CREATE NONCLUSTERED INDEX [IX_UserKanjiProgresses_User_NextReview] ON [dbo].[UserKanjiProgresses] ([UserId], [NextReviewDate])
GO

-- Study Planning Indexes
CREATE NONCLUSTERED INDEX [IX_StudyPlans_User_Active] ON [dbo].[StudyPlans] ([UserId], [IsActive])
GO
CREATE NONCLUSTERED INDEX [IX_StudyPlans_DateRange] ON [dbo].[StudyPlans] ([StartDate], [TargetDate])
GO
CREATE NONCLUSTERED INDEX [IX_StudyGoals_Plan_Completed] ON [dbo].[StudyGoals] ([PlanId], [IsCompleted])
GO
CREATE NONCLUSTERED INDEX [IX_StudyGoals_Status_Priority] ON [dbo].[StudyGoals] ([Status], [Priority])
GO

-- Category Indexes
CREATE NONCLUSTERED INDEX [IX_Categories_Name] ON [dbo].[Categories] ([CategoryName])
GO
CREATE NONCLUSTERED INDEX [IX_Categories_Parent_Active] ON [dbo].[Categories] ([ParentCategoryId], [IsActive])
GO

-- ===============================================================================
-- ?? INITIAL DATA SEEDING
-- ===============================================================================

-- Insert default departments
IF NOT EXISTS (SELECT 1 FROM [dbo].[Departments] WHERE [DepartmentName] = 'IT')
INSERT INTO [dbo].[Departments] ([DepartmentName], [Description]) VALUES ('IT', 'Information Technology Department')

IF NOT EXISTS (SELECT 1 FROM [dbo].[Departments] WHERE [DepartmentName] = 'HR')
INSERT INTO [dbo].[Departments] ([DepartmentName], [Description]) VALUES ('HR', 'Human Resources Department')

-- Insert default roles
IF NOT EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE [RoleName] = 'Administrator')
INSERT INTO [dbo].[Roles] ([RoleName], [Description]) VALUES ('Administrator', 'System Administrator')

IF NOT EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE [RoleName] = 'Teacher')
INSERT INTO [dbo].[Roles] ([RoleName], [Description]) VALUES ('Teacher', 'Language Teacher')

IF NOT EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE [RoleName] = 'Student')
INSERT INTO [dbo].[Roles] ([RoleName], [Description]) VALUES ('Student', 'Language Student')

-- Insert JLPT categories
IF NOT EXISTS (SELECT 1 FROM [dbo].[Categories] WHERE [CategoryName] = 'JLPT N5')
INSERT INTO [dbo].[Categories] ([CategoryName], [Description], [Level], [CategoryType]) 
VALUES ('JLPT N5', 'Japanese Language Proficiency Test Level N5', '5', 'Vocabulary')

IF NOT EXISTS (SELECT 1 FROM [dbo].[Categories] WHERE [CategoryName] = 'JLPT N4')
INSERT INTO [dbo].[Categories] ([CategoryName], [Description], [Level], [CategoryType]) 
VALUES ('JLPT N4', 'Japanese Language Proficiency Test Level N4', '4', 'Vocabulary')

IF NOT EXISTS (SELECT 1 FROM [dbo].[Categories] WHERE [CategoryName] = 'JLPT N3')
INSERT INTO [dbo].[Categories] ([CategoryName], [Description], [Level], [CategoryType]) 
VALUES ('JLPT N3', 'Japanese Language Proficiency Test Level N3', '3', 'Vocabulary')

IF NOT EXISTS (SELECT 1 FROM [dbo].[Categories] WHERE [CategoryName] = 'JLPT N2')
INSERT INTO [dbo].[Categories] ([CategoryName], [Description], [Level], [CategoryType]) 
VALUES ('JLPT N2', 'Japanese Language Proficiency Test Level N2', '2', 'Vocabulary')

IF NOT EXISTS (SELECT 1 FROM [dbo].[Categories] WHERE [CategoryName] = 'JLPT N1')
INSERT INTO [dbo].[Categories] ([CategoryName], [Description], [Level], [CategoryType]) 
VALUES ('JLPT N1', 'Japanese Language Proficiency Test Level N1', '1', 'Vocabulary')

-- ===============================================================================
-- ?? COMPLETION MESSAGE
-- ===============================================================================

PRINT '? LexiFlow Database Schema Created Successfully!'
PRINT '?? Core tables, indexes, and initial data have been set up.'
PRINT '?? Ready for .NET 9 Entity Framework migrations!'

GO