using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace LexiFlow.Core.Services
{
    /// <summary>
    /// Lớp quản lý token JWT cho ứng dụng
    /// </summary>
    public class TokenManager
    {
        private readonly ILogger<TokenManager> _logger;
        private readonly string _encryptionEntropy = "LexiFlow_Secure_Token_Storage";

        public TokenManager(ILogger<TokenManager> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Lưu token vào bộ nhớ an toàn
        /// </summary>
        /// <param name="token">Token cần lưu</param>
        /// <param name="expiresAt">Thời gian hết hạn của token</param>
        public void SaveToken(string token, DateTime expiresAt)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    ClearToken();
                    return;
                }

                // Tạo đối tượng lưu trữ
                var tokenData = new TokenData
                {
                    AccessToken = token,
                    ExpiresAt = expiresAt
                };

                // Chuyển đối tượng thành JSON
                string json = JsonSerializer.Serialize(tokenData);

                // Mã hóa dữ liệu
                byte[] encryptedData = ProtectData(Encoding.UTF8.GetBytes(json));

                // Lưu vào settings
                Properties.Settings.Default.AccessToken = Convert.ToBase64String(encryptedData);
                Properties.Settings.Default.Save();

                _logger.LogInformation("Token đã được lưu trữ an toàn");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lưu token");
                ClearToken();
            }
        }

        /// <summary>
        /// Lấy token từ bộ nhớ an toàn
        /// </summary>
        public TokenData GetToken()
        {
            try
            {
                string encryptedToken = Properties.Settings.Default.AccessToken;

                if (string.IsNullOrEmpty(encryptedToken))
                    return null;

                // Giải mã dữ liệu
                byte[] decryptedData = UnprotectData(Convert.FromBase64String(encryptedToken));

                // Chuyển JSON thành đối tượng
                string json = Encoding.UTF8.GetString(decryptedData);
                return JsonSerializer.Deserialize<TokenData>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy token từ bộ nhớ");
                ClearToken();
                return null;
            }
        }

        /// <summary>
        /// Xóa token khỏi bộ nhớ
        /// </summary>
        public void ClearToken()
        {
            Properties.Settings.Default.AccessToken = string.Empty;
            Properties.Settings.Default.Save();
            _logger.LogInformation("Token đã được xóa khỏi bộ nhớ");
        }

        /// <summary>
        /// Kiểm tra token còn hiệu lực không
        /// </summary>
        public bool IsTokenValid()
        {
            var tokenData = GetToken();
            if (tokenData == null || string.IsNullOrEmpty(tokenData.AccessToken))
                return false;

            // Kiểm tra thời gian hết hạn (để lại 5 phút đệm)
            return tokenData.ExpiresAt > DateTime.Now.AddMinutes(5);
        }

        /// <summary>
        /// Lấy token hiện tại
        /// </summary>
        public string GetCurrentToken()
        {
            var tokenData = GetToken();
            return tokenData?.AccessToken ?? string.Empty;
        }

        /// <summary>
        /// Mã hóa dữ liệu sử dụng DPAPI
        /// </summary>
        private byte[] ProtectData(byte[] data)
        {
            try
            {
                return ProtectedData.Protect(
                    data,
                    Encoding.UTF8.GetBytes(_encryptionEntropy),
                    DataProtectionScope.CurrentUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi mã hóa dữ liệu");
                // Sử dụng fallback đơn giản nếu DPAPI không khả dụng
                return Convert.FromBase64String(
                    Convert.ToBase64String(data) + Convert.ToBase64String(Encoding.UTF8.GetBytes(_encryptionEntropy)));
            }
        }

        /// <summary>
        /// Giải mã dữ liệu sử dụng DPAPI
        /// </summary>
        private byte[] UnprotectData(byte[] encryptedData)
        {
            try
            {
                return ProtectedData.Unprotect(
                    encryptedData,
                    Encoding.UTF8.GetBytes(_encryptionEntropy),
                    DataProtectionScope.CurrentUser);
            }
            catch (CryptographicException)
            {
                // Sử dụng fallback đơn giản nếu DPAPI không khả dụng
                string base64 = Convert.ToBase64String(encryptedData);
                string entropyBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(_encryptionEntropy));

                if (base64.EndsWith(entropyBase64))
                {
                    string actualDataBase64 = base64.Substring(0, base64.Length - entropyBase64.Length);
                    return Convert.FromBase64String(actualDataBase64);
                }

                throw;
            }
        }
    }

    /// <summary>
    /// Lớp lưu trữ thông tin token
    /// </summary>
    public class TokenData
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}