using LexiFlow.Models.Cores;
using LexiFlow.Models.Learning.Commons;
using LexiFlow.Models.Learning.Kanjis;
using LexiFlow.Models.Medias;
using LexiFlow.Models.Progress;
using LexiFlow.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace LexiFlow.Models.Learning.Vocabularys
{
    /// <summary>
    /// Mô hình từ vựng tiếng Nhật - lưu trữ thông tin chi tiết về từ vựng
    /// Bao gồm đọc âm, nghĩa, cấp độ và các thông tin metadata
    /// </summary>
    [Index(nameof(Term), nameof(LanguageCode), Name = "IX_Vocabulary_Term_Lang")]
    [Index(nameof(CategoryId), Name = "IX_Vocabulary_Category")]
    [Index(nameof(Level), Name = "IX_Vocabulary_Level")]
    [Index(nameof(IsCommon), Name = "IX_Vocabulary_IsCommon")]
    public class Vocabulary : BaseLearning
    {
        /// <summary>
        /// ID duy nhất của từ vựng
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VocabularyId { get; set; }

        /// <summary>
        /// Từ vựng (bằng tiếng Nhật hoặc ngôn ngữ gốc)
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Term { get; set; }

        /// <summary>
        /// Cách đọc của từ (Hiragana/Katakana cho tiếng Nhật)
        /// </summary>
        public string Reading { get; set; }

        /// <summary>
        /// Cách đọc phụ (nếu có nhiều cách đọc)
        /// </summary>
        public string AlternativeReadings { get; set; }

        /// <summary>
        /// Mã ngôn ngữ của pattern ngữ pháp
        /// </summary>
        /// <value>
        /// Code chuẩn ISO 639-1:
        /// - "ja": Tiếng Nhật (mặc định)
        /// - "en": Tiếng Anh
        /// - "vi": Tiếng Việt
        /// Mặc định: "ja"
        /// </value>
        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; } = "ja"; // Ngôn ngữ định nghĩa

        /// <summary>
        /// Cấp độ JLPT (N5, N4, N3, N2, N1)
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Level { get; set; }

        /// <summary>
        /// ID danh mục từ vựng
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Mức độ khó của từ (1-5, 1 là khó nhất)
        /// </summary>
        [Range(1, 5)]
        public int DifficultyLevel { get; set; } = 3;

        /// <summary>
        /// Tần suất sử dụng của từ (1-10, 10 là thường dùng nhất)
        /// </summary>
        [Range(1, 10)]
        public int FrequencyRank { get; set; } = 5;

        /// <summary>
        /// Có phải từ thông dụng không
        /// </summary>
        public bool IsCommon { get; set; } = false;

        /// <summary>
        /// Có phải từ ở dạng chính thức không
        /// </summary>
        public bool IsFormal { get; set; } = true;

        /// <summary>
        /// Có phải từ lóng/không chính thức không
        /// </summary>
        public bool IsSlang { get; set; } = false;

        /// <summary>
        /// Có phải từ cổ/lỗi thời không
        /// </summary>
        public bool IsArchaic { get; set; } = false;

        /// <summary>
        /// Loại từ (noun, verb, adjective, adverb, particle, etc.)
        /// </summary>
        [StringLength(50)]
        public string PartOfSpeech { get; set; }

        /// <summary>
        /// Chi tiết loại từ (i-adjective, na-adjective, transitive verb, etc.)
        /// </summary>
        [StringLength(100)]
        public string DetailedPartOfSpeech { get; set; }

        /// <summary>
        /// Dạng thể (polite, casual, humble, honorific)
        /// </summary>
        [StringLength(50)]
        public string PolitenessLevel { get; set; }

        /// <summary>
        /// Bối cảnh sử dụng (business, casual, academic, etc.)
        /// </summary>
        [StringLength(100)]
        public string Context { get; set; }

        /// <summary>
        /// Ghi chú về cách sử dụng
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Từ đồng nghĩa (JSON format)
        /// Hỗ trợ lưu nhiều từ đồng nghĩa và dạng danh sách Id
        /// </summary>
        public string SynonymsJson { get; set; }

        /// <summary>
        /// Từ trái nghĩa (JSON format)
        /// Hỗ trợ lưu nhiều từ trái nghĩa và dạng danh sách Id
        /// </summary>
        public string AntonymsJson { get; set; }

        /// <summary>
        /// Từ liên quan (JSON format)
        /// Hỗ trợ lưu nhiều từ liên quan và dạng danh sách Id
        /// </summary>
        public string RelatedWordsJson { get; set; }

        /// <summary>
        /// Thẻ tag để phân loại (JSON format)
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Vector tìm kiếm toàn văn
        /// </summary>
        public string SearchVector { get; set; }

        /// <summary>
        /// Từ khóa tìm kiếm bổ sung
        /// </summary>
        public string SearchKeywords { get; set; }

        /// <summary>
        /// Nguồn gốc của từ (Dictionary, User, Import, etc.)
        /// </summary>
        [StringLength(50)]
        public string Source { get; set; } = "Dictionary";

        /// <summary>
        /// ID từ nguồn bên ngoài (nếu import)
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// Điểm phổ biến (được tính từ số lượng user học)
        /// </summary>
        public int PopularityScore { get; set; } = 0;

        /// <summary>
        /// Số lần từ này được học bởi users
        /// </summary>
        public int StudyCount { get; set; } = 0;

        /// <summary>
        /// Số lần từ này được favorite
        /// </summary>
        public int FavoriteCount { get; set; } = 0;

        /// <summary>
        /// Điểm đánh giá trung bình (1-5)
        /// </summary>
        [Range(1, 5)]
        public double? AverageRating { get; set; }

        /// <summary>
        /// Số lượt đánh giá
        /// </summary>
        public int RatingCount { get; set; } = 0;

        /// <summary>
        /// Có cần xem xét lại không
        /// </summary>
        public bool NeedsReview { get; set; } = false;

        /// <summary>
        /// Ghi chú nội bộ (chỉ admin)
        /// </summary>
        public string InternalNotes { get; set; } = "";

        // Navigation properties
        /// <summary>
        /// Danh mục từ vựng
        /// </summary>
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        /// <summary>
        /// Danh sách định nghĩa của từ
        /// </summary>
        public virtual ICollection<Definition> Definitions { get; set; }

        /// <summary>
        /// Danh sách ví dụ sử dụng từ
        /// </summary>
        public virtual ICollection<Example> Examples { get; set; }

        /// <summary>
        /// Danh sách bản dịch của từ
        /// </summary>
        public virtual ICollection<Translation> Translations { get; set; }

        /// <summary>
        /// Danh sách file media liên quan (âm thanh, hình ảnh)
        /// </summary>
        public virtual ICollection<MediaFile> MediaFiles { get; set; }

        /// <summary>
        /// Danh sách tiến trình học tập của users
        /// </summary>
        public virtual ICollection<LearningProgress> LearningProgresses { get; set; }

        /// <summary>
        /// Danh sách liên kết với kanji
        /// </summary>
        public virtual ICollection<KanjiVocabulary> KanjiVocabularies { get; set; }

        // Computed Properties (properties không lưu DB)
        /// <summary>
        /// Danh sách từ đồng nghĩa
        /// </summary>
        [NotMapped]
        public List<string> SynonymsList
        {
            get => string.IsNullOrEmpty(SynonymsJson)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(SynonymsJson);
            set => SynonymsJson = JsonSerializer.Serialize(value);
        }

        /// <summary>
        /// Danh sách từ trái nghĩa
        /// </summary>
        [NotMapped]
        public List<string> AntonymsList
        {
            get => string.IsNullOrEmpty(AntonymsJson)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(AntonymsJson);
            set => AntonymsJson = JsonSerializer.Serialize(value);
        }

        /// <summary>
        /// Danh sách từ liên quan
        /// </summary>
        [NotMapped]
        public List<string> RelatedWordsList
        {
            get => string.IsNullOrEmpty(RelatedWordsJson)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(RelatedWordsJson);
            set => RelatedWordsJson = JsonSerializer.Serialize(value);
        }

        /// <summary>
        /// Danh sách thẻ tag
        /// </summary>
        [NotMapped]
        public List<string> TagList
        {
            get => string.IsNullOrEmpty(Tags)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(Tags);
            set => Tags = JsonSerializer.Serialize(value);
        }

        /// <summary>
        /// Cấp độ số của JLPT (5 = N5, 1 = N1)
        /// </summary>
        [NotMapped]
        public int JLPTLevelNumber
        {
            get
            {
                if (string.IsNullOrEmpty(Level)) return 5;
                return Level.ToUpper() switch
                {
                    "N1" => 1,
                    "N2" => 2,
                    "N3" => 3,
                    "N4" => 4,
                    "N5" => 5,
                    _ => 5
                };
            }
        }

        /// <summary>
        /// Kiểm tra có phải động từ không
        /// </summary>
        [NotMapped]
        public bool IsVerb => !string.IsNullOrEmpty(PartOfSpeech) && 
            PartOfSpeech.ToLower().Contains("verb");

        /// <summary>
        /// Kiểm tra có phải danh từ không
        /// </summary>
        [NotMapped]
        public bool IsNoun => !string.IsNullOrEmpty(PartOfSpeech) && 
            PartOfSpeech.ToLower().Contains("noun");

        /// <summary>
        /// Kiểm tra có phải tính từ không
        /// </summary>
        [NotMapped]
        public bool IsAdjective => !string.IsNullOrEmpty(PartOfSpeech) && 
            PartOfSpeech.ToLower().Contains("adjective");

        // Methods
        /// <summary>
        /// Thêm từ đồng nghĩa
        /// </summary>
        /// <param name="synonym">Từ đồng nghĩa</param>
        public virtual void AddSynonym(string synonym)
        {
            if (string.IsNullOrWhiteSpace(synonym)) return;
            
            var synonyms = SynonymsList;
            if (!synonyms.Contains(synonym))
            {
                synonyms.Add(synonym);
                SynonymsList = synonyms;
                UpdateTimestamp();
            }
        }

        /// <summary>
        /// Thêm từ trái nghĩa
        /// </summary>
        /// <param name="antonym">Từ trái nghĩa</param>
        public virtual void AddAntonym(string antonym)
        {
            if (string.IsNullOrWhiteSpace(antonym)) return;
            
            var antonyms = AntonymsList;
            if (!antonyms.Contains(antonym))
            {
                antonyms.Add(antonym);
                AntonymsList = antonyms;
                UpdateTimestamp();
            }
        }

        /// <summary>
        /// Thêm thẻ tag
        /// </summary>
        /// <param name="tag">Thẻ tag</param>
        public virtual void AddTag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag)) return;
            
            var tags = TagList;
            if (!tags.Contains(tag))
            {
                tags.Add(tag);
                TagList = tags;
                UpdateTimestamp();
            }
        }

        /// <summary>
        /// Cập nhật điểm phổ biến
        /// </summary>
        public virtual void UpdatePopularityScore()
        {
            // Tính điểm dựa trên số lượt học, favorite và rating
            PopularityScore = StudyCount + (FavoriteCount * 2) + 
                (int)((AverageRating ?? 0) * RatingCount);
            UpdateTimestamp();
        }

        /// <summary>
        /// Tăng số lượt học
        /// </summary>
        public virtual void IncrementStudyCount()
        {
            StudyCount++;
            UpdatePopularityScore();
        }

        /// <summary>
        /// Tăng số lượt favorite
        /// </summary>
        public virtual void IncrementFavoriteCount()
        {
            FavoriteCount++;
            UpdatePopularityScore();
        }

        /// <summary>
        /// Thêm đánh giá
        /// </summary>
        /// <param name="rating">Điểm đánh giá (1-5)</param>
        public virtual void AddRating(int rating)
        {
            if (rating < 1 || rating > 5) return;
            
            var totalScore = (AverageRating ?? 0) * RatingCount + rating;
            RatingCount++;
            AverageRating = totalScore / RatingCount;
            UpdatePopularityScore();
        }

        /// <summary>
        /// Đánh dấu cần xem xét lại
        /// </summary>
        /// <param name="reason">Lý do cần xem xét</param>
        public virtual void MarkForReview(string reason = null)
        {
            NeedsReview = true;
            if (!string.IsNullOrEmpty(reason))
            {
                InternalNotes = string.IsNullOrEmpty(InternalNotes) 
                    ? $"Cần xem xét: {reason}" 
                    : $"{InternalNotes}\nCần xem xét: {reason}";
            }
            UpdateTimestamp();
        }

        /// <summary>
        /// Lấy tên hiển thị của từ vựng
        /// </summary>
        /// <returns>Tên hiển thị</returns>
        public override string GetDisplayName()
        {
            var display = Term;
            if (!string.IsNullOrEmpty(Reading) && Reading != Term)
                display += $" ({Reading})";
            return display;
        }

        /// <summary>
        /// Validate từ vựng
        /// </summary>
        /// <returns>True nếu hợp lệ</returns>
        public override bool IsValid()
        {
            return base.IsValid() 
                && !string.IsNullOrWhiteSpace(Term)
                && !string.IsNullOrWhiteSpace(LanguageCode)
                && !string.IsNullOrWhiteSpace(Level)
                && DifficultyLevel >= 1 && DifficultyLevel <= 5
                && FrequencyRank >= 1 && FrequencyRank <= 10
                && (AverageRating == null || (AverageRating >= 1 && AverageRating <= 5));
        }
    }
}
