using OrderWebsiteASP.Data.Models;

namespace OrderWebsiteASP.Services.Core.Contracts
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);

        Task<Order?> GetOrderByIdAsync(int orderId, string userId);

        Task AddToCartAsync(int foodItemId, int restaurantId, string userId);

        Task RemoveItemAsync(int orderItemId, int orderId, string userId);

        Task CancelOrderAsync(int orderId, string userId);

        Task IncreaseQuantityAsync(int orderItemId, int orderId, string userId);

        Task DecreaseQuantityAsync(int orderItemId, int orderId, string userId);
    }
}