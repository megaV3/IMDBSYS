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

        // VIEW CART
        public async Task<IActionResult> Cart()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Menu)
                .FirstOrDefaultAsync(o => o.UserId == userId && !o.IsCompleted);

            var userProfile = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            ViewBag.ProfileImagePath = userProfile.ProfileImagePath;
            ViewBag.UserBalance = userProfile.Balance;


            // Checks if the order is null
            if (order == null)
            {
                ViewBag.IsCartEmpty = true;
                return View();
            }

            //If the order is no longer null, it then checks if the count of cartItems is 0, then initializes a ViewBag that cart is empty and returns View
            var cartItems = await _context.Orders
                .Where(o => o.UserId == userId && !o.IsCompleted)
                .SelectMany(o => o.OrderItems)
                .Include(oi => oi.Menu)
                .ToListAsync();

            if (cartItems.Count == 0)
            {
                ViewBag.IsCartEmpty = true;
                return View();
            }


            decimal totalAmount = order.OrderItems.Sum(oi => oi.Price * oi.Quantity);
            totalAmount *= 1.12m;

            ViewBag.UserBalance = userProfile.Balance;

            if (userProfile.Balance >= totalAmount)
            {
                ViewBag.IsAmountEnough = true;
            }
            else
            {
                ViewBag.IsAmountEnough = false;
            }

            ViewBag.IsCartEmpty = false;
            return View(order.OrderItems);
        }

        // ADD TO CART
        [HttpPost]
        public async Task<IActionResult> AddToCart(int menuId, int quantity, string? variation, string? notes)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.UserId == userId && !o.IsCompleted);

            if (order == null)
            {
                order = new Order { UserId = userId, IsCompleted = false};
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }

            var existingItem = await _context.OrderItems
                .FirstOrDefaultAsync(oi =>
                    oi.OrderId == order.OrderId &&
                    oi.MenuId == menuId &&
                    oi.Variation == variation &&
                    oi.Notes == notes);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var menu = await _context.Menus.FindAsync(menuId);
                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.OrderId,
                    MenuId = menuId,
                    Quantity = quantity,
                    Price = menu.Price,
                    Variation = variation,
                    Notes = notes
                });
            }

            await _context.SaveChangesAsync();
            return Ok(new { success = true, quantityAdded = quantity });
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

        // ✅ UPDATED — CHECKOUT with balance check
        public async Task<IActionResult> Checkout()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.UserId == userId && !o.IsCompleted);

            if (order == null)
                return RedirectToAction("Cart");

            var customerName = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var totalAmount = order.OrderItems.Sum(oi => oi.Price * oi.Quantity);

            if (customerName.Balance >= totalAmount)
            {
                order.IsCompleted = true;
                order.OrderDate = DateTime.Now;
                order.CustomerName = $"{customerName.FirstName} {customerName.LastName}";
                order.TotalAmount = totalAmount;

                customerName.Balance = customerName.Balance - order.TotalAmount;

                ViewData["AmountCheck"] = true;
            }
            else
            {
                ViewData["AmountCheck"] = false;
                return RedirectToAction("Cart");
            }

            await _context.SaveChangesAsync();

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
            };

            return View(userOrders);
        }
    }
}