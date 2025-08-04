//using AutoMapper;
//using LexiFlow.API.DTOs.Auth;
//using LexiFlow.API.DTOs.Learning;
//using LexiFlow.API.DTOs.Vocabulary;
//using LexiFlow.API.DTOs.Kanji;
//using LexiFlow.API.DTOs.Grammar;
//using LexiFlow.API.DTOs.User;
//using LexiFlow.API.DTOs.Category;
//using LexiFlow.API.DTOs.TechnicalTerm;
//using LexiFlow.API.DTOs.VocabularyGroup;
//using LexiFlow.API.DTOs.StudyPlan;
//using LexiFlow.Models;
//using LexiFlow.Models.Planning;
//using System;

//namespace LexiFlow.API.Helpers
//{
//    public class AutoMapperProfile : Profile
//    {
//        public AutoMapperProfile()
//        {
//            #region User Management Mappings

//            #region User Mappings
//            // User Mappings
//            // User → UserProfileDto
//            CreateMap<User, UserProfileDto>()
//                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserID))
//                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
//                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
//                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
//                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
//                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
//                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
//                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
//                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
//                .ForMember(dest => dest.AvatarUrl, opt => opt.Ignore()) // Not available in User entity
//                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
//                .ForMember(dest => dest.IsEmailVerified, opt => opt.Ignore()) // Not available in User entity
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
//                .ForMember(dest => dest.LastLoginAt, opt => opt.MapFrom(src => src.LastLogin));

//            // CreateUserDto → User
//            CreateMap<CreateUserDto, User>()
//                .ForMember(dest => dest.UserID, opt => opt.Ignore())
//                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Password will be hashed in service
//                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
//                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
//                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
//                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
//                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
//                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
//                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
//                .ForMember(dest => dest.UserRoles, opt => opt.Ignore()) // Handled in service
//                .ForMember(dest => dest.UserPermissions, opt => opt.Ignore());

//            // RegisterDto → User
//            //CreateMap<RegisterDto, User>()
//            //    .ForMember(dest => dest.UserID, opt => opt.Ignore())
//            //    .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Password will be hashed in service
//            //    .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
//            //    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
//            //    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
//            //    .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
//            //    .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
//            //    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//            //    .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//            //    .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

//            // UpdateUserProfileDto → User
//            CreateMap<UpdateUserProfileDto, User>()
//                .ForMember(dest => dest.UserID, opt => opt.Ignore())
//                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
//                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
//                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
//                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
//                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
//                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
//                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
//                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
//                .ForMember(dest => dest.Username, opt => opt.Ignore())
//                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

//            #endregion

//            #region Department MAPpings

//            // Department Mappings
//            CreateMap<Department, DepartmentDto>()
//                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DepartmentID))
//                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DepartmentName))
//                .ForMember(dest => dest.DepartmentCode, opt => opt.MapFrom(src => src.DepartmentCode))
//                .ForMember(dest => dest.ParentDepartmentID, opt => opt.MapFrom(src => src.ParentDepartmentID))
//                .ForMember(dest => dest.ParentDepartmentName, opt => opt.MapFrom(src => src.ParentDepartment != null ? src.ParentDepartment.DepartmentName : null))
//                .ForMember(dest => dest.ManagerID, opt => opt.MapFrom(src => src.ManagerUserID))
//                .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager != null ? $"{src.Manager.FirstName} {src.Manager.LastName}" : null))
//                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
//                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
//                .ForMember(dest => dest.RowVersionString, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));

//            CreateMap<CreateDepartmentDto, Department>()
//                .ForMember(dest => dest.DepartmentID, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

//            CreateMap<UpdateDepartmentDto, Department>()
//                .ForMember(dest => dest.DepartmentID, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
//                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

//            #endregion

//            #region Team Mappings

//            // Team Mappings
//            CreateMap<Team, TeamDto>()
//                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TeamID))
//                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TeamName))
//                .ForMember(dest => dest.DepartmentID, opt => opt.MapFrom(src => src.DepartmentID))
//                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.DepartmentName : null))
//                .ForMember(dest => dest.LeaderID, opt => opt.MapFrom(src => src.LeaderUserID))
//                .ForMember(dest => dest.LeaderName, opt => opt.MapFrom(src => src.Leader != null ? $"{src.Leader.FirstName} {src.Leader.LastName}" : null))
//                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
//                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
//                .ForMember(dest => dest.RowVersionString, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));

//            CreateMap<CreateTeamDto, Team>()
//                .ForMember(dest => dest.TeamID, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

//            CreateMap<UpdateTeamDto, Team>()
//                .ForMember(dest => dest.TeamID, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
//                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());
//            #endregion

//            #region Role Mappings

//            // Role & Permission Mappings
//            CreateMap<Role, RoleDto>()
//               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleID))
//               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RoleName))
//               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
//               .ForMember(dest => dest.IsSystemRole, opt => opt.MapFrom(src => false)) // Not available in Role entity
//               .ForMember(dest => dest.Permissions, opt => opt.Ignore()) // Will be populated in service
//               .ForMember(dest => dest.UsersCount, opt => opt.MapFrom(src => src.UserRoles != null ? src.UserRoles.Count : 0))
//               .ForMember(dest => dest.RowVersionString, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));

//            CreateMap<CreateRoleDto, Role>()
//                .ForMember(dest => dest.RoleID, opt => opt.Ignore())
//                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
//                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
//                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
//                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
//                .ForMember(dest => dest.RolePermissions, opt => opt.Ignore());

//            CreateMap<UpdateRoleDto, Role>()
//                .ForMember(dest => dest.RoleID, opt => opt.Ignore())
//                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
//                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
//                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
//                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
//                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
//                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
//                .ForMember(dest => dest.RolePermissions, opt => opt.Ignore());

//            #endregion

//            #region Permission Mappings

//            CreateMap<Permission, PermissionDto>()
//                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PermissionID))
//                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PermissionName))
//                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
//                .ForMember(dest => dest.Module, opt => opt.MapFrom(src => src.Module))
//                .ForMember(dest => dest.RowVersionString, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));

//            CreateMap<CreatePermissionDto, Permission>()
//                .ForMember(dest => dest.PermissionID, opt => opt.Ignore())
//                .ForMember(dest => dest.PermissionName, opt => opt.MapFrom(src => src.PermissionName))
//                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
//                .ForMember(dest => dest.Module, opt => opt.MapFrom(src => src.Module))
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
//                .ForMember(dest => dest.RolePermissions, opt => opt.Ignore())
//                .ForMember(dest => dest.UserPermissions, opt => opt.Ignore())
//                .ForMember(dest => dest.PermissionGroupMappings, opt => opt.Ignore());

//            CreateMap<UpdatePermissionDto, Permission>()
//               .ForMember(dest => dest.PermissionID, opt => opt.Ignore())
//               .ForMember(dest => dest.PermissionName, opt => opt.MapFrom(src => src.PermissionName))
//               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
//               .ForMember(dest => dest.Module, opt => opt.MapFrom(src => src.Module))
//               .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
//               .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//               .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
//               .ForMember(dest => dest.RolePermissions, opt => opt.Ignore())
//               .ForMember(dest => dest.UserPermissions, opt => opt.Ignore())
//               .ForMember(dest => dest.PermissionGroupMappings, opt => opt.Ignore());

//            #endregion

//            #endregion

//            #region Category Mappings

//            // Category Mappings
//            CreateMap<Category, CategoryDto>()
//                .ForMember(dest => dest.CategoryID, opt => opt.MapFrom(src => src.CategoryID))
//                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
//                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
//                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
//                .ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => src.DisplayOrder))
//                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
//                .ForMember(dest => dest.ParentCategoryID, opt => opt.MapFrom(src => src.ParentCategoryID))
//                .ForMember(dest => dest.GroupCount, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
//                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
//                .ForMember(dest => dest.RowVersionString, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));

//            CreateMap<CreateCategoryDto, Category>()
//                .ForMember(dest => dest.CategoryID, opt => opt.Ignore())
//                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
//                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
//                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
//                .ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => src.DisplayOrder))
//                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
//                .ForMember(dest => dest.ParentCategoryID, opt => opt.MapFrom(src => src.ParentCategoryID))
//                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Set in service
//                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore()) // Set in service
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
//                .ForMember(dest => dest.ParentCategory, opt => opt.Ignore())
//                .ForMember(dest => dest.ChildCategories, opt => opt.Ignore())
//                .ForMember(dest => dest.Vocabularies, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore())
//                .ForMember(dest => dest.UpdatedByUser, opt => opt.Ignore());

//            CreateMap<UpdateCategoryDto, Category>()
//                .ForMember(dest => dest.CategoryID, opt => opt.Ignore())
//                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
//                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
//                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
//                .ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => src.DisplayOrder))
//                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
//                .ForMember(dest => dest.ParentCategoryID, opt => opt.MapFrom(src => src.ParentCategoryID))
//                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
//                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore()) // Set in service
//                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.RowVersion, opt => opt.Ignore()) // Handled in service for concurrency
//                .ForMember(dest => dest.ParentCategory, opt => opt.Ignore())
//                .ForMember(dest => dest.ChildCategories, opt => opt.Ignore())
//                .ForMember(dest => dest.Vocabularies, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore())
//                .ForMember(dest => dest.UpdatedByUser, opt => opt.Ignore());

//            #endregion

//            #region Vocabulary Group Mappings

//            // Vocabulary Group Mappings
//            CreateMap<VocabularyGroup, VocabularyGroupDto>()
//                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GroupID))
//                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GroupName))
//                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
//                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryID))
//                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
//                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedByUserID))
//                .ForMember(dest => dest.CreatedByUsername, opt => opt.MapFrom(src => src.CreatedByUser != null ? src.CreatedByUser.Username : null))
//                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
//                .ForMember(dest => dest.VocabularyCount, opt => opt.Ignore()) // Sẽ được điền ở service
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
//                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
//                .ForMember(dest => dest.RowVersionString, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));

//            CreateMap<CreateVocabularyGroupDto, VocabularyGroup>()
//                .ForMember(dest => dest.GroupID, opt => opt.Ignore())
//                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.GroupName))
//                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
//                .ForMember(dest => dest.CategoryID, opt => opt.MapFrom(src => src.CategoryId))
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.CreatedByUserID, opt => opt.Ignore())
//                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
//                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

//            CreateMap<UpdateVocabularyGroupDto, VocabularyGroup>()
//                .ForMember(dest => dest.GroupID, opt => opt.Ignore())
//                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.GroupName))
//                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
//                .ForMember(dest => dest.CategoryID, opt => opt.MapFrom(src => src.CategoryId))
//                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
//                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedByUserID, opt => opt.Ignore())
//                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

//            #endregion

//            #region Vocabulary Mappings

//            // Vocabulary Mappings
//            CreateMap<Vocabulary, VocabularyDto>()
//                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
//                .ForMember(dest => dest.Term, opt => opt.MapFrom(src => src.Term))
//                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode))
//                .ForMember(dest => dest.Reading, opt => opt.MapFrom(src => src.Reading))
//                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
//                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
//                .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.DifficultyLevel))
//                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
//                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
//                .ForMember(dest => dest.AudioFile, opt => opt.MapFrom(src => src.AudioFile))
//                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
//                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModifiedAt))
//                .ForMember(dest => dest.RowVersionString, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)))
//                .ForMember(dest => dest.Definitions, opt => opt.MapFrom(src => src.Definitions))
//                .ForMember(dest => dest.Examples, opt => opt.MapFrom(src => src.Examples))
//                .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations));

//            CreateMap<Vocabulary, VocabularyDetailDto>()
//                .ForMember(dest => dest.Vocabulary, opt => opt.MapFrom(src => src))
//                .ForMember(dest => dest.Kanjis, opt => opt.MapFrom(src => src.Term))
//                .ForMember(dest => dest.RelatedGrammar, opt => opt.Ignore()) // Requires custom resolution
//                .ForMember(dest => dest.Synonyms, opt => opt.Ignore()) // Requires custom resolution from VocabularyRelation
//                .ForMember(dest => dest.Antonyms, opt => opt.Ignore()) // Requires custom resolution from VocabularyRelation
//                .ForMember(dest => dest.UserProgress, opt => opt.Ignore()); // Requires user context


//            CreateMap<CreateVocabularyDto, Vocabulary>()
//                .ForMember(dest => dest.Id, opt => opt.Ignore())
//                .ForMember(dest => dest.Term, opt => opt.MapFrom(src => src.Term))
//                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode))
//                .ForMember(dest => dest.Reading, opt => opt.MapFrom(src => src.Reading))
//                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
//                .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.DifficultyLevel))
//                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
//                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
//                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
//                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Set in service
//                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore()) // Set in service
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
//                .ForMember(dest => dest.Category, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore())
//                .ForMember(dest => dest.ModifiedByUser, opt => opt.Ignore());

//            CreateMap<UpdateVocabularyDto, Vocabulary>()
//                .ForMember(dest => dest.Id, opt => opt.Ignore())
//                .ForMember(dest => dest.Term, opt => opt.MapFrom(src => src.Term))
//                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode))
//                .ForMember(dest => dest.Reading, opt => opt.MapFrom(src => src.Reading))
//                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
//                .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.DifficultyLevel))
//                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
//                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
//                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
//                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
//                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore()) // Set in service
//                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.RowVersion, opt => opt.Ignore()) // Handled in service for concurrency
//                .ForMember(dest => dest.Category, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore())
//                .ForMember(dest => dest.ModifiedByUser, opt => opt.Ignore());

//            // Vocabulary Reference Mapping
//            CreateMap<Vocabulary, VocabularyReferenceDto>()
//                .ForMember(dest => dest.VocabularyID, opt => opt.MapFrom(src => src.Id))
//                .ForMember(dest => dest.Term, opt => opt.MapFrom(src => src.Term))
//                .ForMember(dest => dest.Reading, opt => opt.MapFrom(src => src.Reading));

//            #endregion

//            #region Definition Mappings

//            // Definition → DefinitionDto
//            CreateMap<Definition, DefinitionDto>()
//                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
//                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
//                .ForMember(dest => dest.PartOfSpeech, opt => opt.MapFrom(src => src.PartOfSpeech))
//                .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.SortOrder));

//            // DefinitionDto → Definition
//            CreateMap<DefinitionDto, Definition>()
//                .ForMember(dest => dest.Id, opt => opt.Ignore())
//                .ForMember(dest => dest.VocabularyItemId, opt => opt.Ignore()) // Set in service when adding to vocabulary
//                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
//                .ForMember(dest => dest.PartOfSpeech, opt => opt.MapFrom(src => src.PartOfSpeech))
//                .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.SortOrder))
//                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Set in service
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
//                .ForMember(dest => dest.VocabularyItem, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore());

//            #endregion

//            #region Example Mappings

//            // Example → ExampleDto
//            CreateMap<Example, ExampleDto>()
//                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
//                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
//                .ForMember(dest => dest.Translation, opt => opt.MapFrom(src => src.Translation))
//                .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.DifficultyLevel));

//            // ExampleDto → Example
//            CreateMap<ExampleDto, Example>()
//                .ForMember(dest => dest.Id, opt => opt.Ignore())
//                .ForMember(dest => dest.VocabularyItemId, opt => opt.Ignore()) // Set in service when adding to vocabulary
//                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
//                .ForMember(dest => dest.Translation, opt => opt.MapFrom(src => src.Translation))
//                .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.DifficultyLevel))
//                .ForMember(dest => dest.AudioFile, opt => opt.Ignore()) // Not in DTO
//                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Set in service
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
//                .ForMember(dest => dest.VocabularyItem, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore());

//            #endregion

//            #region Translation Mappings

//            // Translation → TranslationDto
//            CreateMap<Translation, TranslationDto>()
//                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
//                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
//                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode));

//            // TranslationDto → Translation
//            CreateMap<TranslationDto, Translation>()
//                .ForMember(dest => dest.Id, opt => opt.Ignore())
//                .ForMember(dest => dest.VocabularyItemId, opt => opt.Ignore()) // Set in service when adding to vocabulary
//                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
//                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode))
//                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Set in service
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
//                .ForMember(dest => dest.VocabularyItem, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore());

//            #endregion

//            #region Kanji-Vocabulary Mappings

//            // KanjiVocabulary → KanjiReferenceDto
//            //CreateMap<KanjiVocabulary, KanjiReferenceDto>()
//            //    .ForMember(dest => dest.KanjiID, opt => opt.MapFrom(src => src.KanjiID))
//            //    .ForMember(dest => dest.Character, opt => opt.MapFrom(src => src.Kanji.Character))
//            //    .ForMember(dest => dest.Onyomi, opt => opt.MapFrom(src => src.Kanji.OnYomi))
//            //    .ForMember(dest => dest.Kunyomi, opt => opt.MapFrom(src => src.Kanji.KunYomi))
//            //    .ForMember(dest => dest.Meaning, opt => opt.MapFrom(src =>
//            //        src.Kanji.Meanings != null && src.Kanji.Meanings.Any()
//            //            ? src.Kanji.Meanings.First().Text
//            //            : string.Empty))
//            //    .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position ?? 0));

//            #endregion

//            #region Technical Term Mappings

//            // Technical Term Mappings
//            CreateMap<TechnicalTerm, TechnicalTermDto>()
//                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
//                .ForMember(dest => dest.Term, opt => opt.MapFrom(src => src.Term))
//                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode))
//                .ForMember(dest => dest.Reading, opt => opt.MapFrom(src => src.Reading))
//                .ForMember(dest => dest.Field, opt => opt.MapFrom(src => src.Field))
//                .ForMember(dest => dest.SubField, opt => opt.MapFrom(src => src.SubField))
//                .ForMember(dest => dest.Abbreviation, opt => opt.MapFrom(src => src.Abbreviation))
//                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
//                .ForMember(dest => dest.Definition, opt => opt.MapFrom(src => src.Definition))
//                .ForMember(dest => dest.Context, opt => opt.MapFrom(src => src.Context))
//                .ForMember(dest => dest.RelatedTerms, opt => opt.MapFrom(src => src.RelatedTerms))
//                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
//                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModifiedAt))
//                .ForMember(dest => dest.RowVersionString, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));

//            CreateMap<CreateTechnicalTermDto, TechnicalTerm>()
//                .ForMember(dest => dest.Id, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
//                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
//                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

//            CreateMap<UpdateTechnicalTermDto, TechnicalTerm>()
//                .ForMember(dest => dest.Id, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
//                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
//                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

//            // Term Example Mappings
//            CreateMap<TermExample, TermExampleDto>()
//                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
//                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
//                .ForMember(dest => dest.Translation, opt => opt.MapFrom(src => src.Translation))
//                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode))
//                .ForMember(dest => dest.Context, opt => opt.MapFrom(src => src.Context));

//            CreateMap<TermExampleDto, TermExample>()
//                .ForMember(dest => dest.Id, opt => opt.Ignore())
//                .ForMember(dest => dest.TermId, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

//            // Term Translation Mappings
//            CreateMap<TermTranslation, TermTranslationDto>()
//                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
//                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
//                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode));

//            CreateMap<TermTranslationDto, TermTranslation>()
//                .ForMember(dest => dest.Id, opt => opt.Ignore())
//                .ForMember(dest => dest.TermId, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

//            #endregion

//            #region Learning Progress Mappings

//            // Learning Progress Mappings
//            CreateMap<LearningProgress, LearningProgressDto>()
//                .ForMember(dest => dest.ProgressID, opt => opt.MapFrom(src => src.ProgressID))
//                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserID))
//                .ForMember(dest => dest.VocabularyID, opt => opt.MapFrom(src => src.VocabularyID))
//                .ForMember(dest => dest.StudyCount, opt => opt.MapFrom(src => src.StudyCount))
//                .ForMember(dest => dest.CorrectCount, opt => opt.MapFrom(src => src.CorrectCount))
//                .ForMember(dest => dest.IncorrectCount, opt => opt.MapFrom(src => src.IncorrectCount))
//                .ForMember(dest => dest.LastStudied, opt => opt.MapFrom(src => src.LastStudied))
//                .ForMember(dest => dest.MemoryStrength, opt => opt.MapFrom(src => src.MemoryStrength))
//                .ForMember(dest => dest.NextReviewDate, opt => opt.MapFrom(src => src.NextReviewDate));

//            CreateMap<UpdateLearningProgressDto, LearningProgress>()
//                .ForMember(dest => dest.ProgressID, opt => opt.Ignore())
//                .ForMember(dest => dest.UserID, opt => opt.Ignore())
//                .ForMember(dest => dest.VocabularyID, opt => opt.Ignore());

//            #endregion

//            #region Kanji Mappings

//            #region Kanji Mappings N

//            // Kanji → KanjiDto
//            //CreateMap<Kanji, KanjiDto>()
//            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
//            //    .ForMember(dest => dest.Character, opt => opt.MapFrom(src => src.Character))
//            //    .ForMember(dest => dest.OnYomi, opt => opt.MapFrom(src => src.OnYomi))
//            //    .ForMember(dest => dest.KunYomi, opt => opt.MapFrom(src => src.KunYomi))
//            //    .ForMember(dest => dest.Strokes, opt => opt.MapFrom(src => src.Strokes))
//            //    .ForMember(dest => dest.JLPT, opt => opt.MapFrom(src => src.JLPT))
//            //    .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade))
//            //    .ForMember(dest => dest.RadicalName, opt => opt.MapFrom(src => src.RadicalName))
//            //    .ForMember(dest => dest.StrokeOrder, opt => opt.MapFrom(src => src.StrokeOrder))
//            //    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
//            //    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
//            //    .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModifiedAt))
//            //    .ForMember(dest => dest.RowVersionString, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)))
//            //    .ForMember(dest => dest.Meanings, opt => opt.MapFrom(src => src.Meanings))
//            //    .ForMember(dest => dest.Examples, opt => opt.MapFrom(src => src.Examples));

//            //// CreateKanjiDto → Kanji
//            //CreateMap<CreateKanjiDto, Kanji>()
//            //    .ForMember(dest => dest.Id, opt => opt.Ignore())
//            //    .ForMember(dest => dest.Character, opt => opt.MapFrom(src => src.Character))
//            //    .ForMember(dest => dest.OnYomi, opt => opt.MapFrom(src => src.OnYomi))
//            //    .ForMember(dest => dest.KunYomi, opt => opt.MapFrom(src => src.KunYomi))
//            //    .ForMember(dest => dest.Strokes, opt => opt.MapFrom(src => src.Strokes))
//            //    .ForMember(dest => dest.JLPT, opt => opt.MapFrom(src => src.JLPT))
//            //    .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade))
//            //    .ForMember(dest => dest.RadicalName, opt => opt.MapFrom(src => src.RadicalName))
//            //    .ForMember(dest => dest.StrokeOrder, opt => opt.MapFrom(src => src.StrokeOrder))
//            //    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
//            //    .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Set in service
//            //    .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore()) // Set in service
//            //    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//            //    .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//            //    .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
//            //    .ForMember(dest => dest.Meanings, opt => opt.Ignore()) // Handled separately in service
//            //    .ForMember(dest => dest.Examples, opt => opt.Ignore()) // Handled separately in service
//            //    .ForMember(dest => dest.Components, opt => opt.Ignore()) // Handled separately in service
//            //    .ForMember(dest => dest.KanjiVocabularies, opt => opt.Ignore());

//            //// UpdateKanjiDto → Kanji
//            //CreateMap<UpdateKanjiDto, Kanji>()
//            //    .ForMember(dest => dest.Id, opt => opt.Ignore())
//            //    .ForMember(dest => dest.Character, opt => opt.Ignore()) // Character is immutable
//            //    .ForMember(dest => dest.OnYomi, opt => opt.MapFrom(src => src.OnYomi))
//            //    .ForMember(dest => dest.KunYomi, opt => opt.MapFrom(src => src.KunYomi))
//            //    .ForMember(dest => dest.Strokes, opt => opt.MapFrom(src => src.Strokes))
//            //    .ForMember(dest => dest.JLPT, opt => opt.MapFrom(src => src.JLPT))
//            //    .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade))
//            //    .ForMember(dest => dest.RadicalName, opt => opt.MapFrom(src => src.RadicalName))
//            //    .ForMember(dest => dest.StrokeOrder, opt => opt.MapFrom(src => src.StrokeOrder))
//            //    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
//            //    .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
//            //    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
//            //    .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore()) // Set in service
//            //    .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//            //    .ForMember(dest => dest.RowVersion, opt => opt.Ignore()) // Handled in service for concurrency
//            //    .ForMember(dest => dest.Meanings, opt => opt.Ignore()) // Handled separately in service
//            //    .ForMember(dest => dest.Examples, opt => opt.Ignore()) // Handled separately in service
//            //    .ForMember(dest => dest.Components, opt => opt.Ignore()) // Handled separately in service
//            //    .ForMember(dest => dest.KanjiVocabularies, opt => opt.Ignore());

//            #endregion

//            #region KanjiMeaning Mappings

//            //// KanjiMeaning → KanjiMeaningDto
//            //CreateMap<KanjiMeaning, KanjiMeaningDto>()
//            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
//            //    .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
//            //    .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode))
//            //    .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.SortOrder));

//            //// KanjiMeaningDto → KanjiMeaning
//            //CreateMap<KanjiMeaningDto, KanjiMeaning>()
//            //    .ForMember(dest => dest.Id, opt => opt.Ignore())
//            //    .ForMember(dest => dest.KanjiId, opt => opt.Ignore()) // Set in service when adding to kanji
//            //    .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
//            //    .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode))
//            //    .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.SortOrder))
//            //    .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Set in service
//            //    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//            //    .ForMember(dest => dest.Kanji, opt => opt.Ignore());

//            #endregion

//            #region KanjiExample Mappings

//            // KanjiExample → KanjiExampleDto
//            //CreateMap<KanjiExample, KanjiExampleDto>()
//            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
//            //    .ForMember(dest => dest.Word, opt => opt.MapFrom(src => src.Word))
//            //    .ForMember(dest => dest.Reading, opt => opt.MapFrom(src => src.Reading))
//            //    .ForMember(dest => dest.Meaning, opt => opt.MapFrom(src => src.Meaning))
//            //    .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode));

//            //// KanjiExampleDto → KanjiExample
//            //CreateMap<KanjiExampleDto, KanjiExample>()
//            //    .ForMember(dest => dest.Id, opt => opt.Ignore())
//            //    .ForMember(dest => dest.KanjiId, opt => opt.Ignore()) // Set in service when adding to kanji
//            //    .ForMember(dest => dest.Word, opt => opt.MapFrom(src => src.Word))
//            //    .ForMember(dest => dest.Reading, opt => opt.MapFrom(src => src.Reading))
//            //    .ForMember(dest => dest.Meaning, opt => opt.MapFrom(src => src.Meaning))
//            //    .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode))
//            //    .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Set in service
//            //    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//            //    .ForMember(dest => dest.Kanji, opt => opt.Ignore());

//            #endregion

//            #region KanjiComponent Mappings

//            // KanjiComponent → KanjiComponentDto
//            //CreateMap<KanjiComponent, KanjiComponentDto>()
//            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
//            //    .ForMember(dest => dest.ComponentId, opt => opt.MapFrom(src => src.ComponentId))
//            //    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
//            //    .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position));

//            //// KanjiComponentDto → KanjiComponent
//            //CreateMap<KanjiComponentDto, KanjiComponent>()
//            //    .ForMember(dest => dest.Id, opt => opt.Ignore())
//            //    .ForMember(dest => dest.KanjiId, opt => opt.Ignore()) // Set in service when adding to kanji
//            //    .ForMember(dest => dest.ComponentId, opt => opt.MapFrom(src => src.ComponentId))
//            //    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
//            //    .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
//            //    .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Set in service
//            //    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//            //    .ForMember(dest => dest.Kanji, opt => opt.Ignore())
//            //    .ForMember(dest => dest.Component, opt => opt.Ignore());

//            #endregion

//            #region UserKanjiProgress Mappings

//            // UserKanjiProgress → UserKanjiProgressDto
//            CreateMap<UserKanjiProgress, UserKanjiProgressDto>()
//                .ForMember(dest => dest.ProgressID, opt => opt.MapFrom(src => src.ProgressID))
//                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserID))
//                .ForMember(dest => dest.KanjiID, opt => opt.MapFrom(src => src.KanjiID))
//                .ForMember(dest => dest.Character, opt => opt.MapFrom(src => src.Kanji != null ? src.Kanji.Character : string.Empty))
//                .ForMember(dest => dest.RecognitionLevel, opt => opt.MapFrom(src => src.RecognitionLevel))
//                .ForMember(dest => dest.WritingLevel, opt => opt.MapFrom(src => src.WritingLevel))
//                .ForMember(dest => dest.LastPracticed, opt => opt.MapFrom(src => src.LastPracticed))
//                .ForMember(dest => dest.PracticeCount, opt => opt.MapFrom(src => src.PracticeCount))
//                .ForMember(dest => dest.CorrectCount, opt => opt.MapFrom(src => src.CorrectCount))
//                .ForMember(dest => dest.NextReviewDate, opt => opt.MapFrom(src => src.NextReviewDate))
//                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
//                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

//            // UpdateKanjiProgressDto → UserKanjiProgress
//            CreateMap<UpdateKanjiProgressDto, UserKanjiProgress>()
//                .ForMember(dest => dest.ProgressID, opt => opt.Ignore())
//                .ForMember(dest => dest.UserID, opt => opt.Ignore()) // Set in service
//                .ForMember(dest => dest.KanjiID, opt => opt.MapFrom(src => src.KanjiID))
//                .ForMember(dest => dest.RecognitionLevel, opt => opt.MapFrom(src => src.RecognitionLevel))
//                .ForMember(dest => dest.WritingLevel, opt => opt.MapFrom(src => src.WritingLevel))
//                .ForMember(dest => dest.LastPracticed, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
//                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.User, opt => opt.Ignore())
//                .ForMember(dest => dest.Kanji, opt => opt.Ignore());

//            #endregion
//            #endregion

//            #region Grammar Mappings

//            // Grammar Mappings
//            CreateMap<Grammar, GrammarDto>()
//                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
//                .ForMember(dest => dest.Pattern, opt => opt.MapFrom(src => src.Pattern))
//                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode))
//                .ForMember(dest => dest.Reading, opt => opt.MapFrom(src => src.Reading))
//                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
//                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
//                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
//                .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.DifficultyLevel))
//                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
//                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
//                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
//                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModifiedAt))
//                .ForMember(dest => dest.RowVersionString, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));

//            // Grammar Definition Mappings
//            CreateMap<GrammarDefinition, GrammarDefinitionDto>()
//                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
//                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
//                .ForMember(dest => dest.Usage, opt => opt.MapFrom(src => src.Usage))
//                .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.SortOrder));

//            CreateMap<GrammarDefinitionDto, GrammarDefinition>()
//                .ForMember(dest => dest.Id, opt => opt.Ignore())
//                .ForMember(dest => dest.GrammarId, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

//            // Grammar Example Mappings
//            CreateMap<GrammarExample, GrammarExampleDto>()
//                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
//                .ForMember(dest => dest.JapaneseSentence, opt => opt.MapFrom(src => src.JapaneseSentence))
//                .ForMember(dest => dest.Reading, opt => opt.MapFrom(src => src.Reading))
//                .ForMember(dest => dest.TranslationText, opt => opt.MapFrom(src => src.TranslationText))
//                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode))
//                .ForMember(dest => dest.Context, opt => opt.MapFrom(src => src.Context))
//                .ForMember(dest => dest.AudioFile, opt => opt.MapFrom(src => src.AudioFile));

//            CreateMap<GrammarExampleDto, GrammarExample>()
//                .ForMember(dest => dest.Id, opt => opt.Ignore())
//                .ForMember(dest => dest.GrammarId, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

//            // Grammar Translation Mappings
//            CreateMap<GrammarTranslation, GrammarTranslationDto>()
//                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
//                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
//                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode));

//            CreateMap<GrammarTranslationDto, GrammarTranslation>()
//                .ForMember(dest => dest.Id, opt => opt.Ignore())
//                .ForMember(dest => dest.GrammarId, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

//            // Grammar → GrammarReferenceDto
//            CreateMap<Grammar, GrammarReferenceDto>()
//                .ForMember(dest => dest.GrammarID, opt => opt.MapFrom(src => src.Id))
//                .ForMember(dest => dest.Pattern, opt => opt.MapFrom(src => src.Pattern))
//                .ForMember(dest => dest.GrammarPoint, opt => opt.MapFrom(src => src.Pattern)) // Assuming Pattern is the grammar point
//                .ForMember(dest => dest.Meaning, opt => opt.MapFrom(src =>
//                    src.Translations != null && src.Translations.Any(t => t.LanguageCode == "en")
//                        ? src.Translations.First(t => t.LanguageCode == "en").Text
//                        : (src.Translations != null && src.Translations.Any()
//                            ? src.Translations.First().Text
//                            : string.Empty)));

//            // User Grammar Progress Mapping
//            CreateMap<UserGrammarProgress, UserGrammarProgressDto>()
//                .ForMember(dest => dest.ProgressID, opt => opt.MapFrom(src => src.ProgressID))
//                .ForMember(dest => dest.GrammarID, opt => opt.MapFrom(src => src.GrammarID))
//                .ForMember(dest => dest.Pattern, opt => opt.MapFrom(src => src.Grammar.Pattern))
//                .ForMember(dest => dest.LastStudied, opt => opt.MapFrom(src => src.LastStudied))
//                .ForMember(dest => dest.UnderstandingLevel, opt => opt.MapFrom(src => src.UnderstandingLevel))
//                .ForMember(dest => dest.UsageLevel, opt => opt.MapFrom(src => src.UsageLevel))
//                .ForMember(dest => dest.StudyCount, opt => opt.MapFrom(src => src.StudyCount))
//                .ForMember(dest => dest.TestScore, opt => opt.MapFrom(src => src.TestScore))
//                .ForMember(dest => dest.NextReviewDate, opt => opt.MapFrom(src => src.NextReviewDate))
//                .ForMember(dest => dest.PersonalNotes, opt => opt.MapFrom(src => src.PersonalNotes))
//                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

//            #endregion

//            #region User Personal Vocabulary Mappings

//            // User Personal Vocabulary Mappings
//            CreateMap<UserPersonalVocabulary, UserPersonalVocabularyDto>()
//                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
//                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserID))
//                .ForMember(dest => dest.Term, opt => opt.MapFrom(src => src.Term))
//                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode))
//                .ForMember(dest => dest.Reading, opt => opt.MapFrom(src => src.Reading))
//                .ForMember(dest => dest.PersonalNote, opt => opt.MapFrom(src => src.PersonalNote))
//                .ForMember(dest => dest.Context, opt => opt.MapFrom(src => src.Context))
//                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.Source))
//                .ForMember(dest => dest.Importance, opt => opt.MapFrom(src => src.Importance))
//                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
//                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.ImagePath))
//                .ForMember(dest => dest.AudioPath, opt => opt.MapFrom(src => src.AudioPath))
//                .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.IsPublic))
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
//                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
//                .ForMember(dest => dest.RowVersionString, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));

//            CreateMap<UserPersonalDefinition, UserPersonalDefinitionDto>()
//                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
//                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
//                .ForMember(dest => dest.PartOfSpeech, opt => opt.MapFrom(src => src.PartOfSpeech))
//                .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.SortOrder));

//            CreateMap<UserPersonalExample, UserPersonalExampleDto>()
//                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
//                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
//                .ForMember(dest => dest.Translation, opt => opt.MapFrom(src => src.Translation))
//                .ForMember(dest => dest.Context, opt => opt.MapFrom(src => src.Context));

//            CreateMap<UserPersonalTranslation, UserPersonalTranslationDto>()
//                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
//                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
//                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode));

//            #endregion
//        }
//    }
//}