namespace CyberGuard.Api.Models
{
    public class NetworkEvent
    {
        public int Id { get; set; }
        public string Ip { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
