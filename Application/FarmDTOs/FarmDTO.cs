using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FarmDTOs
{
    public class FarmsDTO
    {
        public IEnumerable<FarmResponseDTO> Farms { get; set; }
        public bool flag { get; set; }
        public string? message { get; set; }
    }
}
