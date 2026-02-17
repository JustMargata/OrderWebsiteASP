using Microsoft.EntityFrameworkCore;
using OrderWebsiteASP.Data;
using OrderWebsiteASP.Data.Models;
using OrderWebsiteASP.Services.Core.Contracts;

namespace OrderWebsiteASP.Services.Core
{
    public class RestaurantService : IRestaurantService
    {
        private readonly ApplicationDbContext _context;

        public RestaurantService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            return await _context.Restaurants
                .Include(r => r.FoodItems)
                .ToListAsync();
        }

        public async Task<IEnumerable<Restaurant>> GetAllForSelectAsync()
        {
            return await _context.Restaurants
                .OrderBy(r => r.Name)
                .ToListAsync();
        }

        public async Task<Restaurant?> GetByIdAsync(int id)
        {
            return await _context.Restaurants
                .Include(r => r.FoodItems)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task CreateAsync(string name, string address, string? imageUrl)
        {
            var restaurant = new Restaurant
            {
                Name = name,
                Address = address,
                ImageUrl = imageUrl
            };

            await _context.Restaurants.AddAsync(restaurant);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(int id, string name, string address, string? imageUrl)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null) return;

            restaurant.Name = name;
            restaurant.Address = address;
            restaurant.ImageUrl = imageUrl;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null) return;

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Restaurants.AnyAsync(r => r.Id == id);
        }
    }
}