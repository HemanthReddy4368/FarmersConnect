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
    public class Crop : BaseEntity
    {
        [Key]
        public int CropId { get; set; }

        [Required]
        [ForeignKey("Farm")]
        public int FarmId { get; set; }

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

        // Navigation properties
        public virtual Farm Farm { get; set; }
        public virtual Marketplace MarketplaceListing { get; set; }
    }

    public enum CropStatus
    {
        Planted = 1,
        Growing = 2,
        ReadyToHarvest = 3,
        Harvested = 4
    }
}
