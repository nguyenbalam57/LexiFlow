//using LexiFlow.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LexiFlow.Core.Interfaces
//{
//    /// <summary>
//    /// Interface cho dịch vụ đồng bộ dữ liệu giữa client và server
//    /// </summary>
//    public interface ISyncService
//    {
//        /// <summary>
//        /// Đồng bộ dữ liệu từ vựng
//        /// </summary>
//        /// <param name="request">Yêu cầu đồng bộ từ vựng</param>
//        /// <param name="userId">ID người dùng</param>
//        /// <returns>Kết quả đồng bộ từ vựng</returns>
//        Task<SyncResult<Vocabulary>> SyncVocabularyAsync(VocabularySyncRequest request, int userId);

//        /// <summary>
//        /// Đồng bộ dữ liệu kanji
//        /// </summary>
//        /// <param name="request">Yêu cầu đồng bộ kanji</param>
//        /// <param name="userId">ID người dùng</param>
//        /// <returns>Kết quả đồng bộ kanji</returns>
//        Task<SyncResult<Kanji>> SyncKanjiAsync(KanjiSyncRequest request, int userId);

//        /// <summary>
//        /// Đồng bộ dữ liệu ngữ pháp
//        /// </summary>
//        /// <param name="request">Yêu cầu đồng bộ ngữ pháp</param>
//        /// <param name="userId">ID người dùng</param>
//        /// <returns>Kết quả đồng bộ ngữ pháp</returns>
//        Task<SyncResult<Grammar>> SyncGrammarAsync(GrammarSyncRequest request, int userId);

//        /// <summary>
//        /// Đồng bộ tiến trình học tập của người dùng
//        /// </summary>
//        /// <param name="request">Yêu cầu đồng bộ tiến trình</param>
//        /// <param name="userId">ID người dùng</param>
//        /// <returns>Kết quả đồng bộ tiến trình</returns>
//        Task<SyncResult<LearningProgress>> SyncLearningProgressAsync(LearningProgressSyncRequest request, int userId);

//        /// <summary>
//        /// Đồng bộ danh sách từ vựng cá nhân
//        /// </summary>
//        /// <param name="request">Yêu cầu đồng bộ danh sách từ vựng cá nhân</param>
//        /// <param name="userId">ID người dùng</param>
//        /// <returns>Kết quả đồng bộ danh sách từ vựng cá nhân</returns>
//        Task<SyncResult<PersonalWordList>> SyncPersonalWordListsAsync(PersonalWordListSyncRequest request, int userId);

//        /// <summary>
//        /// Đồng bộ tất cả dữ liệu cho người dùng
//        /// </summary>
//        /// <param name="userId">ID người dùng</param>
//        /// <param name="lastSyncTime">Thời gian đồng bộ lần cuối (null cho lần đồng bộ đầu tiên)</param>
//        /// <returns>Kết quả đồng bộ tất cả dữ liệu</returns>
//        Task<FullSyncResult> SyncAllAsync(int userId, DateTime? lastSyncTime = null);

//        /// <summary>
//        /// Lấy thông tin đồng bộ cho người dùng
//        /// </summary>
//        /// <param name="userId">ID người dùng</param>
//        /// <returns>Thông tin đồng bộ</returns>
//        Task<SyncInfo> GetSyncInfoAsync(int userId);

//        /// <summary>
//        /// Đặt lại trạng thái đồng bộ cho người dùng
//        /// </summary>
//        /// <param name="userId">ID người dùng</param>
//        /// <returns>Kết quả đặt lại trạng thái đồng bộ</returns>
//        Task<ApiResponse> ResetSyncStatusAsync(int userId);

//        /// <summary>
//        /// Giải quyết xung đột đồng bộ
//        /// </summary>
//        /// <param name="request">Yêu cầu giải quyết xung đột</param>
//        /// <param name="userId">ID người dùng</param>
//        /// <returns>Kết quả giải quyết xung đột</returns>
//        Task<ApiResponse> ResolveSyncConflictsAsync(SyncConflictResolutionRequest request, int userId);
//    }

//    /// <summary>
//    /// Generic class chứa kết quả đồng bộ cho một loại dữ liệu
//    /// </summary>
//    /// <typeparam name="T">Kiểu dữ liệu đồng bộ</typeparam>
//    public class SyncResult<T> where T : class
//    {
//        /// <summary>
//        /// Các mục cần thêm/cập nhật trên client
//        /// </summary>
//        public List<T> UpdatedItems { get; set; } = new List<T>();

//        /// <summary>
//        /// ID của các mục cần xóa trên client
//        /// </summary>
//        public List<int> DeletedItemIds { get; set; } = new List<int>();

//        /// <summary>
//        /// Số lượng mục đã thêm
//        /// </summary>
//        public int AddedCount { get; set; }

//        /// <summary>
//        /// Số lượng mục đã cập nhật
//        /// </summary>
//        public int UpdatedCount { get; set; }

//        /// <summary>
//        /// Số lượng mục đã xóa
//        /// </summary>
//        public int DeletedCount { get; set; }

//        /// <summary>
//        /// Các xung đột phát hiện được trong quá trình đồng bộ
//        /// </summary>
//        public List<SyncConflict<T>> Conflicts { get; set; } = new List<SyncConflict<T>>();

//        /// <summary>
//        /// Thời gian đồng bộ hiện tại
//        /// </summary>
//        public DateTime SyncTime { get; set; } = DateTime.UtcNow;

//        /// <summary>
//        /// Trạng thái đồng bộ
//        /// </summary>
//        public SyncStatus Status { get; set; } = SyncStatus.Success;

//        /// <summary>
//        /// Thông báo (nếu có)
//        /// </summary>
//        public string Message { get; set; }
//    }

//    /// <summary>
//    /// Kết quả đồng bộ đầy đủ cho tất cả dữ liệu
//    /// </summary>
//    public class FullSyncResult
//    {
//        /// <summary>
//        /// Kết quả đồng bộ từ vựng
//        /// </summary>
//        public SyncResult<Vocabulary> VocabularyResult { get; set; }

//        /// <summary>
//        /// Kết quả đồng bộ kanji
//        /// </summary>
//        public SyncResult<Kanji> KanjiResult { get; set; }

//        /// <summary>
//        /// Kết quả đồng bộ ngữ pháp
//        /// </summary>
//        public SyncResult<Grammar> GrammarResult { get; set; }

//        /// <summary>
//        /// Kết quả đồng bộ tiến trình học tập
//        /// </summary>
//        public SyncResult<LearningProgress> LearningProgressResult { get; set; }

//        /// <summary>
//        /// Kết quả đồng bộ danh sách từ vựng cá nhân
//        /// </summary>
//        public SyncResult<PersonalWordList> PersonalWordListResult { get; set; }

//        /// <summary>
//        /// Thời gian đồng bộ hiện tại
//        /// </summary>
//        public DateTime SyncTime { get; set; } = DateTime.UtcNow;

//        /// <summary>
//        /// Tổng số mục đã đồng bộ
//        /// </summary>
//        public int TotalItemsProcessed { get; set; }

//        /// <summary>
//        /// Trạng thái đồng bộ tổng thể
//        /// </summary>
//        public SyncStatus Status { get; set; } = SyncStatus.Success;

//        /// <summary>
//        /// Thông báo tổng thể
//        /// </summary>
//        public string Message { get; set; }
//    }

//    /// <summary>
//    /// Thông tin về xung đột đồng bộ
//    /// </summary>
//    /// <typeparam name="T">Kiểu dữ liệu xung đột</typeparam>
//    public class SyncConflict<T> where T : class
//    {
//        /// <summary>
//        /// ID của mục xung đột
//        /// </summary>
//        public int ItemId { get; set; }

//        /// <summary>
//        /// Phiên bản của client
//        /// </summary>
//        public T ClientVersion { get; set; }

//        /// <summary>
//        /// Phiên bản của server
//        /// </summary>
//        public T ServerVersion { get; set; }

//        /// <summary>
//        /// Thời gian cập nhật của client
//        /// </summary>
//        public DateTime ClientUpdateTime { get; set; }

//        /// <summary>
//        /// Thời gian cập nhật của server
//        /// </summary>
//        public DateTime ServerUpdateTime { get; set; }

//        /// <summary>
//        /// Loại xung đột
//        /// </summary>
//        public ConflictType ConflictType { get; set; }
//    }

//    /// <summary>
//    /// Loại xung đột
//    /// </summary>
//    public enum ConflictType
//    {
//        /// <summary>
//        /// Cả client và server đều đã sửa đổi
//        /// </summary>
//        BothModified,

//        /// <summary>
//        /// Client đã sửa đổi, server đã xóa
//        /// </summary>
//        ClientModifiedServerDeleted,

//        /// <summary>
//        /// Server đã sửa đổi, client đã xóa
//        /// </summary>
//        ServerModifiedClientDeleted,

//        /// <summary>
//        /// Xung đột khác
//        /// </summary>
//        Other
//    }

//    /// <summary>
//    /// Trạng thái đồng bộ
//    /// </summary>
//    public enum SyncStatus
//    {
//        /// <summary>
//        /// Thành công
//        /// </summary>
//        Success,

//        /// <summary>
//        /// Thành công một phần (có xung đột)
//        /// </summary>
//        PartialSuccess,

//        /// <summary>
//        /// Thất bại
//        /// </summary>
//        Failed,

//        /// <summary>
//        /// Đang xử lý
//        /// </summary>
//        Processing
//    }

//    /// <summary>
//    /// Thông tin trạng thái đồng bộ
//    /// </summary>
//    public class SyncInfo
//    {
//        /// <summary>
//        /// ID người dùng
//        /// </summary>
//        public int UserId { get; set; }

//        /// <summary>
//        /// Thời gian đồng bộ lần cuối
//        /// </summary>
//        public DateTime? LastSyncTime { get; set; }

//        /// <summary>
//        /// Trạng thái đồng bộ hiện tại
//        /// </summary>
//        public SyncStatus Status { get; set; }

//        /// <summary>
//        /// Số lượng mục đã đồng bộ
//        /// </summary>
//        public int TotalItemsSynced { get; set; }

//        /// <summary>
//        /// Số lượng xung đột chưa được giải quyết
//        /// </summary>
//        public int UnresolvedConflicts { get; set; }

//        /// <summary>
//        /// Có cần đồng bộ đầy đủ không
//        /// </summary>
//        public bool NeedsFullSync { get; set; }

//        /// <summary>
//        /// Thông báo
//        /// </summary>
//        public string Message { get; set; }
//    }
//}
