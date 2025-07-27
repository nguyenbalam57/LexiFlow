namespace LexiFlow.API.DTOs.User
{
    /// <summary>
    /// DTO cho quyền
    /// </summary>
    public class PermissionDto
    {
        /// <summary>
        /// ID quyền
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên quyền
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả quyền
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Module của quyền
        /// </summary>
        public string Module { get; set; } = string.Empty;

        /// <summary>
        /// Chuỗi phiên bản hàng
        /// </summary>
        public string RowVersionString { get; set; } = string.Empty;
    }
}
