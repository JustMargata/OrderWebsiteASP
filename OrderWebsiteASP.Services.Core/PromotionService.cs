using Microsoft.EntityFrameworkCore;
using OrderWebsiteASP.Data;
using OrderWebsiteASP.Data.Models;
using OrderWebsiteASP.Services.Core.Contracts;
using OrderWebsiteASP.ViewModels;

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
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PagedResultViewModel<Promotion>> GetActivePromotionsPagedAsync(
    int page,
    int pageSize,
    int? restaurantId = null,
    decimal? minDiscount = null)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 6 : pageSize;

            IQueryable<Promotion> query = _context.Promotions
                .Include(p => p.Restaurant)
                .Where(p => p.EndDate >= DateTime.Now)
                .AsNoTracking();

            if (restaurantId.HasValue)
            {
                query = query.Where(p => p.RestaurantId == restaurantId.Value);
            }

            if (minDiscount.HasValue)
            {
                query = query.Where(p => p.DiscountPercent >= minDiscount.Value);
            }

            query = query.OrderBy(p => p.EndDate);

            int totalItems = await query.CountAsync();

            List<Promotion> items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResultViewModel<Promotion>
            {
                Items = items,
                Pagination = new PaginationViewModel
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalItems,
                    RestaurantId = restaurantId,
                    MinDiscount = minDiscount
                }
            };
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
            if (promotion == null)
            {
                return;
            }

            _context.Promotions.Remove(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Promotions.AnyAsync(p => p.Id == id);
        }
    }
}