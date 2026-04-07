using Microsoft.EntityFrameworkCore;
using OrderWebsiteASP.Data;
using OrderWebsiteASP.Data.Models;
using OrderWebsiteASP.Services.Core.Contracts;

namespace OrderWebsiteASP.Services.Core
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId, string userId)
        {
            return await _context.Orders
                .Include(o => o.Restaurant)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.FoodItem)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.Restaurant)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.FoodItem)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task CancelOrderAsync(int orderId, string userId)
        {
            if (orderId <= 0 || string.IsNullOrWhiteSpace(userId))
            {
                return;
            }

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
            {
                return;
            }

            if (order.OrderItems.Any())
            {
                _context.OrderItems.RemoveRange(order.OrderItems);
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task AddToCartAsync(int foodItemId, int restaurantId, string userId)
        {
            if (foodItemId <= 0 || restaurantId <= 0 || string.IsNullOrWhiteSpace(userId))
            {
                return;
            }

            var foodItem = await _context.FoodItems
                .FirstOrDefaultAsync(f => f.Id == foodItemId && f.RestaurantId == restaurantId);

            if (foodItem == null)
            {
                return;
            }

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.RestaurantId == restaurantId);

            if (order == null)
            {
                order = new Order
                {
                    UserId = userId,
                    RestaurantId = restaurantId,
                    OrderDate = DateTime.UtcNow
                };

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
            }

            var existingOrderItem = await _context.OrderItems
                .FirstOrDefaultAsync(oi => oi.OrderId == order.Id && oi.FoodItemId == foodItemId);

            if (existingOrderItem != null)
            {
                existingOrderItem.Quantity++;
            }
            else
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    FoodItemId = foodItemId,
                    Quantity = 1
                };

                await _context.OrderItems.AddAsync(orderItem);
            }

            await _context.SaveChangesAsync();
        }

        public async Task IncreaseQuantityAsync(int orderItemId, int orderId, string userId)
        {
            if (orderItemId <= 0 || orderId <= 0 || string.IsNullOrWhiteSpace(userId))
            {
                return;
            }

            var orderItem = await _context.OrderItems
                .Include(oi => oi.Order)
                .FirstOrDefaultAsync(oi => oi.Id == orderItemId && oi.OrderId == orderId);

            if (orderItem == null)
            {
                return;
            }

            if (orderItem.Order == null || orderItem.Order.UserId != userId)
            {
                return;
            }

            orderItem.Quantity++;
            await _context.SaveChangesAsync();
        }

        public async Task DecreaseQuantityAsync(int orderItemId, int orderId, string userId)
        {
            var orderItem = await _context.OrderItems
                .Include(oi => oi.Order)
                .FirstOrDefaultAsync(oi => oi.Id == orderItemId && oi.OrderId == orderId);

            if (orderItem == null)
            {
                return;
            }

            if (orderItem.Order == null || orderItem.Order.UserId != userId)
            {
                return;
            }

            if (orderItem.Quantity > 1)
            {
                orderItem.Quantity--;
                await _context.SaveChangesAsync();
                return;
            }

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(int orderItemId, int orderId, string userId)
        {
            if (orderItemId <= 0 || orderId <= 0 || string.IsNullOrWhiteSpace(userId))
            {
                return;
            }

            var orderItem = await _context.OrderItems
                .Include(oi => oi.Order)
                .FirstOrDefaultAsync(oi => oi.Id == orderItemId && oi.OrderId == orderId);

            if (orderItem == null)
            {
                return;
            }

            if (orderItem.Order == null || orderItem.Order.UserId != userId)
            {
                return;
            }

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task CheckoutAsync(string userId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefaultAsync();

            if (order == null || !order.OrderItems.Any())
            {
                return;
            }

            order.OrderDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}