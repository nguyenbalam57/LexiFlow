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
    public class VocabularyGroupController : ControllerBase
    {
        private readonly IVocabularyGroupService _vocabularyGroupService;
        private readonly ILogger<VocabularyGroupController> _logger;

        public VocabularyGroupController(
            IVocabularyGroupService vocabularyGroupService,
            ILogger<VocabularyGroupController> logger)
        {
            _vocabularyGroupService = vocabularyGroupService;
            _logger = logger;
        }

        /// <summary>
        /// Get all vocabulary groups with optional filtering
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetVocabularyGroups(
            [FromQuery] bool includeInactive = false,
            [FromQuery] int? categoryId = null)
        {
            try
            {
                var groups = await _vocabularyGroupService.GetAllAsync(includeInactive, categoryId);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Vocabulary groups retrieved successfully",
                    Data = groups
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vocabulary groups");
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving vocabulary groups"
                });
            }
        }

        /// <summary>
        /// Get a vocabulary group by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVocabularyGroupById(int id)
        {
            try
            {
                var group = await _vocabularyGroupService.GetByIdAsync(id);

                if (group == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Vocabulary group not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Vocabulary group retrieved successfully",
                    Data = group
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vocabulary group with ID: {Id}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving the vocabulary group"
                });
            }
        }

        /// <summary>
        /// Create a new vocabulary group
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateVocabularyGroup([FromBody] CreateVocabularyGroupDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid vocabulary group data",
                        Data = ModelState
                    });
                }

                var userId = GetUserId();
                var createdGroup = await _vocabularyGroupService.CreateAsync(dto, userId);

                if (createdGroup == null)
                {
                    return StatusCode(500, new ApiResponse
                    {
                        Success = false,
                        Message = "Failed to create vocabulary group"
                    });
                }

                return CreatedAtAction(nameof(GetVocabularyGroupById), new { id = createdGroup.GroupID }, new ApiResponse
                {
                    Success = true,
                    Message = "Vocabulary group created successfully",
                    Data = createdGroup
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vocabulary group");
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while creating the vocabulary group"
                });
            }
        }

        /// <summary>
        /// Update an existing vocabulary group
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVocabularyGroup(int id, [FromBody] UpdateVocabularyGroupDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid vocabulary group data",
                        Data = ModelState
                    });
                }

                var userId = GetUserId();
                var updatedGroup = await _vocabularyGroupService.UpdateAsync(id, dto, userId);

                if (updatedGroup == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Vocabulary group not found or could not be updated"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Vocabulary group updated successfully",
                    Data = updatedGroup
                });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Concurrency conflict when updating vocabulary group {Id}", id);
                return Conflict(new ApiResponse
                {
                    Success = false,
                    Message = "The vocabulary group has been modified by another user. Please refresh and try again."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vocabulary group with ID: {Id}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while updating the vocabulary group"
                });
            }
        }

        /// <summary>
        /// Delete a vocabulary group
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVocabularyGroup(int id)
        {
            try
            {
                var userId = GetUserId();
                var result = await _vocabularyGroupService.DeleteAsync(id, userId);

                if (!result)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Vocabulary group not found or could not be deleted"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Vocabulary group deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vocabulary group with ID: {Id}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while deleting the vocabulary group"
                });
            }
        }

        /// <summary>
        /// Get vocabularies by group ID
        /// </summary>
        [HttpGet("{id}/vocabularies")]
        public async Task<IActionResult> GetVocabulariesByGroup(
            int id,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            try
            {
                var result = await _vocabularyGroupService.GetVocabulariesByGroupAsync(id, page, pageSize);

                if (result == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Vocabulary group not found"
                    });
                }

                return Ok(new PagedResponse<Vocabulary>
                {
                    Success = true,
                    Message = "Vocabularies retrieved successfully",
                    Items = result.Items,
                    Page = result.PageIndex,
                    PageSize = result.PageSize,
                    TotalCount = result.TotalCount,
                    TotalPages = result.TotalPages
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vocabularies for group ID: {Id}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving vocabularies"
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