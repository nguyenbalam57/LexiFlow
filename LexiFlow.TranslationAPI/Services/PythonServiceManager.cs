using LexiFlow.TranslationAPI.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.ServiceProcess;

namespace LexiFlow.TranslationAPI.Services
{
    public class PythonServiceManager : IDisposable
    {
        private readonly TranslationServiceOptions _options;
        private readonly ILogger<PythonServiceManager> _logger;
        private readonly IHostApplicationLifetime _lifetime;
        private Process? _pythonProcess;
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private bool _disposed;

        public PythonServiceManager(
            IOptions<TranslationServiceOptions> options,
            ILogger<PythonServiceManager> logger,
            IHostApplicationLifetime lifetime)
        {
            _options = options.Value;
            _logger = logger;
            _lifetime = lifetime;
        }

        public async Task StartAsync()
        {
            try
            {
                // First try to check if Windows service is running
                if (await IsWindowsServiceRunningAsync())
                {
                    _logger.LogInformation("Windows service is already running");
                    return;
                }

                // Try to start Windows service
                if (await TryStartWindowsServiceAsync())
                {
                    _logger.LogInformation("Started Windows service successfully");
                    return;
                }

                // Fallback to starting Python process directly
                _logger.LogInformation("Windows service not available, starting Python process directly");
                await StartPythonProcessAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to start Python translation service");
                throw;
            }
        }

        private async Task<bool> IsWindowsServiceRunningAsync()
        {
            try
            {
                using var service = new ServiceController("LexiFlowTranslationService");
                return service.Status == ServiceControllerStatus.Running;
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Could not check Windows service status");
                return false;
            }
        }

        private async Task<bool> TryStartWindowsServiceAsync()
        {
            try
            {
                using var service = new ServiceController("LexiFlowTranslationService");

                if (service.Status == ServiceControllerStatus.Running)
                {
                    return true;
                }

                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    _logger.LogInformation("Starting Windows service...");
                    service.Start();

                    // Wait for service to start
                    service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));

                    // Wait additional time for the Python service to initialize
                    await Task.Delay(TimeSpan.FromSeconds(10), _cancellationTokenSource.Token);

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not start Windows service");
            }

            return false;
        }

        private async Task StartPythonProcessAsync()
        {
            var scriptPath = Path.Combine(_options.PythonServicePath, _options.ServiceScript);

            if (!File.Exists(scriptPath))
            {
                throw new FileNotFoundException($"Python service script not found: {scriptPath}");
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = _options.PythonExecutable,
                Arguments = $"\"{scriptPath}\"",
                WorkingDirectory = _options.PythonServicePath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            _pythonProcess = new Process { StartInfo = startInfo };

            // Handle process output
            _pythonProcess.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    _logger.LogInformation("Python service: {Output}", e.Data);
                }
            };

            _pythonProcess.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    _logger.LogError("Python service error: {Error}", e.Data);
                }
            };

            _logger.LogInformation("Starting Python process: {FileName} {Arguments}", startInfo.FileName, startInfo.Arguments);

            _pythonProcess.Start();
            _pythonProcess.BeginOutputReadLine();
            _pythonProcess.BeginErrorReadLine();

            // Monitor process in background
            _ = Task.Run(async () => await MonitorPythonProcessAsync(_cancellationTokenSource.Token));

            // Wait for service to be ready
            await WaitForServiceReadyAsync();
        }

        private async Task MonitorPythonProcessAsync(CancellationToken cancellationToken)
        {
            if (_pythonProcess == null) return;

            try
            {
                await _pythonProcess.WaitForExitAsync(cancellationToken);

                if (!cancellationToken.IsCancellationRequested)
                {
                    _logger.LogError("Python service process exited unexpectedly with code: {ExitCode}",
                        _pythonProcess.ExitCode);

                    // Optionally restart the process
                    if (_options.AutoRestart)
                    {
                        _logger.LogInformation("Auto-restarting Python service...");
                        await Task.Delay(TimeSpan.FromSeconds(_options.RetryDelaySeconds), cancellationToken);
                        await StartPythonProcessAsync();
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Expected when cancellation is requested
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error monitoring Python process");
            }
        }

        private async Task WaitForServiceReadyAsync()
        {
            using var httpClient = new HttpClient();
            var timeout = TimeSpan.FromSeconds(_options.StartupTimeoutSeconds);
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Waiting for Python service to be ready...");

            while (stopwatch.Elapsed < timeout && !_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    var response = await httpClient.GetAsync($"{_options.BaseUrl}/health", _cancellationTokenSource.Token);

                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("Python service is ready and healthy");
                        return;
                    }
                }
                catch
                {
                    // Service not ready yet, continue waiting
                }

                await Task.Delay(TimeSpan.FromSeconds(2), _cancellationTokenSource.Token);
            }

            if (stopwatch.Elapsed >= timeout)
            {
                throw new System.TimeoutException($"Python service did not become ready within {timeout.TotalSeconds} seconds");
            }
        }

        public async Task StopAsync()
        {
            _cancellationTokenSource.Cancel();

            if (_pythonProcess != null && !_pythonProcess.HasExited)
            {
                try
                {
                    _logger.LogInformation("Stopping Python service process...");
                    _pythonProcess.Kill();
                    await _pythonProcess.WaitForExitAsync();
                    _logger.LogInformation("Python service process stopped");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error stopping Python service process");
                }
            }

            // Try to stop Windows service if it's running
            try
            {
                using var service = new ServiceController("LexiFlowTranslationService");
                if (service.Status == ServiceControllerStatus.Running)
                {
                    _logger.LogInformation("Stopping Windows service...");
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                    _logger.LogInformation("Windows service stopped");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not stop Windows service");
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();

            _pythonProcess?.Dispose();

            _disposed = true;
        }
    }

    // Health check for translation service
    public class TranslationHealthCheck : IHealthCheck
    {
        private readonly HttpClient _httpClient;
        private readonly TranslationServiceOptions _options;
        private readonly ILogger<TranslationHealthCheck> _logger;

        public TranslationHealthCheck(HttpClient httpClient, IOptions<TranslationServiceOptions> options, ILogger<TranslationHealthCheck> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_options.BaseUrl}/health", cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync(cancellationToken);

                    return HealthCheckResult.Healthy("Translation service is healthy", new Dictionary<string, object>
                    {
                        ["url"] = _options.BaseUrl,
                        ["status"] = "healthy",
                        ["response"] = content
                    });
                }
                else
                {
                    return HealthCheckResult.Unhealthy($"Translation service returned {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "Translation service health check failed");
                return HealthCheckResult.Unhealthy("Translation service is not reachable", ex);
            }
            catch (TaskCanceledException)
            {
                return HealthCheckResult.Unhealthy("Translation service health check timed out");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Translation service health check error");
                return HealthCheckResult.Unhealthy("Translation service health check failed", ex);
            }
        }
    }
}
