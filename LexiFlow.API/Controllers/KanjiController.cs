using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LexiFlow.API.DTOs.Common;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.Controllers
{
    /// <summary>
    /// Controller qu?n lý Kanji
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class KanjiController : ControllerBase
    {
        private readonly ILogger<KanjiController> _logger;

        public KanjiController(ILogger<KanjiController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// L?y danh sách Kanji v?i phân trang và l?c
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PaginatedResultDto<KanjiDto>>> GetKanjis(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? level = null,
            [FromQuery] string? grade = null,
            [FromQuery] int? minStrokes = null,
            [FromQuery] int? maxStrokes = null,
            [FromQuery] string? radical = null,
            [FromQuery] string? sortBy = "Character",
            [FromQuery] string? sortDirection = "asc")
        {
            try
            {
                // Mock data
                var kanjis = new List<KanjiDto>
                {
                    new KanjiDto
                    {
                        KanjiId = 1,
                        Character = "?",
                        OnYomi = "?????",
                        KunYomi = "??",
                        Meaning = "person, human",
                        StrokeCount = 2,
                        Level = "N5",
                        Grade = "1",
                        Radical = "?",
                        RadicalMeaning = "person",
                        Examples = new List<string> { "??", "???", "??" },
                        StrokeOrder = "1. ? (top-left to bottom-right), 2. ? (top-right to bottom-left)",
                        UnicodeHex = "4EBA",
                        Frequency = 5,
                        Difficulty = 1,
                        Mnemonics = "Looks like a person walking",
                        Etymology = "Pictograph of a person",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-30),
                        UpdatedAt = DateTime.UtcNow
                    },
                    new KanjiDto
                    {
                        KanjiId = 2,
                        Character = "?",
                        OnYomi = "?????",
                        KunYomi = "???",
                        Meaning = "day, sun",
                        StrokeCount = 4,
                        Level = "N5",
                        Grade = "1",
                        Radical = "?",
                        RadicalMeaning = "sun",
                        Examples = new List<string> { "??", "??", "??" },
                        StrokeOrder = "1. ? (square), 2. ? (horizontal line inside)",
                        UnicodeHex = "65E5",
                        Frequency = 5,
                        Difficulty = 1,
                        Mnemonics = "Picture of the sun",
                        Etymology = "Pictograph of the sun",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-29),
                        UpdatedAt = DateTime.UtcNow
                    },
                    new KanjiDto
                    {
                        KanjiId = 3,
                        Character = "?",
                        OnYomi = "??",
                        KunYomi = "??",
                        Meaning = "book, origin, main",
                        StrokeCount = 5,
                        Level = "N5",
                        Grade = "1",
                        Radical = "?",
                        RadicalMeaning = "tree",
                        Examples = new List<string> { "??", "??", "??" },
                        StrokeOrder = "1. ? (tree), 2. ? (line at bottom)",
                        UnicodeHex = "672C",
                        Frequency = 4,
                        Difficulty = 2,
                        Mnemonics = "Tree with a line at the root - origin",
                        Etymology = "Tree with a mark at the root indicating origin",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-28),
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                // Apply filters
                if (!string.IsNullOrEmpty(search))
                {
                    kanjis = kanjis.Where(k => 
                        k.Character.Contains(search) ||
                        k.Meaning.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        k.OnYomi.Contains(search) ||
                        k.KunYomi.Contains(search) ||
                        k.Examples.Any(e => e.Contains(search)))
                        .ToList();
                }

                if (!string.IsNullOrEmpty(level))
                {
                    kanjis = kanjis.Where(k => k.Level.Equals(level, StringComparison.OrdinalIgnoreCase))
                                   .ToList();
                }

                if (!string.IsNullOrEmpty(grade))
                {
                    kanjis = kanjis.Where(k => k.Grade.Equals(grade, StringComparison.OrdinalIgnoreCase))
                                   .ToList();
                }

                if (minStrokes.HasValue)
                {
                    kanjis = kanjis.Where(k => k.StrokeCount >= minStrokes.Value).ToList();
                }

                if (maxStrokes.HasValue)
                {
                    kanjis = kanjis.Where(k => k.StrokeCount <= maxStrokes.Value).ToList();
                }

                if (!string.IsNullOrEmpty(radical))
                {
                    kanjis = kanjis.Where(k => k.Radical.Equals(radical, StringComparison.OrdinalIgnoreCase))
                                   .ToList();
                }

                // Apply sorting
                kanjis = sortBy?.ToLower() switch
                {
                    "character" => sortDirection?.ToLower() == "desc" 
                        ? kanjis.OrderByDescending(k => k.Character).ToList()
                        : kanjis.OrderBy(k => k.Character).ToList(),
                    "meaning" => sortDirection?.ToLower() == "desc"
                        ? kanjis.OrderByDescending(k => k.Meaning).ToList()
                        : kanjis.OrderBy(k => k.Meaning).ToList(),
                    "strokecount" => sortDirection?.ToLower() == "desc"
                        ? kanjis.OrderByDescending(k => k.StrokeCount).ToList()
                        : kanjis.OrderBy(k => k.StrokeCount).ToList(),
                    "level" => sortDirection?.ToLower() == "desc"
                        ? kanjis.OrderByDescending(k => k.Level).ToList()
                        : kanjis.OrderBy(k => k.Level).ToList(),
                    "frequency" => sortDirection?.ToLower() == "desc"
                        ? kanjis.OrderByDescending(k => k.Frequency).ToList()
                        : kanjis.OrderBy(k => k.Frequency).ToList(),
                    "difficulty" => sortDirection?.ToLower() == "desc"
                        ? kanjis.OrderByDescending(k => k.Difficulty).ToList()
                        : kanjis.OrderBy(k => k.Difficulty).ToList(),
                    _ => kanjis.OrderBy(k => k.Character).ToList()
                };

                // Pagination
                var totalCount = kanjis.Count;
                var items = kanjis.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var result = new PaginatedResultDto<KanjiDto>
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
                _logger.LogError(ex, "Error getting kanjis");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y chi ti?t Kanji
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<KanjiDto>> GetKanji(int id)
        {
            try
            {
                var kanji = new KanjiDto
                {
                    KanjiId = id,
                    Character = "?",
                    OnYomi = "?????",
                    KunYomi = "??",
                    Meaning = "person, human",
                    StrokeCount = 2,
                    Level = "N5",
                    Grade = "1",
                    Radical = "?",
                    RadicalMeaning = "person",
                    Examples = new List<string> { "??", "???", "??", "??", "???" },
                    StrokeOrder = "1. ? (top-left to bottom-right), 2. ? (top-right to bottom-left)",
                    UnicodeHex = "4EBA",
                    Frequency = 5,
                    Difficulty = 1,
                    Mnemonics = "Looks like a person walking with two legs",
                    Etymology = "Ancient pictograph of a standing person",
                    KanjiGroup = "Basic",
                    Notes = "One of the most fundamental kanji, appears in many common words",
                    RelatedKanjis = new List<string> { "?", "?", "?" },
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    UpdatedAt = DateTime.UtcNow
                };

                return Ok(kanji);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting kanji {KanjiId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// T?o Kanji m?i
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<KanjiDto>> CreateKanji([FromBody] CreateKanjiDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newKanji = new KanjiDto
                {
                    KanjiId = new Random().Next(100, 999),
                    Character = createDto.Character,
                    OnYomi = createDto.OnYomi,
                    KunYomi = createDto.KunYomi,
                    Meaning = createDto.Meaning,
                    StrokeCount = createDto.StrokeCount,
                    Level = createDto.Level,
                    Grade = createDto.Grade,
                    Radical = createDto.Radical,
                    RadicalMeaning = createDto.RadicalMeaning,
                    Examples = createDto.Examples ?? new List<string>(),
                    StrokeOrder = createDto.StrokeOrder,
                    UnicodeHex = createDto.UnicodeHex,
                    Frequency = createDto.Frequency,
                    Difficulty = createDto.Difficulty,
                    Mnemonics = createDto.Mnemonics,
                    Etymology = createDto.Etymology,
                    KanjiGroup = createDto.KanjiGroup,
                    Notes = createDto.Notes,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                return CreatedAtAction(nameof(GetKanji), new { id = newKanji.KanjiId }, newKanji);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating kanji");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// C?p nh?t Kanji
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<KanjiDto>> UpdateKanji(int id, [FromBody] UpdateKanjiDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedKanji = new KanjiDto
                {
                    KanjiId = id,
                    Character = updateDto.Character ?? "?",
                    OnYomi = updateDto.OnYomi ?? "?????",
                    KunYomi = updateDto.KunYomi ?? "??",
                    Meaning = updateDto.Meaning ?? "Updated meaning",
                    StrokeCount = updateDto.StrokeCount ?? 2,
                    Level = updateDto.Level ?? "N5",
                    Grade = updateDto.Grade ?? "1",
                    Radical = updateDto.Radical ?? "?",
                    RadicalMeaning = updateDto.RadicalMeaning ?? "person",
                    Examples = updateDto.Examples ?? new List<string>(),
                    StrokeOrder = updateDto.StrokeOrder ?? "",
                    UnicodeHex = updateDto.UnicodeHex ?? "",
                    Frequency = updateDto.Frequency ?? 3,
                    Difficulty = updateDto.Difficulty ?? 1,
                    Mnemonics = updateDto.Mnemonics ?? "",
                    Etymology = updateDto.Etymology ?? "",
                    KanjiGroup = updateDto.KanjiGroup ?? "",
                    Notes = updateDto.Notes ?? "",
                    IsActive = updateDto.IsActive ?? true,
                    UpdatedAt = DateTime.UtcNow
                };

                return Ok(updatedKanji);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating kanji {KanjiId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Xóa Kanji
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteKanji(int id)
        {
            try
            {
                // TODO: Implement actual deletion logic (soft delete)
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting kanji {KanjiId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y t? v?ng ch?a Kanji này
        /// </summary>
        [HttpGet("{id}/vocabularies")]
        public async Task<ActionResult<List<object>>> GetKanjiVocabularies(int id)
        {
            try
            {
                var vocabularies = new List<object>
                {
                    new { 
                        VocabularyId = 1, 
                        Word = "??", 
                        Hiragana = "????",
                        Meaning = "human being", 
                        Romaji = "ningen",
                        Level = "N5",
                        KanjiPosition = "first"
                    },
                    new { 
                        VocabularyId = 2, 
                        Word = "???", 
                        Hiragana = "?????",
                        Meaning = "Japanese person", 
                        Romaji = "nihonjin",
                        Level = "N5",
                        KanjiPosition = "last"
                    },
                    new { 
                        VocabularyId = 3, 
                        Word = "??", 
                        Hiragana = "????",
                        Meaning = "population", 
                        Romaji = "jinkou",
                        Level = "N4",
                        KanjiPosition = "first"
                    }
                };

                return Ok(vocabularies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabularies for kanji {KanjiId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y Kanji theo c?p ??
        /// </summary>
        [HttpGet("by-level/{level}")]
        public async Task<ActionResult<PaginatedResultDto<KanjiDto>>> GetKanjisByLevel(
            string level,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var kanjis = new List<KanjiDto>
                {
                    new KanjiDto
                    {
                        KanjiId = 1,
                        Character = "?",
                        Meaning = "person",
                        Level = level,
                        StrokeCount = 2,
                        Grade = "1",
                        Frequency = 5,
                        Difficulty = 1,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new KanjiDto
                    {
                        KanjiId = 2,
                        Character = "?",
                        Meaning = "day, sun",
                        Level = level,
                        StrokeCount = 4,
                        Grade = "1",
                        Frequency = 5,
                        Difficulty = 1,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                var filteredKanjis = kanjis.Where(k => k.Level.Equals(level, StringComparison.OrdinalIgnoreCase))
                                          .ToList();

                var totalCount = filteredKanjis.Count;
                var items = filteredKanjis.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var result = new PaginatedResultDto<KanjiDto>
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
                _logger.LogError(ex, "Error getting kanjis by level {Level}", level);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y Kanji theo s? nét
        /// </summary>
        [HttpGet("by-strokes/{strokes}")]
        public async Task<ActionResult<List<KanjiDto>>> GetKanjisByStrokes(int strokes)
        {
            try
            {
                var kanjis = new List<KanjiDto>
                {
                    new KanjiDto
                    {
                        KanjiId = 1,
                        Character = "?",
                        Meaning = "person",
                        Level = "N5",
                        StrokeCount = strokes,
                        Grade = "1",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                return Ok(kanjis.Where(k => k.StrokeCount == strokes).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting kanjis by strokes {Strokes}", strokes);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Tìm ki?m Kanji
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<PaginatedResultDto<KanjiDto>>> SearchKanjis(
            [FromQuery] string query,
            [FromQuery] string? searchType = "all", // all, character, meaning, reading
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var kanjis = new List<KanjiDto>();
                
                if (!string.IsNullOrEmpty(query))
                {
                    kanjis = searchType?.ToLower() switch
                    {
                        "character" => GetMockKanjisByCharacter(query),
                        "meaning" => GetMockKanjisByMeaning(query),
                        "reading" => GetMockKanjisByReading(query),
                        _ => GetMockKanjisByAll(query)
                    };
                }

                var totalCount = kanjis.Count;
                var items = kanjis.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var result = new PaginatedResultDto<KanjiDto>
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
                _logger.LogError(ex, "Error searching kanjis");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y Kanji ng?u nhiên
        /// </summary>
        [HttpGet("random")]
        public async Task<ActionResult<List<KanjiDto>>> GetRandomKanjis(
            [FromQuery] int count = 5,
            [FromQuery] string? level = null,
            [FromQuery] int? maxStrokes = null)
        {
            try
            {
                var kanjis = new List<KanjiDto>
                {
                    new KanjiDto
                    {
                        KanjiId = 1,
                        Character = "?",
                        Meaning = "person",
                        Level = level ?? "N5",
                        StrokeCount = maxStrokes ?? 2,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new KanjiDto
                    {
                        KanjiId = 2,
                        Character = "?",
                        Meaning = "day, sun",
                        Level = level ?? "N5",
                        StrokeCount = maxStrokes ?? 4,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                return Ok(kanjis.Take(count).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting random kanjis");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y th?ng kê Kanji
        /// </summary>
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetKanjiStatistics()
        {
            try
            {
                var statistics = new
                {
                    TotalKanjis = 2136,
                    ByLevel = new
                    {
                        N5 = 103,
                        N4 = 181,
                        N3 = 361,
                        N2 = 415,
                        N1 = 1076
                    },
                    ByGrade = new
                    {
                        Grade1 = 80,
                        Grade2 = 160,
                        Grade3 = 200,
                        Grade4 = 202,
                        Grade5 = 193,
                        Grade6 = 191,
                        Secondary = 1110
                    },
                    ByStrokeCount = new
                    {
                        Simple = 245, // 1-5 strokes
                        Medium = 891, // 6-10 strokes
                        Complex = 1000 // 11+ strokes
                    },
                    CommonRadicals = 214,
                    WithMnemonics = 1500,
                    WithStrokeOrder = 2000
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting kanji statistics");
                return StatusCode(500, "Internal server error");
            }
        }

        #region Private Helper Methods

        private List<KanjiDto> GetMockKanjisByCharacter(string query)
        {
            return new List<KanjiDto>
            {
                new KanjiDto
                {
                    KanjiId = 1,
                    Character = query.Substring(0, 1),
                    Meaning = $"Meaning of {query}",
                    Level = "N5",
                    StrokeCount = 5,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
        }

        private List<KanjiDto> GetMockKanjisByMeaning(string query)
        {
            return new List<KanjiDto>
            {
                new KanjiDto
                {
                    KanjiId = 2,
                    Character = "Sample",
                    Meaning = query,
                    Level = "N5",
                    StrokeCount = 5,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
        }

        private List<KanjiDto> GetMockKanjisByReading(string query)
        {
            return new List<KanjiDto>
            {
                new KanjiDto
                {
                    KanjiId = 3,
                    Character = "Sample",
                    OnYomi = query,
                    KunYomi = query,
                    Meaning = $"Kanji with reading {query}",
                    Level = "N5",
                    StrokeCount = 5,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
        }

        private List<KanjiDto> GetMockKanjisByAll(string query)
        {
            return new List<KanjiDto>
            {
                new KanjiDto
                {
                    KanjiId = 4,
                    Character = query.Substring(0, Math.Min(1, query.Length)),
                    Meaning = $"Search result for: {query}",
                    OnYomi = query,
                    Level = "N5",
                    StrokeCount = 5,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
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

        #endregion
    }

    #region DTOs

    /// <summary>
    /// DTO cho Kanji
    /// </summary>
    public class KanjiDto
    {
        public int KanjiId { get; set; }
        public string Character { get; set; } = string.Empty;
        public string OnYomi { get; set; } = string.Empty;
        public string KunYomi { get; set; } = string.Empty;
        public string Meaning { get; set; } = string.Empty;
        public int StrokeCount { get; set; }
        public string Level { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public string Radical { get; set; } = string.Empty;
        public string RadicalMeaning { get; set; } = string.Empty;
        public List<string> Examples { get; set; } = new List<string>();
        public string StrokeOrder { get; set; } = string.Empty;
        public string UnicodeHex { get; set; } = string.Empty;
        public int Frequency { get; set; } = 3;
        public int Difficulty { get; set; } = 1;
        public string Mnemonics { get; set; } = string.Empty;
        public string Etymology { get; set; } = string.Empty;
        public string KanjiGroup { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public List<string> RelatedKanjis { get; set; } = new List<string>();
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO cho t?o Kanji m?i
    /// </summary>
    public class CreateKanjiDto
    {
        [Required]
        [StringLength(10)]
        public string Character { get; set; } = string.Empty;

        [StringLength(100)]
        public string OnYomi { get; set; } = string.Empty;

        [StringLength(100)]
        public string KunYomi { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Meaning { get; set; } = string.Empty;

        [Required]
        [Range(1, 50)]
        public int StrokeCount { get; set; }

        [Required]
        [StringLength(10)]
        public string Level { get; set; } = string.Empty;

        [StringLength(20)]
        public string Grade { get; set; } = string.Empty;

        [StringLength(50)]
        public string Radical { get; set; } = string.Empty;

        [StringLength(100)]
        public string RadicalMeaning { get; set; } = string.Empty;

        public List<string>? Examples { get; set; }

        public string StrokeOrder { get; set; } = string.Empty;

        [StringLength(10)]
        public string UnicodeHex { get; set; } = string.Empty;

        [Range(1, 5)]
        public int Frequency { get; set; } = 3;

        [Range(1, 5)]
        public int Difficulty { get; set; } = 1;

        [StringLength(200)]
        public string Mnemonics { get; set; } = string.Empty;

        [StringLength(200)]
        public string Etymology { get; set; } = string.Empty;

        [StringLength(50)]
        public string KanjiGroup { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO cho c?p nh?t Kanji
    /// </summary>
    public class UpdateKanjiDto
    {
        [StringLength(10)]
        public string? Character { get; set; }

        [StringLength(100)]
        public string? OnYomi { get; set; }

        [StringLength(100)]
        public string? KunYomi { get; set; }

        [StringLength(200)]
        public string? Meaning { get; set; }

        [Range(1, 50)]
        public int? StrokeCount { get; set; }

        [StringLength(10)]
        public string? Level { get; set; }

        [StringLength(20)]
        public string? Grade { get; set; }

        [StringLength(50)]
        public string? Radical { get; set; }

        [StringLength(100)]
        public string? RadicalMeaning { get; set; }

        public List<string>? Examples { get; set; }

        public string? StrokeOrder { get; set; }

        [StringLength(10)]
        public string? UnicodeHex { get; set; }

        [Range(1, 5)]
        public int? Frequency { get; set; }

        [Range(1, 5)]
        public int? Difficulty { get; set; }

        [StringLength(200)]
        public string? Mnemonics { get; set; }

        [StringLength(200)]
        public string? Etymology { get; set; }

        [StringLength(50)]
        public string? KanjiGroup { get; set; }

        public string? Notes { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion
}