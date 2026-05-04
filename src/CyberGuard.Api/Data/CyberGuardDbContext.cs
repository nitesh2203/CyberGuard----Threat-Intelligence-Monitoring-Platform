using CyberGuard.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberGuard.Api.Data
{
    public class CyberGuardDbContext : DbContext
    {
        public CyberGuardDbContext(DbContextOptions<CyberGuardDbContext> options) : base(options)
        {
        }

        public DbSet<NetworkEvent> NetworkEvents => Set<NetworkEvent>();
        public DbSet<ThreatAlert> ThreatAlerts => Set<ThreatAlert>();
    }
}
