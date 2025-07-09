using FarmersConnect.Core.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.CropDTOs
{
    public class CreateCropDTO
    {
        [Required]
        [StringLength(100)]
        public string CropName { get; set; }

        [Required]
        public DateTime PlantingDate { get; set; }

        public DateTime? HarvestDate { get; set; }

        [Required]
        public CropStatus Status { get; set; }

        [Required]
        [Range(0.1, 100000)]
        public decimal YieldEstimate { get; set; }
    }
}
