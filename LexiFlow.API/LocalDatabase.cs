using System.Text.Json;

namespace LexiFlow.API
{
    public class LocalDatabase
    {
        private readonly string _connectionString;

        public LocalDatabase(string dbPath)
        {
            _connectionString = $"Data Source={dbPath}";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
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
                Level TEXT,
                SyncStatus TEXT DEFAULT 'New',
                LocalVersion INTEGER DEFAULT 1,
                ServerVersion BLOB,
                LastModified TEXT DEFAULT CURRENT_TIMESTAMP
            );
            
            CREATE TABLE IF NOT EXISTS SyncMetadata (
                TableName TEXT PRIMARY KEY,
                LastSyncTimestamp TEXT,
                LastSyncVersion INTEGER
            );
            
            CREATE TABLE IF NOT EXISTS PendingSync (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                TableName TEXT NOT NULL,
                RecordId INTEGER,
                Operation TEXT NOT NULL,
                Data TEXT NOT NULL,
                CreatedAt TEXT DEFAULT CURRENT_TIMESTAMP,
                SyncAttempts INTEGER DEFAULT 0,
                LastError TEXT
            );
        ";
            command.ExecuteNonQuery();
        }

        public async Task<List<Vocabulary>> GetVocabularyAsync(string syncStatus = null)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Vocabulary";
            if (!string.IsNullOrEmpty(syncStatus))
                query += " WHERE SyncStatus = @syncStatus";

            var command = connection.CreateCommand();
            command.CommandText = query;
            if (!string.IsNullOrEmpty(syncStatus))
                command.Parameters.AddWithValue("@syncStatus", syncStatus);

            var result = new List<Vocabulary>();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(MapToVocabulary(reader));
            }

            return result;
        }

        public async Task SaveVocabularyAsync(Vocabulary vocab)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
            INSERT OR REPLACE INTO Vocabulary 
            (VocabularyID, Japanese, Kana, Romaji, Vietnamese, English, 
             Example, Notes, Level, SyncStatus, LocalVersion, LastModified)
            VALUES 
            (@id, @japanese, @kana, @romaji, @vietnamese, @english,
             @example, @notes, @level, @syncStatus, @localVersion, @lastModified)
        ";

            command.Parameters.AddWithValue("@id", vocab.VocabularyID);
            command.Parameters.AddWithValue("@japanese", vocab.Japanese);
            command.Parameters.AddWithValue("@kana", vocab.Kana ?? "");
            command.Parameters.AddWithValue("@romaji", vocab.Romaji ?? "");
            command.Parameters.AddWithValue("@vietnamese", vocab.Vietnamese ?? "");
            command.Parameters.AddWithValue("@english", vocab.English ?? "");
            command.Parameters.AddWithValue("@example", vocab.Example ?? "");
            command.Parameters.AddWithValue("@notes", vocab.Notes ?? "");
            command.Parameters.AddWithValue("@level", vocab.Level ?? "");
            command.Parameters.AddWithValue("@syncStatus", "Modified");
            command.Parameters.AddWithValue("@localVersion", 1);
            command.Parameters.AddWithValue("@lastModified", DateTime.UtcNow);

            await command.ExecuteNonQueryAsync();

            // Add to pending sync
            await AddToPendingSyncAsync("Vocabulary", vocab.VocabularyID, "UPDATE", vocab);
        }

        private async Task AddToPendingSyncAsync(string tableName, int recordId, string operation, object data)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
            INSERT INTO PendingSync (TableName, RecordId, Operation, Data)
            VALUES (@tableName, @recordId, @operation, @data)
        ";

            command.Parameters.AddWithValue("@tableName", tableName);
            command.Parameters.AddWithValue("@recordId", recordId);
            command.Parameters.AddWithValue("@operation", operation);
            command.Parameters.AddWithValue("@data", JsonSerializer.Serialize(data));

            await command.ExecuteNonQueryAsync();
        }
    }
}
