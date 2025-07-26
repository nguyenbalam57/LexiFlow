using LexiFlow.API.DTOs.WordList;
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
    public class PersonalWordListController : ControllerBase
    {
        private readonly IPersonalWordListService _wordListService;
        private readonly ILogger<PersonalWordListController> _logger;

        public PersonalWordListController(
            IPersonalWordListService wordListService,
            ILogger<PersonalWordListController> logger)
        {
            _wordListService = wordListService;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách tất cả danh sách từ vựng cá nhân
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllWordLists()
        {
            try
            {
                var userId = GetUserId();
                var wordLists = await _wordListService.GetAllAsync(userId);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Personal word lists retrieved successfully",
                    Data = wordLists
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving personal word lists for user ID: {UserId}", GetUserId());
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving personal word lists"
                });
            }
        }

        /// <summary>
        /// Lấy thông tin danh sách từ vựng cá nhân theo ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetWordListById(int id)
        {
            try
            {
                var userId = GetUserId();
                var wordList = await _wordListService.GetByIdAsync(id, userId);

                if (wordList == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Personal word list not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Personal word list retrieved successfully",
                    Data = wordList
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving personal word list with ID: {Id}, user ID: {UserId}",
                    id, GetUserId());
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving personal word list"
                });
            }
        }

        /// <summary>
        /// Tạo mới danh sách từ vựng cá nhân
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateWordList([FromBody] CreatePersonalWordListDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid word list data",
                        Data = ModelState
                    });
                }

                var userId = GetUserId();
                var createdWordList = await _wordListService.CreateAsync(dto, userId);

                return CreatedAtAction(nameof(GetWordListById), new { id = createdWordList.ListID }, new ApiResponse
                {
                    Success = true,
                    Message = "Personal word list created successfully",
                    Data = createdWordList
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating personal word list for user ID: {UserId}", GetUserId());
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while creating personal word list"
                });
            }
        }

        /// <summary>
        /// Cập nhật danh sách từ vựng cá nhân
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateWordList(int id, [FromBody] UpdatePersonalWordListDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid word list data",
                        Data = ModelState
                    });
                }

                var userId = GetUserId();
                var updatedWordList = await _wordListService.UpdateAsync(id, dto, userId);

                if (updatedWordList == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Personal word list not found or could not be updated"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Personal word list updated successfully",
                    Data = updatedWordList
                });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Concurrency conflict when updating personal word list {Id}", id);
                return Conflict(new ApiResponse
                {
                    Success = false,
                    Message = "The word list has been modified by another user. Please refresh and try again."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating personal word list with ID: {Id}, user ID: {UserId}",
                    id, GetUserId());
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while updating personal word list"
                });
            }
        }

        /// <summary>
        /// Xóa danh sách từ vựng cá nhân
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteWordList(int id)
        {
            try
            {
                var userId = GetUserId();
                var result = await _wordListService.DeleteAsync(id, userId);

                if (!result)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Personal word list not found or could not be deleted"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Personal word list deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting personal word list with ID: {Id}, user ID: {UserId}",
                    id, GetUserId());
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while deleting personal word list"
                });
            }
        }

        /// <summary>
        /// Lấy các từ vựng trong danh sách
        /// </summary>
        [HttpGet("{id}/items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetWordListItems(int id)
        {
            try
            {
                var userId = GetUserId();
                var items = await _wordListService.GetListItemsAsync(id, userId);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Word list items retrieved successfully",
                    Data = items
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving items for word list ID: {ListId}, user ID: {UserId}",
                    id, GetUserId());
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving word list items"
                });
            }
        }

        /// <summary>
        /// Thêm từ vựng vào danh sách
        /// </summary>
        [HttpPost("{id}/items")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddWordToList(int id, [FromBody] AddWordToListDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid data",
                        Data = ModelState
                    });
                }

                var userId = GetUserId();
                var item = await _wordListService.AddItemAsync(id, dto, userId);

                if (item == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Word list not found or vocabulary not found"
                    });
                }

                return CreatedAtAction(nameof(GetWordListItems), new { id }, new ApiResponse
                {
                    Success = true,
                    Message = "Vocabulary added to list successfully",
                    Data = item
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding vocabulary ID: {VocabularyId} to word list ID: {ListId}, user ID: {UserId}",
                    dto.VocabularyID, id, GetUserId());
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while adding vocabulary to list"
                });
            }
        }

        /// <summary>
        /// Thêm nhiều từ vựng vào danh sách
        /// </summary>
        [HttpPost("{id}/items/bulk")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddMultipleWordsToList(int id, [FromBody] AddMultipleWordsToListDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid data",
                        Data = ModelState
                    });
                }

                var userId = GetUserId();
                var items = await _wordListService.AddItemsAsync(id, dto, userId);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = $"Added {items.Count()} vocabularies to list successfully",
                    Data = items
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding multiple vocabularies to word list ID: {ListId}, user ID: {UserId}",
                    id, GetUserId());
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while adding vocabularies to list"
                });
            }
        }

        /// <summary>
        /// Xóa từ vựng khỏi danh sách
        /// </summary>
        [HttpDelete("{listId}/items/{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveWordFromList(int listId, int itemId)
        {
            try
            {
                var userId = GetUserId();
                var result = await _wordListService.RemoveItemAsync(listId, itemId, userId);

                if (!result)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Word list item not found or could not be removed"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Vocabulary removed from list successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item ID: {ItemId} from word list ID: {ListId}, user ID: {UserId}",
                    itemId, listId, GetUserId());
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while removing vocabulary from list"
                });
            }
        }

        /// <summary>
        /// Xóa từ vựng khỏi danh sách theo ID từ vựng
        /// </summary>
        [HttpDelete("{listId}/vocabulary/{vocabularyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveVocabularyFromList(int listId, int vocabularyId)
        {
            try
            {
                var userId = GetUserId();
                var result = await _wordListService.RemoveItemByVocabularyIdAsync(listId, vocabularyId, userId);

                if (!result)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Vocabulary not found in list or could not be removed"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Vocabulary removed from list successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing vocabulary ID: {VocabularyId} from word list ID: {ListId}, user ID: {UserId}",
                    vocabularyId, listId, GetUserId());
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while removing vocabulary from list"
                });
            }
        }

        /// <summary>
        /// Xóa tất cả từ vựng khỏi danh sách
        /// </summary>
        [HttpDelete("{listId}/items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ClearWordList(int listId)
        {
            try
            {
                var userId = GetUserId();
                var result = await _wordListService.ClearListAsync(listId, userId);

                if (!result)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Word list not found or could not be cleared"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Word list cleared successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing word list ID: {ListId}, user ID: {UserId}",
                    listId, GetUserId());
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while clearing word list"
                });
            }
        }

        /// <summary>
        /// Kiểm tra từ vựng có trong danh sách hay không
        /// </summary>
        [HttpGet("{listId}/check/{vocabularyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CheckVocabularyInList(int listId, int vocabularyId)
        {
            try
            {
                var userId = GetUserId();
                var isInList = await _wordListService.IsVocabularyInListAsync(listId, vocabularyId, userId);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = isInList ? "Vocabulary is in the list" : "Vocabulary is not in the list",
                    Data = new { IsInList = isInList }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking vocabulary ID: {VocabularyId} in word list ID: {ListId}, user ID: {UserId}",
                    vocabularyId, listId, GetUserId());
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while checking vocabulary in list"
                });
            }
        }

        /// <summary>
        /// Lấy danh sách từ vựng cá nhân chứa từ vựng này
        /// </summary>
        [HttpGet("containing/{vocabularyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetListsContainingVocabulary(int vocabularyId)
        {
            try
            {
                var userId = GetUserId();
                var lists = await _wordListService.GetListsContainingVocabularyAsync(vocabularyId, userId);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Lists containing vocabulary retrieved successfully",
                    Data = lists
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving lists containing vocabulary ID: {VocabularyId}, user ID: {UserId}",
                    vocabularyId, GetUserId());
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving lists containing vocabulary"
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
