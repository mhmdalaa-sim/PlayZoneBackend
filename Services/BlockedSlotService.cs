using PlayZone.DTOs;
using PlayZone.Models;
using PlayZone.Repositories;
using PlayZone.Utilities;

namespace PlayZone.Services;

public class BlockedSlotService : IBlockedSlotService
{
    private readonly IBlockedSlotRepository _blockedSlotRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IValidationService _validationService;

    public BlockedSlotService(
 IBlockedSlotRepository blockedSlotRepository,
 IRoomRepository roomRepository,
    IValidationService validationService)
    {
  _blockedSlotRepository = blockedSlotRepository;
     _roomRepository = roomRepository;
     _validationService = validationService;
 }

    public async Task<IEnumerable<BlockedSlotDto>> GetAllBlockedSlotsAsync()
    {
        var slots = await _blockedSlotRepository.GetAllAsync();
        return slots.Select(MapToDto);
 }

    public async Task<BlockedSlotDto?> GetBlockedSlotByIdAsync(Guid id)
    {
   var slot = await _blockedSlotRepository.GetByIdAsync(id);
return slot != null ? MapToDto(slot) : null;
    }

 public async Task<BlockedSlotDto> CreateBlockedSlotAsync(CreateBlockedSlotDto dto)
    {
        // Parse and validate date
   if (!DateOnly.TryParse(dto.Date, out var slotDate))
     throw new ArgumentException("Invalid date format. Use YYYY-MM-DD");

 // Validate
   _validationService.ValidateBookingDate(slotDate);
   _validationService.ValidateTime(dto.StartTime, dto.EndTime);

  // Check if room exists
        var room = await _roomRepository.GetByIdAsync(dto.RoomId);
        if (room == null)
   throw new KeyNotFoundException($"Room with ID {dto.RoomId} not found");

   // Create blocked slot
 var blockedSlot = new BlockedSlot
    {
   Id = Guid.NewGuid(),
    RoomId = dto.RoomId,
      Date = slotDate,
     StartTime = dto.StartTime,
  EndTime = dto.EndTime,
  Reason = dto.Reason,
 CreatedAt = DateTime.UtcNow
 };

        var createdSlot = await _blockedSlotRepository.CreateAsync(blockedSlot);
 return MapToDto(createdSlot);
    }

    public async Task DeleteBlockedSlotAsync(Guid id)
  {
     var slot = await _blockedSlotRepository.GetByIdAsync(id);
   if (slot == null)
      throw new KeyNotFoundException($"Blocked slot with ID {id} not found");

  await _blockedSlotRepository.DeleteAsync(id);
 }

    public async Task<IEnumerable<BlockedSlotDto>> GetBlockedSlotsByRoomAndDateAsync(int roomId, string date)
  {
  if (!DateOnly.TryParse(date, out var slotDate))
  throw new ArgumentException("Invalid date format. Use YYYY-MM-DD");

        var room = await _roomRepository.GetByIdAsync(roomId);
        if (room == null)
     throw new KeyNotFoundException($"Room with ID {roomId} not found");

        var slots = await _blockedSlotRepository.GetByRoomAndDateAsync(roomId, slotDate);
        return slots.Select(MapToDto);
  }

 private static BlockedSlotDto MapToDto(BlockedSlot slot)
    {
   return new BlockedSlotDto
        {
       Id = slot.Id,
 RoomId = slot.RoomId,
      RoomName = slot.Room.Name,
         Date = slot.Date.ToString("yyyy-MM-dd"),
   StartTime = slot.StartTime,
     EndTime = slot.EndTime,
  Reason = slot.Reason,
      CreatedAt = slot.CreatedAt
     };
    }
}
