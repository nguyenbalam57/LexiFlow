using LexiFlow.API.DTOs.Vocabulary;
using LexiFlow.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LexiFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        /// <summary>
        /// Get paginated list of vocabularies with optional filtering
        /// </summary>
        /// <param name="pageIndex">Page index (0-based)</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="search">Search term for Japanese, Kana, Romaji, Vietnamese, or English</param>
        /// <param name="level">JLPT level filter</param>
        /// <param name="groupId">Group ID filter</param>
        /// <returns>Paginated list of vocabularies</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetVocabularies(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? level = null,
            [FromQuery] int? groupId = null)
        {
            try
            {
                var response = await _vocabularyService.GetVocabulariesAsync(pageIndex, pageSize, search, level, groupId);
                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vocabularies");
                return BadRequest(new { Success = false, Message = "Failed to retrieve vocabularies", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get vocabulary by ID
        /// </summary>
        /// <param name="id">Vocabulary ID</param>
        /// <returns>Vocabulary details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetVocabularyById(int id)
        {
            try
            {
                var response = await _vocabularyService.GetVocabularyByIdAsync(id);
                if (!response.Success)
                {
                    return NotFound(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vocabulary with ID {VocabularyId}", id);
                return BadRequest(new { Success = false, Message = $"Failed to retrieve vocabulary with ID {id}", Error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new vocabulary
        /// </summary>
        /// <param name="createDto">Vocabulary creation details</param>
        /// <returns>Created vocabulary</returns>
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateVocabulary([FromBody] CreateVocabularyDto createDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _vocabularyService.CreateVocabularyAsync(createDto, userId);
                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return CreatedAtAction(nameof(GetVocabularyById), new { id = response.Data?.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vocabulary");
                return BadRequest(new { Success = false, Message = "Failed to create vocabulary", Error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing vocabulary
        /// </summary>
        /// <param name="id">Vocabulary ID</param>
        /// <param name="updateDto">Vocabulary update details</param>
        /// <returns>Updated vocabulary</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateVocabulary(int id, [FromBody] UpdateVocabularyDto updateDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _vocabularyService.UpdateVocabularyAsync(id, updateDto, userId);
                if (!response.Success)
                {
                    return response.Message == "Vocabulary not found" ? NotFound(response) : BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vocabulary with ID {VocabularyId}", id);
                return BadRequest(new { Success = false, Message = $"Failed to update vocabulary with ID {id}", Error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a vocabulary
        /// </summary>
        /// <param name="id">Vocabulary ID</param>
        /// <returns>Success message</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVocabulary(int id)
        {
            try
            {
                var response = await _vocabularyService.DeleteVocabularyAsync(id);
                if (!response.Success)
                {
                    return response.Message == "Vocabulary not found" ? NotFound(response) : BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vocabulary with ID {VocabularyId}", id);
                return BadRequest(new { Success = false, Message = $"Failed to delete vocabulary with ID {id}", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get paginated list of vocabulary groups with optional filtering
        /// </summary>
        /// <param name="pageIndex">Page index (0-based)</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="search">Search term for group name or description</param>
        /// <returns>Paginated list of vocabulary groups</returns>
        [HttpGet("groups")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetVocabularyGroups(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            try
            {
                var response = await _vocabularyService.GetVocabularyGroupsAsync(pageIndex, pageSize, search);
                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vocabulary groups");
                return BadRequest(new { Success = false, Message = "Failed to retrieve vocabulary groups", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get vocabulary group by ID
        /// </summary>
        /// <param name="id">Group ID</param>
        /// <returns>Vocabulary group details</returns>
        [HttpGet("groups/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetVocabularyGroupById(int id)
        {
            try
            {
                var response = await _vocabularyService.GetVocabularyGroupByIdAsync(id);
                if (!response.Success)
                {
                    return NotFound(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vocabulary group with ID {GroupId}", id);
                return BadRequest(new { Success = false, Message = $"Failed to retrieve vocabulary group with ID {id}", Error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new vocabulary group
        /// </summary>
        /// <param name="createDto">Group creation details</param>
        /// <returns>Created vocabulary group</returns>
        [HttpPost("groups")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateVocabularyGroup([FromBody] CreateVocabularyGroupDto createDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _vocabularyService.CreateVocabularyGroupAsync(createDto, userId);
                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return CreatedAtAction(nameof(GetVocabularyGroupById), new { id = response.Data?.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vocabulary group");
                return BadRequest(new { Success = false, Message = "Failed to create vocabulary group", Error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing vocabulary group
        /// </summary>
        /// <param name="id">Group ID</param>
        /// <param name="updateDto">Group update details</param>
        /// <returns>Updated vocabulary group</returns>
        [HttpPut("groups/{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateVocabularyGroup(int id, [FromBody] UpdateVocabularyGroupDto updateDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _vocabularyService.UpdateVocabularyGroupAsync(id, updateDto, userId);
                if (!response.Success)
                {
                    return response.Message == "Vocabulary group not found" ? NotFound(response) : BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vocabulary group with ID {GroupId}", id);
                return BadRequest(new { Success = false, Message = $"Failed to update vocabulary group with ID {id}", Error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a vocabulary group
        /// </summary>
        /// <param name="id">Group ID</param>
        /// <returns>Success message</returns>
        [HttpDelete("groups/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVocabularyGroup(int id)
        {
            try
            {
                var response = await _vocabularyService.DeleteVocabularyGroupAsync(id);
                if (!response.Success)
                {
                    return response.Message == "Vocabulary group not found" ? NotFound(response) : BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vocabulary group with ID {GroupId}", id);
                return BadRequest(new { Success = false, Message = $"Failed to delete vocabulary group with ID {id}", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get paginated list of kanji with optional filtering
        /// </summary>
        /// <param name="pageIndex">Page index (0-based)</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="search">Search term for character, readings, or meaning</param>
        /// <param name="level">JLPT level filter</param>
        /// <returns>Paginated list of kanji</returns>
        [HttpGet("kanji")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetKanjis(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? level = null)
        {
            try
            {
                var response = await _vocabularyService.GetKanjisAsync(pageIndex, pageSize, search, level);
                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving kanji list");
                return BadRequest(new { Success = false, Message = "Failed to retrieve kanji list", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get kanji by ID
        /// </summary>
        /// <param name="id">Kanji ID</param>
        /// <returns>Kanji details</returns>
        [HttpGet("kanji/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetKanjiById(int id)
        {
            try
            {
                var response = await _vocabularyService.GetKanjiByIdAsync(id);
                if (!response.Success)
                {
                    return NotFound(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving kanji with ID {KanjiId}", id);
                return BadRequest(new { Success = false, Message = $"Failed to retrieve kanji with ID {id}", Error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new kanji
        /// </summary>
        /// <param name="createDto">Kanji creation details</param>
        /// <returns>Created kanji</returns>
        [HttpPost("kanji")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateKanji([FromBody] CreateKanjiDto createDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _vocabularyService.CreateKanjiAsync(createDto, userId);
                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return CreatedAtAction(nameof(GetKanjiById), new { id = response.Data?.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating kanji");
                return BadRequest(new { Success = false, Message = "Failed to create kanji", Error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing kanji
        /// </summary>
        /// <param name="id">Kanji ID</param>
        /// <param name="updateDto">Kanji update details</param>
        /// <returns>Updated kanji</returns>
        [HttpPut("kanji/{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateKanji(int id, [FromBody] CreateKanjiDto updateDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _vocabularyService.UpdateKanjiAsync(id, updateDto, userId);
                if (!response.Success)
                {
                    return response.Message == "Kanji not found" ? NotFound(response) : BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating kanji with ID {KanjiId}", id);
                return BadRequest(new { Success = false, Message = $"Failed to update kanji with ID {id}", Error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a kanji
        /// </summary>
        /// <param name="id">Kanji ID</param>
        /// <returns>Success message</returns>
        [HttpDelete("kanji/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteKanji(int id)
        {
            try
            {
                var response = await _vocabularyService.DeleteKanjiAsync(id);
                if (!response.Success)
                {
                    return response.Message == "Kanji not found" ? NotFound(response) : BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting kanji with ID {KanjiId}", id);
                return BadRequest(new { Success = false, Message = $"Failed to delete kanji with ID {id}", Error = ex.Message });
            }
        }
    }
}