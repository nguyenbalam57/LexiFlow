using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Sync
{
    /// <summary>
    /// Item danh sách từ vựng cá nhân để đồng bộ
    /// </summary>
    public class PersonalWordListSyncItem
    {
        /// <summary>
        /// ID danh sách từ vựng cá nhân (có thể là ID tạm thời từ client nếu âm)
        /// </summary>
        public int ListID { get; set; }

        /// <summary>
        /// Tên danh sách
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// Màu sắc (hex code)
        /// </summary>
        [StringLength(7)]
        public string Color { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Danh sách các mục trong danh sách từ vựng
        /// </summary>
        public List<PersonalWordListItemSync> ListItems { get; set; } = new List<PersonalWordListItemSync>();

        /// <summary>
        /// Thời gian tạo trên client
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Thời gian cập nhật trên client
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Chuỗi kiểm tra xung đột (thường là hash hoặc timestamp)
        /// </summary>
        public string SyncChecksum { get; set; }

        /// <summary>
        /// ID tham chiếu trên client (sử dụng cho các mục mới)
        /// </summary>
        public string ClientReferenceId { get; set; }
    }
}
