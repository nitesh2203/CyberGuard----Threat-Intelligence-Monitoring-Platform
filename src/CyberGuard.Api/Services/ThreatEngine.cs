using CyberGuard.Api.Data;
using CyberGuard.Api.Models;
using Microsoft.AspNetCore.SignalR;

namespace CyberGuard.Api.Services
{
    public class ThreatEngine : IThreatEngine
    {
        private static readonly string[] BlockedIps =
        {
            "192.168.1.100",
            "10.0.0.55",
            "203.0.113.42"
        };

        private static readonly string[] SuspiciousEvents =
        {
            "PortScan",
            "FailedLogin",
            "MalwareSignature",
            "DataExfiltration"
        };

        private readonly CyberGuardDbContext _dbContext;
        private readonly IHubContext<Hubs.ThreatHub> _hubContext;

        public ThreatEngine(CyberGuardDbContext dbContext, IHubContext<Hubs.ThreatHub> hubContext)
        {
            _dbContext = dbContext;
            _hubContext = hubContext;
        }

        public async Task<ThreatAlert?> AnalyzeAsync(NetworkEventDto networkEvent)
        {
            var networkRecord = new NetworkEvent
            {
                Ip = networkEvent.Ip,
                EventType = networkEvent.EventType,
                Source = networkEvent.Source,
                Timestamp = networkEvent.Timestamp
            };

            _dbContext.NetworkEvents.Add(networkRecord);
            await _dbContext.SaveChangesAsync();

            var alertReason = GetAlertReason(networkEvent);
            if (alertReason == null)
            {
                return null;
            }

            var alert = new ThreatAlert
            {
                Ip = networkEvent.Ip,
                EventType = networkEvent.EventType,
                Severity = GetSeverity(networkEvent),
                Reason = alertReason,
                DetectedAt = DateTime.UtcNow
            };

            _dbContext.ThreatAlerts.Add(alert);
            await _dbContext.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("NewThreat", alert);

            return alert;
        }

        private static string? GetAlertReason(NetworkEventDto networkEvent)
        {
            if (BlockedIps.Contains(networkEvent.Ip))
            {
                return "Blocked IP detected";
            }

            if (SuspiciousEvents.Contains(networkEvent.EventType))
            {
                return "Suspicious event type";
            }

            if (networkEvent.Timestamp < DateTime.UtcNow.AddMinutes(-5))
            {
                return "Delayed event delivery";
            }

            return null;
        }

        private static string GetSeverity(NetworkEventDto networkEvent)
        {
            if (BlockedIps.Contains(networkEvent.Ip) || networkEvent.EventType == "MalwareSignature")
            {
                return "Critical";
            }

            if (networkEvent.EventType == "FailedLogin" || networkEvent.EventType == "PortScan")
            {
                return "High";
            }

            return "Medium";
        }
    }
}
