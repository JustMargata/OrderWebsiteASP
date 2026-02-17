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

        public async Task<Order?> GetOrderByIdAsync(int id, string userId)
        {
            return await _context.Orders
                .Include(o => o.Restaurant)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.FoodItem)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);
        }

        public async Task AddToCartAsync(int foodItemId, int restaurantId, string userId)
        {
            var existingOrder = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.RestaurantId == restaurantId);

            if (existingOrder == null)
            {
                existingOrder = new Order
                {
                    UserId = userId,
                    RestaurantId = restaurantId,
                    OrderDate = DateTime.Now
                };
                _context.Orders.Add(existingOrder);
                await _context.SaveChangesAsync();
            }

            var existingItem = existingOrder.OrderItems
                .FirstOrDefault(oi => oi.FoodItemId == foodItemId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
                _context.Update(existingItem);
            }
            else
            {
                var orderItem = new OrderItem
                {
                    OrderId = existingOrder.Id,
                    FoodItemId = foodItemId,
                    Quantity = 1
                };
                _context.OrderItems.Add(orderItem);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(int orderItemId, int orderId)
        {
            var orderItem = await _context.OrderItems.FindAsync(orderItemId);
            if (orderItem == null) return;

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order != null && !order.OrderItems.Any())
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CancelOrderAsync(int orderId, string userId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null) return;

            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task IncreaseQuantityAsync(int orderItemId)
        {
            var orderItem = await _context.OrderItems.FindAsync(orderItemId);
            if (orderItem == null) return;

            orderItem.Quantity++;
            _context.Update(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task DecreaseQuantityAsync(int orderItemId, int orderId)
        {
            var orderItem = await _context.OrderItems.FindAsync(orderItemId);
            if (orderItem == null) return;

            if (orderItem.Quantity > 1)
            {
                orderItem.Quantity--;
                _context.Update(orderItem);
                await _context.SaveChangesAsync();
            }
            else
            {
                _context.OrderItems.Remove(orderItem);
                await _context.SaveChangesAsync();

                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order != null && !order.OrderItems.Any())
                {
                    _context.Orders.Remove(order);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}