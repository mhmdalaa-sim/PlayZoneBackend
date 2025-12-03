namespace PlayZone.DTOs;

public class BlockedSlotDto
{
 public Guid Id { get; set; }
    public int RoomId { get; set; }
    public string RoomName { get; set; } = null!;
    public string Date { get; set; } = null!;
    public string StartTime { get; set; } = null!;
public string EndTime { get; set; } = null!;
    public string? Reason { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateBlockedSlotDto
{
    public int RoomId { get; set; }
  public string Date { get; set; } = null!;
    public string StartTime { get; set; } = null!;
    public string EndTime { get; set; } = null!;
    public string? Reason { get; set; }
}
