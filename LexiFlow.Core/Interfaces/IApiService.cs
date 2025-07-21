using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using LexiFlow.Core.Entities;
using LexiFlow.Core.Models;
using LexiFlow.Core.Models.Requests;
using LexiFlow.Core.Models.Responses;

namespace LexiFlow.Core.Interfaces
{
    /// <summary>
    /// Interface cho dịch vụ API
    /// </summary>
    public interface IApiService
    {
        /// <summary>
        /// Thiết lập access token cho các yêu cầu API
        /// </summary>
        void SetAccessToken(string token);

        /// <summary>
        /// Xóa access token
        /// </summary>
        void ClearAccessToken();

        /// <summary>
        /// Đăng nhập và lấy token
        /// </summary>
        Task<ApiResponse<LoginResponse>> LoginAsync(string username, string password);

        /// <summary>
        /// Lấy danh sách từ vựng
        /// </summary>
        Task<ApiResponse<PagedResult<Vocabulary>>> GetVocabularyAsync(int page = 1, int pageSize = 20);

        /// <summary>
        /// Lấy thông tin từ vựng theo ID
        /// </summary>
        Task<ApiResponse<Vocabulary>> GetVocabularyByIdAsync(int id);

        /// <summary>
        /// Tạo từ vựng mới
        /// </summary>
        Task<ApiResponse<Vocabulary>> CreateVocabularyAsync(CreateVocabularyRequest request);

        /// <summary>
        /// Cập nhật từ vựng
        /// </summary>
        Task<ApiResponse<Vocabulary>> UpdateVocabularyAsync(int id, UpdateVocabularyRequest request);

        /// <summary>
        /// Xóa từ vựng
        /// </summary>
        Task<ApiResponse<bool>> DeleteVocabularyAsync(int id);

        /// <summary>
        /// Đồng bộ dữ liệu
        /// </summary>
        Task<ApiResponse<SyncResult>> SyncDataAsync(SyncRequest request);

        /// <summary>
        /// Làm mới token
        /// </summary>
        Task<ApiResponse<string>> RefreshTokenAsync();
    }
}
