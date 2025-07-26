using LexiFlow.API.DTOs.TechnicalTerm;
using LexiFlow.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LexiFlow.API.Services
{
    public interface ITechnicalTermService
    {
        Task<(IEnumerable<TechnicalTerm> Items, int TotalCount, int Page, int PageSize, int TotalPages)> GetTechnicalTermsAsync(
            int page = 1, int pageSize = 50, string field = null, string department = null);

        Task<TechnicalTerm> GetByIdAsync(int id);

        Task<TechnicalTerm> CreateAsync(CreateTechnicalTermDto dto, int userId);

        Task<TechnicalTerm> UpdateAsync(int id, UpdateTechnicalTermDto dto, int userId);

        Task<bool> DeleteAsync(int id, int userId);

        Task<IEnumerable<TechnicalTerm>> SearchAsync(TechnicalTermSearchRequestDto searchRequest);

        Task<UserTechnicalTerm> GetUserTermAsync(int userId, int termId);

        Task<IEnumerable<UserTechnicalTerm>> GetUserTermsAsync(int userId);

        Task<UserTechnicalTerm> UpdateUserTermAsync(int userId, int termId, UpdateUserTechnicalTermDto dto);

        Task<TechnicalTermStatsDto> GetStatsAsync();
    }
}