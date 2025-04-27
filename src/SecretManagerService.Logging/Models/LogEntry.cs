using System;

namespace SecretManagerService.Logging.Models
{
    public class LogEntry
    {
        public string RequestName { get; set; }
        public object Request { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}