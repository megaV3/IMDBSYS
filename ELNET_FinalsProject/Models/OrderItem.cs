namespace ELNET_FinalsProject.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; } // ID for each item ordered
        public int OrderId { get; set; } // Foreign key to Order, indicates which order this item belongs to

        public int MenuId { get; set; } // Foreign key to Menu, indicates which menu item was ordered

        //public Menu? Menu { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; } // subject for change, may be calculated as Menu.Price * Quantity, but stored here for historical accuracy in case menu prices change later


        // Navigation property to Order
        public virtual Order Order { get; set; }

    }
}
