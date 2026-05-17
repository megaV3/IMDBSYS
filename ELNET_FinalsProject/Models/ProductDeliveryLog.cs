using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMDBSYS.Models
{
    public class ProductDeliveryLog
    {
        [Key]
        public int DeliveryLogId { get; set; }

        // Relational link to the exact variant that received stock
        [Required]
        public int MenuVariationId { get; set; }

        [ForeignKey("MenuVariationId")]
        public virtual MenuVariation? MenuVariation { get; set; }

        [Required]
        public int QuantityAdded { get; set; }

        [Required]
        public DateTime DeliveryDate { get; set; } = DateTime.Now;

        [Required]
        public string ProcessedBy { get; set; } = "System Admin"; // Expandable to actual user logins later

        [StringLength(250)]
        public string? Remarks { get; set; } // e.g., "Supplier Manifest #INV-9021"
    }
}
