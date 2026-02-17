using OrderWebsiteASP.Data.Models;

namespace OrderWebsiteASP.Services.Core.Contracts
{
    public interface IRestaurantService
    {
        Task<IEnumerable<Restaurant>> GetAllAsync();
        Task<Restaurant?> GetByIdAsync(int id);
        Task CreateAsync(string name, string address, string? imageUrl);
        Task EditAsync(int id, string name, string address, string? imageUrl);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}