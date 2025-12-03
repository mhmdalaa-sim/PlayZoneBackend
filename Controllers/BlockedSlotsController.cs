using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayZone.DTOs;
using PlayZone.Services;

namespace PlayZone.Controllers;

[ApiController]
[Route("api/blocked-slots")]
[Authorize(Roles = "Admin")]
public class BlockedSlotsController : ControllerBase
{
    private readonly IBlockedSlotService _blockedSlotService;

    public BlockedSlotsController(IBlockedSlotService blockedSlotService)
    {
   _blockedSlotService = blockedSlotService;
}

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<BlockedSlotDto>>>> GetAllBlockedSlots()
 {
   var slots = await _blockedSlotService.GetAllBlockedSlotsAsync();
   return Ok(ApiResponse<IEnumerable<BlockedSlotDto>>.SuccessResponse(slots));
  }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<BlockedSlotDto>>> GetBlockedSlotById(Guid id)
    {
   var slot = await _blockedSlotService.GetBlockedSlotByIdAsync(id);
     if (slot == null)
   return NotFound(ApiResponse<BlockedSlotDto>.ErrorResponse($"Blocked slot with ID {id} not found"));

   return Ok(ApiResponse<BlockedSlotDto>.SuccessResponse(slot));
    }

    [HttpPost]
public async Task<ActionResult<ApiResponse<BlockedSlotDto>>> CreateBlockedSlot([FromBody] CreateBlockedSlotDto dto)
 {
   var slot = await _blockedSlotService.CreateBlockedSlotAsync(dto);
     return CreatedAtAction(
    nameof(GetBlockedSlotById),
 new { id = slot.Id },
  ApiResponse<BlockedSlotDto>.SuccessResponse(slot));
  }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteBlockedSlot(Guid id)
    {
     await _blockedSlotService.DeleteBlockedSlotAsync(id);
   return Ok(ApiResponse.SuccessResponse());
  }

 [HttpGet("room/{roomId}/date/{date}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BlockedSlotDto>>>> GetBlockedSlotsByRoomAndDate(
   int roomId, string date)
  {
 var slots = await _blockedSlotService.GetBlockedSlotsByRoomAndDateAsync(roomId, date);
   return Ok(ApiResponse<IEnumerable<BlockedSlotDto>>.SuccessResponse(slots));
  }
}
