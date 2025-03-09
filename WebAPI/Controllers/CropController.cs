// WebAPI/Controllers/CropController.cs
using Application.Contracts;
using Application.DTOs;
using Application.DTOs.CropDTOs;
using FarmersConnect.Core.Entites;
using Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CropController : ControllerBase
    {
        private readonly ICrop _cropService;
        private readonly IFarm _farmService;

        public CropController(ICrop cropService, IFarm farmService)
        {
            _cropService = cropService;
            _farmService = farmService;
        }

        [HttpPost("farm/{farmId}")]
        [Authorize(Policy = Policies.RequireFarmerRole)]
        public async Task<ActionResult<CropResponseDTO>> CreateCrop(int farmId, CreateCropDTO createCropDTO)
        {
            // Get current user id
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            // Verify farm exists and belongs to user
            var farm = await _farmService.GetFarmByIdAsync(farmId);
            if (!farm.flag)
                return NotFound(new { message = "Farm not found" });

            if (farm.UserId != int.Parse(userIdClaim))
                return Forbid();

            var result = await _cropService.CreateCropAsync(farmId, createCropDTO);
            if (!result.flag)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{cropId}")]
        [Authorize(Policy = Policies.RequireFarmerRole)]
        public async Task<ActionResult<CropResponseDTO>> UpdateCrop(int cropId, UpdateCropDTO updateCropDTO)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            // Get crop details
            var crop = await _cropService.GetCropByIdAsync(cropId);
            if (!crop.flag)
                return NotFound(crop);

            // Verify farm belongs to user
            var farm = await _farmService.GetFarmByIdAsync(crop.FarmId);
            if (farm.UserId != int.Parse(userIdClaim))
                return Forbid();

            var result = await _cropService.UpdateCropAsync(cropId, updateCropDTO);
            if (!result.flag)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{cropId}")]
        [Authorize]
        public async Task<ActionResult<CropResponseDTO>> GetCrop(int cropId)
        {
            // Get current user id
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            // Get crop details
            var result = await _cropService.GetCropByIdAsync(cropId);
            if (!result.flag)
                return NotFound(result);

            // Get farm details to verify ownership
            var farm = await _farmService.GetFarmByIdAsync(result.FarmId);
            if (!farm.flag)
                return NotFound(new { message = "Associated farm not found" });

            // Allow access if user is admin or farm owner
            if (!User.IsInRole(UserRole.Admin.ToString()) && farm.UserId != int.Parse(userIdClaim))
                return Forbid();

            return Ok(result);
        }

        [HttpGet("farm/{farmId}")]
        [Authorize]
        public async Task<ActionResult<CropsDTO>> GetFarmCrops(int farmId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            // Verify farm exists and user has access
            var farm = await _farmService.GetFarmByIdAsync(farmId);
            if (!farm.flag)
                return NotFound(new { message = "Farm not found" });

            // Allow access if user is admin or farm owner
            if (!User.IsInRole(UserRole.Admin.ToString()) && farm.UserId != int.Parse(userIdClaim))
                return Forbid();

            var result = await _cropService.GetCropsByFarmIdAsync(farmId);
            if (!result.flag)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{cropId}")]
        [Authorize(Policy = Policies.RequireFarmerRole)]
        public async Task<ActionResult> DeleteCrop(int cropId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            // Get crop details
            var crop = await _cropService.GetCropByIdAsync(cropId);
            if (!crop.flag)
                return NotFound(new { message = "Crop not found" });

            // Verify farm belongs to user
            var farm = await _farmService.GetFarmByIdAsync(crop.FarmId);
            if (farm.UserId != int.Parse(userIdClaim))
                return Forbid();

            var result = await _cropService.DeleteCropAsync(cropId);
            if (!result)
                return NotFound(new { message = "Crop not found or deletion failed" });

            return Ok(new { message = "Crop deleted successfully" });
        }

        [HttpPatch("{cropId}/status")]
        [Authorize(Policy = Policies.RequireFarmerRole)]
        public async Task<ActionResult<CropResponseDTO>> UpdateCropStatus(int cropId, [FromBody] CropStatus newStatus)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            // Get crop details
            var crop = await _cropService.GetCropByIdAsync(cropId);
            if (!crop.flag)
                return NotFound(crop);

            // Verify farm belongs to user
            var farm = await _farmService.GetFarmByIdAsync(crop.FarmId);
            if (farm.UserId != int.Parse(userIdClaim))
                return Forbid();

            var result = await _cropService.UpdateCropStatusAsync(cropId, newStatus);
            if (!result.flag)
                return BadRequest(result);

            return Ok(result);
        }
    }
}