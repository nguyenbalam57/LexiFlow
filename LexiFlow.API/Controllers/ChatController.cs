using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LexiFlow.API.DTOs.Common;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.Controllers
{
    /// <summary>
    /// Controller cho AI Assistant và Chat support
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly ILogger<ChatController> _logger;

        public ChatController(ILogger<ChatController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// G?i tin nh?n ??n AI Assistant
        /// </summary>
        [HttpPost("message")]
        public async Task<ActionResult<ChatResponseDto>> SendMessage([FromBody] ChatMessageDto message)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                // Simulate AI response based on message type
                var response = GenerateAIResponse(message);

                var chatResponse = new ChatResponseDto
                {
                    MessageId = Guid.NewGuid().ToString(),
                    ConversationId = message.ConversationId ?? Guid.NewGuid().ToString(),
                    Response = response,
                    ResponseType = DetermineResponseType(message.Message),
                    Timestamp = DateTime.UtcNow,
                    Confidence = 0.95f,
                    SuggestedActions = GenerateSuggestedActions(message.Message),
                    RelatedContent = GenerateRelatedContent(message.Message),
                    ProcessingTime = 850
                };

                return Ok(chatResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing chat message");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y l?ch s? chat
        /// </summary>
        [HttpGet("history")]
        public async Task<ActionResult<PaginatedResultDto<ChatHistoryDto>>> GetChatHistory(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? conversationId = null)
        {
            try
            {
                var userId = GetCurrentUserId();

                var history = new List<ChatHistoryDto>
                {
                    new ChatHistoryDto
                    {
                        MessageId = "msg-001",
                        ConversationId = "conv-001",
                        UserMessage = "How do I say 'good morning' in Japanese?",
                        AIResponse = "In Japanese, 'good morning' is ???? (ohayou) for casual situations, or ????????? (ohayou gozaimasu) for formal situations.",
                        Timestamp = DateTime.UtcNow.AddHours(-2),
                        MessageType = "Question",
                        WasHelpful = true,
                        UserRating = 5
                    },
                    new ChatHistoryDto
                    {
                        MessageId = "msg-002",
                        ConversationId = "conv-001",
                        UserMessage = "Can you explain the difference between ? and ??",
                        AIResponse = "? (wa) and ? (ga) are both particles, but they serve different functions. ? marks the topic of the sentence (what you're talking about), while ? marks the subject (who or what performs the action).",
                        Timestamp = DateTime.UtcNow.AddHours(-1),
                        MessageType = "Grammar",
                        WasHelpful = null,
                        UserRating = null
                    }
                };

                if (!string.IsNullOrEmpty(conversationId))
                {
                    history = history.Where(h => h.ConversationId == conversationId).ToList();
                }

                var totalItems = history.Count;
                var items = history
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var result = new PaginatedResultDto<ChatHistoryDto>
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
                _logger.LogError(ex, "Error getting chat history");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// ?ánh giá ph?n h?i c?a AI
        /// </summary>
        [HttpPost("feedback")]
        public async Task<ActionResult> ProvideFeedback([FromBody] ChatFeedbackDto feedback)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                // TODO: Store feedback in database

                return Ok(new { Message = "Feedback recorded successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording chat feedback");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y g?i ý câu h?i
        /// </summary>
        [HttpGet("suggestions")]
        public async Task<ActionResult<List<ChatSuggestionDto>>> GetSuggestions([FromQuery] string? context = null)
        {
            try
            {
                var userId = GetCurrentUserId();

                var suggestions = new List<ChatSuggestionDto>
                {
                    new ChatSuggestionDto
                    {
                        SuggestionId = 1,
                        Text = "How do I conjugate verbs in Japanese?",
                        Category = "Grammar",
                        Popularity = 95,
                        EstimatedResponseTime = 30
                    },
                    new ChatSuggestionDto
                    {
                        SuggestionId = 2,
                        Text = "What's the difference between Hiragana and Katakana?",
                        Category = "Writing System",
                        Popularity = 89,
                        EstimatedResponseTime = 45
                    },
                    new ChatSuggestionDto
                    {
                        SuggestionId = 3,
                        Text = "Can you help me practice counting in Japanese?",
                        Category = "Practice",
                        Popularity = 76,
                        EstimatedResponseTime = 60
                    },
                    new ChatSuggestionDto
                    {
                        SuggestionId = 4,
                        Text = "Explain Japanese honorific system",
                        Category = "Culture",
                        Popularity = 82,
                        EstimatedResponseTime = 120
                    }
                };

                if (!string.IsNullOrEmpty(context))
                {
                    suggestions = suggestions.Where(s => s.Category.Contains(context, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                return Ok(suggestions.OrderByDescending(s => s.Popularity).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting chat suggestions");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// B?t ??u cu?c trò chuy?n m?i
        /// </summary>
        [HttpPost("conversation")]
        public async Task<ActionResult<ConversationDto>> StartConversation([FromBody] StartConversationDto startConversation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                var conversation = new ConversationDto
                {
                    ConversationId = Guid.NewGuid().ToString(),
                    UserId = userId,
                    Title = startConversation.Title ?? "New Conversation",
                    Context = startConversation.Context,
                    StartedAt = DateTime.UtcNow,
                    LastActivity = DateTime.UtcNow,
                    MessageCount = 0,
                    Status = "Active",
                    Tags = startConversation.Tags
                };

                return CreatedAtAction(nameof(GetConversation), new { id = conversation.ConversationId }, conversation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting conversation");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y thông tin cu?c trò chuy?n
        /// </summary>
        [HttpGet("conversation/{id}")]
        public async Task<ActionResult<ConversationDetailsDto>> GetConversation(string id)
        {
            try
            {
                var userId = GetCurrentUserId();

                var conversation = new ConversationDetailsDto
                {
                    ConversationId = id,
                    UserId = userId,
                    Title = "Japanese Grammar Discussion",
                    Context = "Learning N4 Grammar",
                    StartedAt = DateTime.UtcNow.AddHours(-3),
                    LastActivity = DateTime.UtcNow.AddMinutes(-15),
                    MessageCount = 8,
                    Status = "Active",
                    Tags = new List<string> { "grammar", "n4", "learning" },
                    Messages = new List<ChatMessageHistoryDto>
                    {
                        new ChatMessageHistoryDto
                        {
                            MessageId = "msg-001",
                            Sender = "User",
                            Content = "Can you explain the ?-form?",
                            Timestamp = DateTime.UtcNow.AddHours(-3),
                            MessageType = "Question"
                        },
                        new ChatMessageHistoryDto
                        {
                            MessageId = "msg-002",
                            Sender = "AI",
                            Content = "The ?-form (te-form) is a fundamental verb conjugation in Japanese...",
                            Timestamp = DateTime.UtcNow.AddHours(-3).AddMinutes(1),
                            MessageType = "Response"
                        }
                    },
                    Summary = "Discussion about Japanese verb conjugations, specifically the ?-form and its usage patterns."
                };

                return Ok(conversation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting conversation {ConversationId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y th?ng kê chat
        /// </summary>
        [HttpGet("statistics")]
        public async Task<ActionResult<ChatStatisticsDto>> GetChatStatistics()
        {
            try
            {
                var userId = GetCurrentUserId();

                var statistics = new ChatStatisticsDto
                {
                    TotalConversations = 15,
                    TotalMessages = 127,
                    AverageResponseTime = 1.2f,
                    SatisfactionRating = 4.6f,
                    TopCategories = new Dictionary<string, int>
                    {
                        { "Grammar", 45 },
                        { "Vocabulary", 32 },
                        { "Pronunciation", 25 },
                        { "Culture", 18 },
                        { "Writing", 7 }
                    },
                    WeeklyUsage = new List<DailyUsageDto>
                    {
                        new DailyUsageDto { Date = DateTime.Today.AddDays(-6), MessageCount = 8 },
                        new DailyUsageDto { Date = DateTime.Today.AddDays(-5), MessageCount = 12 },
                        new DailyUsageDto { Date = DateTime.Today.AddDays(-4), MessageCount = 6 },
                        new DailyUsageDto { Date = DateTime.Today.AddDays(-3), MessageCount = 15 },
                        new DailyUsageDto { Date = DateTime.Today.AddDays(-2), MessageCount = 9 },
                        new DailyUsageDto { Date = DateTime.Today.AddDays(-1), MessageCount = 11 },
                        new DailyUsageDto { Date = DateTime.Today, MessageCount = 5 }
                    },
                    MostHelpfulResponses = new List<string>
                    {
                        "Explanation of particle usage",
                        "Verb conjugation patterns", 
                        "Kanji stroke order guidance"
                    },
                    RecentActivity = DateTime.UtcNow.AddMinutes(-15)
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting chat statistics");
                return StatusCode(500, "Internal server error");
            }
        }

        #region Private Methods

        private string GenerateAIResponse(ChatMessageDto message)
        {
            var responses = new Dictionary<string, string>
            {
                { "greeting", "??????????????????????????????????????????" },
                { "grammar", "???????????????????????????????????????" },
                { "vocabulary", "???????????????????????????????????" },
                { "pronunciation", "????????????????????????????????????" },
                { "kanji", "????????????????????????????????????????????????????" }
            };

            var lowerMessage = message.Message.ToLower();
            
            if (lowerMessage.Contains("hello") || lowerMessage.Contains("?????"))
                return responses["greeting"];
            if (lowerMessage.Contains("grammar") || lowerMessage.Contains("??"))
                return responses["grammar"];
            if (lowerMessage.Contains("vocabulary") || lowerMessage.Contains("??"))
                return responses["vocabulary"];
            if (lowerMessage.Contains("pronunciation") || lowerMessage.Contains("??"))
                return responses["pronunciation"];
            if (lowerMessage.Contains("kanji") || lowerMessage.Contains("??"))
                return responses["kanji"];

            return "????????????????????????????????????????????????????????????????????????????";
        }

        private string DetermineResponseType(string message)
        {
            var lowerMessage = message.ToLower();
            
            if (lowerMessage.Contains("grammar") || lowerMessage.Contains("??"))
                return "Grammar";
            if (lowerMessage.Contains("vocabulary") || lowerMessage.Contains("??"))
                return "Vocabulary";
            if (lowerMessage.Contains("pronunciation") || lowerMessage.Contains("??"))
                return "Pronunciation";
            if (lowerMessage.Contains("kanji") || lowerMessage.Contains("??"))
                return "Kanji";
            if (lowerMessage.Contains("culture") || lowerMessage.Contains("??"))
                return "Culture";
                
            return "General";
        }

        private List<SuggestedActionDto> GenerateSuggestedActions(string message)
        {
            return new List<SuggestedActionDto>
            {
                new SuggestedActionDto
                {
                    ActionType = "Practice",
                    Title = "???????",
                    Description = "?????????????????",
                    ActionUrl = "/practice"
                },
                new SuggestedActionDto
                {
                    ActionType = "Study",
                    Title = "??????",
                    Description = "?????????????????",
                    ActionUrl = "/lessons"
                }
            };
        }

        private List<RelatedContentDto> GenerateRelatedContent(string message)
        {
            return new List<RelatedContentDto>
            {
                new RelatedContentDto
                {
                    ContentType = "Lesson",
                    Title = "????????",
                    Description = "?????????????????",
                    ContentUrl = "/lessons/basic-grammar",
                    Difficulty = "Beginner"
                },
                new RelatedContentDto
                {
                    ContentType = "Vocabulary",
                    Title = "???????",
                    Description = "???????????",
                    ContentUrl = "/vocabulary/daily-conversation",
                    Difficulty = "Beginner"
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

    public class ChatMessageDto
    {
        [Required]
        public string Message { get; set; } = string.Empty;
        public string? ConversationId { get; set; }
        public string MessageType { get; set; } = "Question";
        public string? Context { get; set; }
        public Dictionary<string, object>? Metadata { get; set; }
    }

    public class ChatResponseDto
    {
        public string MessageId { get; set; } = string.Empty;
        public string ConversationId { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty;
        public string ResponseType { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public float Confidence { get; set; }
        public List<SuggestedActionDto> SuggestedActions { get; set; } = new List<SuggestedActionDto>();
        public List<RelatedContentDto> RelatedContent { get; set; } = new List<RelatedContentDto>();
        public int ProcessingTime { get; set; }
    }

    public class SuggestedActionDto
    {
        public string ActionType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ActionUrl { get; set; } = string.Empty;
    }

    public class RelatedContentDto
    {
        public string ContentType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ContentUrl { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
    }

    public class ChatHistoryDto
    {
        public string MessageId { get; set; } = string.Empty;
        public string ConversationId { get; set; } = string.Empty;
        public string UserMessage { get; set; } = string.Empty;
        public string AIResponse { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string MessageType { get; set; } = string.Empty;
        public bool? WasHelpful { get; set; }
        public int? UserRating { get; set; }
    }

    public class ChatFeedbackDto
    {
        [Required]
        public string MessageId { get; set; } = string.Empty;
        [Range(1, 5)]
        public int Rating { get; set; }
        public bool WasHelpful { get; set; }
        public string? Feedback { get; set; }
        public List<string>? ImprovementSuggestions { get; set; }
    }

    public class ChatSuggestionDto
    {
        public int SuggestionId { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Popularity { get; set; }
        public int EstimatedResponseTime { get; set; }
    }

    public class StartConversationDto
    {
        public string? Title { get; set; }
        public string? Context { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }

    public class ConversationDto
    {
        public string ConversationId { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Context { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime LastActivity { get; set; }
        public int MessageCount { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
    }

    public class ConversationDetailsDto : ConversationDto
    {
        public List<ChatMessageHistoryDto> Messages { get; set; } = new List<ChatMessageHistoryDto>();
        public string Summary { get; set; } = string.Empty;
    }

    public class ChatMessageHistoryDto
    {
        public string MessageId { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string MessageType { get; set; } = string.Empty;
    }

    public class ChatStatisticsDto
    {
        public int TotalConversations { get; set; }
        public int TotalMessages { get; set; }
        public float AverageResponseTime { get; set; }
        public float SatisfactionRating { get; set; }
        public Dictionary<string, int> TopCategories { get; set; } = new Dictionary<string, int>();
        public List<DailyUsageDto> WeeklyUsage { get; set; } = new List<DailyUsageDto>();
        public List<string> MostHelpfulResponses { get; set; } = new List<string>();
        public DateTime RecentActivity { get; set; }
    }

    public class DailyUsageDto
    {
        public DateTime Date { get; set; }
        public int MessageCount { get; set; }
    }

    #endregion
}