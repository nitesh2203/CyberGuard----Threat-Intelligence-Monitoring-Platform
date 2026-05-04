using CyberGuard.Api.Models;
using CyberGuard.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CyberGuard.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NetworkEventController : ControllerBase
    {
        private readonly IThreatEngine _threatEngine;

        public NetworkEventController(IThreatEngine threatEngine)
        {
            _threatEngine = threatEngine;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NetworkEventDto networkEvent)
        {
            if (string.IsNullOrWhiteSpace(networkEvent.Ip) || string.IsNullOrWhiteSpace(networkEvent.EventType))
            {
                return BadRequest(new { Error = "Ip and EventType are required." });
            }

            var alert = await _threatEngine.AnalyzeAsync(networkEvent);
            if (alert == null)
            {
                return Ok(new { Message = "Event recorded, no threat detected." });
            }

            return Ok(alert);
        }
    }
}
