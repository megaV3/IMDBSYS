using Microsoft.EntityFrameworkCore;
using ELNET_FinalsProject.Models;

namespace ELNET_FinalsProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
