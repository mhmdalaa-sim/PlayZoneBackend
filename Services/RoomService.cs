using PlayZone.DTOs;
using PlayZone.Models;
using PlayZone.Repositories;

namespace PlayZone.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IBlockedSlotRepository _blockedSlotRepository;

    public RoomService(
        IRoomRepository roomRepository,
        IBookingRepository bookingRepository,
  IBlockedSlotRepository blockedSlotRepository)
    {
        _roomRepository = roomRepository;
        _bookingRepository = bookingRepository;
        _blockedSlotRepository = blockedSlotRepository;
    }

  public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
    {
 var rooms = await _roomRepository.GetAllAsync();
   return rooms.Select(MapToDto);
    }

    public async Task<RoomDto?> GetRoomByIdAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
   return room != null ? MapToDto(room) : null;
  }

    public async Task<RoomDto> UpdateRoomAvailabilityAsync(int id, bool available)
  {
        var room = await _roomRepository.GetByIdAsync(id);
   if (room == null)
    throw new KeyNotFoundException($"Room with ID {id} not found");

        room.Available = available;
        var updatedRoom = await _roomRepository.UpdateAsync(room);
     return MapToDto(updatedRoom);
  }

    public async Task<IEnumerable<RoomDto>> GetAvailableRoomsForTimeSlotAsync(
      DateOnly date, string startTime, string endTime)
  {
      var allRooms = await _roomRepository.GetAvailableRoomsAsync();
     var availableRooms = new List<RoomDto>();

 foreach (var room in allRooms)
        {
   var overlappingBookings = await _bookingRepository
                .GetOverlappingBookingsAsync(room.Id, date, startTime, endTime);
        
   var overlappingSlots = await _blockedSlotRepository
       .GetOverlappingSlotsAsync(room.Id, date, startTime, endTime);

      if (!overlappingBookings.Any() && !overlappingSlots.Any())
         {
    availableRooms.Add(MapToDto(room));
         }
      }

     return availableRooms;
    }

    public async Task<RoomStatusDto> GetRoomStatusAsync(
   int id, DateOnly date, string startTime, string endTime)
    {
   var room = await _roomRepository.GetByIdAsync(id);
  if (room == null)
            throw new KeyNotFoundException($"Room with ID {id} not found");

        var overlappingBookings = await _bookingRepository
      .GetOverlappingBookingsAsync(id, date, startTime, endTime);
        
        var overlappingSlots = await _blockedSlotRepository
     .GetOverlappingSlotsAsync(id, date, startTime, endTime);

     var isAvailable = room.Available 
            && !overlappingBookings.Any() 
            && !overlappingSlots.Any();

   return new RoomStatusDto
      {
            RoomId = room.Id,
   RoomName = room.Name,
            IsAvailable = isAvailable,
       IsDisabled = !room.Available,
     ExistingBookings = overlappingBookings.Select(MapBookingToDto).ToList(),
BlockedSlots = overlappingSlots.Select(MapBlockedSlotToDto).ToList()
     };
    }

    private static RoomDto MapToDto(Room room)
    {
  return new RoomDto
    {
 Id = room.Id,
            Name = room.Name,
       Description = room.Description,
            Available = room.Available,
     CreatedAt = room.CreatedAt,
     UpdatedAt = room.UpdatedAt
    };
  }

    private static BookingDto MapBookingToDto(Booking booking)
    {
      return new BookingDto
        {
            Id = booking.Id,
    RoomId = booking.RoomId,
  RoomName = booking.Room.Name,
            Date = booking.Date.ToString("yyyy-MM-dd"),
       StartTime = booking.StartTime,
    EndTime = booking.EndTime,
      Duration = booking.Duration,
      User = new UserInfoDto
            {
      Name = booking.CustomerName,
  Phone = booking.CustomerPhone
            },
  Notes = booking.Notes,
 CreatedAt = booking.CreatedAt
        };
 }

    private static BlockedSlotDto MapBlockedSlotToDto(BlockedSlot slot)
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
