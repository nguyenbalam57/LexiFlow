using System;
using System.Collections.Generic;

namespace LexiFlow.API.DTOs.Common
{
    /// <summary>
    /// Paginated result DTO
    /// </summary>
    public class PaginatedResultDto<T>
    {
        public List<T> Data { get; set; } = new List<T>();
        public List<T> Items { get => Data; set => Data = value; }
        public int TotalCount { get; set; }
        public int TotalItems { get => TotalCount; set => TotalCount = value; }
        public int PageNumber { get; set; }
        public int Page { get => PageNumber; set => PageNumber = value; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public PaginatedResultDto()
        {

        }

        public PaginatedResultDto(List<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Data = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }

    /// <summary>
    /// Paged response DTO
    /// </summary>
    public class PagedResponseDto<T> 
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageIndex > 0;
        public bool HasNextPage => PageIndex + 1 < TotalPages;
    }
}
