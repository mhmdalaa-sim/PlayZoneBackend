using PlayZone.Models;

namespace PlayZone.Repositories;

public interface IBookingRepository
{
    Task<IEnumerable<Booking>> GetAllAsync();
    Task<Booking?> GetByIdAsync(Guid id);
    Task<Booking> CreateAsync(Booking booking);
 Task DeleteAsync(Guid id);
    Task<IEnumerable<Booking>> GetByDateAsync(DateOnly date);
    Task<IEnumerable<Booking>> GetByRoomIdAsync(int roomId);
Task<IEnumerable<Booking>> GetByRoomAndDateAsync(int roomId, DateOnly date);
    Task<IEnumerable<Booking>> GetOverlappingBookingsAsync(int roomId, DateOnly date, string startTime, string endTime);
 Task<(IEnumerable<Booking> Bookings, int TotalCount)> GetPaginatedAsync(DateOnly? date, int? roomId, int page, int pageSize);
    Task<int> GetTotalCountAsync();
    Task<int> GetCountByDateAsync(DateOnly date);
    Task<int> GetCountByDateRangeAsync(DateOnly startDate, DateOnly endDate);
}
