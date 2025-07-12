using LexiFlow.Core.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Infrastructure.Data
{
    public class SqlEntityAdapter
    {
        private readonly string _connectionString;

        public SqlEntityAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new ArgumentNullException("Connection string not found");
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(
                "SELECT UserID, Username, PasswordHash, Email, FullName, Department, IsActive, CreatedAt, LastLogin " +
                "FROM Users WHERE Username = @Username", connection);

            command.Parameters.AddWithValue("@Username", username);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("UserID")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                    Email = !reader.IsDBNull(reader.GetOrdinal("Email"))
                        ? reader.GetString(reader.GetOrdinal("Email"))
                        : string.Empty,
                    FullName = !reader.IsDBNull(reader.GetOrdinal("FullName"))
                        ? reader.GetString(reader.GetOrdinal("FullName"))
                        : string.Empty,
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                    LastLoginAt = !reader.IsDBNull(reader.GetOrdinal("LastLogin"))
                        ? reader.GetDateTime(reader.GetOrdinal("LastLogin"))
                        : null,
                    RoleId = 1 // Mặc định là admin
                };
            }

            return null;
        }

        public async Task UpdateUserLastLoginAsync(int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(
                "UPDATE Users SET LastLogin = @LastLogin WHERE UserID = @UserID", connection);

            command.Parameters.AddWithValue("@LastLogin", DateTime.Now);
            command.Parameters.AddWithValue("@UserID", userId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(
                "SELECT COUNT(*) FROM Users WHERE Username = @Username", connection);

            command.Parameters.AddWithValue("@Username", username);

            var count = (int)await command.ExecuteScalarAsync();
            return count > 0;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = new List<User>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(
                "SELECT UserID, Username, Email, FullName, Department, IsActive, CreatedAt, LastLogin " +
                "FROM Users", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("UserID")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    Email = !reader.IsDBNull(reader.GetOrdinal("Email"))
                        ? reader.GetString(reader.GetOrdinal("Email"))
                        : string.Empty,
                    FullName = !reader.IsDBNull(reader.GetOrdinal("FullName"))
                        ? reader.GetString(reader.GetOrdinal("FullName"))
                        : string.Empty,
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                    LastLoginAt = !reader.IsDBNull(reader.GetOrdinal("LastLogin"))
                        ? reader.GetDateTime(reader.GetOrdinal("LastLogin"))
                        : null,
                    RoleId = 1 // Mặc định là admin
                });
            }

            return users;
        }

        public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
        {
            var user = await GetUserByUsernameAsync(username);

            if (user == null || !user.IsActive)
                return false;

            // Kiểm tra mật khẩu bằng BCrypt
            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }
    }
}
