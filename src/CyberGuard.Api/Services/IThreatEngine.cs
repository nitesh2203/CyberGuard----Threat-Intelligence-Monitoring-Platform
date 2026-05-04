using CyberGuard.Api.Models;

namespace CyberGuard.Api.Services
{
    public interface IThreatEngine
    {
        Task<ThreatAlert?> AnalyzeAsync(NetworkEventDto networkEvent);
    }
}
