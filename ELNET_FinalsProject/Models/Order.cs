using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELNET_FinalsProject.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public int UserId { get; set; }

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(8, 2)")]
        public decimal TotalAmount { get; set; }

        public bool IsCompleted { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
