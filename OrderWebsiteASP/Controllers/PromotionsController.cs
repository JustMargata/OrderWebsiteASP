using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrderWebsiteASP.Data;
using OrderWebsiteASP.Data.Models;
using OrderWebsiteASP.Services.Core.Contracts;

namespace OrderWebsiteASP.Controllers
{
    public class PromotionsController : Controller
    {
        private readonly IPromotionService _promotionService;
        private readonly ApplicationDbContext _context;

        public PromotionsController(IPromotionService promotionService, ApplicationDbContext context)
        {
            _promotionService = promotionService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var promotions = await _promotionService.GetActivePromotionsAsync();
            return View(promotions);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var promotion = await _promotionService.GetByIdWithRestaurantAsync(id.Value);
            if (promotion == null) return NotFound();

            return View(promotion);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Restaurants = new SelectList(_context.Restaurants, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Title,Description,DiscountPercent,StartDate,EndDate,ImageUrl,RestaurantId")] Promotion promotion)
        {
            ModelState.Remove("Restaurant");

            if (!ModelState.IsValid)
            {
                ViewBag.Restaurants = new SelectList(_context.Restaurants, "Id", "Name");
                return View(promotion);
            }

            await _promotionService.CreateAsync(promotion);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var promotion = await _promotionService.GetByIdAsync(id.Value);
            if (promotion == null) return NotFound();

            ViewBag.Restaurants = new SelectList(_context.Restaurants, "Id", "Name", promotion.RestaurantId);
            return View(promotion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,DiscountPercent,StartDate,EndDate,ImageUrl,RestaurantId")] Promotion promotion)
        {
            if (id != promotion.Id) return NotFound();

            ModelState.Remove("Restaurant");

            if (!ModelState.IsValid)
            {
                ViewBag.Restaurants = new SelectList(_context.Restaurants, "Id", "Name", promotion.RestaurantId);
                return View(promotion);
            }

            await _promotionService.EditAsync(promotion);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var promotion = await _promotionService.GetByIdWithRestaurantAsync(id.Value);
            if (promotion == null) return NotFound();

            return View(promotion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _promotionService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}