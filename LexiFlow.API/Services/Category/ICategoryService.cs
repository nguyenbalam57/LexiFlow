using LexiFlow.API.DTOs.Common;
using LexiFlow.API.DTOs.Category;

namespace LexiFlow.API.Services.Category
{
    /// <summary>
    /// Interface cho Category Service
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// L?y danh sách categories có phân trang
        /// </summary>
        Task<PaginatedResultDto<CategoryDto>> GetCategoriesAsync(
            int page = 1, 
            int pageSize = 10, 
            string? searchTerm = null,
            bool? isActive = true);

        /// <summary>
        /// L?y chi ti?t category theo ID
        /// </summary>
        Task<CategoryDto?> GetCategoryByIdAsync(int id);

        /// <summary>
        /// T?o category m?i
        /// </summary>
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createDto, int createdBy);

        /// <summary>
        /// C?p nh?t category
        /// </summary>
        Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto updateDto, int modifiedBy);

        /// <summary>
        /// Xóa category (soft delete)
        /// </summary>
        Task<bool> DeleteCategoryAsync(int id, int deletedBy);

        /// <summary>
        /// L?y t?t c? categories ?ang active (?? dropdown)
        /// </summary>
        Task<IEnumerable<SelectOptionDto>> GetActiveCategoriesAsync();

        /// <summary>
        /// L?y category tree (cha con)
        /// </summary>
        Task<IEnumerable<CategoryTreeDto>> GetCategoryTreeAsync();
    }

    /// <summary>
    /// DTO cho category
    /// </summary>
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ColorCode { get; set; } = string.Empty;
        public int? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO ?? t?o category
    /// </summary>
    public class CreateCategoryDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ColorCode { get; set; } = string.Empty;
        public int? ParentCategoryId { get; set; }
    }

    /// <summary>
    /// DTO ?? c?p nh?t category
    /// </summary>
    public class UpdateCategoryDto
    {
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public string? ColorCode { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// DTO cho category tree
    /// </summary>
    public class CategoryTreeDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ColorCode { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<CategoryTreeDto> Children { get; set; } = new List<CategoryTreeDto>();
    }
}