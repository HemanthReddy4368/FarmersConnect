using Application.Contracts;
using Application.DTOs.ExpenseDTOs;
using Domain.Entites;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public class ExpenseRepo : IExpense
    {
        private readonly AppDbContext _context;

        public ExpenseRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ExpenseDTO> CreateExpenseAsync(int cropId, CreateExpenseDTO createExpenseDto)
        {
            var expense = new Expense
            {
                CropId = cropId,
                Category = createExpenseDto.Category,
                Amount = createExpenseDto.Amount,
                ExpenseDate = createExpenseDto.ExpenseDate,
                Description = createExpenseDto.Description
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return MapToDto(expense);
        }

        public async Task<bool> DeleteExpenseAsync(int expenseId)
        {
            var expense = await _context.Expenses.FindAsync(expenseId);
            if (expense == null) return false;

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ExpenseDTO> GetExpenseByIdAsync(int expenseId)
        {
            var expense = await _context.Expenses.FindAsync(expenseId);
            return expense == null ? null : MapToDto(expense);
        }

        public async Task<IEnumerable<ExpenseDTO>> GetExpensesByCropIdAsync(int cropId)
        {
            return await _context.Expenses
                .Where(e => e.CropId == cropId)
                .Select(e => MapToDto(e))
                .ToListAsync();
        }

        private static ExpenseDTO MapToDto(Expense expense)
        {
            return new ExpenseDTO
            {
                ExpenseId = expense.ExpenseId,
                CropId = expense.CropId,
                Category = expense.Category,
                Amount = expense.Amount,
                ExpenseDate = expense.ExpenseDate,
                Description = expense.Description
            };
        }
    }
}
