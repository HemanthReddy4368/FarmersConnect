using FarmersConnect.Core.Entites.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersConnect.Core.Entites
{
    public class User : BaseEntity
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public required string Email { get; set; }

        [Required]
        [StringLength(100)]
        public required string Password { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Phone]
        [StringLength(20)]
        public required string PhoneNumber { get; set; }

        [StringLength(200)]
        public required string Address { get; set; }

        // Navigation properties
        public virtual ICollection<Farm> Farms { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }

    public enum UserRole
    {
        Farmer = 1,
        Buyer = 2,
        Admin = 3
    }
}
