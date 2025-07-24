using LexiFlow.API.DTOs;
using LexiFlow.API.DTOs.Vocabulary;

namespace LexiFlow.API.Services
{
    public interface IVocabularyService
    {
        Task<ApiResponse<PagedResponseDto<VocabularyDto>>> GetVocabulariesAsync(int pageIndex = 0, int pageSize = 10, string? search = null, string? level = null, int? groupId = null);
        Task<ApiResponse<VocabularyDto>> GetVocabularyByIdAsync(int id);
        Task<ApiResponse<VocabularyDto>> CreateVocabularyAsync(CreateVocabularyDto createDto, int currentUserId);
        Task<ApiResponse<VocabularyDto>> UpdateVocabularyAsync(int id, UpdateVocabularyDto updateDto, int currentUserId);
        Task<ApiResponse<bool>> DeleteVocabularyAsync(int id);
        Task<ApiResponse<PagedResponseDto<VocabularyGroupDto>>> GetVocabularyGroupsAsync(int pageIndex = 0, int pageSize = 10, string? search = null);
        Task<ApiResponse<VocabularyGroupDto>> GetVocabularyGroupByIdAsync(int id);
        Task<ApiResponse<VocabularyGroupDto>> CreateVocabularyGroupAsync(CreateVocabularyGroupDto createDto, int currentUserId);
        Task<ApiResponse<VocabularyGroupDto>> UpdateVocabularyGroupAsync(int id, UpdateVocabularyGroupDto updateDto, int currentUserId);
        Task<ApiResponse<bool>> DeleteVocabularyGroupAsync(int id);
        Task<ApiResponse<PagedResponseDto<KanjiDto>>> GetKanjisAsync(int pageIndex = 0, int pageSize = 10, string? search = null, string? level = null);
        Task<ApiResponse<KanjiDto>> GetKanjiByIdAsync(int id);
        Task<ApiResponse<KanjiDto>> CreateKanjiAsync(CreateKanjiDto createDto, int currentUserId);
        Task<ApiResponse<KanjiDto>> UpdateKanjiAsync(int id, CreateKanjiDto updateDto, int currentUserId);
        Task<ApiResponse<bool>> DeleteKanjiAsync(int id);
    }
}
