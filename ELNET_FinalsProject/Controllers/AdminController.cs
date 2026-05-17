using IMDBSYS.Data;
using IMDBSYS.Models;
using IMDBSYS.ViewModels;
using IMDBSYS.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IMDBSYS.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        public AdminController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Dashboard()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            // 1. Fetch full user metadata safely
            var userProfile = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (userProfile == null) return NotFound();

            var viewModel = new DashboardViewModel();

            // 1. Fetching Totals
            viewModel.TotalUsers = await _context.Users.CountAsync(); // Assuming you have a Users DbSet
            viewModel.TotalProducts = await _context.Menus.CountAsync();
            viewModel.TotalOrders = await _context.Orders.CountAsync();

            // Calculate total sales from completed orders
            viewModel.TotalSalesRevenue = await _context.Orders
                .Where(o => o.IsCompleted)
                .SumAsync(o => o.TotalAmount);

            // Calculate total successful wallet top-ups
            viewModel.TotalTopUpAmount = await _context.TopUpHistories
                .Where(t => t.Status == "Completed" || t.Status == "Approved") // Adjust string based on your data
                .SumAsync(t => t.Amount);

            // 2. Inventory & Stock Monitoring
            viewModel.TotalStockQuantity = await _context.MenuVariations
                .SumAsync(mv => mv.StockQuantity);

            // Fetch products where inventory is less than or equal to their low-stock threshold
            viewModel.LowStockProducts = await _context.MenuVariations
                .Include(mv => mv.Menu) // Includes parent Menu info (Name, Category, etc.)
                .Where(mv => mv.StockQuantity <= mv.LowStockThreshold)
                .OrderBy(mv => mv.StockQuantity)
                .Take(5) // Limit to top 5 most urgent items
                .ToListAsync();

            // 3. Recent Activities
            viewModel.RecentOrders = await _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToListAsync();

            viewModel.RecentTopUps = await _context.TopUpHistories
                .OrderByDescending(t => t.TransactionDate)
                .Take(5)
                .ToListAsync();

            return View(viewModel);
        }

        // ==========================================
        // 2. PRODUCTS / MENU CATALOG
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> MenuIndex()
        {
            // .Include(m => m.Variations) ensures the View can count how many variations exist
            var menus = await _context.Menus
                .Include(m => m.Variations)
                .OrderBy(m => m.Name)
                .ToListAsync();

            return View("MenuIndex", menus); // Explicitly naming the view if it doesn't match action name
        }

        // ==========================================
        // 3. GLOBAL ORDER REGISTRY (Audit Logs)
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> OrderRegistry()
        {
            // .Include(o => o.OrderItems) pulls children so we can count items inside the order
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View("OrderRegistry", orders);
        }

        // ==========================================
        // 4. WALLET TOP-UPS LEDGER
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> TopUpLedger()
        {
            var topUps = await _context.TopUpHistories
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();

            return View("TopUpLedger", topUps);
        }

        // ==========================================
        // 5. USER ACCESS MANAGEMENT
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> UserManagement()
        {
            // Fetching users from your database context
            // Projecting them directly into a clear view model structure
            var users = await _context.Users
                .Select(u => new UserManagementViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.Role // Assuming "Admin" or "User" strings are saved here
                })
                .OrderBy(u => u.LastName)
                .ToListAsync();

            return View("UserManagement", users);
        }
    }
}
