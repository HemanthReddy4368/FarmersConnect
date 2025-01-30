using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FarmDTOs
{
    public class CreateFarmDTO
    {
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
    }
}
