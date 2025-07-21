using LexiFlow.Core.Interfaces;
using LexiFlow.Core.Models;
using LexiFlow.Core.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LexiFlow.Core.Entities;
using Microsoft.Extensions.Logging;

namespace LexiFlow.Core.Services
{
    /// <summary>
    /// Triển khai dịch vụ quản lý từ vựng
    /// </summary>
    public class VocabularyService : IVocabularyService
    {
        private readonly IApiService _apiService;
        private readonly ILocalStorageService _localStorage;
        private readonly IAuthService _authService;
        private readonly ILogger<VocabularyService> _logger;

        public VocabularyService(
            IApiService apiService,
            ILocalStorageService localStorage,
            IAuthService authService,
            ILogger<VocabularyService> logger)
        {
            _apiService = apiService;
            _localStorage = localStorage;
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách từ vựng từ API và lưu trữ cục bộ
        /// </summary>
        public async Task<ServiceResult<PagedResult<Vocabulary>>> GetVocabularyAsync(
            int page = 1, int pageSize = 20, string searchQuery = null)
        {
            try
            {
                // Kiểm tra xác thực
                if (!_authService.IsAuthenticated())
                {
                    return ServiceResult<PagedResult<Vocabulary>>.Fail("Vui lòng đăng nhập để xem danh sách từ vựng.");
                }

                // Nếu có từ khóa tìm kiếm, sử dụng tìm kiếm cục bộ
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    var searchResults = await _localStorage.SearchVocabularyAsync(searchQuery);
                    var totalCount = searchResults.Count;
                    var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                    // Phân trang kết quả tìm kiếm
                    var paginatedResults = searchResults
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    var pagedResult = new PagedResult<Vocabulary>
                    {
                        Items = paginatedResults,
                        Page = page,
                        PageSize = pageSize,
                        TotalCount = totalCount,
                        TotalPages = totalPages
                    };

                    return ServiceResult<PagedResult<Vocabulary>>.Success(pagedResult);
                }

                // Thử lấy từ cơ sở dữ liệu cục bộ trước
                try
                {
                    var totalCount = await _localStorage.GetVocabularyCountAsync();
                    var items = await _localStorage.GetVocabularyAsync((page - 1) * pageSize, pageSize);

                    // Nếu có dữ liệu cục bộ, trả về ngay
                    if (items.Count > 0)
                    {
                        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                        var pagedResult = new PagedResult<Vocabulary>
                        {
                            Items = items,
                            Page = page,
                            PageSize = pageSize,
                            TotalCount = totalCount,
                            TotalPages = totalPages
                        };

                        // Gọi API bất đồng bộ để cập nhật dữ liệu nền
                        _ = UpdateVocabularyFromApiAsync(page, pageSize);

                        return ServiceResult<PagedResult<Vocabulary>>.Success(pagedResult);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Không thể lấy dữ liệu từ cơ sở dữ liệu cục bộ. Thử kết nối với API...");
                }

                // Nếu không có dữ liệu cục bộ, gọi API
                var response = await _apiService.GetVocabularyAsync(page, pageSize);

                if (response.SuccessResult && response.Data != null)
                {
                    // Lưu dữ liệu vào cơ sở dữ liệu cục bộ
                    foreach (var vocabulary in response.Data.Items)
                    {
                        await _localStorage.SaveVocabularyAsync(vocabulary);
                    }

                    return ServiceResult<PagedResult<Vocabulary>>.Success(response.Data);
                }
                else if (response.SessionExpired)
                {
                    // Nếu phiên đăng nhập hết hạn, thử làm mới token
                    var refreshResult = await _authService.RefreshTokenAsync();
                    if (refreshResult.SuccessResult)
                    {
                        // Thử lại yêu cầu
                        return await GetVocabularyAsync(page, pageSize, searchQuery);
                    }
                    else
                    {
                        return ServiceResult<PagedResult<Vocabulary>>.Fail(
                            "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
                    }
                }
                else
                {
                    return ServiceResult<PagedResult<Vocabulary>>.Fail(response.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách từ vựng");
                return ServiceResult<PagedResult<Vocabulary>>.Fail($"Lỗi: {ex.Message}");
            }
        }

        /// <summary>
        /// Phương thức cập nhật dữ liệu từ API ngầm
        /// </summary>
        private async Task UpdateVocabularyFromApiAsync(int page, int pageSize)
        {
            try
            {
                var response = await _apiService.GetVocabularyAsync(page, pageSize);

                if (response.SuccessResult && response.Data != null)
                {
                    // Lưu dữ liệu vào cơ sở dữ liệu cục bộ
                    foreach (var vocabulary in response.Data.Items)
                    {
                        await _localStorage.SaveVocabularyAsync(vocabulary);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Không thể cập nhật dữ liệu từ API trong nền");
            }
        }

        /// <summary>
        /// Lấy thông tin từ vựng theo ID
        /// </summary>
        public async Task<ServiceResult<Vocabulary>> GetByIdAsync(int id)
        {
            try
            {
                // Kiểm tra xác thực
                if (!_authService.IsAuthenticated())
                {
                    return ServiceResult<Vocabulary>.Fail("Vui lòng đăng nhập để xem chi tiết từ vựng.");
                }

                // Thử lấy từ cơ sở dữ liệu cục bộ trước
                try
                {
                    var localVocabulary = await _localStorage.GetVocabularyByIdAsync(id);
                    if (localVocabulary != null)
                    {
                        // Trả về kết quả từ cơ sở dữ liệu cục bộ
                        return ServiceResult<Vocabulary>.Success(localVocabulary);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"Không thể lấy từ vựng ID {id} từ cơ sở dữ liệu cục bộ. Thử kết nối với API...");
                }

                // Nếu không có dữ liệu cục bộ, gọi API
                var response = await _apiService.GetVocabularyByIdAsync(id);

                if (response.SuccessResult && response.Data != null)
                {
                    // Lưu vào cơ sở dữ liệu cục bộ
                    await _localStorage.SaveVocabularyAsync(response.Data);
                    return ServiceResult<Vocabulary>.Success(response.Data);
                }
                else if (response.SessionExpired)
                {
                    // Nếu phiên đăng nhập hết hạn, thử làm mới token
                    var refreshResult = await _authService.RefreshTokenAsync();
                    if (refreshResult.SuccessResult)
                    {
                        // Thử lại yêu cầu
                        return await GetByIdAsync(id);
                    }
                    else
                    {
                        return ServiceResult<Vocabulary>.Fail(
                            "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
                    }
                }
                else
                {
                    return ServiceResult<Vocabulary>.Fail(response.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy thông tin từ vựng ID {id}");
                return ServiceResult<Vocabulary>.Fail($"Lỗi: {ex.Message}");
            }
        }

        /// <summary>
        /// Tạo từ vựng mới
        /// </summary>
        public async Task<ServiceResult<Vocabulary>> CreateAsync(CreateVocabularyRequest request)
        {
            try
            {
                // Kiểm tra xác thực
                if (!_authService.IsAuthenticated())
                {
                    return ServiceResult<Vocabulary>.Fail("Vui lòng đăng nhập để tạo từ vựng mới.");
                }

                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrEmpty(request.Japanese))
                {
                    return ServiceResult<Vocabulary>.Fail("Từ tiếng Nhật không được để trống.");
                }

                // Gọi API để tạo từ vựng mới
                var response = await _apiService.CreateVocabularyAsync(request);

                if (response.SuccessResult && response.Data != null)
                {
                    // Lưu vào cơ sở dữ liệu cục bộ
                    await _localStorage.SaveVocabularyAsync(response.Data);
                    return ServiceResult<Vocabulary>.Success(response.Data);
                }
                else if (response.SessionExpired)
                {
                    // Nếu phiên đăng nhập hết hạn, thử làm mới token
                    var refreshResult = await _authService.RefreshTokenAsync();
                    if (refreshResult.SuccessResult)
                    {
                        // Thử lại yêu cầu
                        return await CreateAsync(request);
                    }
                    else
                    {
                        return ServiceResult<Vocabulary>.Fail(
                            "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
                    }
                }
                else
                {
                    return ServiceResult<Vocabulary>.Fail(response.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo từ vựng mới");
                return ServiceResult<Vocabulary>.Fail($"Lỗi: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật từ vựng
        /// </summary>
        public async Task<ServiceResult<Vocabulary>> UpdateAsync(int id, UpdateVocabularyRequest request)
        {
            try
            {
                // Kiểm tra xác thực
                if (!_authService.IsAuthenticated())
                {
                    return ServiceResult<Vocabulary>.Fail("Vui lòng đăng nhập để cập nhật từ vựng.");
                }

                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrEmpty(request.Japanese))
                {
                    return ServiceResult<Vocabulary>.Fail("Từ tiếng Nhật không được để trống.");
                }

                // Gọi API để cập nhật từ vựng
                var response = await _apiService.UpdateVocabularyAsync(id, request);

                if (response.SuccessResult && response.Data != null)
                {
                    // Cập nhật vào cơ sở dữ liệu cục bộ
                    await _localStorage.SaveVocabularyAsync(response.Data);
                    return ServiceResult<Vocabulary>.Success(response.Data);
                }
                else if (response.SessionExpired)
                {
                    // Nếu phiên đăng nhập hết hạn, thử làm mới token
                    var refreshResult = await _authService.RefreshTokenAsync();
                    if (refreshResult.SuccessResult)
                    {
                        // Thử lại yêu cầu
                        return await UpdateAsync(id, request);
                    }
                    else
                    {
                        return ServiceResult<Vocabulary>.Fail(
                            "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
                    }
                }
                else
                {
                    return ServiceResult<Vocabulary>.Fail(response.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi cập nhật từ vựng ID {id}");
                return ServiceResult<Vocabulary>.Fail($"Lỗi: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa từ vựng
        /// </summary>
        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            try
            {
                // Kiểm tra xác thực
                if (!_authService.IsAuthenticated())
                {
                    return ServiceResult<bool>.Fail("Vui lòng đăng nhập để xóa từ vựng.");
                }

                // Gọi API để xóa từ vựng
                var response = await _apiService.DeleteVocabularyAsync(id);

                if (response.SuccessResult)
                {
                    // Xóa khỏi cơ sở dữ liệu cục bộ hoặc đánh dấu đã xóa
                    await _localStorage.MarkVocabularyAsDeletedAsync(id);
                    return ServiceResult<bool>.Success(true);
                }
                else if (response.SessionExpired)
                {
                    // Nếu phiên đăng nhập hết hạn, thử làm mới token
                    var refreshResult = await _authService.RefreshTokenAsync();
                    if (refreshResult.SuccessResult)
                    {
                        // Thử lại yêu cầu
                        return await DeleteAsync(id);
                    }
                    else
                    {
                        return ServiceResult<bool>.Fail(
                            "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
                    }
                }
                else
                {
                    return ServiceResult<bool>.Fail(response.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi xóa từ vựng ID {id}");
                return ServiceResult<bool>.Fail($"Lỗi: {ex.Message}");
            }
        }

        /// <summary>
        /// Lưu từ vựng vào cơ sở dữ liệu cục bộ
        /// </summary>
        public async Task<bool> SaveLocalAsync(Vocabulary vocabulary)
        {
            try
            {
                if (vocabulary == null || string.IsNullOrEmpty(vocabulary.Japanese))
                {
                    return false;
                }

                return await _localStorage.SaveVocabularyAsync(vocabulary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lưu từ vựng vào cơ sở dữ liệu cục bộ");
                return false;
            }
        }

        /// <summary>
        /// Tìm kiếm từ vựng theo từ khóa
        /// </summary>
        public async Task<ServiceResult<List<Vocabulary>>> SearchAsync(string keyword, string searchBy = "all")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return ServiceResult<List<Vocabulary>>.Fail("Từ khóa tìm kiếm không được để trống.");
                }

                // Tìm kiếm trong cơ sở dữ liệu cục bộ
                var results = await _localStorage.SearchVocabularyAsync(keyword, searchBy);

                if (results.Count > 0)
                {
                    return ServiceResult<List<Vocabulary>>.Success(results);
                }

                // Nếu không tìm thấy kết quả cục bộ, thử tìm từ API
                // Giả lập việc gọi API tìm kiếm (trong thực tế sẽ triển khai API tìm kiếm)
                var vocabResponse = await _apiService.GetVocabularyAsync(1, 100);

                if (vocabResponse.SuccessResult && vocabResponse.Data != null)
                {
                    // Lọc kết quả từ API theo từ khóa
                    var apiResults = FilterByKeyword(vocabResponse.Data.Items, keyword, searchBy);

                    // Lưu các kết quả vào cơ sở dữ liệu cục bộ
                    foreach (var vocab in apiResults)
                    {
                        await _localStorage.SaveVocabularyAsync(vocab);
                    }

                    return ServiceResult<List<Vocabulary>>.Success(apiResults);
                }
                else if (vocabResponse.SessionExpired)
                {
                    // Nếu phiên đăng nhập hết hạn, thử làm mới token
                    var refreshResult = await _authService.RefreshTokenAsync();
                    if (refreshResult.SuccessResult)
                    {
                        // Thử lại yêu cầu
                        return await SearchAsync(keyword, searchBy);
                    }
                    else
                    {
                        return ServiceResult<List<Vocabulary>>.Fail(
                            "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
                    }
                }
                else
                {
                    return ServiceResult<List<Vocabulary>>.Fail(
                        $"Không tìm thấy kết quả nào cho từ khóa '{keyword}'.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi tìm kiếm từ vựng với từ khóa '{keyword}'");
                return ServiceResult<List<Vocabulary>>.Fail($"Lỗi: {ex.Message}");
            }
        }

        /// <summary>
        /// Lọc danh sách từ vựng theo từ khóa
        /// </summary>
        private List<Vocabulary> FilterByKeyword(List<Vocabulary> vocabularies, string keyword, string searchBy)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return vocabularies;

            keyword = keyword.Trim().ToLower();

            return vocabularies.Where(v =>
            {
                if (searchBy == "japanese" || searchBy == "all")
                {
                    if (v.Japanese?.ToLower().Contains(keyword) == true)
                        return true;
                }

                if (searchBy == "kana" || searchBy == "all")
                {
                    if (v.Kana?.ToLower().Contains(keyword) == true)
                        return true;
                }

                if (searchBy == "romaji" || searchBy == "all")
                {
                    if (v.Romaji?.ToLower().Contains(keyword) == true)
                        return true;
                }

                if (searchBy == "vietnamese" || searchBy == "all")
                {
                    if (v.Vietnamese?.ToLower().Contains(keyword) == true)
                        return true;
                }

                if (searchBy == "english" || searchBy == "all")
                {
                    if (v.English?.ToLower().Contains(keyword) == true)
                        return true;
                }

                return false;
            }).ToList();
        }

        /// <summary>
        /// Đồng bộ dữ liệu với server
        /// </summary>
        public async Task<ServiceResult<SyncResult>> SyncAsync()
        {
            try
            {
                // Kiểm tra xác thực
                if (!_authService.IsAuthenticated())
                {
                    return ServiceResult<SyncResult>.Fail("Vui lòng đăng nhập để đồng bộ dữ liệu.");
                }

                // Lấy thời gian đồng bộ lần cuối
                var lastSyncTime = await _localStorage.GetLastSyncTimeAsync();

                // Lấy danh sách từ vựng cần đồng bộ
                var pendingItems = await _localStorage.GetPendingSyncVocabularyAsync();

                // Lấy danh sách ID từ vựng đã xóa cần đồng bộ
                var deletedIds = await _localStorage.GetDeletedVocabularyIdsAsync();

                // Tạo yêu cầu đồng bộ
                var syncRequest = new SyncRequest
                {
                    LastSyncTime = lastSyncTime,
                    ModifiedItems = pendingItems,
                    DeletedItemIds = deletedIds,
                    DeviceId = Environment.MachineName // Sử dụng tên máy tính làm ID thiết bị
                };

                // Gọi API để đồng bộ
                var response = await _apiService.SyncDataAsync(syncRequest);

                if (response.Success && response.Data != null)
                {
                    // Cập nhật trạng thái đồng bộ cho các phần tử đã xử lý
                    foreach (var item in pendingItems)
                    {
                        await _localStorage.MarkVocabularyAsSyncedAsync(item.VocabularyID);
                    }

                    // Cập nhật thời gian đồng bộ cuối cùng
                    await _localStorage.UpdateLastSyncTimeAsync(response.Data.SyncedAt);

                    return ServiceResult<SyncResult>.Success(response.Data);
                }
                else if (response.SessionExpired)
                {
                    // Nếu phiên đăng nhập hết hạn, thử làm mới token
                    var refreshResult = await _authService.RefreshTokenAsync();
                    if (refreshResult.SuccessResult)
                    {
                        // Thử lại yêu cầu
                        return await SyncAsync();
                    }
                    else
                    {
                        return ServiceResult<SyncResult>.Fail(
                            "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
                    }
                }
                else
                {
                    return ServiceResult<SyncResult>.Fail(response.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đồng bộ dữ liệu");
                return ServiceResult<SyncResult>.Fail($"Lỗi: {ex.Message}");
            }
        }
    }
}
