using Microsoft.EntityFrameworkCore;
using PlayZone.Data;
using PlayZone.Models;

namespace PlayZone.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly PlayZoneDbContext _context;

    public BookingRepository(PlayZoneDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Booking>> GetAllAsync()
    {
        return await _context.Bookings
            .Include(b => b.Room)
            .OrderByDescending(b => b.Date)
          .ThenBy(b => b.StartTime)
            .ToListAsync();
    }

    public async Task<Booking?> GetByIdAsync(Guid id)
    {
        return await _context.Bookings
            .Include(b => b.Room)
   .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Booking> CreateAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
  await _context.SaveChangesAsync();
  return await GetByIdAsync(booking.Id) ?? booking;
    }

    public async Task DeleteAsync(Guid id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
    {
            _context.Bookings.Remove(booking);
   await _context.SaveChangesAsync();
    }
    }

    public async Task<IEnumerable<Booking>> GetByDateAsync(DateOnly date)
  {
    return await _context.Bookings
    .Include(b => b.Room)
            .Where(b => b.Date == date)
    .OrderBy(b => b.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetByRoomIdAsync(int roomId)
    {
        return await _context.Bookings
            .Include(b => b.Room)
      .Where(b => b.RoomId == roomId)
   .OrderByDescending(b => b.Date)
            .ThenBy(b => b.StartTime)
         .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetByRoomAndDateAsync(int roomId, DateOnly date)
    {
   return await _context.Bookings
   .Where(b => b.RoomId == roomId && b.Date == date)
         .OrderBy(b => b.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetOverlappingBookingsAsync(int roomId, DateOnly date, string startTime, string endTime)
    {
        return await _context.Bookings
    .Where(b => b.RoomId == roomId 
         && b.Date == date
                && b.StartTime.CompareTo(endTime) < 0
      && b.EndTime.CompareTo(startTime) > 0)
        .ToListAsync();
    }

    public async Task<(IEnumerable<Booking> Bookings, int TotalCount)> GetPaginatedAsync(
     DateOnly? date, int? roomId, int page, int pageSize)
    {
        var query = _context.Bookings.Include(b => b.Room).AsQueryable();

        if (date.HasValue)
query = query.Where(b => b.Date == date.Value);

        if (roomId.HasValue)
     query = query.Where(b => b.RoomId == roomId.Value);

   var totalCount = await query.CountAsync();

        var bookings = await query
  .OrderByDescending(b => b.Date)
            .ThenBy(b => b.StartTime)
      .Skip((page - 1) * pageSize)
   .Take(pageSize)
        .ToListAsync();

        return (bookings, totalCount);
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Bookings.CountAsync();
    }

    public async Task<int> GetCountByDateAsync(DateOnly date)
    {
        return await _context.Bookings.CountAsync(b => b.Date == date);
    }

    public async Task<int> GetCountByDateRangeAsync(DateOnly startDate, DateOnly endDate)
    {
   return await _context.Bookings
            .CountAsync(b => b.Date >= startDate && b.Date <= endDate);
    }
}
