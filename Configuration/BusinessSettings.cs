namespace PlayZone.Configuration;

public class BusinessSettings
{
    public string OpeningTime { get; set; } = "09:00";
    public string ClosingTime { get; set; } = "23:00";
    public int TimeSlotIntervalMinutes { get; set; } = 30;
    public int TotalRooms { get; set; } = 8;
}
