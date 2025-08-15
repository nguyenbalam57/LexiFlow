using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LexiFlow.API.DTOs.Common;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.Controllers
{
    /// <summary>
    /// Controller qu?n lý file media (audio, image, video)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MediaController : ControllerBase
    {
        private readonly ILogger<MediaController> _logger;

        public MediaController(ILogger<MediaController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Upload file media
        /// </summary>
        [HttpPost("upload")]
        public async Task<ActionResult<MediaUploadResponseDto>> UploadMedia(
            [FromForm] IFormFile file,
            [FromForm] string mediaType,
            [FromForm] string? entityType = null,
            [FromForm] int? entityId = null,
            [FromForm] string? description = null)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded");
                }

                var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "audio/mpeg", "audio/wav", "video/mp4" };
                if (!allowedTypes.Contains(file.ContentType))
                {
                    return BadRequest("Unsupported file type");
                }

                const long maxFileSize = 10 * 1024 * 1024; // 10MB
                if (file.Length > maxFileSize)
                {
                    return BadRequest("File size exceeds limit");
                }

                var userId = GetCurrentUserId();
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                
                // TODO: Implement actual file upload to storage
                
                var mediaFile = new MediaFileDto
                {
                    MediaId = new Random().Next(1000, 9999),
                    FileName = fileName,
                    OriginalFileName = file.FileName,
                    MediaType = mediaType,
                    ContentType = file.ContentType,
                    FileSize = file.Length,
                    FilePath = $"/uploads/{mediaType}/{fileName}",
                    PublicUrl = $"https://cdn.lexiflow.com/uploads/{mediaType}/{fileName}",
                    EntityType = entityType,
                    EntityId = entityId,
                    Description = description,
                    UserId = userId,
                    IsProcessed = false,
                    IsPublic = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var response = new MediaUploadResponseDto
                {
                    Success = true,
                    Message = "File uploaded successfully",
                    MediaFile = mediaFile,
                    ProcessingRequired = mediaType == "Audio" || mediaType == "Video"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading media file");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y danh sách file media
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PaginatedResultDto<MediaFileDto>>> GetMediaFiles(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? mediaType = null,
            [FromQuery] string? entityType = null,
            [FromQuery] int? entityId = null,
            [FromQuery] bool? isPublic = null,
            [FromQuery] bool? isProcessed = null,
            [FromQuery] string? sortBy = "CreatedAt",
            [FromQuery] string? sortDirection = "desc")
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var mediaFiles = new List<MediaFileDto>
                {
                    new MediaFileDto
                    {
                        MediaId = 1,
                        FileName = "konnichiwa_audio.mp3",
                        OriginalFileName = "konnichiwa.mp3",
                        MediaType = "Audio",
                        ContentType = "audio/mpeg",
                        FileSize = 245760,
                        FilePath = "/uploads/audio/konnichiwa_audio.mp3",
                        PublicUrl = "https://cdn.lexiflow.com/uploads/audio/konnichiwa_audio.mp3",
                        EntityType = "Vocabulary",
                        EntityId = 1,
                        Description = "Audio pronunciation for ?????",
                        UserId = userId,
                        IsProcessed = true,
                        IsPublic = true,
                        Duration = 2.5f,
                        CreatedAt = DateTime.UtcNow.AddDays(-5),
                        UpdatedAt = DateTime.UtcNow.AddDays(-5)
                    },
                    new MediaFileDto
                    {
                        MediaId = 2,
                        FileName = "greeting_image.jpg",
                        OriginalFileName = "greeting.jpg",
                        MediaType = "Image",
                        ContentType = "image/jpeg",
                        FileSize = 156420,
                        FilePath = "/uploads/images/greeting_image.jpg",
                        PublicUrl = "https://cdn.lexiflow.com/uploads/images/greeting_image.jpg",
                        EntityType = "Vocabulary",
                        EntityId = 1,
                        Description = "Visual illustration for greeting vocabulary",
                        UserId = userId,
                        IsProcessed = true,
                        IsPublic = true,
                        Width = 800,
                        Height = 600,
                        CreatedAt = DateTime.UtcNow.AddDays(-3),
                        UpdatedAt = DateTime.UtcNow.AddDays(-3)
                    }
                };

                // Apply filters
                if (!string.IsNullOrEmpty(mediaType))
                {
                    mediaFiles = mediaFiles.Where(mf => mf.MediaType.Equals(mediaType, StringComparison.OrdinalIgnoreCase))
                                          .ToList();
                }

                if (!string.IsNullOrEmpty(entityType))
                {
                    mediaFiles = mediaFiles.Where(mf => mf.EntityType?.Equals(entityType, StringComparison.OrdinalIgnoreCase) == true)
                                          .ToList();
                }

                if (entityId.HasValue)
                {
                    mediaFiles = mediaFiles.Where(mf => mf.EntityId == entityId.Value).ToList();
                }

                if (isPublic.HasValue)
                {
                    mediaFiles = mediaFiles.Where(mf => mf.IsPublic == isPublic.Value).ToList();
                }

                if (isProcessed.HasValue)
                {
                    mediaFiles = mediaFiles.Where(mf => mf.IsProcessed == isProcessed.Value).ToList();
                }

                // Apply sorting
                mediaFiles = sortBy?.ToLower() switch
                {
                    "filename" => sortDirection?.ToLower() == "desc"
                        ? mediaFiles.OrderByDescending(mf => mf.FileName).ToList()
                        : mediaFiles.OrderBy(mf => mf.FileName).ToList(),
                    "filesize" => sortDirection?.ToLower() == "desc"
                        ? mediaFiles.OrderByDescending(mf => mf.FileSize).ToList()
                        : mediaFiles.OrderBy(mf => mf.FileSize).ToList(),
                    "mediatype" => sortDirection?.ToLower() == "desc"
                        ? mediaFiles.OrderByDescending(mf => mf.MediaType).ToList()
                        : mediaFiles.OrderBy(mf => mf.MediaType).ToList(),
                    _ => sortDirection?.ToLower() == "desc"
                        ? mediaFiles.OrderByDescending(mf => mf.CreatedAt).ToList()
                        : mediaFiles.OrderBy(mf => mf.CreatedAt).ToList()
                };

                var totalCount = mediaFiles.Count;
                var items = mediaFiles.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var result = new PaginatedResultDto<MediaFileDto>
                {
                    Data = items,
                    TotalCount = totalCount,
                    PageNumber = page,
                    PageSize = pageSize
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting media files");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y chi ti?t file media
        /// </summary>
        [HttpGet("{mediaId}")]
        public async Task<ActionResult<MediaFileDto>> GetMediaFile(int mediaId)
        {
            try
            {
                var mediaFile = new MediaFileDto
                {
                    MediaId = mediaId,
                    FileName = "konnichiwa_audio.mp3",
                    OriginalFileName = "konnichiwa.mp3",
                    MediaType = "Audio",
                    ContentType = "audio/mpeg",
                    FileSize = 245760,
                    FilePath = "/uploads/audio/konnichiwa_audio.mp3",
                    PublicUrl = "https://cdn.lexiflow.com/uploads/audio/konnichiwa_audio.mp3",
                    EntityType = "Vocabulary",
                    EntityId = 1,
                    Description = "Audio pronunciation for ?????",
                    UserId = GetCurrentUserId(),
                    IsProcessed = true,
                    IsPublic = true,
                    Duration = 2.5f,
                    Metadata = new MediaMetadataDto
                    {
                        Bitrate = 128,
                        SampleRate = 44100,
                        Channels = 2,
                        Codec = "MP3"
                    },
                    Variants = new List<MediaVariantDto>
                    {
                        new MediaVariantDto
                        {
                            Quality = "High",
                            Url = "https://cdn.lexiflow.com/uploads/audio/konnichiwa_audio.mp3",
                            FileSize = 245760
                        },
                        new MediaVariantDto
                        {
                            Quality = "Medium",
                            Url = "https://cdn.lexiflow.com/uploads/audio/konnichiwa_audio_medium.mp3",
                            FileSize = 156480
                        }
                    },
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UpdatedAt = DateTime.UtcNow.AddDays(-5)
                };

                return Ok(mediaFile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting media file {MediaId}", mediaId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// C?p nh?t thông tin file media
        /// </summary>
        [HttpPut("{mediaId}")]
        public async Task<ActionResult<MediaFileDto>> UpdateMediaFile(int mediaId, [FromBody] UpdateMediaFileDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // TODO: Implement actual media file update
                
                var updatedMediaFile = new MediaFileDto
                {
                    MediaId = mediaId,
                    FileName = updateDto.FileName ?? "updated_file.mp3",
                    Description = updateDto.Description ?? "Updated description",
                    IsPublic = updateDto.IsPublic ?? false,
                    UpdatedAt = DateTime.UtcNow
                };

                return Ok(updatedMediaFile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating media file {MediaId}", mediaId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Xóa file media
        /// </summary>
        [HttpDelete("{mediaId}")]
        public async Task<ActionResult> DeleteMediaFile(int mediaId)
        {
            try
            {
                // TODO: Implement actual file deletion from storage and database
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting media file {MediaId}", mediaId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// X? lý file media (resize image, convert audio, etc.)
        /// </summary>
        [HttpPost("{mediaId}/process")]
        public async Task<ActionResult<object>> ProcessMediaFile(int mediaId, [FromBody] ProcessMediaRequestDto processDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // TODO: Implement actual media processing
                
                var result = new
                {
                    Success = true,
                    Message = "Media processing started",
                    MediaId = mediaId,
                    ProcessingType = processDto.ProcessingType,
                    JobId = Guid.NewGuid().ToString(),
                    EstimatedTime = "2-5 minutes",
                    Status = "Processing"
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing media file {MediaId}", mediaId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// T?o thumbnail cho image/video
        /// </summary>
        [HttpPost("{mediaId}/thumbnail")]
        public async Task<ActionResult<object>> GenerateThumbnail(int mediaId, [FromBody] ThumbnailRequestDto thumbnailDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // TODO: Implement actual thumbnail generation
                
                var result = new
                {
                    Success = true,
                    Message = "Thumbnail generated successfully",
                    MediaId = mediaId,
                    ThumbnailUrl = $"https://cdn.lexiflow.com/thumbnails/{mediaId}_thumb.jpg",
                    Width = thumbnailDto.Width,
                    Height = thumbnailDto.Height,
                    GeneratedAt = DateTime.UtcNow
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating thumbnail for media {MediaId}", mediaId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y th?ng kê media
        /// </summary>
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetMediaStatistics()
        {
            try
            {
                var statistics = new
                {
                    TotalFiles = 1250,
                    ByType = new
                    {
                        Audio = 650,
                        Image = 450,
                        Video = 150
                    },
                    TotalSize = "5.2 GB",
                    BySize = new
                    {
                        SmallFiles = 800,  // < 1MB
                        MediumFiles = 350, // 1-10MB
                        LargeFiles = 100   // > 10MB
                    },
                    ProcessingStatus = new
                    {
                        Processed = 1100,
                        Processing = 25,
                        Failed = 15,
                        Pending = 110
                    },
                    PublicFiles = 890,
                    PrivateFiles = 360,
                    RecentUploads = new[]
                    {
                        new { Date = DateTime.UtcNow.Date, Count = 15 },
                        new { Date = DateTime.UtcNow.Date.AddDays(-1), Count = 22 },
                        new { Date = DateTime.UtcNow.Date.AddDays(-2), Count = 18 }
                    }
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting media statistics");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Batch upload nhi?u file
        /// </summary>
        [HttpPost("batch-upload")]
        public async Task<ActionResult<object>> BatchUpload([FromForm] IFormFileCollection files, [FromForm] string mediaType)
        {
            try
            {
                if (files == null || files.Count == 0)
                {
                    return BadRequest("No files uploaded");
                }

                var results = new List<object>();
                var errors = new List<string>();

                foreach (var file in files)
                {
                    try
                    {
                        // TODO: Process each file
                        results.Add(new
                        {
                            FileName = file.FileName,
                            Success = true,
                            MediaId = new Random().Next(1000, 9999),
                            Url = $"https://cdn.lexiflow.com/uploads/{mediaType}/{file.FileName}"
                        });
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"Failed to upload {file.FileName}: {ex.Message}");
                    }
                }

                var response = new
                {
                    TotalFiles = files.Count,
                    SuccessfulUploads = results.Count,
                    FailedUploads = errors.Count,
                    Results = results,
                    Errors = errors,
                    UploadedAt = DateTime.UtcNow
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in batch upload");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// T?i ?u hóa storage
        /// </summary>
        [HttpPost("optimize")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<object>> OptimizeStorage([FromBody] OptimizeStorageDto optimizeDto)
        {
            try
            {
                // TODO: Implement storage optimization
                
                var result = new
                {
                    Success = true,
                    Message = "Storage optimization completed",
                    FilesProcessed = 1250,
                    SpaceSaved = "450 MB",
                    DuplicatesRemoved = 15,
                    UnusedFilesRemoved = 8,
                    OptimizedAt = DateTime.UtcNow
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error optimizing storage");
                return StatusCode(500, "Internal server error");
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return 1;
        }
    }

    #region DTOs

    public class MediaFileDto
    {
        public int MediaId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string MediaType { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string PublicUrl { get; set; } = string.Empty;
        public string? EntityType { get; set; }
        public int? EntityId { get; set; }
        public string? Description { get; set; }
        public int UserId { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsPublic { get; set; }
        public float? Duration { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public MediaMetadataDto? Metadata { get; set; }
        public List<MediaVariantDto>? Variants { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class MediaMetadataDto
    {
        public int? Bitrate { get; set; }
        public int? SampleRate { get; set; }
        public int? Channels { get; set; }
        public string? Codec { get; set; }
        public string? Format { get; set; }
        public Dictionary<string, object>? AdditionalData { get; set; }
    }

    public class MediaVariantDto
    {
        public string Quality { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? Bitrate { get; set; }
    }

    public class MediaUploadResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public MediaFileDto? MediaFile { get; set; }
        public bool ProcessingRequired { get; set; }
    }

    public class UpdateMediaFileDto
    {
        [StringLength(255)]
        public string? FileName { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public bool? IsPublic { get; set; }

        public string? EntityType { get; set; }

        public int? EntityId { get; set; }
    }

    public class ProcessMediaRequestDto
    {
        [Required]
        public string ProcessingType { get; set; } = string.Empty; // Resize, Convert, Compress, etc.

        public Dictionary<string, object>? Parameters { get; set; }
    }

    public class ThumbnailRequestDto
    {
        [Range(50, 1000)]
        public int Width { get; set; } = 150;

        [Range(50, 1000)]
        public int Height { get; set; } = 150;

        public string Quality { get; set; } = "Medium"; // Low, Medium, High
    }

    public class OptimizeStorageDto
    {
        public bool RemoveDuplicates { get; set; } = true;
        public bool RemoveUnusedFiles { get; set; } = true;
        public bool CompressImages { get; set; } = true;
        public bool GenerateThumbnails { get; set; } = true;
        public int? OlderThanDays { get; set; }
    }

    #endregion
}