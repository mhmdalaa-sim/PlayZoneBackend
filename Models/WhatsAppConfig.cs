using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayZone.Models;

[Table("whatsapp_config")]
public class WhatsAppConfig
{
    [Key]
    [Column("id")]
    public int Id { get; set; } = 1;

    [Required]
    [MaxLength(20)]
    [Column("business_number")]
    public string BusinessNumber { get; set; } = null!;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
