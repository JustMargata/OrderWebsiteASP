using OrderWebsiteASP.Data.Models;
using OrderWebsiteASP.Services.Core;
using OrderWebsiteASP.Tests.Helpers;

namespace OrderWebsiteASP.Tests
{
    public class PromotionServiceEdgeCaseTests
    {
        [Fact]
        public async Task GetActivePromotionsPagedAsync_DoesNotReturnExpiredPromotions()
        {
            using var context = TestDbHelper.CreateInMemoryDbContext();

            var restaurant = new Restaurant
            {
                Name = "Restaurant",
                Address = "Address"
            };

            context.Restaurants.Add(restaurant);
            await context.SaveChangesAsync();

            context.Promotions.AddRange(
                new Promotion
                {
                    Title = "Expired Promo",
                    Description = "Old promo",
                    DiscountPercent = 20,
                    StartDate = DateTime.UtcNow.AddDays(-10),
                    EndDate = DateTime.UtcNow.AddDays(-1),
                    RestaurantId = restaurant.Id
                },
                new Promotion
                {
                    Title = "Active Promo",
                    Description = "Current promo",
                    DiscountPercent = 25,
                    StartDate = DateTime.UtcNow.AddDays(-1),
                    EndDate = DateTime.UtcNow.AddDays(5),
                    RestaurantId = restaurant.Id
                });

            await context.SaveChangesAsync();

            var service = new PromotionService(context);

            var result = await service.GetActivePromotionsPagedAsync(1, 10);

            Assert.Single(result.Items);
            Assert.Equal("Active Promo", result.Items.First().Title);
        }

        [Fact]
        public async Task GetActivePromotionsPagedAsync_ReturnsEmptyCollection_WhenRestaurantFilterDoesNotMatch()
        {
            using var context = TestDbHelper.CreateInMemoryDbContext();

            var restaurant = new Restaurant
            {
                Name = "Restaurant",
                Address = "Address"
            };

            context.Restaurants.Add(restaurant);
            await context.SaveChangesAsync();

            context.Promotions.Add(new Promotion
            {
                Title = "Promo",
                Description = "Discount",
                DiscountPercent = 20,
                StartDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow.AddDays(3),
                RestaurantId = restaurant.Id
            });

            await context.SaveChangesAsync();

            var service = new PromotionService(context);

            var result = await service.GetActivePromotionsPagedAsync(1, 10, 999, null);

            Assert.Empty(result.Items);
        }

        [Fact]
        public async Task ExistsAsync_ReturnsFalse_WhenPromotionDoesNotExist()
        {
            using var context = TestDbHelper.CreateInMemoryDbContext();

            var service = new PromotionService(context);

            var exists = await service.ExistsAsync(999);

            Assert.False(exists);
        }
    }
}