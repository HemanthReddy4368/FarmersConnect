using Application.Contracts;
using Application.DTOs;
using FarmersConnect.Core.Entites;
using Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class User : ControllerBase
    {
        private readonly IUser user;

        public User(IUser user)
        {
            this.user = user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> LoginUser(LoginDTO loginDTO)
        {
            var result = await user.LoginUserAsync(loginDTO);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegestrationResponse>> RegesterUser(RegisterUserDTO registerDTO)
        {
            var result = await user.RegesterUserAsync(registerDTO);
            return Ok(result);
        }

        [HttpGet("admin-only")]
        [Authorize(Policy = Policies.RequireAdminRole)]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("This is accessible only by admins");
        }

        [HttpGet("farmer-only")]
        [Authorize(Policy = Policies.RequireFarmerRole)]
        public IActionResult FarmerOnlyEndpoint()
        {
            return Ok("This is accessible only by farmers");
        }

        [HttpGet("buyer-only")]
        [Authorize(Policy = Policies.RequireBuyerRole)]
        public IActionResult BuyerOnlyEndpoint()
        {
            return Ok("This is accessible only by buyers");
        }

        [HttpGet("{userId}")]
        [Authorize]
        public async Task<ActionResult<UserResponseDTO>> GetUser(int userId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim != userId.ToString() && !User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();
            var result = await user.GetUserByIdAsync(userId);
            if (!result.flag)
                return NotFound(result);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = Policies.RequireAdminRole)]
        public async Task<ActionResult<UsersDTO>> GetAllUsers()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();
            var result = await user.GetAllUsers();
            if (!result.flag)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPut("{userId}")]
        [Authorize]
        public async Task<ActionResult<UserResponseDTO>> UpdateUser(int userId, UpdateUserDTO updateUserDTO)
        {
            // Ensure users can only update their own profile unless they're admin
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim != userId.ToString() && !User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            var result = await user.UpdateUserAsync(userId, updateUserDTO);
            if (!result.flag)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{userId}")]
        [Authorize]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            // Ensure users can only delete their own profile unless they're admin
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim != userId.ToString() && !User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            var result = await user.DeleteUserAsync(userId);
            if (!result)
                return NotFound(new { message = "User not found or deletion failed" });
            return Ok(new { message = "User deleted successfully" });
        }

        [HttpPut("{userId}/role")]
        [Authorize(Policy = Policies.RequireAdminRole)]
        public async Task<ActionResult<UserResponseDTO>> UpdateUserRole(int userId, [FromBody] UserRole newRole)
        {
            var result = await user.UpdateUserRoleAsync(userId, newRole);
            if (!result.flag)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
