namespace PlayZone.DTOs;

public class WhatsAppMessageDto
{
    public Guid BookingId { get; set; }
}

public class WhatsAppLinkDto
{
    public string WhatsAppUrl { get; set; } = null!;
  public string Message { get; set; } = null!;
}

public class WhatsAppNumberDto
{
    public string BusinessNumber { get; set; } = null!;
}

public class UpdateWhatsAppNumberDto
{
    public string BusinessNumber { get; set; } = null!;
}
