namespace PlayZone.DTOs;

public class RoomDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool Available { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UpdateRoomAvailabilityDto
{
    public bool Available { get; set; }
}

public class RoomStatusDto
{
    public int RoomId { get; set; }
    public string RoomName { get; set; } = null!;
    public bool IsAvailable { get; set; }
    public bool IsDisabled { get; set; }
    public List<BookingDto>? ExistingBookings { get; set; }
    public List<BlockedSlotDto>? BlockedSlots { get; set; }
}
