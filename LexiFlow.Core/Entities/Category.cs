using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.Core.Entities
{
    /// <summary>
    /// Represents a category for organizing vocabulary items
    /// </summary>
    public class Category : BaseEntity
    {
        /// <summary>
        /// Category name
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description of the category
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Parent category ID (for hierarchical categories)
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Navigation property for parent category
        /// </summary>
        [ForeignKey("ParentId")]
        public virtual Category? Parent { get; set; }

        /// <summary>
        /// Navigation property for child categories
        /// </summary>
        public virtual ICollection<Category> Children { get; set; } = new List<Category>();

        /// <summary>
        /// Navigation property for vocabulary items in this category
        /// </summary>
        public virtual ICollection<VocabularyItem> VocabularyItems { get; set; } = new List<VocabularyItem>();

        /// <summary>
        /// Icon or image URL for this category
        /// </summary>
        [StringLength(255)]
        public string? IconUrl { get; set; }

        /// <summary>
        /// Color code for UI display (hex format)
        /// </summary>
        [StringLength(10)]
        public string? ColorCode { get; set; }

        /// <summary>
        /// Display order for sorting
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Language code this category is associated with
        /// </summary>
        [StringLength(10)]
        public string? LanguageCode { get; set; }

        /// <summary>
        /// Status of the category (Active, Archived)
        /// </summary>
        [StringLength(20)]
        public string Status { get; set; } = "Active";

        /// <summary>
        /// Flag for system categories that cannot be deleted
        /// </summary>
        public bool IsSystemCategory { get; set; } = false;

        /// <summary>
        /// SEO-friendly URL slug
        /// </summary>
        [StringLength(150)]
        public string? Slug { get; set; }

        /// <summary>
        /// Flag for featured categories
        /// </summary>
        public bool IsFeatured { get; set; } = false;

        /// <summary>
        /// View count
        /// </summary>
        public int ViewCount { get; set; } = 0;

        /// <summary>
        /// Tags for this category (comma-separated)
        /// </summary>
        [StringLength(255)]
        public string? Tags { get; set; }

        /// <summary>
        /// Number of vocabulary items in this category
        /// This is a denormalized field for performance reasons
        /// </summary>
        public int ItemCount { get; set; } = 0;

        /// <summary>
        /// Gets the full hierarchical path of this category
        /// </summary>
        [NotMapped]
        public string FullPath
        {
            get
            {
                if (Parent == null)
                {
                    return Name;
                }

                return $"{Parent.FullPath} > {Name}";
            }
        }
    }
}