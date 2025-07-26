using LexiFlow.API.DTOs.Kanji;
using LexiFlow.API.Models;
using LexiFlow.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LexiFlow.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class KanjiController : ControllerBase
    {
        private readonly IKanjiService _kanjiService;
        private readonly ILogger<KanjiController> _logger;

        public KanjiController(
            IKanjiService kanjiService,
            ILogger<KanjiController> logger)
        {
            _kanjiService = kanjiService;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách tất cả Kanji
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllKanji(
            [FromQuery] string jlptLevel = null,
            [FromQuery] int? grade = null)
        {
            try
            {
                var kanji = await _kanjiService.GetAllAsync(jlptLevel, grade);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Kanji list retrieved successfully",
                    Data = kanji
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving kanji list");
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving kanji list"
                });
            }
        }

        /// <summary>
        /// Lấy thông tin Kanji theo ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetKanjiById(int id)
        {
            try
            {
                var kanji = await _kanjiService.GetByIdAsync(id);

                if (kanji == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Kanji not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Kanji retrieved successfully",
                    Data = kanji
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving kanji with ID: {Id}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving kanji"
                });
            }
        }

        /// <summary>
        /// Lấy thông tin Kanji theo ký tự
        /// </summary>
        [HttpGet("character/{character}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetKanjiByCharacter(string character)
        {
            try
            {
                var kanji = await _kanjiService.GetByCharacterAsync(character);

                if (kanji == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Kanji not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Kanji retrieved successfully",
                    Data = kanji
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving kanji with character: {Character}", character);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving kanji"
                });
            }
        }

        /// <summary>
        /// Tạo mới Kanji
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateKanji([FromBody] CreateKanjiDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid kanji data",
                        Data = ModelState
                    });
                }

                var userId = GetUserId();
                var createdKanji = await _kanjiService.CreateAsync(dto, userId);

                return CreatedAtAction(nameof(GetKanjiById), new { id = createdKanji.KanjiID }, new ApiResponse
                {
                    Success = true,
                    Message = "Kanji created successfully",
                    Data = createdKanji
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating kanji: {Character}", dto.Character);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while creating kanji"
                });
            }
        }

        /// <summary>
        /// Cập nhật Kanji
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateKanji(int id, [FromBody] UpdateKanjiDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid kanji data",
                        Data = ModelState
                    });
                }

                var userId = GetUserId();
                var updatedKanji = await _kanjiService.UpdateAsync(id, dto, userId);

                if (updatedKanji == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Kanji not found or could not be updated"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Kanji updated successfully",
                    Data = updatedKanji
                });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Concurrency conflict when updating kanji {Id}", id);
                return Conflict(new ApiResponse
                {
                    Success = false,
                    Message = "The kanji has been modified by another user. Please refresh and try again."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating kanji with ID: {Id}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while updating kanji"
                });
            }
        }

        /// <summary>
        /// Xóa Kanji
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteKanji(int id)
        {
            try
            {
                var userId = GetUserId();
                var result = await _kanjiService.DeleteAsync(id, userId);

                if (!result)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Kanji not found or could not be deleted"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Kanji deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting kanji with ID: {Id}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while deleting kanji"
                });
            }
        }

        /// <summary>
        /// Tìm kiếm Kanji
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchKanji([FromQuery] KanjiSearchRequestDto searchRequest)
        {
            try
            {
                var results = await _kanjiService.SearchAsync(searchRequest);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = $"Found {results.Count()} results",
                    Data = results
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching kanji with query: {Query}", searchRequest.Query);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while searching for kanji"
                });
            }
        }

        /// <summary>
        /// Lấy tiến độ học Kanji của người dùng
        /// </summary>
        [HttpGet("progress/{kanjiId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserProgress(int kanjiId)
        {
            try
            {
                var userId = GetUserId();
                var progress = await _kanjiService.GetUserProgressAsync(userId, kanjiId);

                if (progress == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Progress not found for the specified kanji"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Kanji progress retrieved successfully",
                    Data = progress
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving kanji progress for kanji ID: {KanjiId}, user ID: {UserId}",
                    kanjiId, GetUserId());
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving kanji progress"
                });
            }
        }

        /// <summary>
        /// Lấy danh sách tiến độ học Kanji của người dùng
        /// </summary>
        [HttpGet("progress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserProgressList(
            [FromQuery] string jlptLevel = null,
            [FromQuery] int? grade = null)
        {
            try
            {
                var userId = GetUserId();
                var progressList = await _kanjiService.GetUserProgressListAsync(userId, jlptLevel, grade);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Kanji progress list retrieved successfully",
                    Data = progressList
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving kanji progress list for user ID: {UserId}", GetUserId());
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving kanji progress list"
                });
            }
        }

        /// <summary>
        /// Cập nhật tiến độ học Kanji
        /// </summary>
        [HttpPost("progress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProgress([FromBody] UpdateKanjiProgressDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid progress data",
                        Data = ModelState
                    });
                }

                var userId = GetUserId();
                var progress = await _kanjiService.UpdateProgressAsync(userId, dto);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Kanji progress updated successfully",
                    Data = progress
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating kanji progress for kanji ID: {KanjiId}, user ID: {UserId}",
                    dto.KanjiID, GetUserId());
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while updating kanji progress"
                });
            }
        }

        /// <summary>
        /// Lấy danh sách Kanji cần ôn tập
        /// </summary>
        [HttpGet("review")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReviewList(
            [FromQuery] int count = 20,
            [FromQuery] bool includeNew = true)
        {
            try
            {
                var userId = GetUserId();
                var reviewList = await _kanjiService.GetReviewListAsync(userId, count, includeNew);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Kanji review list retrieved successfully",
                    Data = reviewList
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving kanji review list for user ID: {UserId}", GetUserId());
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving kanji review list"
                });
            }
        }

        /// <summary>
        /// Lấy danh sách thành phần của Kanji
        /// </summary>
        [HttpGet("{id}/components")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetKanjiComponents(int id)
        {
            try
            {
                var components = await _kanjiService.GetComponentsAsync(id);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Kanji components retrieved successfully",
                    Data = components
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving components for kanji ID: {KanjiId}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving kanji components"
                });
            }
        }

        /// <summary>
        /// Lấy danh sách từ vựng liên quan đến Kanji
        /// </summary>
        [HttpGet("{id}/vocabulary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRelatedVocabulary(int id)
        {
            try
            {
                var vocabulary = await _kanjiService.GetRelatedVocabularyAsync(id);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Related vocabulary retrieved successfully",
                    Data = vocabulary
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving related vocabulary for kanji ID: {KanjiId}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving related vocabulary"
                });
            }
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("Invalid user ID in token");
        }
    }
}
