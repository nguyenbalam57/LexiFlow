using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace LexiFlow.TranslationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;
        private readonly ILogger<HealthController> _logger;

        public HealthController(HealthCheckService healthCheckService, ILogger<HealthController> logger)
        {
            _healthCheckService = healthCheckService;
            _logger = logger;
        }

        /// <summary>
        /// Get overall health status of the API
        /// </summary>
        /// <returns>Health status including all dependencies</returns>
        [HttpGet]
        [ProducesResponseType(typeof(HealthStatusResponse), 200)]
        [ProducesResponseType(typeof(HealthStatusResponse), 503)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var report = await _healthCheckService.CheckHealthAsync();

                var response = new HealthStatusResponse
                {
                    Status = report.Status.ToString(),
                    TotalDuration = report.TotalDuration,
                    Checks = report.Entries.Select(x => new HealthCheckResponse
                    {
                        Name = x.Key,
                        Status = x.Value.Status.ToString(),
                        Duration = x.Value.Duration,
                        Description = x.Value.Description,
                        Data = x.Value.Data?.ToDictionary(k => k.Key, v => v.Value?.ToString())
                    }).ToList(),
                    SystemInfo = BuildSystemInfo() // Đổi tên method
                };

                return report.Status == HealthStatus.Healthy
                    ? Ok(response)
                    : StatusCode(503, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed");

                var errorResponse = new HealthStatusResponse
                {
                    Status = "Error",
                    TotalDuration = TimeSpan.Zero,
                    Checks = new List<HealthCheckResponse>(),
                    Error = ex.Message,
                    SystemInfo = BuildSystemInfo() // Đổi tên method
                };

                return StatusCode(503, errorResponse);
            }
        }

        /// <summary>
        /// Get basic system information
        /// </summary>
        /// <returns>System information</returns>
        [HttpGet("info")]
        [ProducesResponseType(typeof(SystemInfo), 200)]
        public IActionResult GetInfo() // Đổi tên method
        {
            var systemInfo = BuildSystemInfo();
            return Ok(systemInfo);
        }

        /// <summary>
        /// Get API version information
        /// </summary>
        /// <returns>Version information</returns>
        [HttpGet("version")]
        [ProducesResponseType(typeof(VersionInfo), 200)]
        public IActionResult GetVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;

            return Ok(new VersionInfo
            {
                Version = version?.ToString() ?? "1.0.0",
                BuildDate = GetBuildDate(assembly),
                Environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
                Framework = System.Environment.Version.ToString(),
                MachineName = System.Environment.MachineName,
                ProcessorCount = System.Environment.ProcessorCount
            });
        }

        /// <summary>
        /// Get detailed system metrics
        /// </summary>
        /// <returns>Detailed system metrics</returns>
        [HttpGet("metrics")]
        [ProducesResponseType(typeof(SystemMetrics), 200)]
        public IActionResult GetMetrics()
        {
            var process = Process.GetCurrentProcess();

            return Ok(new SystemMetrics
            {
                CpuUsage = GetCpuUsage(),
                MemoryUsage = new MemoryMetrics
                {
                    WorkingSet = process.WorkingSet64,
                    PrivateMemory = process.PrivateMemorySize64,
                    VirtualMemory = process.VirtualMemorySize64,
                    GcTotalMemory = GC.GetTotalMemory(false)
                },
                ThreadCount = process.Threads.Count,
                HandleCount = process.HandleCount,
                GcCollections = new GcMetrics
                {
                    Gen0 = GC.CollectionCount(0),
                    Gen1 = GC.CollectionCount(1),
                    Gen2 = GC.CollectionCount(2)
                }
            });
        }

        private SystemInfo BuildSystemInfo() // Method riêng để tránh trùng tên
        {
            var process = Process.GetCurrentProcess();

            return new SystemInfo
            {
                MachineName = System.Environment.MachineName,
                OperatingSystem = System.Environment.OSVersion.ToString(),
                ProcessorCount = System.Environment.ProcessorCount,
                WorkingSet = process.WorkingSet64,
                StartTime = process.StartTime,
                Uptime = DateTime.UtcNow - process.StartTime.ToUniversalTime(),
                IsRunningInContainer = System.Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true",
                Framework = System.Environment.Version.ToString()
            };
        }

        private static DateTime GetBuildDate(Assembly assembly)
        {
            const string buildVersionMetadataPrefix = "+build";

            var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            if (attribute?.InformationalVersion != null)
            {
                var value = attribute.InformationalVersion;
                var index = value.IndexOf(buildVersionMetadataPrefix);
                if (index > 0)
                {
                    value = value[(index + buildVersionMetadataPrefix.Length)..];
                    if (DateTime.TryParseExact(value, "yyyyMMddHHmmss", null, DateTimeStyles.None, out var result))
                        return result;
                }
            }

            return new FileInfo(assembly.Location).CreationTime;
        }

        private static double GetCpuUsage()
        {
            try
            {
                var process = Process.GetCurrentProcess();
                var startTime = DateTime.UtcNow;
                var startCpuUsage = process.TotalProcessorTime;

                Thread.Sleep(500); // Sample for 500ms

                var endTime = DateTime.UtcNow;
                var endCpuUsage = process.TotalProcessorTime;

                var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
                var totalMsPassed = (endTime - startTime).TotalMilliseconds;

                var cpuUsageTotal = cpuUsedMs / (System.Environment.ProcessorCount * totalMsPassed);

                return Math.Round(cpuUsageTotal * 100, 2);
            }
            catch
            {
                return 0.0;
            }
        }
    }

    // Response models
    public class HealthStatusResponse
    {
        public string Status { get; set; } = string.Empty;
        public TimeSpan TotalDuration { get; set; }
        public List<HealthCheckResponse> Checks { get; set; } = new();
        public string? Error { get; set; }
        public SystemInfo SystemInfo { get; set; } = new();
    }

    public class HealthCheckResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public string? Description { get; set; }
        public Dictionary<string, string?>? Data { get; set; }
    }

    public class SystemInfo
    {
        public string MachineName { get; set; } = string.Empty;
        public string OperatingSystem { get; set; } = string.Empty;
        public int ProcessorCount { get; set; }
        public long WorkingSet { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Uptime { get; set; }
        public bool IsRunningInContainer { get; set; }
        public string Framework { get; set; } = string.Empty;
    }

    public class VersionInfo
    {
        public string Version { get; set; } = string.Empty;
        public DateTime BuildDate { get; set; }
        public string Environment { get; set; } = string.Empty;
        public string Framework { get; set; } = string.Empty;
        public string MachineName { get; set; } = string.Empty;
        public int ProcessorCount { get; set; }
    }

    public class SystemMetrics
    {
        public double CpuUsage { get; set; }
        public MemoryMetrics MemoryUsage { get; set; } = new();
        public int ThreadCount { get; set; }
        public int HandleCount { get; set; }
        public GcMetrics GcCollections { get; set; } = new();
    }

    public class MemoryMetrics
    {
        public long WorkingSet { get; set; }
        public long PrivateMemory { get; set; }
        public long VirtualMemory { get; set; }
        public long GcTotalMemory { get; set; }
    }

    public class GcMetrics
    {
        public int Gen0 { get; set; }
        public int Gen1 { get; set; }
        public int Gen2 { get; set; }
    }
}
