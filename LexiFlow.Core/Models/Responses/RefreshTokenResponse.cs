using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Core.Models.Responses
{
    /// <summary>
    /// Phản hồi làm mới token
    /// </summary>
    public class RefreshTokenResponse
    {
        /// <summary>
        /// Token mới
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Thời gian hết hạn
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }
}
