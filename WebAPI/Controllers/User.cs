using Application.Contracts;
using Application.DTOs;
using Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
