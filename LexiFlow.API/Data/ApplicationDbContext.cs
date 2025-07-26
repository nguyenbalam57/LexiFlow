using LexiFlow.API.Models;
using LexiFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // User related entities
        public DbSet<User> Users { get; set; }

        // Vocabulary related entities
        public DbSet<Vocabulary> Vocabulary { get; set; }
        public DbSet<VocabularyGroup> VocabularyGroups { get; set; }

        // Synchronization related entities
        public DbSet<SyncMetadata> SyncMetadata { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            ConfigureUsers(modelBuilder);

            // Configure Vocabulary entity
            ConfigureVocabulary(modelBuilder);

            // Configure VocabularyGroup entity
            ConfigureVocabularyGroups(modelBuilder);

            // Configure SyncMetadata entity
            ConfigureSyncMetadata(modelBuilder);

            // Add seed data
            SeedData(modelBuilder);
        }

        private void ConfigureUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.UserID);
                entity.Property(e => e.Username).HasMaxLength(50).IsRequired();
                entity.Property(e => e.PasswordHash).HasMaxLength(255).IsRequired();
                entity.Property(e => e.FullName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Position).HasMaxLength(100);
                entity.Property(e => e.Department).HasMaxLength(100);
                entity.Property(e => e.Role).HasMaxLength(50);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.RowVersion).IsRowVersion();
            });
        }

        private void ConfigureVocabulary(ModelBuilder modelBuilder)
        {
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
                entity.Property(e => e.AudioFile).HasMaxLength(255);
                entity.Property(e => e.Version).HasMaxLength(50);
                entity.Property(e => e.SyncStatus).HasMaxLength(20);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Set foreign keys
                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.CreatedByUserID);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UpdatedByUserID)
                    .IsRequired(false);

                entity.HasOne<VocabularyGroup>()
                    .WithMany(g => g.Vocabularies)
                    .HasForeignKey(e => e.GroupID)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);

                // Add indexes for better performance
                entity.HasIndex(e => e.Level);
                entity.HasIndex(e => e.PartOfSpeech);
                entity.HasIndex(e => e.GroupID);
                entity.HasIndex(e => e.CreatedByUserID);
            });
        }

        private void ConfigureVocabularyGroups(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VocabularyGroup>(entity =>
            {
                entity.ToTable("VocabularyGroups");
                entity.HasKey(e => e.GroupID);
                entity.Property(e => e.GroupName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);

                // Set foreign keys
                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.CreatedByUserID)
                    .IsRequired(false);

                // Add indexes
                entity.HasIndex(e => e.GroupName);
                entity.HasIndex(e => e.CategoryID);
                entity.HasIndex(e => e.CreatedByUserID);
            });
        }

        private void ConfigureSyncMetadata(ModelBuilder modelBuilder)
        {
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
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed admin user - in production, use a proper password hashing mechanism
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserID = 1,
                    Username = "admin",
                    PasswordHash = "AQAAAAIAAYagAAAAEGxTLQQa/1iCzdkB8dHtHNNEcfzGwH2TYP7Hxi3LuYn+JpO3V5UdqbXH4nCiUmb3+w==", // hashed "Admin@123"
                    FullName = "System Administrator",
                    Email = "admin@lexiflow.app",
                    Role = "Administrator",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );

            // Seed basic vocabulary groups
            modelBuilder.Entity<VocabularyGroup>().HasData(
                new VocabularyGroup
                {
                    GroupID = 1,
                    GroupName = "N5 Basic Vocabulary",
                    Description = "Basic vocabulary for JLPT N5 level",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new VocabularyGroup
                {
                    GroupID = 2,
                    GroupName = "N4 Intermediate Vocabulary",
                    Description = "Intermediate vocabulary for JLPT N4 level",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new VocabularyGroup
                {
                    GroupID = 3,
                    GroupName = "Common Expressions",
                    Description = "Everyday Japanese expressions",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}