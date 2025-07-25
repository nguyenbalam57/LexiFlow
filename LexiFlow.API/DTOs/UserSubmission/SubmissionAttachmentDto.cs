﻿namespace LexiFlow.API.DTOs.UserSubmission
{
    /// <summary>
    /// DTO cho tệp đính kèm đơn nộp
    /// </summary>
    public class SubmissionAttachmentDto
    {
        /// <summary>
        /// ID tệp đính kèm
        /// </summary>
        public int AttachmentID { get; set; }

        /// <summary>
        /// ID đơn nộp
        /// </summary>
        public int SubmissionID { get; set; }

        /// <summary>
        /// Tên tệp
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Đường dẫn tệp
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Kích thước tệp
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Loại tệp
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// Thời gian tải lên
        /// </summary>
        public DateTime UploadedAt { get; set; }
    }
}
