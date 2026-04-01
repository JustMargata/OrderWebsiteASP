using OrderWebsiteASP.Data.Models;
using OrderWebsiteASP.ViewModels;

namespace OrderWebsiteASP.Services.Core.Contracts
{
    public interface IRestaurantService
    {
        Task<IEnumerable<Restaurant>> GetAllAsync();

        Task<PagedResultViewModel<Restaurant>> GetPagedAsync(int page, int pageSize, string? searchTerm = null);

        Task<IEnumerable<Restaurant>> GetAllForSelectAsync();

        Task<Restaurant?> GetByIdAsync(int id);

        Task CreateAsync(string name, string address, string? imageUrl);

        Task EditAsync(int id, string name, string address, string? imageUrl);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
    }
}