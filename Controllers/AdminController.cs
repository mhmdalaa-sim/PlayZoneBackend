using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayZone.DTOs;
using PlayZone.Services;

namespace PlayZone.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
   _adminService = adminService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AdminLoginResponseDto>>> Login([FromBody] AdminLoginDto dto)
    {
  var result = await _adminService.LoginAsync(dto.Password);
   if (result == null)
      return Unauthorized(ApiResponse<AdminLoginResponseDto>.ErrorResponse("Invalid password"));

return Ok(ApiResponse<AdminLoginResponseDto>.SuccessResponse(result));
    }

    [HttpPost("logout")]
    [Authorize(Roles = "Admin")]
 public ActionResult<ApiResponse> Logout()
    {
   // JWT is stateless, so logout is handled on client side by removing the token
     return Ok(ApiResponse.SuccessResponse());
    }

    [HttpGet("stats")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<AdminStatsDto>>> GetStatistics()
  {
     var stats = await _adminService.GetStatisticsAsync();
  return Ok(ApiResponse<AdminStatsDto>.SuccessResponse(stats));
}
}
