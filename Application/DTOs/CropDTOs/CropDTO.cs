using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.CropDTOs
{
    public class CropsDTO
    {
        public IEnumerable<CropResponseDTO> Crops { get; set; }
        public bool flag { get; set; }
        public string? message { get; set; }
    }
}
