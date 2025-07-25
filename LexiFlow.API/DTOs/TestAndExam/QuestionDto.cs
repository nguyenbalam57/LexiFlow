﻿namespace LexiFlow.API.DTOs.TestAndExam
{
    /// <summary>
    /// DTO cho câu hỏi
    /// </summary>
    public class QuestionDto
    {
        /// <summary>
        /// ID câu hỏi
        /// </summary>
        public int QuestionID { get; set; }

        /// <summary>
        /// ID phần thi
        /// </summary>
        public int SectionID { get; set; }

        /// <summary>
        /// Nội dung câu hỏi
        /// </summary>
        public string QuestionText { get; set; }

        /// <summary>
        /// Loại câu hỏi
        /// </summary>
        public string QuestionType { get; set; }

        /// <summary>
        /// Điểm số
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Độ khó
        /// </summary>
        public string DifficultyLevel { get; set; }

        /// <summary>
        /// URL media
        /// </summary>
        public string MediaURL { get; set; }

        /// <summary>
        /// Danh sách lựa chọn
        /// </summary>
        public List<QuestionOptionDto> Options { get; set; } = new List<QuestionOptionDto>();
    }

}
