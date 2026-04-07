using OrderWebsiteASP.Data.Models;
using OrderWebsiteASP.Services.Core;
using OrderWebsiteASP.Tests.Helpers;

namespace OrderWebsiteASP.Tests
{
    public class PromotionServiceTests
    {
        [Fact]
        public async Task GetActivePromotionsPagedAsync_FiltersByMinimumDiscount()
        {
            using var context = TestDbHelper.CreateInMemoryDbContext();

            var restaurant = new Restaurant
            {
                Name = "Test Restaurant",
                Address = "Test Address"
            };

            context.Restaurants.Add(restaurant);
            await context.SaveChangesAsync();

            context.Promotions.AddRange(
                new Promotion
                {
                    Title = "Low Discount",
                    Description = "10 percent off",
                    DiscountPercent = 10,
                    StartDate = DateTime.UtcNow.AddDays(-2),
                    EndDate = DateTime.UtcNow.AddDays(5),
                    RestaurantId = restaurant.Id
                },
                new Promotion
                {
                    Title = "High Discount",
                    Description = "30 percent off",
                    DiscountPercent = 30,
                    StartDate = DateTime.UtcNow.AddDays(-2),
                    EndDate = DateTime.UtcNow.AddDays(5),
                    RestaurantId = restaurant.Id
                });

            await context.SaveChangesAsync();

            var service = new PromotionService(context);

            var result = await service.GetActivePromotionsPagedAsync(1, 10, null, 20);

            Assert.Single(result.Items);
            Assert.Equal("High Discount", result.Items.First().Title);
        }

        [Fact]
        public async Task GetActivePromotionsPagedAsync_FiltersByRestaurantId()
        {
            using var context = TestDbHelper.CreateInMemoryDbContext();

            var restaurant1 = new Restaurant
            {
                Name = "Restaurant One",
                Address = "Address One"
            };

            var restaurant2 = new Restaurant
            {
                Name = "Restaurant Two",
                Address = "Address Two"
            };

            context.Restaurants.AddRange(restaurant1, restaurant2);
            await context.SaveChangesAsync();

            context.Promotions.AddRange(
                new Promotion
                {
                    Title = "Promo 1",
                    Description = "Promo for restaurant one",
                    DiscountPercent = 15,
                    StartDate = DateTime.UtcNow.AddDays(-1),
                    EndDate = DateTime.UtcNow.AddDays(3),
                    RestaurantId = restaurant1.Id
                },
                new Promotion
                {
                    Title = "Promo 2",
                    Description = "Promo for restaurant two",
                    DiscountPercent = 25,
                    StartDate = DateTime.UtcNow.AddDays(-1),
                    EndDate = DateTime.UtcNow.AddDays(3),
                    RestaurantId = restaurant2.Id
                });

            await context.SaveChangesAsync();

            var service = new PromotionService(context);

            var result = await service.GetActivePromotionsPagedAsync(1, 10, restaurant1.Id, null);

            Assert.Single(result.Items);
            Assert.Equal("Promo 1", result.Items.First().Title);
        }
    }
}