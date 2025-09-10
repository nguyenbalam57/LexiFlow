using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LexiFlow.API.DTOs.Common;
using LexiFlow.API.DTOs.Vocabulary;
using LexiFlow.API.Services.Vocabulary;
using System.Security.Claims;

namespace LexiFlow.API.Controllers
{
    /// <summary>
    /// Controller qu?n lý t? v?ng v?i database integration
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VocabularyController : ControllerBase
    {
        private readonly IVocabularyService _vocabularyService;
        private readonly ILogger<VocabularyController> _logger;

        public VocabularyController(
            IVocabularyService vocabularyService,
            ILogger<VocabularyController> logger)
        {
            _vocabularyService = vocabularyService;
            _logger = logger;
        }

        /// <summary>
        /// L?y danh sách t? v?ng có phân trang và tìm ki?m
        /// </summary>
        /// <param name="page">S? trang</param>
        /// <param name="pageSize">Kích th??c trang</param>
        /// <param name="searchTerm">T? khóa tìm ki?m</param>
        /// <param name="categoryId">ID danh m?c</param>
        /// <param name="level">C?p ?? JLPT</param>
        /// <param name="partOfSpeech">Lo?i t?</param>
        /// <param name="isActive">Tr?ng thái ho?t ??ng</param>
        [HttpGet]
        public async Task<ActionResult<PaginatedResultDto<VocabularyDto>>> GetVocabularies(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] int? categoryId = null,
            [FromQuery] string? level = null,
            [FromQuery] string? partOfSpeech = null,
            [FromQuery] bool? isActive = true)
        {
            try
            {
                var result = await _vocabularyService.GetVocabulariesAsync(
                    page, pageSize, searchTerm, categoryId, level, partOfSpeech, isActive);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabularies");
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// L?y chi ti?t t? v?ng theo ID
        /// </summary>
        /// <param name="id">ID t? v?ng</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<VocabularyDto>> GetVocabulary(int id)
        {
            try
            {
                var vocabulary = await _vocabularyService.GetVocabularyByIdAsync(id);

                if (vocabulary == null)
                {
                    return NotFound(new { error = "Vocabulary not found", id });
                }

                return Ok(vocabulary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabulary {VocabularyId}", id);
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// L?y t? v?ng theo term
        /// </summary>
        /// <param name="term">T? c?n tìm</param>
        /// <param name="languageCode">Mã ngôn ng?</param>
        [HttpGet("by-term/{term}")]
        public async Task<ActionResult<VocabularyDto>> GetVocabularyByTerm(
            string term, 
            [FromQuery] string languageCode = "ja")
        {
            try
            {
                var vocabulary = await _vocabularyService.GetVocabularyByTermAsync(term, languageCode);

                if (vocabulary == null)
                {
                    return NotFound(new { error = "Vocabulary not found", term, languageCode });
                }

                return Ok(vocabulary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabulary by term {Term}", term);
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// T?o t? v?ng m?i
        /// </summary>
        /// <param name="createDto">Thông tin t? v?ng</param>
        [HttpPost]
        public async Task<ActionResult<VocabularyDto>> CreateVocabulary([FromBody] CreateVocabularyDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();
                var newVocabulary = await _vocabularyService.CreateVocabularyAsync(createDto, userId);

                return CreatedAtAction(nameof(GetVocabulary), new { id = newVocabulary.VocabularyId }, newVocabulary);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vocabulary");
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// C?p nh?t t? v?ng
        /// </summary>
        /// <param name="id">ID t? v?ng</param>
        /// <param name="updateDto">Thông tin c?p nh?t</param>
        [HttpPut("{id}")]
        public async Task<ActionResult<VocabularyDto>> UpdateVocabulary(int id, [FromBody] UpdateVocabularyDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();
                var updatedVocabulary = await _vocabularyService.UpdateVocabularyAsync(id, updateDto, userId);

                if (updatedVocabulary == null)
                {
                    return NotFound(new { error = "Vocabulary not found", id });
                }

                return Ok(updatedVocabulary);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vocabulary {VocabularyId}", id);
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Xóa t? v?ng (soft delete)
        /// </summary>
        /// <param name="id">ID t? v?ng</param>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVocabulary(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var deleted = await _vocabularyService.DeleteVocabularyAsync(id, userId);

                if (!deleted)
                {
                    return NotFound(new { error = "Vocabulary not found", id });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vocabulary {VocabularyId}", id);
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// L?y t? v?ng theo category
        /// </summary>
        /// <param name="categoryId">ID category</param>
        [HttpGet("by-category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<VocabularyDto>>> GetVocabulariesByCategory(int categoryId)
        {
            try
            {
                var vocabularies = await _vocabularyService.GetVocabulariesByCategoryAsync(categoryId);
                return Ok(vocabularies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabularies by category {CategoryId}", categoryId);
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// L?y t? v?ng theo level
        /// </summary>
        /// <param name="level">JLPT level</param>
        [HttpGet("by-level/{level}")]
        public async Task<ActionResult<IEnumerable<VocabularyDto>>> GetVocabulariesByLevel(string level)
        {
            try
            {
                var vocabularies = await _vocabularyService.GetVocabulariesByLevelAsync(level);
                return Ok(vocabularies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabularies by level {Level}", level);
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// L?y t? v?ng ng?u nhiên
        /// </summary>
        /// <param name="count">S? l??ng t?</param>
        /// <param name="level">Level filter</param>
        [HttpGet("random")]
        public async Task<ActionResult<IEnumerable<VocabularyDto>>> GetRandomVocabularies(
            [FromQuery] int count = 10,
            [FromQuery] string? level = null)
        {
            try
            {
                var vocabularies = await _vocabularyService.GetRandomVocabulariesAsync(count, level);
                return Ok(vocabularies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting random vocabularies");
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// L?y t? v?ng ph? bi?n nh?t
        /// </summary>
        /// <param name="count">S? l??ng t?</param>
        [HttpGet("most-common")]
        public async Task<ActionResult<IEnumerable<VocabularyDto>>> GetMostCommonVocabularies(
            [FromQuery] int count = 10)
        {
            try
            {
                var vocabularies = await _vocabularyService.GetMostCommonVocabulariesAsync(count);
                return Ok(vocabularies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting most common vocabularies");
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// L?y t? v?ng m?i nh?t
        /// </summary>
        /// <param name="count">S? l??ng t?</param>
        [HttpGet("recent")]
        public async Task<ActionResult<IEnumerable<VocabularyDto>>> GetRecentVocabularies(
            [FromQuery] int count = 10)
        {
            try
            {
                var vocabularies = await _vocabularyService.GetRecentVocabulariesAsync(count);
                return Ok(vocabularies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent vocabularies");
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Ki?m tra t? v?ng ?ã t?n t?i
        /// </summary>
        /// <param name="term">T? c?n ki?m tra</param>
        /// <param name="languageCode">Mã ngôn ng?</param>
        /// <param name="excludeId">ID ?? lo?i tr?</param>
        [HttpGet("exists")]
        public async Task<ActionResult<bool>> VocabularyExists(
            [FromQuery] string term,
            [FromQuery] string languageCode = "ja",
            [FromQuery] int? excludeId = null)
        {
            try
            {
                var exists = await _vocabularyService.VocabularyExistsAsync(term, languageCode, excludeId);
                return Ok(new { exists, term, languageCode });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking vocabulary exists for term {Term}", term);
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// L?y th?ng kê t? v?ng
        /// </summary>
        [HttpGet("statistics")]
        public async Task<ActionResult<LexiFlow.API.Services.Vocabulary.VocabularyStatisticsDto>> GetVocabularyStatistics()
        {
            try
            {
                var statistics = await _vocabularyService.GetVocabularyStatisticsAsync();
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabulary statistics");
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// L?y ID user hi?n t?i t? JWT token
        /// </summary>
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }

            // Fallback cho development - có th? remove trong production
            _logger.LogWarning("Unable to get user ID from claims, using default user ID 1");
            return 1;
        }
    }
}