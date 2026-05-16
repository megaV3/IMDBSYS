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

        public string? ImagePath { get; set; }

        // Navigation property
        public List<MenuVariation> Variations { get; set; } = new();
    }
}

