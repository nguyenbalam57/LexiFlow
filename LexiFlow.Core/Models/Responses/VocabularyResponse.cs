using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LexiFlow.Core.Entities;

namespace LexiFlow.Core.Models.Responses
{
    /// <summary>
    /// Phản hồi cho thao tác với từ vựng
    /// </summary>
    public class VocabularyResponse
    {
        /// <summary>
        /// Trạng thái thành công
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Thông báo
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Dữ liệu từ vựng
        /// </summary>
        public Vocabulary Data { get; set; }
    }
}
