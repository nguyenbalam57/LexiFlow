namespace LexiFlow.API.DTOs.User
{
    /// <summary>
    /// DTO cho lọc danh sách người dùng
    /// </summary>
    public class UserFilterDto
    {
        /// <summary>
        /// Từ khóa tìm kiếm
        /// </summary>
        public string SearchTerm { get; set; } = string.Empty;

        /// <summary>
        /// Lọc theo vai trò
        /// </summary>
        public int? RoleId { get; set; }

        /// <summary>
        /// Lọc theo phòng ban
        /// </summary>
        public string Department { get; set; } = string.Empty;

        /// <summary>
        /// Chỉ hiển thị người dùng đang hoạt động
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Sắp xếp theo
        /// </summary>
        public string SortBy { get; set; } = "CreatedAt";

        /// <summary>
        /// Sắp xếp tăng dần
        /// </summary>
        public bool SortAscending { get; set; } = false;

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Số lượng mỗi trang
        /// </summary>
        public int PageSize { get; set; } = 20;
    }
}
