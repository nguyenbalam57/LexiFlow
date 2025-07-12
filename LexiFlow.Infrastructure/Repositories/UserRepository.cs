using LexiFlow.Core.Entities;
using LexiFlow.Core.Interfaces;
using LexiFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            // Kiểm tra xem bảng Users có dữ liệu không
            try
            {
                // Kiểm tra xem username là từ SQL script hay từ Entity model
                var sqlUser = await _context.Database.SqlQuery<User>($"SELECT UserID as Id, Username, PasswordHash, Email as Email, FullName as FullName, IsActive, CreatedAt, LastLogin as LastLoginAt, 1 as RoleId FROM Users WHERE Username = {username}")
                    .FirstOrDefaultAsync();

                if (sqlUser != null)
                {
                    return sqlUser;
                }
            }
            catch
            {
                // Nếu có lỗi truy vấn, có thể là cấu trúc bảng khác, thử phương pháp Entity Framework
            }

            // Nếu không tìm thấy từ SQL hoặc có lỗi, thử dùng Entity Framework
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            try
            {
                // Thử kiểm tra từ bảng SQL trước
                var count = await _context.Database.SqlQuery<int>($"SELECT COUNT(*) FROM Users WHERE Username = {username}")
                    .FirstOrDefaultAsync();

                if (count > 0)
                {
                    return true;
                }
            }
            catch
            {
                // Nếu có lỗi, thử phương pháp Entity Framework
            }

            return await _context.Users
                .AnyAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .ToListAsync();
        }

        public async Task<int> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
