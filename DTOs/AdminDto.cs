namespace PlayZone.DTOs;

public class AdminLoginDto
{
    public string Password { get; set; } = null!;
}

public class AdminLoginResponseDto
{
    public string Token { get; set; } = null!;
 public DateTime ExpiresAt { get; set; }
    public string Username { get; set; } = null!;
}

public class AdminStatsDto
{
    public int TotalBookings { get; set; }
    public int BookingsToday { get; set; }
    public int BookingsThisWeek { get; set; }
    public int BookingsThisMonth { get; set; }
    public int TotalRooms { get; set; }
    public int AvailableRooms { get; set; }
    public int DisabledRooms { get; set; }
    public int TotalBlockedSlots { get; set; }
  public decimal TotalRevenue { get; set; }
    public List<RoomBookingStatsDto>? RoomStats { get; set; }
}

public class RoomBookingStatsDto
{
    public int RoomId { get; set; }
    public string RoomName { get; set; } = null!;
    public int BookingCount { get; set; }
    public decimal TotalHours { get; set; }
}
