using AutoMapper;
using LexiFlow.API.DTOs.Auth;
using LexiFlow.API.DTOs.Learning;
using LexiFlow.API.DTOs.Vocabulary;
using LexiFlow.API.DTOs.Kanji;
using LexiFlow.API.DTOs.Grammar;
using LexiFlow.API.DTOs.User;
using LexiFlow.API.DTOs.Category;
using LexiFlow.API.DTOs.TechnicalTerm;
using LexiFlow.API.DTOs.VocabularyGroup;
using LexiFlow.Models;
using System;

namespace LexiFlow.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region User Management Mappings

            // User Mappings
            CreateMap<User, UserProfileDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department != null ? src.Department_Navigation.DepartmentName : null))
                .ForMember(dest => dest.RoleIds, opt => opt.Ignore());

            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => "Active"))
                .ForMember(dest => dest.UserID, opt => opt.Ignore())
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

            // User Profile Mappings
            CreateMap<User, UserProfileDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department != null ? src.Department_Navigation.DepartmentName : null))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive == true))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<UpdateUserProfileDto, User>()
                .ForMember(dest => dest.UserID, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

            // Department Mappings
            CreateMap<Department, DepartmentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DepartmentID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DepartmentName))
                .ForMember(dest => dest.DepartmentCode, opt => opt.MapFrom(src => src.DepartmentCode))
                .ForMember(dest => dest.ParentDepartmentID, opt => opt.MapFrom(src => src.ParentDepartmentID))
                .ForMember(dest => dest.ParentDepartmentName, opt => opt.MapFrom(src => src.ParentDepartment != null ? src.ParentDepartment.DepartmentName : null))
                .ForMember(dest => dest.ManagerID, opt => opt.MapFrom(src => src.ManagerUserID))
                .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager != null ? $"{src.Manager.FirstName} {src.Manager.LastName}" : null))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.RowVersionString, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));

            CreateMap<CreateDepartmentDto, Department>()
                .ForMember(dest => dest.DepartmentID, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

            CreateMap<UpdateDepartmentDto, Department>()
                .ForMember(dest => dest.DepartmentID, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

            // Team Mappings
            CreateMap<Team, TeamDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TeamID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TeamName))
                .ForMember(dest => dest.DepartmentID, opt => opt.MapFrom(src => src.DepartmentID))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.DepartmentName : null))
                .ForMember(dest => dest.LeaderID, opt => opt.MapFrom(src => src.LeaderUserID))
                .ForMember(dest => dest.LeaderName, opt => opt.MapFrom(src => src.Leader != null ? $"{src.Leader.FirstName} {src.Leader.LastName}" : null))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.RowVersionString, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));

            CreateMap<CreateTeamDto, Team>()
                .ForMember(dest => dest.TeamID, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

            CreateMap<UpdateTeamDto, Team>()
                .ForMember(dest => dest.TeamID, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

            // Role & Permission Mappings
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RoleName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsSystemRole, opt => opt.MapFrom(src => false)) // Không có trong model nên mặc định false
                .ForMember(dest => dest.Permissions, opt => opt.Ignore()) // Sẽ được điền ở service
                .ForMember(dest => dest.UsersCount, opt => opt.MapFrom(src => src.UserRoles != null ? src.UserRoles.Count : 0))
                .ForMember(dest => dest.RowVersionString, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));

            CreateMap<CreateRoleDto, Role>()
                .ForMember(dest => dest.RoleID, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
                .ForMember(dest => dest.RolePermissions, opt => opt.Ignore());

            CreateMap<UpdateRoleDto, Role>()
                .ForMember(dest => dest.RoleID, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
                .ForMember(dest => dest.RolePermissions, opt => opt.Ignore());

            CreateMap<Permission, PermissionDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PermissionID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PermissionName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Module, opt => opt.MapFrom(src => src.Module))
                .ForMember(dest => dest.RowVersionString, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));

            CreateMap<CreatePermissionDto, Permission>()
                .ForMember(dest => dest.PermissionID, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
                .ForMember(dest => dest.RolePermissions, opt => opt.Ignore())
                .ForMember(dest => dest.UserPermissions, opt => opt.Ignore())
                .ForMember(dest => dest.PermissionGroupMappings, opt => opt.Ignore());

            CreateMap<UpdatePermissionDto, Permission>()
                .ForMember(dest => dest.PermissionID, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
                .ForMember(dest => dest.RolePermissions, opt => opt.Ignore())
                .ForMember(dest => dest.UserPermissions, opt => opt.Ignore())
                .ForMember(dest => dest.PermissionGroupMappings, opt => opt.Ignore());

            #endregion

            #region Category Mappings

            // Category Mappings
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.CategoryID, opt => opt.MapFrom(src => src.CategoryID))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
                .ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => src.DisplayOrder))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.ParentCategoryID, opt => opt.MapFrom(src => src.ParentCategoryID))
                .ForMember(dest => dest.GroupCount, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.RowVersionString, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));

            CreateMap<CreateCategoryDto, Category>()
                .ForMember(dest => dest.CategoryID, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

            CreateMap<UpdateCategoryDto, Category>()
                .ForMember(dest => dest.CategoryID, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

            #endregion

            #region Vocabulary Group Mappings

            // Vocabulary Group Mappings
            CreateMap<VocabularyGroup, VocabularyGroupDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GroupID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GroupName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryID))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedByUserID))
                .ForMember(dest => dest.CreatedByUsername, opt => opt.MapFrom(src => src.CreatedByUser != null ? src.CreatedByUser.Username : null))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.VocabularyCount, opt => opt.Ignore()) // Sẽ được điền ở service
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.RowVersionString, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));

            CreateMap<CreateVocabularyGroupDto, VocabularyGroup>()
                .ForMember(dest => dest.GroupID, opt => opt.Ignore())
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.GroupName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CategoryID, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedByUserID, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

            CreateMap<UpdateVocabularyGroupDto, VocabularyGroup>()
                .ForMember(dest => dest.GroupID, opt => opt.Ignore())
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.GroupName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CategoryID, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserID, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

            #endregion

            #region Vocabulary Mappings

            // Vocabulary Mappings
            CreateMap<Vocabulary, VocabularyDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Term, opt => opt.MapFrom(src => src.Term))
                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode))
                .ForMember(dest => dest.Reading, opt => opt.MapFrom(src => src.Reading))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
                .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.DifficultyLevel))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
                .ForMember(dest => dest.AudioFile, opt => opt.MapFrom(src => src.AudioFile))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModifiedAt))
                .ForMember(dest => dest.RowVersionString, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)))
                .ForMember(dest => dest.Definitions, opt => opt.MapFrom(src => src.Definitions))
                .ForMember(dest => dest.Examples, opt => opt.MapFrom(src => src.Examples))
                .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations));

            CreateMap<CreateVocabularyDto, Vocabulary>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

            CreateMap<UpdateVocabularyDto, Vocabulary>()
                .ForMember(dest => dest.VocabularyID, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

            // Vocabulary Reference Mapping
            CreateMap<Vocabulary, VocabularyReferenceDto>()
                .ForMember(dest => dest.VocabularyID, opt => opt.MapFrom(src => src.VocabularyID))
                .ForMember(dest => dest.Term, opt => opt.MapFrom(src => src.Term))
                .ForMember(dest => dest.Reading, opt => opt.MapFrom(src => src.Reading));

            // Definition Mappings
            CreateMap<Definition, DefinitionDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.PartOfSpeech, opt => opt.MapFrom(src => src.PartOfSpeech))
                .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.SortOrder));

            CreateMap<DefinitionDto, Definition>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.VocabularyItemId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

            // Example Mappings
            CreateMap<Example, ExampleDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.Translation, opt => opt.MapFrom(src => src.Translation))
                .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.DifficultyLevel));

            CreateMap<ExampleDto, Example>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.VocabularyItemId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

            // Translation Mappings
            CreateMap<Translation, TranslationDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode));

            CreateMap<TranslationDto, Translation>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.VocabularyItemId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

            #endregion

            #region Technical Term Mappings

            // Technical Term Mappings
            CreateMap<TechnicalTerm, TechnicalTermDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Term, opt => opt.MapFrom(src => src.Term))
                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode))
                .ForMember(dest => dest.Reading, opt => opt.MapFrom(src => src.Reading))
                .ForMember(dest => dest.Field, opt => opt.MapFrom(src => src.Field))
                .ForMember(dest => dest.SubField, opt => opt.MapFrom(src => src.SubField))
                .ForMember(dest => dest.Abbreviation, opt => opt.MapFrom(src => src.Abbreviation))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.Definition, opt => opt.MapFrom(src => src.Definition))
                .ForMember(dest => dest.Context, opt => opt.MapFrom(src => src.Context))
                .ForMember(dest => dest.RelatedTerms, opt => opt.MapFrom(src => src.RelatedTerms))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModifiedAt))
                .ForMember(dest => dest.RowVersionString, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));

            CreateMap<CreateTechnicalTermDto, TechnicalTerm>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

            CreateMap<UpdateTechnicalTermDto, TechnicalTerm>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

            // Term Example Mappings
            CreateMap<TermExample, TermExampleDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.Translation, opt => opt.MapFrom(src => src.Translation))
                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode))
                .ForMember(dest => dest.Context, opt => opt.MapFrom(src => src.Context));

            CreateMap<TermExampleDto, TermExample>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TermId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Term Translation Mappings
            CreateMap<TermTranslation, TermTranslationDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode));

            CreateMap<TermTranslationDto, TermTranslation>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TermId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            #endregion

            #region Learning Progress Mappings

            // Learning Progress Mappings
            CreateMap<LearningProgress, LearningProgressDto>()
                .ForMember(dest => dest.ProgressID, opt => opt.MapFrom(src => src.ProgressID))
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dest => dest.VocabularyID, opt => opt.MapFrom(src => src.VocabularyID))
                .ForMember(dest => dest.StudyCount, opt => opt.MapFrom(src => src.StudyCount))
                .ForMember(dest => dest.CorrectCount, opt => opt.MapFrom(src => src.CorrectCount))
                .ForMember(dest => dest.IncorrectCount, opt => opt.MapFrom(src => src.IncorrectCount))
                .ForMember(dest => dest.LastStudied, opt => opt.MapFrom(src => src.LastStudied))
                .ForMember(dest => dest.MemoryStrength, opt => opt.MapFrom(src => src.MemoryStrength))
                .ForMember(dest => dest.NextReviewDate, opt => opt.MapFrom(src => src.NextReviewDate));

            CreateMap<UpdateLearningProgressDto, LearningProgress>()
                .ForMember(dest => dest.ProgressID, opt => opt.Ignore())
                .ForMember(dest => dest.UserID, opt => opt.Ignore())
                .ForMember(dest => dest.VocabularyID, opt => opt.Ignore());

            #endregion

            #region Kanji Mappings

            // Kanji Mappings
            CreateMap<Kanji, KanjiDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.KanjiID))
                .ForMember(dest => dest.Character, opt => opt.MapFrom(src => src.Character))
                .ForMember(dest => dest.OnYomi, opt => opt.MapFrom(src => src.Onyomi))
                .ForMember(dest => dest.KunYomi, opt => opt.MapFrom(src => src.Kunyomi))
                .ForMember(dest => dest.Strokes, opt => opt.MapFrom(src => src.StrokeCount))
                .ForMember(dest => dest.JLPT, opt => opt.MapFrom(src => src.JLPTLevel))
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade))
                .ForMember(dest => dest.RadicalName, opt => opt.MapFrom(src => src.RadicalName))
                .ForMember(dest => dest.StrokeOrder, opt => opt.MapFrom(src => src.StrokeOrder))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.RowVersionString, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));

            CreateMap<CreateKanjiDto, Kanji>()
                .ForMember(dest => dest.KanjiID, opt => opt.Ignore())
                .ForMember(dest => dest.Onyomi, opt => opt.MapFrom(src => src.OnYomi))
                .ForMember(dest => dest.Kunyomi, opt => opt.MapFrom(src => src.KunYomi))
                .ForMember(dest => dest.StrokeCount, opt => opt.MapFrom(src => src.Strokes))
                .ForMember(dest => dest.JLPTLevel, opt => opt.MapFrom(src => src.JLPT))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

            CreateMap<UpdateKanjiDto, Kanji>()
                .ForMember(dest => dest.KanjiID, opt => opt.Ignore())
                .ForMember(dest => dest.Onyomi, opt => opt.MapFrom(src => src.OnYomi))
                .ForMember(dest => dest.Kunyomi, opt => opt.MapFrom(src => src.KunYomi))
                .ForMember(dest => dest.StrokeCount, opt => opt.MapFrom(src => src.Strokes))
                .ForMember(dest => dest.JLPTLevel, opt => opt.MapFrom(src => src.JLPT))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

            // Kanji Reference Mapping
            CreateMap<KanjiVocabulary, KanjiReferenceDto>()
                .ForMember(dest => dest.KanjiID, opt => opt.MapFrom(src => src.KanjiID))
                .ForMember(dest => dest.Character, opt => opt.MapFrom(src => src.Kanji.Character))
                .ForMember(dest => dest.Onyomi, opt => opt.MapFrom(src => src.Kanji.Onyomi))
                .ForMember(dest => dest.Kunyomi, opt => opt.MapFrom(src => src.Kanji.Kunyomi))
                .ForMember(dest => dest.Meaning, opt => opt.MapFrom(src => src.Kanji.Meanings.FirstOrDefault().Text))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position));

            // Kanji Meaning Mappings
            CreateMap<KanjiMeaning, KanjiMeaningDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode));

            CreateMap<KanjiMeaningDto, KanjiMeaning>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.KanjiId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Kanji Example Mappings
            CreateMap<KanjiExample, KanjiExampleDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Word, opt => opt.MapFrom(src => src.Word))
                .ForMember(dest => dest.Reading, opt => opt.MapFrom(src => src.Reading))
                .ForMember(dest => dest.Meaning, opt => opt.MapFrom(src => src.Meaning))
                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode));

            CreateMap<KanjiExampleDto, KanjiExample>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.KanjiId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // User Kanji Progress Mapping
            CreateMap<UserKanjiProgress, UserKanjiProgressDto>()
                .ForMember(dest => dest.ProgressID, opt => opt.MapFrom(src => src.ProgressID))
                .ForMember(dest => dest.KanjiID, opt => opt.MapFrom(src => src.KanjiID))
                .ForMember(dest => dest.Character, opt => opt.MapFrom(src => src.Kanji.Character))
                .ForMember(dest => dest.LastPracticed, opt => opt.MapFrom(src => src.LastPracticed))
                .ForMember(dest => dest.RecognitionLevel, opt => opt.MapFrom(src => src.RecognitionLevel))
                .ForMember(dest => dest.WritingLevel, opt => opt.MapFrom(src => src.WritingLevel))
                .ForMember(dest => dest.StudyCount, opt => opt.MapFrom(src => src.StudyCount))
                .ForMember(dest => dest.NextReviewDate, opt => opt.MapFrom(src => src.NextReviewDate));

            #endregion

            #region Grammar Mappings

            // Grammar Mappings
            CreateMap<Grammar, GrammarDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GrammarID))
                .ForMember(dest => dest.Pattern, opt => opt.MapFrom(src => src.Pattern))
                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode))
                .ForMember(dest => dest.Reading, opt => opt.MapFrom(src => src.Reading))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.JLPTLevel))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
                .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.DifficultyLevel))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModifiedAt))
                .ForMember(dest => dest.RowVersionString, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));

            // Grammar Definition Mappings
            CreateMap<GrammarDefinition, GrammarDefinitionDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.Usage, opt => opt.MapFrom(src => src.Usage))
                .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.SortOrder));

            CreateMap<GrammarDefinitionDto, GrammarDefinition>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.GrammarId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Grammar Example Mappings
            CreateMap<GrammarExample, GrammarExampleDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.JapaneseSentence, opt => opt.MapFrom(src => src.JapaneseSentence))
                .ForMember(dest => dest.Reading, opt => opt.MapFrom(src => src.Reading))
                .ForMember(dest => dest.TranslationText, opt => opt.MapFrom(src => src.TranslationText))
                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode))
                .ForMember(dest => dest.Context, opt => opt.MapFrom(src => src.Context))
                .ForMember(dest => dest.AudioFile, opt => opt.MapFrom(src => src.AudioFile));

            CreateMap<GrammarExampleDto, GrammarExample>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.GrammarId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Grammar Translation Mappings
            CreateMap<GrammarTranslation, GrammarTranslationDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode));

            CreateMap<GrammarTranslationDto, GrammarTranslation>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.GrammarId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Grammar Reference Mapping
            CreateMap<Grammar, GrammarReferenceDto>()
                .ForMember(dest => dest.GrammarID, opt => opt.MapFrom(src => src.GrammarID))
                .ForMember(dest => dest.Pattern, opt => opt.MapFrom(src => src.Pattern))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.JLPTLevel));

            // User Grammar Progress Mapping
            CreateMap<UserGrammarProgress, UserGrammarProgressDto>()
                .ForMember(dest => dest.ProgressID, opt => opt.MapFrom(src => src.ProgressID))
                .ForMember(dest => dest.GrammarID, opt => opt.MapFrom(src => src.GrammarID))
                .ForMember(dest => dest.Pattern, opt => opt.MapFrom(src => src.Grammar.Pattern))
                .ForMember(dest => dest.LastStudied, opt => opt.MapFrom(src => src.LastStudied))
                .ForMember(dest => dest.UnderstandingLevel, opt => opt.MapFrom(src => src.UnderstandingLevel))
                .ForMember(dest => dest.UsageLevel, opt => opt.MapFrom(src => src.UsageLevel))
                .ForMember(dest => dest.StudyCount, opt => opt.MapFrom(src => src.StudyCount))
                .ForMember(dest => dest.TestScore, opt => opt.MapFrom(src => src.TestScore))
                .ForMember(dest => dest.NextReviewDate, opt => opt.MapFrom(src => src.NextReviewDate))
                .ForMember(dest => dest.PersonalNotes, opt => opt.MapFrom(src => src.PersonalNotes))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

            #endregion

            #region User Personal Vocabulary Mappings

            // User Personal Vocabulary Mappings
            CreateMap<UserPersonalVocabulary, UserPersonalVocabularyDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dest => dest.Term, opt => opt.MapFrom(src => src.Term))
                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode))
                .ForMember(dest => dest.Reading, opt => opt.MapFrom(src => src.Reading))
                .ForMember(dest => dest.PersonalNote, opt => opt.MapFrom(src => src.PersonalNote))
                .ForMember(dest => dest.Context, opt => opt.MapFrom(src => src.Context))
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.Source))
                .ForMember(dest => dest.Importance, opt => opt.MapFrom(src => src.Importance))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.ImagePath))
                .ForMember(dest => dest.AudioPath, opt => opt.MapFrom(src => src.AudioPath))
                .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.IsPublic))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.RowVersionString, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));

            CreateMap<UserPersonalDefinition, UserPersonalDefinitionDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.PartOfSpeech, opt => opt.MapFrom(src => src.PartOfSpeech))
                .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.SortOrder));

            CreateMap<UserPersonalExample, UserPersonalExampleDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.Translation, opt => opt.MapFrom(src => src.Translation))
                .ForMember(dest => dest.Context, opt => opt.MapFrom(src => src.Context));

            CreateMap<UserPersonalTranslation, UserPersonalTranslationDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode));

            #endregion
        }
    }
}