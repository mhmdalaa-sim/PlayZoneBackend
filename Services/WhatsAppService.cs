using PlayZone.DTOs;
using PlayZone.Repositories;
using PlayZone.Utilities;

namespace PlayZone.Services;

public class WhatsAppService : IWhatsAppService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IWhatsAppConfigRepository _whatsAppConfigRepository;

    public WhatsAppService(
   IBookingRepository bookingRepository,
     IWhatsAppConfigRepository whatsAppConfigRepository)
    {
_bookingRepository = bookingRepository;
 _whatsAppConfigRepository = whatsAppConfigRepository;
    }

 public async Task<WhatsAppLinkDto> GenerateBookingMessageAsync(Guid bookingId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
  if (booking == null)
       throw new KeyNotFoundException($"Booking with ID {bookingId} not found");

   var config = await _whatsAppConfigRepository.GetConfigAsync();
   var businessNumber = config?.BusinessNumber ?? "+201234567890";

   // Format message
   var message = WhatsAppHelper.FormatBookingMessage(
     booking.Room.Name,
   booking.Date.ToString("yyyy-MM-dd"),
   booking.StartTime,
   booking.EndTime,
   booking.Duration,
     booking.CustomerName,
    booking.CustomerPhone,
     booking.Notes
        );

   // Generate WhatsApp URL
  var whatsAppUrl = WhatsAppHelper.GenerateWhatsAppUrl(businessNumber, message);

   return new WhatsAppLinkDto
        {
       WhatsAppUrl = whatsAppUrl,
Message = message
   };
    }

    public async Task<WhatsAppNumberDto> GetBusinessNumberAsync()
    {
        var config = await _whatsAppConfigRepository.GetConfigAsync();
     return new WhatsAppNumberDto
   {
  BusinessNumber = config?.BusinessNumber ?? "+201234567890"
     };
    }

    public async Task<WhatsAppNumberDto> UpdateBusinessNumberAsync(string businessNumber)
  {
 var config = await _whatsAppConfigRepository.GetConfigAsync();
  if (config == null)
   {
      config = new Models.WhatsAppConfig
      {
       BusinessNumber = businessNumber
  };
        }
   else
   {
    config.BusinessNumber = businessNumber;
     }

var updatedConfig = await _whatsAppConfigRepository.UpdateConfigAsync(config);
   return new WhatsAppNumberDto
  {
     BusinessNumber = updatedConfig.BusinessNumber
     };
    }
}
