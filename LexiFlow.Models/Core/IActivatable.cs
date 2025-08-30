using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Core
{
    /// <summary>
    /// Interface hỗ trợ kích hoạt/vô hiệu hóa
    /// </summary>
    public interface IActivatable
    {
        bool IsActive { get; set; }
        void Activate();
        void Deactivate();
    }
}
