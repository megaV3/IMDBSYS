using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IMDBSYS.Data;
using IMDBSYS.Models;
using System.Security.Claims;
using System.Text.Json;
using IMDBSYS.ViewModels;

namespace IMDBSYS.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly string? PaymentMethod;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }
        // ── VIEW CART ACTION ──
        public async Task<IActionResult> Cart()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            // 1. Fetch full user metadata safely
            var userProfile = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (userProfile == null) return NotFound();

            ViewBag.ProfileImagePath = userProfile.ProfileImagePath ?? "/images/default-profile.png";
            ViewBag.UserBalance = userProfile.Balance;

            // 2. Fetch the active order along with its connected elements
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Menu) // Vital eager-loading step to fix the ₱0.00 issue
                .FirstOrDefaultAsync(o => o.UserId == userId && !o.IsCompleted);

            // 3. FIXED: Clean, unified check to see if cart is completely empty
            if (order == null || order.OrderItems == null || !order.OrderItems.Any())
            {
                ViewBag.IsCartEmpty = true;
                return View(new List<OrderItem>()); // Pass a clean empty list to avoid null models in View loops
            }

            // 4. Compute totals including tax metrics (Using the [NotMapped] OrderItem.Total expression)
            decimal subtotal = order.OrderItems.Sum(oi => oi.Total);
            decimal totalWithTax = subtotal * 1.12m;

            // 5. Build payment validation metrics flags
            ViewBag.IsAmountEnough = userProfile.Balance >= totalWithTax;
            ViewBag.IsCartEmpty = false;

            return View(order.OrderItems.ToList());
        }

        // ── ADD TO CART ACTION ──
        [HttpPost]
        public async Task<IActionResult> AddToCart(int menuId, int quantity, int menuVariationId, string? notes)
        {
            // 1. Safely check user ID parsing context
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Json(new { success = false, message = "User is not authenticated." });
            }

            // 2. Lookup the chosen structural variation to get the real Price and Name specs
            var variation = await _context.MenuVariations
                .FirstOrDefaultAsync(v => v.MenuVariationId == menuVariationId && v.MenuId == menuId);

            if (variation == null)
            {
                return Json(new { success = false, message = "Selected hardware model variation not found." });
            }

            // 3. Find or create the incomplete active order instance
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.UserId == userId && !o.IsCompleted);

            if (order == null)
            {
                order = new Order { UserId = userId, IsCompleted = false, CustomerName = User.Identity?.Name ?? "Customer" };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync(); // Saves to generate order.OrderId
            }

            // 4. Check if this exact variation package is already in the cart
            var existingItem = await _context.OrderItems
                .FirstOrDefaultAsync(oi =>
                    oi.OrderId == order.OrderId &&
                    oi.MenuId == menuId &&
                    oi.Variation == variation.VariantName && // e.g., "512GB" or "Standard"
                    oi.Notes == notes);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.OrderId,
                    MenuId = menuId,
                    Quantity = quantity,
                    Price = variation.Price,           // FIXED: Pulls the correct variation price!
                    Variation = variation.VariantName, // FIXED: Pulls text spec like "2TB"
                    Notes = notes
                });
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, quantityAdded = quantity });
        }
        // UPDATE QUANTITY
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int orderItemId, int quantity)
        {
            var item = await _context.OrderItems.FindAsync(orderItemId);

            if (item != null)
            {
                item.Quantity = quantity;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Cart");
        }

        // REMOVE ITEM
        [HttpPost]
        public async Task<IActionResult> Remove(int orderItemId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return Challenge();

            int userId = int.Parse(userIdString);

            var item = await _context.OrderItems
                .Include(oi => oi.Order)
                .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId && oi.Order.UserId == userId && !oi.Order.IsCompleted);

            if (item != null)
            {
                _context.OrderItems.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Cart");
        }

        // INCREASE ITEM'S QTY.
        [HttpPost]
        public async Task<IActionResult> Increase(int orderItemId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return Challenge();

            int userId = int.Parse(userIdString);

            var item = await _context.OrderItems
                .Include(oi => oi.Order)
                .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId && oi.Order.UserId == userId && !oi.Order.IsCompleted);

            if (item != null)
            {
                item.Quantity++;
                _context.OrderItems.Update(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Cart");
        }

        // DECREASE ITEM'S QTY.
        [HttpPost]
        public async Task<IActionResult> Decrease(int orderItemId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return Challenge();

            int userId = int.Parse(userIdString);

            var item = await _context.OrderItems
                .Include(oi => oi.Order)
                .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId && oi.Order.UserId == userId && !oi.Order.IsCompleted);

            if (item != null)
            {
                item.Quantity--;
                _context.OrderItems.Update(item);

                if (item.Quantity == 0)
                {
                    _context.OrderItems.Remove(item);
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Cart");
        }

        // ========================================================
        // POST / GET: Order/Checkout
        // ========================================================
        [HttpPost] // Recommending [HttpPost] for data mutations like checkout to ensure safety against CSRF
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            // 1. Fetch active incomplete order container along with deep relational item properties
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Menu) // Ensure the parent Menu structure is available if needed
                .FirstOrDefaultAsync(o => o.UserId == userId && !o.IsCompleted);

            if (order == null || !order.OrderItems.Any())
            {
                TempData["Error"] = "Your shopping cart is currently empty.";
                return RedirectToAction("Cart");
            }

            // 2. Fetch customer identity profile data record
            var customerProfile = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (customerProfile == null)
            {
                return NotFound();
            }

            var totalAmount = order.OrderItems.Sum(oi => oi.Price * oi.Quantity);

            // 3. BALANCE CHECK VALIDATION GATE
            if (customerProfile.Balance < totalAmount)
            {
                ViewData["AmountCheck"] = false;
                TempData["Error"] = "Insufficient funds in your account balance to process this checkout transaction.";
                return RedirectToAction("Cart");
            }

            // 4. INVENTORY AVAILABILITY CONTROL GATE
            // We run a dry-run validation pass first so we don't accidentally update half the items if one fails
            foreach (var item in order.OrderItems)
            {
                // Query the corresponding item row mapping match directly from MenuVariations
                var dbVariation = await _context.MenuVariations
                    .FirstOrDefaultAsync(v => v.MenuId == item.MenuId && v.VariantName == item.Variation);

                if (dbVariation == null)
                {
                    TempData["Error"] = $"The specific configuration variant for '{item.Menu?.Name ?? "Item"}' could not be validated.";
                    return RedirectToAction("Cart");
                }

                // Compare cart selection request quantity records against absolute disk stock values
                if (dbVariation.StockQuantity < item.Quantity)
                {
                    TempData["Error"] = $"Fulfillment blocked: '{item.Menu?.Name} ({item.Variation})' has insufficient stock. Remaining units: {dbVariation.StockQuantity}.";
                    return RedirectToAction("Cart");
                }
            }

            // 5. INVENTORY REDUCTION PASS
            // Once everything passes validation, safely reduce the quantities
            foreach (var item in order.OrderItems)
            {
                var dbVariation = await _context.MenuVariations
                    .FirstOrDefaultAsync(v => v.MenuId == item.MenuId && v.VariantName == item.Variation);

                if (dbVariation != null)
                {
                    // Subtract the exact units purchased
                    dbVariation.StockQuantity -= item.Quantity;
                    _context.Update(dbVariation);
                }
            }

            // 6. COMMIT ORDER COMPLETION PARAMETERS
            order.IsCompleted = true;
            order.OrderDate = DateTime.Now;
            order.CustomerName = $"{customerProfile.FirstName} {customerProfile.LastName}".Trim();
            order.TotalAmount = totalAmount;

            // Deduct total billing cost from client structural balance
            customerProfile.Balance -= totalAmount;

            _context.Update(order);
            _context.Update(customerProfile);

            // Commit changes safely to database storage records
            await _context.SaveChangesAsync();

            ViewData["AmountCheck"] = true;
            return RedirectToAction("Receipt");
        }

        // RECEIPT PAGE
        public async Task<IActionResult> Receipt()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Menu)
                .Where(o => o.UserId == userId && o.IsCompleted)
                .OrderByDescending(o => o.OrderId)
                .FirstOrDefaultAsync();

            return View(order);
        }

        // ORDER HISTORY PAGE
        public async Task<IActionResult> History()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Menu)
                .Where(o => o.UserId == userId && o.IsCompleted)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var activeCartItemCount = await _context.OrderItems
                .Where(oi => oi.Order.UserId == userId && !oi.Order.IsCompleted)
                .SumAsync(oi => oi.Quantity);

            var userProfile = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            var userOrders = new OrderHistoryViewModel
            {
                CartCount = activeCartItemCount,
                ProfileImagePath = userProfile.ProfileImagePath,
                OrderHistory = orders,
                UserBalance = userProfile.Balance
            };

            return View(userOrders);
        }
    }
}