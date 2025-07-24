using LexiFlow.API.DTOs.Learning;
using LexiFlow.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LexiFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LearningController : ControllerBase
    {
        private readonly ILearningService _learningService;
        private readonly ILogger<LearningController> _logger;

        public LearningController(ILearningService learningService, ILogger<LearningController> logger)
        {
            _learningService = learningService;
            _logger = logger;
        }

        /// <summary>
        /// Get the current user's vocabulary learning progress
        /// </summary>
        /// <param name="pageIndex">Page index (0-based)</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="search">Search term for vocabulary</param>
        /// <param name="level">JLPT level filter</param>
        /// <returns>Paginated list of vocabulary learning progress</returns>
        [HttpGet("progress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetLearningProgress(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? level = null)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _learningService.GetUserVocabularyProgressAsync(userId, pageIndex, pageSize, search, level);
                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving learning progress for user ID {UserId}", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { Success = false, Message = "Failed to retrieve learning progress", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get the current user's progress for a specific vocabulary
        /// </summary>
        /// <param name="vocabularyId">Vocabulary ID</param>
        /// <returns>Vocabulary learning progress</returns>
        [HttpGet("progress/{vocabularyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVocabularyProgress(int vocabularyId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _learningService.GetVocabularyProgressAsync(userId, vocabularyId);
                if (!response.Success)
                {
                    return response.Message.Contains("not found") ? NotFound(response) : BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vocabulary progress for vocabulary ID {VocabularyId}, user ID {UserId}",
                    vocabularyId, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { Success = false, Message = "Failed to retrieve vocabulary progress", Error = ex.Message });
            }
        }

        /// <summary>
        /// Update the current user's learning progress based on study session results
        /// </summary>
        /// <param name="studyResults">Study session results</param>
        /// <returns>Success status</returns>
        [HttpPost("progress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateLearningProgress([FromBody] StudySessionResultDto studyResults)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _learningService.UpdateLearningProgressAsync(userId, studyResults);
                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating learning progress for user ID {UserId}", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { Success = false, Message = "Failed to update learning progress", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get vocabularies due for review based on spaced repetition algorithm
        /// </summary>
        /// <param name="count">Number of vocabularies to return</param>
        /// <returns>List of due vocabularies</returns>
        [HttpGet("due")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetDueVocabularies([FromQuery] int count = 10)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _learningService.GetDueVocabulariesAsync(userId, count);
                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving due vocabularies for user ID {UserId}", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { Success = false, Message = "Failed to retrieve due vocabularies", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get the current user's personal word lists
        /// </summary>
        /// <param name="pageIndex">Page index (0-based)</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="search">Search term for list name or description</param>
        /// <returns>Paginated list of personal word lists</returns>
        [HttpGet("lists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetPersonalWordLists(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _learningService.GetPersonalWordListsAsync(userId, pageIndex, pageSize, search);
                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving personal word lists for user ID {UserId}", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { Success = false, Message = "Failed to retrieve personal word lists", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get a specific personal word list by ID
        /// </summary>
        /// <param name="listId">Word list ID</param>
        /// <returns>Personal word list details</returns>
        [HttpGet("lists/{listId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPersonalWordListById(int listId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _learningService.GetPersonalWordListByIdAsync(userId, listId);
                if (!response.Success)
                {
                    return response.Message.Contains("not found") ? NotFound(response) : BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving personal word list with ID {ListId} for user ID {UserId}",
                    listId, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { Success = false, Message = "Failed to retrieve personal word list", Error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new personal word list
        /// </summary>
        /// <param name="createDto">Word list creation details</param>
        /// <returns>Created personal word list</returns>
        [HttpPost("lists")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreatePersonalWordList([FromBody] CreatePersonalWordListDto createDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _learningService.CreatePersonalWordListAsync(userId, createDto);
                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return CreatedAtAction(nameof(GetPersonalWordListById), new { listId = response.Data?.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating personal word list for user ID {UserId}", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { Success = false, Message = "Failed to create personal word list", Error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing personal word list
        /// </summary>
        /// <param name="listId">Word list ID</param>
        /// <param name="updateDto">Word list update details</param>
        /// <returns>Updated personal word list</returns>
        [HttpPut("lists/{listId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePersonalWordList(int listId, [FromBody] UpdatePersonalWordListDto updateDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _learningService.UpdatePersonalWordListAsync(userId, listId, updateDto);
                if (!response.Success)
                {
                    return response.Message.Contains("not found") ? NotFound(response) : BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating personal word list with ID {ListId} for user ID {UserId}",
                    listId, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { Success = false, Message = "Failed to update personal word list", Error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a personal word list
        /// </summary>
        /// <param name="listId">Word list ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("lists/{listId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePersonalWordList(int listId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _learningService.DeletePersonalWordListAsync(userId, listId);
                if (!response.Success)
                {
                    return response.Message.Contains("not found") ? NotFound(response) : BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting personal word list with ID {ListId} for user ID {UserId}",
                    listId, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { Success = false, Message = "Failed to delete personal word list", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get items in a personal word list
        /// </summary>
        /// <param name="listId">Word list ID</param>
        /// <returns>List of vocabulary items in the personal word list</returns>
        [HttpGet("lists/{listId}/items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPersonalWordListItems(int listId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _learningService.GetPersonalWordListItemsAsync(userId, listId);
                if (!response.Success)
                {
                    return response.Message.Contains("not found") ? NotFound(response) : BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving items in personal word list with ID {ListId} for user ID {UserId}",
                    listId, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { Success = false, Message = "Failed to retrieve word list items", Error = ex.Message });
            }
        }

        /// <summary>
        /// Add a vocabulary to a personal word list
        /// </summary>
        /// <param name="listId">Word list ID</param>
        /// <param name="addDto">Add vocabulary details</param>
        /// <returns>Added vocabulary item</returns>
        [HttpPost("lists/{listId}/items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddWordToPersonalList(int listId, [FromBody] AddWordToListDto addDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _learningService.AddWordToPersonalListAsync(userId, listId, addDto);
                if (!response.Success)
                {
                    return response.Message.Contains("not found") ? NotFound(response) : BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding word to personal list with ID {ListId} for user ID {UserId}",
                    listId, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { Success = false, Message = "Failed to add word to personal list", Error = ex.Message });
            }
        }

        /// <summary>
        /// Remove a vocabulary from a personal word list
        /// </summary>
        /// <param name="listId">Word list ID</param>
        /// <param name="vocabularyId">Vocabulary ID to remove</param>
        /// <returns>Success status</returns>
        [HttpDelete("lists/{listId}/items/{vocabularyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveWordFromPersonalList(int listId, int vocabularyId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _learningService.RemoveWordFromPersonalListAsync(userId, listId, vocabularyId);
                if (!response.Success)
                {
                    return response.Message.Contains("not found") ? NotFound(response) : BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing vocabulary ID {VocabularyId} from personal list with ID {ListId} for user ID {UserId}",
                    vocabularyId, listId, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { Success = false, Message = "Failed to remove word from personal list", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get the current user's study plans
        /// </summary>
        /// <param name="pageIndex">Page index (0-based)</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="activeOnly">Filter for active plans only</param>
        /// <returns>Paginated list of study plans</returns>
        [HttpGet("plans")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetStudyPlans(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool activeOnly = false)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _learningService.GetUserStudyPlansAsync(userId, pageIndex, pageSize, activeOnly);
                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving study plans for user ID {UserId}", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { Success = false, Message = "Failed to retrieve study plans", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get a specific study plan by ID
        /// </summary>
        /// <param name="planId">Study plan ID</param>
        /// <returns>Study plan details</returns>
        [HttpGet("plans/{planId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudyPlanById(int planId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _learningService.GetStudyPlanByIdAsync(userId, planId);
                if (!response.Success)
                {
                    return response.Message.Contains("not found") ? NotFound(response) : BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving study plan with ID {PlanId} for user ID {UserId}",
                    planId, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { Success = false, Message = "Failed to retrieve study plan", Error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new study plan
        /// </summary>
        /// <param name="createDto">Study plan creation details</param>
        /// <returns>Created study plan</returns>
        [HttpPost("plans")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateStudyPlan([FromBody] CreateStudyPlanDto createDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _learningService.CreateStudyPlanAsync(userId, createDto);
                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return CreatedAtAction(nameof(GetStudyPlanById), new { planId = response.Data?.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating study plan for user ID {UserId}", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { Success = false, Message = "Failed to create study plan", Error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing study plan
        /// </summary>
        /// <param name="planId">Study plan ID</param>
        /// <param name="updateDto">Study plan update details</param>
        /// <returns>Updated study plan</returns>
        [HttpPut("plans/{planId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStudyPlan(int planId, [FromBody] CreateStudyPlanDto updateDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _learningService.UpdateStudyPlanAsync(userId, planId, updateDto);
                if (!response.Success)
                {
                    return response.Message.Contains("not found") ? NotFound(response) : BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating study plan with ID {PlanId} for user ID {UserId}",
                    planId, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { Success = false, Message = "Failed to update study plan", Error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a study plan
        /// </summary>
        /// <param name="planId">Study plan ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("plans/{planId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStudyPlan(int planId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _learningService.DeleteStudyPlanAsync(userId, planId);
                if (!response.Success)
                {
                    return response.Message.Contains("not found") ? NotFound(response) : BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting study plan with ID {PlanId} for user ID {UserId}",
                    planId, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { Success = false, Message = "Failed to delete study plan", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get study goals for a specific study plan
        /// </summary>
        /// <param name="planId">Study plan ID</param>
        /// <returns>List of study goals</returns>
        [HttpGet("plans/{planId}/goals")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudyPlanGoals(int planId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _learningService.GetStudyPlanGoalsAsync(userId, planId);
                if (!response.Success)
                {
                    return response.Message.Contains("not found") ? NotFound(response) : BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving goals for study plan with ID {PlanId} for user ID {UserId}",
                    planId, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { Success = false, Message = "Failed to retrieve study plan goals", Error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new study goal in a study plan
        /// </summary>
        /// <param name="planId">Study plan ID</param>
        /// <param name="createDto">Study goal creation details</param>
        /// <returns>Created study goal</returns>
        [HttpPost("plans/{planId}/goals")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateStudyGoal(int planId, [FromBody] CreateStudyGoalDto createDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _learningService.CreateStudyGoalAsync(userId, planId, createDto);
                if (!response.Success)
                {
                    return response.Message.Contains("not found") ? NotFound(response) : BadRequest(response);
                }

                return CreatedAtAction(nameof(GetStudyPlanGoals), new { planId }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating study goal in plan with ID {PlanId} for user ID {UserId}",
                    planId, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { Success = false, Message = "Failed to create study goal", Error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing study goal
        /// </summary>
        /// <param name="planId">Study plan ID</param>
        /// <param name="goalId">Study goal ID</param>
        /// <param name="updateDto">Study goal update details</param>
        /// <returns>Updated study goal</returns>
        [HttpPut("plans/{planId}/goals/{goalId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStudyGoal(int planId, int goalId, [FromBody] UpdateStudyGoalDto updateDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _learningService.UpdateStudyGoalAsync(userId, planId, goalId, updateDto);
                if (!response.Success)
                {
                    return response.Message.Contains("not found") ? NotFound(response) : BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating study goal with ID {GoalId} in plan with ID {PlanId} for user ID {UserId}",
                    goalId, planId, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { Success = false, Message = "Failed to update study goal", Error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a study goal
        /// </summary>
        /// <param name="planId">Study plan ID</param>
        /// <param name="goalId">Study goal ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("plans/{planId}/goals/{goalId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStudyGoal(int planId, int goalId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _learningService.DeleteStudyGoalAsync(userId, planId, goalId);
                if (!response.Success)
                {
                    return response.Message.Contains("not found") ? NotFound(response) : BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting study goal with ID {GoalId} in plan with ID {PlanId} for user ID {UserId}",
                    goalId, planId, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { Success = false, Message = "Failed to delete study goal", Error = ex.Message });
            }
        }
    }
}