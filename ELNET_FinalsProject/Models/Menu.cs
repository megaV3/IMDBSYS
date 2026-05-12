using System.ComponentModel.DataAnnotations.Schema;

namespace IMDBSYS.Models
{
    public class Menu
    {
        public int MenuId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;
        public bool CanBeHot { get; set; }
        public bool CanBeCold { get; set; }

        public string? ImagePath { get; set; }
    }
}

