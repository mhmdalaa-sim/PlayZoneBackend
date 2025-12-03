using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayZone.DTOs;
using PlayZone.Services;

namespace PlayZone.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
   _bookingService = bookingService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PaginatedResponse<BookingDto>>> GetAllBookings([FromQuery] BookingFilterDto filter)
    {
        var result = await _bookingService.GetAllBookingsAsync(filter);
  return Ok(result);
    }

 [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<BookingDto>>> GetBookingById(Guid id)
  {
     var booking = await _bookingService.GetBookingByIdAsync(id);
 if (booking == null)
 return NotFound(ApiResponse<BookingDto>.ErrorResponse($"Booking with ID {id} not found"));

      return Ok(ApiResponse<BookingDto>.SuccessResponse(booking));
}

    [HttpPost]
    public async Task<ActionResult<ApiResponse<BookingDto>>> CreateBooking([FromBody] CreateBookingDto dto)
  {
        var booking = await _bookingService.CreateBookingAsync(dto);
   return CreatedAtAction(
   nameof(GetBookingById),
       new { id = booking.Id },
      ApiResponse<BookingDto>.SuccessResponse(booking));
  }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse>> DeleteBooking(Guid id)
{
     await _bookingService.DeleteBookingAsync(id);
   return Ok(ApiResponse.SuccessResponse());
    }

    [HttpGet("date/{date}")]
 public async Task<ActionResult<ApiResponse<IEnumerable<BookingDto>>>> GetBookingsByDate(string date)
    {
  var bookings = await _bookingService.GetBookingsByDateAsync(date);
return Ok(ApiResponse<IEnumerable<BookingDto>>.SuccessResponse(bookings));
 }

    [HttpGet("room/{roomId}")]
 public async Task<ActionResult<ApiResponse<IEnumerable<BookingDto>>>> GetBookingsByRoomId(int roomId)
    {
        var bookings = await _bookingService.GetBookingsByRoomIdAsync(roomId);
  return Ok(ApiResponse<IEnumerable<BookingDto>>.SuccessResponse(bookings));
  }
}
