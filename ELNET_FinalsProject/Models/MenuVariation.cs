using System.ComponentModel.DataAnnotations.Schema;

namespace IMDBSYS.Models
{
    public class MenuVariation
    {
        public int MenuVariationId { get; set; }
        public int MenuId { get; set; }
        public Menu? Menu { get; set; }

        public string VariantName { get; set; } = string.Empty; // e.g., "256GB"

        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }

        // INVENTORY MANAGEMENT PROPERTIES GO HERE
        public int StockQuantity { get; set; }
        public int LowStockThreshold { get; set; } = 15; // Optional: Warns you when stock drops below e.g., 5 units
    }
}
