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
    public class Order : BaseEntity
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        [ForeignKey("Listing")]
        public int ListingId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(0.1, 1000000)]
        public decimal Quantity { get; set; }

        [Required]
        [Range(0.1, 10000000)]
        public decimal TotalPrice { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public PaymentStatus PaymentStatus { get; set; }

        // Navigation properties
        public virtual Marketplace Listing { get; set; }

        [InverseProperty("Orders")]
        public virtual User Buyer { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
