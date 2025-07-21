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
        public int Added { get; set; }
        public int Updated { get; set; }
        public int Deleted { get; set; }
        public int Failed { get; set; }
        public string? LastSyncTime { get; set; }
    }
}
