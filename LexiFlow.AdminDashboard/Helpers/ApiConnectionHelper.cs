using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LexiFlow.AdminDashboard.Helpers
{
    public static class ApiConnectionHelper
    {
        /// <summary>
        /// Test API connection and suggest configuration
        /// </summary>
        public static async Task<ApiConnectionResult> TestApiConnectionAsync(string baseUrl, ILogger logger)
        {
            var result = new ApiConnectionResult { BaseUrl = baseUrl };

            // Test HTTPS first
            result.HttpsResult = await TestUrlAsync($"{baseUrl}/health", true, logger);
            
            // If HTTPS fails, test HTTP
            if (!result.HttpsResult.Success)
            {
                var httpUrl = baseUrl.Replace("https://", "http://");
                result.HttpResult = await TestUrlAsync($"{httpUrl}/health", false, logger);
            }

            // Determine recommendations
            result.DetermineRecommendations();
            
            return result;
        }

        private static async Task<ConnectionTestResult> TestUrlAsync(string url, bool isHttps, ILogger logger)
        {
            var result = new ConnectionTestResult { Url = url, IsHttps = isHttps };

            try
            {
                using var handler = new HttpClientHandler();
                
                if (isHttps)
                {
                    // For development, allow self-signed certificates
                    handler.ServerCertificateCustomValidationCallback = ValidateCertificate;
                }

                using var client = new HttpClient(handler);
                client.Timeout = TimeSpan.FromSeconds(10);

                using var response = await client.GetAsync(url);
                result.Success = response.IsSuccessStatusCode;
                result.StatusCode = response.StatusCode;
                result.ResponseTime = DateTime.Now;

                logger.LogInformation("API test successful: {Url} - {StatusCode}", url, response.StatusCode);
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("SSL") || ex.Message.Contains("certificate"))
            {
                result.SslError = true;
                result.ErrorMessage = "SSL/Certificate error";
                logger.LogWarning("SSL error testing {Url}: {Error}", url, ex.Message);
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                logger.LogWarning("Error testing {Url}: {Error}", url, ex.Message);
            }

            return result;
        }

        private static bool ValidateCertificate(
            HttpRequestMessage request,
            X509Certificate2? certificate,
            X509Chain? chain,
            SslPolicyErrors sslPolicyErrors)
        {
            // In development, accept all certificates
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                return true;
            }

            // In production, use default validation
            return sslPolicyErrors == SslPolicyErrors.None;
        }

        public static string GetConnectionTroubleshootingGuide(ApiConnectionResult result)
        {
            return $@"API CONNECTION TROUBLESHOOTING GUIDE:

Current Configuration:
- Base URL: {result.BaseUrl}
- HTTPS Test: {(result.HttpsResult?.Success == true ? "✅ Success" : "❌ Failed")}
- HTTP Test: {(result.HttpResult?.Success == true ? "✅ Success" : "❌ Failed")}

Issues Found:
{string.Join("\n", result.Issues)}

Recommendations:
{string.Join("\n", result.Recommendations)}

Steps to Fix:
1. Ensure API server is running on the correct port
2. Check Windows Firewall settings
3. Verify SSL certificate configuration
4. Consider using HTTP for development
5. Update appsettings.json with correct URL

API Server Status:
- Try opening {result.BaseUrl}/swagger in browser
- Check if port {GetPortFromUrl(result.BaseUrl)} is in use
- Verify API project is set as startup project";
        }

        private static string GetPortFromUrl(string url)
        {
            try
            {
                var uri = new Uri(url);
                return uri.Port.ToString();
            }
            catch
            {
                return "unknown";
            }
        }
    }

    public class ApiConnectionResult
    {
        public string BaseUrl { get; set; } = string.Empty;
        public ConnectionTestResult? HttpsResult { get; set; }
        public ConnectionTestResult? HttpResult { get; set; }
        public List<string> Issues { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();

        public void DetermineRecommendations()
        {
            if (HttpsResult?.SslError == true)
            {
                Issues.Add("SSL/TLS certificate error detected");
                Recommendations.Add("• Configure API to use valid SSL certificate");
                Recommendations.Add("• OR use HTTP for development");
                Recommendations.Add("• OR bypass SSL validation in development");
            }

            if (HttpsResult?.Success != true && HttpResult?.Success != true)
            {
                Issues.Add("API server is not responding");
                Recommendations.Add("• Start the API server project");
                Recommendations.Add("• Check if correct port is configured");
                Recommendations.Add("• Verify no other application is using the port");
            }

            if (HttpResult?.Success == true && HttpsResult?.Success != true)
            {
                Issues.Add("HTTP works but HTTPS fails");
                Recommendations.Add("• Update appsettings.json to use HTTP URL");
                Recommendations.Add("• OR configure proper SSL certificate for API");
            }
        }
    }

    public class ConnectionTestResult
    {
        public string Url { get; set; } = string.Empty;
        public bool IsHttps { get; set; }
        public bool Success { get; set; }
        public bool SslError { get; set; }
        public System.Net.HttpStatusCode? StatusCode { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime? ResponseTime { get; set; }
    }
}