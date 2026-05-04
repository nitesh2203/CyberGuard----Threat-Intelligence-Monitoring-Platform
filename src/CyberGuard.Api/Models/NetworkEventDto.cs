namespace CyberGuard.Api.Models
{
    public class NetworkEventDto
    {
        public string Ip { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
