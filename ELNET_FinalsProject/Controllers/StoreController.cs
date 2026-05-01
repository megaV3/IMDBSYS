using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ELNET_FinalsProject.Data;
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

            // 3. Pass the data to the View
            return View(userProfile);
        }

        public IActionResult OrderMenu() //This is the page where users can see the menu and place orders
        {
            return View();
        }

        public IActionResult OrderRecords() //This is the page where users can see both their past and current orders with order details
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
