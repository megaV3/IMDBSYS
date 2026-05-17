using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMDBSYS.Models
{
    public class ProductDeliveryLog
    {
        [Key]
        public int DeliveryLogId { get; set; }

        [Required]
        public int MenuVariationId { get; set; }

        [ForeignKey("MenuVariationId")]
        public virtual MenuVariation? MenuVariation { get; set; }

        [Required]
        public int QuantityAdded { get; set; }

        // NEW DESIGNATED COLUMN FIELD FOR TOTAL WHOLESALE SPENT
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public DateTime DeliveryDate { get; set; } = DateTime.Now;

        [Required]
        public string ProcessedBy { get; set; } = "System Admin";

        [StringLength(250)]
        public string? Remarks { get; set; }
    }
}
