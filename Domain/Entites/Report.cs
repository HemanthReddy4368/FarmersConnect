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
    public class Report : BaseEntity
    {
        [Key]
        public int ReportId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        public ReportType ReportType { get; set; }

        [Required]
        public DateTime GeneratedAt { get; set; }

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }

    public enum ReportType
    {
        Sales = 1,
        Inventory = 2,
        Financial = 3,
        Analytics = 4
    }
}
