using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayZone.Models;

[Table("admin_users")]
public class AdminUser
{
    [Key]
    [Column("id")]
public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    [Column("username")]
    public string Username { get; set; } = null!;

    [Required]
    [Column("password_hash")]
    public string PasswordHash { get; set; } = null!;

    [Column("created_at")]
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
