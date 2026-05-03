using System.ComponentModel.DataAnnotations.Schema;

namespace ELNET_FinalsProject.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int MenuId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }

        public virtual Order Order { get; set; }

        public virtual Menu Menu { get; set; } 
  
        public decimal Total => Price * Quantity;
    }
}
