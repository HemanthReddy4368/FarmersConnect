using FarmersConnect.Core.Entites;
using FarmersConnect.Core.Entites.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class Expense : BaseEntity
    {
        [Key]
        public int ExpenseId { get; set; }

        [Required]
        [ForeignKey("Crop")]
        public int CropId { get; set; }

        [Required]
        [StringLength(100)]
        public string Category { get; set; } // e.g., Seeds, Fertilizer, Labor

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime ExpenseDate { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        // Navigation property
        public virtual Crop Crop { get; set; }
    }
}
