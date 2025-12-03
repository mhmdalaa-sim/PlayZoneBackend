using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayZone.Models;

[Table("blocked_slots")]
public class BlockedSlot
{
    [Key]
    [Column("id")]
 public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Column("room_id")]
    public int RoomId { get; set; }

[Required]
    [Column("date")]
    public DateOnly Date { get; set; }

    [Required]
    [MaxLength(5)]
    [Column("start_time")]
    public string StartTime { get; set; } = null!;

    [Required]
    [MaxLength(5)]
    [Column("end_time")]
    public string EndTime { get; set; } = null!;

    [MaxLength(500)]
    [Column("reason")]
    public string? Reason { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("RoomId")]
    public virtual Room Room { get; set; } = null!;
}
