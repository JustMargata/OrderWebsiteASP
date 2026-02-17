using OrderWebsiteASP.Data.Models;

namespace OrderWebsiteASP.Services.Core.Contracts
{
    public interface IPromotionService
    {
        Task<IEnumerable<Promotion>> GetActivePromotionsAsync();
        Task<Promotion?> GetByIdAsync(int id);
        Task<Promotion?> GetByIdWithRestaurantAsync(int id);
        Task CreateAsync(Promotion promotion);
        Task EditAsync(Promotion promotion);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}