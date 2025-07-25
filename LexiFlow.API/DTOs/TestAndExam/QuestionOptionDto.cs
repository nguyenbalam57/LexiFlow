﻿namespace LexiFlow.API.DTOs.TestAndExam
{
    /// <summary>
    /// DTO cho lựa chọn câu hỏi
    /// </summary>
    public class QuestionOptionDto
    {
        /// <summary>
        /// ID lựa chọn
        /// </summary>
        public int OptionID { get; set; }

        /// <summary>
        /// ID câu hỏi
        /// </summary>
        public int QuestionID { get; set; }

        /// <summary>
        /// Nội dung lựa chọn
        /// </summary>
        public string OptionText { get; set; }

        /// <summary>
        /// Là đáp án đúng
        /// </summary>
        public bool IsCorrect { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
