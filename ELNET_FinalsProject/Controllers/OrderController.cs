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

            if (existingItem != null) //checks if the item already exists in the cart then increments quantity
            {
                existingItem.Quantity++;
            }
            else //else, the item is created
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
        [HttpPost]
        public async Task<IActionResult> Remove(int orderItemId)
        {
            // 1. Get the User ID from the Claims (stored in the cookie)
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return Challenge(); // Ensure user is logged in

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
            // 1. Get the User ID from the Claims (stored in the cookie)
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return Challenge(); // Ensure user is logged in

            int userId = int.Parse(userIdString);

            // fetches the item added to the cart
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
            // 1. Get the User ID from the Claims (stored in the cookie)
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return Challenge(); // Ensure user is logged in

            int userId = int.Parse(userIdString);

            var item = await _context.OrderItems
                .Include(oi => oi.Order)
                .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId && oi.Order.UserId == userId && !oi.Order.IsCompleted);

            if (item != null)
            {
                item.Quantity--;
                _context.OrderItems.Update(item);

                if (item.Quantity == 0) //removes the item if the quantity reaches 0
                {
                    _context.OrderItems.Remove(item);
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Cart");
        }

        // CHECKOUT
        public async Task<IActionResult> Checkout()
        {
            // fetches the ID of user logged in
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // fetches items placed in the cart
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.UserId == userId && !o.IsCompleted);

            // fetches full name of customer making the order
            var customerName = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            // fetches total amount of the items in the cart
            var totalAmount = order.OrderItems.Sum(oi => oi.Price * oi.Quantity);

            if (order == null)
                return RedirectToAction("Cart");

            order.IsCompleted = true;
            order.OrderDate = DateTime.Now;
            order.CustomerName = $"{customerName.FirstName} {customerName.LastName}";
            order.TotalAmount = totalAmount;

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