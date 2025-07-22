using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LexiFlow.Core.Entities
{
    /// <summary>
    /// Base entity class for all domain entities
    /// Implements common properties and concurrency management
    /// </summary>
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Last modification timestamp
        /// </summary>
        public DateTime? ModifiedAt { get; set; }

        /// <summary>
        /// User ID who created this entity
        /// </summary>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// User ID who last modified this entity
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Deletion timestamp
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// User ID who deleted this entity
        /// </summary>
        public int? DeletedBy { get; set; }

        /// <summary>
        /// RowVersion for optimistic concurrency control
        /// This will be automatically updated by the database on each update
        /// </summary>
        [Timestamp]
        [ConcurrencyCheck]
        public byte[] RowVersion { get; set; } = new byte[8];

        /// <summary>
        /// Optional string representation of RowVersion for API communication
        /// </summary>
        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? RowVersionString
        {
            get => RowVersion != null ? Convert.ToBase64String(RowVersion) : null;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    RowVersion = Convert.FromBase64String(value);
                }
            }
        }

        /// <summary>
        /// Sets the creation metadata
        /// </summary>
        public void SetCreationInfo(int userId)
        {
            CreatedAt = DateTime.UtcNow;
            CreatedBy = userId;
        }

        /// <summary>
        /// Sets the modification metadata
        /// </summary>
        public void SetModificationInfo(int userId)
        {
            ModifiedAt = DateTime.UtcNow;
            ModifiedBy = userId;
        }

        /// <summary>
        /// Sets the deletion metadata
        /// </summary>
        public void SetDeletionInfo(int userId)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = userId;
        }
    }
}