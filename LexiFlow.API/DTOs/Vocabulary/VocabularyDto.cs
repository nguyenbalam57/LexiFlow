using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Vocabulary
{
    /// <summary>
    /// DTO cho từ vựng
    /// </summary>
    public class VocabularyDto
    {
        /// <summary>
        /// ID từ vựng
        /// </summary>
        public int VocabularyId { get; set; }

        /// <summary>
        /// Từ vựng (tiếng Nhật)
        /// </summary>
        public string Word { get; set; } = string.Empty;

        /// <summary>
        /// Hiragana
        /// </summary>
        public string Hiragana { get; set; } = string.Empty;

        /// <summary>
        /// Katakana
        /// </summary>
        public string Katakana { get; set; } = string.Empty;

        /// <summary>
        /// Romaji (phiên âm)
        /// </summary>
        public string Romaji { get; set; } = string.Empty;

        /// <summary>
        /// Nghĩa (tiếng Việt/Anh)
        /// </summary>
        public string Meaning { get; set; } = string.Empty;

        /// <summary>
        /// Ví dụ
        /// </summary>
        public string Example { get; set; } = string.Empty;

        /// <summary>
        /// Nghĩa của ví dụ
        /// </summary>
        public string ExampleMeaning { get; set; } = string.Empty;

        /// <summary>
        /// Cấp độ JLPT
        /// </summary>
        public string Level { get; set; } = string.Empty;

        /// <summary>
        /// ID danh mục
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Tên danh mục
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Loại từ (noun, verb, adj, etc.)
        /// </summary>
        public string WordType { get; set; } = string.Empty;

        /// <summary>
        /// Mức độ khó (1-5)
        /// </summary>
        public int Difficulty { get; set; } = 1;

        /// <summary>
        /// Tần suất sử dụng (1-10)
        /// </summary>
        public int Frequency { get; set; } = 5;

        /// <summary>
        /// Phiên âm IPA
        /// </summary>
        public string IpaNotation { get; set; } = string.Empty;

        /// <summary>
        /// Có phải từ thông dụng
        /// </summary>
        public bool IsCommon { get; set; } = false;

        /// <summary>
        /// Tags (JSON array)
        /// </summary>
        public List<string> Tags { get; set; } = new List<string>();

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        /// URL âm thanh
        /// </summary>
        public string AudioUrl { get; set; } = string.Empty;

        /// <summary>
        /// URL hình ảnh
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Thời gian cập nhật
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO cho tạo từ vựng mới
    /// </summary>
    public class CreateVocabularyDto
    {
        /// <summary>
        /// Từ vựng (tiếng Nhật)
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Word { get; set; } = string.Empty;

        /// <summary>
        /// Hiragana
        /// </summary>
        [StringLength(200)]
        public string Hiragana { get; set; } = string.Empty;

        /// <summary>
        /// Katakana
        /// </summary>
        [StringLength(200)]
        public string Katakana { get; set; } = string.Empty;

        /// <summary>
        /// Romaji (phiên âm)
        /// </summary>
        [StringLength(200)]
        public string Romaji { get; set; } = string.Empty;

        /// <summary>
        /// Nghĩa (tiếng Việt/Anh)
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Meaning { get; set; } = string.Empty;

        /// <summary>
        /// Ví dụ
        /// </summary>
        [StringLength(500)]
        public string Example { get; set; } = string.Empty;

        /// <summary>
        /// Nghĩa của ví dụ
        /// </summary>
        [StringLength(500)]
        public string ExampleMeaning { get; set; } = string.Empty;

        /// <summary>
        /// Cấp độ JLPT
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Level { get; set; } = string.Empty;

        /// <summary>
        /// ID danh mục
        /// </summary>
        [Required]
        public int CategoryId { get; set; }

        /// <summary>
        /// Loại từ
        /// </summary>
        [StringLength(50)]
        public string WordType { get; set; } = string.Empty;

        /// <summary>
        /// Mức độ khó (1-5)
        /// </summary>
        [Range(1, 5)]
        public int Difficulty { get; set; } = 1;

        /// <summary>
        /// Tần suất sử dụng (1-10)
        /// </summary>
        [Range(1, 10)]
        public int Frequency { get; set; } = 5;

        /// <summary>
        /// Phiên âm IPA
        /// </summary>
        [StringLength(200)]
        public string IpaNotation { get; set; } = string.Empty;

        /// <summary>
        /// Có phải từ thông dụng
        /// </summary>
        public bool IsCommon { get; set; } = false;

        /// <summary>
        /// Tags
        /// </summary>
        public List<string> Tags { get; set; } = new List<string>();

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Notes { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO cho cập nhật từ vựng
    /// </summary>
    public class UpdateVocabularyDto
    {
        /// <summary>
        /// Từ vựng (tiếng Nhật)
        /// </summary>
        [StringLength(100)]
        public string? Word { get; set; }

        /// <summary>
        /// Hiragana
        /// </summary>
        [StringLength(200)]
        public string? Hiragana { get; set; }

        /// <summary>
        /// Katakana
        /// </summary>
        [StringLength(200)]
        public string? Katakana { get; set; }

        /// <summary>
        /// Romaji (phiên âm)
        /// </summary>
        [StringLength(200)]
        public string? Romaji { get; set; }

        /// <summary>
        /// Nghĩa (tiếng Việt/Anh)
        /// </summary>
        [StringLength(500)]
        public string? Meaning { get; set; }

        /// <summary>
        /// Ví dụ
        /// </summary>
        [StringLength(500)]
        public string? Example { get; set; }

        /// <summary>
        /// Nghĩa của ví dụ
        /// </summary>
        [StringLength(500)]
        public string? ExampleMeaning { get; set; }

        /// <summary>
        /// Cấp độ JLPT
        /// </summary>
        [StringLength(10)]
        public string? Level { get; set; }

        /// <summary>
        /// ID danh mục
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Loại từ
        /// </summary>
        [StringLength(50)]
        public string? WordType { get; set; }

        /// <summary>
        /// Mức độ khó (1-5)
        /// </summary>
        [Range(1, 5)]
        public int? Difficulty { get; set; }

        /// <summary>
        /// Tần suất sử dụng (1-10)
        /// </summary>
        [Range(1, 10)]
        public int? Frequency { get; set; }

        /// <summary>
        /// Phiên âm IPA
        /// </summary>
        [StringLength(200)]
        public string? IpaNotation { get; set; }

        /// <summary>
        /// Có phải từ thông dụng
        /// </summary>
        public bool? IsCommon { get; set; }

        /// <summary>
        /// Tags
        /// </summary>
        public List<string>? Tags { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        public bool? IsActive { get; set; }
    }
}
