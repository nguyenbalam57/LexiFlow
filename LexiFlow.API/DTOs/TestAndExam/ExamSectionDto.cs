﻿namespace LexiFlow.API.DTOs.TestAndExam
{
    /// <summary>
    /// DTO cho phần thi
    /// </summary>
    public class ExamSectionDto
    {
        /// <summary>
        /// ID phần thi
        /// </summary>
        public int SectionID { get; set; }

        /// <summary>
        /// ID bài kiểm tra
        /// </summary>
        public int ExamID { get; set; }

        /// <summary>
        /// Tên phần thi
        /// </summary>
        public string SectionName { get; set; }

        /// <summary>
        /// Loại phần thi
        /// </summary>
        public string SectionType { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Thời gian làm bài (phút)
        /// </summary>
        public int DurationMinutes { get; set; }

        /// <summary>
        /// Tổng điểm
        /// </summary>
        public int TotalPoints { get; set; }

        /// <summary>
        /// Hướng dẫn
        /// </summary>
        public string Instructions { get; set; }

        /// <summary>
        /// Danh sách câu hỏi
        /// </summary>
        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
    }
}
