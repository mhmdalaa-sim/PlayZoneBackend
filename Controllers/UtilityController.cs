using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PlayZone.Configuration;
using PlayZone.DTOs;
using PlayZone.Utilities;

namespace PlayZone.Controllers;

[ApiController]
[Route("api")]
public class UtilityController : ControllerBase
{
    private readonly BusinessSettings _businessSettings;

    public UtilityController(IOptions<BusinessSettings> businessSettings)
    {
   _businessSettings = businessSettings.Value;
  }

    [HttpGet("time-slots")]
    public ActionResult<ApiResponse<IEnumerable<TimeSlotDto>>> GetTimeSlots()
  {
        var slots = TimeHelper.GenerateTimeSlots(
       _businessSettings.OpeningTime,
      _businessSettings.ClosingTime,
  _businessSettings.TimeSlotIntervalMinutes);

     var slotDtos = slots.Select(s => new TimeSlotDto
    {
      Time = s,
  Display = s
     });

  return Ok(ApiResponse<IEnumerable<TimeSlotDto>>.SuccessResponse(slotDtos));
    }

    [HttpGet("health")]
    public ActionResult<object> HealthCheck()
{
      return Ok(new
   {
   status = "healthy",
   timestamp = DateTime.UtcNow,
 version = "1.0.0",
   environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
   });
  }
}
