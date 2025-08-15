using LexiFlow.Models.Core;
using LexiFlow.Models.Learning.Kanji;
using LexiFlow.Models.Media;
using LexiFlow.Models.Progress;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace LexiFlow.Models.Learning.Vocabulary
{
    /// <summary>
    /// Mô hình từ vựng tiếng Nhật - lưu trữ thông tin chi tiết về từ vựng
    /// Bao gồm đọc âm, nghĩa, cấp độ và các thông tin metadata
    /// </summary>
    [Index(nameof(Term), nameof(LanguageCode), Name = "IX_Vocabulary_Term_Lang")]
    [Index(nameof(CategoryId), Name = "IX_Vocabulary_Category")]
    [Index(nameof(Level), Name = "IX_Vocabulary_Level")]
    [Index(nameof(IsCommon), Name = "IX_Vocabulary_IsCommon")]
    public class Vocabulary : AuditableEntity, ISoftDeletable
    {
        /// <summary>
        /// ID duy nhất của từ vựng
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Từ vựng (bằng tiếng Nhật hoặc ngôn ngữ gốc)
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Term { get; set; }

        /// <summary>
        /// Mã ngôn ngữ (ja = tiếng Nhật, en = tiếng Anh, vi = tiếng Việt)
        /// </summary>
        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; } = "ja";

        /// <summary>
        /// Cách đọc của từ (Hiragana/Katakana cho tiếng Nhật)
        /// </summary>
        [StringLength(200)]
        public string Reading { get; set; }

        /// <summary>
        /// Cách đọc phụ (nếu có nhiều cách đọc)
        /// </summary>
        [StringLength(200)]
        public string AlternativeReadings { get; set; }

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
        /// Mức độ khó của từ (1-5, 5 là khó nhất)
        /// </summary>
        [Range(1, 5)]
        public int DifficultyLevel { get; set; } = 3;

        /// <summary>
        /// Tần suất sử dụng của từ (1-10, 10 là thường dùng nhất)
        /// </summary>
        [Range(1, 10)]
        public int FrequencyRank { get; set; } = 5;

        /// <summary>
        /// Ký hiệu phiên âm quốc tế IPA (nếu có)
        /// </summary>
        [StringLength(100)]
        public string IpaNotation { get; set; }

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
        public string UsageContext { get; set; }

        /// <summary>
        /// Ghi chú về cách sử dụng
        /// </summary>
        public string UsageNotes { get; set; }

        /// <summary>
        /// Từ đồng nghĩa (JSON format)
        /// </summary>
        public string SynonymsJson { get; set; }

        /// <summary>
        /// Từ trái nghĩa (JSON format)
        /// </summary>
        public string AntonymsJson { get; set; }

        /// <summary>
        /// Từ liên quan (JSON format)
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
        /// Trạng thái xóa mềm
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Thời gian xóa
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// ID người xóa
        /// </summary>
        public int? DeletedBy { get; set; }

        /// <summary>
        /// Trạng thái của từ vựng (Active, Pending, Rejected, etc.)
        /// </summary>
        [StringLength(50)]
        public string Status { get; set; } = "Active";

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
        /// Thời gian cập nhật nội dung cuối cùng
        /// </summary>
        public DateTime? LastContentUpdate { get; set; }

        /// <summary>
        /// Phiên bản của nội dung
        /// </summary>
        public int ContentVersion { get; set; } = 1;

        /// <summary>
        /// Có cần xem xét lại không
        /// </summary>
        public bool NeedsReview { get; set; } = false;

        /// <summary>
        /// Có được kiểm duyệt chưa
        /// </summary>
        public bool IsVerified { get; set; } = false;

        /// <summary>
        /// Thời gian kiểm duyệt
        /// </summary>
        public DateTime? VerifiedAt { get; set; }

        /// <summary>
        /// ID người kiểm duyệt
        /// </summary>
        public int? VerifiedBy { get; set; }

        /// <summary>
        /// Ghi chú nội bộ (chỉ admin)
        /// </summary>
        public string InternalNotes { get; set; }

        /// <summary>
        /// Metadata bổ sung (JSON format)
        /// </summary>
        public string MetadataJson { get; set; }

        // Navigation properties
        /// <summary>
        /// Danh mục từ vựng
        /// </summary>
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        /// <summary>
        /// Người xóa từ vựng
        /// </summary>
        [ForeignKey("DeletedBy")]
        public virtual User.User DeletedByUser { get; set; }

        /// <summary>
        /// Người kiểm duyệt
        /// </summary>
        [ForeignKey("VerifiedBy")]
        public virtual User.User VerifiedByUser { get; set; }

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
        /// Xác thực từ vựng
        /// </summary>
        /// <param name="verifiedBy">ID người xác thực</param>
        public virtual void Verify(int verifiedBy)
        {
            IsVerified = true;
            VerifiedAt = DateTime.UtcNow;
            VerifiedBy = verifiedBy;
            Status = "Active";
            UpdateTimestamp();
        }

        /// <summary>
        /// Hủy xác thực
        /// </summary>
        public virtual void Unverify()
        {
            IsVerified = false;
            VerifiedAt = null;
            VerifiedBy = null;
            Status = "Pending";
            UpdateTimestamp();
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
        /// Cập nhật nội dung và tăng version
        /// </summary>
        /// <param name="updatedBy">ID người cập nhật</param>
        public virtual void UpdateContent(int updatedBy)
        {
            ContentVersion++;
            LastContentUpdate = DateTime.UtcNow;
            UpdateModification(updatedBy, $"Cập nhật nội dung - Version {ContentVersion}");
        }

        /// <summary>
        /// Soft delete từ vựng
        /// </summary>
        /// <param name="deletedBy">ID người xóa</param>
        /// <param name="reason">Lý do xóa</param>
        public virtual void SoftDelete(int deletedBy, string reason = null)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = deletedBy;
            Status = "Deleted";
            
            if (!string.IsNullOrEmpty(reason))
            {
                InternalNotes = string.IsNullOrEmpty(InternalNotes) 
                    ? $"Xóa: {reason}" 
                    : $"{InternalNotes}\nXóa: {reason}";
            }
            
            UpdateModification(deletedBy, $"Xóa từ vựng: {reason}");
        }

        /// <summary>
        /// Khôi phục từ vựng đã xóa
        /// </summary>
        /// <param name="restoredBy">ID người khôi phục</param>
        public virtual void Restore(int restoredBy)
        {
            IsDeleted = false;
            DeletedAt = null;
            DeletedBy = null;
            Status = "Active";
            UpdateModification(restoredBy, "Khôi phục từ vựng");
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
