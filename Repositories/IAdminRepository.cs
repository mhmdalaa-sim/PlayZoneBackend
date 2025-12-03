using PlayZone.Models;

namespace PlayZone.Repositories;

public interface IAdminRepository
{
    Task<AdminUser?> GetByUsernameAsync(string username);
    Task<AdminUser?> GetByIdAsync(Guid id);
}
