namespace LexiFlow.TranslationAPI.Configuration
{
    public class TranslationServiceOptions
    {
        public const string SectionName = "TranslationService";

        public string BaseUrl { get; set; } = "http://127.0.0.1:5001";
        public int TimeoutSeconds { get; set; } = 30;
        public int MaxRetries { get; set; } = 3;
        public int RetryDelaySeconds { get; set; } = 2;
        public bool EnableHealthCheck { get; set; } = true;
        public string PythonServicePath { get; set; } = @"C:\LexiFlow\TranslationService";
        public string PythonExecutable { get; set; } = "python";
        public string ServiceScript { get; set; } = "m2m100_service.py";
        public int StartupTimeoutSeconds { get; set; } = 120;
        public bool AutoRestart { get; set; } = true;
    }
}
