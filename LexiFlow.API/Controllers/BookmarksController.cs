using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LexiFlow.API.DTOs.Common;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.Controllers
{
    /// <summary>
    /// Controller qu?n lý bookmark và favorites
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookmarksController : ControllerBase
    {
        private readonly ILogger<BookmarksController> _logger;

        public BookmarksController(ILogger<BookmarksController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// L?y danh sách bookmark c?a ng??i dùng
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PaginatedResultDto<BookmarkDto>>> GetBookmarks(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? type = null,
            [FromQuery] string? category = null,
            [FromQuery] string? search = null)
        {
            try
            {
                var userId = GetCurrentUserId();

                var bookmarks = new List<BookmarkDto>
                {
                    new BookmarkDto
                    {
                        BookmarkId = 1,
                        UserId = userId,
                        ItemType = "Vocabulary",
                        ItemId = 101,
                        Title = "?????",
                        Description = "Hello - Basic greeting",
                        Category = "Greetings",
                        Tags = new List<string> { "basic", "greeting", "common" },
                        CreatedAt = DateTime.UtcNow.AddDays(-7),
                        LastAccessedAt = DateTime.UtcNow.AddHours(-2),
                        AccessCount = 5,
                        IsPublic = false,
                        Priority = "High",
                        Notes = "Important for daily conversation"
                    },
                    new BookmarkDto
                    {
                        BookmarkId = 2,
                        UserId = userId,
                        ItemType = "Kanji",
                        ItemId = 201,
                        Title = "?",
                        Description = "Water - N5 Kanji",
                        Category = "Nature",
                        Tags = new List<string> { "n5", "nature", "basic" },
                        CreatedAt = DateTime.UtcNow.AddDays(-5),
                        LastAccessedAt = DateTime.UtcNow.AddDays(-1),
                        AccessCount = 3,
                        IsPublic = true,
                        Priority = "Medium",
                        Notes = "Remember the stroke order"
                    }
                };

                var filteredBookmarks = bookmarks.AsEnumerable();

                if (!string.IsNullOrEmpty(type))
                {
                    filteredBookmarks = filteredBookmarks.Where(b => b.ItemType.Equals(type, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(category))
                {
                    filteredBookmarks = filteredBookmarks.Where(b => b.Category.Contains(category, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(search))
                {
                    filteredBookmarks = filteredBookmarks.Where(b => 
                        b.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        b.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
                }

                var totalItems = filteredBookmarks.Count();
                var items = filteredBookmarks
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var result = new PaginatedResultDto<BookmarkDto>
                {
                    Items = items,
                    TotalItems = totalItems,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalItems / pageSize)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting bookmarks");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Thêm bookmark m?i
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<BookmarkDto>> CreateBookmark([FromBody] CreateBookmarkDto createBookmark)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                var bookmark = new BookmarkDto
                {
                    BookmarkId = new Random().Next(1000, 9999),
                    UserId = userId,
                    ItemType = createBookmark.ItemType,
                    ItemId = createBookmark.ItemId,
                    Title = createBookmark.Title,
                    Description = createBookmark.Description,
                    Category = createBookmark.Category,
                    Tags = createBookmark.Tags,
                    CreatedAt = DateTime.UtcNow,
                    LastAccessedAt = DateTime.UtcNow,
                    AccessCount = 1,
                    IsPublic = createBookmark.IsPublic,
                    Priority = createBookmark.Priority,
                    Notes = createBookmark.Notes
                };

                return CreatedAtAction(nameof(GetBookmark), new { id = bookmark.BookmarkId }, bookmark);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating bookmark");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y bookmark theo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookmarkDto>> GetBookmark(int id)
        {
            try
            {
                var userId = GetCurrentUserId();

                var bookmark = new BookmarkDto
                {
                    BookmarkId = id,
                    UserId = userId,
                    ItemType = "Vocabulary",
                    ItemId = 101,
                    Title = "?????",
                    Description = "Hello - Basic greeting",
                    Category = "Greetings",
                    Tags = new List<string> { "basic", "greeting" },
                    CreatedAt = DateTime.UtcNow.AddDays(-7),
                    LastAccessedAt = DateTime.UtcNow,
                    AccessCount = 6,
                    IsPublic = false,
                    Priority = "High",
                    Notes = "Important for daily conversation"
                };

                return Ok(bookmark);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting bookmark {BookmarkId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// C?p nh?t bookmark
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<BookmarkDto>> UpdateBookmark(int id, [FromBody] UpdateBookmarkDto updateBookmark)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                var bookmark = new BookmarkDto
                {
                    BookmarkId = id,
                    UserId = userId,
                    ItemType = "Vocabulary",
                    ItemId = 101,
                    Title = updateBookmark.Title,
                    Description = updateBookmark.Description,
                    Category = updateBookmark.Category,
                    Tags = updateBookmark.Tags,
                    CreatedAt = DateTime.UtcNow.AddDays(-7),
                    LastAccessedAt = DateTime.UtcNow,
                    AccessCount = 6,
                    IsPublic = updateBookmark.IsPublic,
                    Priority = updateBookmark.Priority,
                    Notes = updateBookmark.Notes
                };

                return Ok(bookmark);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating bookmark {BookmarkId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Xóa bookmark
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBookmark(int id)
        {
            try
            {
                var userId = GetCurrentUserId();

                // TODO: Delete bookmark from database

                return Ok(new { Message = "Bookmark deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting bookmark {BookmarkId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Thêm/xóa bookmark nhanh
        /// </summary>
        [HttpPost("toggle")]
        public async Task<ActionResult<BookmarkToggleResponseDto>> ToggleBookmark([FromBody] ToggleBookmarkDto toggleBookmark)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                var response = new BookmarkToggleResponseDto
                {
                    ItemType = toggleBookmark.ItemType,
                    ItemId = toggleBookmark.ItemId,
                    IsBookmarked = !toggleBookmark.IsCurrentlyBookmarked,
                    BookmarkId = toggleBookmark.IsCurrentlyBookmarked ? null : new Random().Next(1000, 9999),
                    Message = toggleBookmark.IsCurrentlyBookmarked ? "Bookmark removed" : "Bookmark added"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling bookmark");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y danh sách collections
        /// </summary>
        [HttpGet("collections")]
        public async Task<ActionResult<List<BookmarkCollectionDto>>> GetCollections()
        {
            try
            {
                var userId = GetCurrentUserId();

                var collections = new List<BookmarkCollectionDto>
                {
                    new BookmarkCollectionDto
                    {
                        CollectionId = 1,
                        Name = "N5 Vocabulary",
                        Description = "Essential vocabulary for JLPT N5",
                        ItemCount = 150,
                        IsPublic = false,
                        CreatedAt = DateTime.UtcNow.AddDays(-30),
                        UpdatedAt = DateTime.UtcNow.AddDays(-1),
                        Tags = new List<string> { "jlpt", "n5", "vocabulary" },
                        Color = "#FF5722"
                    },
                    new BookmarkCollectionDto
                    {
                        CollectionId = 2,
                        Name = "Daily Kanji",
                        Description = "Kanji characters for daily use",
                        ItemCount = 89,
                        IsPublic = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-20),
                        UpdatedAt = DateTime.UtcNow.AddHours(-5),
                        Tags = new List<string> { "kanji", "daily", "practice" },
                        Color = "#2196F3"
                    }
                };

                return Ok(collections);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting bookmark collections");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// T?o collection m?i
        /// </summary>
        [HttpPost("collections")]
        public async Task<ActionResult<BookmarkCollectionDto>> CreateCollection([FromBody] CreateCollectionDto createCollection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                var collection = new BookmarkCollectionDto
                {
                    CollectionId = new Random().Next(100, 999),
                    Name = createCollection.Name,
                    Description = createCollection.Description,
                    ItemCount = 0,
                    IsPublic = createCollection.IsPublic,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Tags = createCollection.Tags,
                    Color = createCollection.Color
                };

                return CreatedAtAction(nameof(GetCollection), new { id = collection.CollectionId }, collection);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating bookmark collection");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y collection theo ID
        /// </summary>
        [HttpGet("collections/{id}")]
        public async Task<ActionResult<BookmarkCollectionDetailsDto>> GetCollection(int id)
        {
            try
            {
                var userId = GetCurrentUserId();

                var collection = new BookmarkCollectionDetailsDto
                {
                    CollectionId = id,
                    Name = "N5 Vocabulary",
                    Description = "Essential vocabulary for JLPT N5",
                    ItemCount = 150,
                    IsPublic = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1),
                    Tags = new List<string> { "jlpt", "n5", "vocabulary" },
                    Color = "#FF5722",
                    Bookmarks = new List<BookmarkDto>
                    {
                        new BookmarkDto
                        {
                            BookmarkId = 1,
                            UserId = userId,
                            ItemType = "Vocabulary",
                            ItemId = 101,
                            Title = "?????",
                            Description = "Hello - Basic greeting",
                            Category = "Greetings",
                            Tags = new List<string> { "basic", "greeting" },
                            CreatedAt = DateTime.UtcNow.AddDays(-7),
                            LastAccessedAt = DateTime.UtcNow.AddHours(-2),
                            AccessCount = 5,
                            IsPublic = false,
                            Priority = "High",
                            Notes = "Important for daily conversation"
                        }
                    }
                };

                return Ok(collection);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting bookmark collection {CollectionId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Thêm bookmark vào collection
        /// </summary>
        [HttpPost("collections/{id}/bookmarks")]
        public async Task<ActionResult> AddBookmarkToCollection(int id, [FromBody] AddToCollectionDto addToCollection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                return Ok(new { Message = "Bookmark added to collection successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding bookmark to collection");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y th?ng kê bookmark
        /// </summary>
        [HttpGet("statistics")]
        public async Task<ActionResult<BookmarkStatisticsDto>> GetBookmarkStatistics()
        {
            try
            {
                var userId = GetCurrentUserId();

                var statistics = new BookmarkStatisticsDto
                {
                    TotalBookmarks = 45,
                    TotalCollections = 3,
                    BookmarksByType = new Dictionary<string, int>
                    {
                        { "Vocabulary", 25 },
                        { "Kanji", 15 },
                        { "Grammar", 5 }
                    },
                    BookmarksByCategory = new Dictionary<string, int>
                    {
                        { "Greetings", 8 },
                        { "Numbers", 6 },
                        { "Colors", 4 },
                        { "Food", 7 },
                        { "Others", 20 }
                    },
                    RecentlyAdded = 5,
                    MostAccessed = new List<BookmarkSummaryDto>
                    {
                        new BookmarkSummaryDto
                        {
                            BookmarkId = 1,
                            Title = "?????",
                            ItemType = "Vocabulary",
                            AccessCount = 15
                        }
                    },
                    StudyProgress = new BookmarkProgressDto
                    {
                        StudiedToday = 8,
                        StudiedThisWeek = 25,
                        StudiedThisMonth = 105,
                        MasteredItems = 20,
                        InProgressItems = 25
                    }
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting bookmark statistics");
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

    public class BookmarkDto
    {
        public int BookmarkId { get; set; }
        public int UserId { get; set; }
        public string ItemType { get; set; } = string.Empty;
        public int ItemId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; }
        public DateTime LastAccessedAt { get; set; }
        public int AccessCount { get; set; }
        public bool IsPublic { get; set; }
        public string Priority { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }

    public class CreateBookmarkDto
    {
        [Required]
        public string ItemType { get; set; } = string.Empty;
        [Required]
        public int ItemId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
        public bool IsPublic { get; set; } = false;
        public string Priority { get; set; } = "Medium";
        public string Notes { get; set; } = string.Empty;
    }

    public class UpdateBookmarkDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
        public bool IsPublic { get; set; }
        public string Priority { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }

    public class ToggleBookmarkDto
    {
        [Required]
        public string ItemType { get; set; } = string.Empty;
        [Required]
        public int ItemId { get; set; }
        public bool IsCurrentlyBookmarked { get; set; }
    }

    public class BookmarkToggleResponseDto
    {
        public string ItemType { get; set; } = string.Empty;
        public int ItemId { get; set; }
        public bool IsBookmarked { get; set; }
        public int? BookmarkId { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class BookmarkCollectionDto
    {
        public int CollectionId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ItemCount { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public string Color { get; set; } = string.Empty;
    }

    public class BookmarkCollectionDetailsDto : BookmarkCollectionDto
    {
        public List<BookmarkDto> Bookmarks { get; set; } = new List<BookmarkDto>();
    }

    public class CreateCollectionDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsPublic { get; set; } = false;
        public List<string> Tags { get; set; } = new List<string>();
        public string Color { get; set; } = "#2196F3";
    }

    public class AddToCollectionDto
    {
        [Required]
        public int BookmarkId { get; set; }
    }

    public class BookmarkStatisticsDto
    {
        public int TotalBookmarks { get; set; }
        public int TotalCollections { get; set; }
        public Dictionary<string, int> BookmarksByType { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> BookmarksByCategory { get; set; } = new Dictionary<string, int>();
        public int RecentlyAdded { get; set; }
        public List<BookmarkSummaryDto> MostAccessed { get; set; } = new List<BookmarkSummaryDto>();
        public BookmarkProgressDto StudyProgress { get; set; } = new BookmarkProgressDto();
    }

    public class BookmarkSummaryDto
    {
        public int BookmarkId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ItemType { get; set; } = string.Empty;
        public int AccessCount { get; set; }
    }

    public class BookmarkProgressDto
    {
        public int StudiedToday { get; set; }
        public int StudiedThisWeek { get; set; }
        public int StudiedThisMonth { get; set; }
        public int MasteredItems { get; set; }
        public int InProgressItems { get; set; }
    }

    #endregion
}