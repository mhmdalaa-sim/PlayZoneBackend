using PlayZone.DTOs;

namespace PlayZone.Services;

public interface IBlockedSlotService
{
    Task<IEnumerable<BlockedSlotDto>> GetAllBlockedSlotsAsync();
    Task<BlockedSlotDto?> GetBlockedSlotByIdAsync(Guid id);
    Task<BlockedSlotDto> CreateBlockedSlotAsync(CreateBlockedSlotDto dto);
    Task DeleteBlockedSlotAsync(Guid id);
    Task<IEnumerable<BlockedSlotDto>> GetBlockedSlotsByRoomAndDateAsync(int roomId, string date);
}
