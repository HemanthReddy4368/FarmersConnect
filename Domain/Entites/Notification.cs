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
    public class Notification : BaseEntity
    {
        [Key]
        public int NotificationId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; }

        [Required]
        public NotificationType Type { get; set; }

        [Required]
        public DateTime SentAt { get; set; }

        [Required]
        public bool ReadStatus { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }

    public enum NotificationType
    {
        OrderConfirmation = 1,
        PaymentConfirmation = 2,
        CropUpdate = 3,
        SystemAlert = 4
    }
}
