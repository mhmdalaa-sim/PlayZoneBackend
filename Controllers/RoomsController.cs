using Microsoft.AspNetCore.Mvc;
using PlayZone.DTOs;
using PlayZone.Services;

namespace PlayZone.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
 private readonly IRoomService _roomService;

public RoomsController(IRoomService roomService)
    {
   _roomService = roomService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<RoomDto>>>> GetAllRooms()
    {
     var rooms = await _roomService.GetAllRoomsAsync();
        return Ok(ApiResponse<IEnumerable<RoomDto>>.SuccessResponse(rooms));
    }

    [HttpGet("{id}")]
public async Task<ActionResult<ApiResponse<RoomDto>>> GetRoomById(int id)
    {
  var room = await _roomService.GetRoomByIdAsync(id);
        if (room == null)
  return NotFound(ApiResponse<RoomDto>.ErrorResponse($"Room with ID {id} not found"));

      return Ok(ApiResponse<RoomDto>.SuccessResponse(room));
    }

    [HttpPut("{id}/availability")]
 public async Task<ActionResult<ApiResponse<RoomDto>>> UpdateRoomAvailability(
   int id, [FromBody] UpdateRoomAvailabilityDto dto)
 {
   var room = await _roomService.UpdateRoomAvailabilityAsync(id, dto.Available);
   return Ok(ApiResponse<RoomDto>.SuccessResponse(room));
 }

    [HttpGet("available")]
    public async Task<ActionResult<ApiResponse<IEnumerable<RoomDto>>>> GetAvailableRooms(
  [FromQuery] AvailableRoomsQueryDto query)
    {
  if (!DateOnly.TryParse(query.Date, out var date))
     return BadRequest(ApiResponse<IEnumerable<RoomDto>>.ErrorResponse("Invalid date format. Use YYYY-MM-DD"));

        var rooms = await _roomService.GetAvailableRoomsForTimeSlotAsync(
     date, query.StartTime, query.EndTime);

   return Ok(ApiResponse<IEnumerable<RoomDto>>.SuccessResponse(rooms));
 }

    [HttpGet("{id}/status")]
    public async Task<ActionResult<ApiResponse<RoomStatusDto>>> GetRoomStatus(
   int id, [FromQuery] AvailableRoomsQueryDto query)
    {
  if (!DateOnly.TryParse(query.Date, out var date))
return BadRequest(ApiResponse<RoomStatusDto>.ErrorResponse("Invalid date format. Use YYYY-MM-DD"));

        var status = await _roomService.GetRoomStatusAsync(id, date, query.StartTime, query.EndTime);
        return Ok(ApiResponse<RoomStatusDto>.SuccessResponse(status));
    }
}
