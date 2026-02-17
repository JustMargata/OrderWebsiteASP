using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderWebsiteASP.Services.Core.Contracts;
using OrderWebsiteASP.ViewModels.Restaurants;

namespace OrderWebsiteASP.Controllers
{
    [Authorize]
    public class RestaurantsController : BaseController
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        public async Task<IActionResult> Index()
        {
            var restaurants = await _restaurantService.GetAllAsync();
            return View(restaurants);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var restaurant = await _restaurantService.GetByIdAsync(id.Value);
            if (restaurant == null) return NotFound();

            return View(restaurant);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(RestaurantCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _restaurantService.CreateAsync(model.Name, model.Address, model.ImageUrl);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var restaurant = await _restaurantService.GetByIdAsync(id.Value);
            if (restaurant == null) return NotFound();

            var model = new RestaurantEditViewModel
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Address = restaurant.Address,
                ImageUrl = restaurant.ImageUrl
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, RestaurantEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid) return View(model);

            await _restaurantService.EditAsync(model.Id, model.Name, model.Address, model.ImageUrl);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var restaurant = await _restaurantService.GetByIdAsync(id.Value);
            if (restaurant == null) return NotFound();

            var model = new RestaurantDeleteViewModel
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Address = restaurant.Address,
                ImageUrl = restaurant.ImageUrl
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _restaurantService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}