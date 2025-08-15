using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LexiFlow.API.DTOs.Common;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.Controllers
{
    /// <summary>
    /// Controller qu?n lý danh m?c
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ILogger<CategoriesController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// L?y danh sách danh m?c v?i phân trang
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PaginatedResultDto<CategoryDto>>> GetCategories(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? type = null,
            [FromQuery] bool? isActive = null,
            [FromQuery] string? sortBy = "Name",
            [FromQuery] string? sortDirection = "asc")
        {
            try
            {
                // Mock data
                var categories = new List<CategoryDto>
                {
                    new CategoryDto
                    {
                        CategoryId = 1,
                        Name = "Greetings",
                        NameJapanese = "??",
                        Description = "Basic greetings and polite expressions",
                        Type = "Vocabulary",
                        Icon = "??",
                        Color = "#FF6B6B",
                        ParentCategoryId = null,
                        Level = "Beginner",
                        DisplayOrder = 1,
                        ItemCount = 25,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-30),
                        UpdatedAt = DateTime.UtcNow,
                        SubCategories = new List<CategoryDto>()
                    },
                    new CategoryDto
                    {
                        CategoryId = 2,
                        Name = "Education",
                        NameJapanese = "??",
                        Description = "School, learning, and education related vocabulary",
                        Type = "Vocabulary",
                        Icon = "??",
                        Color = "#4ECDC4",
                        ParentCategoryId = null,
                        Level = "Beginner",
                        DisplayOrder = 2,
                        ItemCount = 40,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-29),
                        UpdatedAt = DateTime.UtcNow,
                        SubCategories = new List<CategoryDto>()
                    },
                    new CategoryDto
                    {
                        CategoryId = 3,
                        Name = "Family",
                        NameJapanese = "??",
                        Description = "Family members and relationships",
                        Type = "Vocabulary",
                        Icon = "???????????",
                        Color = "#45B7D1",
                        ParentCategoryId = null,
                        Level = "Beginner",
                        DisplayOrder = 3,
                        ItemCount = 20,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-28),
                        UpdatedAt = DateTime.UtcNow,
                        SubCategories = new List<CategoryDto>()
                    },
                    new CategoryDto
                    {
                        CategoryId = 4,
                        Name = "Basic Particles",
                        NameJapanese = "????",
                        Description = "Essential Japanese particles",
                        Type = "Grammar",
                        Icon = "??",
                        Color = "#96CEB4",
                        ParentCategoryId = null,
                        Level = "Beginner",
                        DisplayOrder = 4,
                        ItemCount = 15,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-27),
                        UpdatedAt = DateTime.UtcNow,
                        SubCategories = new List<CategoryDto>()
                    },
                    new CategoryDto
                    {
                        CategoryId = 5,
                        Name = "Basic Kanji",
                        NameJapanese = "????",
                        Description = "Fundamental kanji characters",
                        Type = "Kanji",
                        Icon = "?",
                        Color = "#FECA57",
                        ParentCategoryId = null,
                        Level = "Beginner",
                        DisplayOrder = 5,
                        ItemCount = 100,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-26),
                        UpdatedAt = DateTime.UtcNow,
                        SubCategories = new List<CategoryDto>()
                    }
                };

                // Apply filters
                if (!string.IsNullOrEmpty(search))
                {
                    categories = categories.Where(c =>
                        c.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        c.NameJapanese.Contains(search) ||
                        c.Description.Contains(search, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                if (!string.IsNullOrEmpty(type))
                {
                    categories = categories.Where(c => c.Type.Equals(type, StringComparison.OrdinalIgnoreCase))
                                          .ToList();
                }

                if (isActive.HasValue)
                {
                    categories = categories.Where(c => c.IsActive == isActive.Value).ToList();
                }

                // Apply sorting
                categories = sortBy?.ToLower() switch
                {
                    "name" => sortDirection?.ToLower() == "desc"
                        ? categories.OrderByDescending(c => c.Name).ToList()
                        : categories.OrderBy(c => c.Name).ToList(),
                    "type" => sortDirection?.ToLower() == "desc"
                        ? categories.OrderByDescending(c => c.Type).ToList()
                        : categories.OrderBy(c => c.Type).ToList(),
                    "itemcount" => sortDirection?.ToLower() == "desc"
                        ? categories.OrderByDescending(c => c.ItemCount).ToList()
                        : categories.OrderBy(c => c.ItemCount).ToList(),
                    "displayorder" => sortDirection?.ToLower() == "desc"
                        ? categories.OrderByDescending(c => c.DisplayOrder).ToList()
                        : categories.OrderBy(c => c.DisplayOrder).ToList(),
                    _ => categories.OrderBy(c => c.DisplayOrder).ToList()
                };

                // Pagination
                var totalCount = categories.Count;
                var items = categories.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var result = new PaginatedResultDto<CategoryDto>
                {
                    Data = items,
                    TotalCount = totalCount,
                    PageNumber = page,
                    PageSize = pageSize
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting categories");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y chi ti?t danh m?c
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            try
            {
                var category = new CategoryDto
                {
                    CategoryId = id,
                    Name = "Greetings",
                    NameJapanese = "??",
                    Description = "Basic greetings and polite expressions used in daily conversation",
                    Type = "Vocabulary",
                    Icon = "??",
                    Color = "#FF6B6B",
                    ParentCategoryId = null,
                    Level = "Beginner",
                    DisplayOrder = 1,
                    ItemCount = 25,
                    Tags = new List<string> { "basic", "conversation", "polite" },
                    ImageUrl = "/images/categories/greetings.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    UpdatedAt = DateTime.UtcNow,
                    SubCategories = new List<CategoryDto>
                    {
                        new CategoryDto
                        {
                            CategoryId = 11,
                            Name = "Morning Greetings",
                            NameJapanese = "????",
                            Description = "Greetings used in the morning",
                            Type = "Vocabulary",
                            ParentCategoryId = id,
                            ItemCount = 8,
                            IsActive = true
                        },
                        new CategoryDto
                        {
                            CategoryId = 12,
                            Name = "Evening Greetings",
                            NameJapanese = "????",
                            Description = "Greetings used in the evening",
                            Type = "Vocabulary",
                            ParentCategoryId = id,
                            ItemCount = 6,
                            IsActive = true
                        }
                    }
                };

                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting category {CategoryId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// T?o danh m?c m?i
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CreateCategoryDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newCategory = new CategoryDto
                {
                    CategoryId = new Random().Next(100, 999),
                    Name = createDto.Name,
                    NameJapanese = createDto.NameJapanese,
                    Description = createDto.Description,
                    Type = createDto.Type,
                    Icon = createDto.Icon,
                    Color = createDto.Color,
                    ParentCategoryId = createDto.ParentCategoryId,
                    Level = createDto.Level,
                    DisplayOrder = createDto.DisplayOrder,
                    Tags = createDto.Tags ?? new List<string>(),
                    ItemCount = 0,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    SubCategories = new List<CategoryDto>()
                };

                return CreatedAtAction(nameof(GetCategory), new { id = newCategory.CategoryId }, newCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// C?p nh?t danh m?c
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromBody] UpdateCategoryDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedCategory = new CategoryDto
                {
                    CategoryId = id,
                    Name = updateDto.Name ?? "Updated Category",
                    NameJapanese = updateDto.NameJapanese ?? "?????????",
                    Description = updateDto.Description ?? "Updated description",
                    Type = updateDto.Type ?? "Vocabulary",
                    Icon = updateDto.Icon ?? "??",
                    Color = updateDto.Color ?? "#666666",
                    ParentCategoryId = updateDto.ParentCategoryId,
                    Level = updateDto.Level ?? "Beginner",
                    DisplayOrder = updateDto.DisplayOrder ?? 999,
                    Tags = updateDto.Tags ?? new List<string>(),
                    ItemCount = 10,
                    IsActive = updateDto.IsActive ?? true,
                    UpdatedAt = DateTime.UtcNow,
                    SubCategories = new List<CategoryDto>()
                };

                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category {CategoryId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Xóa danh m?c
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                // TODO: Implement actual deletion logic (soft delete, check for dependencies)
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category {CategoryId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y danh m?c theo lo?i
        /// </summary>
        [HttpGet("by-type/{type}")]
        public async Task<ActionResult<List<CategoryDto>>> GetCategoriesByType(string type)
        {
            try
            {
                var categories = new List<CategoryDto>
                {
                    new CategoryDto
                    {
                        CategoryId = 1,
                        Name = $"{type} Category 1",
                        Type = type,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        SubCategories = new List<CategoryDto>()
                    },
                    new CategoryDto
                    {
                        CategoryId = 2,
                        Name = $"{type} Category 2",
                        Type = type,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        SubCategories = new List<CategoryDto>()
                    }
                };

                return Ok(categories.Where(c => c.Type.Equals(type, StringComparison.OrdinalIgnoreCase)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting categories by type {Type}", type);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y danh m?c cha (top-level)
        /// </summary>
        [HttpGet("parents")]
        public async Task<ActionResult<List<CategoryDto>>> GetParentCategories()
        {
            try
            {
                var parentCategories = new List<CategoryDto>
                {
                    new CategoryDto
                    {
                        CategoryId = 1,
                        Name = "Greetings",
                        NameJapanese = "??",
                        Type = "Vocabulary",
                        Icon = "??",
                        Color = "#FF6B6B",
                        ParentCategoryId = null,
                        ItemCount = 25,
                        IsActive = true,
                        SubCategories = new List<CategoryDto>()
                    },
                    new CategoryDto
                    {
                        CategoryId = 2,
                        Name = "Education",
                        NameJapanese = "??",
                        Type = "Vocabulary",
                        Icon = "??",
                        Color = "#4ECDC4",
                        ParentCategoryId = null,
                        ItemCount = 40,
                        IsActive = true,
                        SubCategories = new List<CategoryDto>()
                    }
                };

                return Ok(parentCategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting parent categories");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y danh m?c con
        /// </summary>
        [HttpGet("{id}/subcategories")]
        public async Task<ActionResult<List<CategoryDto>>> GetSubCategories(int id)
        {
            try
            {
                var subCategories = new List<CategoryDto>
                {
                    new CategoryDto
                    {
                        CategoryId = 11,
                        Name = "Morning Greetings",
                        NameJapanese = "????",
                        Type = "Vocabulary",
                        ParentCategoryId = id,
                        ItemCount = 8,
                        IsActive = true,
                        SubCategories = new List<CategoryDto>()
                    },
                    new CategoryDto
                    {
                        CategoryId = 12,
                        Name = "Evening Greetings",
                        NameJapanese = "????",
                        Type = "Vocabulary",
                        ParentCategoryId = id,
                        ItemCount = 6,
                        IsActive = true,
                        SubCategories = new List<CategoryDto>()
                    }
                };

                return Ok(subCategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting subcategories for category {CategoryId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Tìm ki?m danh m?c
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<List<CategoryDto>>> SearchCategories([FromQuery] string query)
        {
            try
            {
                var categories = new List<CategoryDto>();

                if (!string.IsNullOrEmpty(query))
                {
                    categories.Add(new CategoryDto
                    {
                        CategoryId = 1,
                        Name = $"Found: {query}",
                        Description = $"Search result for: {query}",
                        Type = "Vocabulary",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        SubCategories = new List<CategoryDto>()
                    });
                }

                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching categories");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// C?p nh?t th? t? hi?n th?
        /// </summary>
        [HttpPut("reorder")]
        public async Task<ActionResult> ReorderCategories([FromBody] List<CategoryOrderDto> orders)
        {
            try
            {
                // TODO: Implement actual reordering logic
                return Ok(new { Message = "Categories reordered successfully", Count = orders.Count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reordering categories");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y th?ng kê danh m?c
        /// </summary>
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetCategoryStatistics()
        {
            try
            {
                var statistics = new
                {
                    TotalCategories = 50,
                    ByType = new
                    {
                        Vocabulary = 30,
                        Grammar = 12,
                        Kanji = 8
                    },
                    ParentCategories = 15,
                    SubCategories = 35,
                    EmptyCategories = 5,
                    PopularCategories = new[]
                    {
                        new { Name = "Greetings", ItemCount = 25 },
                        new { Name = "Education", ItemCount = 40 },
                        new { Name = "Family", ItemCount = 20 }
                    }
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting category statistics");
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

    #region DTOs

    /// <summary>
    /// DTO cho danh m?c
    /// </summary>
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NameJapanese { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Vocabulary, Grammar, Kanji
        public string Icon { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int? ParentCategoryId { get; set; }
        public string Level { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } = 0;
        public int ItemCount { get; set; } = 0;
        public List<string> Tags { get; set; } = new List<string>();
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<CategoryDto> SubCategories { get; set; } = new List<CategoryDto>();
    }

    /// <summary>
    /// DTO cho t?o danh m?c m?i
    /// </summary>
    public class CreateCategoryDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string NameJapanese { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty;

        [StringLength(10)]
        public string Icon { get; set; } = string.Empty;

        [StringLength(20)]
        public string Color { get; set; } = string.Empty;

        public int? ParentCategoryId { get; set; }

        [StringLength(50)]
        public string Level { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 0;

        public List<string>? Tags { get; set; }
    }

    /// <summary>
    /// DTO cho c?p nh?t danh m?c
    /// </summary>
    public class UpdateCategoryDto
    {
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(100)]
        public string? NameJapanese { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(50)]
        public string? Type { get; set; }

        [StringLength(10)]
        public string? Icon { get; set; }

        [StringLength(20)]
        public string? Color { get; set; }

        public int? ParentCategoryId { get; set; }

        [StringLength(50)]
        public string? Level { get; set; }

        public int? DisplayOrder { get; set; }

        public List<string>? Tags { get; set; }

        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// DTO cho s?p x?p danh m?c
    /// </summary>
    public class CategoryOrderDto
    {
        public int CategoryId { get; set; }
        public int DisplayOrder { get; set; }
    }

    #endregion
}