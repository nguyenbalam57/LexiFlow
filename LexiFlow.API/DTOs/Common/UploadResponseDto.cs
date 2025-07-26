namespace LexiFlow.API.DTOs.Common
{
    /// <summary>
    /// DTO cho phản hồi tải lên
    /// </summary>
    public class UploadResponseDto
    {
        /// <summary>
        /// Tên tệp gốc
        /// </summary>
        public string OriginalFileName { get; set; } = string.Empty;

        /// <summary>
        /// Tên tệp đã lưu
        /// </summary>
        public string SavedFileName { get; set; } = string.Empty;

        /// <summary>
        /// URL tệp
        /// </summary>
        public string FileUrl { get; set; } = string.Empty;

        /// <summary>
        /// Kích thước tệp (byte)
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Loại MIME
        /// </summary>
        public string MimeType { get; set; } = string.Empty;

        /// <summary>
        /// Dấu thời gian tải lên
        /// </summary>
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
