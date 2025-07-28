namespace LexiFlow.API.DTOs.Sync
{
    /// <summary>
    /// Thông tin về một mục đã xóa trong danh sách từ vựng cá nhân
    /// </summary>
    public class PersonalWordListItemDelete
    {
        /// <summary>
        /// ID mục trong danh sách
        /// </summary>
        public int ListItemID { get; set; }

        /// <summary>
        /// ID danh sách từ vựng cá nhân
        /// </summary>
        public int ListID { get; set; }

        /// <summary>
        /// Thời gian xóa
        /// </summary>
        public DateTime DeletedAt { get; set; }
    }
}
