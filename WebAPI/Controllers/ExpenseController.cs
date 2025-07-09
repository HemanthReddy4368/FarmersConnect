using Application.Contracts;
using Application.DTOs.ExpenseDTOs;
using Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/crop/{cropId}/[controller]")]
    [ApiController]
    [Authorize(Policy = Policies.RequireFarmerRole)]
    public class ExpenseController : Controller
    {
        private readonly IExpense _expenseService;
        private readonly ICrop _cropService;
        private readonly IFarm _farmService;

        public ExpenseController(IExpense expenseService, ICrop cropService, IFarm farmService)
        {
            _expenseService = expenseService;
            _cropService = cropService;
            _farmService = farmService;
        }

        private async Task<bool> IsOwnerOfCrop(int cropId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return false;

            var crop = await _cropService.GetCropByIdAsync(cropId);
            if (!crop.flag) return false;

            var farm = await _farmService.GetFarmByIdAsync(crop.FarmId);
            return farm.UserId == int.Parse(userIdClaim);
        }

        [HttpGet]
        public async Task<IActionResult> GetExpenses(int cropId)
        {
            if (!await IsOwnerOfCrop(cropId)) return Forbid();
            var expenses = await _expenseService.GetExpensesByCropIdAsync(cropId);
            return Ok(expenses);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpense(int cropId, [FromBody] CreateExpenseDTO createExpenseDto)
        {
            if (!await IsOwnerOfCrop(cropId)) return Forbid();
            var newExpense = await _expenseService.CreateExpenseAsync(cropId, createExpenseDto);
            return CreatedAtAction(nameof(GetExpenses), new { cropId = newExpense.CropId, expenseId = newExpense.ExpenseId }, newExpense);
        }

        [HttpDelete("{expenseId}")]
        public async Task<IActionResult> DeleteExpense(int cropId, int expenseId)
        {
            if (!await IsOwnerOfCrop(cropId)) return Forbid();
            var result = await _expenseService.DeleteExpenseAsync(expenseId);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
