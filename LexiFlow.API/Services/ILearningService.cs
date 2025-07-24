using LexiFlow.API.DTOs;
using LexiFlow.API.DTOs.Learning;
using LexiFlow.API.DTOs.Vocabulary;

namespace LexiFlow.API.Services
{
    public interface ILearningService
    {
        Task<ApiResponse<PagedResponseDto<LearningProgressDto>>> GetUserVocabularyProgressAsync(int userId, int pageIndex = 0, int pageSize = 10, string? search = null, string? level = null);
        Task<ApiResponse<LearningProgressDto>> GetVocabularyProgressAsync(int userId, int vocabularyId);
        Task<ApiResponse<bool>> UpdateLearningProgressAsync(int userId, StudySessionResultDto studyResults);
        Task<ApiResponse<IEnumerable<VocabularyDto>>> GetDueVocabulariesAsync(int userId, int count = 10);
        Task<ApiResponse<PagedResponseDto<PersonalWordListDto>>> GetPersonalWordListsAsync(int userId, int pageIndex = 0, int pageSize = 10, string? search = null);
        Task<ApiResponse<PersonalWordListDto>> GetPersonalWordListByIdAsync(int userId, int listId);
        Task<ApiResponse<PersonalWordListDto>> CreatePersonalWordListAsync(int userId, CreatePersonalWordListDto createDto);
        Task<ApiResponse<PersonalWordListDto>> UpdatePersonalWordListAsync(int userId, int listId, UpdatePersonalWordListDto updateDto);
        Task<ApiResponse<bool>> DeletePersonalWordListAsync(int userId, int listId);
        Task<ApiResponse<IEnumerable<PersonalWordListItemDto>>> GetPersonalWordListItemsAsync(int userId, int listId);
        Task<ApiResponse<PersonalWordListItemDto>> AddWordToPersonalListAsync(int userId, int listId, AddWordToListDto addDto);
        Task<ApiResponse<bool>> RemoveWordFromPersonalListAsync(int userId, int listId, int vocabularyId);
        Task<ApiResponse<PagedResponseDto<StudyPlanDto>>> GetUserStudyPlansAsync(int userId, int pageIndex = 0, int pageSize = 10, bool activeOnly = false);
        Task<ApiResponse<StudyPlanDto>> GetStudyPlanByIdAsync(int userId, int planId);
        Task<ApiResponse<StudyPlanDto>> CreateStudyPlanAsync(int userId, CreateStudyPlanDto createDto);
        Task<ApiResponse<StudyPlanDto>> UpdateStudyPlanAsync(int userId, int planId, CreateStudyPlanDto updateDto);
        Task<ApiResponse<bool>> DeleteStudyPlanAsync(int userId, int planId);
        Task<ApiResponse<IEnumerable<StudyGoalDto>>> GetStudyPlanGoalsAsync(int userId, int planId);
        Task<ApiResponse<StudyGoalDto>> CreateStudyGoalAsync(int userId, int planId, CreateStudyGoalDto createDto);
        Task<ApiResponse<StudyGoalDto>> UpdateStudyGoalAsync(int userId, int planId, int goalId, UpdateStudyGoalDto updateDto);
        Task<ApiResponse<bool>> DeleteStudyGoalAsync(int userId, int planId, int goalId);
    }
}
