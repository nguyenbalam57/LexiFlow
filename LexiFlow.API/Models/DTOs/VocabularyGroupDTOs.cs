using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.Models.DTOs
{
    /// <summary>
    /// Data transfer object for creating a vocabulary group
    /// </summary>
    public class CreateVocabularyGroupDto
    {
        /// <summary>
        /// Name of the vocabulary group
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string GroupName { get; set; } = string.Empty;

        /// <summary>
        /// Description of the vocabulary group
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Category ID this group belongs to
        /// </summary>
        public int? CategoryId { get; set; }
    }

    /// <summary>
    /// Data transfer object for updating a vocabulary group
    /// </summary>
    public class UpdateVocabularyGroupDto : CreateVocabularyGroupDto
    {
        /// <summary>
        /// Row version for concurrency checking
        /// </summary>
        public string RowVersionString { get; set; } = string.Empty;

        /// <summary>
        /// Flag indicating if the group is active
        /// </summary>
        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// Data transfer object for vocabulary group summary
    /// </summary>
    public class VocabularyGroupSummaryDto
    {
        /// <summary>
        /// Group ID
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// Name of the vocabulary group
        /// </summary>
        public string GroupName { get; set; } = string.Empty;

        /// <summary>
        /// Description of the vocabulary group
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Category ID this group belongs to
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Category name
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        /// Number of vocabulary items in this group
        /// </summary>
        public int VocabularyCount { get; set; }

        /// <summary>
        /// Flag indicating if the group is active
        /// </summary>
        public bool IsActive { get; set; }
    }

    #region VocabularyGroup DTOs

    /// <summary>
    /// DTO cho phản hồi API khi lấy thông tin nhóm từ vựng chi tiết
    /// </summary>
    public class VocabularyGroupDetailDto
    {
        /// <summary>
        /// ID của nhóm từ vựng
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// Tên nhóm từ vựng
        /// </summary>
        public string GroupName { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả về nhóm từ vựng
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// ID của danh mục mà nhóm thuộc về
        /// </summary>
        public int? CategoryID { get; set; }

        /// <summary>
        /// Tên của danh mục mà nhóm thuộc về
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        /// ID của người dùng đã tạo nhóm
        /// </summary>
        public int? CreatedByUserID { get; set; }

        /// <summary>
        /// Tên của người dùng đã tạo nhóm
        /// </summary>
        public string? CreatedByUserName { get; set; }

        /// <summary>
        /// Trạng thái hoạt động của nhóm
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Số lượng từ vựng trong nhóm
        /// </summary>
        public int VocabularyCount { get; set; }

        /// <summary>
        /// Thời gian tạo nhóm
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Thời gian cập nhật nhóm
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Chuỗi phiên bản hàng (dùng cho kiểm soát đồng thời)
        /// </summary>
        public string RowVersionString { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO cho phản hồi API khi lấy danh sách nhóm từ vựng
    /// </summary>
    public class VocabularyGroupListDto
    {
        /// <summary>
        /// ID của nhóm từ vựng
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// Tên nhóm từ vựng
        /// </summary>
        public string GroupName { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả về nhóm từ vựng
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// ID của danh mục mà nhóm thuộc về
        /// </summary>
        public int? CategoryID { get; set; }

        /// <summary>
        /// Tên của danh mục mà nhóm thuộc về
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        /// Trạng thái hoạt động của nhóm
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Số lượng từ vựng trong nhóm
        /// </summary>
        public int VocabularyCount { get; set; }
    }

    /// <summary>
    /// DTO cho phản hồi API với dữ liệu phân trang
    /// </summary>
    public class PagedResponseDto<T>
    {
        /// <summary>
        /// Danh sách các mục dữ liệu
        /// </summary>
        public List<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Kích thước trang
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Tổng số mục dữ liệu
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Kiểm tra có trang trước không
        /// </summary>
        public bool HasPrevious => CurrentPage > 1;

        /// <summary>
        /// Kiểm tra có trang sau không
        /// </summary>
        public bool HasNext => CurrentPage < TotalPages;
    }

    /// <summary>
    /// DTO cho thống kê về nhóm từ vựng
    /// </summary>
    public class VocabularyGroupStatsDto
    {
        /// <summary>
        /// Tổng số nhóm từ vựng
        /// </summary>
        public int TotalGroups { get; set; }

        /// <summary>
        /// Số nhóm từ vựng đang hoạt động
        /// </summary>
        public int ActiveGroups { get; set; }

        /// <summary>
        /// Thống kê số nhóm theo danh mục
        /// </summary>
        public List<CategoryStatsDto> GroupsByCategory { get; set; } = new List<CategoryStatsDto>();

        /// <summary>
        /// Top nhóm từ vựng theo số lượng từ
        /// </summary>
        public List<GroupVocabularyCountDto> TopGroupsByVocabularyCount { get; set; } = new List<GroupVocabularyCountDto>();

        /// <summary>
        /// Các nhóm mới cập nhật gần đây
        /// </summary>
        public List<RecentlyUpdatedGroupDto> RecentlyUpdatedGroups { get; set; } = new List<RecentlyUpdatedGroupDto>();
    }

    /// <summary>
    /// DTO cho thống kê theo danh mục
    /// </summary>
    public class CategoryStatsDto
    {
        /// <summary>
        /// ID của danh mục
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Tên của danh mục
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        /// Số lượng nhóm từ vựng trong danh mục
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// DTO cho số lượng từ vựng trong nhóm
    /// </summary>
    public class GroupVocabularyCountDto
    {
        /// <summary>
        /// ID của nhóm từ vựng
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// Tên nhóm từ vựng
        /// </summary>
        public string GroupName { get; set; } = string.Empty;

        /// <summary>
        /// Số lượng từ vựng trong nhóm
        /// </summary>
        public int VocabularyCount { get; set; }
    }

    /// <summary>
    /// DTO cho nhóm từ vựng mới cập nhật
    /// </summary>
    public class RecentlyUpdatedGroupDto
    {
        /// <summary>
        /// ID của nhóm từ vựng
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// Tên nhóm từ vựng
        /// </summary>
        public string GroupName { get; set; } = string.Empty;

        /// <summary>
        /// Thời gian cập nhật
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO cho tìm kiếm nhóm từ vựng
    /// </summary>
    public class VocabularyGroupSearchDto
    {
        /// <summary>
        /// Từ khóa tìm kiếm
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// Chỉ bao gồm nhóm hoạt động
        /// </summary>
        public bool ActiveOnly { get; set; } = true;

        /// <summary>
        /// ID danh mục để lọc
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Sắp xếp theo
        /// </summary>
        public string? SortBy { get; set; } = "GroupName";

        /// <summary>
        /// Hướng sắp xếp
        /// </summary>
        public string? SortDirection { get; set; } = "Asc";

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Kích thước trang
        /// </summary>
        public int PageSize { get; set; } = 20;
    }

    /// <summary>
    /// DTO cho việc thêm từ vựng vào nhóm
    /// </summary>
    public class AddVocabularyToGroupDto
    {
        /// <summary>
        /// ID của từ vựng cần thêm vào nhóm
        /// </summary>
        [Required]
        public int VocabularyId { get; set; }
    }

    /// <summary>
    /// DTO cho việc xóa từ vựng khỏi nhóm
    /// </summary>
    public class RemoveVocabularyFromGroupDto
    {
        /// <summary>
        /// ID của từ vựng cần xóa khỏi nhóm
        /// </summary>
        [Required]
        public int VocabularyId { get; set; }
    }

    #endregion
}