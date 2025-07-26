using LexiFlow.API.Models.DTOs;
using LexiFlow.API.Models.Responses;
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

        [HttpGet]
        public async Task<IActionResult> GetVocabulary(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] DateTime? lastSync = null)
        {
            try
            {
                var userId = GetUserId();
                var result = await _vocabularyService.GetVocabularyAsync(userId, page, pageSize, lastSync);

                return Ok(new PagedResponse<Core.Entities.VocabularyItem>
                {
                    Success = true,
                    Message = "Vocabulary items retrieved successfully",
                    Items = result.Items,
                    Page = result.Page,
                    PageSize = result.PageSize,
                    TotalCount = result.TotalCount,
                    TotalPages = result.TotalPages
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vocabulary items");
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving vocabulary items"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVocabularyById(int id)
        {
            try
            {
                var vocab = await _vocabularyService.GetByIdAsync(id);

                if (vocab == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Vocabulary item not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Vocabulary item retrieved successfully",
                    Data = vocab
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vocabulary item with ID: {Id}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving the vocabulary item"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateVocabulary([FromBody] CreateVocabularyDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid vocabulary data",
                        Data = ModelState
                    });
                }

                var userId = GetUserId();
                var createdVocab = await _vocabularyService.CreateAsync(dto, userId);

                if (createdVocab == null)
                {
                    return StatusCode(500, new ApiResponse
                    {
                        Success = false,
                        Message = "Failed to create vocabulary item"
                    });
                }

                return CreatedAtAction(nameof(GetVocabularyById), new { id = createdVocab.Id }, new ApiResponse
                {
                    Success = true,
                    Message = "Vocabulary item created successfully",
                    Data = createdVocab
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vocabulary item");
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while creating the vocabulary item"
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVocabulary(int id, [FromBody] UpdateVocabularyDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid vocabulary data",
                        Data = ModelState
                    });
                }

                var userId = GetUserId();
                var updatedVocab = await _vocabularyService.UpdateAsync(id, dto, userId);

                if (updatedVocab == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Vocabulary item not found or could not be updated"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Vocabulary item updated successfully",
                    Data = updatedVocab
                });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Concurrency conflict when updating vocabulary item {Id}", id);
                return Conflict(new ApiResponse
                {
                    Success = false,
                    Message = "The vocabulary item has been modified by another user. Please refresh and try again."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vocabulary item with ID: {Id}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while updating the vocabulary item"
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVocabulary(int id)
        {
            try
            {
                var userId = GetUserId();
                var result = await _vocabularyService.DeleteAsync(id, userId);

                if (!result)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Vocabulary item not found or could not be deleted"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Vocabulary item deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vocabulary item with ID: {Id}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while deleting the vocabulary item"
                });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchVocabulary(
            [FromQuery] string query = "",
            [FromQuery] string language = "all",
            [FromQuery] int maxResults = 20)
        {
            try
            {
                var results = await _vocabularyService.SearchAsync(query, language, maxResults);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = $"Found {results.Count()} results",
                    Data = results
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching vocabulary with query: {Query}", query);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while searching for vocabulary items"
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