using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LexiFlow.API.DTOs.Common;
using LexiFlow.API.DTOs.Vocabulary;
using System.Security.Claims;

namespace LexiFlow.API.Controllers
{
    /// <summary>
    /// Controller qu?n lý t? v?ng
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VocabularyController : ControllerBase
    {
        private readonly ILogger<VocabularyController> _logger;

        public VocabularyController(ILogger<VocabularyController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// L?y danh sách t? v?ng
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PaginatedResultDto<VocabularyDto>>> GetVocabularies(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var vocabularies = new List<VocabularyDto>
                {
                    new VocabularyDto
                    {
                        VocabularyId = 1,
                        Word = "?????",
                        Meaning = "Hello",
                        Level = "N5"
                    }
                };

                var result = new PaginatedResultDto<VocabularyDto>
                {
                    Data = vocabularies,
                    TotalCount = vocabularies.Count,
                    PageNumber = page,
                    PageSize = pageSize
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabularies");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y chi ti?t t? v?ng
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<VocabularyDto>> GetVocabulary(int id)
        {
            try
            {
                var vocabulary = new VocabularyDto
                {
                    VocabularyId = id,
                    Word = "?????",
                    Meaning = "Hello",
                    Level = "N5"
                };

                return Ok(vocabulary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabulary {VocabularyId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// T?o t? v?ng m?i
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<VocabularyDto>> CreateVocabulary([FromBody] CreateVocabularyDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newVocabulary = new VocabularyDto
                {
                    VocabularyId = new Random().Next(100, 999),
                    Word = createDto.Word,
                    Meaning = createDto.Meaning,
                    Level = createDto.Level
                };

                return CreatedAtAction(nameof(GetVocabulary), new { id = newVocabulary.VocabularyId }, newVocabulary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vocabulary");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// C?p nh?t t? v?ng
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<VocabularyDto>> UpdateVocabulary(int id, [FromBody] UpdateVocabularyDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedVocabulary = new VocabularyDto
                {
                    VocabularyId = id,
                    Word = updateDto.Word ?? "Updated Word",
                    Meaning = updateDto.Meaning ?? "Updated meaning",
                    Level = updateDto.Level ?? "N5"
                };

                return Ok(updatedVocabulary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vocabulary {VocabularyId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Xóa t? v?ng
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVocabulary(int id)
        {
            try
            {
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vocabulary {VocabularyId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return 1;
        }
    }
}