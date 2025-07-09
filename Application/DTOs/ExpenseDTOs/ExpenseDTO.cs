using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ExpenseDTOs
{
    public class ExpenseDTO
    {
        public int ExpenseId { get; set; }
        public int CropId { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string? Description { get; set; }
    }
}
