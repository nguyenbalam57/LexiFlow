using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LexiFlow.API.DTOs.Common;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.Controllers
{
    /// <summary>
    /// Controller qu?n lý ng? pháp
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GrammarController : ControllerBase
    {
        private readonly ILogger<GrammarController> _logger;

        public GrammarController(ILogger<GrammarController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// L?y danh sách ng? pháp v?i phân trang và l?c
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PaginatedResultDto<GrammarDto>>> GetGrammars(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? level = null,
            [FromQuery] string? grammarType = null,
            [FromQuery] int? difficulty = null,
            [FromQuery] string? sortBy = "Title",
            [FromQuery] string? sortDirection = "asc")
        {
            try
            {
                // Mock data
                var grammars = new List<GrammarDto>
                {
                    new GrammarDto
                    {
                        GrammarId = 1,
                        Title = "??/??? (desu/de aru)",
                        Pattern = "[Noun] + ??",
                        Level = "N5",
                        GrammarType = "Copula",
                        Meaning = "To be (polite/formal)",
                        Usage = "Used to make statements polite and formal",
                        Formation = "Noun + ??",
                        Examples = new List<GrammarExampleDto>
                        {
                            new GrammarExampleDto
                            {
                                ExampleId = 1,
                                Japanese = "???????",
                                Romaji = "Watashi wa gakusei desu.",
                                English = "I am a student.",
                                Vietnamese = "Tôi là h?c sinh."
                            }
                        },
                        Notes = "?? is the polite form of ?/???",
                        RelatedGrammar = new List<string> { "?", "???", "????" },
                        Difficulty = 1,
                        Frequency = 5,
                        Tags = new List<string> { "basic", "polite", "copula" },
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-30),
                        UpdatedAt = DateTime.UtcNow
                    },
                    new GrammarDto
                    {
                        GrammarId = 2,
                        Title = "??? (masu form)",
                        Pattern = "[Verb stem] + ??",
                        Level = "N5",
                        GrammarType = "Conjugation",
                        Meaning = "Polite present/future tense",
                        Usage = "Used to make verbs polite",
                        Formation = "Verb stem + ??",
                        Examples = new List<GrammarExampleDto>
                        {
                            new GrammarExampleDto
                            {
                                ExampleId = 2,
                                Japanese = "????????????",
                                Romaji = "Mainichi nihongo wo benkyou shimasu.",
                                English = "I study Japanese every day.",
                                Vietnamese = "Tôi h?c ti?ng Nh?t m?i ngày."
                            }
                        },
                        Notes = "The polite form of verbs, used in formal situations",
                        RelatedGrammar = new List<string> { "???", "???", "??????" },
                        Difficulty = 1,
                        Frequency = 5,
                        Tags = new List<string> { "verb", "polite", "conjugation" },
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-29),
                        UpdatedAt = DateTime.UtcNow
                    },
                    new GrammarDto
                    {
                        GrammarId = 3,
                        Title = "?? form + ???",
                        Pattern = "[Verb ?-form] + ???",
                        Level = "N5",
                        GrammarType = "Progressive",
                        Meaning = "Present progressive/continuous action",
                        Usage = "Expresses ongoing actions or states",
                        Formation = "Verb ?-form + ???",
                        Examples = new List<GrammarExampleDto>
                        {
                            new GrammarExampleDto
                            {
                                ExampleId = 3,
                                Japanese = "???????????",
                                Romaji = "Ima, hon wo yonde imasu.",
                                English = "I am reading a book now.",
                                Vietnamese = "Bây gi? tôi ?ang ??c sách."
                            }
                        },
                        Notes = "Can express both ongoing actions and resulting states",
                        RelatedGrammar = new List<string> { "???", "???", "????" },
                        Difficulty = 2,
                        Frequency = 4,
                        Tags = new List<string> { "progressive", "continuous", "te-form" },
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-28),
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                // Apply filters
                if (!string.IsNullOrEmpty(search))
                {
                    grammars = grammars.Where(g =>
                        g.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        g.Pattern.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        g.Meaning.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        g.Usage.Contains(search, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                if (!string.IsNullOrEmpty(level))
                {
                    grammars = grammars.Where(g => g.Level.Equals(level, StringComparison.OrdinalIgnoreCase))
                                      .ToList();
                }

                if (!string.IsNullOrEmpty(grammarType))
                {
                    grammars = grammars.Where(g => g.GrammarType.Equals(grammarType, StringComparison.OrdinalIgnoreCase))
                                      .ToList();
                }

                if (difficulty.HasValue)
                {
                    grammars = grammars.Where(g => g.Difficulty == difficulty.Value).ToList();
                }

                // Apply sorting
                grammars = sortBy?.ToLower() switch
                {
                    "title" => sortDirection?.ToLower() == "desc"
                        ? grammars.OrderByDescending(g => g.Title).ToList()
                        : grammars.OrderBy(g => g.Title).ToList(),
                    "level" => sortDirection?.ToLower() == "desc"
                        ? grammars.OrderByDescending(g => g.Level).ToList()
                        : grammars.OrderBy(g => g.Level).ToList(),
                    "difficulty" => sortDirection?.ToLower() == "desc"
                        ? grammars.OrderByDescending(g => g.Difficulty).ToList()
                        : grammars.OrderBy(g => g.Difficulty).ToList(),
                    "frequency" => sortDirection?.ToLower() == "desc"
                        ? grammars.OrderByDescending(g => g.Frequency).ToList()
                        : grammars.OrderBy(g => g.Frequency).ToList(),
                    _ => grammars.OrderBy(g => g.Title).ToList()
                };

                // Pagination
                var totalCount = grammars.Count;
                var items = grammars.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var result = new PaginatedResultDto<GrammarDto>
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
                _logger.LogError(ex, "Error getting grammars");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y chi ti?t ng? pháp
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<GrammarDto>> GetGrammar(int id)
        {
            try
            {
                var grammar = new GrammarDto
                {
                    GrammarId = id,
                    Title = "??/??? (desu/de aru)",
                    Pattern = "[Noun] + ??",
                    Level = "N5",
                    GrammarType = "Copula",
                    Meaning = "To be (polite/formal)",
                    Usage = "Used to make statements polite and formal. Essential for basic conversation.",
                    Formation = "Noun + ?? (polite) / Noun + ? (casual) / Noun + ??? (written/formal)",
                    Examples = new List<GrammarExampleDto>
                    {
                        new GrammarExampleDto
                        {
                            ExampleId = 1,
                            Japanese = "???????",
                            Romaji = "Watashi wa gakusei desu.",
                            English = "I am a student.",
                            Vietnamese = "Tôi là h?c sinh.",
                            Notes = "Polite form"
                        },
                        new GrammarExampleDto
                        {
                            ExampleId = 2,
                            Japanese = "??????",
                            Romaji = "Kare wa isha da.",
                            English = "He is a doctor.",
                            Vietnamese = "Anh ?y là bác s?.",
                            Notes = "Casual form"
                        }
                    },
                    Notes = "?? is the polite form of ?/???. Use ?? in formal situations and ? in casual conversations.",
                    RelatedGrammar = new List<string> { "?", "???", "????", "????" },
                    Difficulty = 1,
                    Frequency = 5,
                    Tags = new List<string> { "basic", "polite", "copula", "essential" },
                    AudioUrl = "/audio/grammar/desu.mp3",
                    VideoUrl = "/videos/grammar/desu-explanation.mp4",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    UpdatedAt = DateTime.UtcNow
                };

                return Ok(grammar);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting grammar {GrammarId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// T?o ng? pháp m?i
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<GrammarDto>> CreateGrammar([FromBody] CreateGrammarDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newGrammar = new GrammarDto
                {
                    GrammarId = new Random().Next(100, 999),
                    Title = createDto.Title,
                    Pattern = createDto.Pattern,
                    Level = createDto.Level,
                    GrammarType = createDto.GrammarType,
                    Meaning = createDto.Meaning,
                    Usage = createDto.Usage,
                    Formation = createDto.Formation,
                    Examples = createDto.Examples ?? new List<GrammarExampleDto>(),
                    Notes = createDto.Notes,
                    RelatedGrammar = createDto.RelatedGrammar ?? new List<string>(),
                    Difficulty = createDto.Difficulty,
                    Frequency = createDto.Frequency,
                    Tags = createDto.Tags ?? new List<string>(),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                return CreatedAtAction(nameof(GetGrammar), new { id = newGrammar.GrammarId }, newGrammar);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating grammar");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// C?p nh?t ng? pháp
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<GrammarDto>> UpdateGrammar(int id, [FromBody] UpdateGrammarDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedGrammar = new GrammarDto
                {
                    GrammarId = id,
                    Title = updateDto.Title ?? "Updated Grammar",
                    Pattern = updateDto.Pattern ?? "[Updated Pattern]",
                    Level = updateDto.Level ?? "N5",
                    GrammarType = updateDto.GrammarType ?? "General",
                    Meaning = updateDto.Meaning ?? "Updated meaning",
                    Usage = updateDto.Usage ?? "Updated usage",
                    Formation = updateDto.Formation ?? "Updated formation",
                    Examples = updateDto.Examples ?? new List<GrammarExampleDto>(),
                    Notes = updateDto.Notes ?? "",
                    RelatedGrammar = updateDto.RelatedGrammar ?? new List<string>(),
                    Difficulty = updateDto.Difficulty ?? 1,
                    Frequency = updateDto.Frequency ?? 3,
                    Tags = updateDto.Tags ?? new List<string>(),
                    IsActive = updateDto.IsActive ?? true,
                    UpdatedAt = DateTime.UtcNow
                };

                return Ok(updatedGrammar);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating grammar {GrammarId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Xóa ng? pháp
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGrammar(int id)
        {
            try
            {
                // TODO: Implement actual deletion logic (soft delete)
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting grammar {GrammarId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y ng? pháp theo c?p ??
        /// </summary>
        [HttpGet("by-level/{level}")]
        public async Task<ActionResult<PaginatedResultDto<GrammarDto>>> GetGrammarsByLevel(
            string level,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var grammars = GetMockGrammarsByLevel(level);

                var totalCount = grammars.Count;
                var items = grammars.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var result = new PaginatedResultDto<GrammarDto>
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
                _logger.LogError(ex, "Error getting grammars by level {Level}", level);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Tìm ki?m ng? pháp
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<PaginatedResultDto<GrammarDto>>> SearchGrammars(
            [FromQuery] string query,
            [FromQuery] string? searchType = "all", // all, title, pattern, meaning, usage
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var grammars = new List<GrammarDto>();

                if (!string.IsNullOrEmpty(query))
                {
                    grammars = searchType?.ToLower() switch
                    {
                        "title" => GetMockGrammarsByTitle(query),
                        "pattern" => GetMockGrammarsByPattern(query),
                        "meaning" => GetMockGrammarsByMeaning(query),
                        "usage" => GetMockGrammarsByUsage(query),
                        _ => GetMockGrammarsByAll(query)
                    };
                }

                var totalCount = grammars.Count;
                var items = grammars.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var result = new PaginatedResultDto<GrammarDto>
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
                _logger.LogError(ex, "Error searching grammars");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y ng? pháp ng?u nhiên
        /// </summary>
        [HttpGet("random")]
        public async Task<ActionResult<List<GrammarDto>>> GetRandomGrammars(
            [FromQuery] int count = 5,
            [FromQuery] string? level = null,
            [FromQuery] int? maxDifficulty = null)
        {
            try
            {
                var grammars = new List<GrammarDto>
                {
                    new GrammarDto
                    {
                        GrammarId = 1,
                        Title = "??",
                        Pattern = "[Noun] + ??",
                        Meaning = "To be (polite)",
                        Level = level ?? "N5",
                        Difficulty = maxDifficulty ?? 1,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                return Ok(grammars.Take(count).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting random grammars");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y th?ng kê ng? pháp
        /// </summary>
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetGrammarStatistics()
        {
            try
            {
                var statistics = new
                {
                    TotalGrammars = 500,
                    ByLevel = new
                    {
                        N5 = 100,
                        N4 = 120,
                        N3 = 130,
                        N2 = 100,
                        N1 = 50
                    },
                    ByType = new
                    {
                        Particles = 80,
                        Conjugations = 150,
                        Expressions = 100,
                        Patterns = 120,
                        Honorifics = 50
                    },
                    ByDifficulty = new
                    {
                        Level1 = 150,
                        Level2 = 120,
                        Level3 = 100,
                        Level4 = 80,
                        Level5 = 50
                    },
                    WithExamples = 480,
                    WithAudio = 300,
                    WithVideo = 150
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting grammar statistics");
                return StatusCode(500, "Internal server error");
            }
        }

        #region Private Helper Methods

        private List<GrammarDto> GetMockGrammarsByLevel(string level)
        {
            return new List<GrammarDto>
            {
                new GrammarDto
                {
                    GrammarId = 1,
                    Title = $"{level} Grammar Pattern",
                    Pattern = $"[{level} Pattern]",
                    Meaning = $"Grammar for {level} level",
                    Level = level,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
        }

        private List<GrammarDto> GetMockGrammarsByTitle(string query)
        {
            return new List<GrammarDto>
            {
                new GrammarDto
                {
                    GrammarId = 1,
                    Title = query,
                    Pattern = "[Pattern]",
                    Meaning = $"Grammar with title {query}",
                    Level = "N5",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
        }

        private List<GrammarDto> GetMockGrammarsByPattern(string query)
        {
            return new List<GrammarDto>
            {
                new GrammarDto
                {
                    GrammarId = 2,
                    Title = "Grammar Pattern",
                    Pattern = query,
                    Meaning = $"Grammar with pattern {query}",
                    Level = "N5",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
        }

        private List<GrammarDto> GetMockGrammarsByMeaning(string query)
        {
            return new List<GrammarDto>
            {
                new GrammarDto
                {
                    GrammarId = 3,
                    Title = "Grammar Meaning",
                    Pattern = "[Pattern]",
                    Meaning = query,
                    Level = "N5",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
        }

        private List<GrammarDto> GetMockGrammarsByUsage(string query)
        {
            return new List<GrammarDto>
            {
                new GrammarDto
                {
                    GrammarId = 4,
                    Title = "Grammar Usage",
                    Pattern = "[Pattern]",
                    Meaning = "Grammar meaning",
                    Usage = query,
                    Level = "N5",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
        }

        private List<GrammarDto> GetMockGrammarsByAll(string query)
        {
            return new List<GrammarDto>
            {
                new GrammarDto
                {
                    GrammarId = 5,
                    Title = $"Search result: {query}",
                    Pattern = $"[{query}]",
                    Meaning = $"Grammar for: {query}",
                    Level = "N5",
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
    /// DTO cho ng? pháp
    /// </summary>
    public class GrammarDto
    {
        public int GrammarId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Pattern { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string GrammarType { get; set; } = string.Empty;
        public string Meaning { get; set; } = string.Empty;
        public string Usage { get; set; } = string.Empty;
        public string Formation { get; set; } = string.Empty;
        public List<GrammarExampleDto> Examples { get; set; } = new List<GrammarExampleDto>();
        public string Notes { get; set; } = string.Empty;
        public List<string> RelatedGrammar { get; set; } = new List<string>();
        public int Difficulty { get; set; } = 1;
        public int Frequency { get; set; } = 3;
        public List<string> Tags { get; set; } = new List<string>();
        public string AudioUrl { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO cho ví d? ng? pháp
    /// </summary>
    public class GrammarExampleDto
    {
        public int ExampleId { get; set; }
        public string Japanese { get; set; } = string.Empty;
        public string Romaji { get; set; } = string.Empty;
        public string English { get; set; } = string.Empty;
        public string Vietnamese { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO cho t?o ng? pháp m?i
    /// </summary>
    public class CreateGrammarDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Pattern { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Level { get; set; } = string.Empty;

        [StringLength(50)]
        public string GrammarType { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Meaning { get; set; } = string.Empty;

        public string Usage { get; set; } = string.Empty;

        public string Formation { get; set; } = string.Empty;

        public List<GrammarExampleDto>? Examples { get; set; }

        public string Notes { get; set; } = string.Empty;

        public List<string>? RelatedGrammar { get; set; }

        [Range(1, 5)]
        public int Difficulty { get; set; } = 1;

        [Range(1, 5)]
        public int Frequency { get; set; } = 3;

        public List<string>? Tags { get; set; }
    }

    /// <summary>
    /// DTO cho c?p nh?t ng? pháp
    /// </summary>
    public class UpdateGrammarDto
    {
        [StringLength(200)]
        public string? Title { get; set; }

        [StringLength(200)]
        public string? Pattern { get; set; }

        [StringLength(10)]
        public string? Level { get; set; }

        [StringLength(50)]
        public string? GrammarType { get; set; }

        [StringLength(500)]
        public string? Meaning { get; set; }

        public string? Usage { get; set; }

        public string? Formation { get; set; }

        public List<GrammarExampleDto>? Examples { get; set; }

        public string? Notes { get; set; }

        public List<string>? RelatedGrammar { get; set; }

        [Range(1, 5)]
        public int? Difficulty { get; set; }

        [Range(1, 5)]
        public int? Frequency { get; set; }

        public List<string>? Tags { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion
}