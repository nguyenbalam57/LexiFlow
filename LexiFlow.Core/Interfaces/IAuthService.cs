using LexiFlow.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Core.Interfaces
{
    public interface IAuthService
    {
        Task<User?> AuthenticateAsync(string username, string password);
        Task<bool> ValidateCredentialsAsync(string username, string password);
        Task<bool> UserExistsAsync(string username);
        Task<User?> GetUserByUsernameAsync(string username);
        Task UpdateLastLoginAsync(int userId);
    }
}
