using ELNET_FinalsProject.Data;
using ELNET_FinalsProject.Models;
using ELNET_FinalsProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ELNET_FinalsProject.Controllers
{
    [Authorize] // This ensures ONLY logged-in users can enter this controller
    public class StoreController : Controller
    {
        private readonly AppDbContext _context;

        public StoreController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            // 1. Get the User ID from the Claims (stored in the cookie)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 2. Fetch the full user data from the database
            var userProfile = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == int.Parse(userId));

            // 2. Calculate Cart Count from the database
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.UserId == int.Parse(userId) && !o.IsCompleted);

            // Sum up the quantities of all items in the cart
            int count = order?.OrderItems.Sum(oi => oi.Quantity) ?? 0;
            
            StoreViewModel viewModel = new StoreViewModel
            {
                UserProfile = userProfile,
                Menus = await _context.Menus.ToListAsync(), // Fetch all menu items from the database
                CartCount = count, // Pass the count here,
                ProfileImagePath = userProfile.ProfileImagePath // Pass the profile
            };

            // 3. Pass the data to the View
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
