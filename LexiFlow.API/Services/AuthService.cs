using LexiFlow.API.Data.UnitOfWork;
using LexiFlow.API.DTOs;
using LexiFlow.API.DTOs.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LexiFlow.Models;

namespace LexiFlow.API.Services
{

    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetFirstOrDefaultAsync(u => u.Username == loginDto.Username);
                if (user == null)
                {
                    return ApiResponse<LoginResponseDto>.Fail("User not found");
                }

                if (!VerifyPasswordHash(loginDto.Password, user.PasswordHash))
                {
                    return ApiResponse<LoginResponseDto>.Fail("Invalid password");
                }

                if (!user.IsActive)
                {
                    return ApiResponse<LoginResponseDto>.Fail("User account is inactive");
                }

                // Update last login time
                user.LastLogin = DateTime.UtcNow;
                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveChangesAsync();

                // Generate JWT token
                var token = GenerateJwtToken(user);
                var refreshToken = GenerateRefreshToken();

                // Save refresh token to user
                user.ApiKey = refreshToken;
                user.ApiKeyExpiry = DateTime.UtcNow.AddDays(7);
                await _unitOfWork.SaveChangesAsync();

                // Get user roles
                var userRoles = await _unitOfWork.UserRoles.FindAsync(ur => ur.UserID == user.UserID);
                var roleIds = userRoles.Select(ur => ur.RoleID).ToList();
                var roles = await _unitOfWork.Roles.FindAsync(r => roleIds.Contains(r.RoleID));
                var roleNames = roles.Select(r => r.RoleName).ToList();

                // Create response
                var response = new LoginResponseDto
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    Expiration = DateTime.UtcNow.AddHours(1),
                    User = new UserDto
                    {
                        Id = user.UserID,
                        Username = user.Username,
                        FullName = user.FullName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Description = user.Description,
                        DepartmentName = user.DepartmentName,
                        Position = user.Position,
                        LastLogin = user.LastLogin,
                        IsActive = user.IsActive,
                        DepartmentID = user.DepartmentID,
                        CreatedAt = user.CreatedAt,
                        UpdatedAt = user.UpdatedAt,
                        Roles = roleNames
                    }
                };

                return ApiResponse<LoginResponseDto>.CreateSuccess(response, "Login successful");
            }
            catch (Exception ex)
            {
                return ApiResponse<LoginResponseDto>.Fail($"Login failed: {ex.Message}");
            }
        }

        public async Task<ApiResponse<UserDto>> RegisterAsync(RegisterUserDto registerDto)
        {
            try
            {
                // Check if username already exists
                var existingUser = await _unitOfWork.Users.GetFirstOrDefaultAsync(u => u.Username == registerDto.Username);
                if (existingUser != null)
                {
                    return ApiResponse<UserDto>.Fail("Username already exists");
                }

                // Check if email already exists (if provided)
                if (!string.IsNullOrEmpty(registerDto.Email))
                {
                    var existingEmail = await _unitOfWork.Users.GetFirstOrDefaultAsync(u => u.Email == registerDto.Email);
                    if (existingEmail != null)
                    {
                        return ApiResponse<UserDto>.Fail("Email already exists");
                    }
                }

                // Create new user
                var user = new User
                {
                    Username = registerDto.Username,
                    PasswordHash = HashPassword(registerDto.Password),
                    FullName = registerDto.FullName,
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.PhoneNumber,
                    Position = registerDto.Position,
                    Description = registerDto.Description,
                    DepartmentID = registerDto.DepartmentID,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                // Assign default "Student" role to new user
                var studentRole = await _unitOfWork.Roles.GetFirstOrDefaultAsync(r => r.RoleName == "Student");
                if (studentRole != null)
                {
                    var userRole = new UserRole
                    {
                        UserID = user.UserID,
                        RoleID = studentRole.RoleID,
                        AssignedAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _unitOfWork.UserRoles.AddAsync(userRole);
                    await _unitOfWork.SaveChangesAsync();
                }

                // Create response
                var userDto = new UserDto
                {
                    Id = user.UserID,
                    Username = user.Username,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Description = user.Description,
                    DepartmentName = user.DepartmentName,
                    Position = user.Position,
                    IsActive = user.IsActive,
                    DepartmentID = user.DepartmentID,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                    Roles = new List<string> { "Student" }
                };

                return ApiResponse<UserDto>.CreateSuccess(userDto, "User registered successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.Fail($"Registration failed: {ex.Message}");
            }
        }

        public async Task<ApiResponse<LoginResponseDto>> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var user = await _unitOfWork.Users.GetFirstOrDefaultAsync(u => u.ApiKey == refreshToken);
                if (user == null)
                {
                    return ApiResponse<LoginResponseDto>.Fail("Invalid refresh token");
                }

                if (user.ApiKeyExpiry < DateTime.UtcNow)
                {
                    return ApiResponse<LoginResponseDto>.Fail("Refresh token expired");
                }

                // Generate new JWT token
                var token = GenerateJwtToken(user);
                var newRefreshToken = GenerateRefreshToken();

                // Update refresh token
                user.ApiKey = newRefreshToken;
                user.ApiKeyExpiry = DateTime.UtcNow.AddDays(7);
                await _unitOfWork.SaveChangesAsync();

                // Get user roles
                var userRoles = await _unitOfWork.UserRoles.FindAsync(ur => ur.UserID == user.UserID);
                var roleIds = userRoles.Select(ur => ur.RoleID).ToList();
                var roles = await _unitOfWork.Roles.FindAsync(r => roleIds.Contains(r.RoleID));
                var roleNames = roles.Select(r => r.RoleName).ToList();

                // Create response
                var response = new LoginResponseDto
                {
                    Token = token,
                    RefreshToken = newRefreshToken,
                    Expiration = DateTime.UtcNow.AddHours(1),
                    User = new UserDto
                    {
                        Id = user.UserID,
                        Username = user.Username,
                        FullName = user.FullName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Description = user.Description,
                        DepartmentName = user.DepartmentName,
                        Position = user.Position,
                        LastLogin = user.LastLogin,
                        IsActive = user.IsActive,
                        DepartmentID = user.DepartmentID,
                        CreatedAt = user.CreatedAt,
                        UpdatedAt = user.UpdatedAt,
                        Roles = roleNames
                    }
                };

                return ApiResponse<LoginResponseDto>.CreateSuccess(response, "Token refreshed successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<LoginResponseDto>.Fail($"Token refresh failed: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<bool>.Fail("User not found");
                }

                if (!VerifyPasswordHash(changePasswordDto.CurrentPassword, user.PasswordHash))
                {
                    return ApiResponse<bool>.Fail("Current password is incorrect");
                }

                // Update password
                user.PasswordHash = HashPassword(changePasswordDto.NewPassword);
                user.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.CreateSuccess(true, "Password changed successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"Password change failed: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> RevokeTokenAsync(string username)
        {
            try
            {
                var user = await _unitOfWork.Users.GetFirstOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    return ApiResponse<bool>.Fail("User not found");
                }

                // Revoke refresh token
                user.ApiKey = null;
                user.ApiKeyExpiry = null;
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.CreateSuccess(true, "Token revoked successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"Token revocation failed: {ex.Message}");
            }
        }

        private string GenerateJwtToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "DefaultSecretKeyForDevelopment12345678901234");
            var tokenHandler = new JwtSecurityTokenHandler();

            // Get user roles
            var userRoles = _unitOfWork.UserRoles.FindAsync(ur => ur.UserID == user.UserID).Result;
            var roleIds = userRoles.Select(ur => ur.RoleID).ToList();
            var roles = _unitOfWork.Roles.FindAsync(r => roleIds.Contains(r.RoleID)).Result;
            var roleNames = roles.Select(r => r.RoleName).ToList();

            // Create claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim("FullName", user.FullName)
            };

            // Add role claims
            foreach (var role in roleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}