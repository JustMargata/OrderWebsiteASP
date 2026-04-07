using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderWebsiteASP.Services.Core.Contracts;

namespace OrderWebsiteASP.Controllers
{
    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return View(orders);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var userId = GetUserId();
            var order = await _orderService.GetOrderByIdAsync(id.Value, userId);

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
            if (foodItemId <= 0 || restaurantId <= 0)
            {
                TempData["Error"] = "Invalid item request.";
                return RedirectToAction("Index", "Restaurants");
            }

            var userId = GetUserId();
            await _orderService.AddToCartAsync(foodItemId, restaurantId, userId);

            TempData["Success"] = "Item was added to your order!";
            return RedirectToAction("Details", "Restaurants", new { id = restaurantId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItem(int orderItemId, int orderId)
        {
            if (orderItemId <= 0 || orderId <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var userId = GetUserId();
            await _orderService.RemoveItemAsync(orderItemId, orderId, userId);

            return RedirectToAction(nameof(Details), new { id = orderId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            if (orderId <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var userId = GetUserId();
            await _orderService.CancelOrderAsync(orderId, userId);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IncreaseQuantity(int orderItemId, int orderId)
        {
            if (orderItemId <= 0 || orderId <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var userId = GetUserId();
            await _orderService.IncreaseQuantityAsync(orderItemId, orderId, userId);

            return RedirectToAction(nameof(Details), new { id = orderId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DecreaseQuantity(int orderItemId, int orderId)
        {
            if (orderItemId <= 0 || orderId <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var userId = GetUserId();
            await _orderService.DecreaseQuantityAsync(orderItemId, orderId, userId);

            return RedirectToAction(nameof(Details), new { id = orderId });
        }
    }
}