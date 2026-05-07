using System.ComponentModel.DataAnnotations.Schema;

namespace ELNET_FinalsProject.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int MenuId { get; set; }
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }

        public string? Temperature { get; set; }   // "Hot" or "Iced"
        public string? Notes { get; set; }          // allergy/special instructions

        public Order Order { get; set; }
        public Menu Menu { get; set; }

        [NotMapped]
        public decimal Total => Price * Quantity;
    }
}
