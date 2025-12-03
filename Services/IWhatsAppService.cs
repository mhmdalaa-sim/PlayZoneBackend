using PlayZone.DTOs;

namespace PlayZone.Services;

public interface IWhatsAppService
{
    Task<WhatsAppLinkDto> GenerateBookingMessageAsync(Guid bookingId);
    Task<WhatsAppNumberDto> GetBusinessNumberAsync();
    Task<WhatsAppNumberDto> UpdateBusinessNumberAsync(string businessNumber);
}
