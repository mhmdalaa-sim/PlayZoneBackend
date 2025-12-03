using PlayZone.DTOs;

namespace PlayZone.Services;

public interface IAdminService
{
    Task<AdminLoginResponseDto?> LoginAsync(string password);
    Task<AdminStatsDto> GetStatisticsAsync();
}
