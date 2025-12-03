using PlayZone.DTOs;

namespace PlayZone.Services;

public interface IBookingService
{
    Task<PaginatedResponse<BookingDto>> GetAllBookingsAsync(BookingFilterDto filter);
    Task<BookingDto?> GetBookingByIdAsync(Guid id);
    Task<BookingDto> CreateBookingAsync(CreateBookingDto dto);
    Task DeleteBookingAsync(Guid id);
    Task<IEnumerable<BookingDto>> GetBookingsByDateAsync(string date);
  Task<IEnumerable<BookingDto>> GetBookingsByRoomIdAsync(int roomId);
}
