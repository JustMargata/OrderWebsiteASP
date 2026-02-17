using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderWebsiteASP.Data;
using OrderWebsiteASP.Data.Models;

namespace OrderWebsiteASP.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var orders = await _context.Orders
                .Include(o => o.Restaurant)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.FoodItem)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            var order = await _context.Orders
                .Include(o => o.Restaurant)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.FoodItem)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int foodItemId, int restaurantId)
        {
            var userId = _userManager.GetUserId(User);

            var existingOrder = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.RestaurantId == restaurantId);

            if (existingOrder == null)
            {
                existingOrder = new Order
                {
                    UserId = userId,
                    RestaurantId = restaurantId,
                    OrderDate = DateTime.Now
                };
                _context.Orders.Add(existingOrder);
                await _context.SaveChangesAsync();
            }

            var existingOrderItem = existingOrder.OrderItems
                .FirstOrDefault(oi => oi.FoodItemId == foodItemId);

            if (existingOrderItem != null)
            {
                existingOrderItem.Quantity++;
                _context.Update(existingOrderItem);
            }
            else
            {
                var orderItem = new OrderItem
                {
                    OrderId = existingOrder.Id,
                    FoodItemId = foodItemId,
                    Quantity = 1
                };
                _context.OrderItems.Add(orderItem);
            }

            await _context.SaveChangesAsync();

            var foodItem = await _context.FoodItems.FindAsync(foodItemId);
            TempData["Success"] = $"{foodItem.Name} was added to your order!";

            return RedirectToAction("Details", "Restaurants", new { id = restaurantId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItem(int orderItemId, int orderId)
        {
            var orderItem = await _context.OrderItems.FindAsync(orderItemId);
            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
                await _context.SaveChangesAsync();
            }

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order != null && !order.OrderItems.Any())
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Details), new { id = orderId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var userId = _userManager.GetUserId(User);

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order != null)
            {
                _context.OrderItems.RemoveRange(order.OrderItems);
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IncreaseQuantity(int orderItemId, int orderId)
        {
            var orderItem = await _context.OrderItems.FindAsync(orderItemId);
            if (orderItem != null)
            {
                orderItem.Quantity++;
                _context.Update(orderItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = orderId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DecreaseQuantity(int orderItemId, int orderId)
        {
            var orderItem = await _context.OrderItems.FindAsync(orderItemId);
            if (orderItem != null)
            {
                if (orderItem.Quantity > 1)
                {
                    orderItem.Quantity--;
                    _context.Update(orderItem);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _context.OrderItems.Remove(orderItem);
                    await _context.SaveChangesAsync();

                    var order = await _context.Orders
                        .Include(o => o.OrderItems)
                        .FirstOrDefaultAsync(o => o.Id == orderId);

                    if (order != null && !order.OrderItems.Any())
                    {
                        _context.Orders.Remove(order);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            return RedirectToAction(nameof(Details), new { id = orderId });
        }
    }
}