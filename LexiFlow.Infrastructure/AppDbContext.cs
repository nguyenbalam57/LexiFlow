using LexiFlow.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace LexiFlow.Infrastructure
{
    /// <summary>
    /// Application database context
    /// </summary>
    public class AppDbContext : DbContext
    {
        private readonly ILogger<AppDbContext> _logger;

        #region DbSets

        // User related entities
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }

        // Vocabulary related entities
        public DbSet<VocabularyItem> VocabularyItems { get; set; }
        public DbSet<Definition> Definitions { get; set; }
        public DbSet<Example> Examples { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<UserVocabularyProgress> UserVocabularyProgress { get; set; }

        // Category related entities
        public DbSet<Category> Categories { get; set; }

        // Lesson related entities
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LessonSection> LessonSections { get; set; }
        public DbSet<LessonVocabulary> LessonVocabulary { get; set; }
        public DbSet<UserLessonProgress> UserLessonProgress { get; set; }

        // Course related entities
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseModule> CourseModules { get; set; }
        public DbSet<UserEnrollment> UserEnrollments { get; set; }

        // Exercise related entities
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseQuestion> ExerciseQuestions { get; set; }
        public DbSet<ExerciseVocabulary> ExerciseVocabulary { get; set; }
        public DbSet<UserExerciseAttempt> UserExerciseAttempts { get; set; }

        #endregion

        public AppDbContext(DbContextOptions<AppDbContext> options, ILogger<AppDbContext> logger)
            : base(options)
        {
            _logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entities
            ConfigureUsers(modelBuilder);
            ConfigureVocabulary(modelBuilder);
            ConfigureCategories(modelBuilder);
            ConfigureLessons(modelBuilder);
            ConfigureCourses(modelBuilder);
            ConfigureExercises(modelBuilder);

            // Configure global query filters for soft delete
            ApplySoftDeleteQueryFilter(modelBuilder);
        }

        private void ConfigureUsers(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            // Role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            // UserActivity
            modelBuilder.Entity<UserActivity>(entity =>
            {
                entity.ToTable("UserActivities");
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.Timestamp);
                entity.HasIndex(e => new { e.Module, e.Action });
                entity.Property(e => e.RowVersion).IsRowVersion();
            });
        }

        private void ConfigureVocabulary(ModelBuilder modelBuilder)
        {
            // VocabularyItem
            modelBuilder.Entity<VocabularyItem>(entity =>
            {
                entity.ToTable("VocabularyItems");
                entity.HasIndex(e => new { e.Term, e.LanguageCode }).IsUnique();
                entity.HasIndex(e => e.CategoryId);
                entity.HasIndex(e => e.LanguageCode);
                entity.HasIndex(e => e.DifficultyLevel);
                entity.HasIndex(e => e.Status);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.VocabularyItems)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Definition
            modelBuilder.Entity<Definition>(entity =>
            {
                entity.ToTable("Definitions");
                entity.HasIndex(e => e.VocabularyItemId);
                entity.HasIndex(e => e.PartOfSpeech);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.VocabularyItem)
                    .WithMany(v => v.Definitions)
                    .HasForeignKey(e => e.VocabularyItemId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Example
            modelBuilder.Entity<Example>(entity =>
            {
                entity.ToTable("Examples");
                entity.HasIndex(e => e.VocabularyItemId);
                entity.HasIndex(e => e.DifficultyLevel);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.VocabularyItem)
                    .WithMany(v => v.Examples)
                    .HasForeignKey(e => e.VocabularyItemId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Translation
            modelBuilder.Entity<Translation>(entity =>
            {
                entity.ToTable("Translations");
                entity.HasIndex(e => e.VocabularyItemId);
                entity.HasIndex(e => e.LanguageCode);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.VocabularyItem)
                    .WithMany(v => v.Translations)
                    .HasForeignKey(e => e.VocabularyItemId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // UserVocabularyProgress
            modelBuilder.Entity<UserVocabularyProgress>(entity =>
            {
                entity.ToTable("UserVocabularyProgress");
                entity.HasIndex(e => new { e.UserId, e.VocabularyItemId }).IsUnique();
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.NextReviewAt);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.VocabularyItem)
                    .WithMany(v => v.UserProgress)
                    .HasForeignKey(e => e.VocabularyItemId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureCategories(ModelBuilder modelBuilder)
        {
            // Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasIndex(e => new { e.Name, e.ParentId }).IsUnique();
                entity.HasIndex(e => e.ParentId);
                entity.HasIndex(e => e.Status);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.Parent)
                    .WithMany(c => c.Children)
                    .HasForeignKey(e => e.ParentId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }

        private void ConfigureLessons(ModelBuilder modelBuilder)
        {
            // Lesson
            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.ToTable("Lessons");
                entity.HasIndex(e => e.CourseId);
                entity.HasIndex(e => e.LanguageCode);
                entity.HasIndex(e => e.TargetLanguageCode);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.DifficultyLevel);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.Course)
                    .WithMany(c => c.Lessons)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // LessonSection
            modelBuilder.Entity<LessonSection>(entity =>
            {
                entity.ToTable("LessonSections");
                entity.HasIndex(e => e.LessonId);
                entity.HasIndex(e => e.ContentType);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.Lesson)
                    .WithMany(l => l.Sections)
                    .HasForeignKey(e => e.LessonId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Exercise)
                    .WithMany()
                    .HasForeignKey(e => e.ExerciseId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // LessonVocabulary
            modelBuilder.Entity<LessonVocabulary>(entity =>
            {
                entity.ToTable("LessonVocabulary");
                entity.HasIndex(e => new { e.LessonId, e.VocabularyItemId }).IsUnique();
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.Lesson)
                    .WithMany(l => l.VocabularyItems)
                    .HasForeignKey(e => e.LessonId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.VocabularyItem)
                    .WithMany()
                    .HasForeignKey(e => e.VocabularyItemId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // UserLessonProgress
            modelBuilder.Entity<UserLessonProgress>(entity =>
            {
                entity.ToTable("UserLessonProgress");
                entity.HasIndex(e => new { e.UserId, e.LessonId }).IsUnique();
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.CompletionPercentage);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Lesson)
                    .WithMany(l => l.UserProgress)
                    .HasForeignKey(e => e.LessonId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureCourses(ModelBuilder modelBuilder)
        {
            // Course
            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Courses");
                entity.HasIndex(e => e.Title);
                entity.HasIndex(e => e.LanguageCode);
                entity.HasIndex(e => e.TargetLanguageCode);
                entity.HasIndex(e => e.Level);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.CategoryId);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.Category)
                    .WithMany()
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // CourseModule
            modelBuilder.Entity<CourseModule>(entity =>
            {
                entity.ToTable("CourseModules");
                entity.HasIndex(e => e.CourseId);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.Course)
                    .WithMany(c => c.Modules)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // UserEnrollment
            modelBuilder.Entity<UserEnrollment>(entity =>
            {
                entity.ToTable("UserEnrollments");
                entity.HasIndex(e => new { e.UserId, e.CourseId }).IsUnique();
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.CompletionPercentage);
                entity.HasIndex(e => e.EnrolledAt);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Course)
                    .WithMany(c => c.Enrollments)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureExercises(ModelBuilder modelBuilder)
        {
            // Exercise
            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.ToTable("Exercises");
                entity.HasIndex(e => e.Type);
                entity.HasIndex(e => e.LanguageCode);
                entity.HasIndex(e => e.TargetLanguageCode);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.DifficultyLevel);
                entity.HasIndex(e => e.CategoryId);
                entity.HasIndex(e => e.LessonId);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.Category)
                    .WithMany()
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Lesson)
                    .WithMany()
                    .HasForeignKey(e => e.LessonId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // ExerciseQuestion
            modelBuilder.Entity<ExerciseQuestion>(entity =>
            {
                entity.ToTable("ExerciseQuestions");
                entity.HasIndex(e => e.ExerciseId);
                entity.HasIndex(e => e.Type);
                entity.HasIndex(e => e.DifficultyLevel);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.Exercise)
                    .WithMany(ex => ex.Questions)
                    .HasForeignKey(e => e.ExerciseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ExerciseVocabulary
            modelBuilder.Entity<ExerciseVocabulary>(entity =>
            {
                entity.ToTable("ExerciseVocabulary");
                entity.HasIndex(e => new { e.ExerciseId, e.VocabularyItemId }).IsUnique();
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.Exercise)
                    .WithMany(ex => ex.VocabularyItems)
                    .HasForeignKey(e => e.ExerciseId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.VocabularyItem)
                    .WithMany()
                    .HasForeignKey(e => e.VocabularyItemId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // UserExerciseAttempt
            modelBuilder.Entity<UserExerciseAttempt>(entity =>
            {
                entity.ToTable("UserExerciseAttempts");
                entity.HasIndex(e => new { e.UserId, e.ExerciseId, e.AttemptNumber }).IsUnique();
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.ScorePercentage);
                entity.HasIndex(e => e.StartedAt);
                entity.Property(e => e.RowVersion).IsRowVersion();

                // Configure relationships
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Exercise)
                    .WithMany(ex => ex.UserAttempts)
                    .HasForeignKey(e => e.ExerciseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ApplySoftDeleteQueryFilter(ModelBuilder modelBuilder)
        {
            // Apply global query filter for soft delete to all entities deriving from BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Check if the entity derives from BaseEntity
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, "IsDeleted");
                    var falseConstant = Expression.Constant(false);
                    var compareExpression = Expression.Equal(property, falseConstant);
                    var lambda = Expression.Lambda(compareExpression, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // Handle timestamp/rowversion for concurrency
                foreach (var entry in ChangeTracker.Entries<BaseEntity>())
                {
                    if (entry.State == EntityState.Modified)
                    {
                        // Set ModifiedAt if not already set
                        if (entry.Entity.ModifiedAt == null)
                        {
                            entry.Entity.ModifiedAt = DateTime.UtcNow;
                        }

                        // Make sure the original row version value is tracked
                        entry.Property(e => e.RowVersion).OriginalValue = entry.Entity.RowVersion;
                    }
                }

                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict detected in SaveChangesAsync");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SaveChangesAsync");
                throw;
            }
        }
    }
}