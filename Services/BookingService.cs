using PlayZone.DTOs;
using PlayZone.Models;
using PlayZone.Repositories;
using PlayZone.Utilities;

namespace PlayZone.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
 private readonly IBlockedSlotRepository _blockedSlotRepository;
    private readonly IValidationService _validationService;

    public BookingService(
 IBookingRepository bookingRepository,
  IRoomRepository roomRepository,
      IBlockedSlotRepository blockedSlotRepository,
  IValidationService validationService)
    {
   _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
   _blockedSlotRepository = blockedSlotRepository;
        _validationService = validationService;
    }

 public async Task<PaginatedResponse<BookingDto>> GetAllBookingsAsync(BookingFilterDto filter)
    {
   DateOnly? date = null;
  if (!string.IsNullOrEmpty(filter.Date))
 {
            if (!DateOnly.TryParse(filter.Date, out var parsedDate))
          throw new ArgumentException("Invalid date format. Use YYYY-MM-DD");
   date = parsedDate;
        }

        var (bookings, totalCount) = await _bookingRepository.GetPaginatedAsync(
     date, filter.RoomId, filter.Page, filter.PageSize);

   var bookingDtos = bookings.Select(MapToDto).ToList();

        return new PaginatedResponse<BookingDto>
        {
      Success = true,
    Data = bookingDtos,
    Pagination = new PaginationMetadata
   {
    Page = filter.Page,
          PageSize = filter.PageSize,
            TotalCount = totalCount,
   TotalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize)
   }
     };
 }

    public async Task<BookingDto?> GetBookingByIdAsync(Guid id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);
   return booking != null ? MapToDto(booking) : null;
 }

    public async Task<BookingDto> CreateBookingAsync(CreateBookingDto dto)
    {
   // Parse and validate date
    if (!DateOnly.TryParse(dto.Date, out var bookingDate))
 throw new ArgumentException("Invalid date format. Use YYYY-MM-DD");

   // Validate booking
   _validationService.ValidateBookingDate(bookingDate);
  _validationService.ValidateTime(dto.StartTime, dto.EndTime);

  // Check if room exists and is available
        var room = await _roomRepository.GetByIdAsync(dto.RoomId);
   if (room == null)
     throw new KeyNotFoundException($"Room with ID {dto.RoomId} not found");

 if (!room.Available)
    throw new InvalidOperationException($"Room {room.Name} is currently disabled");

   // Check for overlapping bookings
   var overlappingBookings = await _bookingRepository
      .GetOverlappingBookingsAsync(dto.RoomId, bookingDate, dto.StartTime, dto.EndTime);

        if (overlappingBookings.Any())
     {
throw new InvalidOperationException(
         $"Room {room.Name} is already booked for the selected time slot");
      }

 // Check for blocked slots
        var overlappingSlots = await _blockedSlotRepository
       .GetOverlappingSlotsAsync(dto.RoomId, bookingDate, dto.StartTime, dto.EndTime);

   if (overlappingSlots.Any())
   {
     throw new InvalidOperationException(
         $"The selected time slot is blocked for maintenance");
 }

 // Calculate duration
     var duration = TimeHelper.CalculateDuration(dto.StartTime, dto.EndTime);

    // Create booking
   var booking = new Booking
   {
      Id = Guid.NewGuid(),
      RoomId = dto.RoomId,
   Date = bookingDate,
 StartTime = dto.StartTime,
 EndTime = dto.EndTime,
   Duration = duration,
     CustomerName = dto.User.Name,
   CustomerPhone = dto.User.Phone,
     Notes = dto.Notes,
   CreatedAt = DateTime.UtcNow
};

   var createdBooking = await _bookingRepository.CreateAsync(booking);
   return MapToDto(createdBooking);
    }

    public async Task DeleteBookingAsync(Guid id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);
   if (booking == null)
     throw new KeyNotFoundException($"Booking with ID {id} not found");

   await _bookingRepository.DeleteAsync(id);
}

    public async Task<IEnumerable<BookingDto>> GetBookingsByDateAsync(string date)
    {
 if (!DateOnly.TryParse(date, out var bookingDate))
        throw new ArgumentException("Invalid date format. Use YYYY-MM-DD");

 var bookings = await _bookingRepository.GetByDateAsync(bookingDate);
        return bookings.Select(MapToDto);
    }

 public async Task<IEnumerable<BookingDto>> GetBookingsByRoomIdAsync(int roomId)
    {
        var room = await _roomRepository.GetByIdAsync(roomId);
        if (room == null)
       throw new KeyNotFoundException($"Room with ID {roomId} not found");

   var bookings = await _bookingRepository.GetByRoomIdAsync(roomId);
   return bookings.Select(MapToDto);
    }

    private static BookingDto MapToDto(Booking booking)
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
}
