using PlayZone.Models;

namespace PlayZone.Repositories;

public interface IBlockedSlotRepository
{
    Task<IEnumerable<BlockedSlot>> GetAllAsync();
    Task<BlockedSlot?> GetByIdAsync(Guid id);
    Task<BlockedSlot> CreateAsync(BlockedSlot blockedSlot);
    Task DeleteAsync(Guid id);
 Task<IEnumerable<BlockedSlot>> GetByRoomAndDateAsync(int roomId, DateOnly date);
Task<IEnumerable<BlockedSlot>> GetOverlappingSlotsAsync(int roomId, DateOnly date, string startTime, string endTime);
}
