using LexiFlow.API.Data.Entities;
using LexiFlow.API.Data.Repositories;
using LexiFlow.API.Data.UnitOfWork;
using LexiFlow.API.DTOs;
using LexiFlow.API.DTOs.Learning;
using LexiFlow.API.DTOs.Vocabulary;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LexiFlow.API.Services
{

    public class LearningService : ILearningService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVocabularyService _vocabularyService;

        public LearningService(IUnitOfWork unitOfWork, IVocabularyService vocabularyService)
        {
            _unitOfWork = unitOfWork;
            _vocabularyService = vocabularyService;
        }

        public async Task<ApiResponse<PagedResponseDto<LearningProgressDto>>> GetUserVocabularyProgressAsync(int userId, int pageIndex = 0, int pageSize = 10, string? search = null, string? level = null)
        {
            try
            {
                // Verify user exists
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<PagedResponseDto<LearningProgressDto>>.Fail("User not found");
                }

                // Build query
                var query = _unitOfWork.LearningProgresses.FindAsync(lp => lp.UserID == userId);
                var progressList = await query;

                // Filter by search term
                if (!string.IsNullOrEmpty(search))
                {
                    // Get vocabulary IDs matching search
                    var vocabularies = await _unitOfWork.Vocabularies.FindAsync(v =>
                        v.Japanese.Contains(search) ||
                        v.Kana.Contains(search) ||
                        v.Romaji.Contains(search) ||
                        v.Vietnamese.Contains(search) ||
                        v.English.Contains(search));

                    var vocabIds = vocabularies.Select(v => v.VocabularyID).ToList();

                    // Filter progress by vocabulary IDs
                    progressList = progressList.Where(lp => vocabIds.Contains(lp.VocabularyID)).ToList();
                }

                // Filter by level
                if (!string.IsNullOrEmpty(level))
                {
                    // Get vocabulary IDs matching level
                    var vocabularies = await _unitOfWork.Vocabularies.FindAsync(v => v.Level == level);
                    var vocabIds = vocabularies.Select(v => v.VocabularyID).ToList();

                    // Filter progress by vocabulary IDs
                    progressList = progressList.Where(lp => vocabIds.Contains(lp.VocabularyID)).ToList();
                }

                // Sort by next review date
                progressList = progressList.OrderBy(lp => lp.NextReviewDate ?? DateTime.MaxValue).ToList();

                // Apply pagination
                var totalCount = progressList.Count();
                var pagedList = progressList.Skip(pageIndex * pageSize).Take(pageSize).ToList();

                // Get vocabulary details
                var vocabularyDtos = new List<VocabularyDto>();
                foreach (var progress in pagedList)
                {
                    var vocabResponse = await _vocabularyService.GetVocabularyByIdAsync(progress.VocabularyID);
                    if (vocabResponse.Success && vocabResponse.Data != null)
                    {
                        vocabularyDtos.Add(vocabResponse.Data);
                    }
                }

                // Create progress DTOs
                var progressDtos = new List<LearningProgressDto>();
                foreach (var progress in pagedList)
                {
                    var vocabularyDto = vocabularyDtos.FirstOrDefault(v => v.Id == progress.VocabularyID);

                    progressDtos.Add(new LearningProgressDto
                    {
                        Id = progress.ProgressID,
                        UserID = progress.UserID,
                        VocabularyID = progress.VocabularyID,
                        Vocabulary = vocabularyDto,
                        StudyCount = progress.StudyCount,
                        CorrectCount = progress.CorrectCount,
                        IncorrectCount = progress.IncorrectCount,
                        LastStudied = progress.LastStudied,
                        MemoryStrength = progress.MemoryStrength,
                        NextReviewDate = progress.NextReviewDate,
                        CreatedAt = progress.CreatedAt,
                        UpdatedAt = progress.UpdatedAt
                    });
                }

                // Create response
                var response = new PagedResponseDto<LearningProgressDto>
                {
                    Items = progressDtos,
                    TotalCount = totalCount,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                return ApiResponse<PagedResponseDto<LearningProgressDto>>.CreateSuccess(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponseDto<LearningProgressDto>>.Fail($"Error retrieving vocabulary progress: {ex.Message}");
            }
        }

        public async Task<ApiResponse<LearningProgressDto>> GetVocabularyProgressAsync(int userId, int vocabularyId)
        {
            try
            {
                // Verify user exists
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<LearningProgressDto>.Fail("User not found");
                }

                // Verify vocabulary exists
                var vocabulary = await _unitOfWork.Vocabularies.GetByIdAsync(vocabularyId);
                if (vocabulary == null)
                {
                    return ApiResponse<LearningProgressDto>.Fail("Vocabulary not found");
                }

                // Get learning progress
                var progress = await _unitOfWork.LearningProgresses.GetFirstOrDefaultAsync(lp =>
                    lp.UserID == userId && lp.VocabularyID == vocabularyId);

                // If no progress found, create new progress entry
                if (progress == null)
                {
                    progress = new LearningProgress
                    {
                        UserID = userId,
                        VocabularyID = vocabularyId,
                        StudyCount = 0,
                        CorrectCount = 0,
                        IncorrectCount = 0,
                        MemoryStrength = 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _unitOfWork.LearningProgresses.AddAsync(progress);
                    await _unitOfWork.SaveChangesAsync();
                }

                // Get vocabulary details
                var vocabResponse = await _vocabularyService.GetVocabularyByIdAsync(vocabularyId);
                VocabularyDto? vocabularyDto = null;
                if (vocabResponse.Success && vocabResponse.Data != null)
                {
                    vocabularyDto = vocabResponse.Data;
                }

                // Create response
                var progressDto = new LearningProgressDto
                {
                    Id = progress.ProgressID,
                    UserID = progress.UserID,
                    VocabularyID = progress.VocabularyID,
                    Vocabulary = vocabularyDto,
                    StudyCount = progress.StudyCount,
                    CorrectCount = progress.CorrectCount,
                    IncorrectCount = progress.IncorrectCount,
                    LastStudied = progress.LastStudied,
                    MemoryStrength = progress.MemoryStrength,
                    NextReviewDate = progress.NextReviewDate,
                    CreatedAt = progress.CreatedAt,
                    UpdatedAt = progress.UpdatedAt
                };

                return ApiResponse<LearningProgressDto>.CreateSuccess(progressDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<LearningProgressDto>.Fail($"Error retrieving vocabulary progress: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> UpdateLearningProgressAsync(int userId, StudySessionResultDto studyResults)
        {
            try
            {
                // Verify user exists
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<bool>.Fail("User not found");
                }

                // Check if user ID matches
                if (userId != studyResults.UserID)
                {
                    return ApiResponse<bool>.Fail("User ID mismatch");
                }

                // Process each vocabulary result
                foreach (var result in studyResults.Results)
                {
                    // Verify vocabulary exists
                    var vocabulary = await _unitOfWork.Vocabularies.GetByIdAsync(result.VocabularyID);
                    if (vocabulary == null)
                    {
                        continue; // Skip invalid vocabulary
                    }

                    // Get existing progress or create new entry
                    var progress = await _unitOfWork.LearningProgresses.GetFirstOrDefaultAsync(lp =>
                        lp.UserID == userId && lp.VocabularyID == result.VocabularyID);

                    if (progress == null)
                    {
                        progress = new LearningProgress
                        {
                            UserID = userId,
                            VocabularyID = result.VocabularyID,
                            StudyCount = 0,
                            CorrectCount = 0,
                            IncorrectCount = 0,
                            MemoryStrength = 0,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        await _unitOfWork.LearningProgresses.AddAsync(progress);
                    }

                    // Update progress
                    progress.StudyCount += 1;
                    if (result.IsCorrect)
                    {
                        progress.CorrectCount += 1;
                        // Increase memory strength (max 100)
                        progress.MemoryStrength = Math.Min(100, progress.MemoryStrength + 10);
                    }
                    else
                    {
                        progress.IncorrectCount += 1;
                        // Decrease memory strength (min 0)
                        progress.MemoryStrength = Math.Max(0, progress.MemoryStrength - 5);
                    }

                    // Update review timestamp
                    progress.LastStudied = DateTime.UtcNow;

                    // Calculate next review date based on spaced repetition algorithm
                    progress.NextReviewDate = CalculateNextReviewDate(progress.MemoryStrength);

                    progress.UpdatedAt = DateTime.UtcNow;

                    // Update progress
                    _unitOfWork.LearningProgresses.Update(progress);
                }

                // Save all changes
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.CreateSuccess(true, "Learning progress updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"Error updating learning progress: {ex.Message}");
            }
        }

        public async Task<ApiResponse<IEnumerable<VocabularyDto>>> GetDueVocabulariesAsync(int userId, int count = 10)
        {
            try
            {
                // Verify user exists
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<IEnumerable<VocabularyDto>>.Fail("User not found");
                }

                // Get due vocabularies
                var now = DateTime.UtcNow;
                var dueProgress = await _unitOfWork.LearningProgresses.FindAsync(lp =>
                    lp.UserID == userId && (lp.NextReviewDate == null || lp.NextReviewDate <= now));

                // Sort by priority:
                // 1. Never studied (NextReviewDate is null)
                // 2. Lowest memory strength
                // 3. Earliest due date
                var sortedProgress = dueProgress
                    .OrderBy(lp => lp.NextReviewDate.HasValue) // Null values first
                    .ThenBy(lp => lp.MemoryStrength)
                    .ThenBy(lp => lp.NextReviewDate)
                    .Take(count)
                    .ToList();

                // Get vocabulary details
                var vocabularyDtos = new List<VocabularyDto>();
                foreach (var progress in sortedProgress)
                {
                    var vocabResponse = await _vocabularyService.GetVocabularyByIdAsync(progress.VocabularyID);
                    if (vocabResponse.Success && vocabResponse.Data != null)
                    {
                        vocabularyDtos.Add(vocabResponse.Data);
                    }
                }

                // If not enough due vocabularies, add new ones that haven't been studied yet
                if (vocabularyDtos.Count < count)
                {
                    // Get vocabulary IDs that user has already studied
                    var studiedVocabIds = dueProgress.Select(lp => lp.VocabularyID).ToList();

                    // Get new vocabularies that user hasn't studied yet
                    var newVocabs = await _unitOfWork.Vocabularies.GetPagedAsync(
                        filter: v => !studiedVocabIds.Contains(v.VocabularyID),
                        orderBy: q => q.OrderBy(v => v.VocabularyID),
                        pageSize: count - vocabularyDtos.Count);

                    foreach (var vocab in newVocabs.Items)
                    {
                        var vocabResponse = await _vocabularyService.GetVocabularyByIdAsync(vocab.VocabularyID);
                        if (vocabResponse.Success && vocabResponse.Data != null)
                        {
                            vocabularyDtos.Add(vocabResponse.Data);
                        }
                    }
                }

                return ApiResponse<IEnumerable<VocabularyDto>>.CreateSuccess(vocabularyDtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<VocabularyDto>>.Fail($"Error retrieving due vocabularies: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PagedResponseDto<PersonalWordListDto>>> GetPersonalWordListsAsync(int userId, int pageIndex = 0, int pageSize = 10, string? search = null)
        {
            try
            {
                // Verify user exists
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<PagedResponseDto<PersonalWordListDto>>.Fail("User not found");
                }

                // Build filter
                var filter = PredicateBuilder.True<PersonalWordList>().And(l => l.UserID == userId);

                if (!string.IsNullOrEmpty(search))
                {
                    filter = filter.And(l => l.ListName.Contains(search) || l.Description.Contains(search));
                }

                // Get paged result
                var result = await _unitOfWork.PersonalWordLists.GetPagedAsync(
                    filter: filter,
                    orderBy: q => q.OrderBy(l => l.ListName),
                    pageIndex: pageIndex,
                    pageSize: pageSize);

                // Map to DTOs
                var listDtos = new List<PersonalWordListDto>();
                foreach (var list in result.Items)
                {
                    // Count items
                    var itemCount = await _unitOfWork.PersonalWordListItems.CountAsync(i => i.ListID == list.ListID);

                    listDtos.Add(new PersonalWordListDto
                    {
                        Id = list.ListID,
                        ListName = list.ListName,
                        Description = list.Description,
                        UserID = list.UserID,
                        ItemCount = itemCount,
                        CreatedAt = list.CreatedAt,
                        UpdatedAt = list.UpdatedAt
                    });
                }

                // Create response
                var response = new PagedResponseDto<PersonalWordListDto>
                {
                    Items = listDtos,
                    TotalCount = result.TotalCount,
                    PageIndex = result.PageIndex,
                    PageSize = result.PageSize
                };

                return ApiResponse<PagedResponseDto<PersonalWordListDto>>.CreateSuccess(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponseDto<PersonalWordListDto>>.Fail($"Error retrieving personal word lists: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PersonalWordListDto>> GetPersonalWordListByIdAsync(int userId, int listId)
        {
            try
            {
                // Verify user exists
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<PersonalWordListDto>.Fail("User not found");
                }

                // Get word list
                var list = await _unitOfWork.PersonalWordLists.GetFirstOrDefaultAsync(l =>
                    l.ListID == listId && l.UserID == userId);

                if (list == null)
                {
                    return ApiResponse<PersonalWordListDto>.Fail("Personal word list not found");
                }

                // Count items
                var itemCount = await _unitOfWork.PersonalWordListItems.CountAsync(i => i.ListID == list.ListID);

                // Create response
                var listDto = new PersonalWordListDto
                {
                    Id = list.ListID,
                    ListName = list.ListName,
                    Description = list.Description,
                    UserID = list.UserID,
                    ItemCount = itemCount,
                    CreatedAt = list.CreatedAt,
                    UpdatedAt = list.UpdatedAt
                };

                return ApiResponse<PersonalWordListDto>.CreateSuccess(listDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<PersonalWordListDto>.Fail($"Error retrieving personal word list: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PersonalWordListDto>> CreatePersonalWordListAsync(int userId, CreatePersonalWordListDto createDto)
        {
            try
            {
                // Verify user exists
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<PersonalWordListDto>.Fail("User not found");
                }

                // Check if list name already exists for this user
                var existingList = await _unitOfWork.PersonalWordLists.GetFirstOrDefaultAsync(l =>
                    l.UserID == userId && l.ListName == createDto.ListName);

                if (existingList != null)
                {
                    return ApiResponse<PersonalWordListDto>.Fail("A list with this name already exists");
                }

                // Create new list
                var list = new PersonalWordList
                {
                    ListName = createDto.ListName,
                    Description = createDto.Description,
                    UserID = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.PersonalWordLists.AddAsync(list);
                await _unitOfWork.SaveChangesAsync();

                // Create response
                var listDto = new PersonalWordListDto
                {
                    Id = list.ListID,
                    ListName = list.ListName,
                    Description = list.Description,
                    UserID = list.UserID,
                    ItemCount = 0,
                    CreatedAt = list.CreatedAt,
                    UpdatedAt = list.UpdatedAt
                };

                return ApiResponse<PersonalWordListDto>.CreateSuccess(listDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<PersonalWordListDto>.Fail($"Error creating personal word list: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PersonalWordListDto>> UpdatePersonalWordListAsync(int userId, int listId, UpdatePersonalWordListDto updateDto)
        {
            try
            {
                // Verify user exists
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<PersonalWordListDto>.Fail("User not found");
                }

                // Get word list
                var list = await _unitOfWork.PersonalWordLists.GetFirstOrDefaultAsync(l =>
                    l.ListID == listId && l.UserID == userId);

                if (list == null)
                {
                    return ApiResponse<PersonalWordListDto>.Fail("Personal word list not found");
                }

                // Check if new name already exists for this user
                if (!string.IsNullOrEmpty(updateDto.ListName) && updateDto.ListName != list.ListName)
                {
                    var existingList = await _unitOfWork.PersonalWordLists.GetFirstOrDefaultAsync(l =>
                        l.UserID == userId && l.ListName == updateDto.ListName);

                    if (existingList != null)
                    {
                        return ApiResponse<PersonalWordListDto>.Fail("A list with this name already exists");
                    }
                }

                // Update properties
                if (!string.IsNullOrEmpty(updateDto.ListName))
                    list.ListName = updateDto.ListName;

                if (updateDto.Description != null)
                    list.Description = updateDto.Description;

                list.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.PersonalWordLists.Update(list);
                await _unitOfWork.SaveChangesAsync();

                // Count items
                var itemCount = await _unitOfWork.PersonalWordListItems.CountAsync(i => i.ListID == list.ListID);

                // Create response
                var listDto = new PersonalWordListDto
                {
                    Id = list.ListID,
                    ListName = list.ListName,
                    Description = list.Description,
                    UserID = list.UserID,
                    ItemCount = itemCount,
                    CreatedAt = list.CreatedAt,
                    UpdatedAt = list.UpdatedAt
                };

                return ApiResponse<PersonalWordListDto>.CreateSuccess(listDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<PersonalWordListDto>.Fail($"Error updating personal word list: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeletePersonalWordListAsync(int userId, int listId)
        {
            try
            {
                // Verify user exists
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<bool>.Fail("User not found");
                }

                // Get word list
                var list = await _unitOfWork.PersonalWordLists.GetFirstOrDefaultAsync(l =>
                    l.ListID == listId && l.UserID == userId);

                if (list == null)
                {
                    return ApiResponse<bool>.Fail("Personal word list not found");
                }

                // Remove list items
                var items = await _unitOfWork.PersonalWordListItems.FindAsync(i => i.ListID == listId);
                if (items.Any())
                {
                    _unitOfWork.PersonalWordListItems.RemoveRange(items);
                    await _unitOfWork.SaveChangesAsync();
                }

                // Remove list
                _unitOfWork.PersonalWordLists.Remove(list);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.CreateSuccess(true, "Personal word list deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"Error deleting personal word list: {ex.Message}");
            }
        }

        public async Task<ApiResponse<IEnumerable<PersonalWordListItemDto>>> GetPersonalWordListItemsAsync(int userId, int listId)
        {
            try
            {
                // Verify user exists
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<IEnumerable<PersonalWordListItemDto>>.Fail("User not found");
                }

                // Get word list
                var list = await _unitOfWork.PersonalWordLists.GetFirstOrDefaultAsync(l =>
                    l.ListID == listId && l.UserID == userId);

                if (list == null)
                {
                    return ApiResponse<IEnumerable<PersonalWordListItemDto>>.Fail("Personal word list not found");
                }

                // Get list items
                var items = await _unitOfWork.PersonalWordListItems.FindAsync(i => i.ListID == listId);

                // Get vocabulary details
                var itemDtos = new List<PersonalWordListItemDto>();
                foreach (var item in items)
                {
                    var vocabResponse = await _vocabularyService.GetVocabularyByIdAsync(item.VocabularyID);
                    VocabularyDto? vocabularyDto = null;
                    if (vocabResponse.Success && vocabResponse.Data != null)
                    {
                        vocabularyDto = vocabResponse.Data;
                    }

                    itemDtos.Add(new PersonalWordListItemDto
                    {
                        ItemID = item.ItemID,
                        ListID = item.ListID,
                        VocabularyID = item.VocabularyID,
                        Vocabulary = vocabularyDto,
                        AddedAt = item.AddedAt
                    });
                }

                return ApiResponse<IEnumerable<PersonalWordListItemDto>>.CreateSuccess(itemDtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PersonalWordListItemDto>>.Fail($"Error retrieving personal word list items: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PersonalWordListItemDto>> AddWordToPersonalListAsync(int userId, int listId, AddWordToListDto addDto)
        {
            try
            {
                // Verify user exists
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<PersonalWordListItemDto>.Fail("User not found");
                }

                // Get word list
                var list = await _unitOfWork.PersonalWordLists.GetFirstOrDefaultAsync(l =>
                    l.ListID == listId && l.UserID == userId);

                if (list == null)
                {
                    return ApiResponse<PersonalWordListItemDto>.Fail("Personal word list not found");
                }

                // Verify vocabulary exists
                var vocabulary = await _unitOfWork.Vocabularies.GetByIdAsync(addDto.VocabularyID);
                if (vocabulary == null)
                {
                    return ApiResponse<PersonalWordListItemDto>.Fail("Vocabulary not found");
                }

                // Check if vocabulary already in list
                var existingItem = await _unitOfWork.PersonalWordListItems.GetFirstOrDefaultAsync(i =>
                    i.ListID == listId && i.VocabularyID == addDto.VocabularyID);

                if (existingItem != null)
                {
                    return ApiResponse<PersonalWordListItemDto>.Fail("Vocabulary already in list");
                }

                // Add vocabulary to list
                var item = new PersonalWordListItem
                {
                    ListID = listId,
                    VocabularyID = addDto.VocabularyID,
                    AddedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.PersonalWordListItems.AddAsync(item);
                await _unitOfWork.SaveChangesAsync();

                // Get vocabulary details
                var vocabResponse = await _vocabularyService.GetVocabularyByIdAsync(addDto.VocabularyID);
                VocabularyDto? vocabularyDto = null;
                if (vocabResponse.Success && vocabResponse.Data != null)
                {
                    vocabularyDto = vocabResponse.Data;
                }

                // Create response
                var itemDto = new PersonalWordListItemDto
                {
                    ItemID = item.ItemID,
                    ListID = item.ListID,
                    VocabularyID = item.VocabularyID,
                    Vocabulary = vocabularyDto,
                    AddedAt = item.AddedAt
                };

                return ApiResponse<PersonalWordListItemDto>.CreateSuccess(itemDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<PersonalWordListItemDto>.Fail($"Error adding word to personal list: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> RemoveWordFromPersonalListAsync(int userId, int listId, int vocabularyId)
        {
            try
            {
                // Verify user exists
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<bool>.Fail("User not found");
                }

                // Get word list
                var list = await _unitOfWork.PersonalWordLists.GetFirstOrDefaultAsync(l =>
                    l.ListID == listId && l.UserID == userId);

                if (list == null)
                {
                    return ApiResponse<bool>.Fail("Personal word list not found");
                }

                // Get list item
                var item = await _unitOfWork.PersonalWordListItems.GetFirstOrDefaultAsync(i =>
                    i.ListID == listId && i.VocabularyID == vocabularyId);

                if (item == null)
                {
                    return ApiResponse<bool>.Fail("Vocabulary not found in list");
                }

                // Remove item
                _unitOfWork.PersonalWordListItems.Remove(item);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.CreateSuccess(true, "Vocabulary removed from list successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"Error removing word from personal list: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PagedResponseDto<StudyPlanDto>>> GetUserStudyPlansAsync(int userId, int pageIndex = 0, int pageSize = 10, bool activeOnly = false)
        {
            try
            {
                // Verify user exists
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<PagedResponseDto<StudyPlanDto>>.Fail("User not found");
                }

                // Build filter
                var filter = PredicateBuilder.True<StudyPlan>().And(p => p.UserID == userId);

                if (activeOnly)
                {
                    filter = filter.And(p => p.IsActive);
                }

                // Get paged result
                var result = await _unitOfWork.StudyPlans.GetPagedAsync(
                    filter: filter,
                    orderBy: q => q.OrderByDescending(p => p.IsActive).ThenBy(p => p.PlanName),
                    pageIndex: pageIndex,
                    pageSize: pageSize);

                // Map to DTOs
                var planDtos = new List<StudyPlanDto>();
                foreach (var plan in result.Items)
                {
                    // Count goals
                    var goalsCount = await _unitOfWork.StudyGoals.CountAsync(g => g.PlanID == plan.PlanID);

                    planDtos.Add(new StudyPlanDto
                    {
                        Id = plan.PlanID,
                        UserID = plan.UserID,
                        PlanName = plan.PlanName,
                        TargetLevel = plan.TargetLevel,
                        StartDate = plan.StartDate,
                        TargetDate = plan.TargetDate,
                        Description = plan.Description,
                        MinutesPerDay = plan.MinutesPerDay,
                        IsActive = plan.IsActive,
                        CurrentStatus = plan.CurrentStatus,
                        CompletionPercentage = plan.CompletionPercentage,
                        LastUpdated = plan.LastUpdated,
                        GoalsCount = goalsCount,
                        CreatedAt = plan.CreatedAt,
                        UpdatedAt = plan.UpdatedAt
                    });
                }

                // Create response
                var response = new PagedResponseDto<StudyPlanDto>
                {
                    Items = planDtos,
                    TotalCount = result.TotalCount,
                    PageIndex = result.PageIndex,
                    PageSize = result.PageSize
                };

                return ApiResponse<PagedResponseDto<StudyPlanDto>>.CreateSuccess(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponseDto<StudyPlanDto>>.Fail($"Error retrieving study plans: {ex.Message}");
            }
        }

        public async Task<ApiResponse<StudyPlanDto>> GetStudyPlanByIdAsync(int userId, int planId)
        {
            try
            {
                // Verify user exists
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<StudyPlanDto>.Fail("User not found");
                }

                // Get study plan
                var plan = await _unitOfWork.StudyPlans.GetFirstOrDefaultAsync(p =>
                    p.PlanID == planId && p.UserID == userId);

                if (plan == null)
                {
                    return ApiResponse<StudyPlanDto>.Fail("Study plan not found");
                }

                // Count goals
                var goalsCount = await _unitOfWork.StudyGoals.CountAsync(g => g.PlanID == plan.PlanID);

                // Create response
                var planDto = new StudyPlanDto
                {
                    Id = plan.PlanID,
                    UserID = plan.UserID,
                    PlanName = plan.PlanName,
                    TargetLevel = plan.TargetLevel,
                    StartDate = plan.StartDate,
                    TargetDate = plan.TargetDate,
                    Description = plan.Description,
                    MinutesPerDay = plan.MinutesPerDay,
                    IsActive = plan.IsActive,
                    CurrentStatus = plan.CurrentStatus,
                    CompletionPercentage = plan.CompletionPercentage,
                    LastUpdated = plan.LastUpdated,
                    GoalsCount = goalsCount,
                    CreatedAt = plan.CreatedAt,
                    UpdatedAt = plan.UpdatedAt
                };

                return ApiResponse<StudyPlanDto>.CreateSuccess(planDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<StudyPlanDto>.Fail($"Error retrieving study plan: {ex.Message}");
            }
        }

        public async Task<ApiResponse<StudyPlanDto>> CreateStudyPlanAsync(int userId, CreateStudyPlanDto createDto)
        {
            try
            {
                // Verify user exists
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<StudyPlanDto>.Fail("User not found");
                }

                // Check if plan name already exists for this user
                var existingPlan = await _unitOfWork.StudyPlans.GetFirstOrDefaultAsync(p =>
                    p.UserID == userId && p.PlanName == createDto.PlanName);

                if (existingPlan != null)
                {
                    return ApiResponse<StudyPlanDto>.Fail("A plan with this name already exists");
                }

                // Create new plan
                var plan = new StudyPlan
                {
                    UserID = userId,
                    PlanName = createDto.PlanName,
                    TargetLevel = createDto.TargetLevel,
                    StartDate = createDto.StartDate,
                    TargetDate = createDto.TargetDate,
                    Description = createDto.Description,
                    MinutesPerDay = createDto.MinutesPerDay,
                    IsActive = true,
                    CurrentStatus = "New",
                    CompletionPercentage = 0,
                    LastUpdated = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.StudyPlans.AddAsync(plan);
                await _unitOfWork.SaveChangesAsync();

                // Create response
                var planDto = new StudyPlanDto
                {
                    Id = plan.PlanID,
                    UserID = plan.UserID,
                    PlanName = plan.PlanName,
                    TargetLevel = plan.TargetLevel,
                    StartDate = plan.StartDate,
                    TargetDate = plan.TargetDate,
                    Description = plan.Description,
                    MinutesPerDay = plan.MinutesPerDay,
                    IsActive = plan.IsActive,
                    CurrentStatus = plan.CurrentStatus,
                    CompletionPercentage = plan.CompletionPercentage,
                    LastUpdated = plan.LastUpdated,
                    GoalsCount = 0,
                    CreatedAt = plan.CreatedAt,
                    UpdatedAt = plan.UpdatedAt
                };

                return ApiResponse<StudyPlanDto>.CreateSuccess(planDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<StudyPlanDto>.Fail($"Error creating study plan: {ex.Message}");
            }
        }

        public async Task<ApiResponse<StudyPlanDto>> UpdateStudyPlanAsync(int userId, int planId, CreateStudyPlanDto updateDto)
        {
            try
            {
                // Verify user exists
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<StudyPlanDto>.Fail("User not found");
                }

                // Get study plan
                var plan = await _unitOfWork.StudyPlans.GetFirstOrDefaultAsync(p =>
                    p.PlanID == planId && p.UserID == userId);

                if (plan == null)
                {
                    return ApiResponse<StudyPlanDto>.Fail("Study plan not found");
                }

                // Check if new name already exists for this user
                if (updateDto.PlanName != plan.PlanName)
                {
                    var existingPlan = await _unitOfWork.StudyPlans.GetFirstOrDefaultAsync(p =>
                        p.UserID == userId && p.PlanName == updateDto.PlanName);

                    if (existingPlan != null)
                    {
                        return ApiResponse<StudyPlanDto>.Fail("A plan with this name already exists");
                    }
                }

                // Update properties
                plan.PlanName = updateDto.PlanName;
                plan.TargetLevel = updateDto.TargetLevel;
                plan.StartDate = updateDto.StartDate;
                plan.TargetDate = updateDto.TargetDate;
                plan.Description = updateDto.Description;
                plan.MinutesPerDay = updateDto.MinutesPerDay;
                plan.LastUpdated = DateTime.UtcNow;
                plan.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.StudyPlans.Update(plan);
                await _unitOfWork.SaveChangesAsync();

                // Count goals
                var goalsCount = await _unitOfWork.StudyGoals.CountAsync(g => g.PlanID == plan.PlanID);

                // Create response
                var planDto = new StudyPlanDto
                {
                    Id = plan.PlanID,
                    UserID = plan.UserID,
                    PlanName = plan.PlanName,
                    TargetLevel = plan.TargetLevel,
                    StartDate = plan.StartDate,
                    TargetDate = plan.TargetDate,
                    Description = plan.Description,
                    MinutesPerDay = plan.MinutesPerDay,
                    IsActive = plan.IsActive,
                    CurrentStatus = plan.CurrentStatus,
                    CompletionPercentage = plan.CompletionPercentage,
                    LastUpdated = plan.LastUpdated,
                    GoalsCount = goalsCount,
                    CreatedAt = plan.CreatedAt,
                    UpdatedAt = plan.UpdatedAt
                };

                return ApiResponse<StudyPlanDto>.CreateSuccess(planDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<StudyPlanDto>.Fail($"Error updating study plan: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteStudyPlanAsync(int userId, int planId)
        {
            try
            {
                // Verify user exists
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<bool>.Fail("User not found");
                }

                // Get study plan
                var plan = await _unitOfWork.StudyPlans.GetFirstOrDefaultAsync(p =>
                    p.PlanID == planId && p.UserID == userId);

                if (plan == null)
                {
                    return ApiResponse<bool>.Fail("Study plan not found");
                }

                // Delete study goals and tasks
                var goals = await _unitOfWork.StudyGoals.FindAsync(g => g.PlanID == planId);
                foreach (var goal in goals)
                {
                    var tasks = await _unitOfWork.StudyTasks.FindAsync(t => t.GoalID == goal.GoalID);
                    if (tasks.Any())
                    {
                        _unitOfWork.StudyTasks.RemoveRange(tasks);
                    }
                }

                if (goals.Any())
                {
                    _unitOfWork.StudyGoals.RemoveRange(goals);
                }

                // Delete study plan
                _unitOfWork.StudyPlans.Remove(plan);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.CreateSuccess(true, "Study plan deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"Error deleting study plan: {ex.Message}");
            }
        }

        public async Task<ApiResponse<IEnumerable<StudyGoalDto>>> GetStudyPlanGoalsAsync(int userId, int planId)
        {
            try
            {
                // Verify user exists
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<IEnumerable<StudyGoalDto>>.Fail("User not found");
                }

                // Get study plan
                var plan = await _unitOfWork.StudyPlans.GetFirstOrDefaultAsync(p =>
                    p.PlanID == planId && p.UserID == userId);

                if (plan == null)
                {
                    return ApiResponse<IEnumerable<StudyGoalDto>>.Fail("Study plan not found");
                }

                // Get goals
                var goals = await _unitOfWork.StudyGoals.FindAsync(g => g.PlanID == planId);

                // Sort by importance, then target date
                goals = goals.OrderByDescending(g => g.Importance).ThenBy(g => g.TargetDate).ToList();

                // Map to DTOs
                var goalDtos = new List<StudyGoalDto>();
                foreach (var goal in goals)
                {
                    // Get JLPT level and topic details
                    JLPTLevel? level = null;
                    if (goal.LevelID.HasValue)
                    {
                        level = await _unitOfWork.JLPTLevels.GetByIdAsync(goal.LevelID.Value);
                    }

                    StudyTopic? topic = null;
                    if (goal.TopicID.HasValue)
                    {
                        topic = await _unitOfWork.StudyTopics.GetByIdAsync(goal.TopicID.Value);
                    }

                    // Count tasks
                    var tasksCount = await _unitOfWork.StudyTasks.CountAsync(t => t.GoalID == goal.GoalID);
                    var completedTasksCount = await _unitOfWork.StudyTasks.CountAsync(t => t.GoalID == goal.GoalID && t.IsCompleted);

                    goalDtos.Add(new StudyGoalDto
                    {
                        Id = goal.GoalID,
                        PlanID = goal.PlanID,
                        GoalName = goal.GoalName,
                        Description = goal.Description,
                        LevelID = goal.LevelID,
                        LevelName = level?.LevelName,
                        TopicID = goal.TopicID,
                        TopicName = topic?.TopicName,
                        TargetDate = goal.TargetDate,
                        Importance = goal.Importance,
                        Difficulty = goal.Difficulty,
                        IsCompleted = goal.IsCompleted,
                        ProgressPercentage = goal.ProgressPercentage,
                        TasksCount = tasksCount,
                        CompletedTasksCount = completedTasksCount,
                        CreatedAt = goal.CreatedAt,
                        UpdatedAt = goal.UpdatedAt
                    });
                }

                return ApiResponse<IEnumerable<StudyGoalDto>>.CreateSuccess(goalDtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<StudyGoalDto>>.Fail($"Error retrieving study plan goals: {ex.Message}");
            }
        }

        public async Task<ApiResponse<StudyGoalDto>> CreateStudyGoalAsync(int userId, int planId, CreateStudyGoalDto createDto)
        {
            try
            {
                // Verify user exists
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<StudyGoalDto>.Fail("User not found");
                }

                // Get study plan
                var plan = await _unitOfWork.StudyPlans.GetFirstOrDefaultAsync(p =>
                    p.PlanID == planId && p.UserID == userId);

                if (plan == null)
                {
                    return ApiResponse<StudyGoalDto>.Fail("Study plan not found");
                }

                // Validate JLPT level if provided
                if (createDto.LevelID.HasValue)
                {
                    var levelC = await _unitOfWork.JLPTLevels.GetByIdAsync(createDto.LevelID.Value);
                    if (levelC == null)
                    {
                        return ApiResponse<StudyGoalDto>.Fail("Invalid JLPT level");
                    }
                }

                // Validate topic if provided
                if (createDto.TopicID.HasValue)
                {
                    var topicC = await _unitOfWork.StudyTopics.GetByIdAsync(createDto.TopicID.Value);
                    if (topicC == null)
                    {
                        return ApiResponse<StudyGoalDto>.Fail("Invalid study topic");
                    }
                }

                // Create new goal
                var goal = new StudyGoal
                {
                    PlanID = planId,
                    GoalName = createDto.GoalName,
                    Description = createDto.Description,
                    LevelID = createDto.LevelID,
                    TopicID = createDto.TopicID,
                    TargetDate = createDto.TargetDate,
                    Importance = createDto.Importance,
                    Difficulty = createDto.Difficulty,
                    IsCompleted = false,
                    ProgressPercentage = 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.StudyGoals.AddAsync(goal);
                await _unitOfWork.SaveChangesAsync();

                // Update plan's last updated timestamp
                plan.LastUpdated = DateTime.UtcNow;
                plan.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.StudyPlans.Update(plan);
                await _unitOfWork.SaveChangesAsync();

                // Get JLPT level and topic details
                JLPTLevel? level = null;
                if (goal.LevelID.HasValue)
                {
                    level = await _unitOfWork.JLPTLevels.GetByIdAsync(goal.LevelID.Value);
                }

                StudyTopic? topic = null;
                if (goal.TopicID.HasValue)
                {
                    topic = await _unitOfWork.StudyTopics.GetByIdAsync(goal.TopicID.Value);
                }

                // Create response
                var goalDto = new StudyGoalDto
                {
                    Id = goal.GoalID,
                    PlanID = goal.PlanID,
                    GoalName = goal.GoalName,
                    Description = goal.Description,
                    LevelID = goal.LevelID,
                    LevelName = level?.LevelName,
                    TopicID = goal.TopicID,
                    TopicName = topic?.TopicName,
                    TargetDate = goal.TargetDate,
                    Importance = goal.Importance,
                    Difficulty = goal.Difficulty,
                    IsCompleted = goal.IsCompleted,
                    ProgressPercentage = goal.ProgressPercentage,
                    TasksCount = 0,
                    CompletedTasksCount = 0,
                    CreatedAt = goal.CreatedAt,
                    UpdatedAt = goal.UpdatedAt
                };

                return ApiResponse<StudyGoalDto>.CreateSuccess(goalDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<StudyGoalDto>.Fail($"Error creating study goal: {ex.Message}");
            }
        }

        public async Task<ApiResponse<StudyGoalDto>> UpdateStudyGoalAsync(int userId, int planId, int goalId, UpdateStudyGoalDto updateDto)
        {
            try
            {
                // Verify user exists
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<StudyGoalDto>.Fail("User not found");
                }

                // Get study plan
                var plan = await _unitOfWork.StudyPlans.GetFirstOrDefaultAsync(p =>
                    p.PlanID == planId && p.UserID == userId);

                if (plan == null)
                {
                    return ApiResponse<StudyGoalDto>.Fail("Study plan not found");
                }

                // Get study goal
                var goal = await _unitOfWork.StudyGoals.GetFirstOrDefaultAsync(g =>
                    g.GoalID == goalId && g.PlanID == planId);

                if (goal == null)
                {
                    return ApiResponse<StudyGoalDto>.Fail("Study goal not found");
                }

                // Validate JLPT level if provided
                if (updateDto.LevelID.HasValue)
                {
                    var levelU = await _unitOfWork.JLPTLevels.GetByIdAsync(updateDto.LevelID.Value);
                    if (levelU == null)
                    {
                        return ApiResponse<StudyGoalDto>.Fail("Invalid JLPT level");
                    }
                }

                // Validate topic if provided
                if (updateDto.TopicID.HasValue)
                {
                    var topicU = await _unitOfWork.StudyTopics.GetByIdAsync(updateDto.TopicID.Value);
                    if (topicU == null)
                    {
                        return ApiResponse<StudyGoalDto>.Fail("Invalid study topic");
                    }
                }

                // Update properties
                if (!string.IsNullOrEmpty(updateDto.GoalName))
                    goal.GoalName = updateDto.GoalName;

                if (updateDto.Description != null)
                    goal.Description = updateDto.Description;

                if (updateDto.LevelID.HasValue)
                    goal.LevelID = updateDto.LevelID;

                if (updateDto.TopicID.HasValue)
                    goal.TopicID = updateDto.TopicID;

                if (updateDto.TargetDate.HasValue)
                    goal.TargetDate = updateDto.TargetDate;

                if (updateDto.Importance.HasValue)
                    goal.Importance = updateDto.Importance;

                if (updateDto.Difficulty.HasValue)
                    goal.Difficulty = updateDto.Difficulty;

                if (updateDto.IsCompleted.HasValue)
                    goal.IsCompleted = updateDto.IsCompleted.Value;

                if (updateDto.ProgressPercentage.HasValue)
                    goal.ProgressPercentage = updateDto.ProgressPercentage.Value;

                goal.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.StudyGoals.Update(goal);
                await _unitOfWork.SaveChangesAsync();

                // Update plan's last updated timestamp
                plan.LastUpdated = DateTime.UtcNow;
                plan.UpdatedAt = DateTime.UtcNow;

                // Update plan's completion percentage
                var goals = await _unitOfWork.StudyGoals.FindAsync(g => g.PlanID == planId);
                if (goals.Any())
                {
                    var completedGoals = goals.Count(g => g.IsCompleted);
                    var totalGoals = goals.Count();
                    plan.CompletionPercentage = (float)completedGoals / totalGoals * 100;
                }

                _unitOfWork.StudyPlans.Update(plan);
                await _unitOfWork.SaveChangesAsync();

                // Get JLPT level and topic details
                JLPTLevel? level = null;
                if (goal.LevelID.HasValue)
                {
                    level = await _unitOfWork.JLPTLevels.GetByIdAsync(goal.LevelID.Value);
                }

                StudyTopic? topic = null;
                if (goal.TopicID.HasValue)
                {
                    topic = await _unitOfWork.StudyTopics.GetByIdAsync(goal.TopicID.Value);
                }

                // Count tasks
                var tasksCount = await _unitOfWork.StudyTasks.CountAsync(t => t.GoalID == goal.GoalID);
                var completedTasksCount = await _unitOfWork.StudyTasks.CountAsync(t => t.GoalID == goal.GoalID && t.IsCompleted);

                // Create response
                var goalDto = new StudyGoalDto
                {
                    Id = goal.GoalID,
                    PlanID = goal.PlanID,
                    GoalName = goal.GoalName,
                    Description = goal.Description,
                    LevelID = goal.LevelID,
                    LevelName = level?.LevelName,
                    TopicID = goal.TopicID,
                    TopicName = topic?.TopicName,
                    TargetDate = goal.TargetDate,
                    Importance = goal.Importance,
                    Difficulty = goal.Difficulty,
                    IsCompleted = goal.IsCompleted,
                    ProgressPercentage = goal.ProgressPercentage,
                    TasksCount = tasksCount,
                    CompletedTasksCount = completedTasksCount,
                    CreatedAt = goal.CreatedAt,
                    UpdatedAt = goal.UpdatedAt
                };

                return ApiResponse<StudyGoalDto>.CreateSuccess(goalDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<StudyGoalDto>.Fail($"Error updating study goal: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteStudyGoalAsync(int userId, int planId, int goalId)
        {
            try
            {
                // Verify user exists
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<bool>.Fail("User not found");
                }

                // Get study plan
                var plan = await _unitOfWork.StudyPlans.GetFirstOrDefaultAsync(p =>
                    p.PlanID == planId && p.UserID == userId);

                if (plan == null)
                {
                    return ApiResponse<bool>.Fail("Study plan not found");
                }

                // Get study goal
                var goal = await _unitOfWork.StudyGoals.GetFirstOrDefaultAsync(g =>
                    g.GoalID == goalId && g.PlanID == planId);

                if (goal == null)
                {
                    return ApiResponse<bool>.Fail("Study goal not found");
                }

                // Delete study tasks
                var tasks = await _unitOfWork.StudyTasks.FindAsync(t => t.GoalID == goalId);
                if (tasks.Any())
                {
                    _unitOfWork.StudyTasks.RemoveRange(tasks);
                    await _unitOfWork.SaveChangesAsync();
                }

                // Delete study goal
                _unitOfWork.StudyGoals.Remove(goal);
                await _unitOfWork.SaveChangesAsync();

                // Update plan's last updated timestamp
                plan.LastUpdated = DateTime.UtcNow;
                plan.UpdatedAt = DateTime.UtcNow;

                // Update plan's completion percentage
                var goals = await _unitOfWork.StudyGoals.FindAsync(g => g.PlanID == planId);
                if (goals.Any())
                {
                    var completedGoals = goals.Count(g => g.IsCompleted);
                    var totalGoals = goals.Count();
                    plan.CompletionPercentage = (float)completedGoals / totalGoals * 100;
                }
                else
                {
                    plan.CompletionPercentage = 0;
                }

                _unitOfWork.StudyPlans.Update(plan);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.CreateSuccess(true, "Study goal deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"Error deleting study goal: {ex.Message}");
            }
        }

        // Helper method to calculate next review date based on memory strength
        private DateTime CalculateNextReviewDate(int memoryStrength)
        {
            var now = DateTime.UtcNow;

            // Spaced repetition algorithm: stronger memory = longer intervals
            int daysToAdd;

            if (memoryStrength <= 20)
                daysToAdd = 1; // Very weak memory - review tomorrow
            else if (memoryStrength <= 40)
                daysToAdd = 3; // Weak memory - review in 3 days
            else if (memoryStrength <= 60)
                daysToAdd = 7; // Medium memory - review in a week
            else if (memoryStrength <= 80)
                daysToAdd = 14; // Strong memory - review in 2 weeks
            else
                daysToAdd = 30; // Very strong memory - review in a month

            return now.AddDays(daysToAdd);
        }
    }
}