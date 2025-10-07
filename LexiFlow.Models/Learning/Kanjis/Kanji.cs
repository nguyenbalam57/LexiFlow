using LexiFlow.Models.Cores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using LexiFlow.Models.Learning.Commons;
using LexiFlow.Models.Users;
using LexiFlow.Models.Medias;

namespace LexiFlow.Models.Learning.Kanjis
{
    /// <summary>
    /// Ký tự Kanji - lưu trữ thông tin chi tiết về từng ký tự Kanji
    /// Bao gồm âm Hán, âm Nhật, số nét, cấp độ JLPT và các thông tin khác
    /// </summary>
    [Index(nameof(Character), IsUnique = true, Name = "IX_Kanji_Character")]
    [Index(nameof(JLPTLevel), Name = "IX_Kanji_JLPT")]
    [Index(nameof(StrokeCount), Name = "IX_Kanji_StrokeCount")]
    [Index(nameof(Grade), Name = "IX_Kanji_Grade")]
    public class Kanji : BaseLearning
    {
        /// <summary>
        /// ID duy nhất của ký tự Kanji
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KanjiId { get; set; }

        /// <summary>
        /// Ký tự Kanji (1 ký tự Unicode)
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Character { get; set; }

        /// <summary>
        /// Âm Hán (On-yomi) - cách đọc theo âm Trung Quốc
        /// Có thể có nhiều cách đọc, ngăn cách bởi dấu phẩy
        /// </summary>
        [StringLength(200)]
        public string OnYomi { get; set; }

        /// <summary>
        /// Âm Nhật (Kun-yomi) - cách đọc bản địa Nhật Bản
        /// Có thể có nhiều cách đọc, ngăn cách bởi dấu phẩy
        /// </summary>
        [StringLength(200)]
        public string KunYomi { get; set; }

        /// <summary>
        /// Các cách đọc khác (cách đọc đặc biệt, nanori, etc.)
        /// </summary>
        [StringLength(200)]
        public string OtherReadings { get; set; }

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
        /// Số nét của ký tự Kanji
        /// </summary>
        [Range(1, 50)]
        public int StrokeCount { get; set; }

        /// <summary>
        /// Cấp độ JLPT (N5, N4, N3, N2, N1)
        /// </summary>
        [StringLength(20)]
        public string JLPTLevel { get; set; }

        /// <summary>
        /// Lớp học tại Nhật (1-6 cho tiểu học, S cho trung học)
        /// </summary>
        [StringLength(20)]
        public string Grade { get; set; }

        /// <summary>
        /// Tên bộ thủ (radical)
        /// </summary>
        [StringLength(100)]
        public string RadicalName { get; set; }

        /// <summary>
        /// Số hiệu bộ thủ theo từ điển Kangxi
        /// </summary>
        public int? RadicalNumber { get; set; }

        /// <summary>
        /// Bộ thủ dạng ký tự
        /// </summary>
        [StringLength(10)]
        public string RadicalCharacter { get; set; }

        /// <summary>
        /// Số nét của bộ thủ
        /// </summary>
        public int? RadicalStrokeCount { get; set; }

        /// <summary>
        /// Thứ tự viết nét (mô tả text hoặc JSON)
        /// </summary>
        public string StrokeOrder { get; set; }

        /// <summary>
        /// Chi tiết thứ tự viết nét (JSON format)
        /// Chứa thông tin chi tiết về cách viết từng nét
        /// </summary>
        public string StrokeOrderJson { get; set; }

        /// <summary>
        /// Mã Unicode hex của ký tự
        /// </summary>
        [StringLength(50)]
        public string UnicodeHex { get; set; }

        /// <summary>
        /// Mã JIS của ký tự (nếu có)
        /// </summary>
        [StringLength(50)]
        public string JisCode { get; set; }

        /// <summary>
        /// Gợi nhớ để học ký tự này
        /// </summary>
        [StringLength(500)]
        public string Mnemonics { get; set; }

        /// <summary>
        /// Nguồn gốc, etymology của ký tự
        /// </summary>
        [StringLength(500)]
        public string Etymology { get; set; }

        /// <summary>
        /// Lịch sử phát triển của ký tự
        /// </summary>
        public string HistoricalEvolution { get; set; }

        /// <summary>
        /// Tần suất sử dụng (1-10, 10 là thường dùng nhất)
        /// </summary>
        [Range(1, 10)]
        public int? FrequencyRank { get; set; }

        /// <summary>
        /// Mức độ khó (1-5, 1 là khó nhất)
        /// </summary>
        [Range(1, 5)]
        public int? DifficultyLevel { get; set; }

        /// <summary>
        /// Nhóm Kanji (Jouyou, Jinmeiyou, Hyougai, etc.)
        /// </summary>
        [StringLength(50)]
        public string KanjiGroup { get; set; }

        /// <summary>
        /// Phân loại theo Jouyou Kanji
        /// </summary>
        [StringLength(50)]
        public string JouyouClassification { get; set; }

        /// <summary>
        /// ID danh mục (nếu được phân loại vào danh mục)
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Từ khóa tìm kiếm (meanings, keywords)
        /// </summary>
        public string SearchKeywords { get; set; }

        /// <summary>
        /// Ghi chú bổ sung
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Thẻ tag để phân loại (JSON format)
        /// </summary>
        public string TagsJson { get; set; }

        /// <summary>
        /// Danh sách Kanji liên quan (JSON format)
        /// </summary>
        public string RelatedKanjiJson { get; set; }

        /// <summary>
        /// Các biến thể của ký tự (traditional, simplified, variants)
        /// </summary>
        public string VariantsJson { get; set; }

        /// <summary>
        /// Thông tin về các phần tử cấu thành (JSON format)
        /// </summary>
        public string ComponentsJson { get; set; }

        /// <summary>
        /// Nguồn dữ liệu (Database, User, Import, etc.)
        /// </summary>
        [StringLength(50)]
        public string Source { get; set; } = "Database";

        /// <summary>
        /// ID từ nguồn bên ngoài (nếu import)
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// Điểm phổ biến (được tính từ số lượng user học)
        /// </summary>
        public int PopularityScore { get; set; } = 0;

        /// <summary>
        /// Số lần ký tự này được học bởi users
        /// </summary>
        public int StudyCount { get; set; } = 0;

        /// <summary>
        /// Số lần ký tự này được favorite
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
        /// Ghi chú nội bộ (chỉ admin)
        /// </summary>
        public string InternalNotes { get; set; }

        // Navigation properties
        /// <summary>
        /// Danh mục Kanji thuộc về
        /// </summary>
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        /// <summary>
        /// Danh sách nghĩa của Kanji
        /// </summary>
        public virtual ICollection<Translation> Translations { get; set; }

        /// <summary>
        /// Danh sách ví dụ sử dụng Kanji
        /// </summary>
        public virtual ICollection<Example> Examples { get; set; }

        /// <summary>
        /// Danh sách định nghĩa chi tiết
        /// </summary>
        public virtual ICollection<Definition> Definitions { get; set; }

        /// <summary>
        /// Danh sách mapping với các component
        /// </summary>
        public virtual ICollection<KanjiComponentMapping> ComponentMappings { get; set; }

        /// <summary>
        /// Danh sách từ vựng chứa Kanji này
        /// </summary>
        public virtual ICollection<KanjiVocabulary> KanjiVocabularies { get; set; }

        /// <summary>
        /// Danh sách file media liên quan (hình ảnh stroke order, âm thanh, etc.)
        /// </summary>
        public virtual ICollection<MediaFile> MediaFiles { get; set; }

        // Computed Properties
        /// <summary>
        /// Danh sách thẻ tag
        /// </summary>
        [NotMapped]
        public List<string> TagsList
        {
            get => string.IsNullOrEmpty(TagsJson)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(TagsJson);
            set => TagsJson = JsonSerializer.Serialize(value);
        }

        /// <summary>
        /// Danh sách Kanji liên quan
        /// </summary>
        [NotMapped]
        public List<string> RelatedKanjiList
        {
            get => string.IsNullOrEmpty(RelatedKanjiJson)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(RelatedKanjiJson);
            set => RelatedKanjiJson = JsonSerializer.Serialize(value);
        }

        /// <summary>
        /// Danh sách biến thể
        /// </summary>
        [NotMapped]
        public List<string> VariantsList
        {
            get => string.IsNullOrEmpty(VariantsJson)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(VariantsJson);
            set => VariantsJson = JsonSerializer.Serialize(value);
        }

        /// <summary>
        /// Cấp độ số của JLPT (5 = N5, 1 = N1)
        /// </summary>
        [NotMapped]
        public int JLPTLevelNumber
        {
            get
            {
                if (string.IsNullOrEmpty(JLPTLevel)) return 6; // Không có cấp độ
                return JLPTLevel.ToUpper() switch
                {
                    "N1" => 1,
                    "N2" => 2,
                    "N3" => 3,
                    "N4" => 4,
                    "N5" => 5,
                    _ => 6
                };
            }
        }

        /// <summary>
        /// Lớp học số (1-6 cho tiểu học, 7+ cho trung học)
        /// </summary>
        [NotMapped]
        public int GradeNumber
        {
            get
            {
                if (string.IsNullOrEmpty(Grade)) return 0;
                if (int.TryParse(Grade, out int grade)) return grade;
                return Grade.ToUpper() switch
                {
                    "S" => 7, // Secondary school
                    "JH" => 7, // Junior high
                    "SH" => 10, // Senior high
                    _ => 0
                };
            }
        }

        /// <summary>
        /// Có phải Kanji thường dùng không (Jouyou)
        /// </summary>
        [NotMapped]
        public bool IsJouyou => !string.IsNullOrEmpty(KanjiGroup) && 
            KanjiGroup.ToLower().Contains("jouyou");

        /// <summary>
        /// Có phải Kanji dùng cho tên người không (Jinmeiyou)
        /// </summary>
        [NotMapped]
        public bool IsJinmeiyou => !string.IsNullOrEmpty(KanjiGroup) && 
            KanjiGroup.ToLower().Contains("jinmeiyou");

        // Methods
        /// <summary>
        /// Thêm thẻ tag
        /// </summary>
        /// <param name="tag">Thẻ tag mới</param>
        public virtual void AddTag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag)) return;
            
            var tags = TagsList;
            if (!tags.Contains(tag))
            {
                tags.Add(tag);
                TagsList = tags;
                UpdateTimestamp();
            }
        }

        /// <summary>
        /// Thêm Kanji liên quan
        /// </summary>
        /// <param name="relatedKanji">Kanji liên quan</param>
        public virtual void AddRelatedKanji(string relatedKanji)
        {
            if (string.IsNullOrWhiteSpace(relatedKanji)) return;
            
            var related = RelatedKanjiList;
            if (!related.Contains(relatedKanji))
            {
                related.Add(relatedKanji);
                RelatedKanjiList = related;
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
        /// Lấy tất cả cách đọc của Kanji
        /// </summary>
        /// <returns>Danh sách cách đọc</returns>
        public virtual List<string> GetAllReadings()
        {
            var readings = new List<string>();
            
            if (!string.IsNullOrEmpty(OnYomi))
                readings.AddRange(OnYomi.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim()));
                
            if (!string.IsNullOrEmpty(KunYomi))
                readings.AddRange(KunYomi.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim()));
                
            if (!string.IsNullOrEmpty(OtherReadings))
                readings.AddRange(OtherReadings.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim()));
            
            return readings.Distinct().ToList();
        }

        /// <summary>
        /// Lấy tên hiển thị của Kanji
        /// </summary>
        /// <returns>Tên hiển thị</returns>
        public override string GetDisplayName()
        {
            var display = Character;
            var readings = new List<string>();
            
            if (!string.IsNullOrEmpty(OnYomi))
                readings.Add($"On: {OnYomi}");
            if (!string.IsNullOrEmpty(KunYomi))
                readings.Add($"Kun: {KunYomi}");
                
            if (readings.Any())
                display += $" ({string.Join(", ", readings)})";
                
            return display;
        }

        /// <summary>
        /// Validate Kanji
        /// </summary>
        /// <returns>True nếu hợp lệ</returns>
        public override bool IsValid()
        {
            return base.IsValid() 
                && !string.IsNullOrWhiteSpace(Character)
                && Character.Length <= 10
                && StrokeCount >= 1 && StrokeCount <= 50
                && (FrequencyRank == null || (FrequencyRank >= 1 && FrequencyRank <= 10))
                && (DifficultyLevel == null || (DifficultyLevel >= 1 && DifficultyLevel <= 5))
                && (AverageRating == null || (AverageRating >= 1 && AverageRating <= 5));
        }
    }
}
