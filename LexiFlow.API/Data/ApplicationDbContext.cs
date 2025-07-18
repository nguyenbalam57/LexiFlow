using LexiFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Vocabulary> Vocabulary { get; set; }
        public DbSet<SyncMetadata> SyncMetadata { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.UserID);
                entity.Property(e => e.Username).HasMaxLength(50).IsRequired();
                entity.Property(e => e.PasswordHash).HasMaxLength(255).IsRequired();
                entity.Property(e => e.FullName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.Role).HasMaxLength(50);
                entity.HasIndex(e => e.Username).IsUnique();
            });

            // Configure Vocabulary entity
            modelBuilder.Entity<Vocabulary>(entity =>
            {
                entity.ToTable("Vocabulary");
                entity.HasKey(e => e.VocabularyID);
                entity.Property(e => e.Japanese).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Kana).HasMaxLength(100);
                entity.Property(e => e.Romaji).HasMaxLength(100);
                entity.Property(e => e.Vietnamese).HasMaxLength(255);
                entity.Property(e => e.English).HasMaxLength(255);
                entity.Property(e => e.Example).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Notes).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Level).HasMaxLength(20);
                entity.Property(e => e.PartOfSpeech).HasMaxLength(50);

                // Set foreign keys
                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.CreatedByUserID);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UpdatedByUserID)
                    .IsRequired(false);
            });

            // Configure SyncMetadata entity
            modelBuilder.Entity<SyncMetadata>(entity =>
            {
                entity.ToTable("SyncMetadata");
                entity.HasKey(e => new { e.UserID, e.TableName });
                entity.Property(e => e.TableName).HasMaxLength(100).IsRequired();

                // Set foreign key
                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UserID);
            });

            // Add seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed admin user - in production, use a proper password hashing mechanism
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserID = 1,
                    Username = "admin",
                    // This is just a placeholder hash - in production, use a proper password hashing mechanism
                    PasswordHash = "AQAAAAEAACcQAAAAELGAeQ+wfIZ+JX+m4BhJBwLQVXSuTXzfza9xg8ZPcx3r98U0BZiJ1zMF+oLAE7Y7iQ==", // Password: Admin@123
                    FullName = "System Administrator",
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );

            // Seed sample vocabulary
            modelBuilder.Entity<Vocabulary>().HasData(
                new Vocabulary
                {
                    VocabularyID = 1,
                    Japanese = "こんにちは",
                    Kana = "こんにちは",
                    Romaji = "konnichiwa",
                    Vietnamese = "Xin chào",
                    English = "Hello",
                    Example = "こんにちは、元気ですか？",
                    Level = "N5",
                    CreatedByUserID = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new Vocabulary
                {
                    VocabularyID = 2,
                    Japanese = "ありがとう",
                    Kana = "ありがとう",
                    Romaji = "arigatou",
                    Vietnamese = "Cảm ơn",
                    English = "Thank you",
                    Example = "ありがとう、助かりました。",
                    Level = "N5",
                    CreatedByUserID = 1,
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
