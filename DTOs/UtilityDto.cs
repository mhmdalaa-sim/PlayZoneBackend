namespace PlayZone.DTOs;

public class TimeSlotDto
{
    public string Time { get; set; } = null!;
    public string Display { get; set; } = null!;
}

public class AvailableRoomsQueryDto
{
    public string Date { get; set; } = null!;
    public string StartTime { get; set; } = null!;
    public string EndTime { get; set; } = null!;
}
