using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderWebsiteASP.Services.Core.Contracts;

namespace OrderWebsiteASP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FoodItemsController : Controller
    {
        private readonly IFoodItemService _foodItemService;

        public FoodItemsController(IFoodItemService foodItemService)
        {
            _foodItemService = foodItemService;
        }

        public IActionResult Create(int restaurantId)
        {
            ViewBag.RestaurantId = restaurantId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name, decimal price, string? imageUrl, int restaurantId)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.RestaurantId = restaurantId;
                return View();
            }

            await _foodItemService.CreateAsync(name, price, imageUrl, restaurantId);
            return RedirectToAction("Details", "Restaurants", new { id = restaurantId });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var foodItem = await _foodItemService.GetByIdAsync(id.Value);
            if (foodItem == null) return NotFound();

            return View(foodItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string name, decimal price, string? imageUrl, int restaurantId)
        {
            if (!await _foodItemService.ExistsAsync(id)) return NotFound();

            if (!ModelState.IsValid)
            {
                var foodItem = await _foodItemService.GetByIdAsync(id);
                return View(foodItem);
            }

            await _foodItemService.EditAsync(id, name, price, imageUrl, restaurantId);
            return RedirectToAction("Details", "Restaurants", new { id = restaurantId });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var foodItem = await _foodItemService.GetByIdWithRestaurantAsync(id.Value);
            if (foodItem == null) return NotFound();

            return View(foodItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var foodItem = await _foodItemService.GetByIdAsync(id);
            var restaurantId = foodItem?.RestaurantId ?? 0;

            await _foodItemService.DeleteAsync(id);
            return RedirectToAction("Details", "Restaurants", new { id = restaurantId });
        }
    }
}