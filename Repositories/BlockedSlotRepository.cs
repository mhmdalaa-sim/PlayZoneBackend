using Microsoft.EntityFrameworkCore;
using PlayZone.Data;
using PlayZone.Models;

namespace PlayZone.Repositories;

public class BlockedSlotRepository : IBlockedSlotRepository
{
    private readonly PlayZoneDbContext _context;

    public BlockedSlotRepository(PlayZoneDbContext context)
    {
    _context = context;
    }

    public async Task<IEnumerable<BlockedSlot>> GetAllAsync()
    {
        return await _context.BlockedSlots
     .Include(bs => bs.Room)
      .OrderByDescending(bs => bs.Date)
    .ThenBy(bs => bs.StartTime)
            .ToListAsync();
  }

    public async Task<BlockedSlot?> GetByIdAsync(Guid id)
  {
        return await _context.BlockedSlots
       .Include(bs => bs.Room)
   .FirstOrDefaultAsync(bs => bs.Id == id);
    }

    public async Task<BlockedSlot> CreateAsync(BlockedSlot blockedSlot)
    {
await _context.BlockedSlots.AddAsync(blockedSlot);
     await _context.SaveChangesAsync();
        return await GetByIdAsync(blockedSlot.Id) ?? blockedSlot;
    }

    public async Task DeleteAsync(Guid id)
    {
        var blockedSlot = await _context.BlockedSlots.FindAsync(id);
     if (blockedSlot != null)
        {
      _context.BlockedSlots.Remove(blockedSlot);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<BlockedSlot>> GetByRoomAndDateAsync(int roomId, DateOnly date)
    {
        return await _context.BlockedSlots
    .Where(bs => bs.RoomId == roomId && bs.Date == date)
   .OrderBy(bs => bs.StartTime)
   .ToListAsync();
    }

    public async Task<IEnumerable<BlockedSlot>> GetOverlappingSlotsAsync(
   int roomId, DateOnly date, string startTime, string endTime)
 {
   return await _context.BlockedSlots
      .Where(bs => bs.RoomId == roomId 
    && bs.Date == date
    && bs.StartTime.CompareTo(endTime) < 0
      && bs.EndTime.CompareTo(startTime) > 0)
       .ToListAsync();
    }
}
