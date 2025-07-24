using AutoMapper;
using LexiFlow.API.Data.Entities;
using LexiFlow.API.DTOs.Auth;
using LexiFlow.API.DTOs.Learning;
using LexiFlow.API.DTOs.Vocabulary;

namespace LexiFlow.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User Mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName))
                .ForMember(dest => dest.Roles, opt => opt.Ignore());

            CreateMap<RegisterUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Vocabulary Mappings
            CreateMap<Vocabulary, VocabularyDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.VocabularyID))
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Group.GroupName))
                .ForMember(dest => dest.Kanjis, opt => opt.Ignore());

            CreateMap<CreateVocabularyDto, Vocabulary>()
                .ForMember(dest => dest.VocabularyID, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<UpdateVocabularyDto, Vocabulary>()
                .ForMember(dest => dest.VocabularyID, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<VocabularyGroup, VocabularyGroupDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GroupID))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.VocabularyCount, opt => opt.Ignore());

            CreateMap<CreateVocabularyGroupDto, VocabularyGroup>()
                .ForMember(dest => dest.GroupID, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<Kanji, KanjiDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.KanjiID))
                .ForMember(dest => dest.KanjiComponents, opt => opt.Ignore());

            CreateMap<CreateKanjiDto, Kanji>()
                .ForMember(dest => dest.KanjiID, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<KanjiComponent, KanjiComponentDto>();

            // Learning Progress Mappings
            CreateMap<LearningProgress, LearningProgressDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProgressID))
                .ForMember(dest => dest.Vocabulary, opt => opt.Ignore());

            CreateMap<PersonalWordList, PersonalWordListDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ListID))
                .ForMember(dest => dest.ItemCount, opt => opt.Ignore());

            CreateMap<CreatePersonalWordListDto, PersonalWordList>()
                .ForMember(dest => dest.ListID, opt => opt.Ignore())
                .ForMember(dest => dest.UserID, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<PersonalWordListItem, PersonalWordListItemDto>()
                .ForMember(dest => dest.Vocabulary, opt => opt.Ignore());

            CreateMap<StudyPlan, StudyPlanDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PlanID))
                .ForMember(dest => dest.GoalsCount, opt => opt.Ignore());

            CreateMap<CreateStudyPlanDto, StudyPlan>()
                .ForMember(dest => dest.PlanID, opt => opt.Ignore())
                .ForMember(dest => dest.UserID, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CurrentStatus, opt => opt.MapFrom(src => "New"))
                .ForMember(dest => dest.CompletionPercentage, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<StudyGoal, StudyGoalDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GoalID))
                .ForMember(dest => dest.LevelName, opt => opt.MapFrom(src => src.Level.LevelName))
                .ForMember(dest => dest.TopicName, opt => opt.MapFrom(src => src.Topic.TopicName))
                .ForMember(dest => dest.TasksCount, opt => opt.Ignore())
                .ForMember(dest => dest.CompletedTasksCount, opt => opt.Ignore());

            CreateMap<CreateStudyGoalDto, StudyGoal>()
                .ForMember(dest => dest.GoalID, opt => opt.Ignore())
                .ForMember(dest => dest.PlanID, opt => opt.Ignore())
                .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.ProgressPercentage, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<UpdateStudyGoalDto, StudyGoal>()
                .ForMember(dest => dest.GoalID, opt => opt.Ignore())
                .ForMember(dest => dest.PlanID, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}