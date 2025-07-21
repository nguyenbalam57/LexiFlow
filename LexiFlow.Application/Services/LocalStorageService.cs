using LexiFlow.Core.Entities;
using LexiFlow.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace LexiFlow.Application.Services
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly ILogger<LocalStorageService> _logger;
        private readonly string _dataFolder;
        private readonly string _vocabularyFile;

        public LocalStorageService(ILogger<LocalStorageService> logger)
        {
            _logger = logger;
            _dataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LexiFlow");
            _vocabularyFile = Path.Combine(_dataFolder, "vocabulary.json");

            // Ensure data folder exists
            if (!Directory.Exists(_dataFolder))
            {
                Directory.CreateDirectory(_dataFolder);
            }
        }

        public async Task<Vocabulary> GetVocabularyByIdAsync(int id)
        {
            try
            {
                var allVocabulary = await GetAllVocabularyAsync();
                return allVocabulary.Find(v => v.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting vocabulary with ID {id}");
                throw;
            }
        }

        public async Task<List<Vocabulary>> GetAllVocabularyAsync()
        {
            try
            {
                if (!File.Exists(_vocabularyFile))
                {
                    return new List<Vocabulary>();
                }

                var json = await File.ReadAllTextAsync(_vocabularyFile);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<Vocabulary>>(json, options) ?? new List<Vocabulary>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all vocabulary");
                return new List<Vocabulary>();
            }
        }

        public async Task<bool> SaveVocabularyAsync(Vocabulary vocabulary)
        {
            try
            {
                // Get existing vocabulary
                var allVocabulary = await GetAllVocabularyAsync();

                // Check if vocabulary already exists
                var existingIndex = allVocabulary.FindIndex(v => v.Id == vocabulary.Id);
                if (existingIndex >= 0)
                {
                    // Update existing
                    allVocabulary[existingIndex] = vocabulary;
                }
                else
                {
                    // Add new
                    allVocabulary.Add(vocabulary);
                }

                // Save to file
                var json = JsonSerializer.Serialize(allVocabulary);
                await File.WriteAllTextAsync(_vocabularyFile, json);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error saving vocabulary with ID {vocabulary.Id}");
                return false;
            }
        }

        public async Task<bool> DeleteVocabularyAsync(int id)
        {
            try
            {
                // Get existing vocabulary
                var allVocabulary = await GetAllVocabularyAsync();

                // Remove vocabulary with matching ID
                var removed = allVocabulary.RemoveAll(v => v.Id == id);
                if (removed > 0)
                {
                    // Save to file
                    var json = JsonSerializer.Serialize(allVocabulary);
                    await File.WriteAllTextAsync(_vocabularyFile, json);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting vocabulary with ID {id}");
                return false;
            }
        }

        public async Task<bool> ClearAllVocabularyAsync()
        {
            try
            {
                // Create empty list and save to file
                var emptyList = new List<Vocabulary>();
                var json = JsonSerializer.Serialize(emptyList);
                await File.WriteAllTextAsync(_vocabularyFile, json);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing all vocabulary");
                return false;
            }
        }
    }
}
