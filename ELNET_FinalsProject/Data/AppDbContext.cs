
using Microsoft.EntityFrameworkCore;
using IMDBSYS.Models;

namespace IMDBSYS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Menu> Menus { get; set; } // should change name to MenuItems?
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<TopUpHistory> TopUpHistories { get; set; }
    }
}
