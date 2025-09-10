using LexiFlow.Infrastructure.Data;
using LexiFlow.Models.Learning.Vocabulary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LexiFlow.Infrastructure.Data.Seed
{
    /// <summary>
    /// Seed data cho Vocabulary
    /// </summary>
    public class VocabularySeedData
    {
        /// <summary>
        /// Seed vocabulary data
        /// </summary>
        public static async Task SeedVocabularyAsync(LexiFlowContext context, ILogger logger)
        {
            try
            {
                logger.LogInformation("Seeding vocabulary data...");

                // Ki?m tra có vocabulary nào ch?a
                var existingCount = await context.Vocabularies.CountAsync();
                if (existingCount > 0)
                {
                    logger.LogInformation($"Vocabulary data already exists ({existingCount} records). Skipping seed.");
                    return;
                }

                // ??m b?o có user admin ?? làm CreatedBy
                var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
                int createdBy = adminUser?.UserId ?? 1;

                // T?o categories tr??c
                var basicCategory = await context.Categories.FirstOrDefaultAsync(c => c.CategoryName == "Basic");
                if (basicCategory == null)
                {
                    basicCategory = new Category
                    {
                        CategoryName = "Basic",
                        Description = "Basic Japanese vocabulary",
                        ColorCode = "#4CAF50",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = createdBy
                    };
                    context.Categories.Add(basicCategory);
                    await context.SaveChangesAsync();
                }

                var greetingsCategory = await context.Categories.FirstOrDefaultAsync(c => c.CategoryName == "Greetings");
                if (greetingsCategory == null)
                {
                    greetingsCategory = new Category
                    {
                        CategoryName = "Greetings",
                        Description = "Greeting expressions",
                        ColorCode = "#FF5722",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = createdBy
                    };
                    context.Categories.Add(greetingsCategory);
                    await context.SaveChangesAsync();
                }

                // Sample vocabulary data
                var vocabularies = new List<Vocabulary>
                {
                    new Vocabulary
                    {
                        Term = "?????",
                        Reading = "?????",
                        Level = "N5",
                        CategoryId = greetingsCategory.CategoryId,
                        PartOfSpeech = "expression",
                        DifficultyLevel = 1,
                        FrequencyRank = 10,
                        IsCommon = true,
                        Status = "Active",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = createdBy,
                        LanguageCode = "ja",
                        Definitions = new List<Definition>
                        {
                            new Definition
                            {
                                Text = "Hello, good afternoon",
                                LanguageCode = "en",
                                CreatedBy = createdBy,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow,
                                IsActive = true
                            }
                        },
                        Examples = new List<Example>
                        {
                            new Example
                            {
                                Text = "???????????",
                                Translation = "Hello, Mr. Tanaka.",
                                LanguageCode = "en",
                                CreatedBy = createdBy,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow,
                                IsActive = true
                            }
                        }
                    },
                    new Vocabulary
                    {
                        Term = "?????",
                        Reading = "?????",
                        Level = "N5",
                        CategoryId = greetingsCategory.CategoryId,
                        PartOfSpeech = "expression",
                        DifficultyLevel = 1,
                        FrequencyRank = 9,
                        IsCommon = true,
                        Status = "Active",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = createdBy,
                        LanguageCode = "ja",
                        Definitions = new List<Definition>
                        {
                            new Definition
                            {
                                Text = "Thank you",
                                LanguageCode = "en",
                                CreatedBy = createdBy,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow,
                                IsActive = true
                            }
                        },
                        Examples = new List<Example>
                        {
                            new Example
                            {
                                Text = "???????????",
                                Translation = "Thank you very much.",
                                LanguageCode = "en",
                                CreatedBy = createdBy,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow,
                                IsActive = true
                            }
                        }
                    },
                    new Vocabulary
                    {
                        Term = "?????",
                        Reading = "?????",
                        Level = "N5",
                        CategoryId = greetingsCategory.CategoryId,
                        PartOfSpeech = "expression",
                        DifficultyLevel = 1,
                        FrequencyRank = 8,
                        IsCommon = true,
                        Status = "Active",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = createdBy,
                        LanguageCode = "ja",
                        Definitions = new List<Definition>
                        {
                            new Definition
                            {
                                Text = "Goodbye",
                                LanguageCode = "en",
                                CreatedBy = createdBy,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow,
                                IsActive = true
                            }
                        }
                    },
                    new Vocabulary
                    {
                        Term = "??",
                        Reading = "????",
                        AlternativeReadings = "????",
                        Level = "N5",
                        CategoryId = basicCategory.CategoryId,
                        PartOfSpeech = "noun",
                        DifficultyLevel = 2,
                        FrequencyRank = 7,
                        IsCommon = true,
                        Status = "Active",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = createdBy,
                        LanguageCode = "ja",
                        Definitions = new List<Definition>
                        {
                            new Definition
                            {
                                Text = "School",
                                LanguageCode = "en",
                                CreatedBy = createdBy,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow,
                                IsActive = true
                            }
                        },
                        Examples = new List<Example>
                        {
                            new Example
                            {
                                Text = "??????????",
                                Translation = "I go to school.",
                                LanguageCode = "en",
                                CreatedBy = createdBy,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow,
                                IsActive = true
                            }
                        }
                    },
                    new Vocabulary
                    {
                        Term = "???",
                        Reading = "???",
                        Level = "N5",
                        CategoryId = basicCategory.CategoryId,
                        PartOfSpeech = "verb",
                        DifficultyLevel = 2,
                        FrequencyRank = 8,
                        IsCommon = true,
                        Status = "Active",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = createdBy,
                        LanguageCode = "ja",
                        Definitions = new List<Definition>
                        {
                            new Definition
                            {
                                Text = "To eat",
                                LanguageCode = "en",
                                CreatedBy = createdBy,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow,
                                IsActive = true
                            }
                        },
                        Examples = new List<Example>
                        {
                            new Example
                            {
                                Text = "??????????",
                                Translation = "I eat breakfast.",
                                LanguageCode = "en",
                                CreatedBy = createdBy,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow,
                                IsActive = true
                            }
                        }
                    }
                };

                context.Vocabularies.AddRange(vocabularies);
                await context.SaveChangesAsync();

                logger.LogInformation($"Successfully seeded {vocabularies.Count} vocabulary records");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error seeding vocabulary data");
                throw;
            }
        }
    }
}