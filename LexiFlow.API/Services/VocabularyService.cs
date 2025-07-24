using LexiFlow.API.Data.Entities;
using LexiFlow.API.Data.Repositories;
using LexiFlow.API.Data.UnitOfWork;
using LexiFlow.API.DTOs;
using LexiFlow.API.DTOs.Vocabulary;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LexiFlow.API.Services
{

    public class VocabularyService : IVocabularyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VocabularyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<PagedResponseDto<VocabularyDto>>> GetVocabulariesAsync(int pageIndex = 0, int pageSize = 10, string? search = null, string? level = null, int? groupId = null)
        {
            try
            {
                // Build filter
                var filter = PredicateBuilder.True<Vocabulary>();

                if (!string.IsNullOrEmpty(search))
                {
                    filter = filter.And(v =>
                        v.Japanese.Contains(search) ||
                        v.Kana.Contains(search) ||
                        v.Romaji.Contains(search) ||
                        v.Vietnamese.Contains(search) ||
                        v.English.Contains(search));
                }

                if (!string.IsNullOrEmpty(level))
                {
                    filter = filter.And(v => v.Level == level);
                }

                if (groupId.HasValue)
                {
                    filter = filter.And(v => v.GroupID == groupId.Value);
                }

                // Get paged result
                var result = await _unitOfWork.Vocabularies.GetPagedAsync(
                    filter: filter,
                    orderBy: q => q.OrderBy(v => v.Japanese),
                    includeProperties: "Group",
                    pageIndex: pageIndex,
                    pageSize: pageSize);

                // Map to DTOs
                var vocabularyDtos = new List<VocabularyDto>();
                foreach (var vocabulary in result.Items)
                {
                    var kanjiVocabularies = await _unitOfWork.KanjiVocabularies.FindAsync(kv => kv.VocabularyID == vocabulary.VocabularyID);
                    var kanjiIds = kanjiVocabularies.Select(kv => kv.KanjiID).ToList();
                    var kanjis = await _unitOfWork.Kanjis.FindAsync(k => kanjiIds.Contains(k.KanjiID));

                    var kanjiInfoDtos = new List<KanjiInfoDto>();
                    foreach (var kanji in kanjis)
                    {
                        var kanjiVocabulary = kanjiVocabularies.FirstOrDefault(kv => kv.KanjiID == kanji.KanjiID);
                        kanjiInfoDtos.Add(new KanjiInfoDto
                        {
                            KanjiID = kanji.KanjiID,
                            Character = kanji.Character,
                            Onyomi = kanji.Onyomi,
                            Kunyomi = kanji.Kunyomi,
                            Meaning = kanji.Meaning,
                            JLPTLevel = kanji.JLPTLevel,
                            Position = kanjiVocabulary?.Position
                        });
                    }

                    vocabularyDtos.Add(new VocabularyDto
                    {
                        Id = vocabulary.VocabularyID,
                        Japanese = vocabulary.Japanese,
                        Kana = vocabulary.Kana,
                        Romaji = vocabulary.Romaji,
                        Vietnamese = vocabulary.Vietnamese,
                        English = vocabulary.English,
                        Example = vocabulary.Example,
                        Notes = vocabulary.Notes,
                        GroupID = vocabulary.GroupID,
                        GroupName = vocabulary.Group?.GroupName,
                        Level = vocabulary.Level,
                        PartOfSpeech = vocabulary.PartOfSpeech,
                        AudioFile = vocabulary.AudioFile,
                        CreatedAt = vocabulary.CreatedAt,
                        UpdatedAt = vocabulary.UpdatedAt,
                        LastModifiedAt = vocabulary.LastModifiedAt,
                        Kanjis = kanjiInfoDtos
                    });
                }

                // Create response
                var response = new PagedResponseDto<VocabularyDto>
                {
                    Items = vocabularyDtos,
                    TotalCount = result.TotalCount,
                    PageIndex = result.PageIndex,
                    PageSize = result.PageSize
                };

                return ApiResponse<PagedResponseDto<VocabularyDto>>.CreateSuccess(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponseDto<VocabularyDto>>.Fail($"Error retrieving vocabularies: {ex.Message}");
            }
        }

        public async Task<ApiResponse<VocabularyDto>> GetVocabularyByIdAsync(int id)
        {
            try
            {
                var vocabulary = await _unitOfWork.Vocabularies.GetFirstOrDefaultAsync(v => v.VocabularyID == id);
                if (vocabulary == null)
                {
                    return ApiResponse<VocabularyDto>.Fail("Vocabulary not found");
                }

                // Get group
                var group = await _unitOfWork.VocabularyGroups.GetFirstOrDefaultAsync(g => g.GroupID == vocabulary.GroupID);

                // Get kanjis
                var kanjiVocabularies = await _unitOfWork.KanjiVocabularies.FindAsync(kv => kv.VocabularyID == vocabulary.VocabularyID);
                var kanjiIds = kanjiVocabularies.Select(kv => kv.KanjiID).ToList();
                var kanjis = await _unitOfWork.Kanjis.FindAsync(k => kanjiIds.Contains(k.KanjiID));

                var kanjiInfoDtos = new List<KanjiInfoDto>();
                foreach (var kanji in kanjis)
                {
                    var kanjiVocabulary = kanjiVocabularies.FirstOrDefault(kv => kv.KanjiID == kanji.KanjiID);
                    kanjiInfoDtos.Add(new KanjiInfoDto
                    {
                        KanjiID = kanji.KanjiID,
                        Character = kanji.Character,
                        Onyomi = kanji.Onyomi,
                        Kunyomi = kanji.Kunyomi,
                        Meaning = kanji.Meaning,
                        JLPTLevel = kanji.JLPTLevel,
                        Position = kanjiVocabulary?.Position
                    });
                }

                // Create response
                var vocabularyDto = new VocabularyDto
                {
                    Id = vocabulary.VocabularyID,
                    Japanese = vocabulary.Japanese,
                    Kana = vocabulary.Kana,
                    Romaji = vocabulary.Romaji,
                    Vietnamese = vocabulary.Vietnamese,
                    English = vocabulary.English,
                    Example = vocabulary.Example,
                    Notes = vocabulary.Notes,
                    GroupID = vocabulary.GroupID,
                    GroupName = group?.GroupName,
                    Level = vocabulary.Level,
                    PartOfSpeech = vocabulary.PartOfSpeech,
                    AudioFile = vocabulary.AudioFile,
                    CreatedAt = vocabulary.CreatedAt,
                    UpdatedAt = vocabulary.UpdatedAt,
                    LastModifiedAt = vocabulary.LastModifiedAt,
                    Kanjis = kanjiInfoDtos
                };

                return ApiResponse<VocabularyDto>.CreateSuccess(vocabularyDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<VocabularyDto>.Fail($"Error retrieving vocabulary: {ex.Message}");
            }
        }

        public async Task<ApiResponse<VocabularyDto>> CreateVocabularyAsync(CreateVocabularyDto createDto, int currentUserId)
        {
            try
            {
                // Create new vocabulary
                var vocabulary = new Vocabulary
                {
                    Japanese = createDto.Japanese,
                    Kana = createDto.Kana,
                    Romaji = createDto.Romaji,
                    Vietnamese = createDto.Vietnamese,
                    English = createDto.English,
                    Example = createDto.Example,
                    Notes = createDto.Notes,
                    GroupID = createDto.GroupID,
                    Level = createDto.Level,
                    PartOfSpeech = createDto.PartOfSpeech,
                    CreatedByUserID = currentUserId,
                    UpdatedByUserID = currentUserId,
                    LastModifiedBy = currentUserId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow
                };

                await _unitOfWork.Vocabularies.AddAsync(vocabulary);
                await _unitOfWork.SaveChangesAsync();

                // Add kanji associations if provided
                if (createDto.KanjiIDs != null && createDto.KanjiIDs.Any())
                {
                    int position = 0;
                    foreach (var kanjiId in createDto.KanjiIDs)
                    {
                        // Check if kanji exists
                        var kanji = await _unitOfWork.Kanjis.GetByIdAsync(kanjiId);
                        if (kanji != null)
                        {
                            var kanjiVocabulary = new KanjiVocabulary
                            {
                                KanjiID = kanjiId,
                                VocabularyID = vocabulary.VocabularyID,
                                Position = position++,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow
                            };
                            await _unitOfWork.KanjiVocabularies.AddAsync(kanjiVocabulary);
                        }
                    }
                    await _unitOfWork.SaveChangesAsync();
                }

                // Get created vocabulary
                return await GetVocabularyByIdAsync(vocabulary.VocabularyID);
            }
            catch (Exception ex)
            {
                return ApiResponse<VocabularyDto>.Fail($"Error creating vocabulary: {ex.Message}");
            }
        }

        public async Task<ApiResponse<VocabularyDto>> UpdateVocabularyAsync(int id, UpdateVocabularyDto updateDto, int currentUserId)
        {
            try
            {
                var vocabulary = await _unitOfWork.Vocabularies.GetByIdAsync(id);
                if (vocabulary == null)
                {
                    return ApiResponse<VocabularyDto>.Fail("Vocabulary not found");
                }

                // Update properties
                if (!string.IsNullOrEmpty(updateDto.Japanese))
                    vocabulary.Japanese = updateDto.Japanese;

                if (updateDto.Kana != null)
                    vocabulary.Kana = updateDto.Kana;

                if (updateDto.Romaji != null)
                    vocabulary.Romaji = updateDto.Romaji;

                if (updateDto.Vietnamese != null)
                    vocabulary.Vietnamese = updateDto.Vietnamese;

                if (updateDto.English != null)
                    vocabulary.English = updateDto.English;

                if (updateDto.Example != null)
                    vocabulary.Example = updateDto.Example;

                if (updateDto.Notes != null)
                    vocabulary.Notes = updateDto.Notes;

                if (updateDto.GroupID.HasValue)
                    vocabulary.GroupID = updateDto.GroupID;

                if (updateDto.Level != null)
                    vocabulary.Level = updateDto.Level;

                if (updateDto.PartOfSpeech != null)
                    vocabulary.PartOfSpeech = updateDto.PartOfSpeech;

                vocabulary.UpdatedByUserID = currentUserId;
                vocabulary.LastModifiedBy = currentUserId;
                vocabulary.UpdatedAt = DateTime.UtcNow;
                vocabulary.LastModifiedAt = DateTime.UtcNow;

                _unitOfWork.Vocabularies.Update(vocabulary);
                await _unitOfWork.SaveChangesAsync();

                // Update kanji associations if provided
                if (updateDto.KanjiIDs != null)
                {
                    // Remove existing associations
                    var existingAssociations = await _unitOfWork.KanjiVocabularies.FindAsync(kv => kv.VocabularyID == id);
                    if (existingAssociations.Any())
                    {
                        _unitOfWork.KanjiVocabularies.RemoveRange(existingAssociations);
                        await _unitOfWork.SaveChangesAsync();
                    }

                    // Add new associations
                    int position = 0;
                    foreach (var kanjiId in updateDto.KanjiIDs)
                    {
                        // Check if kanji exists
                        var kanji = await _unitOfWork.Kanjis.GetByIdAsync(kanjiId);
                        if (kanji != null)
                        {
                            var kanjiVocabulary = new KanjiVocabulary
                            {
                                KanjiID = kanjiId,
                                VocabularyID = vocabulary.VocabularyID,
                                Position = position++,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow
                            };
                            await _unitOfWork.KanjiVocabularies.AddAsync(kanjiVocabulary);
                        }
                    }
                    await _unitOfWork.SaveChangesAsync();
                }

                // Get updated vocabulary
                return await GetVocabularyByIdAsync(vocabulary.VocabularyID);
            }
            catch (Exception ex)
            {
                return ApiResponse<VocabularyDto>.Fail($"Error updating vocabulary: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteVocabularyAsync(int id)
        {
            try
            {
                var vocabulary = await _unitOfWork.Vocabularies.GetByIdAsync(id);
                if (vocabulary == null)
                {
                    return ApiResponse<bool>.Fail("Vocabulary not found");
                }

                // Remove kanji associations
                var kanjiVocabularies = await _unitOfWork.KanjiVocabularies.FindAsync(kv => kv.VocabularyID == id);
                if (kanjiVocabularies.Any())
                {
                    _unitOfWork.KanjiVocabularies.RemoveRange(kanjiVocabularies);
                    await _unitOfWork.SaveChangesAsync();
                }

                // Remove learning progress
                var learningProgresses = await _unitOfWork.LearningProgresses.FindAsync(lp => lp.VocabularyID == id);
                if (learningProgresses.Any())
                {
                    _unitOfWork.LearningProgresses.RemoveRange(learningProgresses);
                    await _unitOfWork.SaveChangesAsync();
                }

                // Remove from personal word lists
                var personalWordListItems = await _unitOfWork.PersonalWordListItems.FindAsync(pwl => pwl.VocabularyID == id);
                if (personalWordListItems.Any())
                {
                    _unitOfWork.PersonalWordListItems.RemoveRange(personalWordListItems);
                    await _unitOfWork.SaveChangesAsync();
                }

                // Remove vocabulary
                _unitOfWork.Vocabularies.Remove(vocabulary);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.CreateSuccess(true, "Vocabulary deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"Error deleting vocabulary: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PagedResponseDto<VocabularyGroupDto>>> GetVocabularyGroupsAsync(int pageIndex = 0, int pageSize = 10, string? search = null)
        {
            try
            {
                // Build filter
                var filter = PredicateBuilder.True<VocabularyGroup>();

                if (!string.IsNullOrEmpty(search))
                {
                    filter = filter.And(g => g.GroupName.Contains(search) || g.Description.Contains(search));
                }

                // Get paged result
                var result = await _unitOfWork.VocabularyGroups.GetPagedAsync(
                    filter: filter,
                    orderBy: q => q.OrderBy(g => g.GroupName),
                    includeProperties: "Category",
                    pageIndex: pageIndex,
                    pageSize: pageSize);

                // Map to DTOs
                var groupDtos = new List<VocabularyGroupDto>();
                foreach (var group in result.Items)
                {
                    // Count vocabularies in this group
                    var count = await _unitOfWork.Vocabularies.CountAsync(v => v.GroupID == group.GroupID);

                    groupDtos.Add(new VocabularyGroupDto
                    {
                        Id = group.GroupID,
                        GroupName = group.GroupName,
                        Description = group.Description,
                        CategoryID = group.CategoryID,
                        CategoryName = group.Category?.CategoryName,
                        IsActive = group.IsActive,
                        VocabularyCount = count,
                        CreatedAt = group.CreatedAt,
                        UpdatedAt = group.UpdatedAt
                    });
                }

                // Create response
                var response = new PagedResponseDto<VocabularyGroupDto>
                {
                    Items = groupDtos,
                    TotalCount = result.TotalCount,
                    PageIndex = result.PageIndex,
                    PageSize = result.PageSize
                };

                return ApiResponse<PagedResponseDto<VocabularyGroupDto>>.CreateSuccess(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponseDto<VocabularyGroupDto>>.Fail($"Error retrieving vocabulary groups: {ex.Message}");
            }
        }

        public async Task<ApiResponse<VocabularyGroupDto>> GetVocabularyGroupByIdAsync(int id)
        {
            try
            {
                var group = await _unitOfWork.VocabularyGroups.GetFirstOrDefaultAsync(g => g.GroupID == id);
                if (group == null)
                {
                    return ApiResponse<VocabularyGroupDto>.Fail("Vocabulary group not found");
                }

                // Get category
                var category = await _unitOfWork.Categories.GetFirstOrDefaultAsync(c => c.CategoryID == group.CategoryID);

                // Count vocabularies in this group
                var count = await _unitOfWork.Vocabularies.CountAsync(v => v.GroupID == group.GroupID);

                // Create response
                var groupDto = new VocabularyGroupDto
                {
                    Id = group.GroupID,
                    GroupName = group.GroupName,
                    Description = group.Description,
                    CategoryID = group.CategoryID,
                    CategoryName = category?.CategoryName,
                    IsActive = group.IsActive,
                    VocabularyCount = count,
                    CreatedAt = group.CreatedAt,
                    UpdatedAt = group.UpdatedAt
                };

                return ApiResponse<VocabularyGroupDto>.CreateSuccess(groupDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<VocabularyGroupDto>.Fail($"Error retrieving vocabulary group: {ex.Message}");
            }
        }

        public async Task<ApiResponse<VocabularyGroupDto>> CreateVocabularyGroupAsync(CreateVocabularyGroupDto createDto, int currentUserId)
        {
            try
            {
                // Check if group name already exists
                var existingGroup = await _unitOfWork.VocabularyGroups.GetFirstOrDefaultAsync(g => g.GroupName == createDto.GroupName);
                if (existingGroup != null)
                {
                    return ApiResponse<VocabularyGroupDto>.Fail("Group name already exists");
                }

                // Create new group
                var group = new VocabularyGroup
                {
                    GroupName = createDto.GroupName,
                    Description = createDto.Description,
                    CategoryID = createDto.CategoryID,
                    CreatedByUserID = currentUserId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.VocabularyGroups.AddAsync(group);
                await _unitOfWork.SaveChangesAsync();

                // Get created group
                return await GetVocabularyGroupByIdAsync(group.GroupID);
            }
            catch (Exception ex)
            {
                return ApiResponse<VocabularyGroupDto>.Fail($"Error creating vocabulary group: {ex.Message}");
            }
        }

        public async Task<ApiResponse<VocabularyGroupDto>> UpdateVocabularyGroupAsync(int id, UpdateVocabularyGroupDto updateDto, int currentUserId)
        {
            try
            {
                var group = await _unitOfWork.VocabularyGroups.GetByIdAsync(id);
                if (group == null)
                {
                    return ApiResponse<VocabularyGroupDto>.Fail("Vocabulary group not found");
                }

                // Check if new group name already exists
                if (!string.IsNullOrEmpty(updateDto.GroupName) && updateDto.GroupName != group.GroupName)
                {
                    var existingGroup = await _unitOfWork.VocabularyGroups.GetFirstOrDefaultAsync(g => g.GroupName == updateDto.GroupName);
                    if (existingGroup != null)
                    {
                        return ApiResponse<VocabularyGroupDto>.Fail("Group name already exists");
                    }
                }

                // Update properties
                if (!string.IsNullOrEmpty(updateDto.GroupName))
                    group.GroupName = updateDto.GroupName;

                if (updateDto.Description != null)
                    group.Description = updateDto.Description;

                if (updateDto.CategoryID.HasValue)
                    group.CategoryID = updateDto.CategoryID;

                if (updateDto.IsActive.HasValue)
                    group.IsActive = updateDto.IsActive.Value;

                group.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.VocabularyGroups.Update(group);
                await _unitOfWork.SaveChangesAsync();

                // Get updated group
                return await GetVocabularyGroupByIdAsync(group.GroupID);
            }
            catch (Exception ex)
            {
                return ApiResponse<VocabularyGroupDto>.Fail($"Error updating vocabulary group: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteVocabularyGroupAsync(int id)
        {
            try
            {
                var group = await _unitOfWork.VocabularyGroups.GetByIdAsync(id);
                if (group == null)
                {
                    return ApiResponse<bool>.Fail("Vocabulary group not found");
                }

                // Check if group has vocabularies
                var hasVocabularies = await _unitOfWork.Vocabularies.AnyAsync(v => v.GroupID == id);
                if (hasVocabularies)
                {
                    return ApiResponse<bool>.Fail("Cannot delete group with associated vocabularies");
                }

                // Delete group
                _unitOfWork.VocabularyGroups.Remove(group);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.CreateSuccess(true, "Vocabulary group deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"Error deleting vocabulary group: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PagedResponseDto<KanjiDto>>> GetKanjisAsync(int pageIndex = 0, int pageSize = 10, string? search = null, string? level = null)
        {
            try
            {
                // Build filter
                var filter = PredicateBuilder.True<Kanji>();

                if (!string.IsNullOrEmpty(search))
                {
                    filter = filter.And(k =>
                        k.Character.Contains(search) ||
                        k.Onyomi.Contains(search) ||
                        k.Kunyomi.Contains(search) ||
                        k.Meaning.Contains(search));
                }

                if (!string.IsNullOrEmpty(level))
                {
                    filter = filter.And(k => k.JLPTLevel == level);
                }

                // Get paged result
                var result = await _unitOfWork.Kanjis.GetPagedAsync(
                    filter: filter,
                    orderBy: q => q.OrderBy(k => k.Character),
                    pageIndex: pageIndex,
                    pageSize: pageSize);

                // Map to DTOs
                var kanjiDtos = new List<KanjiDto>();
                foreach (var kanji in result.Items)
                {
                    // Get component mappings
                    var componentMappings = await _unitOfWork.KanjiComponentMappings.FindAsync(m => m.KanjiID == kanji.KanjiID);
                    var componentIds = componentMappings.Select(m => m.ComponentID).ToList();
                    var components = await _unitOfWork.KanjiComponents.FindAsync(c => componentIds.Contains(c.ComponentID));

                    var componentDtos = new List<KanjiComponentDto>();
                    foreach (var component in components)
                    {
                        var mapping = componentMappings.FirstOrDefault(m => m.ComponentID == component.ComponentID);
                        componentDtos.Add(new KanjiComponentDto
                        {
                            ComponentID = component.ComponentID,
                            ComponentName = component.ComponentName,
                            Character = component.Character,
                            Meaning = component.Meaning,
                            Type = component.Type,
                            Position = mapping?.Position
                        });
                    }

                    kanjiDtos.Add(new KanjiDto
                    {
                        Id = kanji.KanjiID,
                        Character = kanji.Character,
                        Onyomi = kanji.Onyomi,
                        Kunyomi = kanji.Kunyomi,
                        Meaning = kanji.Meaning,
                        StrokeCount = kanji.StrokeCount,
                        JLPTLevel = kanji.JLPTLevel,
                        Grade = kanji.Grade,
                        Radicals = kanji.Radicals,
                        Components = kanji.Components,
                        Examples = kanji.Examples,
                        MnemonicHint = kanji.MnemonicHint,
                        WritingOrderImage = kanji.WritingOrderImage,
                        CreatedAt = kanji.CreatedAt,
                        UpdatedAt = kanji.UpdatedAt,
                        KanjiComponents = componentDtos
                    });
                }

                // Create response
                var response = new PagedResponseDto<KanjiDto>
                {
                    Items = kanjiDtos,
                    TotalCount = result.TotalCount,
                    PageIndex = result.PageIndex,
                    PageSize = result.PageSize
                };

                return ApiResponse<PagedResponseDto<KanjiDto>>.CreateSuccess(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponseDto<KanjiDto>>.Fail($"Error retrieving kanjis: {ex.Message}");
            }
        }

        public async Task<ApiResponse<KanjiDto>> GetKanjiByIdAsync(int id)
        {
            try
            {
                var kanji = await _unitOfWork.Kanjis.GetByIdAsync(id);
                if (kanji == null)
                {
                    return ApiResponse<KanjiDto>.Fail("Kanji not found");
                }

                // Get component mappings
                var componentMappings = await _unitOfWork.KanjiComponentMappings.FindAsync(m => m.KanjiID == kanji.KanjiID);
                var componentIds = componentMappings.Select(m => m.ComponentID).ToList();
                var components = await _unitOfWork.KanjiComponents.FindAsync(c => componentIds.Contains(c.ComponentID));

                var componentDtos = new List<KanjiComponentDto>();
                foreach (var component in components)
                {
                    var mapping = componentMappings.FirstOrDefault(m => m.ComponentID == component.ComponentID);
                    componentDtos.Add(new KanjiComponentDto
                    {
                        ComponentID = component.ComponentID,
                        ComponentName = component.ComponentName,
                        Character = component.Character,
                        Meaning = component.Meaning,
                        Type = component.Type,
                        Position = mapping?.Position
                    });
                }

                // Create response
                var kanjiDto = new KanjiDto
                {
                    Id = kanji.KanjiID,
                    Character = kanji.Character,
                    Onyomi = kanji.Onyomi,
                    Kunyomi = kanji.Kunyomi,
                    Meaning = kanji.Meaning,
                    StrokeCount = kanji.StrokeCount,
                    JLPTLevel = kanji.JLPTLevel,
                    Grade = kanji.Grade,
                    Radicals = kanji.Radicals,
                    Components = kanji.Components,
                    Examples = kanji.Examples,
                    MnemonicHint = kanji.MnemonicHint,
                    WritingOrderImage = kanji.WritingOrderImage,
                    CreatedAt = kanji.CreatedAt,
                    UpdatedAt = kanji.UpdatedAt,
                    KanjiComponents = componentDtos
                };

                return ApiResponse<KanjiDto>.CreateSuccess(kanjiDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<KanjiDto>.Fail($"Error retrieving kanji: {ex.Message}");
            }
        }

        public async Task<ApiResponse<KanjiDto>> CreateKanjiAsync(CreateKanjiDto createDto, int currentUserId)
        {
            try
            {
                // Check if kanji character already exists
                var existingKanji = await _unitOfWork.Kanjis.GetFirstOrDefaultAsync(k => k.Character == createDto.Character);
                if (existingKanji != null)
                {
                    return ApiResponse<KanjiDto>.Fail("Kanji character already exists");
                }

                // Create new kanji
                var kanji = new Kanji
                {
                    Character = createDto.Character,
                    Onyomi = createDto.Onyomi,
                    Kunyomi = createDto.Kunyomi,
                    Meaning = createDto.Meaning,
                    StrokeCount = createDto.StrokeCount,
                    JLPTLevel = createDto.JLPTLevel,
                    Grade = createDto.Grade,
                    Radicals = createDto.Radicals,
                    Components = createDto.Components,
                    Examples = createDto.Examples,
                    MnemonicHint = createDto.MnemonicHint,
                    CreatedByUserID = currentUserId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Kanjis.AddAsync(kanji);
                await _unitOfWork.SaveChangesAsync();

                // Add component mappings if provided
                if (createDto.ComponentMappings != null && createDto.ComponentMappings.Any())
                {
                    foreach (var mapping in createDto.ComponentMappings)
                    {
                        // Check if component exists
                        var component = await _unitOfWork.KanjiComponents.GetByIdAsync(mapping.ComponentID);
                        if (component != null)
                        {
                            var componentMapping = new KanjiComponentMapping
                            {
                                KanjiID = kanji.KanjiID,
                                ComponentID = mapping.ComponentID,
                                Position = mapping.Position,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow
                            };
                            await _unitOfWork.KanjiComponentMappings.AddAsync(componentMapping);
                        }
                    }
                    await _unitOfWork.SaveChangesAsync();
                }

                // Get created kanji
                return await GetKanjiByIdAsync(kanji.KanjiID);
            }
            catch (Exception ex)
            {
                return ApiResponse<KanjiDto>.Fail($"Error creating kanji: {ex.Message}");
            }
        }

        public async Task<ApiResponse<KanjiDto>> UpdateKanjiAsync(int id, CreateKanjiDto updateDto, int currentUserId)
        {
            try
            {
                var kanji = await _unitOfWork.Kanjis.GetByIdAsync(id);
                if (kanji == null)
                {
                    return ApiResponse<KanjiDto>.Fail("Kanji not found");
                }

                // Check if new kanji character already exists
                if (updateDto.Character != kanji.Character)
                {
                    var existingKanji = await _unitOfWork.Kanjis.GetFirstOrDefaultAsync(k => k.Character == updateDto.Character);
                    if (existingKanji != null)
                    {
                        return ApiResponse<KanjiDto>.Fail("Kanji character already exists");
                    }
                }

                // Update properties
                kanji.Character = updateDto.Character;
                kanji.Onyomi = updateDto.Onyomi;
                kanji.Kunyomi = updateDto.Kunyomi;
                kanji.Meaning = updateDto.Meaning;
                kanji.StrokeCount = updateDto.StrokeCount;
                kanji.JLPTLevel = updateDto.JLPTLevel;
                kanji.Grade = updateDto.Grade;
                kanji.Radicals = updateDto.Radicals;
                kanji.Components = updateDto.Components;
                kanji.Examples = updateDto.Examples;
                kanji.MnemonicHint = updateDto.MnemonicHint;
                kanji.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.Kanjis.Update(kanji);
                await _unitOfWork.SaveChangesAsync();

                // Update component mappings if provided
                if (updateDto.ComponentMappings != null)
                {
                    // Remove existing mappings
                    var existingMappings = await _unitOfWork.KanjiComponentMappings.FindAsync(m => m.KanjiID == id);
                    if (existingMappings.Any())
                    {
                        _unitOfWork.KanjiComponentMappings.RemoveRange(existingMappings);
                        await _unitOfWork.SaveChangesAsync();
                    }

                    // Add new mappings
                    foreach (var mapping in updateDto.ComponentMappings)
                    {
                        // Check if component exists
                        var component = await _unitOfWork.KanjiComponents.GetByIdAsync(mapping.ComponentID);
                        if (component != null)
                        {
                            var componentMapping = new KanjiComponentMapping
                            {
                                KanjiID = kanji.KanjiID,
                                ComponentID = mapping.ComponentID,
                                Position = mapping.Position,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow
                            };
                            await _unitOfWork.KanjiComponentMappings.AddAsync(componentMapping);
                        }
                    }
                    await _unitOfWork.SaveChangesAsync();
                }

                // Get updated kanji
                return await GetKanjiByIdAsync(kanji.KanjiID);
            }
            catch (Exception ex)
            {
                return ApiResponse<KanjiDto>.Fail($"Error updating kanji: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteKanjiAsync(int id)
        {
            try
            {
                var kanji = await _unitOfWork.Kanjis.GetByIdAsync(id);
                if (kanji == null)
                {
                    return ApiResponse<bool>.Fail("Kanji not found");
                }

                // Check if kanji is used in vocabularies
                var hasVocabularies = await _unitOfWork.KanjiVocabularies.AnyAsync(kv => kv.KanjiID == id);
                if (hasVocabularies)
                {
                    return ApiResponse<bool>.Fail("Cannot delete kanji used in vocabularies");
                }

                // Remove component mappings
                var componentMappings = await _unitOfWork.KanjiComponentMappings.FindAsync(m => m.KanjiID == id);
                if (componentMappings.Any())
                {
                    _unitOfWork.KanjiComponentMappings.RemoveRange(componentMappings);
                    await _unitOfWork.SaveChangesAsync();
                }

                // Remove user kanji progress
                var userKanjiProgresses = await _unitOfWork.UserKanjiProgresses.FindAsync(p => p.KanjiID == id);
                if (userKanjiProgresses.Any())
                {
                    _unitOfWork.UserKanjiProgresses.RemoveRange(userKanjiProgresses);
                    await _unitOfWork.SaveChangesAsync();
                }

                // Delete kanji
                _unitOfWork.Kanjis.Remove(kanji);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.CreateSuccess(true, "Kanji deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"Error deleting kanji: {ex.Message}");
            }
        }
    }
}