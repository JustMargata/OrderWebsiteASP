using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderWebsiteASP.Services.Core.Contracts;
using OrderWebsiteASP.ViewModels.FoodItems;

namespace OrderWebsiteASP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FoodItemsController : BaseController
    {
        private readonly IFoodItemService _foodItemService;

        public FoodItemsController(IFoodItemService foodItemService)
        {
            _foodItemService = foodItemService;
        }

        public IActionResult Create(int restaurantId)
        {
            var model = new FoodItemCreateViewModel
            {
                RestaurantId = restaurantId
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FoodItemCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _foodItemService.CreateAsync(model.Name, model.Price, model.ImageUrl, model.RestaurantId);
            return RedirectToAction("Details", "Restaurants", new { id = model.RestaurantId });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var foodItem = await _foodItemService.GetByIdAsync(id.Value);
            if (foodItem == null) return NotFound();

            var model = new FoodItemEditViewModel
            {
                Id = foodItem.Id,
                Name = foodItem.Name,
                Price = foodItem.Price,
                ImageUrl = foodItem.ImageUrl,
                RestaurantId = foodItem.RestaurantId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FoodItemEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid) return View(model);

            await _foodItemService.EditAsync(model.Id, model.Name, model.Price, model.ImageUrl, model.RestaurantId);
            return RedirectToAction("Details", "Restaurants", new { id = model.RestaurantId });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var foodItem = await _foodItemService.GetByIdWithRestaurantAsync(id.Value);
            if (foodItem == null) return NotFound();

            var model = new FoodItemDeleteViewModel
            {
                Id = foodItem.Id,
                Name = foodItem.Name,
                Price = foodItem.Price,
                ImageUrl = foodItem.ImageUrl,
                RestaurantId = foodItem.RestaurantId,
                RestaurantName = foodItem.Restaurant.Name
            };

            return View(model);
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