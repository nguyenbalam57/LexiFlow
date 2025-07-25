﻿namespace LexiFlow.API.DTOs.UserSubmission
{
    /// <summary>
    /// DTO cho trạng thái đơn nộp
    /// </summary>
    public class SubmissionStatusDto
    {
        /// <summary>
        /// ID trạng thái
        /// </summary>
        public int StatusID { get; set; }

        /// <summary>
        /// Tên trạng thái
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Mã màu
        /// </summary>
        public string ColorCode { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Là trạng thái cuối
        /// </summary>
        public bool IsTerminal { get; set; }

        /// <summary>
        /// Là trạng thái mặc định
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
