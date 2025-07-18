using LexiFlow.API.Models.Responses;
using LexiFlow.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LexiFlow.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class VocabularyController : ControllerBase
    {
        private readonly IVocabularyService _vocabularyService;
        private readonly ILogger<VocabularyController> _logger;

        public VocabularyController(IVocabularyService vocabularyService, ILogger<VocabularyController> logger)
        {
            _vocabularyService = vocabularyService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetVocabulary(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] DateTime? lastSync = null)
        {
            var userId = GetUserId();

            var result = await _vocabularyService.GetVocabularyAsync(
                userId, page, pageSize, lastSync);

            return Ok(new
            {
                success = true,
                data = result.Items,
                pagination = new
                {
                    page = result.Page,
                    pageSize = result.PageSize,
                    totalCount = result.TotalCount,
                    totalPages = result.TotalPages
                },
                lastSync = DateTime.UtcNow
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVocabularyById(int id)
        {
            var vocab = await _vocabularyService.GetByIdAsync(id);

            if (vocab == null)
                return NotFound(new ApiResponse { Success = false, Message = "Vocabulary not found" });

            return Ok(new { success = true, data = vocab });
        }

        [HttpPost]
        public async Task<IActionResult> CreateVocabulary([FromBody] CreateVocabularyDto dto)
        {
            var userId = GetUserId();

            var vocab = new Vocabulary
            {
                Japanese = dto.Japanese,
                Kana = dto.Kana,
                Romaji = dto.Romaji,
                Vietnamese = dto.Vietnamese,
                English = dto.English,
                Example = dto.Example,
                Notes = dto.Notes,
                Level = dto.Level,
                CreatedByUserID = userId,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _vocabularyService.CreateAsync(vocab);

            return CreatedAtAction(
                nameof(GetVocabularyById),
                new { id = created.VocabularyID },
                new { success = true, data = created });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVocabulary(int id, [FromBody] UpdateVocabularyDto dto)
        {
            var userId = GetUserId();

            var existing = await _vocabularyService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new ApiResponse { Success = false, Message = "Vocabulary not found" });

            // Basic update for Phase 1 (no row version check yet)
            existing.Japanese = dto.Japanese;
            existing.Kana = dto.Kana;
            existing.Vietnamese = dto.Vietnamese;
            existing.English = dto.English;
            existing.Example = dto.Example;
            existing.Notes = dto.Notes;
            existing.UpdatedByUserID = userId;
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _vocabularyService.UpdateAsync(existing);

            return Ok(new { success = true, data = updated });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVocabulary(int id)
        {
            var result = await _vocabularyService.DeleteAsync(id);

            if (!result)
                return NotFound(new ApiResponse { Success = false, Message = "Vocabulary not found" });

            return Ok(new ApiResponse { Success = true, Message = "Deleted successfully" });
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        }
    }
}
