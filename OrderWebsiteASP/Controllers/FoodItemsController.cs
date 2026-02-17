using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderWebsiteASP.Data;
using OrderWebsiteASP.Data.Models;

namespace OrderWebsiteASP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FoodItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FoodItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create(int restaurantId)
        {
            ViewBag.RestaurantId = restaurantId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,ImageUrl,RestaurantId")] FoodItem foodItem)
        {
            ModelState.Remove("Restaurant");
            ModelState.Remove("OrderItems");

            if (ModelState.IsValid)
            {
                _context.Add(foodItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Restaurants", new { id = foodItem.RestaurantId });
            }

            ViewBag.RestaurantId = foodItem.RestaurantId;
            return View(foodItem);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var foodItem = await _context.FoodItems.FindAsync(id);
            if (foodItem == null) return NotFound();

            return View(foodItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Price,ImageUrl,RestaurantId")] FoodItem foodItem)
        {
            foodItem.Id = id;

            ModelState.Remove("Restaurant");
            ModelState.Remove("OrderItems");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(foodItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodItemExists(foodItem.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction("Details", "Restaurants", new { id = foodItem.RestaurantId });
            }
            return View(foodItem);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var foodItem = await _context.FoodItems
                .Include(f => f.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (foodItem == null) return NotFound();

            return View(foodItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var foodItem = await _context.FoodItems.FindAsync(id);
            var restaurantId = foodItem.RestaurantId;
            if (foodItem != null)
            {
                _context.FoodItems.Remove(foodItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Restaurants", new { id = restaurantId });
        }

        private bool FoodItemExists(int id)
        {
            return _context.FoodItems.Any(e => e.Id == id);
        }
    }
}