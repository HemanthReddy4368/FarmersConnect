using Application.Contracts;
using Application.FarmDTOs;
using FarmersConnect.Core.Entites;
using Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    // WebAPI/Controllers/FarmController.cs
    [Route("api/[controller]")]
    [ApiController]
    public class FarmController : ControllerBase
    {
        private readonly IFarm _farmService;

        public FarmController(IFarm farmService)
        {
            _farmService = farmService;
        }

        [HttpPost]
        [Authorize(Policy = Policies.RequireFarmerRole)]
        public async Task<ActionResult<FarmResponseDTO>> CreateFarm(CreateFarmDTO createFarmDTO)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            var userId = int.Parse(userIdClaim);
            var result = await _farmService.CreateFarmAsync(userId, createFarmDTO);

            if (!result.flag)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{farmId}")]
        [Authorize(Policy = Policies.RequireFarmerRole)]
        public async Task<ActionResult<FarmResponseDTO>> UpdateFarm(int farmId, UpdateFarmDTO updateFarmDTO)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            // Verify farm belongs to user
            var farm = await _farmService.GetFarmByIdAsync(farmId);
            if (!farm.flag || farm.UserId != int.Parse(userIdClaim))
                return Forbid();

            var result = await _farmService.UpdateFarmAsync(farmId, updateFarmDTO);
            if (!result.flag)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{farmId}")]
        [Authorize(Policy = Policies.RequireFarmerRole)]
        [Authorize]
        public async Task<ActionResult<FarmResponseDTO>> GetFarm(int farmId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            // Verify farm belongs to user
            var farm = await _farmService.GetFarmByIdAsync(farmId);
            if (!farm.flag || farm.UserId != int.Parse(userIdClaim))
                return Forbid();

            var result = await _farmService.GetFarmByIdAsync(farmId);
            if (!result.flag)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<ActionResult<FarmsDTO>> GetUserFarms(int userId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || (!User.IsInRole(UserRole.Admin.ToString()) && userId != int.Parse(userIdClaim)))
                return Forbid();

            var result = await _farmService.GetAllFarmsByUserIdAsync(userId);
            if (!result.flag)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = Policies.RequireAdminRole)]
        public async Task<ActionResult<FarmsDTO>> GetAllFarms()
        {
            var result = await _farmService.GetAllFarmsAsync();
            if (!result.flag)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{farmId}")]
        [Authorize(Policy = Policies.RequireFarmerRole)]
        public async Task<ActionResult> DeleteFarm(int farmId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            // Verify farm belongs to user
            var farm = await _farmService.GetFarmByIdAsync(farmId);
            if (!farm.flag || farm.UserId != int.Parse(userIdClaim))
                return Forbid();

            var result = await _farmService.DeleteFarmAsync(farmId);
            if (!result)
                return NotFound(new { message = "Farm not found or deletion failed" });

            return Ok(new { message = "Farm deleted successfully" });
        }
    }
}
