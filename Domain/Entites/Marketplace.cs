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
    public class Marketplace : BaseEntity
    {
        [Key]
        public int ListingId { get; set; }

        [Required]
        [ForeignKey("Crop")]
        public int CropId { get; set; }

        [Required]
        [Range(0.01, 1000000)]
        public decimal PricePerUnit { get; set; }

        [Required]
        [Range(0.1, 1000000)]
        public decimal QuantityAvailable { get; set; }

        [Required]
        public DateTime ListingDate { get; set; }

        [Required]
        public ListingStatus Status { get; set; }

        // Navigation properties
        public virtual Crop Crop { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }

    public enum ListingStatus
    {
        Active = 1,
        Sold = 2,
        Cancelled = 3
    }
}
