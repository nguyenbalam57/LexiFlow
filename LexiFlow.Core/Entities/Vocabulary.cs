using System;
using System.ComponentModel.DataAnnotations;

namespace LexiFlow.Core.Entities
{
    /// <summary>
    /// Lớp thực thể từ vựng
    /// </summary>
    public class Vocabulary
    {
        /// <summary>
        /// ID của từ vựng
        /// </summary>
        public int VocabularyID { get; set; }

        /// <summary>
        /// Từ tiếng Nhật (Kanji)
        /// </summary>
        [Required]
        public string Japanese { get; set; } = string.Empty;

        /// <summary>
        /// Hiragana/Katakana
        /// </summary>
        public string? Kana { get; set; }

        /// <summary>
        /// Phiên âm Latin
        /// </summary>
        public string? Romaji { get; set; }

        /// <summary>
        /// Nghĩa tiếng Việt
        /// </summary>
        public string? Vietnamese { get; set; }

        /// <summary>
        /// Nghĩa tiếng Anh
        /// </summary>
        public string? English { get; set; }

        /// <summary>
        /// Ví dụ sử dụng
        /// </summary>
        public string? Example { get; set; }

        /// <summary>
        /// Ghi chú bổ sung
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// ID nhóm từ vựng
        /// </summary>
        public int? GroupID { get; set; }

        /// <summary>
        /// Cấp độ JLPT (N5, N4, N3, N2, N1)
        /// </summary>
        public string? Level { get; set; }

        /// <summary>
        /// Loại từ (Danh từ, Động từ, Tính từ, v.v.)
        /// </summary>
        public string? PartOfSpeech { get; set; }

        /// <summary>
        /// Đường dẫn đến file âm thanh
        /// </summary>
        public string? AudioFile { get; set; }

        /// <summary>
        /// ID người tạo
        /// </summary>
        public int? CreatedByUserID { get; set; }

        /// <summary>
        /// ID người cập nhật
        /// </summary>
        public int? UpdatedByUserID { get; set; }

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Thời gian cập nhật
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Phiên bản dữ liệu (dùng cho đồng bộ)
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// Trạng thái đồng bộ (New, Modified, Synced, Deleted)
        /// </summary>
        public string? SyncStatus { get; set; } = "New";
    }
}