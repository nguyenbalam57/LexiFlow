using LexiFlow.API.DTOs.Grammar;
using LexiFlow.API.Models;
using LexiFlow.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LexiFlow.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class GrammarController : ControllerBase
    {
        private readonly IGrammarService _grammarService;
        private readonly ILogger<GrammarController> _logger;

        public GrammarController(
            IGrammarService grammarService,
            ILogger<GrammarController> logger)
        {
            _grammarService = grammarService;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách tất cả điểm ngữ pháp
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllGrammar(
            [FromQuery] string jlptLevel = null,
            [FromQuery] int? categoryId = null)
        {
            try
            {
                var grammar = await _grammarService.GetAllAsync(jlptLevel, categoryId);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Grammar list retrieved successfully",
                    Data = grammar
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving grammar list");
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving grammar list"
                });
            }
        }

        /// <summary>
        /// Lấy thông tin điểm ngữ pháp theo ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGrammarById(int id)
        {
            try
            {
                var grammar = await _grammarService.GetByIdAsync(id);

                if (grammar == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Grammar not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Grammar retrieved successfully",
                    Data = grammar
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving grammar with ID: {Id}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving grammar"
                });
            }
        }

        /// <summary>
        /// Tạo mới điểm ngữ pháp
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateGrammar([FromBody] CreateGrammarDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid grammar data",
                        Data = ModelState
                    });
                }

                var userId = GetUserId();
                var createdGrammar = await _grammarService.CreateAsync(dto, userId);

                return CreatedAtAction(nameof(GetGrammarById), new { id = createdGrammar.GrammarID }, new ApiResponse
                {
                    Success = true,
                    Message = "Grammar created successfully",
                    Data = createdGrammar
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating grammar: {GrammarPoint}", dto.GrammarPoint);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while creating grammar"
                });
            }
        }

        /// <summary>
        /// Cập nhật điểm ngữ pháp
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateGrammar(int id, [FromBody] UpdateGrammarDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid grammar data",
                        Data = ModelState
                    });
                }

                var userId = GetUserId();
                var updatedGrammar = await _grammarService.UpdateAsync(id, dto, userId);

                if (updatedGrammar == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Grammar not found or could not be updated"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Grammar updated successfully",
                    Data = updatedGrammar
                });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Concurrency conflict when updating grammar {Id}", id);
                return Conflict(new ApiResponse
                {
                    Success = false,
                    Message = "The grammar has been modified by another user. Please refresh and try again."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating grammar with ID: {Id}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while updating grammar"
                });
            }
        }

        /// <summary>
        /// Xóa điểm ngữ pháp
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteGrammar(int id)
        {
            try
            {
                var userId = GetUserId();
                var result = await _grammarService.DeleteAsync(id, userId);

                if (!result)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Grammar not found or could not be deleted"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Grammar deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting grammar with ID: {Id}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while deleting grammar"
                });
            }
        }

        /// <summary>
        /// Tìm kiếm điểm ngữ pháp
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchGrammar([FromQuery] GrammarSearchRequestDto searchRequest)
        {
            try
            {
                var results = await _grammarService.SearchAsync(searchRequest);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = $"Found {results.Count()} results",
                    Data = results
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching grammar with query: {Query}", searchRequest.Query);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while searching for grammar"
                });
            }
        }

        /// <summary>
        /// Thêm ví dụ cho điểm ngữ pháp
        /// </summary>
        [HttpPost("{grammarId}/examples")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddExample(int grammarId, [FromBody] CreateGrammarExampleDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid example data",
                        Data = ModelState
                    });
                }

                var userId = GetUserId();
                var example = await _grammarService.AddExampleAsync(grammarId, dto, userId);

                if (example == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Grammar not found or example could not be added"
                    });
                }

                return CreatedAtAction(nameof(GetGrammarById), new { id = grammarId }, new ApiResponse
                {
                    Success = true,
                    Message = "Example added successfully",
                    Data = example
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding example to grammar ID: {GrammarId}", grammarId);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while adding example"
                });
            }
        }

        /// <summary>
        /// Xóa ví dụ của điểm ngữ pháp
        /// </summary>
        [HttpDelete("examples/{exampleId}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteExample(int exampleId)
        {
            try
            {
                var userId = GetUserId();
                var result = await _grammarService.DeleteExampleAsync(exampleId, userId);

                if (!result)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Example not found or could not be deleted"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Example deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting grammar example with ID: {ExampleId}", exampleId);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while deleting example"
                });
            }
        }

        /// <summary>
        /// Lấy tiến độ học ngữ pháp của người dùng
        /// </summary>
        [HttpGet("progress/{grammarId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserProgress(int grammarId)
        {
            try
            {
                var userId = GetUserId();
                var progress = await _grammarService.GetUserProgressAsync(userId, grammarId);

                if (progress == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Progress not found for the specified grammar"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Grammar progress retrieved successfully",
                    Data = progress
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving grammar progress for grammar ID: {GrammarId}, user ID: {UserId}",
                    grammarId, GetUserId());
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving grammar progress"
                });
            }
        }

        /// <summary>
        /// Lấy danh sách tiến độ học ngữ pháp của người dùng
        /// </summary>
        [HttpGet("progress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserProgressList([FromQuery] string jlptLevel = null)
        {
            try
            {
                var userId = GetUserId();
                var progressList = await _grammarService.GetUserProgressListAsync(userId, jlptLevel);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Grammar progress list retrieved successfully",
                    Data = progressList
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving grammar progress list for user ID: {UserId}", GetUserId());
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving grammar progress list"
                });
            }
        }

        /// <summary>
        /// Cập nhật tiến độ học ngữ pháp
        /// </summary>
        [HttpPost("progress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProgress([FromBody] UpdateGrammarProgressDto dto)
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
                var progress = await _grammarService.UpdateProgressAsync(userId, dto);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Grammar progress updated successfully",
                    Data = progress
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating grammar progress for grammar ID: {GrammarId}, user ID: {UserId}",
                    dto.GrammarID, GetUserId());
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while updating grammar progress"
                });
            }
        }

        /// <summary>
        /// Lấy danh sách điểm ngữ pháp cần ôn tập
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
                var reviewList = await _grammarService.GetReviewListAsync(userId, count, includeNew);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Grammar review list retrieved successfully",
                    Data = reviewList
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving grammar review list for user ID: {UserId}", GetUserId());
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving grammar review list"
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
