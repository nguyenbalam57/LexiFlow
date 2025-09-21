using LexiFlow.Models.Cores;
using LexiFlow.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Medias
{
    /// <summary>
    /// Model đại diện cho file media (hình ảnh, âm thanh, video, tài liệu) trong hệ thống LexiFlow.
    /// Hỗ trợ lưu trữ và quản lý đa dạng các loại media phục vụ cho việc học tập.
    /// </summary>
    /// <remarks>
    /// Entity này cung cấp:
    /// - Lưu trữ đa dạng loại media: Image, Audio, Video, Document
    /// - Hỗ trợ nhiều entity khác nhau: Vocabulary, Kanji, Grammar, Question, v.v.
    /// - Quản lý metadata và variants (thumbnails, different resolutions)
    /// - Tích hợp CDN và xử lý bất đồng bộ
    /// - Soft delete và audit trail
    /// - Quản lý bản quyền và attribution
    /// </remarks>
    [Index(nameof(VocabularyId), nameof(MediaType), nameof(IsPrimary), Name = "IX_MediaFiles_Vocabulary")]
    [Index(nameof(UserId), nameof(MediaType), nameof(IsPrimary), Name = "IX_MediaFiles_User")]
    [Index(nameof(MediaType), nameof(IsPrimary), Name = "IX_MediaFiles_Primary")]
    public class MediaFile : AuditableEntity
    {
        /// <summary>
        /// Khóa chính của bảng media file
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MediaId { get; set; }

        #region Thông tin cơ bản về file

        /// <summary>
        /// Loại media file
        /// </summary>
        /// <value>
        /// Các giá trị hợp lệ:
        /// - "Image": Hình ảnh (JPG, PNG, GIF, WebP, SVG)
        /// - "Audio": File âm thanh (MP3, WAV, OGG, M4A)
        /// - "Video": Video (MP4, WebM, AVI, MOV)
        /// - "Document": Tài liệu (PDF, DOC, PPT, TXT)
        /// Tối đa 50 ký tự
        /// </value>
        [Required]
        [StringLength(50)]
        public string MediaType { get; set; }

        /// <summary>
        /// Tên file được lưu trong hệ thống (thường được hash hoặc rename)
        /// </summary>
        /// <value>Tên file thực tế trong storage, tối đa 255 ký tự</value>
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        /// <summary>
        /// Đường dẫn lưu trữ file trong hệ thống
        /// </summary>
        /// <value>
        /// Đường dẫn tương đối hoặc tuyệt đối đến file
        /// Ví dụ: "/uploads/media/2024/01/abc123.jpg" hoặc "media/audio/xyz789.mp3"
        /// Tối đa 255 ký tự
        /// </value>
        [Required]
        [StringLength(255)]
        public string StoragePath { get; set; }

        /// <summary>
        /// Tên file gốc khi user upload (giữ nguyên tên người dùng đặt)
        /// </summary>
        /// <value>Tên file ban đầu để hiển thị cho user, tối đa 255 ký tự</value>
        [StringLength(255)]
        public string OriginalFileName { get; set; }

        /// <summary>
        /// Loại nội dung MIME type của file
        /// </summary>
        /// <value>
        /// MIME type chuẩn như:
        /// - "image/jpeg", "image/png", "image/gif"
        /// - "audio/mpeg", "audio/wav", "audio/ogg"
        /// - "video/mp4", "video/webm"
        /// - "application/pdf", "text/plain"
        /// Tối đa 100 ký tự
        /// </value>
        [StringLength(100)]
        public string ContentType { get; set; }

        /// <summary>
        /// Kích thước file tính bằng bytes
        /// </summary>
        /// <value>Số bytes của file, dùng để quản lý storage và bandwidth</value>
        public long FileSize { get; set; }

        /// <summary>
        /// URL của file nếu được lưu trên CDN (Content Delivery Network)
        /// </summary>
        /// <value>
        /// URL đầy đủ để truy cập file từ CDN như CloudFront, CloudFlare
        /// Ví dụ: "https://cdn.example.com/media/abc123.jpg"
        /// Tối đa 255 ký tự
        /// </value>
        [StringLength(255)]
        public string CdnUrl { get; set; }

        #endregion

        #region Metadata và thông tin mở rộng

        /// <summary>
        /// Metadata của file được lưu dưới dạng JSON
        /// Đã có ở AuditableEntity.BaseEntity.Metadata
        /// </summary>
        /// <value>
        /// Chuỗi JSON chứa thông tin metadata như:
        /// - Hình ảnh: width, height, camera, location, color_profile
        /// - Âm thanh: duration, bitrate, sample_rate, channels, artist, album
        /// - Video: duration, resolution, fps, codec, thumbnail_time
        /// - Document: page_count, author, creation_date, word_count
        /// </value>
        //public string MetadataJson { get; set; }


        #endregion

        #region Liên kết với các entity khác

        /// <summary>
        /// ID của user sở hữu media này
        /// </summary>
        /// <value>Null nếu media thuộc hệ thống</value>
        public int? UserId { get; set; }

        /// <summary>
        /// ID của vocabulary mà media này thuộc về
        /// </summary>
        /// <value>Null nếu media không thuộc vocabulary nào</value>
        public int? VocabularyId { get; set; }

        /// <summary>
        /// ID của kanji mà media này thuộc về
        /// </summary>
        /// <value>Null nếu media không thuộc kanji nào</value>
        public int? KanjiId { get; set; }

        /// <summary>
        /// ID của example mà media này thuộc về
        /// </summary>
        /// <value>Null nếu media không thuộc example nào</value>
        public int? ExampleId { get; set; }

        /// <summary>
        /// ID của grammar mà media này thuộc về
        /// </summary>
        /// <value>Null nếu media không thuộc grammar nào</value>
        public int? GrammarId { get; set; }

        /// <summary>
        /// ID của question mà media này thuộc về
        /// </summary>
        /// <value>Null nếu media không thuộc question nào</value>
        public int? QuestionId { get; set; }

        /// <summary>
        /// ID của question option mà media này thuộc về
        /// </summary>
        /// <value>Null nếu media không thuộc question option nào</value>
        public int? QuestionOptionId { get; set; }

        /// <summary>
        /// ID của grammar example mà media này thuộc về
        /// </summary>
        /// <value>Null nếu media không thuộc grammar example nào</value>
        public int? GrammarExampleId { get; set; }

        /// <summary>
        /// ID của kanji example mà media này thuộc về
        /// </summary>
        /// <value>Null nếu media không thuộc kanji example nào</value>
        public int? KanjiExampleId { get; set; }

        /// <summary>
        /// ID của technical term mà media này thuộc về
        /// </summary>
        /// <value>Null nếu media không thuộc technical term nào</value>
        public int? TechnicalTermId { get; set; }

        /// <summary>
        /// ID của term example mà media này thuộc về
        /// </summary>
        /// <value>Null nếu media không thuộc term example nào</value>
        public int? TermExampleId { get; set; }

        #endregion

        #region Hiển thị và sắp xếp

        /// <summary>
        /// Thứ tự hiển thị khi có nhiều media cho cùng một entity
        /// </summary>
        /// <value>Số nguyên, số nhỏ hơn hiển thị trước. Mặc định: 0</value>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Đánh dấu đây là media chính (thumbnail hoặc audio chính)
        /// </summary>
        /// <value>
        /// - True: Là media đại diện chính (thumbnail, audio pronunciation chính)
        /// - False: Là media phụ
        /// Mỗi entity chỉ nên có 1 media primary cho mỗi MediaType. Mặc định: false
        /// </value>
        public bool IsPrimary { get; set; } = false;

        #endregion

        #region Xử lý và processing

        /// <summary>
        /// Trạng thái xử lý file (resize, convert, generate thumbnails, etc.)
        /// </summary>
        /// <value>
        /// - True: File đã được xử lý xong (thumbnails, conversions, optimization)
        /// - False: File chưa được xử lý hoặc đang trong queue xử lý
        /// Mặc định: false
        /// </value>
        public bool IsProcessed { get; set; } = false;

        /// <summary>
        /// Thời điểm file được xử lý xong
        /// </summary>
        /// <value>Null nếu chưa được xử lý</value>
        public DateTime? ProcessedAt { get; set; }

        /// <summary>
        /// Các phiên bản khác nhau của file được lưu dưới dạng JSON
        /// </summary>
        /// <value>
        /// Chuỗi JSON chứa các variant như:
        /// - Hình ảnh: {"thumbnail": "url1", "small": "url2", "medium": "url3", "large": "url4"}
        /// - Audio: {"high_quality": "url1", "medium_quality": "url2", "low_quality": "url3"}
        /// - Video: {"480p": "url1", "720p": "url2", "1080p": "url3", "thumbnail": "url4"}
        /// </value>
        public string Variants { get; set; }

        #endregion

        #region Quyền truy cập và bảo mật

        /// <summary>
        /// File có được phép truy cập công khai không cần đăng nhập
        /// </summary>
        /// <value>
        /// - True: Public, ai cũng có thể truy cập
        /// - False: Chỉ user có quyền mới truy cập được
        /// Mặc định: false
        /// </value>
        public bool IsPublic { get; set; } = false;

        #endregion

        #region Phân loại và tổ chức

        /// <summary>
        /// ID của category để nhóm các media cùng loại
        /// </summary>
        /// <value>
        /// Null nếu không thuộc category nào.
        /// Ví dụ: "Profile Pictures", "Lesson Audio", "Exercise Images"
        /// </value>
        public int? CategoryId { get; set; }

        #endregion

        #region Bản quyền và attribution

        /// <summary>
        /// Thông tin bản quyền/giấy phép của file media
        /// </summary>
        /// <value>
        /// Ví dụ: "CC BY 4.0", "All Rights Reserved", "Public Domain", "Fair Use"
        /// Tối đa 100 ký tự
        /// </value>
        [StringLength(100)]
        public string License { get; set; }

        /// <summary>
        /// Văn bản ghi công nguồn gốc/tác giả của file
        /// </summary>
        /// <value>
        /// Ví dụ: "Photo by John Doe on Unsplash", "Audio from Freesound.org"
        /// Tối đa 255 ký tự
        /// </value>
        [StringLength(255)]
        public string AttributionText { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// Thông tin vocabulary mà media này thuộc về
        /// </summary>
        [ForeignKey("VocabularyId")]
        public virtual Vocabulary Vocabulary { get; set; }

        /// <summary>
        /// Thông tin user sở hữu media này
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// Thông tin kanji mà media này thuộc về
        /// </summary>
        [ForeignKey("KanjiId")]
        public virtual Kanji Kanji { get; set; }

        /// <summary>
        /// Thông tin example mà media này thuộc về
        /// </summary>
        [ForeignKey("ExampleId")]
        public virtual Example Example { get; set; }

        /// <summary>
        /// Thông tin grammar mà media này thuộc về
        /// </summary>
        [ForeignKey("GrammarId")]
        public virtual Grammar Grammar { get; set; }

        /// <summary>
        /// Thông tin question mà media này thuộc về
        /// </summary>
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }

        /// <summary>
        /// Thông tin question option mà media này thuộc về
        /// </summary>
        [ForeignKey("QuestionOptionId")]
        public virtual QuestionOption QuestionOption { get; set; }

        /// <summary>
        /// Thông tin grammar example mà media này thuộc về
        /// </summary>
        [ForeignKey("GrammarExampleId")]
        public virtual GrammarExample GrammarExample { get; set; }

        /// <summary>
        /// Thông tin kanji example mà media này thuộc về
        /// </summary>
        [ForeignKey("KanjiExampleId")]
        public virtual KanjiExample KanjiExample { get; set; }

        /// <summary>
        /// Thông tin technical term mà media này thuộc về
        /// </summary>
        [ForeignKey("TechnicalTermId")]
        public virtual TechnicalTerm TechnicalTerm { get; set; }

        /// <summary>
        /// Thông tin term example mà media này thuộc về
        /// </summary>
        [ForeignKey("TermExampleId")]
        public virtual TermExample TermExample { get; set; }

        /// <summary>
        /// Thông tin user đã xóa media này
        /// </summary>
        [ForeignKey("DeletedBy")]
        public virtual User DeletedByUser { get; set; }

        /// <summary>
        /// Thông tin category mà media này thuộc về
        /// </summary>
        [ForeignKey("CategoryId")]
        public virtual MediaCategory Category { get; set; }

        #endregion

        #region Computed Properties (Not Mapped)

        /// <summary>
        /// Các URL variants của file dưới dạng Dictionary để dễ truy cập
        /// </summary>
        /// <value>
        /// Tự động parse từ Variants JSON thành Dictionary với key là tên variant và value là URL.
        /// Trả về empty Dictionary nếu Variants null hoặc rỗng
        /// </value>
        [NotMapped]
        public Dictionary<string, string> VariantUrls
        {
            get => string.IsNullOrEmpty(Variants)
                ? new Dictionary<string, string>()
                : JsonSerializer.Deserialize<Dictionary<string, string>>(Variants);
            set => Variants = JsonSerializer.Serialize(value);
        }

        #endregion
    }
}