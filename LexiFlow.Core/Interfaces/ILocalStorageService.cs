using LexiFlow.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LexiFlow.Core.Interfaces
{
    /// <summary>
    /// Interface cho dịch vụ lưu trữ cục bộ
    /// </summary>
    public interface ILocalStorageService
    {
        /// <summary>
        /// Khởi tạo cơ sở dữ liệu cục bộ
        /// </summary>
        Task InitializeDatabaseAsync();

        /// <summary>
        /// Lấy danh sách từ vựng
        /// </summary>
        Task<List<Vocabulary>> GetVocabularyAsync(int skip = 0, int take = 20, string searchQuery = null);

        /// <summary>
        /// Lấy tổng số từ vựng
        /// </summary>
        Task<int> GetVocabularyCountAsync(string searchQuery = null);

        /// <summary>
        /// Lấy từ vựng theo ID
        /// </summary>
        Task<Vocabulary> GetVocabularyByIdAsync(int id);

        /// <summary>
        /// Lưu từ vựng
        /// </summary>
        Task<bool> SaveVocabularyAsync(Vocabulary vocabulary);

        /// <summary>
        /// Xóa từ vựng
        /// </summary>
        Task<bool> DeleteVocabularyAsync(int id);

        /// <summary>
        /// Đánh dấu từ vựng đã xóa
        /// </summary>
        Task<bool> MarkVocabularyAsDeletedAsync(int id);

        /// <summary>
        /// Lấy danh sách từ vựng cần đồng bộ
        /// </summary>
        Task<List<Vocabulary>> GetPendingSyncVocabularyAsync();

        /// <summary>
        /// Lấy danh sách ID từ vựng đã xóa cần đồng bộ
        /// </summary>
        Task<List<int>> GetDeletedVocabularyIdsAsync();

        /// <summary>
        /// Đánh dấu từ vựng đã đồng bộ
        /// </summary>
        Task<bool> MarkVocabularyAsSyncedAsync(int id);

        /// <summary>
        /// Cập nhật thời gian đồng bộ lần cuối
        /// </summary>
        Task UpdateLastSyncTimeAsync(DateTime syncTime);

        /// <summary>
        /// Lấy thời gian đồng bộ lần cuối
        /// </summary>
        Task<DateTime?> GetLastSyncTimeAsync();

        /// <summary>
        /// Tìm kiếm từ vựng theo từ khóa
        /// </summary>
        Task<List<Vocabulary>> SearchVocabularyAsync(string keyword, string searchBy = "all");
    }
}