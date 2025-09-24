using HeartSpace.Api.Models;
using HeartSpace.Application.Services.UserService;
using HeartSpace.Application.Services.UserService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HeartSpace.Api.Controllers
{
    [Route("api/users")]
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("profile")]
        public async Task<ActionResult<ApiResponse<UserProfileResponse?>>> GetProfile()
        {
            // Get user ID from JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest<UserProfileResponse>("invalid user token", null);
            }
            var profile = await _userService.GetUserProfileAsync(userId);

            return Ok(profile, "Get profile successfully");
        }

        [HttpPut("profile")]
        public async Task<ActionResult<ApiResponse>> UpdateProfile([FromBody] UserProfileUpdateDto request)
        {
            // Get user ID from JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("invalid user token");
            }
            // Update the user profile
            var result = await _userService.UpdateUserProfileAsync(userId, request);
            if (!result)
            {
                return BadRequest("Failed to update profile");
            }
            return Ok("Profile updated successfully");
        }
    }
}
