using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderWebsiteASP.Services.Core.Contracts;

namespace OrderWebsiteASP.Controllers
{
    [Authorize]
    public class RestaurantsController : Controller
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
        public async Task<IActionResult> Create([Bind("Name,Address,ImageUrl")] string name, string address, string? imageUrl)
        {
            if (!ModelState.IsValid) return View();

            await _restaurantService.CreateAsync(name, address, imageUrl);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var restaurant = await _restaurantService.GetByIdAsync(id.Value);
            if (restaurant == null) return NotFound();

            return View(restaurant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, string name, string address, string? imageUrl)
        {
            if (!await _restaurantService.ExistsAsync(id)) return NotFound();

            if (!ModelState.IsValid)
            {
                var restaurant = await _restaurantService.GetByIdAsync(id);
                return View(restaurant);
            }

            await _restaurantService.EditAsync(id, name, address, imageUrl);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var restaurant = await _restaurantService.GetByIdAsync(id.Value);
            if (restaurant == null) return NotFound();

            return View(restaurant);
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