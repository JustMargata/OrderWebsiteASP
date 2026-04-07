using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrderWebsiteASP.Data.Models;
using OrderWebsiteASP.Services.Core.Contracts;

namespace OrderWebsiteASP.Controllers
{
    public class PromotionsController : BaseController
    {
        private readonly IPromotionService _promotionService;
        private readonly IRestaurantService _restaurantService;

        public PromotionsController(IPromotionService promotionService, IRestaurantService restaurantService)
        {
            _promotionService = promotionService;
            _restaurantService = restaurantService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 6, int? restaurantId = null, decimal? minDiscount = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 6;
            }

            var promotions = await _promotionService.GetActivePromotionsPagedAsync(page, pageSize, restaurantId, minDiscount);
            ViewBag.Restaurants = await _restaurantService.GetAllForSelectAsync();

            return View(promotions);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var promotion = await _promotionService.GetByIdWithRestaurantAsync(id.Value);
            if (promotion == null)
            {
                return NotFound();
            }

            return View(promotion);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var restaurants = await _restaurantService.GetAllForSelectAsync();
            ViewBag.Restaurants = new SelectList(restaurants, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Title,Description,DiscountPercent,StartDate,EndDate,ImageUrl,RestaurantId")] Promotion promotion)
        {
            if (promotion.RestaurantId <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.Remove("Restaurant");

            if (!ModelState.IsValid)
            {
                var restaurants = await _restaurantService.GetAllForSelectAsync();
                ViewBag.Restaurants = new SelectList(restaurants, "Id", "Name", promotion.RestaurantId);
                return View(promotion);
            }

            await _promotionService.CreateAsync(promotion);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var promotion = await _promotionService.GetByIdAsync(id.Value);
            if (promotion == null)
            {
                return NotFound();
            }

            var restaurants = await _restaurantService.GetAllForSelectAsync();
            ViewBag.Restaurants = new SelectList(restaurants, "Id", "Name", promotion.RestaurantId);

            return View(promotion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,DiscountPercent,StartDate,EndDate,ImageUrl,RestaurantId")] Promotion promotion)
        {
            if (id <= 0 || promotion.Id <= 0 || id != promotion.Id)
            {
                return NotFound();
            }

            if (!await _promotionService.ExistsAsync(id))
            {
                return NotFound();
            }

            ModelState.Remove("Restaurant");

            if (!ModelState.IsValid)
            {
                var restaurants = await _restaurantService.GetAllForSelectAsync();
                ViewBag.Restaurants = new SelectList(restaurants, "Id", "Name", promotion.RestaurantId);
                return View(promotion);
            }

            await _promotionService.EditAsync(promotion);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var promotion = await _promotionService.GetByIdWithRestaurantAsync(id.Value);
            if (promotion == null)
            {
                return NotFound();
            }

            return View(promotion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            if (!await _promotionService.ExistsAsync(id))
            {
                return RedirectToAction(nameof(Index));
            }

            await _promotionService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}