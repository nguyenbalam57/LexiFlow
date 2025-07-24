namespace LexiFlow.API.DTOs
{
    /// <summary>
    /// Base DTO for API responses
    /// </summary>
    public abstract class BaseDto
    {
        /// <summary>
        /// Entity ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last update timestamp
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// Base response for paginated queries
    /// </summary>
    /// <typeparam name="T">DTO type</typeparam>
    public class PagedResponseDto<T> where T : class
    {
        /// <summary>
        /// List of items
        /// </summary>
        public IEnumerable<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Total count of items
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Current page number (0-based)
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Number of items per page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        /// <summary>
        /// Whether there is a previous page
        /// </summary>
        public bool HasPreviousPage => PageIndex > 0;

        /// <summary>
        /// Whether there is a next page
        /// </summary>
        public bool HasNextPage => PageIndex < TotalPages - 1;
    }

    /// <summary>
    /// API Response wrapper
    /// </summary>
    /// <typeparam name="T">Response data type</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Whether the request was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Response message
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Response data
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Error details if any
        /// </summary>
        public List<string>? Errors { get; set; }

        /// <summary>
        /// Create a successful response
        /// </summary>
        /// <param name="data">Response data</param>
        /// <param name="message">Response message</param>
        /// <returns>Successful API response</returns>
        public static ApiResponse<T> CreateSuccess(T data, string message = "Operation completed successfully")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// Create a failed response
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="errors">Detailed errors</param>
        /// <returns>Failed API response</returns>
        public static ApiResponse<T> Fail(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }
    }
}