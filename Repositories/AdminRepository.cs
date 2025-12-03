using Microsoft.EntityFrameworkCore;
using PlayZone.Data;
using PlayZone.Models;

namespace PlayZone.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly PlayZoneDbContext _context;

  public AdminRepository(PlayZoneDbContext context)
    {
_context = context;
    }

    public async Task<AdminUser?> GetByUsernameAsync(string username)
    {
  return await _context.AdminUsers
            .FirstOrDefaultAsync(a => a.Username == username);
    }

    public async Task<AdminUser?> GetByIdAsync(Guid id)
    {
   return await _context.AdminUsers.FindAsync(id);
    }
}
