using Microsoft.EntityFrameworkCore;
using OrderWebsiteASP.Data.Models;
using OrderWebsiteASP.Services.Core;
using OrderWebsiteASP.Tests.Helpers;

namespace OrderWebsiteASP.Tests
{
    public class OrderServiceTests
    {
        [Fact]
        public async Task GetUserOrdersAsync_ReturnsOnlyOrdersForRequestedUser()
        {
            using var context = TestDbHelper.CreateInMemoryDbContext();

            var restaurant = new Restaurant
            {
                Name = "Test Restaurant",
                Address = "Test Address"
            };

            context.Restaurants.Add(restaurant);
            await context.SaveChangesAsync();

            context.Orders.AddRange(
                new Order
                {
                    UserId = "user-1",
                    RestaurantId = restaurant.Id,
                    OrderDate = DateTime.UtcNow.AddDays(-1)
                },
                new Order
                {
                    UserId = "user-2",
                    RestaurantId = restaurant.Id,
                    OrderDate = DateTime.UtcNow
                });

            await context.SaveChangesAsync();

            var service = new OrderService(context);

            var result = await service.GetUserOrdersAsync("user-1");

            Assert.Single(result);
            Assert.Equal("user-1", result.First().UserId);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ReturnsNull_WhenOrderBelongsToDifferentUser()
        {
            using var context = TestDbHelper.CreateInMemoryDbContext();

            var restaurant = new Restaurant
            {
                Name = "Test Restaurant",
                Address = "Test Address"
            };

            context.Restaurants.Add(restaurant);
            await context.SaveChangesAsync();

            var order = new Order
            {
                UserId = "owner-user",
                RestaurantId = restaurant.Id,
                OrderDate = DateTime.UtcNow
            };

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var service = new OrderService(context);

            var result = await service.GetOrderByIdAsync(order.Id, "different-user");

            Assert.Null(result);
        }

        [Fact]
        public async Task RemoveItemAsync_DoesNotRemoveItem_WhenOrderBelongsToAnotherUser()
        {
            using var context = TestDbHelper.CreateInMemoryDbContext();

            var restaurant = new Restaurant
            {
                Name = "Restaurant",
                Address = "Address"
            };

            context.Restaurants.Add(restaurant);
            await context.SaveChangesAsync();

            var foodItem = new FoodItem
            {
                Name = "Pizza",
                Price = 12.50m,
                RestaurantId = restaurant.Id
            };

            context.FoodItems.Add(foodItem);
            await context.SaveChangesAsync();

            var order = new Order
            {
                UserId = "owner-user",
                RestaurantId = restaurant.Id,
                OrderDate = DateTime.UtcNow
            };

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                FoodItemId = foodItem.Id,
                Quantity = 1
            };

            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            var service = new OrderService(context);

            await service.RemoveItemAsync(orderItem.Id, order.Id, "different-user");

            Assert.Equal(1, context.OrderItems.Count());
        }

        [Fact]
        public async Task IncreaseQuantityAsync_DoesNotChangeQuantity_WhenOrderBelongsToAnotherUser()
        {
            using var context = TestDbHelper.CreateInMemoryDbContext();

            var restaurant = new Restaurant
            {
                Name = "Restaurant",
                Address = "Address"
            };

            context.Restaurants.Add(restaurant);
            await context.SaveChangesAsync();

            var foodItem = new FoodItem
            {
                Name = "Burger",
                Price = 10.00m,
                RestaurantId = restaurant.Id
            };

            context.FoodItems.Add(foodItem);
            await context.SaveChangesAsync();

            var order = new Order
            {
                UserId = "owner-user",
                RestaurantId = restaurant.Id,
                OrderDate = DateTime.UtcNow
            };

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                FoodItemId = foodItem.Id,
                Quantity = 1
            };

            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            var service = new OrderService(context);

            await service.IncreaseQuantityAsync(orderItem.Id, order.Id, "different-user");

            var updatedItem = await context.OrderItems.FirstAsync();
            Assert.Equal(1, updatedItem.Quantity);
        }

        [Fact]
        public async Task DecreaseQuantityAsync_RemovesItem_WhenQuantityBecomesZero()
        {
            using var context = TestDbHelper.CreateInMemoryDbContext();

            var restaurant = new Restaurant
            {
                Name = "Restaurant",
                Address = "Address"
            };

            context.Restaurants.Add(restaurant);
            await context.SaveChangesAsync();

            var foodItem = new FoodItem
            {
                Name = "Pasta",
                Price = 15.00m,
                RestaurantId = restaurant.Id
            };

            context.FoodItems.Add(foodItem);
            await context.SaveChangesAsync();

            var order = new Order
            {
                UserId = "owner-user",
                RestaurantId = restaurant.Id,
                OrderDate = DateTime.UtcNow
            };

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                FoodItemId = foodItem.Id,
                Quantity = 1
            };

            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            var service = new OrderService(context);

            await service.DecreaseQuantityAsync(orderItem.Id, order.Id, "owner-user");

            Assert.Empty(context.OrderItems);
        }

        [Fact]
        public async Task AddToCartAsync_CreatesOrderAndOrderItem_WhenNoExistingOrderExists()
        {
            using var context = TestDbHelper.CreateInMemoryDbContext();

            var restaurant = new Restaurant
            {
                Name = "Restaurant",
                Address = "Address"
            };

            context.Restaurants.Add(restaurant);
            await context.SaveChangesAsync();

            var foodItem = new FoodItem
            {
                Name = "Salad",
                Price = 8.50m,
                RestaurantId = restaurant.Id
            };

            context.FoodItems.Add(foodItem);
            await context.SaveChangesAsync();

            var service = new OrderService(context);

            await service.AddToCartAsync(foodItem.Id, restaurant.Id, "test-user");

            Assert.Single(context.Orders);
            Assert.Single(context.OrderItems);
        }
    }
}