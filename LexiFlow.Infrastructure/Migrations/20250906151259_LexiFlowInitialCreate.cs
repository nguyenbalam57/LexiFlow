using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LexiFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LexiFlowInitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Level = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    IconPath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CategoryType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsRestricted = table.Column<bool>(type: "bit", nullable: false),
                    AllowedRoles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AllowedUserIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemCount = table.Column<int>(type: "int", nullable: true),
                    LastUpdatedContent = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IconPath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    GroupType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    MaxMembers = table.Column<int>(type: "int", nullable: true),
                    JoinPolicy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AllowSelfJoin = table.Column<bool>(type: "bit", nullable: false),
                    ParentGroupId = table.Column<int>(type: "int", nullable: true),
                    IsNested = table.Column<bool>(type: "bit", nullable: false),
                    DefaultPermissions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSystemGroup = table.Column<bool>(type: "bit", nullable: false),
                    IsSynchronized = table.Column<bool>(type: "bit", nullable: false),
                    SyncSource = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MemberCount = table.Column<int>(type: "int", nullable: false),
                    LastActivityDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_Groups_Groups_ParentGroupId",
                        column: x => x.ParentGroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId");
                });

            migrationBuilder.CreateTable(
                name: "JLPTLevels",
                columns: table => new
                {
                    LevelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LevelName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VocabularyCount = table.Column<int>(type: "int", nullable: true),
                    KanjiCount = table.Column<int>(type: "int", nullable: true),
                    GrammarPoints = table.Column<int>(type: "int", nullable: true),
                    PassingScore = table.Column<int>(type: "int", nullable: true),
                    RequiredSkills = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CEFREquivalent = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    RecommendedStudyHours = table.Column<int>(type: "int", nullable: true),
                    PrerequisiteLevelId = table.Column<int>(type: "int", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JLPTLevels", x => x.LevelId);
                    table.ForeignKey(
                        name: "FK_JLPTLevels_JLPTLevels_PrerequisiteLevelId",
                        column: x => x.PrerequisiteLevelId,
                        principalTable: "JLPTLevels",
                        principalColumn: "LevelId");
                });

            migrationBuilder.CreateTable(
                name: "MediaCategories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    MediaTypes = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaxFileSizeBytes = table.Column<long>(type: "bigint", nullable: true),
                    AllowedExtensions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoragePath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StorageType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StorageOptions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnableCompression = table.Column<bool>(type: "bit", nullable: false),
                    CreateThumbnails = table.Column<bool>(type: "bit", nullable: false),
                    ProcessingRules = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    ViewPermissions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadPermissions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    FileCount = table.Column<int>(type: "int", nullable: true),
                    TotalSizeBytes = table.Column<long>(type: "bigint", nullable: true),
                    LastUpload = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaCategories", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK_MediaCategories_MediaCategories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "MediaCategories",
                        principalColumn: "CategoryId");
                });

            migrationBuilder.CreateTable(
                name: "NotificationPriorities",
                columns: table => new
                {
                    PriorityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriorityName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    ColorCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EnableInterruption = table.Column<bool>(type: "bit", nullable: false),
                    PersistUntilAction = table.Column<bool>(type: "bit", nullable: false),
                    RepeatReminderMinutes = table.Column<int>(type: "int", nullable: true),
                    ForceEmail = table.Column<bool>(type: "bit", nullable: false),
                    ForcePush = table.Column<bool>(type: "bit", nullable: false),
                    ForceSMS = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationPriorities", x => x.PriorityId);
                });

            migrationBuilder.CreateTable(
                name: "NotificationStatuses",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsTerminal = table.Column<bool>(type: "bit", nullable: false),
                    RequiresAction = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    AllowedTransitions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransitionActions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationStatuses", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IconPath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TemplateSubject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateParams = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnableInApp = table.Column<bool>(type: "bit", nullable: false),
                    EnableEmail = table.Column<bool>(type: "bit", nullable: false),
                    EnablePush = table.Column<bool>(type: "bit", nullable: false),
                    EnableSMS = table.Column<bool>(type: "bit", nullable: false),
                    DisplayDurationSeconds = table.Column<int>(type: "int", nullable: false),
                    RequiresAcknowledgment = table.Column<bool>(type: "bit", nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "PermissionGroups",
                columns: table => new
                {
                    PermissionGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ModuleArea = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IconPath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ParentGroupId = table.Column<int>(type: "int", nullable: true),
                    IsSystemGroup = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGroups", x => x.PermissionGroupId);
                    table.ForeignKey(
                        name: "FK_PermissionGroups_PermissionGroups_ParentGroupId",
                        column: x => x.ParentGroupId,
                        principalTable: "PermissionGroups",
                        principalColumn: "PermissionGroupId");
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Module = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ResourceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsSystemPermission = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.PermissionId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ParentRoleId = table.Column<int>(type: "int", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    IsSystemRole = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_Roles_Roles_ParentRoleId",
                        column: x => x.ParentRoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "ScheduleItemTypes",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IconPath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DefaultColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IconClass = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DefaultDurationMinutes = table.Column<int>(type: "int", nullable: true),
                    DefaultReminders = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AllowOverlap = table.Column<bool>(type: "bit", nullable: false),
                    RequiresApproval = table.Column<bool>(type: "bit", nullable: false),
                    CompletionRules = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateFields = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleItemTypes", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleRecurrences",
                columns: table => new
                {
                    RecurrenceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecurrenceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Interval = table.Column<int>(type: "int", nullable: true),
                    DaysOfWeek = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DayOfMonth = table.Column<int>(type: "int", nullable: true),
                    MonthOfYear = table.Column<int>(type: "int", nullable: true),
                    CustomPattern = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IncludeWeekends = table.Column<bool>(type: "bit", nullable: false),
                    ExcludeDates = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IncludeDates = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExcludeHolidays = table.Column<bool>(type: "bit", nullable: false),
                    HolidayAdjustmentRule = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeekOfMonth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DayPosition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecurrenceEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaxOccurrences = table.Column<int>(type: "int", nullable: true),
                    ExceptionRules = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExceptionDates = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleRecurrences", x => x.RecurrenceId);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    SettingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SettingKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SettingValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Group = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ValidationRules = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEditable = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    RequiresRestart = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    AccessRoles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsGlobal = table.Column<bool>(type: "bit", nullable: false),
                    IsUserOverridable = table.Column<bool>(type: "bit", nullable: false),
                    VersionAdded = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VersionDeprecated = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.SettingId);
                });

            migrationBuilder.CreateTable(
                name: "Grammars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pattern = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Reading = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Level = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    DifficultyLevel = table.Column<int>(type: "int", nullable: false),
                    GrammarType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FormType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsNegative = table.Column<bool>(type: "bit", nullable: false),
                    IsPast = table.Column<bool>(type: "bit", nullable: false),
                    FormationRules = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsageNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommonMistakes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelatedPatterns = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentGrammarId = table.Column<int>(type: "int", nullable: true),
                    Shortcut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mnemonic = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grammars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grammars_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK_Grammars_Grammars_ParentGrammarId",
                        column: x => x.ParentGrammarId,
                        principalTable: "Grammars",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TechnicalTerms",
                columns: table => new
                {
                    TechnicalTermId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Term = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Reading = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Field = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SubField = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Definition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelatedTerms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TermType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Etymology = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    Specificity = table.Column<int>(type: "int", nullable: true),
                    IsStandardTerm = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalTerms", x => x.TechnicalTermId);
                    table.ForeignKey(
                        name: "FK_TechnicalTerms_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                });

            migrationBuilder.CreateTable(
                name: "VocabularyGroups",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    GroupType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IconPath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    SortingMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IncludeInPractice = table.Column<bool>(type: "bit", nullable: false),
                    IncludeInTests = table.Column<bool>(type: "bit", nullable: false),
                    SuggestedStudyTimeMinutes = table.Column<int>(type: "int", nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    AllowedRoles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocabularyGroups", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_VocabularyGroups_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                });

            migrationBuilder.CreateTable(
                name: "Definitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VocabularyId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PartOfSpeech = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Definitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeletedItems",
                columns: table => new
                {
                    DeletedItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BackupData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedItems", x => x.DeletedItemID);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DepartmentCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ParentDepartmentId = table.Column<int>(type: "int", nullable: true),
                    ManagerUserId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CostCenter = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HeadCount = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                    table.ForeignKey(
                        name: "FK_Departments_Departments_ParentDepartmentId",
                        column: x => x.ParentDepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsMfaEnabled = table.Column<bool>(type: "bit", nullable: false),
                    MfaKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLoginIP = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    PreferredLanguage = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TimeZone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DailyGoalMinutes = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "GrammarDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrammarId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Usage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    DefinitionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    Context = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Limitations = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Caution = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Source = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrammarDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrammarDefinitions_Grammars_GrammarId",
                        column: x => x.GrammarId,
                        principalTable: "Grammars",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GrammarDefinitions_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_GrammarDefinitions_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "GrammarExamples",
                columns: table => new
                {
                    GrammarExampleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrammarId = table.Column<int>(type: "int", nullable: false),
                    JapaneseSentence = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Reading = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TranslationText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ExampleType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Context = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: true),
                    GrammarBreakdown = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlternativeSentence = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WordsToNote = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AudioFile = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrammarExamples", x => x.GrammarExampleId);
                    table.ForeignKey(
                        name: "FK_GrammarExamples_Grammars_GrammarId",
                        column: x => x.GrammarId,
                        principalTable: "Grammars",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GrammarExamples_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_GrammarExamples_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "GrammarTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrammarId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    TranslationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Context = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrammarTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrammarTranslations_Grammars_GrammarId",
                        column: x => x.GrammarId,
                        principalTable: "Grammars",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GrammarTranslations_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_GrammarTranslations_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_GrammarTranslations_Users_VerifiedBy",
                        column: x => x.VerifiedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "GroupPermissions",
                columns: table => new
                {
                    GroupPermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    GrantedByUserId = table.Column<int>(type: "int", nullable: true),
                    GrantedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Scope = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    GrantReason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPermissions", x => x.GroupPermissionId);
                    table.ForeignKey(
                        name: "FK_GroupPermissions_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId");
                    table.ForeignKey(
                        name: "FK_GroupPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId");
                    table.ForeignKey(
                        name: "FK_GroupPermissions_Users_GrantedByUserId",
                        column: x => x.GrantedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "JLPTExams",
                columns: table => new
                {
                    ExamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Level = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LevelId = table.Column<int>(type: "int", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true),
                    Month = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TotalTime = table.Column<int>(type: "int", nullable: true),
                    TotalScore = table.Column<int>(type: "int", nullable: true),
                    TotalQuestions = table.Column<int>(type: "int", nullable: true),
                    PassingScore = table.Column<int>(type: "int", nullable: true),
                    HasListeningSection = table.Column<bool>(type: "bit", nullable: false),
                    HasReadingSection = table.Column<bool>(type: "bit", nullable: false),
                    HasVocabularySection = table.Column<bool>(type: "bit", nullable: false),
                    HasGrammarSection = table.Column<bool>(type: "bit", nullable: false),
                    ExamVersion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsOfficial = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JLPTExams", x => x.ExamId);
                    table.ForeignKey(
                        name: "FK_JLPTExams_JLPTLevels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "JLPTLevels",
                        principalColumn: "LevelId");
                    table.ForeignKey(
                        name: "FK_JLPTExams_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "KanjiComponents",
                columns: table => new
                {
                    ComponentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Character = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Meaning = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StrokeCount = table.Column<int>(type: "int", nullable: false),
                    UnicodeHex = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Mnemonics = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StrokeOrder = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VariantOf = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsCommon = table.Column<bool>(type: "bit", nullable: false),
                    Example = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Variants = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DeprecatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KanjiComponents", x => x.ComponentId);
                    table.ForeignKey(
                        name: "FK_KanjiComponents_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Kanjis",
                columns: table => new
                {
                    KanjiId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Character = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OnYomi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    KunYomi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OtherReadings = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    StrokeCount = table.Column<int>(type: "int", nullable: false),
                    JLPTLevel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RadicalName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RadicalNumber = table.Column<int>(type: "int", nullable: true),
                    RadicalCharacter = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    RadicalStrokeCount = table.Column<int>(type: "int", nullable: true),
                    StrokeOrder = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StrokeOrderJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnicodeHex = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JisCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Mnemonics = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Etymology = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    HistoricalEvolution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FrequencyRank = table.Column<int>(type: "int", nullable: true),
                    DifficultyLevel = table.Column<int>(type: "int", nullable: true),
                    KanjiGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JouyouClassification = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    SearchKeywords = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TagsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelatedKanjiJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VariantsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComponentsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataSource = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PopularityScore = table.Column<int>(type: "int", nullable: false),
                    StudyCount = table.Column<int>(type: "int", nullable: false),
                    FavoriteCount = table.Column<int>(type: "int", nullable: false),
                    AverageRating = table.Column<double>(type: "float", nullable: true),
                    RatingCount = table.Column<int>(type: "int", nullable: false),
                    LastContentUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContentVersion = table.Column<int>(type: "int", nullable: false),
                    NeedsReview = table.Column<bool>(type: "bit", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerifiedBy = table.Column<int>(type: "int", nullable: true),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MetadataJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kanjis", x => x.KanjiId);
                    table.ForeignKey(
                        name: "FK_Kanjis_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK_Kanjis_Users_VerifiedBy",
                        column: x => x.VerifiedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderUserId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    PriorityId = table.Column<int>(type: "int", nullable: true),
                    RichContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataPayload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UseFullWidth = table.Column<bool>(type: "bit", nullable: false),
                    ThemeColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AllowResponses = table.Column<bool>(type: "bit", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsSystemGenerated = table.Column<bool>(type: "bit", nullable: false),
                    IsScheduled = table.Column<bool>(type: "bit", nullable: false),
                    ScheduledFor = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelivered = table.Column<bool>(type: "bit", nullable: false),
                    DeliveredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RelatedEntityType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelatedEntityId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_NotificationPriorities_PriorityId",
                        column: x => x.PriorityId,
                        principalTable: "NotificationPriorities",
                        principalColumn: "PriorityId");
                    table.ForeignKey(
                        name: "FK_Notifications_NotificationTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "NotificationTypes",
                        principalColumn: "TypeId");
                    table.ForeignKey(
                        name: "FK_Notifications_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Notifications_Users_SenderUserId",
                        column: x => x.SenderUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "PermissionGroupMappings",
                columns: table => new
                {
                    MappingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionGroupId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    SubCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    DependsOnPermissionId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGroupMappings", x => x.MappingId);
                    table.ForeignKey(
                        name: "FK_PermissionGroupMappings_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId");
                    table.ForeignKey(
                        name: "FK_PermissionGroupMappings_PermissionGroups_PermissionGroupId",
                        column: x => x.PermissionGroupId,
                        principalTable: "PermissionGroups",
                        principalColumn: "PermissionGroupId");
                    table.ForeignKey(
                        name: "FK_PermissionGroupMappings_Permissions_DependsOnPermissionId",
                        column: x => x.DependsOnPermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId");
                    table.ForeignKey(
                        name: "FK_PermissionGroupMappings_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId");
                    table.ForeignKey(
                        name: "FK_PermissionGroupMappings_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "PersonalWordLists",
                columns: table => new
                {
                    ListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ListType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IconPath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    SortingMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFavorite = table.Column<bool>(type: "bit", nullable: false),
                    IncludeInReview = table.Column<bool>(type: "bit", nullable: false),
                    IsShared = table.Column<bool>(type: "bit", nullable: false),
                    SharedWith = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemCount = table.Column<int>(type: "int", nullable: true),
                    LastStudied = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MasteryPercentage = table.Column<float>(type: "real", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalWordLists", x => x.ListId);
                    table.ForeignKey(
                        name: "FK_PersonalWordLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RolePermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    Scope = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsOverride = table.Column<bool>(type: "bit", nullable: false),
                    GrantedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.RolePermissionId);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId");
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId");
                    table.ForeignKey(
                        name: "FK_RolePermissions_Users_GrantedBy",
                        column: x => x.GrantedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "StudyPlans",
                columns: table => new
                {
                    StudyPlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PlanName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TargetLevel = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TargetDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinutesPerDay = table.Column<int>(type: "int", nullable: true),
                    PlanType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Intensity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DaysPerWeek = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CurrentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CompletionPercentage = table.Column<float>(type: "real", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SchedulePattern = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExcludedDates = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AutoAdjust = table.Column<bool>(type: "bit", nullable: false),
                    EnableReminders = table.Column<bool>(type: "bit", nullable: false),
                    ReminderSettings = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SyncWithCalendar = table.Column<bool>(type: "bit", nullable: false),
                    IsShared = table.Column<bool>(type: "bit", nullable: false),
                    SharedWith = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPlans", x => x.StudyPlanId);
                    table.ForeignKey(
                        name: "FK_StudyPlans_JLPTLevels_TargetLevel",
                        column: x => x.TargetLevel,
                        principalTable: "JLPTLevels",
                        principalColumn: "LevelId");
                    table.ForeignKey(
                        name: "FK_StudyPlans_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "SyncConflicts",
                columns: table => new
                {
                    ConflictID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    ClientData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServerData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServerUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConflictType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolutionMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DetectedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncConflicts", x => x.ConflictID);
                    table.ForeignKey(
                        name: "FK_SyncConflicts_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "SyncMetadata",
                columns: table => new
                {
                    SyncMetadataID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    LastSyncTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TotalItemsSynced = table.Column<int>(type: "int", nullable: false),
                    NeedsFullSync = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncMetadata", x => x.SyncMetadataID);
                    table.ForeignKey(
                        name: "FK_SyncMetadata_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    LeaderUserId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TeamType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Mission = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FormationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisbandDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaxMembers = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamId);
                    table.ForeignKey(
                        name: "FK_Teams_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId");
                    table.ForeignKey(
                        name: "FK_Teams_Users_LeaderUserId",
                        column: x => x.LeaderUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "TermExamples",
                columns: table => new
                {
                    TermExampleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TermId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Translation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Context = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ExampleType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Scenario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: true),
                    IsCommon = table.Column<bool>(type: "bit", nullable: false),
                    AudioUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermExamples", x => x.TermExampleId);
                    table.ForeignKey(
                        name: "FK_TermExamples_TechnicalTerms_TermId",
                        column: x => x.TermId,
                        principalTable: "TechnicalTerms",
                        principalColumn: "TechnicalTermId");
                    table.ForeignKey(
                        name: "FK_TermExamples_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_TermExamples_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "TermRelations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TermId1 = table.Column<int>(type: "int", nullable: false),
                    TermId2 = table.Column<int>(type: "int", nullable: false),
                    RelationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RelationStrength = table.Column<int>(type: "int", nullable: true),
                    IsBidirectional = table.Column<bool>(type: "bit", nullable: false),
                    ExampleContext = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TermRelations_TechnicalTerms_TermId1",
                        column: x => x.TermId1,
                        principalTable: "TechnicalTerms",
                        principalColumn: "TechnicalTermId");
                    table.ForeignKey(
                        name: "FK_TermRelations_TechnicalTerms_TermId2",
                        column: x => x.TermId2,
                        principalTable: "TechnicalTerms",
                        principalColumn: "TechnicalTermId");
                    table.ForeignKey(
                        name: "FK_TermRelations_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_TermRelations_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_TermRelations_Users_VerifiedBy",
                        column: x => x.VerifiedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "TermTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TermId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    TranslationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Context = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ApprovedBy = table.Column<int>(type: "int", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TermTranslations_TechnicalTerms_TermId",
                        column: x => x.TermId,
                        principalTable: "TechnicalTerms",
                        principalColumn: "TechnicalTermId");
                    table.ForeignKey(
                        name: "FK_TermTranslations_Users_ApprovedBy",
                        column: x => x.ApprovedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_TermTranslations_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_TermTranslations_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "TestResults",
                columns: table => new
                {
                    TestResultId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TestType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TestName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Level = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    TotalQuestions = table.Column<int>(type: "int", nullable: true),
                    CorrectAnswers = table.Column<int>(type: "int", nullable: true),
                    Score = table.Column<int>(type: "int", nullable: false),
                    DurationSeconds = table.Column<int>(type: "int", nullable: true),
                    AccuracyRate = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    AverageResponseTime = table.Column<int>(type: "int", nullable: true),
                    MaxResponseTime = table.Column<int>(type: "int", nullable: true),
                    MinResponseTime = table.Column<int>(type: "int", nullable: true),
                    WeakAreas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StrongAreas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Recommendations = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImprovementRate = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    RankPercentile = table.Column<int>(type: "int", nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResults", x => x.TestResultId);
                    table.ForeignKey(
                        name: "FK_TestResults_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK_TestResults_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserGrammarProgresses",
                columns: table => new
                {
                    ProgressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GrammarId = table.Column<int>(type: "int", nullable: false),
                    UnderstandingLevel = table.Column<int>(type: "int", nullable: false),
                    UsageLevel = table.Column<int>(type: "int", nullable: false),
                    LastStudied = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StudyCount = table.Column<int>(type: "int", nullable: false),
                    TestScore = table.Column<float>(type: "real", nullable: true),
                    NextReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MemoryStrength = table.Column<int>(type: "int", nullable: false),
                    EaseFactor = table.Column<double>(type: "float", nullable: false),
                    IntervalDays = table.Column<int>(type: "int", nullable: false),
                    CorrectCount = table.Column<int>(type: "int", nullable: false),
                    IncorrectCount = table.Column<int>(type: "int", nullable: false),
                    ConsecutiveCorrect = table.Column<int>(type: "int", nullable: false),
                    AverageResponseTimeMs = table.Column<int>(type: "int", nullable: true),
                    RecognitionLevel = table.Column<int>(type: "int", nullable: false),
                    ProductionLevel = table.Column<int>(type: "int", nullable: false),
                    ContextualLevel = table.Column<int>(type: "int", nullable: false),
                    PersonalNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommonMistakes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LearningHistory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBookmarked = table.Column<bool>(type: "bit", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGrammarProgresses", x => x.ProgressId);
                    table.ForeignKey(
                        name: "FK_UserGrammarProgresses_Grammars_GrammarId",
                        column: x => x.GrammarId,
                        principalTable: "Grammars",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserGrammarProgresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    UserGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MemberRole = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvitedBy = table.Column<int>(type: "int", nullable: true),
                    JoinReason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CustomPermissions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CanInvite = table.Column<bool>(type: "bit", nullable: false),
                    CanModifyContent = table.Column<bool>(type: "bit", nullable: false),
                    LastActivity = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActivitySummary = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContributionPoints = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.UserGroupId);
                    table.ForeignKey(
                        name: "FK_UserGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId");
                    table.ForeignKey(
                        name: "FK_UserGroups_Users_InvitedBy",
                        column: x => x.InvitedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_UserGroups_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserLearningPreferences",
                columns: table => new
                {
                    PreferenceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DailyNewWordsGoal = table.Column<int>(type: "int", nullable: false),
                    DailyReviewsGoal = table.Column<int>(type: "int", nullable: false),
                    StudySessionLengthMinutes = table.Column<int>(type: "int", nullable: false),
                    LearningStyle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StudyFocus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SpacedRepetitionAlgorithm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EaseFactorModifier = table.Column<double>(type: "float", nullable: false),
                    ShowHints = table.Column<bool>(type: "bit", nullable: false),
                    ShowFurigana = table.Column<bool>(type: "bit", nullable: false),
                    PreferredDifficulty = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ThemePreference = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FontSize = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AutoDownloadAudio = table.Column<bool>(type: "bit", nullable: false),
                    ShowExplicitContent = table.Column<bool>(type: "bit", nullable: false),
                    PreferSimplifiedContent = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLearningPreferences", x => x.PreferenceId);
                    table.ForeignKey(
                        name: "FK_UserLearningPreferences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserNotificationSettings",
                columns: table => new
                {
                    SettingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EmailNotificationsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    PushNotificationsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    InAppNotificationsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    SmsNotificationsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    StudyRemindersEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AchievementNotificationsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    SystemUpdatesEnabled = table.Column<bool>(type: "bit", nullable: false),
                    NewContentNotificationsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    SocialNotificationsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    StudyReminderFrequency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    QuietHoursStart = table.Column<TimeSpan>(type: "time", nullable: true),
                    QuietHoursEnd = table.Column<TimeSpan>(type: "time", nullable: true),
                    QuietHoursEnabled = table.Column<bool>(type: "bit", nullable: false),
                    DailyReminderTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    WeeklyReminderDay = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomNotificationSettings = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailDigestFrequency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotificationSettings", x => x.SettingId);
                    table.ForeignKey(
                        name: "FK_UserNotificationSettings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                columns: table => new
                {
                    UserPermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    GrantedByUserId = table.Column<int>(type: "int", nullable: true),
                    GrantedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Scope = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsOverride = table.Column<bool>(type: "bit", nullable: false),
                    GrantReason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => x.UserPermissionId);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId");
                    table.ForeignKey(
                        name: "FK_UserPermissions_Users_GrantedByUserId",
                        column: x => x.GrantedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_UserPermissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    UserRoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedBy = table.Column<int>(type: "int", nullable: true),
                    Scope = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RoleId1 = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId");
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId1",
                        column: x => x.RoleId1,
                        principalTable: "Roles",
                        principalColumn: "RoleId");
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_AssignedBy",
                        column: x => x.AssignedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserTechnicalTerms",
                columns: table => new
                {
                    UserTermId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TermId = table.Column<int>(type: "int", nullable: false),
                    ProficiencyLevel = table.Column<int>(type: "int", nullable: false),
                    LastPracticed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StudyCount = table.Column<int>(type: "int", nullable: false),
                    CorrectCount = table.Column<int>(type: "int", nullable: false),
                    NextReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MemoryStrength = table.Column<int>(type: "int", nullable: false),
                    EaseFactor = table.Column<double>(type: "float", nullable: false),
                    IntervalDays = table.Column<int>(type: "int", nullable: false),
                    IncorrectCount = table.Column<int>(type: "int", nullable: false),
                    ConsecutiveCorrect = table.Column<int>(type: "int", nullable: false),
                    AverageResponseTimeMs = table.Column<int>(type: "int", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    UserNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBookmarked = table.Column<bool>(type: "bit", nullable: false),
                    UserDifficultyRating = table.Column<int>(type: "int", nullable: true),
                    UserImportanceRating = table.Column<int>(type: "int", nullable: true),
                    UserTags = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UserCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PersonalExample = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PersonalDefinition = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LearningHistory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTechnicalTerms", x => x.UserTermId);
                    table.ForeignKey(
                        name: "FK_UserTechnicalTerms_TechnicalTerms_TermId",
                        column: x => x.TermId,
                        principalTable: "TechnicalTerms",
                        principalColumn: "TechnicalTermId");
                    table.ForeignKey(
                        name: "FK_UserTechnicalTerms_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Vocabularies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Term = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Reading = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AlternativeReadings = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Level = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    DifficultyLevel = table.Column<int>(type: "int", nullable: false),
                    FrequencyRank = table.Column<int>(type: "int", nullable: false),
                    IpaNotation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsCommon = table.Column<bool>(type: "bit", nullable: false),
                    IsFormal = table.Column<bool>(type: "bit", nullable: false),
                    IsSlang = table.Column<bool>(type: "bit", nullable: false),
                    IsArchaic = table.Column<bool>(type: "bit", nullable: false),
                    PartOfSpeech = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DetailedPartOfSpeech = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PolitenessLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UsageContext = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UsageNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SynonymsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AntonymsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelatedWordsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SearchVector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SearchKeywords = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Source = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PopularityScore = table.Column<int>(type: "int", nullable: false),
                    StudyCount = table.Column<int>(type: "int", nullable: false),
                    FavoriteCount = table.Column<int>(type: "int", nullable: false),
                    AverageRating = table.Column<double>(type: "float", nullable: true),
                    RatingCount = table.Column<int>(type: "int", nullable: false),
                    LastContentUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContentVersion = table.Column<int>(type: "int", nullable: false),
                    NeedsReview = table.Column<bool>(type: "bit", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerifiedBy = table.Column<int>(type: "int", nullable: true),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MetadataJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VocabularyGroupGroupId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vocabularies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vocabularies_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Vocabularies_Users_VerifiedBy",
                        column: x => x.VerifiedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Vocabularies_VocabularyGroups_VocabularyGroupGroupId",
                        column: x => x.VocabularyGroupGroupId,
                        principalTable: "VocabularyGroups",
                        principalColumn: "GroupId");
                });

            migrationBuilder.CreateTable(
                name: "JLPTSections",
                columns: table => new
                {
                    SectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    SectionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SectionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderNumber = table.Column<int>(type: "int", nullable: true),
                    TimeAllocation = table.Column<int>(type: "int", nullable: true),
                    ScoreAllocation = table.Column<int>(type: "int", nullable: true),
                    QuestionCount = table.Column<int>(type: "int", nullable: true),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    WeightPercentage = table.Column<double>(type: "float", nullable: true),
                    PassingScore = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JLPTSections", x => x.SectionId);
                    table.ForeignKey(
                        name: "FK_JLPTSections_JLPTExams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "JLPTExams",
                        principalColumn: "ExamId");
                });

            migrationBuilder.CreateTable(
                name: "UserExams",
                columns: table => new
                {
                    UserExamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ExamId = table.Column<int>(type: "int", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TotalQuestions = table.Column<int>(type: "int", nullable: true),
                    CorrectAnswers = table.Column<int>(type: "int", nullable: true),
                    IncorrectAnswers = table.Column<int>(type: "int", nullable: true),
                    SkippedAnswers = table.Column<int>(type: "int", nullable: true),
                    ScorePercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ScorePoints = table.Column<int>(type: "int", nullable: true),
                    IsPassed = table.Column<bool>(type: "bit", nullable: true),
                    Grade = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentQuestionIndex = table.Column<int>(type: "int", nullable: true),
                    BookmarkedQuestions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlaggedQuestions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsTimeLimited = table.Column<bool>(type: "bit", nullable: false),
                    TimeLimit = table.Column<int>(type: "int", nullable: true),
                    TimeRemaining = table.Column<int>(type: "int", nullable: true),
                    QuestionTimeSpent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PauseCount = table.Column<int>(type: "int", nullable: false),
                    TotalPauseTime = table.Column<int>(type: "int", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExams", x => x.UserExamId);
                    table.ForeignKey(
                        name: "FK_UserExams_JLPTExams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "JLPTExams",
                        principalColumn: "ExamId");
                    table.ForeignKey(
                        name: "FK_UserExams_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "KanjiComponentMappings",
                columns: table => new
                {
                    MappingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KanjiId = table.Column<int>(type: "int", nullable: false),
                    ComponentId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    X = table.Column<float>(type: "real", nullable: true),
                    Y = table.Column<float>(type: "real", nullable: true),
                    Width = table.Column<float>(type: "real", nullable: true),
                    Height = table.Column<float>(type: "real", nullable: true),
                    MeaningContribution = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PronunciationContribution = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KanjiComponentMappings", x => x.MappingId);
                    table.ForeignKey(
                        name: "FK_KanjiComponentMappings_KanjiComponents_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "KanjiComponents",
                        principalColumn: "ComponentId");
                    table.ForeignKey(
                        name: "FK_KanjiComponentMappings_Kanjis_KanjiId",
                        column: x => x.KanjiId,
                        principalTable: "Kanjis",
                        principalColumn: "KanjiId");
                });

            migrationBuilder.CreateTable(
                name: "KanjiExamples",
                columns: table => new
                {
                    KanjiExampleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KanjiId = table.Column<int>(type: "int", nullable: false),
                    Japanese = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Kana = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Translation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TranslationLanguage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExampleType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Context = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: true),
                    AudioUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsCommon = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KanjiExamples", x => x.KanjiExampleId);
                    table.ForeignKey(
                        name: "FK_KanjiExamples_Kanjis_KanjiId",
                        column: x => x.KanjiId,
                        principalTable: "Kanjis",
                        principalColumn: "KanjiId");
                    table.ForeignKey(
                        name: "FK_KanjiExamples_Users_VerifiedBy",
                        column: x => x.VerifiedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "KanjiMeanings",
                columns: table => new
                {
                    MeaningId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KanjiId = table.Column<int>(type: "int", nullable: false),
                    Meaning = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Language = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    MeaningType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Context = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Example = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Frequency = table.Column<int>(type: "int", nullable: true),
                    IsArchaic = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KanjiMeanings", x => x.MeaningId);
                    table.ForeignKey(
                        name: "FK_KanjiMeanings_Kanjis_KanjiId",
                        column: x => x.KanjiId,
                        principalTable: "Kanjis",
                        principalColumn: "KanjiId");
                    table.ForeignKey(
                        name: "FK_KanjiMeanings_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_KanjiMeanings_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserKanjiProgresses",
                columns: table => new
                {
                    ProgressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    KanjiId = table.Column<int>(type: "int", nullable: false),
                    RecognitionLevel = table.Column<int>(type: "int", nullable: false),
                    WritingLevel = table.Column<int>(type: "int", nullable: false),
                    LastPracticed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PracticeCount = table.Column<int>(type: "int", nullable: false),
                    CorrectCount = table.Column<int>(type: "int", nullable: false),
                    NextReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MemoryStrength = table.Column<int>(type: "int", nullable: false),
                    EaseFactor = table.Column<double>(type: "float", nullable: false),
                    IntervalDays = table.Column<int>(type: "int", nullable: false),
                    IncorrectCount = table.Column<int>(type: "int", nullable: false),
                    ConsecutiveCorrect = table.Column<int>(type: "int", nullable: false),
                    AverageResponseTimeMs = table.Column<int>(type: "int", nullable: true),
                    ReadingLevel = table.Column<int>(type: "int", nullable: false),
                    CompoundLevel = table.Column<int>(type: "int", nullable: false),
                    MeaningLevel = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LearningHistory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBookmarked = table.Column<bool>(type: "bit", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserKanjiProgresses", x => x.ProgressId);
                    table.ForeignKey(
                        name: "FK_UserKanjiProgresses_Kanjis_KanjiId",
                        column: x => x.KanjiId,
                        principalTable: "Kanjis",
                        principalColumn: "KanjiId");
                    table.ForeignKey(
                        name: "FK_UserKanjiProgresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "NotificationRecipients",
                columns: table => new
                {
                    RecipientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    GroupId = table.Column<int>(type: "int", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    IsDelivered = table.Column<bool>(type: "bit", nullable: false),
                    DeliveryChannels = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsFailed = table.Column<bool>(type: "bit", nullable: false),
                    FailureReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsOpened = table.Column<bool>(type: "bit", nullable: false),
                    IsClicked = table.Column<bool>(type: "bit", nullable: false),
                    IsActioned = table.Column<bool>(type: "bit", nullable: false),
                    ActionedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActionTaken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    IsPinned = table.Column<bool>(type: "bit", nullable: false),
                    IsFlagged = table.Column<bool>(type: "bit", nullable: false),
                    CustomLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationRecipients", x => x.RecipientId);
                    table.ForeignKey(
                        name: "FK_NotificationRecipients_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId");
                    table.ForeignKey(
                        name: "FK_NotificationRecipients_NotificationStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "NotificationStatuses",
                        principalColumn: "StatusId");
                    table.ForeignKey(
                        name: "FK_NotificationRecipients_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "NotificationId");
                    table.ForeignKey(
                        name: "FK_NotificationRecipients_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ScheduleType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ThemeColor = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EnableReminders = table.Column<bool>(type: "bit", nullable: false),
                    EnableSync = table.Column<bool>(type: "bit", nullable: false),
                    SyncSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SyncSettings = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SharedWith = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ViewPermissions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EditPermissions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    StudyPlanId = table.Column<int>(type: "int", nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalSessions = table.Column<int>(type: "int", nullable: true),
                    SessionPattern = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK_Schedules_StudyPlans_StudyPlanId",
                        column: x => x.StudyPlanId,
                        principalTable: "StudyPlans",
                        principalColumn: "StudyPlanId");
                    table.ForeignKey(
                        name: "FK_Schedules_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserTeams",
                columns: table => new
                {
                    UserTeamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<int>(type: "int", nullable: true),
                    TimeCommitment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AllocationPercentage = table.Column<int>(type: "int", nullable: true),
                    JoinReason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTeams", x => x.UserTeamId);
                    table.ForeignKey(
                        name: "FK_UserTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId");
                    table.ForeignKey(
                        name: "FK_UserTeams_Users_AddedBy",
                        column: x => x.AddedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_UserTeams_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Examples",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VocabularyId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Translation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DifficultyLevel = table.Column<int>(type: "int", nullable: false),
                    Context = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Examples", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Examples_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Examples_Vocabularies_VocabularyId",
                        column: x => x.VocabularyId,
                        principalTable: "Vocabularies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "KanjiVocabularies",
                columns: table => new
                {
                    KanjiVocabularyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KanjiId = table.Column<int>(type: "int", nullable: false),
                    VocabularyId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: true),
                    ReadingUsed = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReadingNotes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UsageType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsIrregularReading = table.Column<bool>(type: "bit", nullable: false),
                    MeaningRole = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StudyNote = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KanjiVocabularies", x => x.KanjiVocabularyId);
                    table.ForeignKey(
                        name: "FK_KanjiVocabularies_Kanjis_KanjiId",
                        column: x => x.KanjiId,
                        principalTable: "Kanjis",
                        principalColumn: "KanjiId");
                    table.ForeignKey(
                        name: "FK_KanjiVocabularies_Vocabularies_VocabularyId",
                        column: x => x.VocabularyId,
                        principalTable: "Vocabularies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LearningProgresses",
                columns: table => new
                {
                    ProgressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VocabularyId = table.Column<int>(type: "int", nullable: false),
                    StudyCount = table.Column<int>(type: "int", nullable: false),
                    CorrectCount = table.Column<int>(type: "int", nullable: false),
                    IncorrectCount = table.Column<int>(type: "int", nullable: false),
                    LastStudied = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstStudied = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MasteredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MemoryStrength = table.Column<int>(type: "int", nullable: false),
                    EaseFactor = table.Column<double>(type: "float", nullable: false),
                    IntervalDays = table.Column<int>(type: "int", nullable: false),
                    ConsecutiveCorrect = table.Column<int>(type: "int", nullable: false),
                    ConsecutiveIncorrect = table.Column<int>(type: "int", nullable: false),
                    NextReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NeedsReview = table.Column<bool>(type: "bit", nullable: false),
                    RecognitionLevel = table.Column<int>(type: "int", nullable: false),
                    WritingLevel = table.Column<int>(type: "int", nullable: false),
                    ListeningLevel = table.Column<int>(type: "int", nullable: false),
                    SpeakingLevel = table.Column<int>(type: "int", nullable: false),
                    MasteryLevel = table.Column<int>(type: "int", nullable: false),
                    AverageResponseTimeMs = table.Column<int>(type: "int", nullable: false),
                    LastResponseTimeMs = table.Column<int>(type: "int", nullable: false),
                    BestResponseTimeMs = table.Column<int>(type: "int", nullable: false),
                    WorstResponseTimeMs = table.Column<int>(type: "int", nullable: false),
                    LearningNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommonMistakesJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalMnemonics = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EncounteredContextsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBookmarked = table.Column<bool>(type: "bit", nullable: false),
                    IsMarkedAsDifficult = table.Column<bool>(type: "bit", nullable: false),
                    IsMarkedAsMastered = table.Column<bool>(type: "bit", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    AppearanceFrequency = table.Column<int>(type: "int", nullable: false),
                    TotalStudyTimeMinutes = table.Column<int>(type: "int", nullable: false),
                    StudyStreak = table.Column<int>(type: "int", nullable: false),
                    MaxStudyStreak = table.Column<int>(type: "int", nullable: false),
                    AccuracyRate = table.Column<double>(type: "float", nullable: false),
                    LearningTrend = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PreferredLearningStyle = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OptimalStudyTime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ConfidenceLevel = table.Column<int>(type: "int", nullable: false),
                    ResetCount = table.Column<int>(type: "int", nullable: false),
                    MetadataJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VocabularyId1 = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningProgresses", x => x.ProgressId);
                    table.ForeignKey(
                        name: "FK_LearningProgresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_LearningProgresses_Vocabularies_VocabularyId",
                        column: x => x.VocabularyId,
                        principalTable: "Vocabularies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LearningProgresses_Vocabularies_VocabularyId1",
                        column: x => x.VocabularyId1,
                        principalTable: "Vocabularies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PersonalWordListItems",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListId = table.Column<int>(type: "int", nullable: false),
                    VocabularyId = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsFavorite = table.Column<bool>(type: "bit", nullable: false),
                    PersonalNote = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PersonalExample = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StudyCount = table.Column<int>(type: "int", nullable: true),
                    CorrectCount = table.Column<int>(type: "int", nullable: true),
                    LastStudied = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MasteryLevel = table.Column<int>(type: "int", nullable: true),
                    PersonalTags = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Difficulty = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalWordListItems", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_PersonalWordListItems_PersonalWordLists_ListId",
                        column: x => x.ListId,
                        principalTable: "PersonalWordLists",
                        principalColumn: "ListId");
                    table.ForeignKey(
                        name: "FK_PersonalWordListItems_Vocabularies_VocabularyId",
                        column: x => x.VocabularyId,
                        principalTable: "Vocabularies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Translations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VocabularyId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Accuracy = table.Column<int>(type: "int", nullable: false),
                    IsMachineTranslated = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Translations_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Translations_Vocabularies_VocabularyId",
                        column: x => x.VocabularyId,
                        principalTable: "Vocabularies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VocabularyRelations",
                columns: table => new
                {
                    RelationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    VocabularyId = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    RelationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Importance = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocabularyRelations", x => x.RelationId);
                    table.ForeignKey(
                        name: "FK_VocabularyRelations_Vocabularies_VocabularyId",
                        column: x => x.VocabularyId,
                        principalTable: "Vocabularies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VocabularyRelations_VocabularyGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "VocabularyGroups",
                        principalColumn: "GroupId");
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionId = table.Column<int>(type: "int", nullable: true),
                    QuestionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionInstruction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Difficulty = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    TimeLimit = table.Column<int>(type: "int", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UsageCount = table.Column<int>(type: "int", nullable: false),
                    AverageResponseTime = table.Column<double>(type: "float", nullable: true),
                    SuccessRate = table.Column<double>(type: "float", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SearchVector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Questions_JLPTSections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "JLPTSections",
                        principalColumn: "SectionId");
                });

            migrationBuilder.CreateTable(
                name: "KanjiExampleMeanings",
                columns: table => new
                {
                    MeaningId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExampleId = table.Column<int>(type: "int", nullable: false),
                    Meaning = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Language = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KanjiExampleMeanings", x => x.MeaningId);
                    table.ForeignKey(
                        name: "FK_KanjiExampleMeanings_KanjiExamples_ExampleId",
                        column: x => x.ExampleId,
                        principalTable: "KanjiExamples",
                        principalColumn: "KanjiExampleId");
                    table.ForeignKey(
                        name: "FK_KanjiExampleMeanings_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "NotificationResponses",
                columns: table => new
                {
                    ResponseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipientId = table.Column<int>(type: "int", nullable: false),
                    ResponseContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ResponseAction = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ResponseData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RespondedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsSystemResponse = table.Column<bool>(type: "bit", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    ResponseTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionTarget = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionCompleted = table.Column<bool>(type: "bit", nullable: false),
                    ActionCompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationResponses", x => x.ResponseId);
                    table.ForeignKey(
                        name: "FK_NotificationResponses_NotificationRecipients_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "NotificationRecipients",
                        principalColumn: "RecipientId");
                    table.ForeignKey(
                        name: "FK_NotificationResponses_Users_RespondedByUserId",
                        column: x => x.RespondedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "QuestionOptions",
                columns: table => new
                {
                    QuestionOptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    OptionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionImage = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    Explanation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectionRate = table.Column<double>(type: "float", nullable: true),
                    DistractorLevel = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionOptions", x => x.QuestionOptionId);
                    table.ForeignKey(
                        name: "FK_QuestionOptions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId");
                });

            migrationBuilder.CreateTable(
                name: "TestDetails",
                columns: table => new
                {
                    TestDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestResultId = table.Column<int>(type: "int", nullable: false),
                    VocabularyId = table.Column<int>(type: "int", nullable: true),
                    QuestionId = table.Column<int>(type: "int", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    TimeSpent = table.Column<int>(type: "int", nullable: true),
                    UserAnswer = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Difficulty = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestDetails", x => x.TestDetailId);
                    table.ForeignKey(
                        name: "FK_TestDetails_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId");
                    table.ForeignKey(
                        name: "FK_TestDetails_TestResults_TestResultId",
                        column: x => x.TestResultId,
                        principalTable: "TestResults",
                        principalColumn: "TestResultId");
                    table.ForeignKey(
                        name: "FK_TestDetails_Vocabularies_VocabularyId",
                        column: x => x.VocabularyId,
                        principalTable: "Vocabularies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MediaFiles",
                columns: table => new
                {
                    MediaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MediaType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StoragePath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    CdnUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MetadataJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VocabularyId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    KanjiId = table.Column<int>(type: "int", nullable: true),
                    ExampleId = table.Column<int>(type: "int", nullable: true),
                    GrammarId = table.Column<int>(type: "int", nullable: true),
                    QuestionId = table.Column<int>(type: "int", nullable: true),
                    QuestionOptionId = table.Column<int>(type: "int", nullable: true),
                    GrammarExampleId = table.Column<int>(type: "int", nullable: true),
                    KanjiExampleId = table.Column<int>(type: "int", nullable: true),
                    TechnicalTermId = table.Column<int>(type: "int", nullable: true),
                    TermExampleId = table.Column<int>(type: "int", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    IsProcessed = table.Column<bool>(type: "bit", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Variants = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    License = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AttributionText = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MediaCategoryCategoryId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFiles", x => x.MediaId);
                    table.ForeignKey(
                        name: "FK_MediaFiles_Examples_ExampleId",
                        column: x => x.ExampleId,
                        principalTable: "Examples",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MediaFiles_GrammarExamples_GrammarExampleId",
                        column: x => x.GrammarExampleId,
                        principalTable: "GrammarExamples",
                        principalColumn: "GrammarExampleId");
                    table.ForeignKey(
                        name: "FK_MediaFiles_Grammars_GrammarId",
                        column: x => x.GrammarId,
                        principalTable: "Grammars",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MediaFiles_KanjiExamples_KanjiExampleId",
                        column: x => x.KanjiExampleId,
                        principalTable: "KanjiExamples",
                        principalColumn: "KanjiExampleId");
                    table.ForeignKey(
                        name: "FK_MediaFiles_Kanjis_KanjiId",
                        column: x => x.KanjiId,
                        principalTable: "Kanjis",
                        principalColumn: "KanjiId");
                    table.ForeignKey(
                        name: "FK_MediaFiles_MediaCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "MediaCategories",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK_MediaFiles_MediaCategories_MediaCategoryCategoryId",
                        column: x => x.MediaCategoryCategoryId,
                        principalTable: "MediaCategories",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK_MediaFiles_QuestionOptions_QuestionOptionId",
                        column: x => x.QuestionOptionId,
                        principalTable: "QuestionOptions",
                        principalColumn: "QuestionOptionId");
                    table.ForeignKey(
                        name: "FK_MediaFiles_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId");
                    table.ForeignKey(
                        name: "FK_MediaFiles_TechnicalTerms_TechnicalTermId",
                        column: x => x.TechnicalTermId,
                        principalTable: "TechnicalTerms",
                        principalColumn: "TechnicalTermId");
                    table.ForeignKey(
                        name: "FK_MediaFiles_TermExamples_TermExampleId",
                        column: x => x.TermExampleId,
                        principalTable: "TermExamples",
                        principalColumn: "TermExampleId");
                    table.ForeignKey(
                        name: "FK_MediaFiles_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_MediaFiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_MediaFiles_Vocabularies_VocabularyId",
                        column: x => x.VocabularyId,
                        principalTable: "Vocabularies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserAnswers",
                columns: table => new
                {
                    AnswerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserExamId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    SelectedOptionId = table.Column<int>(type: "int", nullable: true),
                    UserInput = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    TimeSpent = table.Column<int>(type: "int", nullable: true),
                    Attempt = table.Column<int>(type: "int", nullable: true),
                    ConfidenceLevel = table.Column<int>(type: "int", nullable: true),
                    ThinkingTimeMs = table.Column<int>(type: "int", nullable: true),
                    ChangeCount = table.Column<int>(type: "int", nullable: true),
                    IsFlagged = table.Column<bool>(type: "bit", nullable: false),
                    AnsweredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnswers", x => x.AnswerId);
                    table.ForeignKey(
                        name: "FK_UserAnswers_QuestionOptions_SelectedOptionId",
                        column: x => x.SelectedOptionId,
                        principalTable: "QuestionOptions",
                        principalColumn: "QuestionOptionId");
                    table.ForeignKey(
                        name: "FK_UserAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId");
                    table.ForeignKey(
                        name: "FK_UserAnswers_UserExams_UserExamId",
                        column: x => x.UserExamId,
                        principalTable: "UserExams",
                        principalColumn: "UserExamId");
                });

            migrationBuilder.CreateTable(
                name: "MediaProcessingHistories",
                columns: table => new
                {
                    HistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MediaId = table.Column<int>(type: "int", nullable: false),
                    Operation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ProcessedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaProcessingHistories", x => x.HistoryId);
                    table.ForeignKey(
                        name: "FK_MediaProcessingHistories_MediaFiles_MediaId",
                        column: x => x.MediaId,
                        principalTable: "MediaFiles",
                        principalColumn: "MediaId");
                    table.ForeignKey(
                        name: "FK_MediaProcessingHistories_Users_ProcessedBy",
                        column: x => x.ProcessedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Biography = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WorkPhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NativeLanguage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    KnownLanguagesJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TimeZone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Company = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    AvatarMediaId = table.Column<int>(type: "int", nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CoverImageMediaId = table.Column<int>(type: "int", nullable: true),
                    CoverImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PersonalWebsite = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SocialLinksJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublicProfile = table.Column<bool>(type: "bit", nullable: false),
                    ShowLearningStats = table.Column<bool>(type: "bit", nullable: false),
                    ShowBadges = table.Column<bool>(type: "bit", nullable: false),
                    ShowAchievements = table.Column<bool>(type: "bit", nullable: false),
                    ShowRecentActivity = table.Column<bool>(type: "bit", nullable: false),
                    ShowContactInfo = table.Column<bool>(type: "bit", nullable: false),
                    AllowMessages = table.Column<bool>(type: "bit", nullable: false),
                    AllowFriendRequests = table.Column<bool>(type: "bit", nullable: false),
                    PrivacySettingsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreferredThemeColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ThemeMode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    InterfaceLanguage = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CurrentLevel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LearningGoals = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    InterestsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SkillsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LearningMotivation = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PreviousExperience = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    LearningStrengths = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LearningChallenges = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerifiedBy = table.Column<int>(type: "int", nullable: true),
                    ReputationScore = table.Column<int>(type: "int", nullable: false),
                    LastOnlineAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    ActivityStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StatusMessage = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProfileViews = table.Column<int>(type: "int", nullable: false),
                    FollowerCount = table.Column<int>(type: "int", nullable: false),
                    FollowingCount = table.Column<int>(type: "int", nullable: false),
                    StatisticsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomSettingsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId");
                    table.ForeignKey(
                        name: "FK_UserProfiles_MediaFiles_AvatarMediaId",
                        column: x => x.AvatarMediaId,
                        principalTable: "MediaFiles",
                        principalColumn: "MediaId");
                    table.ForeignKey(
                        name: "FK_UserProfiles_MediaFiles_CoverImageMediaId",
                        column: x => x.CoverImageMediaId,
                        principalTable: "MediaFiles",
                        principalColumn: "MediaId");
                    table.ForeignKey(
                        name: "FK_UserProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_UserProfiles_Users_VerifiedBy",
                        column: x => x.VerifiedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "GoalProgresses",
                columns: table => new
                {
                    ProgressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoalId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    PercentageComplete = table.Column<float>(type: "real", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalProgresses", x => x.ProgressId);
                });

            migrationBuilder.CreateTable(
                name: "LearningSessions",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    SessionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ItemsStudied = table.Column<int>(type: "int", nullable: false),
                    CorrectAnswers = table.Column<int>(type: "int", nullable: false),
                    MistakesMade = table.Column<int>(type: "int", nullable: false),
                    EfficiencyScore = table.Column<double>(type: "float", nullable: false),
                    Platform = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DeviceInfo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    GoalId = table.Column<int>(type: "int", nullable: true),
                    GroupId = table.Column<int>(type: "int", nullable: true),
                    MoodBefore = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MoodAfter = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningSessions", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_LearningSessions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK_LearningSessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_LearningSessions_VocabularyGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "VocabularyGroups",
                        principalColumn: "GroupId");
                });

            migrationBuilder.CreateTable(
                name: "ScheduleItemParticipants",
                columns: table => new
                {
                    ParticipantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    GroupId = table.Column<int>(type: "int", nullable: true),
                    ParticipantRole = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AttendanceStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ResponseStatus = table.Column<int>(type: "int", nullable: true),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LeftAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AttendanceMinutes = table.Column<int>(type: "int", nullable: true),
                    IsLate = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttendanceCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParticipantNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResponseReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleItemParticipants", x => x.ParticipantId);
                    table.ForeignKey(
                        name: "FK_ScheduleItemParticipants_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId");
                    table.ForeignKey(
                        name: "FK_ScheduleItemParticipants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "ScheduleItems",
                columns: table => new
                {
                    ScheduleItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    RecurrenceId = table.Column<int>(type: "int", nullable: true),
                    StudyTaskId = table.Column<int>(type: "int", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LocationUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LocationDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    AttachmentUrls = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MeetingUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JoinCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAllDay = table.Column<bool>(type: "bit", nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CompletionPercentage = table.Column<float>(type: "real", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletionNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LessonId = table.Column<int>(type: "int", nullable: true),
                    MaterialId = table.Column<int>(type: "int", nullable: true),
                    RelatedEntityType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelatedEntityId = table.Column<int>(type: "int", nullable: true),
                    PriorityLevel = table.Column<int>(type: "int", nullable: true),
                    ColorCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SourceId = table.Column<int>(type: "int", nullable: true),
                    SourceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReminderMinutes = table.Column<int>(type: "int", nullable: true),
                    RequiresConfirmation = table.Column<bool>(type: "bit", nullable: false),
                    AutoComplete = table.Column<bool>(type: "bit", nullable: false),
                    EstimatedDurationMinutes = table.Column<int>(type: "int", nullable: true),
                    ActualDurationMinutes = table.Column<int>(type: "int", nullable: true),
                    PrivateNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    IsEditable = table.Column<bool>(type: "bit", nullable: false),
                    RescheduleCount = table.Column<int>(type: "int", nullable: false),
                    ChangeHistory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudyTaskId1 = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleItems", x => x.ScheduleItemId);
                    table.ForeignKey(
                        name: "FK_ScheduleItems_ScheduleItemTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ScheduleItemTypes",
                        principalColumn: "TypeId");
                    table.ForeignKey(
                        name: "FK_ScheduleItems_ScheduleRecurrences_RecurrenceId",
                        column: x => x.RecurrenceId,
                        principalTable: "ScheduleRecurrences",
                        principalColumn: "RecurrenceId");
                    table.ForeignKey(
                        name: "FK_ScheduleItems_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "ScheduleId");
                });

            migrationBuilder.CreateTable(
                name: "ScheduleReminders",
                columns: table => new
                {
                    ReminderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    ReminderTime = table.Column<int>(type: "int", nullable: true),
                    ReminderUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsEmailReminder = table.Column<bool>(type: "bit", nullable: false),
                    IsPopupReminder = table.Column<bool>(type: "bit", nullable: false),
                    IsPushReminder = table.Column<bool>(type: "bit", nullable: false),
                    IsSmsReminder = table.Column<bool>(type: "bit", nullable: false),
                    ReminderMessage = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CustomSound = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRepeating = table.Column<bool>(type: "bit", nullable: false),
                    RepeatIntervalMinutes = table.Column<int>(type: "int", nullable: true),
                    IsSent = table.Column<bool>(type: "bit", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsAcknowledged = table.Column<bool>(type: "bit", nullable: false),
                    AcknowledgedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AcknowledgementAction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleReminders", x => x.ReminderId);
                    table.ForeignKey(
                        name: "FK_ScheduleReminders_ScheduleItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "ScheduleItems",
                        principalColumn: "ScheduleItemId");
                    table.ForeignKey(
                        name: "FK_ScheduleReminders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "SessionDetails",
                columns: table => new
                {
                    DetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    ItemType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    WasCorrect = table.Column<bool>(type: "bit", nullable: false),
                    ResponseTimeMs = table.Column<int>(type: "int", nullable: false),
                    AttemptCount = table.Column<int>(type: "int", nullable: false),
                    UserAnswer = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Difficulty = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MemoryStrengthBefore = table.Column<int>(type: "int", nullable: false),
                    MemoryStrengthAfter = table.Column<int>(type: "int", nullable: false),
                    EaseFactorBefore = table.Column<double>(type: "float", nullable: false),
                    EaseFactorAfter = table.Column<double>(type: "float", nullable: false),
                    StudySessionSessionId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionDetails", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_SessionDetails_LearningSessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "LearningSessions",
                        principalColumn: "SessionId");
                });

            migrationBuilder.CreateTable(
                name: "StudyGoals",
                columns: table => new
                {
                    GoalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    GoalName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LevelId = table.Column<int>(type: "int", nullable: true),
                    TopicId = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    TargetDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Importance = table.Column<int>(type: "int", nullable: true),
                    Difficulty = table.Column<int>(type: "int", nullable: true),
                    GoalType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GoalScope = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TargetCount = table.Column<int>(type: "int", nullable: true),
                    MeasurementUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SuccessCriteria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VerificationMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProgressPercentage = table.Column<float>(type: "real", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EstimatedHours = table.Column<int>(type: "int", nullable: true),
                    ActualHours = table.Column<int>(type: "int", nullable: true),
                    RequiredResources = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentGoalId = table.Column<int>(type: "int", nullable: true),
                    DependsOn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    AutoUpdateProgress = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyGoals", x => x.GoalId);
                    table.ForeignKey(
                        name: "FK_StudyGoals_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK_StudyGoals_JLPTLevels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "JLPTLevels",
                        principalColumn: "LevelId");
                    table.ForeignKey(
                        name: "FK_StudyGoals_StudyGoals_ParentGoalId",
                        column: x => x.ParentGoalId,
                        principalTable: "StudyGoals",
                        principalColumn: "GoalId");
                    table.ForeignKey(
                        name: "FK_StudyGoals_StudyPlans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "StudyPlans",
                        principalColumn: "StudyPlanId");
                });

            migrationBuilder.CreateTable(
                name: "StudyPlanItems",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ItemType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    EstimatedTimeMinutes = table.Column<int>(type: "int", nullable: true),
                    ActualTimeMinutes = table.Column<int>(type: "int", nullable: true),
                    TopicId = table.Column<int>(type: "int", nullable: true),
                    VocabularyGroupId = table.Column<int>(type: "int", nullable: true),
                    GrammarId = table.Column<int>(type: "int", nullable: true),
                    ExamId = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    RecurrencePattern = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RecurrenceCount = table.Column<int>(type: "int", nullable: true),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CompletionPercentage = table.Column<float>(type: "real", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsHighlighted = table.Column<bool>(type: "bit", nullable: false),
                    IsFavorite = table.Column<bool>(type: "bit", nullable: false),
                    DependenciesJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompletionRequirements = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartConditions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LearningObjectives = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssessmentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResourcesJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TagsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpectedDifficulty = table.Column<int>(type: "int", nullable: true),
                    ActualDifficulty = table.Column<int>(type: "int", nullable: true),
                    AchievedScore = table.Column<float>(type: "real", nullable: true),
                    TargetScore = table.Column<float>(type: "real", nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    AllowRetry = table.Column<bool>(type: "bit", nullable: false),
                    MaxRetries = table.Column<int>(type: "int", nullable: true),
                    AutoAdvance = table.Column<bool>(type: "bit", nullable: false),
                    NeedsReview = table.Column<bool>(type: "bit", nullable: false),
                    NextReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewIntervalDays = table.Column<int>(type: "int", nullable: true),
                    ReminderSettings = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSynced = table.Column<bool>(type: "bit", nullable: false),
                    LastSyncedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SourceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MetadataJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPlanItems", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_StudyPlanItems_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK_StudyPlanItems_Grammars_GrammarId",
                        column: x => x.GrammarId,
                        principalTable: "Grammars",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudyPlanItems_JLPTExams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "JLPTExams",
                        principalColumn: "ExamId");
                    table.ForeignKey(
                        name: "FK_StudyPlanItems_StudyPlans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "StudyPlans",
                        principalColumn: "StudyPlanId");
                    table.ForeignKey(
                        name: "FK_StudyPlanItems_VocabularyGroups_VocabularyGroupId",
                        column: x => x.VocabularyGroupId,
                        principalTable: "VocabularyGroups",
                        principalColumn: "GroupId");
                });

            migrationBuilder.CreateTable(
                name: "StudyPlanProgresses",
                columns: table => new
                {
                    ProgressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudyPlanId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    CompletionPercentage = table.Column<int>(type: "int", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActualTimeMinutes = table.Column<int>(type: "int", nullable: true),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PerceivedDifficulty = table.Column<int>(type: "int", nullable: true),
                    SatisfactionRating = table.Column<int>(type: "int", nullable: true),
                    EffectivenessRating = table.Column<int>(type: "int", nullable: true),
                    UserNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EncounteredChallenges = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImprovementSuggestions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubItemsCompleted = table.Column<int>(type: "int", nullable: true),
                    TotalSubItems = table.Column<int>(type: "int", nullable: true),
                    CorrectAnswers = table.Column<int>(type: "int", nullable: true),
                    TotalQuestions = table.Column<int>(type: "int", nullable: true),
                    AchievedScore = table.Column<float>(type: "real", nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    IsFirstAttempt = table.Column<bool>(type: "bit", nullable: false),
                    TimeVarianceMinutes = table.Column<int>(type: "int", nullable: true),
                    DifficultyVariance = table.Column<int>(type: "int", nullable: true),
                    ProgressDataJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LearningMetricsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProgressTags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsImportant = table.Column<bool>(type: "bit", nullable: false),
                    NeedsReview = table.Column<bool>(type: "bit", nullable: false),
                    IsSynced = table.Column<bool>(type: "bit", nullable: false),
                    LastSyncedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPlanProgresses", x => x.ProgressId);
                    table.ForeignKey(
                        name: "FK_StudyPlanProgresses_StudyPlanItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "StudyPlanItems",
                        principalColumn: "ItemId");
                    table.ForeignKey(
                        name: "FK_StudyPlanProgresses_StudyPlans_StudyPlanId",
                        column: x => x.StudyPlanId,
                        principalTable: "StudyPlans",
                        principalColumn: "StudyPlanId");
                    table.ForeignKey(
                        name: "FK_StudyPlanProgresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "StudyTasks",
                columns: table => new
                {
                    StudyTaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoalId = table.Column<int>(type: "int", nullable: false),
                    TaskName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedDuration = table.Column<int>(type: "int", nullable: true),
                    DurationUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: true),
                    TaskType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TaskCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HasTimeConstraint = table.Column<bool>(type: "bit", nullable: false),
                    RequiredResources = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentUrls = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CompletionPercentage = table.Column<float>(type: "real", nullable: false),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    RecurrencePattern = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EnableReminders = table.Column<bool>(type: "bit", nullable: false),
                    ReminderSettings = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Dependencies = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompletionConditions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyTasks", x => x.StudyTaskId);
                    table.ForeignKey(
                        name: "FK_StudyTasks_StudyGoals_GoalId",
                        column: x => x.GoalId,
                        principalTable: "StudyGoals",
                        principalColumn: "GoalId");
                    table.ForeignKey(
                        name: "FK_StudyTasks_StudyPlanItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "StudyPlanItems",
                        principalColumn: "ItemId");
                });

            migrationBuilder.CreateTable(
                name: "StudySessions",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    SessionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    StudyPlanId = table.Column<int>(type: "int", nullable: true),
                    ItemsStudied = table.Column<int>(type: "int", nullable: false),
                    ItemsCompleted = table.Column<int>(type: "int", nullable: false),
                    CorrectAnswers = table.Column<int>(type: "int", nullable: false),
                    WrongAnswers = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<float>(type: "real", nullable: false),
                    DeviceInfo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AppVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Platform = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NetworkType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StudyMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StudyContext = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsSynced = table.Column<bool>(type: "bit", nullable: false),
                    SyncedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AverageResponseTimeMs = table.Column<int>(type: "int", nullable: true),
                    PerformanceMetrics = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EvaluationMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StudyTopicTopicId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudySessions", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_StudySessions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK_StudySessions_StudyPlans_StudyPlanId",
                        column: x => x.StudyPlanId,
                        principalTable: "StudyPlans",
                        principalColumn: "StudyPlanId");
                    table.ForeignKey(
                        name: "FK_StudySessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "StudyTopics",
                columns: table => new
                {
                    TopicId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TopicName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ParentTopicId = table.Column<int>(type: "int", nullable: true),
                    SessionId = table.Column<int>(type: "int", nullable: true),
                    IconPath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    JLPTLevel = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DifficultyLevel = table.Column<int>(type: "int", nullable: true),
                    EstimatedStudyHours = table.Column<int>(type: "int", nullable: true),
                    KeyVocabularyJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KeyGrammarJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelatedTopicsIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LearningResourcesJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecommendedMaterialsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LearningObjectives = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prerequisites = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpectedOutcomes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssessmentMethods = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActiveUserCount = table.Column<int>(type: "int", nullable: false),
                    TotalUserCount = table.Column<int>(type: "int", nullable: false),
                    AverageCompletionDays = table.Column<double>(type: "float", nullable: true),
                    SuccessRate = table.Column<double>(type: "float", nullable: true),
                    AverageRating = table.Column<double>(type: "float", nullable: true),
                    RatingCount = table.Column<int>(type: "int", nullable: false),
                    SearchKeywords = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPopular = table.Column<bool>(type: "bit", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    IsRecommendedForBeginners = table.Column<bool>(type: "bit", nullable: false),
                    LastContentUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContentVersion = table.Column<int>(type: "int", nullable: false),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyTopics", x => x.TopicId);
                    table.ForeignKey(
                        name: "FK_StudyTopics_StudySessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "StudySessions",
                        principalColumn: "SessionId");
                    table.ForeignKey(
                        name: "FK_StudyTopics_StudyTopics_ParentTopicId",
                        column: x => x.ParentTopicId,
                        principalTable: "StudyTopics",
                        principalColumn: "TopicId");
                });

            migrationBuilder.CreateTable(
                name: "TaskCompletions",
                columns: table => new
                {
                    CompletionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SessionId = table.Column<int>(type: "int", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletionPercentage = table.Column<int>(type: "int", nullable: false),
                    ActualDurationMinutes = table.Column<int>(type: "int", nullable: true),
                    PerceivedDifficulty = table.Column<int>(type: "int", nullable: true),
                    SatisfactionRating = table.Column<int>(type: "int", nullable: true),
                    EffectivenessRating = table.Column<int>(type: "int", nullable: true),
                    UserNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DetailedFeedback = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LearningOutcome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AchievedScore = table.Column<int>(type: "int", nullable: true),
                    CorrectAnswers = table.Column<int>(type: "int", nullable: true),
                    TotalQuestions = table.Column<int>(type: "int", nullable: true),
                    CompletionDataJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LearningMetricsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelatedContentId = table.Column<int>(type: "int", nullable: true),
                    RelatedContentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    IsFirstAttempt = table.Column<bool>(type: "bit", nullable: false),
                    DifficultyVariance = table.Column<int>(type: "int", nullable: true),
                    TimeVariance = table.Column<int>(type: "int", nullable: true),
                    CompletionStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CompletionTags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskCompletions", x => x.CompletionId);
                    table.ForeignKey(
                        name: "FK_TaskCompletions_StudySessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "StudySessions",
                        principalColumn: "SessionId");
                    table.ForeignKey(
                        name: "FK_TaskCompletions_StudyTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "StudyTasks",
                        principalColumn: "StudyTaskId");
                    table.ForeignKey(
                        name: "FK_TaskCompletions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryName",
                table: "Categories",
                column: "CategoryName");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_Name",
                table: "Categories",
                column: "CategoryName");

            migrationBuilder.CreateIndex(
                name: "IX_Definitions_CreatedBy",
                table: "Definitions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Definitions_VocabularyId",
                table: "Definitions",
                column: "VocabularyId");

            migrationBuilder.CreateIndex(
                name: "IX_DeletedItems_EntityType_EntityId",
                table: "DeletedItems",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_DeletedItems_UserID",
                table: "DeletedItems",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Department_Code",
                table: "Departments",
                column: "DepartmentCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Department_Name",
                table: "Departments",
                column: "DepartmentName");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_DepartmentCode",
                table: "Departments",
                column: "DepartmentCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_DepartmentName",
                table: "Departments",
                column: "DepartmentName");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ManagerUserId",
                table: "Departments",
                column: "ManagerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ParentDepartmentId",
                table: "Departments",
                column: "ParentDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Example_VocabularyId",
                table: "Examples",
                column: "VocabularyId");

            migrationBuilder.CreateIndex(
                name: "IX_Examples_CreatedBy",
                table: "Examples",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Examples_VocabularyId",
                table: "Examples",
                column: "VocabularyId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalProgresses_GoalId",
                table: "GoalProgresses",
                column: "GoalId");

            migrationBuilder.CreateIndex(
                name: "IX_GrammarDefinitions_CreatedBy",
                table: "GrammarDefinitions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GrammarDefinitions_GrammarId",
                table: "GrammarDefinitions",
                column: "GrammarId");

            migrationBuilder.CreateIndex(
                name: "IX_GrammarDefinitions_UpdatedBy",
                table: "GrammarDefinitions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GrammarExample_Grammar",
                table: "GrammarExamples",
                column: "GrammarId");

            migrationBuilder.CreateIndex(
                name: "IX_GrammarExamples_CreatedBy",
                table: "GrammarExamples",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GrammarExamples_UpdatedBy",
                table: "GrammarExamples",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Grammar_Level",
                table: "Grammars",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_Grammar_Pattern",
                table: "Grammars",
                column: "Pattern",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Grammars_CategoryId",
                table: "Grammars",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Grammars_Level",
                table: "Grammars",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_Grammars_ParentGrammarId",
                table: "Grammars",
                column: "ParentGrammarId");

            migrationBuilder.CreateIndex(
                name: "IX_Grammars_Pattern",
                table: "Grammars",
                column: "Pattern");

            migrationBuilder.CreateIndex(
                name: "IX_GrammarTranslation_Grammar_Lang",
                table: "GrammarTranslations",
                columns: new[] { "GrammarId", "LanguageCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GrammarTranslations_CreatedBy",
                table: "GrammarTranslations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GrammarTranslations_UpdatedBy",
                table: "GrammarTranslations",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GrammarTranslations_VerifiedBy",
                table: "GrammarTranslations",
                column: "VerifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPermission_Group_Permission",
                table: "GroupPermissions",
                columns: new[] { "GroupId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupPermissions_GrantedByUserId",
                table: "GroupPermissions",
                column: "GrantedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPermissions_PermissionId",
                table: "GroupPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_Name",
                table: "Groups",
                column: "GroupName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ParentGroupId",
                table: "Groups",
                column: "ParentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_Level_Date",
                table: "JLPTExams",
                columns: new[] { "Level", "Year", "Month" },
                unique: true,
                filter: "[Year] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_Name",
                table: "JLPTExams",
                column: "ExamName");

            migrationBuilder.CreateIndex(
                name: "IX_JLPTExam_CreatedBy",
                table: "JLPTExams",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JLPTExams_LevelId",
                table: "JLPTExams",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_JLPT_Level",
                table: "JLPTLevels",
                column: "LevelName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JLPTLevels_PrerequisiteLevelId",
                table: "JLPTLevels",
                column: "PrerequisiteLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Section_Exam_Name",
                table: "JLPTSections",
                columns: new[] { "ExamId", "SectionName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KanjiComponentMapping_Kanji_Component",
                table: "KanjiComponentMappings",
                columns: new[] { "KanjiId", "ComponentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KanjiComponentMappings_ComponentId",
                table: "KanjiComponentMappings",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_KanjiComponent_Character",
                table: "KanjiComponents",
                column: "Character",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KanjiComponents_CreatedBy",
                table: "KanjiComponents",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_KanjiExampleMeaning_Example_Lang",
                table: "KanjiExampleMeanings",
                columns: new[] { "ExampleId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KanjiExampleMeanings_CreatedBy",
                table: "KanjiExampleMeanings",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_KanjiExample_Kanji",
                table: "KanjiExamples",
                column: "KanjiId");

            migrationBuilder.CreateIndex(
                name: "IX_KanjiExamples_VerifiedBy",
                table: "KanjiExamples",
                column: "VerifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_KanjiMeaning_Kanji_Lang",
                table: "KanjiMeanings",
                columns: new[] { "KanjiId", "Language" });

            migrationBuilder.CreateIndex(
                name: "IX_KanjiMeanings_CreatedBy",
                table: "KanjiMeanings",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_KanjiMeanings_UpdatedBy",
                table: "KanjiMeanings",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Kanji_Character",
                table: "Kanjis",
                column: "Character",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kanji_Grade",
                table: "Kanjis",
                column: "Grade");

            migrationBuilder.CreateIndex(
                name: "IX_Kanji_JLPT",
                table: "Kanjis",
                column: "JLPTLevel");

            migrationBuilder.CreateIndex(
                name: "IX_Kanji_StrokeCount",
                table: "Kanjis",
                column: "StrokeCount");

            migrationBuilder.CreateIndex(
                name: "IX_Kanjis_CategoryId",
                table: "Kanjis",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Kanjis_Character",
                table: "Kanjis",
                column: "Character",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kanjis_JLPTLevel",
                table: "Kanjis",
                column: "JLPTLevel");

            migrationBuilder.CreateIndex(
                name: "IX_Kanjis_StrokeCount",
                table: "Kanjis",
                column: "StrokeCount");

            migrationBuilder.CreateIndex(
                name: "IX_Kanjis_VerifiedBy",
                table: "Kanjis",
                column: "VerifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_KanjiVocabularies_VocabularyId",
                table: "KanjiVocabularies",
                column: "VocabularyId");

            migrationBuilder.CreateIndex(
                name: "IX_KanjiVocabulary_Kanji_Vocab",
                table: "KanjiVocabularies",
                columns: new[] { "KanjiId", "VocabularyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LearningProgress_LastStudied",
                table: "LearningProgresses",
                column: "LastStudied");

            migrationBuilder.CreateIndex(
                name: "IX_LearningProgress_MasteryLevel",
                table: "LearningProgresses",
                column: "MasteryLevel");

            migrationBuilder.CreateIndex(
                name: "IX_LearningProgress_NextReviewDate",
                table: "LearningProgresses",
                column: "NextReviewDate");

            migrationBuilder.CreateIndex(
                name: "IX_LearningProgresses_UserId_NextReviewDate",
                table: "LearningProgresses",
                columns: new[] { "UserId", "NextReviewDate" });

            migrationBuilder.CreateIndex(
                name: "IX_LearningProgresses_UserId_VocabularyId",
                table: "LearningProgresses",
                columns: new[] { "UserId", "VocabularyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LearningProgresses_VocabularyId",
                table: "LearningProgresses",
                column: "VocabularyId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningProgresses_VocabularyId1",
                table: "LearningProgresses",
                column: "VocabularyId1");

            migrationBuilder.CreateIndex(
                name: "IX_User_NextReview",
                table: "LearningProgresses",
                columns: new[] { "UserId", "NextReviewDate" });

            migrationBuilder.CreateIndex(
                name: "IX_User_Vocabulary",
                table: "LearningProgresses",
                columns: new[] { "UserId", "VocabularyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LearningSessions_CategoryId",
                table: "LearningSessions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningSessions_GoalId",
                table: "LearningSessions",
                column: "GoalId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningSessions_GroupId",
                table: "LearningSessions",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningSessions_UserId_StartTime",
                table: "LearningSessions",
                columns: new[] { "UserId", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_User_Session",
                table: "LearningSessions",
                columns: new[] { "UserId", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_MediaCategories_CategoryName",
                table: "MediaCategories",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaCategories_ParentCategoryId",
                table: "MediaCategories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaCategory_Name",
                table: "MediaCategories",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_CategoryId",
                table: "MediaFiles",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_DeletedBy",
                table: "MediaFiles",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_ExampleId",
                table: "MediaFiles",
                column: "ExampleId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_GrammarExampleId",
                table: "MediaFiles",
                column: "GrammarExampleId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_GrammarId",
                table: "MediaFiles",
                column: "GrammarId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_KanjiExampleId",
                table: "MediaFiles",
                column: "KanjiExampleId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_KanjiId",
                table: "MediaFiles",
                column: "KanjiId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_MediaCategoryCategoryId",
                table: "MediaFiles",
                column: "MediaCategoryCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_MediaType_IsPrimary",
                table: "MediaFiles",
                columns: new[] { "MediaType", "IsPrimary" });

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_Primary",
                table: "MediaFiles",
                columns: new[] { "MediaType", "IsPrimary" });

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_QuestionId",
                table: "MediaFiles",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_QuestionOptionId",
                table: "MediaFiles",
                column: "QuestionOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_TechnicalTermId",
                table: "MediaFiles",
                column: "TechnicalTermId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_TermExampleId",
                table: "MediaFiles",
                column: "TermExampleId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_User",
                table: "MediaFiles",
                columns: new[] { "UserId", "MediaType", "IsPrimary" });

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_UserId",
                table: "MediaFiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_Vocabulary",
                table: "MediaFiles",
                columns: new[] { "VocabularyId", "MediaType", "IsPrimary" });

            migrationBuilder.CreateIndex(
                name: "IX_MediaProcessingHistories_MediaId",
                table: "MediaProcessingHistories",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaProcessingHistories_ProcessedBy",
                table: "MediaProcessingHistories",
                column: "ProcessedBy");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationPriority_Name",
                table: "NotificationPriorities",
                column: "PriorityName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationRecipient_Notification_User",
                table: "NotificationRecipients",
                columns: new[] { "NotificationId", "UserId" },
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationRecipients_GroupId",
                table: "NotificationRecipients",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationRecipients_NotificationId_UserId",
                table: "NotificationRecipients",
                columns: new[] { "NotificationId", "UserId" },
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationRecipients_StatusId",
                table: "NotificationRecipients",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationRecipients_UserId",
                table: "NotificationRecipients",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationResponses_RecipientId",
                table: "NotificationResponses",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationResponses_RespondedByUserId",
                table: "NotificationResponses",
                column: "RespondedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatedAt",
                table: "Notifications",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_DeletedBy",
                table: "Notifications",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_PriorityId",
                table: "Notifications",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SenderUserId",
                table: "Notifications",
                column: "SenderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_TypeId",
                table: "Notifications",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationStatus_Name",
                table: "NotificationStatuses",
                column: "StatusName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationType_Name",
                table: "NotificationTypes",
                column: "TypeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroupMapping_Group_Permission",
                table: "PermissionGroupMappings",
                columns: new[] { "PermissionGroupId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroupMappings_CreatedByUserId",
                table: "PermissionGroupMappings",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroupMappings_DependsOnPermissionId",
                table: "PermissionGroupMappings",
                column: "DependsOnPermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroupMappings_GroupId",
                table: "PermissionGroupMappings",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroupMappings_PermissionGroupId_PermissionId",
                table: "PermissionGroupMappings",
                columns: new[] { "PermissionGroupId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroupMappings_PermissionId",
                table: "PermissionGroupMappings",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroup_Name",
                table: "PermissionGroups",
                column: "GroupName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroups_GroupName",
                table: "PermissionGroups",
                column: "GroupName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroups_ParentGroupId",
                table: "PermissionGroups",
                column: "ParentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Module",
                table: "Permissions",
                column: "Module");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Name",
                table: "Permissions",
                column: "PermissionName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_PermissionName",
                table: "Permissions",
                column: "PermissionName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalWordListItem_List_Vocab",
                table: "PersonalWordListItems",
                columns: new[] { "ListId", "VocabularyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalWordListItems_VocabularyId",
                table: "PersonalWordListItems",
                column: "VocabularyId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalWordList_User_Name",
                table: "PersonalWordLists",
                columns: new[] { "UserId", "ListName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalWordLists_UserId_ListName",
                table: "PersonalWordLists",
                columns: new[] { "UserId", "ListName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_QuestionId",
                table: "QuestionOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_Active_Type",
                table: "Questions",
                columns: new[] { "IsActive", "QuestionType" });

            migrationBuilder.CreateIndex(
                name: "IX_Question_CreatedBy",
                table: "Questions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Question_Difficulty",
                table: "Questions",
                column: "Difficulty");

            migrationBuilder.CreateIndex(
                name: "IX_Question_Section",
                table: "Questions",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_SoftDelete",
                table: "Questions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Question_Tags",
                table: "Questions",
                column: "Tags");

            migrationBuilder.CreateIndex(
                name: "IX_Question_Type",
                table: "Questions",
                column: "QuestionType");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionType",
                table: "Questions",
                column: "QuestionType");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RolePermission",
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_GrantedBy",
                table: "RolePermissions",
                column: "GrantedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_ParentRoleId",
                table: "Roles",
                column: "ParentRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleItemParticipant_Item_User",
                table: "ScheduleItemParticipants",
                columns: new[] { "ItemId", "UserId" },
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleItemParticipants_GroupId",
                table: "ScheduleItemParticipants",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleItemParticipants_UserId",
                table: "ScheduleItemParticipants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleItem_Schedule_StartTime",
                table: "ScheduleItems",
                columns: new[] { "ScheduleId", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleItem_StartTime",
                table: "ScheduleItems",
                column: "StartTime");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleItem_StudyTask",
                table: "ScheduleItems",
                column: "StudyTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleItems_RecurrenceId",
                table: "ScheduleItems",
                column: "RecurrenceId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleItems_ScheduleId_StartTime",
                table: "ScheduleItems",
                columns: new[] { "ScheduleId", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleItems_StartTime",
                table: "ScheduleItems",
                column: "StartTime");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleItems_StudyTaskId1",
                table: "ScheduleItems",
                column: "StudyTaskId1");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleItems_TypeId",
                table: "ScheduleItems",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleItemType_Name",
                table: "ScheduleItemTypes",
                column: "TypeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleReminders_ItemId",
                table: "ScheduleReminders",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleReminders_UserId",
                table: "ScheduleReminders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Name",
                table: "Schedules",
                column: "ScheduleName");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_CreatedByUserId",
                table: "Schedules",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ScheduleName",
                table: "Schedules",
                column: "ScheduleName");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_StudyPlanId",
                table: "Schedules",
                column: "StudyPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionDetails_SessionId",
                table: "SessionDetails",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionDetails_StudySessionSessionId",
                table: "SessionDetails",
                column: "StudySessionSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Setting_Key",
                table: "Settings",
                column: "SettingKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Settings_SettingKey",
                table: "Settings",
                column: "SettingKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudyGoal_Plan_Name",
                table: "StudyGoals",
                columns: new[] { "PlanId", "GoalName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudyGoals_CategoryId",
                table: "StudyGoals",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyGoals_LevelId",
                table: "StudyGoals",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyGoals_ParentGoalId",
                table: "StudyGoals",
                column: "ParentGoalId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyGoals_PlanId_GoalName",
                table: "StudyGoals",
                columns: new[] { "PlanId", "GoalName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudyGoals_TopicId",
                table: "StudyGoals",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanItem_Plan_Type",
                table: "StudyPlanItems",
                columns: new[] { "PlanId", "ItemType" });

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanItem_Priority",
                table: "StudyPlanItems",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanItem_ScheduledDate",
                table: "StudyPlanItems",
                column: "ScheduledDate");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanItems_CategoryId",
                table: "StudyPlanItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanItems_ExamId",
                table: "StudyPlanItems",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanItems_GrammarId",
                table: "StudyPlanItems",
                column: "GrammarId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanItems_TopicId",
                table: "StudyPlanItems",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanItems_VocabularyGroupId",
                table: "StudyPlanItems",
                column: "VocabularyGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanProgress_CompletedDate",
                table: "StudyPlanProgresses",
                column: "CompletedDate");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanProgress_Item_User",
                table: "StudyPlanProgresses",
                columns: new[] { "ItemId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanProgress_Plan_User",
                table: "StudyPlanProgresses",
                columns: new[] { "StudyPlanId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanProgresses_UserId",
                table: "StudyPlanProgresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlan_User_Name",
                table: "StudyPlans",
                columns: new[] { "UserId", "PlanName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlans_TargetLevel",
                table: "StudyPlans",
                column: "TargetLevel");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlans_UserId_PlanName",
                table: "StudyPlans",
                columns: new[] { "UserId", "PlanName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudySession_User_StartTime",
                table: "StudySessions",
                columns: new[] { "UserId", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_StudySessions_CategoryId",
                table: "StudySessions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StudySessions_StudyPlanId",
                table: "StudySessions",
                column: "StudyPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_StudySessions_StudyTopicTopicId",
                table: "StudySessions",
                column: "StudyTopicTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyTask_Goal_Name",
                table: "StudyTasks",
                columns: new[] { "GoalId", "TaskName" });

            migrationBuilder.CreateIndex(
                name: "IX_StudyTasks_GoalId_TaskName",
                table: "StudyTasks",
                columns: new[] { "GoalId", "TaskName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudyTasks_ItemId",
                table: "StudyTasks",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyTopic_JLPTLevel",
                table: "StudyTopics",
                column: "JLPTLevel");

            migrationBuilder.CreateIndex(
                name: "IX_StudyTopic_Name_Category",
                table: "StudyTopics",
                columns: new[] { "TopicName", "Category" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudyTopic_Parent",
                table: "StudyTopics",
                column: "ParentTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyTopics_SessionId",
                table: "StudyTopics",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SyncConflicts_UserID",
                table: "SyncConflicts",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SyncMetadata_UserID",
                table: "SyncMetadata",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCompletion_Task_Date",
                table: "TaskCompletions",
                columns: new[] { "TaskId", "CompletionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskCompletion_User_Date",
                table: "TaskCompletions",
                columns: new[] { "UserId", "CompletionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskCompletions_SessionId",
                table: "TaskCompletions",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Name",
                table: "Teams",
                column: "TeamName");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_DepartmentId",
                table: "Teams",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_LeaderUserId",
                table: "Teams",
                column: "LeaderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalTerm_Field",
                table: "TechnicalTerms",
                column: "Field");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalTerm_Term_Lang_Field",
                table: "TechnicalTerms",
                columns: new[] { "Term", "LanguageCode", "Field" });

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalTerms_CategoryId",
                table: "TechnicalTerms",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalTerms_Field",
                table: "TechnicalTerms",
                column: "Field");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalTerms_Term_LanguageCode_Field",
                table: "TechnicalTerms",
                columns: new[] { "Term", "LanguageCode", "Field" });

            migrationBuilder.CreateIndex(
                name: "IX_TermExample_Term",
                table: "TermExamples",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_TermExamples_CreatedBy",
                table: "TermExamples",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TermExamples_UpdatedBy",
                table: "TermExamples",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TermRelation_Term1_Term2",
                table: "TermRelations",
                columns: new[] { "TermId1", "TermId2" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TermRelations_CreatedBy",
                table: "TermRelations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TermRelations_TermId2",
                table: "TermRelations",
                column: "TermId2");

            migrationBuilder.CreateIndex(
                name: "IX_TermRelations_UpdatedBy",
                table: "TermRelations",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TermRelations_VerifiedBy",
                table: "TermRelations",
                column: "VerifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TermTranslation_Term_Lang",
                table: "TermTranslations",
                columns: new[] { "TermId", "LanguageCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TermTranslations_ApprovedBy",
                table: "TermTranslations",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TermTranslations_CreatedBy",
                table: "TermTranslations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TermTranslations_UpdatedBy",
                table: "TermTranslations",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TestDetails_QuestionId",
                table: "TestDetails",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestDetails_TestResultId",
                table: "TestDetails",
                column: "TestResultId");

            migrationBuilder.CreateIndex(
                name: "IX_TestDetails_VocabularyId",
                table: "TestDetails",
                column: "VocabularyId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResult_Category",
                table: "TestResults",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResult_Level",
                table: "TestResults",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_TestResult_Score",
                table: "TestResults",
                column: "Score");

            migrationBuilder.CreateIndex(
                name: "IX_TestResult_SoftDelete_Type",
                table: "TestResults",
                columns: new[] { "IsDeleted", "TestType" });

            migrationBuilder.CreateIndex(
                name: "IX_TestResult_Type",
                table: "TestResults",
                column: "TestType");

            migrationBuilder.CreateIndex(
                name: "IX_TestResult_User_Date",
                table: "TestResults",
                columns: new[] { "UserId", "TestDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TestResult_User_Type_Date",
                table: "TestResults",
                columns: new[] { "UserId", "TestType", "TestDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_Score",
                table: "TestResults",
                column: "Score");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_TestType",
                table: "TestResults",
                column: "TestType");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_UserId_TestDate",
                table: "TestResults",
                columns: new[] { "UserId", "TestDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Translation_VocabLang",
                table: "Translations",
                columns: new[] { "VocabularyId", "LanguageCode" });

            migrationBuilder.CreateIndex(
                name: "IX_Translations_CreatedBy",
                table: "Translations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswer_Exam_Question",
                table: "UserAnswers",
                columns: new[] { "UserExamId", "QuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_QuestionId",
                table: "UserAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_SelectedOptionId",
                table: "UserAnswers",
                column: "SelectedOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExam_EndTime",
                table: "UserExams",
                column: "EndTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserExam_SoftDelete_Status",
                table: "UserExams",
                columns: new[] { "IsDeleted", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_UserExam_Status",
                table: "UserExams",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_UserExam_User_StartTime",
                table: "UserExams",
                columns: new[] { "UserId", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_UserExam_User_Status",
                table: "UserExams",
                columns: new[] { "UserId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_UserExams_ExamId",
                table: "UserExams",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGrammarProgress_User_Grammar",
                table: "UserGrammarProgresses",
                columns: new[] { "UserId", "GrammarId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserGrammarProgresses_GrammarId",
                table: "UserGrammarProgresses",
                column: "GrammarId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroup_User_Group",
                table: "UserGroups",
                columns: new[] { "UserId", "GroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_GroupId",
                table: "UserGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_InvitedBy",
                table: "UserGroups",
                column: "InvitedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserKanjiProgress_User_Kanji",
                table: "UserKanjiProgresses",
                columns: new[] { "UserId", "KanjiId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserKanjiProgresses_KanjiId",
                table: "UserKanjiProgresses",
                column: "KanjiId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLearningPreference_User",
                table: "UserLearningPreferences",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserNotificationSetting_User",
                table: "UserNotificationSettings",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPermission_UserPermission",
                table: "UserPermissions",
                columns: new[] { "UserId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_GrantedByUserId",
                table: "UserPermissions",
                column: "GrantedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_PermissionId",
                table: "UserPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_AvatarMediaId",
                table: "UserProfiles",
                column: "AvatarMediaId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_CoverImageMediaId",
                table: "UserProfiles",
                column: "CoverImageMediaId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_DepartmentId",
                table: "UserProfiles",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_VerifiedBy",
                table: "UserProfiles",
                column: "VerifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserRole",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_AssignedBy",
                table: "UserRoles",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId1",
                table: "UserRoles",
                column: "RoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "Users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_DeletedBy",
                table: "Users",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTeam_UserTeam",
                table: "UserTeams",
                columns: new[] { "UserId", "TeamId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_AddedBy",
                table: "UserTeams",
                column: "AddedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_TeamId",
                table: "UserTeams",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTechnicalTerm_User_Term",
                table: "UserTechnicalTerms",
                columns: new[] { "UserId", "TermId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTechnicalTerms_TermId",
                table: "UserTechnicalTerms",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_Vocabularies_CategoryId",
                table: "Vocabularies",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Vocabularies_Level",
                table: "Vocabularies",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_Vocabularies_Term_LanguageCode",
                table: "Vocabularies",
                columns: new[] { "Term", "LanguageCode" });

            migrationBuilder.CreateIndex(
                name: "IX_Vocabularies_VerifiedBy",
                table: "Vocabularies",
                column: "VerifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Vocabularies_VocabularyGroupGroupId",
                table: "Vocabularies",
                column: "VocabularyGroupGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Vocabulary_Category",
                table: "Vocabularies",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Vocabulary_IsCommon",
                table: "Vocabularies",
                column: "IsCommon");

            migrationBuilder.CreateIndex(
                name: "IX_Vocabulary_Level",
                table: "Vocabularies",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_Vocabulary_Term_Lang",
                table: "Vocabularies",
                columns: new[] { "Term", "LanguageCode" });

            migrationBuilder.CreateIndex(
                name: "IX_VocabularyGroup_Name",
                table: "VocabularyGroups",
                column: "GroupName");

            migrationBuilder.CreateIndex(
                name: "IX_VocabularyGroups_CategoryId",
                table: "VocabularyGroups",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupVocabulary_Group_Vocab",
                table: "VocabularyRelations",
                columns: new[] { "GroupId", "VocabularyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VocabularyRelations_VocabularyId",
                table: "VocabularyRelations",
                column: "VocabularyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Definitions_Users_CreatedBy",
                table: "Definitions",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Definitions_Vocabularies_VocabularyId",
                table: "Definitions",
                column: "VocabularyId",
                principalTable: "Vocabularies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeletedItems_Users_UserID",
                table: "DeletedItems",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Users_ManagerUserId",
                table: "Departments",
                column: "ManagerUserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GoalProgresses_StudyGoals_GoalId",
                table: "GoalProgresses",
                column: "GoalId",
                principalTable: "StudyGoals",
                principalColumn: "GoalId");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningSessions_StudyGoals_GoalId",
                table: "LearningSessions",
                column: "GoalId",
                principalTable: "StudyGoals",
                principalColumn: "GoalId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleItemParticipants_ScheduleItems_ItemId",
                table: "ScheduleItemParticipants",
                column: "ItemId",
                principalTable: "ScheduleItems",
                principalColumn: "ScheduleItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleItems_StudyTasks_StudyTaskId",
                table: "ScheduleItems",
                column: "StudyTaskId",
                principalTable: "StudyTasks",
                principalColumn: "StudyTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleItems_StudyTasks_StudyTaskId1",
                table: "ScheduleItems",
                column: "StudyTaskId1",
                principalTable: "StudyTasks",
                principalColumn: "StudyTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionDetails_StudySessions_StudySessionSessionId",
                table: "SessionDetails",
                column: "StudySessionSessionId",
                principalTable: "StudySessions",
                principalColumn: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGoals_StudyTopics_TopicId",
                table: "StudyGoals",
                column: "TopicId",
                principalTable: "StudyTopics",
                principalColumn: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyPlanItems_StudyTopics_TopicId",
                table: "StudyPlanItems",
                column: "TopicId",
                principalTable: "StudyTopics",
                principalColumn: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudySessions_StudyTopics_StudyTopicTopicId",
                table: "StudySessions",
                column: "StudyTopicTopicId",
                principalTable: "StudyTopics",
                principalColumn: "TopicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Users_ManagerUserId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyPlans_Users_UserId",
                table: "StudyPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_StudySessions_Users_UserId",
                table: "StudySessions");

            migrationBuilder.DropForeignKey(
                name: "FK_StudySessions_Categories_CategoryId",
                table: "StudySessions");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyPlans_JLPTLevels_TargetLevel",
                table: "StudyPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_StudySessions_StudyPlans_StudyPlanId",
                table: "StudySessions");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyTopics_StudySessions_SessionId",
                table: "StudyTopics");

            migrationBuilder.DropTable(
                name: "Definitions");

            migrationBuilder.DropTable(
                name: "DeletedItems");

            migrationBuilder.DropTable(
                name: "GoalProgresses");

            migrationBuilder.DropTable(
                name: "GrammarDefinitions");

            migrationBuilder.DropTable(
                name: "GrammarTranslations");

            migrationBuilder.DropTable(
                name: "GroupPermissions");

            migrationBuilder.DropTable(
                name: "KanjiComponentMappings");

            migrationBuilder.DropTable(
                name: "KanjiExampleMeanings");

            migrationBuilder.DropTable(
                name: "KanjiMeanings");

            migrationBuilder.DropTable(
                name: "KanjiVocabularies");

            migrationBuilder.DropTable(
                name: "LearningProgresses");

            migrationBuilder.DropTable(
                name: "MediaProcessingHistories");

            migrationBuilder.DropTable(
                name: "NotificationResponses");

            migrationBuilder.DropTable(
                name: "PermissionGroupMappings");

            migrationBuilder.DropTable(
                name: "PersonalWordListItems");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "ScheduleItemParticipants");

            migrationBuilder.DropTable(
                name: "ScheduleReminders");

            migrationBuilder.DropTable(
                name: "SessionDetails");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "StudyPlanProgresses");

            migrationBuilder.DropTable(
                name: "SyncConflicts");

            migrationBuilder.DropTable(
                name: "SyncMetadata");

            migrationBuilder.DropTable(
                name: "TaskCompletions");

            migrationBuilder.DropTable(
                name: "TermRelations");

            migrationBuilder.DropTable(
                name: "TermTranslations");

            migrationBuilder.DropTable(
                name: "TestDetails");

            migrationBuilder.DropTable(
                name: "Translations");

            migrationBuilder.DropTable(
                name: "UserAnswers");

            migrationBuilder.DropTable(
                name: "UserGrammarProgresses");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropTable(
                name: "UserKanjiProgresses");

            migrationBuilder.DropTable(
                name: "UserLearningPreferences");

            migrationBuilder.DropTable(
                name: "UserNotificationSettings");

            migrationBuilder.DropTable(
                name: "UserPermissions");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTeams");

            migrationBuilder.DropTable(
                name: "UserTechnicalTerms");

            migrationBuilder.DropTable(
                name: "VocabularyRelations");

            migrationBuilder.DropTable(
                name: "KanjiComponents");

            migrationBuilder.DropTable(
                name: "NotificationRecipients");

            migrationBuilder.DropTable(
                name: "PermissionGroups");

            migrationBuilder.DropTable(
                name: "PersonalWordLists");

            migrationBuilder.DropTable(
                name: "ScheduleItems");

            migrationBuilder.DropTable(
                name: "LearningSessions");

            migrationBuilder.DropTable(
                name: "TestResults");

            migrationBuilder.DropTable(
                name: "UserExams");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "MediaFiles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "NotificationStatuses");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "ScheduleItemTypes");

            migrationBuilder.DropTable(
                name: "ScheduleRecurrences");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "StudyTasks");

            migrationBuilder.DropTable(
                name: "Examples");

            migrationBuilder.DropTable(
                name: "GrammarExamples");

            migrationBuilder.DropTable(
                name: "KanjiExamples");

            migrationBuilder.DropTable(
                name: "MediaCategories");

            migrationBuilder.DropTable(
                name: "QuestionOptions");

            migrationBuilder.DropTable(
                name: "TermExamples");

            migrationBuilder.DropTable(
                name: "NotificationPriorities");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropTable(
                name: "StudyGoals");

            migrationBuilder.DropTable(
                name: "StudyPlanItems");

            migrationBuilder.DropTable(
                name: "Vocabularies");

            migrationBuilder.DropTable(
                name: "Kanjis");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "TechnicalTerms");

            migrationBuilder.DropTable(
                name: "Grammars");

            migrationBuilder.DropTable(
                name: "VocabularyGroups");

            migrationBuilder.DropTable(
                name: "JLPTSections");

            migrationBuilder.DropTable(
                name: "JLPTExams");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "JLPTLevels");

            migrationBuilder.DropTable(
                name: "StudyPlans");

            migrationBuilder.DropTable(
                name: "StudySessions");

            migrationBuilder.DropTable(
                name: "StudyTopics");
        }
    }
}
