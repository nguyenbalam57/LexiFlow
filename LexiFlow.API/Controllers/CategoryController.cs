using LexiFlow.API.Data;
using LexiFlow.API.Models.Responses;
using LexiFlow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LexiFlow.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(
            ApplicationDbContext dbContext,
            ILogger<CategoryController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] int? parentId = null)
        {
            try
            {
                var query = _dbContext.Categories.AsQueryable();

                if (parentId.HasValue)
                {
                    query = query.Where(c => c.ParentId == parentId);
                }
                else
                {
                    query = query.Where(c => c.ParentId == null);
                }

                var categories = await query
                    .Where(c => !c.IsDeleted && c.Status == "Active")
                    .OrderBy(c => c.SortOrder)
                    .ThenBy(c => c.Name)
                    .ToListAsync();

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _dbContext.Categories
                    .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto dto)
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

                // Check for duplicate name in same parent
                var exists = await _dbContext.Categories
                    .AnyAsync(c => c.Name == dto.Name && c.ParentId == dto.ParentId && !c.IsDeleted);

                if (exists)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "A category with this name already exists at this level"
                    });
                }

                var userId = GetUserId();
                var category = new Category
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    ParentId = dto.ParentId,
                    Status = dto.Status ?? "Active",
                    SortOrder = dto.SortOrder ?? 0,
                    CreatedBy = userId,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.Categories.Add(category);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, new ApiResponse
                {
                    Success = true,
                    Message = "Category created successfully",
                    Data = category
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

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto dto)
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

                var category = await _dbContext.Categories.FindAsync(id);
                if (category == null || category.IsDeleted)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Category not found"
                    });
                }

                // Check for duplicate name in same parent
                var exists = await _dbContext.Categories
                    .AnyAsync(c => c.Name == dto.Name && c.ParentId == dto.ParentId && c.Id != id && !c.IsDeleted);

                if (exists)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "A category with this name already exists at this level"
                    });
                }

                var userId = GetUserId();
                category.Name = dto.Name;
                category.Description = dto.Description;
                category.ParentId = dto.ParentId;
                category.Status = dto.Status ?? category.Status;
                category.SortOrder = dto.SortOrder ?? category.SortOrder;
                category.ModifiedBy = userId;
                category.ModifiedAt = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Category updated successfully",
                    Data = category
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _dbContext.Categories.FindAsync(id);
                if (category == null || category.IsDeleted)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Category not found"
                    });
                }

                // Check if category has child categories
                var hasChildren = await _dbContext.Categories
                    .AnyAsync(c => c.ParentId == id && !c.IsDeleted);

                if (hasChildren)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Cannot delete category with child categories. Remove or reassign them first."
                    });
                }

                // Check if category has vocabulary items
                var hasVocabularyItems = await _dbContext.VocabularyItems
                    .AnyAsync(v => v.CategoryId == id && !v.IsDeleted);

                if (hasVocabularyItems)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Cannot delete category with vocabulary items. Remove or reassign them first."
                    });
                }

                var userId = GetUserId();
                category.IsDeleted = true;
                category.DeletedBy = userId;
                category.DeletedAt = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();

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

    public class CategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public string? Status { get; set; }
        public int? SortOrder { get; set; }
    }
}