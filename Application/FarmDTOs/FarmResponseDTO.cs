using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FarmDTOs
{
    public class FarmResponseDTO
    {
        public int FarmId { get; set; }
        public int UserId { get; set; }
        public string FarmName { get; set; }
        public string Location { get; set; }
        public string SoilType { get; set; }
        public decimal Size { get; set; }
        public bool flag { get; set; }
        public string? message { get; set; }
    }
}
