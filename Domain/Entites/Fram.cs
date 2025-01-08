using FarmersConnect.Core.Entites.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersConnect.Core.Entites
{
    public class Farm : BaseEntity
    {
        [Key]
        public int FarmId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FarmName { get; set; }

        [Required]
        [StringLength(200)]
        public string Location { get; set; }

        [StringLength(50)]
        public string SoilType { get; set; }

        [Required]
        [Range(0.1, 10000)]
        public decimal Size { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual ICollection<Crop> Crops { get; set; }
    }
}
