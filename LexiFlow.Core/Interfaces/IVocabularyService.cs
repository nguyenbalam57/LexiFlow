using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using LexiFlow.Core.Entities;
using LexiFlow.Core.Models;
using LexiFlow.Core.Models.Requests;

namespace LexiFlow.Core.Interfaces
{
    /// <summary>
    /// Interface cho dịch vụ quản lý từ vựng
    /// </summary>
    public interface IVocabularyService
    {
        /// <summary>
        /// Lấy danh sách từ vựng
        /// </summary>
        Task<ServiceResult<PagedResult<Vocabulary>>> GetVocabularyAsync(int page = 1, int pageSize = 20, string searchQuery = null);

        /// <summary>
        /// Lấy từ vựng theo ID
        /// </summary>
        Task<ServiceResult<Vocabulary>> GetByIdAsync(int id);

        /// <summary>
        /// Tạo từ vựng mới
        /// </summary>
        Task<ServiceResult<Vocabulary>> CreateAsync(CreateVocabularyRequest request);

        /// <summary>
        /// Cập nhật từ vựng
        /// </summary>
        Task<ServiceResult<Vocabulary>> UpdateAsync(int id, UpdateVocabularyRequest request);

        /// <summary>
        /// Xóa từ vựng
        /// </summary>
        Task<ServiceResult<bool>> DeleteAsync(int id);

        /// <summary>
        /// Lưu từ vựng vào cơ sở dữ liệu cục bộ
        /// </summary>
        Task<bool> SaveLocalAsync(Vocabulary vocabulary);

        /// <summary>
        /// Tìm kiếm từ vựng theo từ khóa
        /// </summary>
        Task<ServiceResult<List<Vocabulary>>> SearchAsync(string keyword, string searchBy = "all");

        /// <summary>
        /// Đồng bộ dữ liệu với server
        /// </summary>
        Task<ServiceResult<SyncResult>> SyncAsync();
    }
}
