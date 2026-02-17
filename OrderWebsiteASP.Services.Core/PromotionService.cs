using Microsoft.EntityFrameworkCore;
using OrderWebsiteASP.Data;
using OrderWebsiteASP.Data.Models;
using OrderWebsiteASP.Services.Core.Contracts;

namespace OrderWebsiteASP.Services.Core
{
    public class PromotionService : IPromotionService
    {
        private readonly ApplicationDbContext _context;

        public PromotionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Promotion>> GetActivePromotionsAsync()
        {
            return await _context.Promotions
                .Include(p => p.Restaurant)
                .Where(p => p.EndDate >= DateTime.Now)
                .OrderBy(p => p.EndDate)
                .ToListAsync();
        }

        public async Task<Promotion?> GetByIdAsync(int id)
        {
            return await _context.Promotions.FindAsync(id);
        }

        public async Task<Promotion?> GetByIdWithRestaurantAsync(int id)
        {
            return await _context.Promotions
                .Include(p => p.Restaurant)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreateAsync(Promotion promotion)
        {
            await _context.Promotions.AddAsync(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(Promotion promotion)
        {
            _context.Promotions.Update(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion == null) return;

            _context.Promotions.Remove(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Promotions.AnyAsync(p => p.Id == id);
        }
    }
}