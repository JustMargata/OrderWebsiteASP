using OrderWebsiteASP.Data.Models;

namespace OrderWebsiteASP.Services.Core.Contracts
{
    public interface IFoodItemService
    {
        Task<FoodItem?> GetByIdAsync(int id);
        Task<FoodItem?> GetByIdWithRestaurantAsync(int id);
        Task CreateAsync(string name, decimal price, string? imageUrl, int restaurantId);
        Task EditAsync(int id, string name, decimal price, string? imageUrl, int restaurantId);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}