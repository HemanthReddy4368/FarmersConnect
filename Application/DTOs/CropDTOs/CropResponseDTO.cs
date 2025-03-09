using FarmersConnect.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.CropDTOs
{
    public class CropResponseDTO
    {
        public int CropId { get; set; }
        public int FarmId { get; set; }
        public string CropName { get; set; }
        public DateTime PlantingDate { get; set; }
        public DateTime? HarvestDate { get; set; }
        public CropStatus Status { get; set; }
        public decimal YieldEstimate { get; set; }
        public bool flag { get; set; }
        public string? message { get; set; }
    }
}
