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

namespace LexiFlow.Models.Progress
{
    /// <summary>
    /// Tiến trình học tập từ vựng - theo dõi chi tiết quá trình học từng từ vựng của người dùng
    /// Sử dụng thuật toán spaced repetition để tối ưu hóa việc ôn tập
    /// </summary>
    [Index(nameof(UserId), nameof(VocabularyId), IsUnique = true, Name = "IX_User_Vocabulary")]
    [Index(nameof(UserId), nameof(NextReviewDate), Name = "IX_User_NextReview")]
    [Index(nameof(LastStudied), Name = "IX_LearningProgress_LastStudied")]
    [Index(nameof(NextReviewDate), Name = "IX_LearningProgress_NextReviewDate")]
    [Index(nameof(MasteryLevel), Name = "IX_LearningProgress_MasteryLevel")]
    public class LearningProgress : AuditableEntity
    {
        /// <summary>
        /// ID duy nhất của tiến trình học tập
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProgressId { get; set; }

        /// <summary>
        /// ID người dùng
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// ID từ vựng
        /// </summary>
        [Required]
        public int VocabularyId { get; set; }

        /// <summary>
        /// Tổng số lần học từ này
        /// </summary>
        public int StudyCount { get; set; } = 0;

        /// <summary>
        /// Số lần trả lời đúng
        /// </summary>
        public int CorrectCount { get; set; } = 0;

        /// <summary>
        /// Số lần trả lời sai
        /// </summary>
        public int IncorrectCount { get; set; } = 0;

        /// <summary>
        /// Lần học cuối cùng
        /// </summary>
        public DateTime? LastStudied { get; set; }

        /// <summary>
        /// Lần đầu tiên học từ này
        /// </summary>
        public DateTime? FirstStudied { get; set; }

        /// <summary>
        /// Lần đánh dấu đã thuộc
        /// </summary>
        public DateTime? MasteredAt { get; set; }

        // Spaced Repetition Algorithm Properties
        /// <summary>
        /// Độ mạnh của memory (0-10, 10 là nhớ rất tốt)
        /// </summary>
        [Range(0, 10)]
        public int MemoryStrength { get; set; } = 0;

        /// <summary>
        /// Hệ số dễ dàng cho thuật toán spaced repetition (1.3 - 2.5)
        /// </summary>
        [Range(1.3, 2.5)]
        public double EaseFactor { get; set; } = 2.5;

        /// <summary>
        /// Khoảng cách ngày cho lần ôn tập tiếp theo
        /// </summary>
        public int IntervalDays { get; set; } = 1;

        /// <summary>
        /// Số lần trả lời đúng liên tiếp
        /// </summary>
        public int ConsecutiveCorrect { get; set; } = 0;

        /// <summary>
        /// Số lần trả lời sai liên tiếp
        /// </summary>
        public int ConsecutiveIncorrect { get; set; } = 0;

        /// <summary>
        /// Ngày cần ôn tập tiếp theo
        /// </summary>
        public DateTime? NextReviewDate { get; set; }

        /// <summary>
        /// Có cần ôn tập ngay bây giờ không
        /// </summary>
        public bool NeedsReview { get; set; } = true;

        // Skill-specific tracking
        /// <summary>
        /// Mức độ nhận biết từ (0-10, 10 là thành thạo)
        /// </summary>
        [Range(0, 10)]
        public int RecognitionLevel { get; set; } = 0;

        /// <summary>
        /// Mức độ viết từ (0-10, 10 là thành thạo)
        /// </summary>
        [Range(0, 10)]
        public int WritingLevel { get; set; } = 0;

        /// <summary>
        /// Mức độ nghe từ (0-10, 10 là thành thạo)
        /// </summary>
        [Range(0, 10)]
        public int ListeningLevel { get; set; } = 0;

        /// <summary>
        /// Mức độ nói từ (0-10, 10 là thành thạo)
        /// </summary>
        [Range(0, 10)]
        public int SpeakingLevel { get; set; } = 0;

        /// <summary>
        /// Mức độ thành thạo tổng thể (0-10)
        /// </summary>
        [Range(0, 10)]
        public int MasteryLevel { get; set; } = 0;

        // Response time tracking
        /// <summary>
        /// Thời gian phản hồi trung bình (milliseconds)
        /// </summary>
        public int AverageResponseTimeMs { get; set; } = 0;

        /// <summary>
        /// Thời gian phản hồi lần cuối (milliseconds)
        /// </summary>
        public int LastResponseTimeMs { get; set; } = 0;

        /// <summary>
        /// Thời gian phản hồi nhanh nhất (milliseconds)
        /// </summary>
        public int BestResponseTimeMs { get; set; } = int.MaxValue;

        /// <summary>
        /// Thời gian phản hồi chậm nhất (milliseconds)
        /// </summary>
        public int WorstResponseTimeMs { get; set; } = 0;

        // Learning context and notes
        /// <summary>
        /// Ghi chú học tập của người dùng
        /// </summary>
        public string LearningNotes { get; set; }

        /// <summary>
        /// Các lỗi thường gặp (JSON format)
        /// </summary>
        public string CommonMistakesJson { get; set; }

        /// <summary>
        /// Mẹo ghi nhớ cá nhân
        /// </summary>
        public string PersonalMnemonics { get; set; }

        /// <summary>
        /// Ngữ cảnh đã gặp từ này (JSON format)
        /// </summary>
        public string EncounteredContextsJson { get; set; }

        // Progress flags and preferences
        /// <summary>
        /// Có được đánh dấu bookmark không
        /// </summary>
        public bool IsBookmarked { get; set; } = false;

        /// <summary>
        /// Có được đánh dấu là khó không
        /// </summary>
        public bool IsMarkedAsDifficult { get; set; } = false;

        /// <summary>
        /// Có được đánh dấu đã thuộc không
        /// </summary>
        public bool IsMarkedAsMastered { get; set; } = false;

        /// <summary>
        /// Độ ưu tiên học (1-5, 5 là cao nhất)
        /// </summary>
        [Range(1, 5)]
        public int Priority { get; set; } = 3;

        /// <summary>
        /// Tần suất xuất hiện trong bài học
        /// </summary>
        public int AppearanceFrequency { get; set; } = 0;

        // Statistical data
        /// <summary>
        /// Tổng thời gian học từ này (phút)
        /// </summary>
        public int TotalStudyTimeMinutes { get; set; } = 0;

        /// <summary>
        /// Số ngày liên tục ôn tập
        /// </summary>
        public int StudyStreak { get; set; } = 0;

        /// <summary>
        /// Số ngày liên tục ôn tập tối đa
        /// </summary>
        public int MaxStudyStreak { get; set; } = 0;

        /// <summary>
        /// Tỷ lệ trả lời đúng (%)
        /// </summary>
        [Range(0, 100)]
        public double AccuracyRate { get; set; } = 0;

        /// <summary>
        /// Xu hướng học tập (Improving, Stable, Declining)
        /// </summary>
        [StringLength(20)]
        public string LearningTrend { get; set; } = "Stable";

        /// <summary>
        /// Loại học tập ưa thích (Visual, Auditory, Kinesthetic, Reading)
        /// </summary>
        [StringLength(20)]
        public string PreferredLearningStyle { get; set; }

        /// <summary>
        /// Thời gian tốt nhất trong ngày để học từ này
        /// </summary>
        [StringLength(20)]
        public string OptimalStudyTime { get; set; }

        /// <summary>
        /// Điểm confidence của người dùng với từ này (1-10)
        /// </summary>
        [Range(1, 10)]
        public int ConfidenceLevel { get; set; } = 1;

        /// <summary>
        /// Số lần reset lại tiến trình
        /// </summary>
        public int ResetCount { get; set; } = 0;

        /// <summary>
        /// Metadata bổ sung (JSON format)
        /// </summary>
        public string MetadataJson { get; set; }

        // Navigation properties
        /// <summary>
        /// Người dùng sở hữu tiến trình này
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        /// <summary>
        /// Từ vựng được học
        /// </summary>
        [ForeignKey("VocabularyId")]
        public virtual Learning.Vocabulary.Vocabulary Vocabulary { get; set; }

        // Computed Properties
        /// <summary>
        /// Tỷ lệ thành công tổng thể
        /// </summary>
        [NotMapped]
        public double SuccessRate
        {
            get
            {
                var total = CorrectCount + IncorrectCount;
                return total > 0 ? (double)CorrectCount / total * 100 : 0;
            }
        }

        /// <summary>
        /// Số ngày đã học từ này
        /// </summary>
        [NotMapped]
        public int DaysLearning
        {
            get
            {
                if (!FirstStudied.HasValue) return 0;
                return (DateTime.UtcNow - FirstStudied.Value).Days + 1;
            }
        }

        /// <summary>
        /// Có quá hạn ôn tập không
        /// </summary>
        [NotMapped]
        public bool IsOverdue
        {
            get
            {
                return NextReviewDate.HasValue && DateTime.UtcNow > NextReviewDate.Value;
            }
        }

        /// <summary>
        /// Có sắp đến hạn ôn tập không (trong 24h)
        /// </summary>
        [NotMapped]
        public bool IsDueSoon
        {
            get
            {
                return NextReviewDate.HasValue && 
                       NextReviewDate.Value <= DateTime.UtcNow.AddHours(24) &&
                       NextReviewDate.Value > DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Mức độ thành thạo trung bình tất cả kỹ năng
        /// </summary>
        [NotMapped]
        public double AverageSkillLevel
        {
            get
            {
                return (RecognitionLevel + WritingLevel + ListeningLevel + SpeakingLevel) / 4.0;
            }
        }

        // Methods
        /// <summary>
        /// Ghi nhận một lần học tập
        /// </summary>
        /// <param name="isCorrect">Có trả lời đúng không</param>
        /// <param name="responseTimeMs">Thời gian phản hồi</param>
        /// <param name="skillType">Loại kỹ năng được test</param>
        /// <param name="difficulty">Mức độ khó của câu hỏi (1-5)</param>
        public virtual void RecordStudySession(bool isCorrect, int responseTimeMs = 0, 
            string skillType = "Recognition", int difficulty = 3)
        {
            StudyCount++;
            LastStudied = DateTime.UtcNow;
            
            if (FirstStudied == null)
                FirstStudied = DateTime.UtcNow;

            if (isCorrect)
            {
                CorrectCount++;
                ConsecutiveCorrect++;
                ConsecutiveIncorrect = 0;
                
                // Cập nhật skill level dựa trên loại test
                UpdateSkillLevel(skillType, true, difficulty);
                
                // Cập nhật spaced repetition
                UpdateSpacedRepetition(true, difficulty);
            }
            else
            {
                IncorrectCount++;
                ConsecutiveIncorrect++;
                ConsecutiveCorrect = 0;
                
                UpdateSkillLevel(skillType, false, difficulty);
                UpdateSpacedRepetition(false, difficulty);
            }

            // Cập nhật thời gian phản hồi
            if (responseTimeMs > 0)
            {
                UpdateResponseTime(responseTimeMs);
            }

            // Cập nhật accuracy rate
            AccuracyRate = SuccessRate;
            
            // Cập nhật mastery level
            UpdateMasteryLevel();
            
            // Cập nhật learning trend
            UpdateLearningTrend();

            UpdateTimestamp();
        }

        /// <summary>
        /// Cập nhật mức độ kỹ năng cụ thể
        /// </summary>
        /// <param name="skillType">Loại kỹ năng</param>
        /// <param name="isCorrect">Có đúng không</param>
        /// <param name="difficulty">Mức độ khó</param>
        private void UpdateSkillLevel(string skillType, bool isCorrect, int difficulty)
        {
            var improvement = isCorrect ? 1 : -1;
            var adjustedImprovement = improvement * (difficulty / 3.0); // Điều chỉnh theo độ khó

            switch (skillType.ToLower())
            {
                case "recognition":
                    RecognitionLevel = Math.Max(0, Math.Min(10, RecognitionLevel + (int)adjustedImprovement));
                    break;
                case "writing":
                    WritingLevel = Math.Max(0, Math.Min(10, WritingLevel + (int)adjustedImprovement));
                    break;
                case "listening":
                    ListeningLevel = Math.Max(0, Math.Min(10, ListeningLevel + (int)adjustedImprovement));
                    break;
                case "speaking":
                    SpeakingLevel = Math.Max(0, Math.Min(10, SpeakingLevel + (int)adjustedImprovement));
                    break;
            }
        }

        /// <summary>
        /// Cập nhật thuật toán spaced repetition
        /// </summary>
        /// <param name="isCorrect">Có trả lời đúng không</param>
        /// <param name="difficulty">Mức độ khó (1-5)</param>
        private void UpdateSpacedRepetition(bool isCorrect, int difficulty)
        {
            if (isCorrect)
            {
                // Tăng memory strength
                MemoryStrength = Math.Min(10, MemoryStrength + 1);
                
                // Tăng ease factor nếu dễ
                if (difficulty <= 2)
                {
                    EaseFactor = Math.Min(2.5, EaseFactor + 0.1);
                }
                
                // Tăng interval
                if (MemoryStrength == 1)
                    IntervalDays = 1;
                else if (MemoryStrength == 2)
                    IntervalDays = 6;
                else
                    IntervalDays = (int)(IntervalDays * EaseFactor);
            }
            else
            {
                // Giảm memory strength
                MemoryStrength = Math.Max(0, MemoryStrength - 2);
                
                // Giảm ease factor
                EaseFactor = Math.Max(1.3, EaseFactor - 0.2);
                
                // Reset interval
                IntervalDays = 1;
            }

            // Tính ngày ôn tập tiếp theo
            NextReviewDate = DateTime.UtcNow.AddDays(IntervalDays);
            NeedsReview = false;
        }

        /// <summary>
        /// Cập nhật thời gian phản hồi
        /// </summary>
        /// <param name="responseTimeMs">Thời gian phản hồi mới</param>
        private void UpdateResponseTime(int responseTimeMs)
        {
            LastResponseTimeMs = responseTimeMs;
            
            // Cập nhật best/worst time
            if (responseTimeMs < BestResponseTimeMs)
                BestResponseTimeMs = responseTimeMs;
            if (responseTimeMs > WorstResponseTimeMs)
                WorstResponseTimeMs = responseTimeMs;

            // Cập nhật average time
            if (AverageResponseTimeMs == 0)
                AverageResponseTimeMs = responseTimeMs;
            else
                AverageResponseTimeMs = (AverageResponseTimeMs + responseTimeMs) / 2;
        }

        /// <summary>
        /// Cập nhật mức độ thành thạo tổng thể
        /// </summary>
        private void UpdateMasteryLevel()
        {
            var skillAverage = AverageSkillLevel;
            var accuracyFactor = AccuracyRate / 100.0;
            var memoryFactor = MemoryStrength / 10.0;
            
            MasteryLevel = (int)Math.Round((skillAverage * 0.5 + accuracyFactor * 10 * 0.3 + memoryFactor * 10 * 0.2));
            MasteryLevel = Math.Max(0, Math.Min(10, MasteryLevel));

            // Đánh dấu mastered nếu đạt ngưỡng
            if (MasteryLevel >= 8 && !IsMarkedAsMastered)
            {
                IsMarkedAsMastered = true;
                MasteredAt = DateTime.UtcNow;
            }
            else if (MasteryLevel < 6)
            {
                IsMarkedAsMastered = false;
                MasteredAt = null;
            }
        }

        /// <summary>
        /// Cập nhật xu hướng học tập
        /// </summary>
        private void UpdateLearningTrend()
        {
            if (ConsecutiveCorrect >= 3)
                LearningTrend = "Improving";
            else if (ConsecutiveIncorrect >= 3)
                LearningTrend = "Declining";
            else
                LearningTrend = "Stable";
        }

        /// <summary>
        /// Đặt lại tiến trình học tập
        /// </summary>
        /// <param name="reason">Lý do reset</param>
        public virtual void ResetProgress(string reason = null)
        {
            ResetCount++;
            MemoryStrength = 0;
            EaseFactor = 2.5;
            IntervalDays = 1;
            ConsecutiveCorrect = 0;
            ConsecutiveIncorrect = 0;
            RecognitionLevel = 0;
            WritingLevel = 0;
            ListeningLevel = 0;
            SpeakingLevel = 0;
            MasteryLevel = 0;
            IsMarkedAsMastered = false;
            MasteredAt = null;
            NextReviewDate = DateTime.UtcNow;
            NeedsReview = true;
            LearningTrend = "Stable";
            
            if (!string.IsNullOrEmpty(reason))
            {
                LearningNotes = string.IsNullOrEmpty(LearningNotes) 
                    ? $"Reset: {reason}" 
                    : $"{LearningNotes}\nReset: {reason}";
            }

            UpdateTimestamp();
        }

        /// <summary>
        /// Đánh dấu cần ôn tập
        /// </summary>
        public virtual void MarkForReview()
        {
            NeedsReview = true;
            NextReviewDate = DateTime.UtcNow;
            UpdateTimestamp();
        }

        /// <summary>
        /// Cập nhật ghi chú học tập
        /// </summary>
        /// <param name="note">Ghi chú mới</param>
        public virtual void AddNote(string note)
        {
            if (string.IsNullOrWhiteSpace(note)) return;
            
            var timestamp = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm");
            var timestampedNote = $"[{timestamp}] {note}";
            
            LearningNotes = string.IsNullOrEmpty(LearningNotes) 
                ? timestampedNote 
                : $"{LearningNotes}\n{timestampedNote}";
                
            UpdateTimestamp();
        }

        /// <summary>
        /// Toggle bookmark
        /// </summary>
        public virtual void ToggleBookmark()
        {
            IsBookmarked = !IsBookmarked;
            UpdateTimestamp();
        }

        /// <summary>
        /// Đánh dấu là khó
        /// </summary>
        public virtual void MarkAsDifficult()
        {
            IsMarkedAsDifficult = true;
            Priority = Math.Max(Priority, 4); // Tăng priority
            UpdateTimestamp();
        }

        /// <summary>
        /// Lấy tên hiển thị của tiến trình
        /// </summary>
        /// <returns>Tên hiển thị</returns>
        public override string GetDisplayName()
        {
            return $"{User?.Username ?? "User"} - {Vocabulary?.Term ?? "Vocabulary"} (Mastery: {MasteryLevel}/10)";
        }

        /// <summary>
        /// Validate tiến trình học tập
        /// </summary>
        /// <returns>True nếu hợp lệ</returns>
        public override bool IsValid()
        {
            return base.IsValid() 
                && UserId > 0 
                && VocabularyId > 0
                && StudyCount >= 0
                && CorrectCount >= 0
                && IncorrectCount >= 0
                && MemoryStrength >= 0 && MemoryStrength <= 10
                && EaseFactor >= 1.3 && EaseFactor <= 2.5
                && RecognitionLevel >= 0 && RecognitionLevel <= 10
                && WritingLevel >= 0 && WritingLevel <= 10
                && ListeningLevel >= 0 && ListeningLevel <= 10
                && SpeakingLevel >= 0 && SpeakingLevel <= 10
                && MasteryLevel >= 0 && MasteryLevel <= 10
                && Priority >= 1 && Priority <= 5
                && AccuracyRate >= 0 && AccuracyRate <= 100;
        }
    }
}
