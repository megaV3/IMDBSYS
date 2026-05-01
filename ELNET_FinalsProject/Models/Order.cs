using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELNET_FinalsProject.Models
{
    public class Order
    {
        public int OrderID { get; set; } // ID for each order 

        [Required]
        public string CustomerName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Order date is required")]

        public DateTime OrderDate { get; set; }
        [Column(TypeName = "decimal(8, 2)")]

        public decimal TotalAmount { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        // Navigation property for One to Many relationship with OrderItem (One Order can have many OrderItems)
        // The 'virtual' keyword enables lazy loading of related OrderItems when accessing the OrderItems property
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
