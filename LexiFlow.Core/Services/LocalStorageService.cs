using LexiFlow.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using LexiFlow.Core.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace LexiFlow.Core.Services
{
    /// <summary>
    /// Triển khai lưu trữ cục bộ sử dụng SQLite
    /// </summary>
    public class LocalStorageService : ILocalStorageService
    {
        private readonly IAppSettingsService _appSettings;
        private readonly ILogger<LocalStorageService> _logger;
        private readonly string _dbPath;
        private readonly string _connectionString;

        public LocalStorageService(IAppSettingsService appSettings, ILogger<LocalStorageService> logger)
        {
            _appSettings = appSettings;
            _logger = logger;

            // Xác định đường dẫn đến file cơ sở dữ liệu
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var lexiFlowFolder = Path.Combine(appDataPath, "LexiFlow");

            // Tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(lexiFlowFolder))
            {
                Directory.CreateDirectory(lexiFlowFolder);
            }

            _dbPath = Path.Combine(lexiFlowFolder, "lexiflow.db");
            _connectionString = $"Data Source={_dbPath}";

            // Ghi chuỗi kết nối vào cài đặt
            _appSettings.LocalDbConnectionString = _connectionString;
        }

        /// <summary>
        /// Khởi tạo cơ sở dữ liệu cục bộ
        /// </summary>
        public async Task InitializeDatabaseAsync()
        {
            try
            {
                // Tạo kết nối
                using var connection = new SqliteConnection(_connectionString);
                await connection.OpenAsync();

                // Tạo bảng Vocabulary nếu chưa tồn tại
                using var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Vocabulary (
                        VocabularyID INTEGER PRIMARY KEY,
                        Japanese TEXT NOT NULL,
                        Kana TEXT,
                        Romaji TEXT,
                        Vietnamese TEXT,
                        English TEXT,
                        Example TEXT,
                        Notes TEXT,
                        GroupID INTEGER NULL,
                        Level TEXT,
                        PartOfSpeech TEXT,
                        AudioFile TEXT,
                        CreatedByUserID INTEGER NULL,
                        UpdatedByUserID INTEGER NULL,
                        CreatedAt TEXT,
                        UpdatedAt TEXT,
                        Version TEXT,
                        SyncStatus TEXT DEFAULT 'Synced',
                        IsDeleted INTEGER DEFAULT 0
                    );

                    CREATE TABLE IF NOT EXISTS SyncMetadata (
                        Key TEXT PRIMARY KEY,
                        Value TEXT
                    );
                    
                    CREATE INDEX IF NOT EXISTS idx_vocabulary_japanese ON Vocabulary(Japanese);
                    CREATE INDEX IF NOT EXISTS idx_vocabulary_sync_status ON Vocabulary(SyncStatus);
                    CREATE INDEX IF NOT EXISTS idx_vocabulary_is_deleted ON Vocabulary(IsDeleted);
                ";

                await command.ExecuteNonQueryAsync();
                _logger.LogInformation("Cơ sở dữ liệu cục bộ đã được khởi tạo thành công.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi khởi tạo cơ sở dữ liệu cục bộ");
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách từ vựng
        /// </summary>
        public async Task<List<Vocabulary>> GetVocabularyAsync(int skip = 0, int take = 20, string searchQuery = null)
        {
            var result = new List<Vocabulary>();

            try
            {
                using var connection = new SqliteConnection(_connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();

                if (string.IsNullOrEmpty(searchQuery))
                {
                    // Truy vấn không có tìm kiếm
                    command.CommandText = @"
                        SELECT * FROM Vocabulary 
                        WHERE IsDeleted = 0
                        ORDER BY VocabularyID DESC
                        LIMIT @take OFFSET @skip
                    ";
                }
                else
                {
                    // Truy vấn có tìm kiếm
                    command.CommandText = @"
                        SELECT * FROM Vocabulary 
                        WHERE IsDeleted = 0
                          AND (Japanese LIKE @search 
                               OR Kana LIKE @search 
                               OR Romaji LIKE @search 
                               OR Vietnamese LIKE @search 
                               OR English LIKE @search)
                        ORDER BY VocabularyID DESC
                        LIMIT @take OFFSET @skip
                    ";
                    command.Parameters.AddWithValue("@search", $"%{searchQuery}%");
                }

                command.Parameters.AddWithValue("@skip", skip);
                command.Parameters.AddWithValue("@take", take);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    result.Add(MapToVocabulary(reader));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách từ vựng từ cơ sở dữ liệu cục bộ");
                throw;
            }

            return result;
        }

        /// <summary>
        /// Lấy tổng số từ vựng
        /// </summary>
        public async Task<int> GetVocabularyCountAsync(string searchQuery = null)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();

                if (string.IsNullOrEmpty(searchQuery))
                {
                    // Đếm không có tìm kiếm
                    command.CommandText = "SELECT COUNT(*) FROM Vocabulary WHERE IsDeleted = 0";
                }
                else
                {
                    // Đếm có tìm kiếm
                    command.CommandText = @"
                        SELECT COUNT(*) FROM Vocabulary 
                        WHERE IsDeleted = 0
                          AND (Japanese LIKE @search 
                               OR Kana LIKE @search 
                               OR Romaji LIKE @search 
                               OR Vietnamese LIKE @search 
                               OR English LIKE @search)
                    ";
                    command.Parameters.AddWithValue("@search", $"%{searchQuery}%");
                }

                var result = await command.ExecuteScalarAsync();
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy số lượng từ vựng từ cơ sở dữ liệu cục bộ");
                throw;
            }
        }

        /// <summary>
        /// Lấy từ vựng theo ID
        /// </summary>
        public async Task<Vocabulary> GetVocabularyByIdAsync(int id)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Vocabulary WHERE VocabularyID = @id AND IsDeleted = 0";
                command.Parameters.AddWithValue("@id", id);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return MapToVocabulary(reader);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi đánh dấu từ vựng ID {id} là đã xóa trong cơ sở dữ liệu cục bộ");
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách từ vựng cần đồng bộ
        /// </summary>
        public async Task<List<Vocabulary>> GetPendingSyncVocabularyAsync()
        {
            var result = new List<Vocabulary>();

            try
            {
                using var connection = new SqliteConnection(_connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT * FROM Vocabulary 
                    WHERE (SyncStatus = 'New' OR SyncStatus = 'Modified') 
                      AND IsDeleted = 0
                ";

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    result.Add(MapToVocabulary(reader));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách từ vựng cần đồng bộ từ cơ sở dữ liệu cục bộ");
                throw;
            }

            return result;
        }

        /// <summary>
        /// Lấy danh sách ID từ vựng đã xóa cần đồng bộ
        /// </summary>
        public async Task<List<int>> GetDeletedVocabularyIdsAsync()
        {
            var result = new List<int>();

            try
            {
                using var connection = new SqliteConnection(_connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT VocabularyID FROM Vocabulary 
                    WHERE SyncStatus = 'Deleted' AND IsDeleted = 1
                ";

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    result.Add(reader.GetInt32(0));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách ID từ vựng đã xóa từ cơ sở dữ liệu cục bộ");
                throw;
            }

            return result;
        }

        /// <summary>
        /// Đánh dấu từ vựng đã đồng bộ
        /// </summary>
        public async Task<bool> MarkVocabularyAsSyncedAsync(int id)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "UPDATE Vocabulary SET SyncStatus = 'Synced' WHERE VocabularyID = @id";
                command.Parameters.AddWithValue("@id", id);

                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi đánh dấu từ vựng ID {id} là đã đồng bộ trong cơ sở dữ liệu cục bộ");
                throw;
            }
        }

        /// <summary>
        /// Cập nhật thời gian đồng bộ lần cuối
        /// </summary>
        public async Task UpdateLastSyncTimeAsync(DateTime syncTime)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT OR REPLACE INTO SyncMetadata (Key, Value) 
                    VALUES ('LastSyncTime', @value)
                ";
                command.Parameters.AddWithValue("@value", syncTime.ToString("yyyy-MM-dd HH:mm:ss"));

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật thời gian đồng bộ lần cuối trong cơ sở dữ liệu cục bộ");
                throw;
            }
        }

        /// <summary>
        /// Lấy thời gian đồng bộ lần cuối
        /// </summary>
        public async Task<DateTime?> GetLastSyncTimeAsync()
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "SELECT Value FROM SyncMetadata WHERE Key = 'LastSyncTime'";

                var result = await command.ExecuteScalarAsync();

                if (result != null && result != DBNull.Value)
                {
                    return DateTime.Parse(result.ToString());
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thời gian đồng bộ lần cuối từ cơ sở dữ liệu cục bộ");
                throw;
            }
        }

        /// <summary>
        /// Tìm kiếm từ vựng theo từ khóa
        /// </summary>
        public async Task<List<Vocabulary>> SearchVocabularyAsync(string keyword, string searchBy = "all")
        {
            var result = new List<Vocabulary>();

            if (string.IsNullOrWhiteSpace(keyword))
                return result;

            try
            {
                using var connection = new SqliteConnection(_connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();

                // Xây dựng điều kiện tìm kiếm dựa trên trường tìm kiếm
                var searchConditions = new List<string>();

                if (searchBy == "all" || searchBy == "japanese")
                {
                    searchConditions.Add("Japanese LIKE @keyword");
                }
                if (searchBy == "all" || searchBy == "kana")
                {
                    searchConditions.Add("Kana LIKE @keyword");
                }
                if (searchBy == "all" || searchBy == "romaji")
                {
                    searchConditions.Add("Romaji LIKE @keyword");
                }
                if (searchBy == "all" || searchBy == "vietnamese")
                {
                    searchConditions.Add("Vietnamese LIKE @keyword");
                }
                if (searchBy == "all" || searchBy == "english")
                {
                    searchConditions.Add("English LIKE @keyword");
                }

                var whereClause = string.Join(" OR ", searchConditions);

                command.CommandText = $@"
                    SELECT * FROM Vocabulary 
                    WHERE IsDeleted = 0 AND ({whereClause})
                    ORDER BY VocabularyID DESC
                ";
                command.Parameters.AddWithValue("@keyword", $"%{keyword}%");

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    result.Add(MapToVocabulary(reader));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi tìm kiếm từ vựng với từ khóa '{keyword}' từ cơ sở dữ liệu cục bộ");
                throw;
            }

            return result;
        }

        /// <summary>
        /// Chuyển đổi từ SqliteDataReader sang đối tượng Vocabulary
        /// </summary>
        private Vocabulary MapToVocabulary(SqliteDataReader reader)
        {
            var vocabulary = new Vocabulary
            {
                VocabularyID = reader.GetInt32(reader.GetOrdinal("VocabularyID")),
                Japanese = reader.GetString(reader.GetOrdinal("Japanese"))
            };

            // Các trường có thể null
            if (!reader.IsDBNull(reader.GetOrdinal("Kana")))
                vocabulary.Kana = reader.GetString(reader.GetOrdinal("Kana"));

            if (!reader.IsDBNull(reader.GetOrdinal("Romaji")))
                vocabulary.Romaji = reader.GetString(reader.GetOrdinal("Romaji"));

            if (!reader.IsDBNull(reader.GetOrdinal("Vietnamese")))
                vocabulary.Vietnamese = reader.GetString(reader.GetOrdinal("Vietnamese"));

            if (!reader.IsDBNull(reader.GetOrdinal("English")))
                vocabulary.English = reader.GetString(reader.GetOrdinal("English"));

            if (!reader.IsDBNull(reader.GetOrdinal("Example")))
                vocabulary.Example = reader.GetString(reader.GetOrdinal("Example"));

            if (!reader.IsDBNull(reader.GetOrdinal("Notes")))
                vocabulary.Notes = reader.GetString(reader.GetOrdinal("Notes"));

            if (!reader.IsDBNull(reader.GetOrdinal("GroupID")))
                vocabulary.GroupID = reader.GetInt32(reader.GetOrdinal("GroupID"));

            if (!reader.IsDBNull(reader.GetOrdinal("Level")))
                vocabulary.Level = reader.GetString(reader.GetOrdinal("Level"));

            if (!reader.IsDBNull(reader.GetOrdinal("PartOfSpeech")))
                vocabulary.PartOfSpeech = reader.GetString(reader.GetOrdinal("PartOfSpeech"));

            if (!reader.IsDBNull(reader.GetOrdinal("AudioFile")))
                vocabulary.AudioFile = reader.GetString(reader.GetOrdinal("AudioFile"));

            if (!reader.IsDBNull(reader.GetOrdinal("CreatedByUserID")))
                vocabulary.CreatedByUserID = reader.GetInt32(reader.GetOrdinal("CreatedByUserID"));

            if (!reader.IsDBNull(reader.GetOrdinal("UpdatedByUserID")))
                vocabulary.UpdatedByUserID = reader.GetInt32(reader.GetOrdinal("UpdatedByUserID"));

            if (!reader.IsDBNull(reader.GetOrdinal("CreatedAt")))
                vocabulary.CreatedAt = DateTime.Parse(reader.GetString(reader.GetOrdinal("CreatedAt")));

            if (!reader.IsDBNull(reader.GetOrdinal("UpdatedAt")))
                vocabulary.UpdatedAt = DateTime.Parse(reader.GetString(reader.GetOrdinal("UpdatedAt")));

            if (!reader.IsDBNull(reader.GetOrdinal("Version")))
                vocabulary.Version = reader.GetString(reader.GetOrdinal("Version"));

            if (!reader.IsDBNull(reader.GetOrdinal("SyncStatus")))
                vocabulary.SyncStatus = reader.GetString(reader.GetOrdinal("SyncStatus"));

            return vocabulary;
        }
    }

    /// <summary>
        /// Lưu từ vựng
        /// </summary>
        public async Task<bool> SaveVocabularyAsync(Vocabulary vocabulary)
        {
            if (vocabulary == null)
                return false;

            try
            {
                using var connection = new SqliteConnection(_connectionString);
                await connection.OpenAsync();

                // Kiểm tra xem từ vựng đã tồn tại chưa
                using var checkCommand = connection.CreateCommand();
                checkCommand.CommandText = "SELECT COUNT(*) FROM Vocabulary WHERE VocabularyID = @id";
                checkCommand.Parameters.AddWithValue("@id", vocabulary.VocabularyID);

                var count = Convert.ToInt32(await checkCommand.ExecuteScalarAsync());

                using var command = connection.CreateCommand();

                if (count > 0)
                {
                    // Cập nhật
                    command.CommandText = @"
                        UPDATE Vocabulary
                        SET Japanese = @japanese,
                            Kana = @kana,
                            Romaji = @romaji,
                            Vietnamese = @vietnamese,
                            English = @english,
                            Example = @example,
                            Notes = @notes,
                            GroupID = @groupId,
                            Level = @level,
                            PartOfSpeech = @partOfSpeech,
                            AudioFile = @audioFile,
                            CreatedByUserID = @createdById,
                            UpdatedByUserID = @updatedById,
                            CreatedAt = @createdAt,
                            UpdatedAt = @updatedAt,
                            Version = @version,
                            SyncStatus = 'Modified',
                            IsDeleted = 0
                        WHERE VocabularyID = @id
                    ";
                }
                else
                {
                    // Thêm mới
                    command.CommandText = @"
                        INSERT INTO Vocabulary (
                            VocabularyID, Japanese, Kana, Romaji, Vietnamese, English,
                            Example, Notes, GroupID, Level, PartOfSpeech, AudioFile,
                            CreatedByUserID, UpdatedByUserID, CreatedAt, UpdatedAt,
                            Version, SyncStatus, IsDeleted
                        ) VALUES (
                            @id, @japanese, @kana, @romaji, @vietnamese, @english,
                            @example, @notes, @groupId, @level, @partOfSpeech, @audioFile,
                            @createdById, @updatedById, @createdAt, @updatedAt,
                            @version, 'New', 0
                        )
                    ";
                }

                command.Parameters.AddWithValue("@id", vocabulary.VocabularyID);
                command.Parameters.AddWithValue("@japanese", vocabulary.Japanese);
                command.Parameters.AddWithValue("@kana", vocabulary.Kana ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@romaji", vocabulary.Romaji ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@vietnamese", vocabulary.Vietnamese ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@english", vocabulary.English ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@example", vocabulary.Example ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@notes", vocabulary.Notes ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@groupId", vocabulary.GroupID.HasValue ? (object)vocabulary.GroupID.Value : DBNull.Value);
                command.Parameters.AddWithValue("@level", vocabulary.Level ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@partOfSpeech", vocabulary.PartOfSpeech ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@audioFile", vocabulary.AudioFile ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@createdById", vocabulary.CreatedByUserID.HasValue ? (object)vocabulary.CreatedByUserID.Value : DBNull.Value);
                command.Parameters.AddWithValue("@updatedById", vocabulary.UpdatedByUserID.HasValue ? (object)vocabulary.UpdatedByUserID.Value : DBNull.Value);
                command.Parameters.AddWithValue("@createdAt", vocabulary.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@updatedAt", vocabulary.UpdatedAt.HasValue ? vocabulary.UpdatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : (object)DBNull.Value);
                command.Parameters.AddWithValue("@version", vocabulary.Version ?? Guid.NewGuid().ToString());

                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lưu từ vựng ID {vocabulary.VocabularyID} vào cơ sở dữ liệu cục bộ");
                throw;
            }
        }

        /// <summary>
        /// Xóa từ vựng
        /// </summary>
        public async Task<bool> DeleteVocabularyAsync(int id)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Vocabulary WHERE VocabularyID = @id";
                command.Parameters.AddWithValue("@id", id);

                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi xóa từ vựng ID {id} từ cơ sở dữ liệu cục bộ");
                throw;
            }
        }
        /// <summary>
        /// Đánh dấu từ vựng đã xóa
        /// </summary>
        public async Task<bool> MarkVocabularyAsDeletedAsync(int id)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "UPDATE Vocabulary SET IsDeleted = 1, SyncStatus = 'Deleted' WHERE VocabularyID = @id";
                command.Parameters.AddWithValue("@id", id);

                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy từ vựng ID {id} từ cơ sở dữ liệu cục bộ");
                throw;
            }
        }
    }

}
