﻿namespace LexiFlow.API.DTOs.TestAndExam
{
    /// <summary>
    /// DTO cho bài kiểm tra
    /// </summary>
    public class ExamDto
    {
        /// <summary>
        /// ID bài kiểm tra
        /// </summary>
        public int ExamID { get; set; }

        /// <summary>
        /// Tên bài kiểm tra
        /// </summary>
        public string ExamName { get; set; }

        /// <summary>
        /// Loại bài kiểm tra
        /// </summary>
        public string ExamType { get; set; }

        /// <summary>
        /// Cấp độ JLPT
        /// </summary>
        public string JLPTLevel { get; set; }

        /// <summary>
        /// Thời gian làm bài (phút)
        /// </summary>
        public int DurationMinutes { get; set; }

        /// <summary>
        /// Tổng điểm
        /// </summary>
        public int TotalPoints { get; set; }

        /// <summary>
        /// Điểm đỗ
        /// </summary>
        public int PassingScore { get; set; }

        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Danh sách phần thi
        /// </summary>
        public List<ExamSectionDto> Sections { get; set; } = new List<ExamSectionDto>();
    }
}
