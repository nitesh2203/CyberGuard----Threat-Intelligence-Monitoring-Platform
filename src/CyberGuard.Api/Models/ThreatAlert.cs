namespace CyberGuard.Api.Models
{
    public class ThreatAlert
    {
        public int Id { get; set; }
        public string Ip { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public DateTime DetectedAt { get; set; } = DateTime.UtcNow;
    }
}
