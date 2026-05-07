using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ELNET_FinalsProject.Data;
using ELNET_FinalsProject.Models;
using System.Security.Claims;
using System.Text.Json;

namespace ELNET_FinalsProject.Controllers
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

            if (order == null)
            {
                return View(new List<OrderItem>());
            }

            return View(order.OrderItems);
        }

        // ADD TO CART
        [HttpPost]
        public async Task<IActionResult> AddToCart(int menuId, int quantity, string? temperature, string? notes)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.UserId == userId && !o.IsCompleted);

            if (order == null)
            {
                order = new Order { UserId = userId, IsCompleted = false };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }

            var existingItem = await _context.OrderItems
                .FirstOrDefaultAsync(oi =>
                    oi.OrderId == order.OrderId &&
                    oi.MenuId == menuId &&
                    oi.Temperature == temperature &&
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
                    Temperature = temperature,
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

            order.IsCompleted = true;
            order.OrderDate = DateTime.Now;
            order.CustomerName = $"{customerName.FirstName} {customerName.LastName}";
            order.TotalAmount = totalAmount;
            order.PaymentMethod =PaymentMethod;

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
    }
}