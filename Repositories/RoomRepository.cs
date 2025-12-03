using Microsoft.EntityFrameworkCore;
using PlayZone.Data;
using PlayZone.Models;

namespace PlayZone.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly PlayZoneDbContext _context;

    public RoomRepository(PlayZoneDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Room>> GetAllAsync()
    {
   return await _context.Rooms
            .OrderBy(r => r.Id)
   .ToListAsync();
    }

    public async Task<Room?> GetByIdAsync(int id)
    {
 return await _context.Rooms.FindAsync(id);
    }

    public async Task<Room> UpdateAsync(Room room)
    {
        room.UpdatedAt = DateTime.UtcNow;
      _context.Rooms.Update(room);
await _context.SaveChangesAsync();
        return room;
  }

    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync()
    {
      return await _context.Rooms
            .Where(r => r.Available)
  .OrderBy(r => r.Id)
            .ToListAsync();
    }
}
