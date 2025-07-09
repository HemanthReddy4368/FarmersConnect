using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ExpenseDTOs
{
    public class CreateExpenseDTO
    {
        [Required]
        public string Category { get; set; }
        [Required]
        [Range(0.01, 1000000)]
        public decimal Amount { get; set; }
        [Required]
        public DateTime ExpenseDate { get; set; }
        public string? Description { get; set; }
    }
}
