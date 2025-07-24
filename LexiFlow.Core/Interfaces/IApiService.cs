using LexiFlow.Models;
using LexiFlow.Core.Models;
using LexiFlow.Core.Models.Requests;
using LexiFlow.Core.Models.Responses;
using System;
using System.Threading.Tasks;

namespace LexiFlow.Core.Interfaces
{
    /// <summary>
    /// Interface cho dịch vụ API
    /// </summary>
    public interface IApiService
    {
        /// <summary>
        /// Thực hiện đăng nhập và lấy token
        /// </summary>
        Task<ServiceResult<LoginResponse>> LoginAsync(string username, string password);

        /// <summary>
        /// Làm mới token
        /// </summary>
        Task<ServiceResult<string>> RefreshTokenAsync();

        /// <summary>
        /// Lấy token hiện tại
        /// </summary>
        string GetCurrentToken();

        /// <summary>
        /// Xóa access token khi đăng xuất
        /// </summary>
        void ClearAccessToken();

        /// <summary>
        /// Lấy danh sách từ vựng từ API
        /// </summary>
        Task<ServiceResult<PagedResult<Vocabulary>>> GetVocabularyAsync(int page = 1, int pageSize = 20, DateTime? lastSync = null);

        /// <summary>
        /// Lấy thông tin chi tiết từ vựng theo ID
        /// </summary>
        Task<ServiceResult<Vocabulary>> GetVocabularyByIdAsync(int id);

        /// <summary>
        /// Tạo từ vựng mới
        /// </summary>
        Task<ServiceResult<Vocabulary>> CreateVocabularyAsync(CreateVocabularyRequest request);

        /// <summary>
        /// Cập nhật từ vựng
        /// </summary>
        Task<ServiceResult<Vocabulary>> UpdateVocabularyAsync(int id, UpdateVocabularyRequest request);

        /// <summary>
        /// Xóa từ vựng
        /// </summary>
        Task<ServiceResult<bool>> DeleteVocabularyAsync(int id);

        /// <summary>
        /// Đồng bộ dữ liệu với server
        /// </summary>
        /// <param name="request">Yêu cầu đồng bộ</param>
        /// <returns>Kết quả đồng bộ</returns>
        Task<ServiceResult<SyncResult>> SyncDataAsync(SyncRequest request);
    }
}