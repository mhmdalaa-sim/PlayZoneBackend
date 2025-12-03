using Microsoft.Extensions.Options;
using PlayZone.Configuration;
using PlayZone.DTOs;
using PlayZone.Repositories;
using PlayZone.Utilities;

namespace PlayZone.Services;

public class AdminService : IAdminService
{
    private readonly IAdminRepository _adminRepository;
  private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
  private readonly IBlockedSlotRepository _blockedSlotRepository;
    private readonly JwtSettings _jwtSettings;

    public AdminService(
  IAdminRepository adminRepository,
  IBookingRepository bookingRepository,
        IRoomRepository roomRepository,
     IBlockedSlotRepository blockedSlotRepository,
   IOptions<JwtSettings> jwtSettings)
    {
        _adminRepository = adminRepository;
_bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
   _blockedSlotRepository = blockedSlotRepository;
_jwtSettings = jwtSettings.Value;
    }

    public async Task<AdminLoginResponseDto?> LoginAsync(string password)
    {
   // Get admin user (username is always "admin")
var admin = await _adminRepository.GetByUsernameAsync("admin");
 if (admin == null)
    return null;

  // Verify password
        if (!BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash))
   return null;

   // Generate JWT token
   var token = JwtHelper.GenerateToken(admin.Id, admin.Username, _jwtSettings);
   var expiresAt = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours);

     return new AdminLoginResponseDto
   {
   Token = token,
    ExpiresAt = expiresAt,
 Username = admin.Username
        };
  }

 public async Task<AdminStatsDto> GetStatisticsAsync()
    {
 var today = DateOnly.FromDateTime(DateTime.Today);
   var weekStart = today.AddDays(-(int)DateTime.Today.DayOfWeek);
   var monthStart = new DateOnly(today.Year, today.Month, 1);

   // Get counts
   var totalBookings = await _bookingRepository.GetTotalCountAsync();
        var bookingsToday = await _bookingRepository.GetCountByDateAsync(today);
  var bookingsThisWeek = await _bookingRepository.GetCountByDateRangeAsync(weekStart, today);
     var bookingsThisMonth = await _bookingRepository.GetCountByDateRangeAsync(monthStart, today);

  // Get room stats
        var allRooms = await _roomRepository.GetAllAsync();
        var totalRooms = allRooms.Count();
   var availableRooms = allRooms.Count(r => r.Available);
 var disabledRooms = totalRooms - availableRooms;

 // Get blocked slots count
        var allBlockedSlots = await _blockedSlotRepository.GetAllAsync();
   var totalBlockedSlots = allBlockedSlots.Count();

     // Calculate total revenue (assuming 100 EGP per hour)
   var allBookings = await _bookingRepository.GetAllAsync();
  var totalRevenue = allBookings.Sum(b => b.Duration * 100);

   // Room booking stats
        var roomStats = allBookings
   .GroupBy(b => b.RoomId)
     .Select(g => new RoomBookingStatsDto
       {
   RoomId = g.Key,
      RoomName = g.First().Room.Name,
      BookingCount = g.Count(),
   TotalHours = g.Sum(b => b.Duration)
       })
     .OrderByDescending(r => r.BookingCount)
       .ToList();

  return new AdminStatsDto
 {
   TotalBookings = totalBookings,
BookingsToday = bookingsToday,
      BookingsThisWeek = bookingsThisWeek,
BookingsThisMonth = bookingsThisMonth,
  TotalRooms = totalRooms,
    AvailableRooms = availableRooms,
   DisabledRooms = disabledRooms,
    TotalBlockedSlots = totalBlockedSlots,
    TotalRevenue = totalRevenue,
     RoomStats = roomStats
   };
  }
}
