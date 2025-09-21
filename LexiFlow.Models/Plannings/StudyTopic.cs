using LexiFlow.Models.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Planning
{
    /// <summary>
    /// Chủ đề học tập - định nghĩa các chủ đề/lĩnh vực học tập trong hệ thống
    /// Có thể được sử dụng để nhóm các mục tiêu, nhiệm vụ và phiên học tập
    /// </summary>
    [Index(nameof(TopicName), nameof(Category), IsUnique = true, Name = "IX_StudyTopic_Name_Category")]
    [Index(nameof(JLPTLevel), Name = "IX_StudyTopic_JLPTLevel")]
    [Index(nameof(ParentTopicId), Name = "IX_StudyTopic_Parent")]
    public class StudyTopic : AuditableEntity
    {
        /// <summary>
        /// ID duy nhất của chủ đề học tập
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TopicId { get; set; }

        /// <summary>
        /// Tên của chủ đề học tập
        /// </summary>
        [Required]
        [StringLength(100)]
        public string TopicName { get; set; }

        /// <summary>
        /// Mô tả chi tiết về chủ đề
        /// </summary>
        [StringLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// Danh mục của chủ đề (Vocabulary, Grammar, Reading, Listening, etc.)
        /// </summary>
        [StringLength(50)]
        public string Category { get; set; }

        /// <summary>
        /// ID của chủ đề cha (nếu là chủ đề con)
        /// </summary>
        public int? ParentTopicId { get; set; }

        /// <summary>
        /// ID phiên học tập liên quan (nếu có)
        /// </summary>
        public int? SessionId { get; set; }

        /// <summary>
        /// Đường dẫn tới icon của chủ đề
        /// </summary>
        [StringLength(255)]
        public string IconPath { get; set; }

        /// <summary>
        /// Mã màu hiển thị của chủ đề (HEX format)
        /// </summary>
        [StringLength(20)]
        public string ColorCode { get; set; } = "#2196F3";

        /// <summary>
        /// Thứ tự hiển thị của chủ đề
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Cấp độ JLPT mà chủ đề này thuộc về
        /// </summary>
        [StringLength(10)]
        public string JLPTLevel { get; set; } // N5, N4, N3, N2, N1

        /// <summary>
        /// Mức độ khó của chủ đề (1-5, 5 là khó nhất)
        /// </summary>
        [Range(1, 5)]
        public int? DifficultyLevel { get; set; } = 3;

        /// <summary>
        /// Số giờ học ước tính để hoàn thành chủ đề
        /// </summary>
        public int? EstimatedStudyHours { get; set; }

        /// <summary>
        /// Danh sách từ vựng chính của chủ đề (JSON format)
        /// </summary>
        public string KeyVocabularyJson { get; set; }

        /// <summary>
        /// Danh sách ngữ pháp chính của chủ đề (JSON format)
        /// </summary>
        public string KeyGrammarJson { get; set; }

        /// <summary>
        /// Danh sách ID các chủ đề liên quan
        /// </summary>
        public string RelatedTopicsIds { get; set; }

        /// <summary>
        /// Tài nguyên học tập cho chủ đề (JSON format)
        /// </summary>
        public string LearningResourcesJson { get; set; }

        /// <summary>
        /// Tài liệu đề xuất cho chủ đề (JSON format)
        /// </summary>
        public string RecommendedMaterialsJson { get; set; }

        /// <summary>
        /// Mục tiêu học tập của chủ đề
        /// </summary>
        public string LearningObjectives { get; set; }

        /// <summary>
        /// Điều kiện tiên quyết để học chủ đề này
        /// </summary>
        public string Prerequisites { get; set; }

        /// <summary>
        /// Kỹ năng sẽ đạt được sau khi hoàn thành chủ đề
        /// </summary>
        public string ExpectedOutcomes { get; set; }

        /// <summary>
        /// Phương pháp đánh giá cho chủ đề
        /// </summary>
        public string AssessmentMethods { get; set; }

        /// <summary>
        /// Số người dùng đang học chủ đề này
        /// </summary>
        public int ActiveUserCount { get; set; } = 0;

        /// <summary>
        /// Tổng số người dùng đã học chủ đề này
        /// </summary>
        public int TotalUserCount { get; set; } = 0;

        /// <summary>
        /// Số ngày hoàn thành trung bình của chủ đề
        /// </summary>
        public double? AverageCompletionDays { get; set; }

        /// <summary>
        /// Tỷ lệ thành công của chủ đề (%)
        /// </summary>
        [Range(0, 100)]
        public double? SuccessRate { get; set; }

        /// <summary>
        /// Điểm đánh giá trung bình của chủ đề (1-5)
        /// </summary>
        [Range(1, 5)]
        public double? AverageRating { get; set; }

        /// <summary>
        /// Số lượt đánh giá
        /// </summary>
        public int RatingCount { get; set; } = 0;

        /// <summary>
        /// Từ khóa tìm kiếm cho chủ đề
        /// </summary>
        public string SearchKeywords { get; set; }

        /// <summary>
        /// Thẻ tag để phân loại chủ đề
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Có phải chủ đề phổ biến không
        /// </summary>
        public bool IsPopular { get; set; } = false;

        /// <summary>
        /// Có phải chủ đề bắt buộc không
        /// </summary>
        public bool IsRequired { get; set; } = false;

        /// <summary>
        /// Có được đề xuất cho người mới bắt đầu không
        /// </summary>
        public bool IsRecommendedForBeginners { get; set; } = false;

        /// <summary>
        /// Thời gian cập nhật nội dung cuối cùng
        /// </summary>
        public DateTime? LastContentUpdate { get; set; }

        /// <summary>
        /// Phiên bản của chủ đề
        /// </summary>
        public int ContentVersion { get; set; } = 1;

        /// <summary>
        /// Ghi chú nội bộ về chủ đề
        /// </summary>
        public string InternalNotes { get; set; }

        // Navigation properties
        /// <summary>
        /// Chủ đề cha (nếu là chủ đề con)
        /// </summary>
        [ForeignKey("ParentTopicId")]
        public virtual StudyTopic ParentTopic { get; set; }

        /// <summary>
        /// Phiên học tập liên quan (nếu có)
        /// </summary>
        [ForeignKey("SessionId")]
        public virtual StudySession StudySession { get; set; }

        /// <summary>
        /// Danh sách các chủ đề con
        /// </summary>
        public virtual ICollection<StudyTopic> ChildTopics { get; set; }

        /// <summary>
        /// Danh sách các mục tiêu học tập thuộc chủ đề này
        /// </summary>
        public virtual ICollection<StudyGoal> StudyGoals { get; set; }

        /// <summary>
        /// Danh sách các mục trong kế hoạch học tập thuộc chủ đề này
        /// </summary>
        public virtual ICollection<StudyPlanItem> StudyPlanItems { get; set; }

        /// <summary>
        /// Danh sách các phiên học tập thuộc chủ đề này
        /// </summary>
        public virtual ICollection<StudySession> StudySessions { get; set; }

        // Computed Properties
        /// <summary>
        /// Số chủ đề con
        /// </summary>
        [NotMapped]
        public int ChildTopicCount => ChildTopics?.Count ?? 0;

        /// <summary>
        /// Có phải chủ đề gốc không (không có parent)
        /// </summary>
        [NotMapped]
        public bool IsRootTopic => ParentTopicId == null;

        /// <summary>
        /// Có phải chủ đề lá không (không có con)
        /// </summary>
        [NotMapped]
        public bool IsLeafTopic => ChildTopicCount == 0;

        /// <summary>
        /// Mức độ của chủ đề trong cây phân cấp
        /// </summary>
        [NotMapped]
        public int TopicLevel
        {
            get
            {
                int level = 0;
                var current = this.ParentTopic;
                while (current != null)
                {
                    level++;
                    current = current.ParentTopic;
                }
                return level;
            }
        }

        // Methods
        /// <summary>
        /// Lấy đường dẫn đầy đủ của chủ đề (từ root đến current)
        /// </summary>
        /// <returns>Đường dẫn chủ đề</returns>
        public virtual string GetTopicPath()
        {
            var path = new List<string>();
            var current = this;
            
            while (current != null)
            {
                path.Insert(0, current.TopicName);
                current = current.ParentTopic;
            }
            
            return string.Join(" > ", path);
        }

        /// <summary>
        /// Lấy tất cả chủ đề con (recursive)
        /// </summary>
        /// <returns>Danh sách tất cả chủ đề con</returns>
        public virtual List<StudyTopic> GetAllChildTopics()
        {
            var allChildren = new List<StudyTopic>();
            
            if (ChildTopics != null)
            {
                foreach (var child in ChildTopics)
                {
                    allChildren.Add(child);
                    allChildren.AddRange(child.GetAllChildTopics());
                }
            }
            
            return allChildren;
        }

        /// <summary>
        /// Cập nhật thống kê của chủ đề
        /// </summary>
        /// <param name="newUserCount">Số người dùng mới</param>
        /// <param name="completionRate">Tỷ lệ hoàn thành</param>
        /// <param name="avgDays">Số ngày hoàn thành trung bình</param>
        public virtual void UpdateStatistics(int? newUserCount = null, double? completionRate = null, double? avgDays = null)
        {
            if (newUserCount.HasValue)
            {
                TotalUserCount = Math.Max(TotalUserCount, newUserCount.Value);
            }
            
            if (completionRate.HasValue)
            {
                SuccessRate = Math.Max(0, Math.Min(100, completionRate.Value));
            }
            
            if (avgDays.HasValue)
            {
                AverageCompletionDays = Math.Max(0, avgDays.Value);
            }
            
            UpdateTimestamp();
        }

        /// <summary>
        /// Thêm đánh giá cho chủ đề
        /// </summary>
        /// <param name="rating">Điểm đánh giá (1-5)</param>
        public virtual void AddRating(int rating)
        {
            if (rating < 1 || rating > 5) return;
            
            var totalScore = (AverageRating ?? 0) * RatingCount + rating;
            RatingCount++;
            AverageRating = totalScore / RatingCount;
            
            UpdateTimestamp();
        }

        /// <summary>
        /// Lấy tên hiển thị của chủ đề
        /// </summary>
        /// <returns>Tên hiển thị</returns>
        public override string GetDisplayName()
        {
            return IsRootTopic ? TopicName : GetTopicPath();
        }

        /// <summary>
        /// Validate chủ đề
        /// </summary>
        /// <returns>True nếu hợp lệ</returns>
        public override bool IsValid()
        {
            return base.IsValid() 
                && !string.IsNullOrWhiteSpace(TopicName)
                && (DifficultyLevel == null || (DifficultyLevel >= 1 && DifficultyLevel <= 5))
                && (AverageRating == null || (AverageRating >= 1 && AverageRating <= 5))
                && (SuccessRate == null || (SuccessRate >= 0 && SuccessRate <= 100))
                && ParentTopicId != TopicId; // Không thể là parent của chính nó
        }
    }
}
