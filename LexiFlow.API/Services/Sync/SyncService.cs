using LexiFlow.API.Interfaces.Sync;
using LexiFlow.Models;

using LexiFlow.API.DTOs.Sync;
using LexiFlow.API.DTOs.Auth;
using Microsoft.EntityFrameworkCore;
using LexiFlow.Models.Learning.Vocabulary;
using LexiFlow.Models.Learning.Kanji;
using LexiFlow.Models.Learning.Grammar;
using LexiFlow.Models.Progress;
using LexiFlow.Models.Sync;

namespace LexiFlow.API.Services.Sync
{
    /// <summary>
    /// Dịch vụ đồng bộ dữ liệu giữa client và server - TEMPORARILY DISABLED
    /// </summary>
    public class SyncServiceDisabled // : ISyncService - Temporarily disabled due to namespace conflicts
    {
        // Implementation temporarily disabled to fix build issues
        // Will be re-enabled after fixing namespace conflicts with Vocabulary
    }
}