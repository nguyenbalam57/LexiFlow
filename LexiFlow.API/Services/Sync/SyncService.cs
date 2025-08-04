//using LexiFlow.API.Interfaces.Sync;
//using LexiFlow.Models;
//using LexiFlow.API.Data;
//using LexiFlow.API.DTOs.Sync;
//using LexiFlow.API.DTOs.Auth;
//using Microsoft.EntityFrameworkCore;

//namespace LexiFlow.API.Services.Sync
//{
//    /// <summary>
//    /// Dịch vụ đồng bộ dữ liệu giữa client và server
//    /// </summary>
//    public class SyncService : ISyncService
//    {
//        private readonly LexiFlow.Infrastructure.Data.LexiFlowContext _dbContext;
//        private readonly ILogger<SyncService> _logger;

//        /// <summary>
//        /// Khởi tạo SyncService
//        /// </summary>
//        public SyncService(
//            LexiFlow.Infrastructure.Data.LexiFlowContext dbContext,
//            ILogger<SyncService> logger)
//        {
//            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
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
//                var clientItems = request.Items.ToDictionary(item => item.Id);

//                // Lấy tất cả ID từ vựng từ client
//                var clientItemIds = clientItems.Keys.ToList();

//                // Lấy thời gian đồng bộ lần cuối
//                var lastSyncTime = request.LastSyncTime ?? DateTime.MinValue;

//                // Lấy tất cả từ vựng từ DB đã được cập nhật sau lần đồng bộ cuối cùng
//                // hoặc thuộc về các ID mà client đã gửi
//                var serverItems = await _dbContext.Vocabularies
//                    .Where(v => v.ModifiedAt > lastSyncTime || clientItemIds.Contains(v.Id))
//                    .ToListAsync();

//                // Tạo từ điển cho các mục từ server để dễ tìm kiếm
//                var serverItemsDict = serverItems.ToDictionary(item => item.Id);

//                // Tìm danh sách ID từ vựng đã bị xóa trên server sau lần đồng bộ cuối
//                var deletedIds = await _dbContext.DeletedItems
//                    .Where(d => d.EntityType == "Vocabulary" && d.DeletedAt > lastSyncTime)
//                    .Select(d => d.EntityId)
//                    .ToListAsync();

//                // Xử lý từng mục từ client
//                foreach (var clientItem in request.Items)
//                {
//                    // Nếu mục đã bị xóa trên server, bỏ qua và thêm vào danh sách xóa
//                    if (deletedIds.Contains(clientItem.Id))
//                    {
//                        result.DeletedItemIds.Add(clientItem.Id);
//                        result.DeletedCount++;
//                        continue;
//                    }

//                    // Kiểm tra xem mục có tồn tại trên server không
//                    if (serverItemsDict.TryGetValue(clientItem.Id, out var serverItem))
//                    {
//                        // Mục tồn tại trên cả client và server, kiểm tra xung đột
//                        if (clientItem.ModifiedAt > lastSyncTime && serverItem.ModifiedAt > lastSyncTime)
//                        {
//                            // Cả client và server đều đã cập nhật mục này, phát hiện xung đột
//                            result.Conflicts.Add(new SyncConflict<Vocabulary>
//                            {
//                                ItemId = clientItem.Id,
//                                ClientVersion = clientItem,
//                                ServerVersion = serverItem,
//                                ClientUpdateTime = clientItem.ModifiedAt,
//                                ServerUpdateTime = serverItem.ModifiedAt,
//                                ConflictType = ConflictType.BothModified
//                            });
//                        }
//                        else if (clientItem.ModifiedAt > serverItem.ModifiedAt)
//                        {
//                            // Client có phiên bản mới hơn, cập nhật server
//                            serverItem.Term = clientItem.Term;
//                            serverItem.Reading = clientItem.Reading;
//                            serverItem.DifficultyLevel = clientItem.DifficultyLevel;
//                            serverItem.Notes = clientItem.Notes;
//                            serverItem.Tags = clientItem.Tags;
//                            serverItem.AudioFile = clientItem.AudioFile;
//                            serverItem.Status = clientItem.Status;
//                            serverItem.ModifiedBy = userId;
//                            serverItem.ModifiedAt = DateTime.UtcNow;

//                            _dbContext.Vocabularies.Update(serverItem);
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
//                        if (clientItem.Id < 0)
//                        {
//                            clientItem.Id = 0; // ID sẽ được DB tự động tạo
//                        }

//                        clientItem.CreatedBy = userId;
//                        clientItem.ModifiedBy = userId;
//                        clientItem.CreatedAt = DateTime.UtcNow;
//                        clientItem.ModifiedAt = DateTime.UtcNow;

//                        _dbContext.Vocabularies.Add(clientItem);
//                        result.AddedCount++;
//                    }
//                }

//                // Tìm các mục trên server đã được cập nhật sau lần đồng bộ cuối
//                // nhưng không có trong danh sách từ client
//                var newServerItems = serverItems
//                    .Where(s => s.ModifiedAt > lastSyncTime && !clientItemIds.Contains(s.Id))
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

//                // Tạo từ điển cho các mục từ client để dễ tìm kiếm
//                var clientItems = request.Items.ToDictionary(item => item.KanjiID);

//                // Lấy tất cả ID kanji từ client
//                var clientItemIds = clientItems.Keys.ToList();

//                // Lấy thời gian đồng bộ lần cuối
//                var lastSyncTime = request.LastSyncTime ?? DateTime.MinValue;

//                // Lấy tất cả kanji từ DB đã được cập nhật sau lần đồng bộ cuối cùng
//                // hoặc thuộc về các ID mà client đã gửi
//                var serverItems = await _dbContext.Kanjis
//                    .Where(k => k.UpdatedAt > lastSyncTime || clientItemIds.Contains(k.KanjiID))
//                    .ToListAsync();

//                // Tạo từ điển cho các mục từ server để dễ tìm kiếm
//                var serverItemsDict = serverItems.ToDictionary(item => item.KanjiID);

//                // Tìm danh sách ID kanji đã bị xóa trên server sau lần đồng bộ cuối
//                var deletedIds = await _dbContext.DeletedItems
//                    .Where(d => d.EntityType == "Kanji" && d.DeletedAt > lastSyncTime)
//                    .Select(d => d.EntityId)
//                    .ToListAsync();

//                // Xử lý từng mục từ client
//                foreach (var clientItem in request.Items)
//                {
//                    // Nếu mục đã bị xóa trên server, bỏ qua và thêm vào danh sách xóa
//                    if (deletedIds.Contains(clientItem.KanjiID))
//                    {
//                        result.DeletedItemIds.Add(clientItem.KanjiID);
//                        result.DeletedCount++;
//                        continue;
//                    }

//                    // Kiểm tra xem mục có tồn tại trên server không
//                    if (serverItemsDict.TryGetValue(clientItem.KanjiID, out var serverItem))
//                    {
//                        // Mục tồn tại trên cả client và server, kiểm tra xung đột
//                        if (clientItem.UpdatedAt > lastSyncTime && serverItem.UpdatedAt > lastSyncTime)
//                        {
//                            // Cả client và server đều đã cập nhật mục này, phát hiện xung đột
//                            result.Conflicts.Add(new SyncConflict<Kanji>
//                            {
//                                ItemId = clientItem.KanjiID,
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
//                            serverItem.Character = clientItem.Character;
//                            serverItem.OnYomi = clientItem.OnYomi;
//                            serverItem.KunYomi = clientItem.KunYomi;
//                            serverItem.StrokeCount = clientItem.StrokeCount;
//                            serverItem.JLPTLevel = clientItem.JLPTLevel;
//                            serverItem.Grade = clientItem.Grade;
//                            serverItem.RadicalName = clientItem.RadicalName;
//                            serverItem.StrokeOrder = clientItem.StrokeOrder;
//                            serverItem.Status = clientItem.Status;
//                            serverItem.LastModifiedBy = userId;
//                            serverItem.UpdatedAt = DateTime.UtcNow;

//                            _dbContext.Kanjis.Update(serverItem);
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
//                        if (clientItem.KanjiID < 0)
//                        {
//                            clientItem.KanjiID = 0; // ID sẽ được DB tự động tạo
//                        }

//                        clientItem.CreatedByUserID = userId;
//                        clientItem.LastModifiedBy = userId;
//                        clientItem.CreatedAt = DateTime.UtcNow;
//                        clientItem.UpdatedAt = DateTime.UtcNow;

//                        _dbContext.Kanjis.Add(clientItem);
//                        result.AddedCount++;
//                    }
//                }

//                // Tìm các mục trên server đã được cập nhật sau lần đồng bộ cuối
//                // nhưng không có trong danh sách từ client
//                var newServerItems = serverItems
//                    .Where(s => s.UpdatedAt > lastSyncTime && !clientItemIds.Contains(s.KanjiID))
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
//                    result.Message = "Đồng bộ kanji thành công.";
//                }

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

//                // Tạo từ điển cho các mục từ client để dễ tìm kiếm
//                var clientItems = request.Items.ToDictionary(item => item.Id);

//                // Lấy tất cả ID ngữ pháp từ client
//                var clientItemIds = clientItems.Keys.ToList();

//                // Lấy thời gian đồng bộ lần cuối
//                var lastSyncTime = request.LastSyncTime ?? DateTime.MinValue;

//                // Lấy tất cả ngữ pháp từ DB đã được cập nhật sau lần đồng bộ cuối cùng
//                // hoặc thuộc về các ID mà client đã gửi
//                var serverItems = await _dbContext.Grammars
//                    .Where(g => g.ModifiedAt > lastSyncTime || clientItemIds.Contains(g.Id))
//                    .ToListAsync();

//                // Tạo từ điển cho các mục từ server để dễ tìm kiếm
//                var serverItemsDict = serverItems.ToDictionary(item => item.Id);

//                // Tìm danh sách ID ngữ pháp đã bị xóa trên server sau lần đồng bộ cuối
//                var deletedIds = await _dbContext.DeletedItems
//                    .Where(d => d.EntityType == "Grammar" && d.DeletedAt > lastSyncTime)
//                    .Select(d => d.EntityId)
//                    .ToListAsync();

//                // Xử lý từng mục từ client
//                foreach (var clientItem in request.Items)
//                {
//                    // Nếu mục đã bị xóa trên server, bỏ qua và thêm vào danh sách xóa
//                    if (deletedIds.Contains(clientItem.Id))
//                    {
//                        result.DeletedItemIds.Add(clientItem.Id);
//                        result.DeletedCount++;
//                        continue;
//                    }

//                    // Kiểm tra xem mục có tồn tại trên server không
//                    if (serverItemsDict.TryGetValue(clientItem.Id, out var serverItem))
//                    {
//                        // Mục tồn tại trên cả client và server, kiểm tra xung đột
//                        if (clientItem.ModifiedAt > lastSyncTime && serverItem.ModifiedAt > lastSyncTime)
//                        {
//                            // Cả client và server đều đã cập nhật mục này, phát hiện xung đột
//                            result.Conflicts.Add(new SyncConflict<Grammar>
//                            {
//                                ItemId = clientItem.Id,
//                                ClientVersion = clientItem,
//                                ServerVersion = serverItem,
//                                ClientUpdateTime = clientItem.ModifiedAt,
//                                ServerUpdateTime = serverItem.ModifiedAt,
//                                ConflictType = ConflictType.BothModified
//                            });
//                        }
//                        else if (clientItem.ModifiedAt > serverItem.ModifiedAt)
//                        {
//                            // Client có phiên bản mới hơn, cập nhật server
//                            serverItem.Pattern = clientItem.Pattern;
//                            serverItem.Reading = clientItem.Reading;
//                            serverItem.Level = clientItem.Level;
//                            serverItem.CategoryId = clientItem.CategoryId;
//                            serverItem.DifficultyLevel = clientItem.DifficultyLevel;
//                            serverItem.Notes = clientItem.Notes;
//                            serverItem.Tags = clientItem.Tags;
//                            serverItem.Status = clientItem.Status;
//                            serverItem.ModifiedBy = userId;
//                            serverItem.ModifiedAt = DateTime.UtcNow;

//                            _dbContext.Grammars.Update(serverItem);
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
//                        if (clientItem.Id < 0)
//                        {
//                            clientItem.Id = 0; // ID sẽ được DB tự động tạo
//                        }

//                        clientItem.CreatedBy = userId;
//                        clientItem.ModifiedBy = userId;
//                        clientItem.CreatedAt = DateTime.UtcNow;
//                        clientItem.ModifiedAt = DateTime.UtcNow;

//                        _dbContext.Grammars.Add(clientItem);
//                        result.AddedCount++;
//                    }
//                }

//                // Tìm các mục trên server đã được cập nhật sau lần đồng bộ cuối
//                // nhưng không có trong danh sách từ client
//                var newServerItems = serverItems
//                    .Where(s => s.ModifiedAt > lastSyncTime && !clientItemIds.Contains(s.Id))
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
//                    result.Message = "Đồng bộ ngữ pháp thành công.";
//                }

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

//                // Tạo từ điển cho các mục từ client để dễ tìm kiếm
//                var clientItems = request.Items.ToDictionary(item => item.ProgressID);

//                // Lấy tất cả ID tiến trình học tập từ client
//                var clientItemIds = clientItems.Keys.ToList();

//                // Lấy thời gian đồng bộ lần cuối
//                var lastSyncTime = request.LastSyncTime ?? DateTime.MinValue;

//                // Lấy tất cả tiến trình học tập từ DB đã được cập nhật sau lần đồng bộ cuối cùng
//                // hoặc thuộc về các ID mà client đã gửi
//                var serverItems = await _dbContext.LearningProgresses
//                    .Where(lp => lp.UpdatedAt > lastSyncTime || clientItemIds.Contains(lp.ProgressID))
//                    .ToListAsync();

//                // Tạo từ điển cho các mục từ server để dễ tìm kiếm
//                var serverItemsDict = serverItems.ToDictionary(item => item.ProgressID);

//                // Tìm danh sách ID tiến trình học tập đã bị xóa trên server sau lần đồng bộ cuối
//                var deletedIds = await _dbContext.DeletedItems
//                    .Where(d => d.EntityType == "LearningProgress" && d.DeletedAt > lastSyncTime)
//                    .Select(d => d.EntityId)
//                    .ToListAsync();

//                // Xử lý từng mục từ client
//                foreach (var clientItem in request.Items)
//                {
//                    // Nếu mục đã bị xóa trên server, bỏ qua và thêm vào danh sách xóa
//                    if (deletedIds.Contains(clientItem.ProgressID))
//                    {
//                        result.DeletedItemIds.Add(clientItem.ProgressID);
//                        result.DeletedCount++;
//                        continue;
//                    }

//                    // Kiểm tra xem mục có tồn tại trên server không
//                    if (serverItemsDict.TryGetValue(clientItem.ProgressID, out var serverItem))
//                    {
//                        // Mục tồn tại trên cả client và server, kiểm tra xung đột
//                        if (clientItem.UpdatedAt > lastSyncTime && serverItem.UpdatedAt > lastSyncTime)
//                        {
//                            // Cả client và server đều đã cập nhật mục này, phát hiện xung đột
//                            result.Conflicts.Add(new SyncConflict<LearningProgress>
//                            {
//                                ItemId = clientItem.ProgressID,
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
//                            serverItem.StudyCount = clientItem.StudyCount;
//                            serverItem.CorrectCount = clientItem.CorrectCount;
//                            serverItem.IncorrectCount = clientItem.IncorrectCount;
//                            serverItem.LastStudied = clientItem.LastStudied;
//                            serverItem.MemoryStrength = clientItem.MemoryStrength;
//                            serverItem.NextReviewDate = clientItem.NextReviewDate;
//                            serverItem.UpdatedAt = DateTime.UtcNow;

//                            _dbContext.LearningProgresses.Update(serverItem);
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
//                        if (clientItem.ProgressID < 0)
//                        {
//                            clientItem.ProgressID = 0; // ID sẽ được DB tự động tạo
//                        }

//                        clientItem.UserID = userId; // Đảm bảo UserId được thiết lập đúng
//                        clientItem.CreatedAt = DateTime.UtcNow;
//                        clientItem.UpdatedAt = DateTime.UtcNow;

//                        _dbContext.LearningProgresses.Add(clientItem);
//                        result.AddedCount++;
//                    }
//                }

//                // Tìm các mục trên server đã được cập nhật sau lần đồng bộ cuối
//                // nhưng không có trong danh sách từ client
//                var newServerItems = serverItems
//                    .Where(s => s.UpdatedAt > lastSyncTime && !clientItemIds.Contains(s.ProgressID))
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
//                    result.Message = "Đồng bộ tiến trình học tập thành công.";
//                }

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

//                // Tạo từ điển cho các mục từ client để dễ tìm kiếm
//                var clientItems = request.Items.ToDictionary(item => item.ListID);

//                // Lấy tất cả ID danh sách từ vựng cá nhân từ client
//                var clientItemIds = clientItems.Keys.ToList();

//                // Lấy thời gian đồng bộ lần cuối
//                var lastSyncTime = request.LastSyncTime ?? DateTime.MinValue;

//                // Lấy tất cả danh sách từ vựng cá nhân từ DB đã được cập nhật sau lần đồng bộ cuối cùng
//                // hoặc thuộc về các ID mà client đã gửi
//                var serverItems = await _dbContext.PersonalWordLists
//                    .Where(pwl => pwl.UpdatedAt > lastSyncTime || clientItemIds.Contains(pwl.ListID))
//                    .ToListAsync();

//                // Tạo từ điển cho các mục từ server để dễ tìm kiếm
//                var serverItemsDict = serverItems.ToDictionary(item => item.ListID);

//                // Tìm danh sách ID danh sách từ vựng cá nhân đã bị xóa trên server sau lần đồng bộ cuối
//                var deletedIds = await _dbContext.DeletedItems
//                    .Where(d => d.EntityType == "PersonalWordList" && d.DeletedAt > lastSyncTime)
//                    .Select(d => d.EntityId)
//                    .ToListAsync();

//                // Xử lý từng mục từ client
//                foreach (var clientItem in request.Items)
//                {
//                    // Nếu mục đã bị xóa trên server, bỏ qua và thêm vào danh sách xóa
//                    if (deletedIds.Contains(clientItem.ListID))
//                    {
//                        result.DeletedItemIds.Add(clientItem.ListID);
//                        result.DeletedCount++;
//                        continue;
//                    }

//                    // Kiểm tra xem mục có tồn tại trên server không
//                    if (serverItemsDict.TryGetValue(clientItem.ListID, out var serverItem))
//                    {
//                        // Mục tồn tại trên cả client và server, kiểm tra xung đột
//                        if (clientItem.UpdatedAt > lastSyncTime && serverItem.UpdatedAt > lastSyncTime)
//                        {
//                            // Cả client và server đều đã cập nhật mục này, phát hiện xung đột
//                            result.Conflicts.Add(new SyncConflict<PersonalWordList>
//                            {
//                                ItemId = clientItem.ListID,
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
//                            serverItem.ListName = clientItem.ListName;
//                            serverItem.Description = clientItem.Description;
//                            serverItem.UpdatedAt = DateTime.UtcNow;

//                            _dbContext.PersonalWordLists.Update(serverItem);
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
//                        if (clientItem.ListID < 0)
//                        {
//                            clientItem.ListID = 0; // ID sẽ được DB tự động tạo
//                        }

//                        clientItem.UserID = userId; // Đảm bảo UserId được thiết lập đúng
//                        clientItem.CreatedAt = DateTime.UtcNow;
//                        clientItem.UpdatedAt = DateTime.UtcNow;

//                        _dbContext.PersonalWordLists.Add(clientItem);
//                        result.AddedCount++;
//                    }
//                }

//                // Tìm các mục trên server đã được cập nhật sau lần đồng bộ cuối
//                // nhưng không có trong danh sách từ client
//                var newServerItems = serverItems
//                    .Where(s => s.UpdatedAt > lastSyncTime && !clientItemIds.Contains(s.ListID))
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
//                    result.Message = "Đồng bộ danh sách từ vựng cá nhân thành công.";
//                }

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
//            // Phân tích dữ liệu xung đột để xác định loại entity (Vocabulary, Kanji, etc.)
//            // Lấy dữ liệu phiên bản client từ xung đột
//            // Cập nhật entity tương ứng trong database

//            if (conflict.EntityType == "Vocabulary")
//            {
//                // Lấy entity từ DB
//                var entity = await _dbContext.Vocabularies.FindAsync(conflict.EntityId);
//                if (entity != null)
//                {
//                    // Cập nhật với dữ liệu client (cần phân tích ClientData)
//                    var clientData = System.Text.Json.JsonSerializer.Deserialize<Vocabulary>(conflict.ClientData);
//                    if (clientData != null)
//                    {
//                        entity.Term = clientData.Term;
//                        entity.Reading = clientData.Reading;
//                        entity.DifficultyLevel = clientData.DifficultyLevel;
//                        entity.Notes = clientData.Notes;
//                        entity.Tags = clientData.Tags;
//                        entity.AudioFile = clientData.AudioFile;
//                        entity.Status = clientData.Status;
//                        entity.ModifiedBy = conflict.UserID;
//                        entity.ModifiedAt = DateTime.UtcNow;

//                        _dbContext.Vocabularies.Update(entity);
//                    }
//                }
//            }
//            else if (conflict.EntityType == "Kanji")
//            {
//                var entity = await _dbContext.Kanjis.FindAsync(conflict.EntityId);
//                if (entity != null)
//                {
//                    var clientData = System.Text.Json.JsonSerializer.Deserialize<Kanji>(conflict.ClientData);
//                    if (clientData != null)
//                    {
//                        entity.Character = clientData.Character;
//                        entity.OnYomi = clientData.OnYomi;
//                        entity.KunYomi = clientData.KunYomi;
//                        entity.StrokeCount = clientData.StrokeCount;
//                        entity.JLPTLevel = clientData.JLPTLevel;
//                        entity.Grade = clientData.Grade;
//                        entity.RadicalName = clientData.RadicalName;
//                        entity.StrokeOrder = clientData.StrokeOrder;
//                        entity.Status = clientData.Status;
//                        entity.LastModifiedBy = conflict.UserID;
//                        entity.UpdatedAt = DateTime.UtcNow;

//                        _dbContext.Kanjis.Update(entity);
//                    }
//                }
//            }
//            else if (conflict.EntityType == "Grammar")
//            {
//                var entity = await _dbContext.Grammars.FindAsync(conflict.EntityId);
//                if (entity != null)
//                {
//                    var clientData = System.Text.Json.JsonSerializer.Deserialize<Grammar>(conflict.ClientData);
//                    if (clientData != null)
//                    {
//                        entity.Pattern = clientData.Pattern;
//                        entity.Reading = clientData.Reading;
//                        entity.Level = clientData.Level;
//                        entity.CategoryId = clientData.CategoryId;
//                        entity.DifficultyLevel = clientData.DifficultyLevel;
//                        entity.Notes = clientData.Notes;
//                        entity.Tags = clientData.Tags;
//                        entity.Status = clientData.Status;
//                        entity.ModifiedBy = conflict.UserID;
//                        entity.ModifiedAt = DateTime.UtcNow;

//                        _dbContext.Grammars.Update(entity);
//                    }
//                }
//            }
//            else if (conflict.EntityType == "LearningProgress")
//            {
//                var entity = await _dbContext.LearningProgresses.FindAsync(conflict.EntityId);
//                if (entity != null)
//                {
//                    var clientData = System.Text.Json.JsonSerializer.Deserialize<LearningProgress>(conflict.ClientData);
//                    if (clientData != null)
//                    {
//                        entity.StudyCount = clientData.StudyCount;
//                        entity.CorrectCount = clientData.CorrectCount;
//                        entity.IncorrectCount = clientData.IncorrectCount;
//                        entity.LastStudied = clientData.LastStudied;
//                        entity.MemoryStrength = clientData.MemoryStrength;
//                        entity.NextReviewDate = clientData.NextReviewDate;
//                        entity.UpdatedAt = DateTime.UtcNow;

//                        _dbContext.LearningProgresses.Update(entity);
//                    }
//                }
//            }
//            else if (conflict.EntityType == "PersonalWordList")
//            {
//                var entity = await _dbContext.PersonalWordLists.FindAsync(conflict.EntityId);
//                if (entity != null)
//                {
//                    var clientData = System.Text.Json.JsonSerializer.Deserialize<PersonalWordList>(conflict.ClientData);
//                    if (clientData != null)
//                    {
//                        entity.ListName = clientData.ListName;
//                        entity.Description = clientData.Description;
//                        entity.UpdatedAt = DateTime.UtcNow;

//                        _dbContext.PersonalWordLists.Update(entity);
//                    }
//                }
//            }
//        }

//        /// <summary>
//        /// Giải quyết xung đột bằng cách sử dụng phiên bản từ server
//        /// </summary>
//        private Task ResolveConflictWithServerVersion(SyncConflict conflict)
//        {
//            // Không cần làm gì vì dữ liệu server đã là phiên bản hiện tại
//            // Chỉ cần đánh dấu xung đột đã được giải quyết
//            return Task.CompletedTask;
//        }

//        /// <summary>
//        /// Giải quyết xung đột bằng cách sử dụng phiên bản tùy chỉnh
//        /// </summary>
//        private async Task ResolveConflictWithCustomVersion(SyncConflict conflict, string customData)
//        {
//            if (string.IsNullOrEmpty(customData))
//                return;

//            if (conflict.EntityType == "Vocabulary")
//            {
//                var entity = await _dbContext.Vocabularies.FindAsync(conflict.EntityId);
//                if (entity != null)
//                {
//                    var customEntity = System.Text.Json.JsonSerializer.Deserialize<Vocabulary>(customData);
//                    if (customEntity != null)
//                    {
//                        entity.Term = customEntity.Term;
//                        entity.Reading = customEntity.Reading;
//                        entity.DifficultyLevel = customEntity.DifficultyLevel;
//                        entity.Notes = customEntity.Notes;
//                        entity.Tags = customEntity.Tags;
//                        entity.AudioFile = customEntity.AudioFile;
//                        entity.Status = customEntity.Status;
//                        entity.ModifiedBy = conflict.UserID;
//                        entity.ModifiedAt = DateTime.UtcNow;

//                        _dbContext.Vocabularies.Update(entity);
//                    }
//                }
//            }
//            else if (conflict.EntityType == "Kanji")
//            {
//                var entity = await _dbContext.Kanjis.FindAsync(conflict.EntityId);
//                if (entity != null)
//                {
//                    var customEntity = System.Text.Json.JsonSerializer.Deserialize<Kanji>(customData);
//                    if (customEntity != null)
//                    {
//                        entity.Character = customEntity.Character;
//                        entity.OnYomi = customEntity.OnYomi;
//                        entity.KunYomi = customEntity.KunYomi;
//                        entity.StrokeCount = customEntity.StrokeCount;
//                        entity.JLPTLevel = customEntity.JLPTLevel;
//                        entity.Grade = customEntity.Grade;
//                        entity.RadicalName = customEntity.RadicalName;
//                        entity.StrokeOrder = customEntity.StrokeOrder;
//                        entity.Status = customEntity.Status;
//                        entity.LastModifiedBy = conflict.UserID;
//                        entity.UpdatedAt = DateTime.UtcNow;

//                        _dbContext.Kanjis.Update(entity);
//                    }
//                }
//            }
//            else if (conflict.EntityType == "Grammar")
//            {
//                var entity = await _dbContext.Grammars.FindAsync(conflict.EntityId);
//                if (entity != null)
//                {
//                    var customEntity = System.Text.Json.JsonSerializer.Deserialize<Grammar>(customData);
//                    if (customEntity != null)
//                    {
//                        entity.Pattern = customEntity.Pattern;
//                        entity.Reading = customEntity.Reading;
//                        entity.Level = customEntity.Level;
//                        entity.CategoryId = customEntity.CategoryId;
//                        entity.DifficultyLevel = customEntity.DifficultyLevel;
//                        entity.Notes = customEntity.Notes;
//                        entity.Tags = customEntity.Tags;
//                        entity.Status = customEntity.Status;
//                        entity.ModifiedBy = conflict.UserID;
//                        entity.ModifiedAt = DateTime.UtcNow;

//                        _dbContext.Grammars.Update(entity);
//                    }
//                }
//            }
//            else if (conflict.EntityType == "LearningProgress")
//            {
//                var entity = await _dbContext.LearningProgresses.FindAsync(conflict.EntityId);
//                if (entity != null)
//                {
//                    var customEntity = System.Text.Json.JsonSerializer.Deserialize<LearningProgress>(customData);
//                    if (customEntity != null)
//                    {
//                        entity.StudyCount = customEntity.StudyCount;
//                        entity.CorrectCount = customEntity.CorrectCount;
//                        entity.IncorrectCount = customEntity.IncorrectCount;
//                        entity.LastStudied = customEntity.LastStudied;
//                        entity.MemoryStrength = customEntity.MemoryStrength;
//                        entity.NextReviewDate = customEntity.NextReviewDate;
//                        entity.UpdatedAt = DateTime.UtcNow;

//                        _dbContext.LearningProgresses.Update(entity);
//                    }
//                }
//            }
//            else if (conflict.EntityType == "PersonalWordList")
//            {
//                var entity = await _dbContext.PersonalWordLists.FindAsync(conflict.EntityId);
//                if (entity != null)
//                {
//                    var customEntity = System.Text.Json.JsonSerializer.Deserialize<PersonalWordList>(customData);
//                    if (customEntity != null)
//                    {
//                        entity.ListName = customEntity.ListName;
//                        entity.Description = customEntity.Description;
//                        entity.UpdatedAt = DateTime.UtcNow;

//                        _dbContext.PersonalWordLists.Update(entity);
//                    }
//                }
//            }
//        }

//        /// <summary>
//        /// Giải quyết xung đột bằng cách xóa mục
//        /// </summary>
//        private async Task ResolveConflictWithDeletion(SyncConflict conflict)
//        {
//            if (conflict.EntityType == "Vocabulary")
//            {
//                var entity = await _dbContext.Vocabularies.FindAsync(conflict.EntityId);
//                if (entity != null)
//                {
//                    _dbContext.Vocabularies.Remove(entity);

//                    _dbContext.DeletedItems.Add(new DeletedItem
//                    {
//                        EntityId = conflict.EntityId,
//                        EntityType = "Vocabulary",
//                        DeletedAt = DateTime.UtcNow,
//                        UserID = conflict.UserID
//                    });
//                }
//            }
//            else if (conflict.EntityType == "Kanji")
//            {
//                var entity = await _dbContext.Kanjis.FindAsync(conflict.EntityId);
//                if (entity != null)
//                {
//                    _dbContext.Kanjis.Remove(entity);

//                    _dbContext.DeletedItems.Add(new DeletedItem
//                    {
//                        EntityId = conflict.EntityId,
//                        EntityType = "Kanji",
//                        DeletedAt = DateTime.UtcNow,
//                        UserID = conflict.UserID
//                    });
//                }
//            }
//            else if (conflict.EntityType == "Grammar")
//            {
//                var entity = await _dbContext.Grammars.FindAsync(conflict.EntityId);
//                if (entity != null)
//                {
//                    _dbContext.Grammars.Remove(entity);

//                    _dbContext.DeletedItems.Add(new DeletedItem
//                    {
//                        EntityId = conflict.EntityId,
//                        EntityType = "Grammar",
//                        DeletedAt = DateTime.UtcNow,
//                        UserID = conflict.UserID
//                    });
//                }
//            }
//            else if (conflict.EntityType == "LearningProgress")
//            {
//                var entity = await _dbContext.LearningProgresses.FindAsync(conflict.EntityId);
//                if (entity != null)
//                {
//                    _dbContext.LearningProgresses.Remove(entity);

//                    _dbContext.DeletedItems.Add(new DeletedItem
//                    {
//                        EntityId = conflict.EntityId,
//                        EntityType = "LearningProgress",
//                        DeletedAt = DateTime.UtcNow,
//                        UserID = conflict.UserID
//                    });
//                }
//            }
//            else if (conflict.EntityType == "PersonalWordList")
//            {
//                var entity = await _dbContext.PersonalWordLists.FindAsync(conflict.EntityId);
//                if (entity != null)
//                {
//                    _dbContext.PersonalWordLists.Remove(entity);

//                    _dbContext.DeletedItems.Add(new DeletedItem
//                    {
//                        EntityId = conflict.EntityId,
//                        EntityType = "PersonalWordList",
//                        DeletedAt = DateTime.UtcNow,
//                        UserID = conflict.UserID
//                    });
//                }
//            }
//        }

//        #endregion
//    }
//}