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
        public async Task<IActionResult> AddToCart(int menuId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.UserId == userId && !o.IsCompleted);

            // If no cart yet → create one
            if (order == null)
            {
                order = new Order
                {
                    UserId = userId,
                    IsCompleted = false
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }

            var existingItem = await _context.OrderItems
                .FirstOrDefaultAsync(oi => oi.OrderId == order.OrderId && oi.MenuId == menuId);

            if (existingItem != null)

            {
                existingItem.Quantity++;
            }
            else
            {
                var menu = await _context.Menus.FindAsync(menuId);

                var newItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    MenuId = menuId,
                    Quantity = 1,
                    Price = menu.Price
                };

                _context.OrderItems.Add(newItem);
            }

            await _context.SaveChangesAsync();

            //return RedirectToAction("Index","Store");
            return Ok();
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
        public async Task<IActionResult> Remove(int orderItemId)
        {
            var item = await _context.OrderItems.FindAsync(orderItemId);

            if (item != null)
            {
                _context.OrderItems.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Cart");
        }

        // CHECKOUT
        public async Task<IActionResult> Checkout()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.UserId == userId && !o.IsCompleted);

            if (order == null)
                return RedirectToAction("Cart");

            order.IsCompleted = true;
            order.OrderDate = DateTime.Now;

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