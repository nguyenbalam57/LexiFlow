using LexiFlow.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Enums
{
    /// <summary>
    /// Đại diện cho các trạng thái hoạt động phổ biến của một thực thể.
    /// </summary>
    public enum ContentStatus
    {
        // --- Các trạng thái ban đầu và chờ xử lý (< 10) ---

        /// <summary>
        /// Bản nháp (Đang soạn thảo)
        /// Trạng thái: 0
        /// </summary>
        [Display(Name = "Bản nháp", ShortName = "Nháp")]
        [Icon("fa-pencil-alt")]
        Draft = 0,

        /// <summary>
        /// Chờ xét duyệt (Đã gửi đi)
        /// Trạng thái: 1
        /// </summary>
        [Display(Name = "Chờ xét duyệt", ShortName = "Chờ duyệt")]
        [Icon("fa-clock")]
        Review = 1,

        /// <summary>
        /// CẦN CHỈNH SỬA (Bị trả về để sửa lại)
        /// Trạng thái: 2
        /// </summary>
        [Display(Name = "Cần chỉnh sửa", ShortName = "Sửa lại")]
        [Description("Đã được xem xét nhưng bị trả lại để chỉnh sửa thêm.")]
        [Icon("fa-edit")] // Icon chỉnh sửa
        NeedsRevision = 2,

        /// <summary>
        /// Bị từ chối (Từ chối vĩnh viễn)
        /// Trạng thái: 3
        /// </summary>
        [Display(Name = "Bị từ chối", ShortName = "Từ chối")]
        [Description("Nội dung đã bị từ chối phê duyệt hoàn toàn.")]
        [Icon("fa-times-circle")]
        Rejected = 3, // <-- ĐÃ CẬP NHẬT (từ 2 lên 3)

        // --- Các trạng thái công khai và hoạt động (10-19) ---

        /// <summary>
        /// Đang hoạt động (Đã duyệt, công khai)
        /// Trạng thái: 10
        /// </summary>
        [Display(Name = "Đang hoạt động", ShortName = "Hoạt động")]
        [Icon("fa-check-circle")]
        Active = 10,

        /// <summary>
        /// Đã lên lịch (Sẽ tự động đăng)
        /// Trạng thái: 11
        /// </summary>
        [Display(Name = "Đã lên lịch", ShortName = "Lên lịch")]
        [Icon("fa-calendar-alt")]
        Scheduled = 11,

        // --- Các trạng thái ngừng hoạt động (20-29) ---

        /// <summary>
        /// Tạm ẩn / Vô hiệu hóa
        /// Trạng thái: 20
        /// </summary>
        [Display(Name = "Tạm ẩn", ShortName = "Ẩn")]
        [Icon("fa-eye-slash")]
        Inactive = 20,

        /// <summary>
        /// Không còn dùng nữa (Lỗi thời)
        /// Trạng thái: 21
        /// </summary>
        [Display(Name = "Không còn dùng", ShortName = "Lỗi thời")]
        [Icon("fa-exclamation-triangle")]
        Deprecated = 21,

        /// <summary>
        /// Đã lưu trữ (Đưa vào kho)
        /// Trạng thái: 22
        /// </summary>
        [Display(Name = "Đã lưu trữ", ShortName = "Lưu trữ")]
        [Icon("fa-archive")]
        Archived = 22,

        // --- Trạng thái cuối cùng (>= 30) ---

        /// <summary>
        /// Đã xóa (Xóa mềm)
        /// Trạng thái: 30
        /// </summary>
        [Display(Name = "Đã xóa", ShortName = "Đã xóa")]
        [Icon("fa-trash-alt")]
        Deleted = 30
    }
}
