using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayZone.DTOs;
using PlayZone.Services;

namespace PlayZone.Controllers;

[ApiController]
[Route("api/whatsapp")]
public class WhatsAppController : ControllerBase
{
    private readonly IWhatsAppService _whatsAppService;

    public WhatsAppController(IWhatsAppService whatsAppService)
 {
   _whatsAppService = whatsAppService;
    }

[HttpPost("send-booking")]
    public async Task<ActionResult<ApiResponse<WhatsAppLinkDto>>> SendBookingConfirmation(
   [FromBody] WhatsAppMessageDto dto)
    {
        var result = await _whatsAppService.GenerateBookingMessageAsync(dto.BookingId);
        return Ok(ApiResponse<WhatsAppLinkDto>.SuccessResponse(result));
    }

    [HttpGet("number")]
    public async Task<ActionResult<ApiResponse<WhatsAppNumberDto>>> GetBusinessNumber()
    {
   var result = await _whatsAppService.GetBusinessNumberAsync();
   return Ok(ApiResponse<WhatsAppNumberDto>.SuccessResponse(result));
    }

    [HttpPut("number")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<WhatsAppNumberDto>>> UpdateBusinessNumber(
   [FromBody] UpdateWhatsAppNumberDto dto)
 {
  var result = await _whatsAppService.UpdateBusinessNumberAsync(dto.BusinessNumber);
   return Ok(ApiResponse<WhatsAppNumberDto>.SuccessResponse(result));
  }
}
