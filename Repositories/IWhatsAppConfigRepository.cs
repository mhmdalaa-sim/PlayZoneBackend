using PlayZone.Models;

namespace PlayZone.Repositories;

public interface IWhatsAppConfigRepository
{
    Task<WhatsAppConfig?> GetConfigAsync();
    Task<WhatsAppConfig> UpdateConfigAsync(WhatsAppConfig config);
}
