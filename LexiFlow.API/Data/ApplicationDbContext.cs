using LexiFlow.API.Models;
using LexiFlow.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace LexiFlow.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        #region User Management Entities
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<UserTeam> UserTeams { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<Group> Groups { get; set; }
        #endregion

        #region Vocabulary Management Entities
        public DbSet<Category> Categories { get; set; }
        public DbSet<VocabularyGroup> VocabularyGroups { get; set; }
        public DbSet<Vocabulary> Vocabulary { get; set; }
        public DbSet<VocabularyCategory> VocabularyCategories { get; set; }
        public DbSet<Kanji> Kanji { get; set; }
        public DbSet<KanjiVocabulary> KanjiVocabulary { get; set; }
        public DbSet<Grammar> Grammar { get; set; }
        public DbSet<GrammarExample> GrammarExamples { get; set; }
        public DbSet<TechnicalTerm> TechnicalTerms { get; set; }
        public DbSet<UserPersonalVocabulary> UserPersonalVocabulary { get; set; }
        public DbSet<KanjiComponent> KanjiComponents { get; set; }
        public DbSet<KanjiComponentMapping> KanjiComponentMappings { get; set; }
        #endregion

        #region Learning Progress Entities
        public DbSet<LearningProgress> LearningProgress { get; set; }
        public DbSet<UserKanjiProgress> UserKanjiProgress { get; set; }
        public DbSet<UserGrammarProgress> UserGrammarProgress { get; set; }
        public DbSet<PersonalWordList> PersonalWordLists { get; set; }
        public DbSet<PersonalWordListItem> PersonalWordListItems { get; set; }
        #endregion

        #region Synchronization Entities
        public DbSet<SyncMetadata> SyncMetadata { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User Management Entities
            ConfigureUserEntities(modelBuilder);
            ConfigureRoleAndPermissionEntities(modelBuilder);
            ConfigureTeamAndDepartmentEntities(modelBuilder);

            // Configure Vocabulary Management Entities
            ConfigureVocabularyEntities(modelBuilder);
            ConfigureVocabularyGroupEntities(modelBuilder);
            ConfigureCategoryEntities(modelBuilder);
            ConfigureKanjiEntities(modelBuilder);
            ConfigureGrammarEntities(modelBuilder);

            // Configure Learning Progress Entities
            ConfigureLearningProgressEntities(modelBuilder);

            // Configure Synchronization Entities
            ConfigureSyncMetadata(modelBuilder);

            // Add seed data
            SeedData(modelBuilder);
        }

        #region User Management Configuration
        private void ConfigureUserEntities(ModelBuilder modelBuilder)
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
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();

                // Configure relationship with Department
                entity.HasOne(e => e.Department_Navigation)
                    .WithMany(d => d.Users)
                    .HasForeignKey(e => e.DepartmentID)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }

        private void ConfigureRoleAndPermissionEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(e => e.RoleID);
                entity.Property(e => e.RoleName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.RowVersion).IsRowVersion();
                entity.HasIndex(e => e.RoleName).IsUnique();
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRoles");
                entity.HasKey(e => e.UserRoleID);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships with User and Role
                entity.HasOne(e => e.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(e => e.UserID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(e => e.RoleID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("Permissions");
                entity.HasKey(e => e.PermissionID);
                entity.Property(e => e.PermissionName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.Module).HasMaxLength(50);
                entity.Property(e => e.RowVersion).IsRowVersion();
                entity.HasIndex(e => e.PermissionName).IsUnique();
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("RolePermissions");
                entity.HasKey(e => e.RolePermissionID);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.Role)
                    .WithMany(r => r.RolePermissions)
                    .HasForeignKey(e => e.RoleID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(e => e.PermissionID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.ToTable("UserPermissions");
                entity.HasKey(e => e.UserPermissionID);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.User)
                    .WithMany(u => u.UserPermissions)
                    .HasForeignKey(e => e.UserID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Permission)
                    .WithMany(p => p.UserPermissions)
                    .HasForeignKey(e => e.PermissionID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.GrantedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.GrantedByUserID)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }

        private void ConfigureTeamAndDepartmentEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Groups");
                entity.HasKey(e => e.GroupID);
                entity.Property(e => e.GroupName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.RowVersion).IsRowVersion();
                entity.HasIndex(e => e.GroupName);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Departments");
                entity.HasKey(e => e.DepartmentID);
                entity.Property(e => e.DepartmentName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.DepartmentCode).HasMaxLength(20);
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.RowVersion).IsRowVersion();
                entity.HasIndex(e => e.DepartmentName).IsUnique();
                entity.HasIndex(e => e.DepartmentCode).IsUnique();

                // Configure relationships
                entity.HasOne(e => e.ParentDepartment)
                    .WithMany(d => d.ChildDepartments)
                    .HasForeignKey(e => e.ParentDepartmentID)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Manager)
                    .WithMany(u => u.ManagedDepartments)
                    .HasForeignKey(e => e.ManagerUserID)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("Teams");
                entity.HasKey(e => e.TeamID);
                entity.Property(e => e.TeamName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.RowVersion).IsRowVersion();
                entity.HasIndex(e => e.TeamName);

                // Configure relationships
                entity.HasOne(e => e.Department)
                    .WithMany(d => d.Teams)
                    .HasForeignKey(e => e.DepartmentID)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Leader)
                    .WithMany(u => u.LedTeams)
                    .HasForeignKey(e => e.LeaderUserID)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<UserTeam>(entity =>
            {
                entity.ToTable("UserTeams");
                entity.HasKey(e => e.UserTeamID);
                entity.Property(e => e.Role).HasMaxLength(50);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.User)
                    .WithMany(u => u.UserTeams)
                    .HasForeignKey(e => e.UserID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Team)
                    .WithMany(t => t.UserTeams)
                    .HasForeignKey(e => e.TeamID)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
        #endregion

        #region Vocabulary Management Configuration
        private void ConfigureVocabularyEntities(ModelBuilder modelBuilder)
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
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.Group)
                    .WithMany(g => g.Vocabularies)
                    .HasForeignKey(e => e.GroupID)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.CreatedByUser)
                    .WithMany(u => u.CreatedVocabularies)
                    .HasForeignKey(e => e.CreatedByUserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.UpdatedByUser)
                    .WithMany(u => u.UpdatedVocabularies)
                    .HasForeignKey(e => e.UpdatedByUserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.LastModifiedByUser)
                    .WithMany(u => u.LastModifiedVocabularies)
                    .HasForeignKey(e => e.LastModifiedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                // Add indexes for better query performance
                entity.HasIndex(e => e.Level);
                entity.HasIndex(e => e.PartOfSpeech);
                entity.HasIndex(e => e.GroupID);
                entity.HasIndex(e => e.CreatedByUserID);
            });
        }

        private void ConfigureVocabularyGroupEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VocabularyGroup>(entity =>
            {
                entity.ToTable("VocabularyGroups");
                entity.HasKey(e => e.GroupID);
                entity.Property(e => e.GroupName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.VocabularyGroups)
                    .HasForeignKey(e => e.CategoryID)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedByUserID)
                    .OnDelete(DeleteBehavior.SetNull);

                // Add indexes
                entity.HasIndex(e => e.GroupName);
                entity.HasIndex(e => e.CategoryID);
                entity.HasIndex(e => e.CreatedByUserID);
            });
        }

        private void ConfigureCategoryEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(e => e.CategoryID);
                entity.Property(e => e.CategoryName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.Level).HasMaxLength(20);
                entity.Property(e => e.RowVersion).IsRowVersion();
                entity.HasIndex(e => e.CategoryName);
            });

            modelBuilder.Entity<VocabularyCategory>(entity =>
            {
                entity.ToTable("VocabularyCategories");
                entity.HasKey(e => e.CategoryID);
                entity.Property(e => e.CategoryName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationship
                entity.HasOne(e => e.ParentCategory)
                    .WithMany(c => c.ChildCategories)
                    .HasForeignKey(e => e.ParentCategoryID)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasIndex(e => e.CategoryName);
            });
        }

        private void ConfigureKanjiEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kanji>(entity =>
            {
                entity.ToTable("Kanji");
                entity.HasKey(e => e.KanjiID);
                entity.Property(e => e.Character).HasMaxLength(10).IsRequired();
                entity.Property(e => e.Onyomi).HasMaxLength(100);
                entity.Property(e => e.Kunyomi).HasMaxLength(100);
                entity.Property(e => e.Meaning).HasMaxLength(255);
                entity.Property(e => e.JLPTLevel).HasMaxLength(10);
                entity.Property(e => e.Radicals).HasMaxLength(100);
                entity.Property(e => e.Components).HasMaxLength(100);
                entity.Property(e => e.WritingOrderImage).HasMaxLength(255);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationship
                entity.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedByUserID)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(e => e.Character).IsUnique();
                entity.HasIndex(e => e.JLPTLevel);
                entity.HasIndex(e => e.Grade);
            });

            modelBuilder.Entity<KanjiVocabulary>(entity =>
            {
                entity.ToTable("KanjiVocabulary");
                entity.HasKey(e => e.KanjiVocabularyID);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.Kanji)
                    .WithMany(k => k.KanjiVocabularies)
                    .HasForeignKey(e => e.KanjiID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Vocabulary)
                    .WithMany(v => v.KanjiVocabularies)
                    .HasForeignKey(e => e.VocabularyID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<KanjiComponent>(entity =>
            {
                entity.ToTable("KanjiComponents");
                entity.HasKey(e => e.ComponentID);
                entity.Property(e => e.ComponentName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Character).HasMaxLength(10);
                entity.Property(e => e.Meaning).HasMaxLength(255);
                entity.Property(e => e.Type).HasMaxLength(50);
                entity.Property(e => e.Position).HasMaxLength(50);
                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasIndex(e => e.ComponentName).IsUnique();
            });

            modelBuilder.Entity<KanjiComponentMapping>(entity =>
            {
                entity.ToTable("KanjiComponentMappings");
                entity.HasKey(e => e.MappingID);
                entity.Property(e => e.Position).HasMaxLength(50);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.Kanji)
                    .WithMany(k => k.KanjiComponentMappings)
                    .HasForeignKey(e => e.KanjiID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Component)
                    .WithMany(c => c.KanjiComponentMappings)
                    .HasForeignKey(e => e.ComponentID)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureGrammarEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Grammar>(entity =>
            {
                entity.ToTable("Grammar");
                entity.HasKey(e => e.GrammarID);
                entity.Property(e => e.GrammarPoint).HasMaxLength(100).IsRequired();
                entity.Property(e => e.JLPTLevel).HasMaxLength(10);
                entity.Property(e => e.Pattern).HasMaxLength(255);
                entity.Property(e => e.RelatedGrammar).HasMaxLength(255);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Grammars)
                    .HasForeignKey(e => e.CategoryID)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedByUserID)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(e => e.GrammarPoint);
                entity.HasIndex(e => e.JLPTLevel);
            });

            modelBuilder.Entity<GrammarExample>(entity =>
            {
                entity.ToTable("GrammarExamples");
                entity.HasKey(e => e.ExampleID);
                entity.Property(e => e.Context).HasMaxLength(255);
                entity.Property(e => e.AudioFile).HasMaxLength(255);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationship
                entity.HasOne(e => e.Grammar)
                    .WithMany(g => g.GrammarExamples)
                    .HasForeignKey(e => e.GrammarID)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
        #endregion

        #region Learning Progress Configuration
        private void ConfigureLearningProgressEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserKanjiProgress>(entity =>
            {
                entity.ToTable("UserKanjiProgress");
                entity.HasKey(e => e.ProgressID);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.User)
                    .WithMany(u => u.KanjiProgresses)
                    .HasForeignKey(e => e.UserID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Kanji)
                    .WithMany(k => k.UserKanjiProgresses)
                    .HasForeignKey(e => e.KanjiID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.UserID, e.KanjiID }).IsUnique();
            });

            modelBuilder.Entity<UserGrammarProgress>(entity =>
            {
                entity.ToTable("UserGrammarProgress");
                entity.HasKey(e => e.ProgressID);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.User)
                    .WithMany(u => u.GrammarProgresses)
                    .HasForeignKey(e => e.UserID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Grammar)
                    .WithMany(g => g.UserGrammarProgresses)
                    .HasForeignKey(e => e.GrammarID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.UserID, e.GrammarID }).IsUnique();
            });

            modelBuilder.Entity<UserPersonalVocabulary>(entity =>
            {
                entity.ToTable("UserPersonalVocabulary");
                entity.HasKey(e => e.PersonalVocabID);
                entity.Property(e => e.Japanese).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Kana).HasMaxLength(100);
                entity.Property(e => e.Romaji).HasMaxLength(100);
                entity.Property(e => e.Vietnamese).HasMaxLength(255);
                entity.Property(e => e.English).HasMaxLength(255);
                entity.Property(e => e.Source).HasMaxLength(255);
                entity.Property(e => e.Tags).HasMaxLength(255);
                entity.Property(e => e.ImagePath).HasMaxLength(255);
                entity.Property(e => e.AudioPath).HasMaxLength(255);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationship
                entity.HasOne(e => e.User)
                    .WithMany(u => u.PersonalVocabularies)
                    .HasForeignKey(e => e.UserID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.UserID, e.Japanese });
                entity.HasIndex(e => e.Importance);
            });

            modelBuilder.Entity<PersonalWordList>(entity =>
            {
                entity.ToTable("PersonalWordLists");
                entity.HasKey(e => e.ListID);
                entity.Property(e => e.ListName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationship
                entity.HasOne(e => e.User)
                    .WithMany(u => u.PersonalWordLists)
                    .HasForeignKey(e => e.UserID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PersonalWordListItem>(entity =>
            {
                entity.ToTable("PersonalWordListItems");
                entity.HasKey(e => e.ItemID);

                // Chỉ cấu hình các thuộc tính thực tế tồn tại
                entity.Property(e => e.AddedAt).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Mối quan hệ với PersonalWordList
                entity.HasOne(e => e.List)  // List là tên chính xác, không phải WordList
                    .WithMany(l => l.Items)
                    .HasForeignKey(e => e.ListID)
                    .OnDelete(DeleteBehavior.Cascade);

                // Mối quan hệ với Vocabulary
                entity.HasOne(e => e.Vocabulary)
                    .WithMany(v => v.PersonalWordListItems)  // PersonalWordListItems tồn tại trong class Vocabulary
                    .HasForeignKey(e => e.VocabularyID)
                    .OnDelete(DeleteBehavior.Cascade);

                // Thêm các index để tối ưu hiệu suất
                entity.HasIndex(e => e.ListID);
                entity.HasIndex(e => e.VocabularyID);
                entity.HasIndex(e => new { e.ListID, e.VocabularyID }).IsUnique();
            });
        }
        #endregion

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
                    .HasForeignKey(e => e.UserID)
                    .OnDelete(DeleteBehavior.Cascade);
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
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );

            // Seed roles
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    RoleID = 1,
                    RoleName = "Administrator",
                    Description = "System administrator with full access",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Role
                {
                    RoleID = 2,
                    RoleName = "Teacher",
                    Description = "Can create and manage content",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Role
                {
                    RoleID = 3,
                    RoleName = "Student",
                    Description = "Regular user learning Japanese",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );

            // Assign admin to Administrator role
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole
                {
                    UserRoleID = 1,
                    UserID = 1,
                    RoleID = 1,
                    AssignedAt = DateTime.UtcNow,
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
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new VocabularyGroup
                {
                    GroupID = 2,
                    GroupName = "N4 Intermediate Vocabulary",
                    Description = "Intermediate vocabulary for JLPT N4 level",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new VocabularyGroup
                {
                    GroupID = 3,
                    GroupName = "Common Expressions",
                    Description = "Everyday Japanese expressions",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );

            // Seed basic categories
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryID = 1,
                    CategoryName = "JLPT",
                    Description = "Japanese Language Proficiency Test levels",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Category
                {
                    CategoryID = 2,
                    CategoryName = "Everyday Life",
                    Description = "Vocabulary for daily situations",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Category
                {
                    CategoryID = 3,
                    CategoryName = "Business",
                    Description = "Business and workplace vocabulary",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
    }
}