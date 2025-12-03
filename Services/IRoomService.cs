using PlayZone.DTOs;
using PlayZone.Models;

namespace PlayZone.Services;

public interface IRoomService
{
    Task<IEnumerable<RoomDto>> GetAllRoomsAsync();
    Task<RoomDto?> GetRoomByIdAsync(int id);
    Task<RoomDto> UpdateRoomAvailabilityAsync(int id, bool available);
    Task<IEnumerable<RoomDto>> GetAvailableRoomsForTimeSlotAsync(DateOnly date, string startTime, string endTime);
    Task<RoomStatusDto> GetRoomStatusAsync(int id, DateOnly date, string startTime, string endTime);
}
