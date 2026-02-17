using OrderWebsiteASP.Data.Models;

namespace OrderWebsiteASP.Services.Core.Contracts
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task<Order?> GetOrderByIdAsync(int id, string userId);
        Task AddToCartAsync(int foodItemId, int restaurantId, string userId);
        Task RemoveItemAsync(int orderItemId, int orderId);
        Task CancelOrderAsync(int orderId, string userId);
        Task IncreaseQuantityAsync(int orderItemId);
        Task DecreaseQuantityAsync(int orderItemId, int orderId);
    }
}