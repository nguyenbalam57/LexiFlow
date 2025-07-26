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
    [Produces("application/json")]
    [SwaggerTag("Manage categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(
            ICategoryService categoryService,
            ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        /// <summary>
        /// Get all categories with optional filtering
        /// </summary>
        /// <param name="includeInactive">Whether to include inactive categories</param>
        /// <param name="level">Filter by level</param>
        /// <returns>List of categories</returns>
        /// <response code="200">Returns the list of categories</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all categories",
            Description = "Retrieves all categories with optional filtering",
            OperationId = "GetCategories",
            Tags = new[] { "Categories" }
        )]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategories(
            [FromQuery] bool includeInactive = false,
            [FromQuery] string? level = null)
        {
            try
            {
                var categories = await _categoryService.GetAllAsync(includeInactive, level);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Categories retrieved successfully",
                    Data = categories
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categories");
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving categories"
                });
            }
        }

        /// <summary>
        /// Get a category by ID
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Category details</returns>
        /// <response code="200">Returns the category</response>
        /// <response code="404">Category not found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get category by ID",
            Description = "Retrieves a specific category by its ID",
            OperationId = "GetCategoryById",
            Tags = new[] { "Categories" }
        )]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(id);

                if (category == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Category not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Category retrieved successfully",
                    Data = category
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category with ID: {Id}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving the category"
                });
            }
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        /// <param name="dto">Category data</param>
        /// <returns>Created category</returns>
        /// <response code="201">Category created successfully</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Authorize(Roles = "Administrator,Teacher")]
        [SwaggerOperation(
            Summary = "Create a new category",
            Description = "Creates a new category with the provided data",
            OperationId = "CreateCategory",
            Tags = new[] { "Categories" }
        )]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid category data",
                        Data = ModelState
                    });
                }

                var userId = GetUserId();
                var createdCategory = await _categoryService.CreateAsync(dto, userId);

                if (createdCategory == null)
                {
                    return StatusCode(500, new ApiResponse
                    {
                        Success = false,
                        Message = "Failed to create category"
                    });
                }

                return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.CategoryID }, new ApiResponse
                {
                    Success = true,
                    Message = "Category created successfully",
                    Data = createdCategory
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category");
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while creating the category"
                });
            }
        }

        /// <summary>
        /// Update an existing category
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <param name="dto">Updated category data</param>
        /// <returns>Updated category</returns>
        /// <response code="200">Category updated successfully</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="404">Category not found</response>
        /// <response code="409">Concurrency conflict</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator,Teacher")]
        [SwaggerOperation(
            Summary = "Update an existing category",
            Description = "Updates an existing category with the provided data",
            OperationId = "UpdateCategory",
            Tags = new[] { "Categories" }
        )]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid category data",
                        Data = ModelState
                    });
                }

                var userId = GetUserId();
                var updatedCategory = await _categoryService.UpdateAsync(id, dto, userId);

                if (updatedCategory == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Category not found or could not be updated"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Category updated successfully",
                    Data = updatedCategory
                });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Concurrency conflict when updating category {Id}", id);
                return Conflict(new ApiResponse
                {
                    Success = false,
                    Message = "The category has been modified by another user. Please refresh and try again."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category with ID: {Id}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while updating the category"
                });
            }
        }

        /// <summary>
        /// Delete a category
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Success indicator</returns>
        /// <response code="200">Category deleted successfully</response>
        /// <response code="404">Category not found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        [SwaggerOperation(
            Summary = "Delete a category",
            Description = "Deletes a category with the specified ID",
            OperationId = "DeleteCategory",
            Tags = new[] { "Categories" }
        )]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var userId = GetUserId();
                var result = await _categoryService.DeleteAsync(id, userId);

                if (!result)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Category not found or could not be deleted"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Category deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category with ID: {Id}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while deleting the category"
                });
            }
        }

        /// <summary>
        /// Get vocabulary groups by category ID
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <param name="includeInactive">Whether to include inactive groups</param>
        /// <returns>List of vocabulary groups in the category</returns>
        /// <response code="200">Returns the list of vocabulary groups</response>
        /// <response code="404">Category not found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}/groups")]
        [SwaggerOperation(
            Summary = "Get vocabulary groups by category",
            Description = "Retrieves all vocabulary groups associated with a specific category",
            OperationId = "GetVocabularyGroupsByCategory",
            Tags = new[] { "Categories" }
        )]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetVocabularyGroupsByCategory(
            int id,
            [FromQuery] bool includeInactive = false)
        {
            try
            {
                // Check if category exists
                var category = await _categoryService.GetByIdAsync(id);
                if (category == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Category not found"
                    });
                }

                var groups = await _categoryService.GetGroupsByCategoryAsync(id, includeInactive);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Vocabulary groups retrieved successfully",
                    Data = groups
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vocabulary groups for category ID: {Id}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving vocabulary groups"
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