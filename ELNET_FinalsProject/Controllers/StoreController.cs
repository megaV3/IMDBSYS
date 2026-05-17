using IMDBSYS.Data;
using IMDBSYS.Models;
using IMDBSYS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IMDBSYS.Controllers
{
    [Authorize] // This ensures ONLY logged-in users can enter this controller
    public class StoreController : Controller
    {
        private readonly AppDbContext _context;

        public StoreController(AppDbContext context) => _context = context;

            public async Task<IActionResult> Index()
            {
                // 1. Get the User ID safely from the Claims
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Initialize default safe states for guests
                User? userProfile = null;
                int cartCount = 0;

                // Check if the user is actually authenticated before querying user-specific data
                if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out int userId))
                {
                    // 2. Fetch the full user data from the database
                    userProfile = await _context.Users
                        .FirstOrDefaultAsync(u => u.Id == userId);

                    // 3. Calculate Cart Count from the database for an incomplete order
                    var order = await _context.Orders
                        .Include(o => o.OrderItems) // FIXED: Changed from OrderItem to OrderItems
                        .FirstOrDefaultAsync(o => o.UserId == userId && !o.IsCompleted);

                    if (order != null && order.OrderItems != null) // FIXED: Changed from OrderItem to OrderItems
                    {
                        cartCount = order.OrderItems.Sum(oi => oi.Quantity); // FIXED: Changed from OrderItem to OrderItems
                    }
                }

                // 4. CRITICAL FIX: Include dynamic variations so the HTML page can read prices and specs!
                var menuItems = await _context.Menus
                    .Include(m => m.Variations)
                    .ToListAsync();

                // 5. Map data safely to your StoreViewModel
                StoreViewModel viewModel = new StoreViewModel
                {
                    // Fallback to a blank User initialization if visitor is a guest, preventing View null-crashes
                    UserProfile = userProfile ?? new User { FirstName = "Guest", Balance = 0.00m },
                    Menus = menuItems,
                    CartCount = cartCount,
                    ProfileImagePath = userProfile?.ProfileImagePath ?? "/images/default-profile.png"
                };

                // 6. Pass the data to the View
                return View(viewModel);
            }

        public async Task<IActionResult> OrderMenu() //This is the page where users can see the menu and place orders
        {
            // 1. Get the User ID from the Claims (stored in the cookie)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 2. Fetch the full user data from the database
            var userProfile = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == int.Parse(userId));

            var menuItems = _context.Menus.ToList(); // Fetch all menu items from the database

            // A ViewModel can be created to pass both userProfile and menuItems to the view if needed, but for simplicity, we will just pass the menu items for now
            return View(menuItems);
        }

        public async Task<IActionResult> OrderRecords() //This is the page where users can see both their past and current orders with order details
        {
            // 1. Get the User ID from the Claims (stored in the cookie)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 2. Fetch the full user data from the database
            var userProfile = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == int.Parse(userId));

            // Fetching the orders for the logged-in user, including related order items
            var userOrders = _context.Orders
                .Where(o => o.UserId == int.Parse(userId))
                .Include(o => o.OrderItems) // Include related order items
                .ToList();

            // A ViewModel can be created to pass both userProfile and userOrders to the view if needed, but for simplicity, we will just pass the orders for now
            return View(userOrders);
        }

    }
}
