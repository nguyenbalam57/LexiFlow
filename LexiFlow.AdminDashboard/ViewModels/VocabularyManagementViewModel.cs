using LexiFlow.AdminDashboard.ViewModels.Base;
using LexiFlow.AdminDashboard.Services;
using LexiFlow.Models.Learning.Vocabulary;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LexiFlow.AdminDashboard.ViewModels
{
    /// <summary>
    /// ViewModel for Vocabulary Management
    /// </summary>
    public class VocabularyManagementViewModel : ViewModelBase
    {
        private readonly IVocabularyManagementService _vocabularyService;
        private readonly IDialogService _dialogService;
        private readonly ILogger<VocabularyManagementViewModel> _logger;

        // Collections
        public ObservableCollection<Vocabulary> Vocabularies { get; } = new();
        public ObservableCollection<Category> Categories { get; } = new();
        public ObservableCollection<Vocabulary> SelectedVocabularies { get; } = new();

        // Selected Items
        private Vocabulary _selectedVocabulary;
        private Category _selectedCategory;

        // UI State
        private bool _isLoading;
        private string _searchText = "";
        private int _currentPage = 1;
        private int _pageSize = 50;
        private int _totalVocabularies;
        private string _statusMessage = "";

        // Filter Properties
        private List<int> _filterCategoryIds = new();
        private List<string> _filterJLPTLevels = new();
        private bool? _filterIsActive = true;

        // Form Properties for Create/Edit Vocabulary
        private string _formWord = "";
        private string _formHiragana = "";
        private string _formKatakana = "";
        private string _formRomaji = "";
        private string _formMeaning = "";
        private string _formPartOfSpeech = "";
        private string _formJLPTLevel = "N5";
        private int? _formCategoryId;
        private string _formExampleSentence = "";
        private string _formExampleMeaning = "";
        private string _formNotes = "";
        private string _formTags = "";
        private int _formDifficultyLevel = 1;
        private bool _formIsActive = true;
        private bool _isEditMode = false;

        // Form Properties for Create/Edit Category
        private string _formCategoryName = "";
        private string _formCategoryDescription = "";
        private bool _formCategoryIsActive = true;
        private bool _isCategoryEditMode = false;

        // Statistics
        private VocabularyStatistics _statistics;

        // Properties
        public Vocabulary SelectedVocabulary
        {
            get => _selectedVocabulary;
            set
            {
                if (SetProperty(ref _selectedVocabulary, value))
                {
                    OnPropertyChanged(nameof(IsVocabularySelected));
                    LoadVocabularyForEdit();
                }
            }
        }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (SetProperty(ref _selectedCategory, value))
                {
                    OnPropertyChanged(nameof(IsCategorySelected));
                    LoadCategoryForEdit();
                }
            }
        }

        public bool IsVocabularySelected => SelectedVocabulary != null;
        public bool IsCategorySelected => SelectedCategory != null;
        public bool IsMultipleVocabulariesSelected => SelectedVocabularies?.Count > 1;
        public int SelectedVocabulariesCount => SelectedVocabularies?.Count ?? 0;

        public new bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    SearchCommand.Execute(null);
                }
            }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (SetProperty(ref _currentPage, value))
                {
                    _ = LoadVocabulariesAsync();
                }
            }
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (SetProperty(ref _pageSize, value))
                {
                    CurrentPage = 1;
                    _ = LoadVocabulariesAsync();
                }
            }
        }

        public int TotalVocabularies
        {
            get => _totalVocabularies;
            set => SetProperty(ref _totalVocabularies, value);
        }

        public int TotalPages => (int)Math.Ceiling((double)TotalVocabularies / PageSize);

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        // Filter Properties
        public List<int> FilterCategoryIds
        {
            get => _filterCategoryIds;
            set
            {
                if (SetProperty(ref _filterCategoryIds, value))
                {
                    ApplyFiltersCommand.Execute(null);
                }
            }
        }

        public List<string> FilterJLPTLevels
        {
            get => _filterJLPTLevels;
            set
            {
                if (SetProperty(ref _filterJLPTLevels, value))
                {
                    ApplyFiltersCommand.Execute(null);
                }
            }
        }

        public bool? FilterIsActive
        {
            get => _filterIsActive;
            set
            {
                if (SetProperty(ref _filterIsActive, value))
                {
                    ApplyFiltersCommand.Execute(null);
                }
            }
        }

        // Form Properties - Vocabulary
        public string FormWord
        {
            get => _formWord;
            set => SetProperty(ref _formWord, value);
        }

        public string FormHiragana
        {
            get => _formHiragana;
            set => SetProperty(ref _formHiragana, value);
        }

        public string FormKatakana
        {
            get => _formKatakana;
            set => SetProperty(ref _formKatakana, value);
        }

        public string FormRomaji
        {
            get => _formRomaji;
            set => SetProperty(ref _formRomaji, value);
        }

        public string FormMeaning
        {
            get => _formMeaning;
            set => SetProperty(ref _formMeaning, value);
        }

        public string FormPartOfSpeech
        {
            get => _formPartOfSpeech;
            set => SetProperty(ref _formPartOfSpeech, value);
        }

        public string FormJLPTLevel
        {
            get => _formJLPTLevel;
            set => SetProperty(ref _formJLPTLevel, value);
        }

        public int? FormCategoryId
        {
            get => _formCategoryId;
            set => SetProperty(ref _formCategoryId, value);
        }

        public string FormExampleSentence
        {
            get => _formExampleSentence;
            set => SetProperty(ref _formExampleSentence, value);
        }

        public string FormExampleMeaning
        {
            get => _formExampleMeaning;
            set => SetProperty(ref _formExampleMeaning, value);
        }

        public string FormNotes
        {
            get => _formNotes;
            set => SetProperty(ref _formNotes, value);
        }

        public string FormTags
        {
            get => _formTags;
            set => SetProperty(ref _formTags, value);
        }

        public int FormDifficultyLevel
        {
            get => _formDifficultyLevel;
            set => SetProperty(ref _formDifficultyLevel, value);
        }

        public bool FormIsActive
        {
            get => _formIsActive;
            set => SetProperty(ref _formIsActive, value);
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        // Form Properties - Category
        public string FormCategoryName
        {
            get => _formCategoryName;
            set => SetProperty(ref _formCategoryName, value);
        }

        public string FormCategoryDescription
        {
            get => _formCategoryDescription;
            set => SetProperty(ref _formCategoryDescription, value);
        }

        public bool FormCategoryIsActive
        {
            get => _formCategoryIsActive;
            set => SetProperty(ref _formCategoryIsActive, value);
        }

        public bool IsCategoryEditMode
        {
            get => _isCategoryEditMode;
            set => SetProperty(ref _isCategoryEditMode, value);
        }

        // Statistics
        public VocabularyStatistics Statistics
        {
            get => _statistics;
            set => SetProperty(ref _statistics, value);
        }

        // Commands
        public ICommand LoadVocabulariesCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ApplyFiltersCommand { get; }
        public ICommand ClearFiltersCommand { get; }
        public ICommand RefreshCommand { get; }

        // Vocabulary CRUD Commands
        public ICommand CreateVocabularyCommand { get; }
        public ICommand EditVocabularyCommand { get; }
        public ICommand DeleteVocabularyCommand { get; }
        public ICommand SaveVocabularyCommand { get; }
        public ICommand CancelEditVocabularyCommand { get; }

        // Category CRUD Commands
        public ICommand CreateCategoryCommand { get; }
        public ICommand EditCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }
        public ICommand SaveCategoryCommand { get; }
        public ICommand CancelEditCategoryCommand { get; }

        // Bulk Commands
        public ICommand BulkDeleteVocabulariesCommand { get; }
        public ICommand BulkUpdateCategoryCommand { get; }
        public ICommand ExportVocabulariesCommand { get; }
        public ICommand ImportVocabulariesCommand { get; }

        // Advanced Commands
        public ICommand ViewStatisticsCommand { get; }
        public ICommand GetRandomVocabulariesCommand { get; }
        public ICommand UploadAudioCommand { get; }

        // Pagination Commands
        public ICommand FirstPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand LastPageCommand { get; }

        public VocabularyManagementViewModel(
            IVocabularyManagementService vocabularyService, 
            IDialogService dialogService,
            ILogger<VocabularyManagementViewModel> logger)
        {
            _vocabularyService = vocabularyService;
            _dialogService = dialogService;
            _logger = logger;

            // Initialize Commands
            LoadVocabulariesCommand = CreateCommand(async _ => await LoadVocabulariesAsync());
            SearchCommand = CreateCommand(async _ => await SearchVocabulariesAsync(), _ => !IsLoading);
            ApplyFiltersCommand = CreateCommand(async _ => await ApplyFiltersAsync(), _ => !IsLoading);
            ClearFiltersCommand = CreateCommand(_ => ClearFilters());
            RefreshCommand = CreateCommand(async _ => await RefreshAsync());

            // Vocabulary CRUD Commands
            CreateVocabularyCommand = CreateCommand(_ => StartCreateVocabulary());
            EditVocabularyCommand = CreateCommand(_ => StartEditVocabulary(), _ => IsVocabularySelected);
            DeleteVocabularyCommand = CreateCommand(async _ => await DeleteVocabularyAsync(), _ => IsVocabularySelected);
            SaveVocabularyCommand = CreateCommand(async _ => await SaveVocabularyAsync());
            CancelEditVocabularyCommand = CreateCommand(_ => CancelEditVocabulary());

            // Category CRUD Commands
            CreateCategoryCommand = CreateCommand(_ => StartCreateCategory());
            EditCategoryCommand = CreateCommand(_ => StartEditCategory(), _ => IsCategorySelected);
            DeleteCategoryCommand = CreateCommand(async _ => await DeleteCategoryAsync(), _ => IsCategorySelected);
            SaveCategoryCommand = CreateCommand(async _ => await SaveCategoryAsync());
            CancelEditCategoryCommand = CreateCommand(_ => CancelEditCategory());

            // Bulk Commands
            BulkDeleteVocabulariesCommand = CreateCommand(async _ => await BulkDeleteVocabulariesAsync(), _ => IsMultipleVocabulariesSelected);
            BulkUpdateCategoryCommand = CreateCommand(async _ => await BulkUpdateCategoryAsync(), _ => IsMultipleVocabulariesSelected);
            ExportVocabulariesCommand = CreateCommand(async _ => await ExportVocabulariesAsync());
            ImportVocabulariesCommand = CreateCommand(async _ => await ImportVocabulariesAsync());

            // Advanced Commands
            ViewStatisticsCommand = CreateCommand(async _ => await LoadStatisticsAsync());
            GetRandomVocabulariesCommand = CreateCommand(async _ => await GetRandomVocabulariesAsync());
            UploadAudioCommand = CreateCommand(async _ => await UploadAudioAsync(), _ => IsVocabularySelected);

            // Pagination Commands
            FirstPageCommand = CreateCommand(_ => CurrentPage = 1, _ => CurrentPage > 1);
            PreviousPageCommand = CreateCommand(_ => CurrentPage--, _ => CurrentPage > 1);
            NextPageCommand = CreateCommand(_ => CurrentPage++, _ => CurrentPage < TotalPages);
            LastPageCommand = CreateCommand(_ => CurrentPage = TotalPages, _ => CurrentPage < TotalPages);

            // Initialize data - Load real data on startup
            _ = Task.Run(async () =>
            {
                await LoadInitialDataAsync();
            });

            StatusMessage = "Vocabulary Management ready";
        }

        #region Data Loading Methods

        /// <summary>
        /// Load initial data including categories and vocabularies
        /// </summary>
        private async Task LoadInitialDataAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Loading initial data...";

                // Load categories first
                await LoadCategoriesAsync();
                
                // Load vocabularies
                await LoadVocabulariesAsync();
                
                // Load statistics
                await LoadStatisticsAsync();
                
                StatusMessage = "Data loaded successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading initial data");
                StatusMessage = "Error loading data";
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Load categories from service
        /// </summary>
        private async Task LoadCategoriesAsync()
        {
            try
            {
                var categories = await _vocabularyService.GetCategoriesAsync();
                
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Categories.Clear();
                    foreach (var category in categories)
                    {
                        Categories.Add(category);
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading categories");
                StatusMessage = "Error loading categories";
            }
        }

        /// <summary>
        /// Load vocabularies with current filters
        /// </summary>
        private async Task LoadVocabulariesAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Loading vocabularies...";

                var vocabularies = await _vocabularyService.GetVocabulariesAsync(CurrentPage, PageSize, SearchText);
                var totalCount = await _vocabularyService.GetVocabularyCountAsync(SearchText);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Vocabularies.Clear();
                    foreach (var vocabulary in vocabularies)
                    {
                        Vocabularies.Add(vocabulary);
                    }
                    
                    TotalVocabularies = totalCount;
                    OnPropertyChanged(nameof(TotalPages));
                });

                StatusMessage = $"Loaded {vocabularies.Count} vocabularies (Page {CurrentPage} of {TotalPages})";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading vocabularies");
                StatusMessage = "Error loading vocabularies";
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Search vocabularies with filters
        /// </summary>
        private async Task SearchVocabulariesAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Searching vocabularies...";

                var filter = new VocabularySearchFilter
                {
                    SearchTerm = SearchText,
                    CategoryIds = FilterCategoryIds?.Count > 0 ? FilterCategoryIds : null,
                    JLPTLevels = FilterJLPTLevels?.Count > 0 ? FilterJLPTLevels : null,
                    IsActive = FilterIsActive,
                    Page = CurrentPage,
                    PageSize = PageSize,
                    SortBy = "Term",
                    SortDescending = false
                };

                var vocabularies = await _vocabularyService.SearchVocabulariesAsync(filter);
                var totalCount = await _vocabularyService.GetVocabularyCountAsync(SearchText);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Vocabularies.Clear();
                    foreach (var vocabulary in vocabularies)
                    {
                        Vocabularies.Add(vocabulary);
                    }
                    
                    TotalVocabularies = totalCount;
                    OnPropertyChanged(nameof(TotalPages));
                });

                StatusMessage = $"Found {vocabularies.Count} vocabularies matching search criteria";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching vocabularies");
                StatusMessage = "Error searching vocabularies";
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Apply current filters
        /// </summary>
        private async Task ApplyFiltersAsync()
        {
            CurrentPage = 1; // Reset to first page when applying filters
            await SearchVocabulariesAsync();
        }

        /// <summary>
        /// Load vocabulary statistics
        /// </summary>
        private async Task LoadStatisticsAsync()
        {
            try
            {
                var stats = await _vocabularyService.GetVocabularyStatisticsAsync();
                
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Statistics = stats;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading statistics");
            }
        }

        #endregion

        #region Vocabulary CRUD Methods

        /// <summary>
        /// Load vocabulary data for editing
        /// </summary>
        private void LoadVocabularyForEdit()
        {
            if (SelectedVocabulary == null) return;

            var vocab = SelectedVocabulary;
            FormWord = vocab.Term;
            FormHiragana = vocab.Reading;
            FormKatakana = vocab.AlternativeReadings;
            FormMeaning = vocab.Translations?.FirstOrDefault()?.Text ?? "";
            FormPartOfSpeech = vocab.PartOfSpeech;
            FormJLPTLevel = vocab.Level;
            FormCategoryId = vocab.CategoryId;
            FormDifficultyLevel = vocab.DifficultyLevel;
            FormIsActive = vocab.Status == "Active";
            
            // Load metadata if exists
            if (!string.IsNullOrEmpty(vocab.MetadataJson))
            {
                try
                {
                    var metadata = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(vocab.MetadataJson);
                    FormRomaji = metadata.GetValueOrDefault("romaji", "")?.ToString() ?? "";
                    FormExampleSentence = metadata.GetValueOrDefault("exampleSentence", "")?.ToString() ?? "";
                    FormExampleMeaning = metadata.GetValueOrDefault("exampleMeaning", "")?.ToString() ?? "";
                    FormNotes = metadata.GetValueOrDefault("notes", "")?.ToString() ?? "";
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error parsing metadata for vocabulary {VocabularyId}", vocab.Id);
                }
            }

            // Load tags if exists
            if (!string.IsNullOrEmpty(vocab.Tags))
            {
                try
                {
                    var tags = System.Text.Json.JsonSerializer.Deserialize<List<string>>(vocab.Tags);
                    FormTags = string.Join(", ", tags);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error parsing tags for vocabulary {VocabularyId}", vocab.Id);
                }
            }
        }

        /// <summary>
        /// Save vocabulary (create or update)
        /// </summary>
        private async Task SaveVocabularyAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = IsEditMode ? "Updating vocabulary..." : "Creating vocabulary...";

                // Validate required fields
                if (string.IsNullOrWhiteSpace(FormWord) || string.IsNullOrWhiteSpace(FormMeaning))
                {
                    StatusMessage = "Please fill in required fields (Word and Meaning)";
                    return;
                }

                if (IsEditMode && SelectedVocabulary != null)
                {
                    // Update existing vocabulary
                    var updateRequest = new UpdateVocabularyRequest
                    {
                        Word = FormWord.Trim(),
                        Hiragana = FormHiragana?.Trim(),
                        Katakana = FormKatakana?.Trim(),
                        Romaji = FormRomaji?.Trim(),
                        Meaning = FormMeaning.Trim(),
                        PartOfSpeech = FormPartOfSpeech?.Trim(),
                        JLPTLevel = FormJLPTLevel,
                        CategoryId = FormCategoryId,
                        ExampleSentence = FormExampleSentence?.Trim(),
                        ExampleMeaning = FormExampleMeaning?.Trim(),
                        Notes = FormNotes?.Trim(),
                        Tags = string.IsNullOrWhiteSpace(FormTags) ? new List<string>() : 
                               FormTags.Split(',').Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)).ToList(),
                        DifficultyLevel = FormDifficultyLevel,
                        IsActive = FormIsActive
                    };

                    var updatedVocabulary = await _vocabularyService.UpdateVocabularyAsync(SelectedVocabulary.Id, updateRequest);
                    
                    // Update in collection
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var index = Vocabularies.IndexOf(SelectedVocabulary);
                        if (index >= 0)
                        {
                            Vocabularies[index] = updatedVocabulary;
                        }
                    });

                    StatusMessage = "Vocabulary updated successfully";
                }
                else
                {
                    // Create new vocabulary
                    var createRequest = new CreateVocabularyRequest
                    {
                        Word = FormWord.Trim(),
                        Hiragana = FormHiragana?.Trim(),
                        Katakana = FormKatakana?.Trim(),
                        Romaji = FormRomaji?.Trim(),
                        Meaning = FormMeaning.Trim(),
                        PartOfSpeech = FormPartOfSpeech?.Trim(),
                        JLPTLevel = FormJLPTLevel,
                        CategoryId = FormCategoryId,
                        ExampleSentence = FormExampleSentence?.Trim(),
                        ExampleMeaning = FormExampleMeaning?.Trim(),
                        Notes = FormNotes?.Trim(),
                        Tags = string.IsNullOrWhiteSpace(FormTags) ? new List<string>() : 
                               FormTags.Split(',').Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)).ToList(),
                        DifficultyLevel = FormDifficultyLevel,
                        IsActive = FormIsActive
                    };

                    var newVocabulary = await _vocabularyService.CreateVocabularyAsync(createRequest);
                    
                    // Add to collection
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Vocabularies.Insert(0, newVocabulary);
                        TotalVocabularies++;
                        OnPropertyChanged(nameof(TotalPages));
                    });

                    StatusMessage = "Vocabulary created successfully";
                }

                ClearVocabularyForm();
                await LoadStatisticsAsync(); // Refresh statistics
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving vocabulary");
                StatusMessage = $"Error saving vocabulary: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Delete selected vocabulary
        /// </summary>
        private async Task DeleteVocabularyAsync()
        {
            if (SelectedVocabulary == null) return;

            try
            {
                // Confirm deletion
                var confirmed = _dialogService.ShowConfirmDialog(
                    $"Are you sure you want to delete the vocabulary '{SelectedVocabulary.Term}'?",
                    "Confirm Deletion");

                if (!confirmed) return;

                IsLoading = true;
                StatusMessage = "Deleting vocabulary...";

                var success = await _vocabularyService.DeleteVocabularyAsync(SelectedVocabulary.Id, true); // Soft delete

                if (success)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Vocabularies.Remove(SelectedVocabulary);
                        TotalVocabularies--;
                        OnPropertyChanged(nameof(TotalPages));
                        SelectedVocabulary = null;
                    });

                    StatusMessage = "Vocabulary deleted successfully";
                    await LoadStatisticsAsync(); // Refresh statistics
                }
                else
                {
                    StatusMessage = "Failed to delete vocabulary";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vocabulary");
                StatusMessage = $"Error deleting vocabulary: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        #endregion

        #region Category CRUD Methods

        /// <summary>
        /// Load category data for editing
        /// </summary>
        private void LoadCategoryForEdit()
        {
            if (SelectedCategory == null) return;

            var category = SelectedCategory;
            FormCategoryName = category.CategoryName;
            FormCategoryDescription = category.Description;
            FormCategoryIsActive = category.IsActive;
        }

        /// <summary>
        /// Save category (create or update)
        /// </summary>
        private async Task SaveCategoryAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = IsCategoryEditMode ? "Updating category..." : "Creating category...";

                // Validate required fields
                if (string.IsNullOrWhiteSpace(FormCategoryName))
                {
                    StatusMessage = "Please enter a category name";
                    return;
                }

                if (IsCategoryEditMode && SelectedCategory != null)
                {
                    // Update existing category
                    var updateRequest = new UpdateCategoryRequest
                    {
                        CategoryName = FormCategoryName.Trim(),
                        Description = FormCategoryDescription?.Trim(),
                        Level = "General",
                        CategoryType = "Vocabulary",
                        IsActive = FormCategoryIsActive
                    };

                    var updatedCategory = await _vocabularyService.UpdateCategoryAsync(SelectedCategory.CategoryId, updateRequest);
                    
                    // Update in collection
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var index = Categories.IndexOf(SelectedCategory);
                        if (index >= 0)
                        {
                            Categories[index] = updatedCategory;
                        }
                    });

                    StatusMessage = "Category updated successfully";
                }
                else
                {
                    // Create new category
                    var createRequest = new CreateCategoryRequest
                    {
                        CategoryName = FormCategoryName.Trim(),
                        Description = FormCategoryDescription?.Trim(),
                        Level = "General",
                        CategoryType = "Vocabulary",
                        IsActive = FormCategoryIsActive
                    };

                    var newCategory = await _vocabularyService.CreateCategoryAsync(createRequest);
                    
                    // Add to collection
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Categories.Add(newCategory);
                    });

                    StatusMessage = "Category created successfully";
                }

                ClearCategoryForm();
                await LoadStatisticsAsync(); // Refresh statistics
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving category");
                StatusMessage = $"Error saving category: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Delete selected category
        /// </summary>
        private async Task DeleteCategoryAsync()
        {
            if (SelectedCategory == null) return;

            try
            {
                // Confirm deletion
                var confirmed = _dialogService.ShowConfirmDialog(
                    $"Are you sure you want to delete the category '{SelectedCategory.CategoryName}'?",
                    "Confirm Deletion");

                if (!confirmed) return;

                IsLoading = true;
                StatusMessage = "Deleting category...";

                var success = await _vocabularyService.DeleteCategoryAsync(SelectedCategory.CategoryId, true); // Soft delete

                if (success)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Categories.Remove(SelectedCategory);
                        SelectedCategory = null;
                    });

                    StatusMessage = "Category deleted successfully";
                    await LoadStatisticsAsync(); // Refresh statistics
                }
                else
                {
                    StatusMessage = "Failed to delete category";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category");
                StatusMessage = $"Error deleting category: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        #endregion

        #region Bulk Operations

        /// <summary>
        /// Bulk delete selected vocabularies
        /// </summary>
        private async Task BulkDeleteVocabulariesAsync()
        {
            if (SelectedVocabularies?.Count == 0) return;

            try
            {
                var count = SelectedVocabularies.Count;
                var confirmed = _dialogService.ShowConfirmDialog(
                    $"Are you sure you want to delete {count} selected vocabularies?",
                    "Confirm Bulk Deletion");

                if (!confirmed) return;

                IsLoading = true;
                StatusMessage = $"Deleting {count} vocabularies...";

                var vocabularyIds = SelectedVocabularies.Select(v => v.Id).ToList();
                var success = await _vocabularyService.BulkDeleteAsync(vocabularyIds, true); // Soft delete

                if (success)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var vocab in SelectedVocabularies.ToList())
                        {
                            Vocabularies.Remove(vocab);
                        }
                        SelectedVocabularies.Clear();
                        TotalVocabularies -= count;
                        OnPropertyChanged(nameof(TotalPages));
                        OnPropertyChanged(nameof(IsMultipleVocabulariesSelected));
                        OnPropertyChanged(nameof(SelectedVocabulariesCount));
                    });

                    StatusMessage = $"{count} vocabularies deleted successfully";
                    await LoadStatisticsAsync(); // Refresh statistics
                }
                else
                {
                    StatusMessage = "Failed to delete vocabularies";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error bulk deleting vocabularies");
                StatusMessage = $"Error deleting vocabularies: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Bulk update category for selected vocabularies
        /// </summary>
        private async Task BulkUpdateCategoryAsync()
        {
            if (SelectedVocabularies?.Count == 0) return;

            try
            {
                // Show category selection dialog (simplified - in real implementation would show proper dialog)
                if (SelectedCategory == null)
                {
                    StatusMessage = "Please select a category first";
                    _dialogService.ShowInfoDialog("Please select a category first", "Category Required");
                    return;
                }

                var count = SelectedVocabularies.Count;
                var confirmed = _dialogService.ShowConfirmDialog(
                    $"Update category for {count} selected vocabularies to '{SelectedCategory.CategoryName}'?",
                    "Confirm Bulk Update");

                if (!confirmed) return;

                IsLoading = true;
                StatusMessage = $"Updating category for {count} vocabularies...";

                var vocabularyIds = SelectedVocabularies.Select(v => v.Id).ToList();
                var success = await _vocabularyService.BulkUpdateCategoryAsync(vocabularyIds, SelectedCategory.CategoryId);

                if (success)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var vocab in SelectedVocabularies)
                        {
                            vocab.CategoryId = SelectedCategory.CategoryId;
                            vocab.Category = SelectedCategory;
                        }
                        SelectedVocabularies.Clear();
                        OnPropertyChanged(nameof(IsMultipleVocabulariesSelected));
                        OnPropertyChanged(nameof(SelectedVocabulariesCount));
                    });

                    StatusMessage = $"Category updated for {count} vocabularies";
                }
                else
                {
                    StatusMessage = "Failed to update category";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error bulk updating category");
                StatusMessage = $"Error updating category: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        #endregion

        #region Import/Export Operations

        /// <summary>
        /// Export vocabularies to file
        /// </summary>
        private async Task ExportVocabulariesAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Exporting vocabularies...";

                // Export selected vocabularies or all if none selected
                var vocabularyIds = SelectedVocabularies?.Count > 0 ? 
                    SelectedVocabularies.Select(v => v.Id).ToList() : null;

                var csvData = await _vocabularyService.ExportVocabulariesAsync(vocabularyIds);

                // Save file dialog
                var fileName = _dialogService.ShowSaveFileDialog(
                    "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                    "csv",
                    $"vocabularies_{DateTime.Now:yyyyMMdd_HHmmss}.csv");

                if (!string.IsNullOrEmpty(fileName))
                {
                    await System.IO.File.WriteAllBytesAsync(fileName, csvData);
                    StatusMessage = $"Vocabularies exported to {fileName}";
                }
                else
                {
                    StatusMessage = "Export cancelled";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting vocabularies");
                StatusMessage = $"Error exporting vocabularies: {ex.Message}";
                _dialogService.ShowErrorDialog($"Error exporting vocabularies: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Import vocabularies from file
        /// </summary>
        private async Task ImportVocabulariesAsync()
        {
            try
            {
                var fileName = _dialogService.ShowOpenFileDialog(
                    "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                    "csv");

                if (string.IsNullOrEmpty(fileName)) return;

                IsLoading = true;
                StatusMessage = "Importing vocabularies...";

                // Read and parse CSV file (simplified implementation)
                var csvContent = await System.IO.File.ReadAllTextAsync(fileName);
                var lines = csvContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                
                var vocabularies = new List<CreateVocabularyRequest>();
                
                // Skip header row
                for (int i = 1; i < lines.Length; i++)
                {
                    var columns = lines[i].Split(',');
                    if (columns.Length >= 5) // Minimum required columns
                    {
                        vocabularies.Add(new CreateVocabularyRequest
                        {
                            Word = columns[0].Trim(),
                            Hiragana = columns.Length > 1 ? columns[1].Trim() : "",
                            Katakana = columns.Length > 2 ? columns[2].Trim() : "",
                            Romaji = columns.Length > 3 ? columns[3].Trim() : "",
                            Meaning = columns.Length > 4 ? columns[4].Trim() : "",
                            PartOfSpeech = columns.Length > 5 ? columns[5].Trim() : "",
                            JLPTLevel = columns.Length > 6 ? columns[6].Trim() : "N5",
                            DifficultyLevel = columns.Length > 12 && int.TryParse(columns[12], out int diff) ? diff : 1,
                            IsActive = columns.Length > 13 ? columns[13].Trim().Equals("Yes", StringComparison.OrdinalIgnoreCase) : true
                        });
                    }
                }

                if (vocabularies.Count > 0)
                {
                    var importedVocabularies = await _vocabularyService.ImportVocabulariesAsync(vocabularies);
                    
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var vocab in importedVocabularies)
                        {
                            Vocabularies.Insert(0, vocab);
                        }
                        TotalVocabularies += importedVocabularies.Count;
                        OnPropertyChanged(nameof(TotalPages));
                    });

                    StatusMessage = $"Imported {importedVocabularies.Count} vocabularies successfully";
                    await LoadStatisticsAsync(); // Refresh statistics
                }
                else
                {
                    StatusMessage = "No valid vocabulary data found in file";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing vocabularies");
                StatusMessage = $"Error importing vocabularies: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        #endregion

        #region Advanced Operations

        /// <summary>
        /// Get random vocabularies for practice
        /// </summary>
        private async Task GetRandomVocabulariesAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Loading random vocabularies...";

                var randomVocabularies = await _vocabularyService.GetRandomVocabulariesAsync(10, FormJLPTLevel);
                
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Vocabularies.Clear();
                    foreach (var vocab in randomVocabularies)
                    {
                        Vocabularies.Add(vocab);
                    }
                });

                StatusMessage = $"Loaded {randomVocabularies.Count} random vocabularies";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting random vocabularies");
                StatusMessage = $"Error loading random vocabularies: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Upload audio file for selected vocabulary
        /// </summary>
        private async Task UploadAudioAsync()
        {
            if (SelectedVocabulary == null) return;

            try
            {
                var fileName = _dialogService.ShowOpenFileDialog(
                    "Audio files (*.mp3;*.wav)|*.mp3;*.wav|All files (*.*)|*.*",
                    "mp3");

                if (string.IsNullOrEmpty(fileName)) return;

                IsLoading = true;
                StatusMessage = "Uploading audio file...";

                var audioData = await System.IO.File.ReadAllBytesAsync(fileName);
                var fileNameOnly = System.IO.Path.GetFileName(fileName);

                var success = await _vocabularyService.UploadAudioFileAsync(SelectedVocabulary.Id, audioData, fileNameOnly);

                if (success)
                {
                    StatusMessage = "Audio file uploaded successfully";
                }
                else
                {
                    StatusMessage = "Failed to upload audio file";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading audio file");
                StatusMessage = $"Error uploading audio: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Refresh all data
        /// </summary>
        private async Task RefreshAsync()
        {
            await LoadInitialDataAsync();
        }

        /// <summary>
        /// Clear all filters and reset view
        /// </summary>
        private void ClearFilters()
        {
            SearchText = "";
            FilterCategoryIds = new List<int>();
            FilterJLPTLevels = new List<string>();
            FilterIsActive = true;
            CurrentPage = 1;
            _ = Task.Run(async () => await LoadVocabulariesAsync());
        }

        #endregion

        #region Dialog Methods

        /// <summary>
        /// Start creating a new vocabulary entry
        /// </summary>
        private void StartCreateVocabulary()
        {
            ClearVocabularyForm();
            IsEditMode = false;
            
            // Show dialog
            var result = _dialogService.ShowVocabularyEditDialog(this);
            if (result == true)
            {
                // Dialog was saved via SaveVocabularyCommand
            }
        }

        /// <summary>
        /// Start editing the selected vocabulary entry
        /// </summary>
        private void StartEditVocabulary()
        {
            if (SelectedVocabulary == null) return;
            
            LoadVocabularyForEdit();
            IsEditMode = true;
            
            // Show dialog
            var result = _dialogService.ShowVocabularyEditDialog(this);
            if (result == true)
            {
                // Dialog was saved via SaveVocabularyCommand
            }
        }

        /// <summary>
        /// Start creating a new category
        /// </summary>
        private void StartCreateCategory()
        {
            ClearCategoryForm();
            IsCategoryEditMode = false;
            
            // Show dialog
            var result = _dialogService.ShowCategoryEditDialog(this);
            if (result == true)
            {
                // Dialog was saved via SaveCategoryCommand
            }
        }

        /// <summary>
        /// Start editing the selected category
        /// </summary>
        private void StartEditCategory()
        {
            if (SelectedCategory == null) return;
            
            LoadCategoryForEdit();
            IsCategoryEditMode = true;
            
            // Show dialog
            var result = _dialogService.ShowCategoryEditDialog(this);
            if (result == true)
            {
                // Dialog was saved via SaveCategoryCommand
            }
        }

        #endregion
    }
}
