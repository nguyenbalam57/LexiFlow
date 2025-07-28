//using LexiFlow.Core.Interfaces;
//using LexiFlow.Models;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LexiFlow.Core.Services
//{
//    /// <summary>
//    /// Dịch vụ đồng bộ dữ liệu giữa client và server
//    /// </summary>
//    public class SyncService : ISyncService
//    {
//        //private readonly ApplicationDbContext _dbContext;
//        private readonly ILogger<SyncService> _logger;

//        /// <summary>
//        /// Khởi tạo SyncService
//        /// </summary>
//        public SyncService(
//            //ApplicationDbContext dbContext,
//            ILogger<SyncService> logger)
//        {
//            //_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
//            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//        }

//        /// <summary>
//        /// Đồng bộ dữ liệu từ vựng
//        /// </summary>
//        public async Task<SyncResult<Vocabulary>> SyncVocabularyAsync(VocabularySyncRequest request, int userId)
//        {
//            try
//            {
//                _logger.LogInformation("Bắt đầu đồng bộ từ vựng cho user {UserId}, {Count} items", userId, request.Items.Count);

//                var result = new SyncResult<Vocabulary>();

//                // Tạo từ điển cho các mục từ client để dễ tìm kiếm
//                var clientItems = request.Items.ToDictionary(item => item.VocabularyID);

//                // Lấy tất cả ID từ vựng từ client
//                var clientItemIds = clientItems.Keys.ToList();

//                // Lấy thời gian đồng bộ lần cuối
//                var lastSyncTime = request.LastSyncTime ?? DateTime.MinValue;

//                // Lấy tất cả từ vựng từ DB đã được cập nhật sau lần đồng bộ cuối cùng
//                // hoặc thuộc về các ID mà client đã gửi
//                var serverItems = await _dbContext.Vocabulary
//                    .Where(v => v.UpdatedAt > lastSyncTime || clientItemIds.Contains(v.VocabularyID))
//                    .ToListAsync();

//                // Tạo từ điển cho các mục từ server để dễ tìm kiếm
//                var serverItemsDict = serverItems.ToDictionary(item => item.VocabularyID);

//                // Tìm danh sách ID từ vựng đã bị xóa trên server sau lần đồng bộ cuối
//                var deletedIds = await _dbContext.DeletedItems
//                    .Where(d => d.EntityType == "Vocabulary" && d.DeletedAt > lastSyncTime)
//                    .Select(d => d.EntityId)
//                    .ToListAsync();

//                // Xử lý từng mục từ client
//                foreach (var clientItem in request.Items)
//                {
//                    // Nếu mục đã bị xóa trên server, bỏ qua và thêm vào danh sách xóa
//                    if (deletedIds.Contains(clientItem.VocabularyID))
//                    {
//                        result.DeletedItemIds.Add(clientItem.VocabularyID);
//                        result.DeletedCount++;
//                        continue;
//                    }

//                    // Kiểm tra xem mục có tồn tại trên server không
//                    if (serverItemsDict.TryGetValue(clientItem.VocabularyID, out var serverItem))
//                    {
//                        // Mục tồn tại trên cả client và server, kiểm tra xung đột
//                        if (clientItem.UpdatedAt > lastSyncTime && serverItem.UpdatedAt > lastSyncTime)
//                        {
//                            // Cả client và server đều đã cập nhật mục này, phát hiện xung đột
//                            result.Conflicts.Add(new SyncConflict<Vocabulary>
//                            {
//                                ItemId = clientItem.VocabularyID,
//                                ClientVersion = clientItem,
//                                ServerVersion = serverItem,
//                                ClientUpdateTime = clientItem.UpdatedAt,
//                                ServerUpdateTime = serverItem.UpdatedAt,
//                                ConflictType = ConflictType.BothModified
//                            });
//                        }
//                        else if (clientItem.UpdatedAt > serverItem.UpdatedAt)
//                        {
//                            // Client có phiên bản mới hơn, cập nhật server
//                            serverItem.Japanese = clientItem.Japanese;
//                            serverItem.Meaning = clientItem.Meaning;
//                            serverItem.Reading = clientItem.Reading;
//                            serverItem.Example = clientItem.Example;
//                            serverItem.Notes = clientItem.Notes;
//                            serverItem.Level = clientItem.Level;
//                            serverItem.UpdatedAt = DateTime.UtcNow;

//                            _dbContext.Vocabulary.Update(serverItem);
//                            result.UpdatedCount++;
//                        }
//                        else
//                        {
//                            // Server có phiên bản mới hơn, thêm vào danh sách cập nhật cho client
//                            result.UpdatedItems.Add(serverItem);
//                            result.UpdatedCount++;
//                        }
//                    }
//                    else
//                    {
//                        // Mục không tồn tại trên server, thêm mới
//                        // Reset ID nếu là ID tạm thời từ client (ID âm)
//                        if (clientItem.VocabularyID < 0)
//                        {
//                            clientItem.VocabularyID = 0; // ID sẽ được DB tự động tạo
//                        }

//                        clientItem.CreatedAt = DateTime.UtcNow;
//                        clientItem.UpdatedAt = DateTime.UtcNow;

//                        _dbContext.Vocabulary.Add(clientItem);
//                        result.AddedCount++;
//                    }
//                }

//                // Tìm các mục trên server đã được cập nhật sau lần đồng bộ cuối
//                // nhưng không có trong danh sách từ client
//                var newServerItems = serverItems
//                    .Where(s => s.UpdatedAt > lastSyncTime && !clientItemIds.Contains(s.VocabularyID))
//                    .ToList();

//                // Thêm các mục mới/cập nhật vào kết quả để gửi về client
//                result.UpdatedItems.AddRange(newServerItems);
//                result.UpdatedCount += newServerItems.Count;

//                // Thêm các ID đã xóa vào kết quả để client cũng xóa
//                result.DeletedItemIds.AddRange(deletedIds.Where(id => !result.DeletedItemIds.Contains(id)));
//                result.DeletedCount = result.DeletedItemIds.Count;

//                // Lưu thay đổi vào DB
//                await _dbContext.SaveChangesAsync();

//                // Cập nhật trạng thái đồng bộ
//                if (result.Conflicts.Any())
//                {
//                    result.Status = SyncStatus.PartialSuccess;
//                    result.Message = $"Đồng bộ hoàn tất với {result.Conflicts.Count} xung đột cần giải quyết.";
//                }
//                else
//                {
//                    result.Status = SyncStatus.Success;
//                    result.Message = "Đồng bộ từ vựng thành công.";
//                }

//                result.SyncTime = DateTime.UtcNow;

//                _logger.LogInformation(
//                    "Đồng bộ từ vựng hoàn tất cho user {UserId}: {Added} thêm mới, {Updated} cập nhật, {Deleted} xóa, {Conflicts} xung đột",
//                    userId, result.AddedCount, result.UpdatedCount, result.DeletedCount, result.Conflicts.Count);

//                return result;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi đồng bộ từ vựng cho user {UserId}", userId);
//                return new SyncResult<Vocabulary>
//                {
//                    Status = SyncStatus.Failed,
//                    Message = $"Đồng bộ thất bại: {ex.Message}"
//                };
//            }
//        }

//        /// <summary>
//        /// Đồng bộ dữ liệu kanji
//        /// </summary>
//        public async Task<SyncResult<Kanji>> SyncKanjiAsync(KanjiSyncRequest request, int userId)
//        {
//            try
//            {
//                _logger.LogInformation("Bắt đầu đồng bộ kanji cho user {UserId}, {Count} items", userId, request.Items.Count);

//                var result = new SyncResult<Kanji>();

//                // Tương tự như đồng bộ từ vựng, thực hiện các bước đồng bộ cho kanji
//                // Phần code này sẽ tương tự như SyncVocabularyAsync với các trường dữ liệu tương ứng của Kanji

//                // Cài đặt mẫu, cần điều chỉnh theo cấu trúc dữ liệu Kanji thực tế
//                result.Status = SyncStatus.Success;
//                result.Message = "Đồng bộ kanji thành công.";
//                result.SyncTime = DateTime.UtcNow;

//                _logger.LogInformation(
//                    "Đồng bộ kanji hoàn tất cho user {UserId}: {Added} thêm mới, {Updated} cập nhật, {Deleted} xóa, {Conflicts} xung đột",
//                    userId, result.AddedCount, result.UpdatedCount, result.DeletedCount, result.Conflicts.Count);

//                return result;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi đồng bộ kanji cho user {UserId}", userId);
//                return new SyncResult<Kanji>
//                {
//                    Status = SyncStatus.Failed,
//                    Message = $"Đồng bộ thất bại: {ex.Message}"
//                };
//            }
//        }

//        /// <summary>
//        /// Đồng bộ dữ liệu ngữ pháp
//        /// </summary>
//        public async Task<SyncResult<Grammar>> SyncGrammarAsync(GrammarSyncRequest request, int userId)
//        {
//            try
//            {
//                _logger.LogInformation("Bắt đầu đồng bộ ngữ pháp cho user {UserId}, {Count} items", userId, request.Items.Count);

//                var result = new SyncResult<Grammar>();

//                // Tương tự như đồng bộ từ vựng, thực hiện các bước đồng bộ cho ngữ pháp
//                // Phần code này sẽ tương tự như SyncVocabularyAsync với các trường dữ liệu tương ứng của Grammar

//                // Cài đặt mẫu, cần điều chỉnh theo cấu trúc dữ liệu Grammar thực tế
//                result.Status = SyncStatus.Success;
//                result.Message = "Đồng bộ ngữ pháp thành công.";
//                result.SyncTime = DateTime.UtcNow;

//                _logger.LogInformation(
//                    "Đồng bộ ngữ pháp hoàn tất cho user {UserId}: {Added} thêm mới, {Updated} cập nhật, {Deleted} xóa, {Conflicts} xung đột",
//                    userId, result.AddedCount, result.UpdatedCount, result.DeletedCount, result.Conflicts.Count);

//                return result;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi đồng bộ ngữ pháp cho user {UserId}", userId);
//                return new SyncResult<Grammar>
//                {
//                    Status = SyncStatus.Failed,
//                    Message = $"Đồng bộ thất bại: {ex.Message}"
//                };
//            }
//        }

//        /// <summary>
//        /// Đồng bộ tiến trình học tập của người dùng
//        /// </summary>
//        public async Task<SyncResult<LearningProgress>> SyncLearningProgressAsync(LearningProgressSyncRequest request, int userId)
//        {
//            try
//            {
//                _logger.LogInformation("Bắt đầu đồng bộ tiến trình học tập cho user {UserId}, {Count} items", userId, request.Items.Count);

//                var result = new SyncResult<LearningProgress>();

//                // Tương tự như đồng bộ từ vựng, thực hiện các bước đồng bộ cho tiến trình học tập
//                // Phần code này sẽ tương tự như SyncVocabularyAsync với các trường dữ liệu tương ứng của LearningProgress

//                // Cài đặt mẫu, cần điều chỉnh theo cấu trúc dữ liệu LearningProgress thực tế
//                result.Status = SyncStatus.Success;
//                result.Message = "Đồng bộ tiến trình học tập thành công.";
//                result.SyncTime = DateTime.UtcNow;

//                _logger.LogInformation(
//                    "Đồng bộ tiến trình học tập hoàn tất cho user {UserId}: {Added} thêm mới, {Updated} cập nhật, {Deleted} xóa, {Conflicts} xung đột",
//                    userId, result.AddedCount, result.UpdatedCount, result.DeletedCount, result.Conflicts.Count);

//                return result;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi đồng bộ tiến trình học tập cho user {UserId}", userId);
//                return new SyncResult<LearningProgress>
//                {
//                    Status = SyncStatus.Failed,
//                    Message = $"Đồng bộ thất bại: {ex.Message}"
//                };
//            }
//        }

//        /// <summary>
//        /// Đồng bộ danh sách từ vựng cá nhân
//        /// </summary>
//        public async Task<SyncResult<PersonalWordList>> SyncPersonalWordListsAsync(PersonalWordListSyncRequest request, int userId)
//        {
//            try
//            {
//                _logger.LogInformation("Bắt đầu đồng bộ danh sách từ vựng cá nhân cho user {UserId}, {Count} items", userId, request.Items.Count);

//                var result = new SyncResult<PersonalWordList>();

//                // Tương tự như đồng bộ từ vựng, thực hiện các bước đồng bộ cho danh sách từ vựng cá nhân
//                // Phần code này sẽ tương tự như SyncVocabularyAsync với các trường dữ liệu tương ứng của PersonalWordList

//                // Cài đặt mẫu, cần điều chỉnh theo cấu trúc dữ liệu PersonalWordList thực tế
//                result.Status = SyncStatus.Success;
//                result.Message = "Đồng bộ danh sách từ vựng cá nhân thành công.";
//                result.SyncTime = DateTime.UtcNow;

//                _logger.LogInformation(
//                    "Đồng bộ danh sách từ vựng cá nhân hoàn tất cho user {UserId}: {Added} thêm mới, {Updated} cập nhật, {Deleted} xóa, {Conflicts} xung đột",
//                    userId, result.AddedCount, result.UpdatedCount, result.DeletedCount, result.Conflicts.Count);

//                return result;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi đồng bộ danh sách từ vựng cá nhân cho user {UserId}", userId);
//                return new SyncResult<PersonalWordList>
//                {
//                    Status = SyncStatus.Failed,
//                    Message = $"Đồng bộ thất bại: {ex.Message}"
//                };
//            }
//        }

//        /// <summary>
//        /// Đồng bộ tất cả dữ liệu cho người dùng
//        /// </summary>
//        public async Task<FullSyncResult> SyncAllAsync(int userId, DateTime? lastSyncTime = null)
//        {
//            try
//            {
//                _logger.LogInformation("Bắt đầu đồng bộ toàn bộ dữ liệu cho user {UserId}", userId);

//                var result = new FullSyncResult();
//                int totalItems = 0;

//                // Lấy thông tin đồng bộ của người dùng
//                var syncInfo = await GetSyncInfoAsync(userId);

//                // Sử dụng thời gian đồng bộ lần cuối từ tham số hoặc từ thông tin đồng bộ
//                var syncTime = lastSyncTime ?? syncInfo.LastSyncTime ?? DateTime.MinValue;

//                // Đồng bộ từ vựng
//                var vocabRequest = new VocabularySyncRequest
//                {
//                    LastSyncTime = syncTime,
//                    Items = new List<Vocabulary>() // Không có dữ liệu từ client trong full sync
//                };
//                result.VocabularyResult = await SyncVocabularyAsync(vocabRequest, userId);
//                totalItems += result.VocabularyResult.UpdatedItems.Count;

//                // Đồng bộ kanji
//                var kanjiRequest = new KanjiSyncRequest
//                {
//                    LastSyncTime = syncTime,
//                    Items = new List<Kanji>() // Không có dữ liệu từ client trong full sync
//                };
//                result.KanjiResult = await SyncKanjiAsync(kanjiRequest, userId);
//                totalItems += result.KanjiResult.UpdatedItems.Count;

//                // Đồng bộ ngữ pháp
//                var grammarRequest = new GrammarSyncRequest
//                {
//                    LastSyncTime = syncTime,
//                    Items = new List<Grammar>() // Không có dữ liệu từ client trong full sync
//                };
//                result.GrammarResult = await SyncGrammarAsync(grammarRequest, userId);
//                totalItems += result.GrammarResult.UpdatedItems.Count;

//                // Đồng bộ tiến trình học tập
//                var learningRequest = new LearningProgressSyncRequest
//                {
//                    LastSyncTime = syncTime,
//                    Items = new List<LearningProgress>() // Không có dữ liệu từ client trong full sync
//                };
//                result.LearningProgressResult = await SyncLearningProgressAsync(learningRequest, userId);
//                totalItems += result.LearningProgressResult.UpdatedItems.Count;

//                // Đồng bộ danh sách từ vựng cá nhân
//                var personalListRequest = new PersonalWordListSyncRequest
//                {
//                    LastSyncTime = syncTime,
//                    Items = new List<PersonalWordList>() // Không có dữ liệu từ client trong full sync
//                };
//                result.PersonalWordListResult = await SyncPersonalWordListsAsync(personalListRequest, userId);
//                totalItems += result.PersonalWordListResult.UpdatedItems.Count;

//                // Cập nhật kết quả tổng thể
//                result.TotalItemsProcessed = totalItems;
//                result.SyncTime = DateTime.UtcNow;

//                // Kiểm tra xem có xung đột nào không
//                bool hasConflicts = result.VocabularyResult.Conflicts.Any() ||
//                                    result.KanjiResult.Conflicts.Any() ||
//                                    result.GrammarResult.Conflicts.Any() ||
//                                    result.LearningProgressResult.Conflicts.Any() ||
//                                    result.PersonalWordListResult.Conflicts.Any();

//                if (hasConflicts)
//                {
//                    result.Status = SyncStatus.PartialSuccess;
//                    result.Message = "Đồng bộ hoàn tất với một số xung đột cần giải quyết.";
//                }
//                else
//                {
//                    result.Status = SyncStatus.Success;
//                    result.Message = "Đồng bộ toàn bộ dữ liệu thành công.";
//                }

//                // Cập nhật thông tin đồng bộ của người dùng
//                await UpdateUserSyncInfoAsync(userId, result.SyncTime, result.Status, totalItems);

//                _logger.LogInformation("Đồng bộ toàn bộ dữ liệu hoàn tất cho user {UserId}: {TotalItems} mục", userId, totalItems);

//                return result;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi đồng bộ toàn bộ dữ liệu cho user {UserId}", userId);
//                return new FullSyncResult
//                {
//                    Status = SyncStatus.Failed,
//                    Message = $"Đồng bộ thất bại: {ex.Message}",
//                    SyncTime = DateTime.UtcNow
//                };
//            }
//        }

//        /// <summary>
//        /// Lấy thông tin đồng bộ cho người dùng
//        /// </summary>
//        public async Task<SyncInfo> GetSyncInfoAsync(int userId)
//        {
//            try
//            {
//                _logger.LogInformation("Lấy thông tin đồng bộ cho user {UserId}", userId);

//                // Lấy thông tin đồng bộ từ DB
//                var syncMetadata = await _dbContext.SyncMetadata
//                    .FirstOrDefaultAsync(s => s.UserID == userId);

//                if (syncMetadata == null)
//                {
//                    // Tạo mới nếu chưa có
//                    return new SyncInfo
//                    {
//                        UserId = userId,
//                        LastSyncTime = null,
//                        Status = SyncStatus.Success,
//                        TotalItemsSynced = 0,
//                        UnresolvedConflicts = 0,
//                        NeedsFullSync = true,
//                        Message = "Chưa có dữ liệu đồng bộ. Hãy thực hiện đồng bộ đầy đủ."
//                    };
//                }

//                // Lấy số lượng xung đột chưa giải quyết
//                var unresolvedConflicts = await _dbContext.SyncConflicts
//                    .CountAsync(c => c.UserID == userId && !c.IsResolved);

//                return new SyncInfo
//                {
//                    UserId = userId,
//                    LastSyncTime = syncMetadata.LastSyncTime,
//                    Status = Enum.Parse<SyncStatus>(syncMetadata.Status),
//                    TotalItemsSynced = syncMetadata.TotalItemsSynced,
//                    UnresolvedConflicts = unresolvedConflicts,
//                    NeedsFullSync = syncMetadata.NeedsFullSync,
//                    Message = syncMetadata.Message
//                };
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi lấy thông tin đồng bộ cho user {UserId}", userId);
//                throw;
//            }
//        }

//        /// <summary>
//        /// Đặt lại trạng thái đồng bộ cho người dùng
//        /// </summary>
//        public async Task<ApiResponse> ResetSyncStatusAsync(int userId)
//        {
//            try
//            {
//                _logger.LogInformation("Đặt lại trạng thái đồng bộ cho user {UserId}", userId);

//                // Lấy thông tin đồng bộ từ DB
//                var syncMetadata = await _dbContext.SyncMetadata
//                    .FirstOrDefaultAsync(s => s.UserID == userId);

//                if (syncMetadata == null)
//                {
//                    // Tạo mới nếu chưa có
//                    syncMetadata = new SyncMetadata
//                    {
//                        UserID = userId,
//                        LastSyncTime = null,
//                        Status = SyncStatus.Success.ToString(),
//                        TotalItemsSynced = 0,
//                        NeedsFullSync = true,
//                        Message = "Đặt lại trạng thái đồng bộ thành công.",
//                        CreatedAt = DateTime.UtcNow,
//                        UpdatedAt = DateTime.UtcNow
//                    };

//                    _dbContext.SyncMetadata.Add(syncMetadata);
//                }
//                else
//                {
//                    // Cập nhật thông tin hiện có
//                    syncMetadata.LastSyncTime = null;
//                    syncMetadata.Status = SyncStatus.Success.ToString();
//                    syncMetadata.TotalItemsSynced = 0;
//                    syncMetadata.NeedsFullSync = true;
//                    syncMetadata.Message = "Đặt lại trạng thái đồng bộ thành công.";
//                    syncMetadata.UpdatedAt = DateTime.UtcNow;

//                    _dbContext.SyncMetadata.Update(syncMetadata);
//                }

//                // Xóa tất cả các xung đột chưa giải quyết
//                var conflicts = await _dbContext.SyncConflicts
//                    .Where(c => c.UserID == userId)
//                    .ToListAsync();

//                if (conflicts.Any())
//                {
//                    _dbContext.SyncConflicts.RemoveRange(conflicts);
//                }

//                await _dbContext.SaveChangesAsync();

//                _logger.LogInformation("Đặt lại trạng thái đồng bộ thành công cho user {UserId}", userId);

//                return new ApiResponse
//                {
//                    Success = true,
//                    Message = "Đặt lại trạng thái đồng bộ thành công."
//                };
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi đặt lại trạng thái đồng bộ cho user {UserId}", userId);
//                return new ApiResponse
//                {
//                    Success = false,
//                    Message = $"Đặt lại trạng thái đồng bộ thất bại: {ex.Message}"
//                };
//            }
//        }

//        /// <summary>
//        /// Giải quyết xung đột đồng bộ
//        /// </summary>
//        public async Task<ApiResponse> ResolveSyncConflictsAsync(SyncConflictResolutionRequest request, int userId)
//        {
//            try
//            {
//                _logger.LogInformation("Giải quyết xung đột đồng bộ cho user {UserId}, {Count} xung đột", userId, request.Resolutions.Count);

//                foreach (var resolution in request.Resolutions)
//                {
//                    // Lấy thông tin xung đột
//                    var conflict = await _dbContext.SyncConflicts
//                        .FirstOrDefaultAsync(c => c.ConflictID == resolution.ConflictId && c.UserID == userId);

//                    if (conflict == null)
//                    {
//                        _logger.LogWarning("Không tìm thấy xung đột {ConflictId} cho user {UserId}", resolution.ConflictId, userId);
//                        continue;
//                    }

//                    // Giải quyết xung đột dựa trên lựa chọn của người dùng
//                    switch (resolution.Resolution)
//                    {
//                        case ConflictResolution.UseClientVersion:
//                            await ResolveConflictWithClientVersion(conflict);
//                            break;
//                        case ConflictResolution.UseServerVersion:
//                            await ResolveConflictWithServerVersion(conflict);
//                            break;
//                        case ConflictResolution.UseCustomVersion:
//                            await ResolveConflictWithCustomVersion(conflict, resolution.CustomData);
//                            break;
//                        case ConflictResolution.DeleteItem:
//                            await ResolveConflictWithDeletion(conflict);
//                            break;
//                        default:
//                            _logger.LogWarning("Phương thức giải quyết không hợp lệ: {Resolution}", resolution.Resolution);
//                            continue;
//                    }

//                    // Đánh dấu xung đột đã được giải quyết
//                    conflict.IsResolved = true;
//                    conflict.ResolvedAt = DateTime.UtcNow;
//                    conflict.ResolutionMethod = resolution.Resolution.ToString();

//                    _dbContext.SyncConflicts.Update(conflict);
//                }

//                await _dbContext.SaveChangesAsync();

//                _logger.LogInformation("Giải quyết xung đột đồng bộ thành công cho user {UserId}", userId);

//                return new ApiResponse
//                {
//                    Success = true,
//                    Message = "Giải quyết xung đột đồng bộ thành công."
//                };
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi giải quyết xung đột đồng bộ cho user {UserId}", userId);
//                return new ApiResponse
//                {
//                    Success = false,
//                    Message = $"Giải quyết xung đột đồng bộ thất bại: {ex.Message}"
//                };
//            }
//        }

//        #region Helper Methods

//        /// <summary>
//        /// Cập nhật thông tin đồng bộ của người dùng
//        /// </summary>
//        private async Task UpdateUserSyncInfoAsync(int userId, DateTime syncTime, SyncStatus status, int totalItems)
//        {
//            try
//            {
//                var syncMetadata = await _dbContext.SyncMetadata
//                    .FirstOrDefaultAsync(s => s.UserID == userId);

//                if (syncMetadata == null)
//                {
//                    // Tạo mới nếu chưa có
//                    syncMetadata = new SyncMetadata
//                    {
//                        UserID = userId,
//                        LastSyncTime = syncTime,
//                        Status = status.ToString(),
//                        TotalItemsSynced = totalItems,
//                        NeedsFullSync = false,
//                        Message = "Đồng bộ thành công.",
//                        CreatedAt = DateTime.UtcNow,
//                        UpdatedAt = DateTime.UtcNow
//                    };

//                    _dbContext.SyncMetadata.Add(syncMetadata);
//                }
//                else
//                {
//                    // Cập nhật thông tin hiện có
//                    syncMetadata.LastSyncTime = syncTime;
//                    syncMetadata.Status = status.ToString();
//                    syncMetadata.TotalItemsSynced += totalItems;
//                    syncMetadata.NeedsFullSync = false;
//                    syncMetadata.Message = "Đồng bộ thành công.";
//                    syncMetadata.UpdatedAt = DateTime.UtcNow;

//                    _dbContext.SyncMetadata.Update(syncMetadata);
//                }

//                await _dbContext.SaveChangesAsync();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi cập nhật thông tin đồng bộ cho user {UserId}", userId);
//                throw;
//            }
//        }

//        /// <summary>
//        /// Giải quyết xung đột bằng cách sử dụng phiên bản từ client
//        /// </summary>
//        private async Task ResolveConflictWithClientVersion(SyncConflict conflict)
//        {
//            // Phần này sẽ được triển khai dựa trên cấu trúc dữ liệu thực tế của xung đột
//            // Ví dụ:
//            // 1. Phân tích dữ liệu xung đột để xác định loại entity (Vocabulary, Kanji, etc.)
//            // 2. Lấy dữ liệu phiên bản client từ xung đột
//            // 3. Cập nhật entity tương ứng trong database

//            // Mẫu triển khai:
//            if (conflict.EntityType == "Vocabulary")
//            {
//                // Lấy entity từ DB
//                var entity = await _dbContext.Vocabulary.FindAsync(conflict.EntityId);
//                if (entity != null)
//                {
//                    // Cập nhật với dữ liệu client (cần phân tích ClientData)
//                    // entity.PropertyName = clientData.PropertyName;

//                    entity.UpdatedAt = DateTime.UtcNow;
//                    _dbContext.Vocabulary.Update(entity);
//                }
//            }
//            // Tương tự cho các loại entity khác...
//        }

//        /// <summary>
//        /// Giải quyết xung đột bằng cách sử dụng phiên bản từ server
//        /// </summary>
//        private async Task ResolveConflictWithServerVersion(SyncConflict conflict)
//        {
//            // Không cần làm gì vì dữ liệu server đã là phiên bản hiện tại
//            // Chỉ cần đánh dấu xung đột đã được giải quyết
//        }

//        /// <summary>
//        /// Giải quyết xung đột bằng cách sử dụng phiên bản tùy chỉnh
//        /// </summary>
//        private async Task ResolveConflictWithCustomVersion(SyncConflict conflict, string customData)
//        {
//            // Phần này sẽ được triển khai dựa trên cấu trúc dữ liệu thực tế của xung đột
//            // Ví dụ:
//            // 1. Phân tích dữ liệu xung đột để xác định loại entity (Vocabulary, Kanji, etc.)
//            // 2. Phân tích customData để lấy dữ liệu tùy chỉnh
//            // 3. Cập nhật entity tương ứng trong database

//            // Mẫu triển khai:
//            if (conflict.EntityType == "Vocabulary" && !string.IsNullOrEmpty(customData))
//            {
//                // Lấy entity từ DB
//                var entity = await _dbContext.Vocabulary.FindAsync(conflict.EntityId);
//                if (entity != null)
//                {
//                    // Phân tích customData (giả sử là JSON)
//                    var customEntity = System.Text.Json.JsonSerializer.Deserialize<Vocabulary>(customData);

//                    // Cập nhật với dữ liệu tùy chỉnh
//                    // entity.PropertyName = customEntity.PropertyName;

//                    entity.UpdatedAt = DateTime.UtcNow;
//                    _dbContext.Vocabulary.Update(entity);
//                }
//            }
//            // Tương tự cho các loại entity khác...
//        }

//        /// <summary>
//        /// Giải quyết xung đột bằng cách xóa mục
//        /// </summary>
//        private async Task ResolveConflictWithDeletion(SyncConflict conflict)
//        {
//            // Phần này sẽ được triển khai dựa trên cấu trúc dữ liệu thực tế của xung đột
//            // Ví dụ:
//            // 1. Phân tích dữ liệu xung đột để xác định loại entity (Vocabulary, Kanji, etc.)
//            // 2. Xóa entity tương ứng trong database
//            // 3. Thêm bản ghi vào bảng DeletedItems để thông báo cho clients

//            // Mẫu triển khai:
//            if (conflict.EntityType == "Vocabulary")
//            {
//                // Lấy entity từ DB
//                var entity = await _dbContext.Vocabulary.FindAsync(conflict.EntityId);
//                if (entity != null)
//                {
//                    // Xóa entity
//                    _dbContext.Vocabulary.Remove(entity);

//                    // Thêm vào bảng DeletedItems
//                    _dbContext.DeletedItems.Add(new DeletedItem
//                    {
//                        EntityId = conflict.EntityId,
//                        EntityType = "Vocabulary",
//                        DeletedAt = DateTime.UtcNow,
//                        UserID = conflict.UserID
//                    });
//                }
//            }
//            // Tương tự cho các loại entity khác...
//        }

//        #endregion
//    }

//    #region Request Models for Sync API

//    /// <summary>
//    /// Request model cho đồng bộ từ vựng
//    /// </summary>
//    public class VocabularySyncRequest
//    {
//        /// <summary>
//        /// Thời gian đồng bộ lần cuối
//        /// </summary>
//        public DateTime? LastSyncTime { get; set; }

//        /// <summary>
//        /// Danh sách từ vựng cần đồng bộ
//        /// </summary>
//        public List<Vocabulary> Items { get; set; } = new List<Vocabulary>();
//    }

//    /// <summary>
//    /// Request model cho đồng bộ kanji
//    /// </summary>
//    public class KanjiSyncRequest
//    {
//        /// <summary>
//        /// Thời gian đồng bộ lần cuối
//        /// </summary>
//        public DateTime? LastSyncTime { get; set; }

//        /// <summary>
//        /// Danh sách kanji cần đồng bộ
//        /// </summary>
//        public List<Kanji> Items { get; set; } = new List<Kanji>();
//    }

//    /// <summary>
//    /// Request model cho đồng bộ ngữ pháp
//    /// </summary>
//    public class GrammarSyncRequest
//    {
//        /// <summary>
//        /// Thời gian đồng bộ lần cuối
//        /// </summary>
//        public DateTime? LastSyncTime { get; set; }

//        /// <summary>
//        /// Danh sách ngữ pháp cần đồng bộ
//        /// </summary>
//        public List<Grammar> Items { get; set; } = new List<Grammar>();
//    }

//    /// <summary>
//    /// Request model cho đồng bộ tiến trình học tập
//    /// </summary>
//    public class LearningProgressSyncRequest
//    {
//        /// <summary>
//        /// Thời gian đồng bộ lần cuối
//        /// </summary>
//        public DateTime? LastSyncTime { get; set; }

//        /// <summary>
//        /// Danh sách tiến trình học tập cần đồng bộ
//        /// </summary>
//        public List<LearningProgress> Items { get; set; } = new List<LearningProgress>();
//    }

//    /// <summary>
//    /// Request model cho đồng bộ danh sách từ vựng cá nhân
//    /// </summary>
//    public class PersonalWordListSyncRequest
//    {
//        /// <summary>
//        /// Thời gian đồng bộ lần cuối
//        /// </summary>
//        public DateTime? LastSyncTime { get; set; }

//        /// <summary>
//        /// Danh sách từ vựng cá nhân cần đồng bộ
//        /// </summary>
//        public List<PersonalWordList> Items { get; set; } = new List<PersonalWordList>();
//    }

//    /// <summary>
//    /// Request model cho giải quyết xung đột đồng bộ
//    /// </summary>
//    public class SyncConflictResolutionRequest
//    {
//        /// <summary>
//        /// Danh sách các quyết định giải quyết xung đột
//        /// </summary>
//        public List<ConflictResolutionItem> Resolutions { get; set; } = new List<ConflictResolutionItem>();
//    }

//    #endregion
//}
