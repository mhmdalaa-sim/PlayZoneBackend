using Microsoft.EntityFrameworkCore;
using PlayZone.Data;
using PlayZone.Models;

namespace PlayZone.Repositories;

public class WhatsAppConfigRepository : IWhatsAppConfigRepository
{
    private readonly PlayZoneDbContext _context;

    public WhatsAppConfigRepository(PlayZoneDbContext context)
    {
        _context = context;
    }

    public async Task<WhatsAppConfig?> GetConfigAsync()
    {
   return await _context.WhatsAppConfigs.FirstOrDefaultAsync();
    }

    public async Task<WhatsAppConfig> UpdateConfigAsync(WhatsAppConfig config)
    {
   config.UpdatedAt = DateTime.UtcNow;
     _context.WhatsAppConfigs.Update(config);
    await _context.SaveChangesAsync();
        return config;
    }
}
