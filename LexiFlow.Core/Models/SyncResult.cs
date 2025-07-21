using LexiFlow.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Core.Models
{
    /// <summary>
    /// Kết quả đồng bộ
    /// </summary>
    public class SyncResult
    {
        public DateTime SyncedAt { get; set; }
        public List<Vocabulary> UpdatedItems { get; set; } = new List<Vocabulary>();
        public List<int> ProcessedDeletedIds { get; set; } = new List<int>();
        public int TotalUpdated { get; set; }
        public int TotalDeleted { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
