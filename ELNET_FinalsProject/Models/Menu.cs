using System.ComponentModel.DataAnnotations.Schema;

namespace ELNET_FinalsProject.Models
{
    public class Menu
    {
        public long? MenuID { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Category { get; set; } = String.Empty; 

        [Column(TypeName = "decimal(B, 2)")]
        public decimal Price { get; set; }
        public string Description { get; set; } = String.Empty;
        public bool CanBeHot { get; set; }
        public bool CanBeCold { get; set; }

        public string? ImagePath { get; set; }
    }
}
