using Microsoft.EntityFrameworkCore;
using OrderWebsiteASP.Data;
using OrderWebsiteASP.Data.Models;
using OrderWebsiteASP.Services.Core.Contracts;

namespace OrderWebsiteASP.Services.Core
{
    public class FoodItemService : IFoodItemService
    {
        private readonly ApplicationDbContext _context;

        public FoodItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FoodItem?> GetByIdAsync(int id)
        {
            return await _context.FoodItems.FindAsync(id);
        }

        public async Task<FoodItem?> GetByIdWithRestaurantAsync(int id)
        {
            return await _context.FoodItems
                .Include(f => f.Restaurant)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task CreateAsync(string name, decimal price, string? imageUrl, int restaurantId)
        {
            var foodItem = new FoodItem
            {
                Name = name,
                Price = price,
                ImageUrl = imageUrl,
                RestaurantId = restaurantId
            };

            await _context.FoodItems.AddAsync(foodItem);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(int id, string name, decimal price, string? imageUrl, int restaurantId)
        {
            var foodItem = await _context.FoodItems.FindAsync(id);
            if (foodItem == null) return;

            foodItem.Name = name;
            foodItem.Price = price;
            foodItem.ImageUrl = imageUrl;
            foodItem.RestaurantId = restaurantId;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var foodItem = await _context.FoodItems.FindAsync(id);
            if (foodItem == null) return;

            _context.FoodItems.Remove(foodItem);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.FoodItems.AnyAsync(f => f.Id == id);
        }
    }
}