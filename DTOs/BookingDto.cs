namespace PlayZone.DTOs;

public class BookingDto
{
public Guid Id { get; set; }
    public int RoomId { get; set; }
    public string RoomName { get; set; } = null!;
    public string Date { get; set; } = null!;
    public string StartTime { get; set; } = null!;
    public string EndTime { get; set; } = null!;
    public decimal Duration { get; set; }
    public UserInfoDto User { get; set; } = null!;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateBookingDto
{
    public int RoomId { get; set; }
    public string Date { get; set; } = null!;
    public string StartTime { get; set; } = null!;
    public string EndTime { get; set; } = null!;
    public UserInfoDto User { get; set; } = null!;
    public string? Notes { get; set; }
}

public class UserInfoDto
{
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
}

public class BookingFilterDto
{
    public string? Date { get; set; }
  public int? RoomId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class PaginatedResponse<T>
{
    public bool Success { get; set; }
    public List<T> Data { get; set; } = new();
    public PaginationMetadata Pagination { get; set; } = null!;
}

public class PaginationMetadata
{
    public int Page { get; set; }
  public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
