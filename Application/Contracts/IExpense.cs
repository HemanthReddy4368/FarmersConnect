using Application.DTOs.ExpenseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IExpense
    {
        Task<IEnumerable<ExpenseDTO>> GetExpensesByCropIdAsync(int cropId);
        Task<ExpenseDTO> GetExpenseByIdAsync(int expenseId);
        Task<ExpenseDTO> CreateExpenseAsync(int cropId, CreateExpenseDTO createExpenseDto);
        Task<bool> DeleteExpenseAsync(int expenseId);
    }
}
