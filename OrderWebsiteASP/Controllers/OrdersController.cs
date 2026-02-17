using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderWebsiteASP.Services.Core.Contracts;

namespace OrderWebsiteASP.Controllers
{
    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(IOrderService orderService, UserManager<IdentityUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return View(orders);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            var order = await _orderService.GetOrderByIdAsync(id.Value, userId);

            if (order == null) return NotFound();

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int foodItemId, int restaurantId)
        {
            var userId = _userManager.GetUserId(User);
            await _orderService.AddToCartAsync(foodItemId, restaurantId, userId);

            TempData["Success"] = "Item was added to your order!";
            return RedirectToAction("Details", "Restaurants", new { id = restaurantId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItem(int orderItemId, int orderId)
        {
            await _orderService.RemoveItemAsync(orderItemId, orderId);
            return RedirectToAction(nameof(Details), new { id = orderId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var userId = _userManager.GetUserId(User);
            await _orderService.CancelOrderAsync(orderId, userId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IncreaseQuantity(int orderItemId, int orderId)
        {
            await _orderService.IncreaseQuantityAsync(orderItemId);
            return RedirectToAction(nameof(Details), new { id = orderId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DecreaseQuantity(int orderItemId, int orderId)
        {
            await _orderService.DecreaseQuantityAsync(orderItemId, orderId);
            return RedirectToAction(nameof(Details), new { id = orderId });
        }
    }
}