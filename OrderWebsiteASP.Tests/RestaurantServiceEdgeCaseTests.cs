using OrderWebsiteASP.Data.Models;
using OrderWebsiteASP.Services.Core;
using OrderWebsiteASP.Tests.Helpers;

namespace OrderWebsiteASP.Tests
{
    public class RestaurantServiceEdgeCaseTests
    {
        [Fact]
        public async Task GetPagedAsync_ReturnsFirstPage_WhenPageIsLessThanOne()
        {
            using var context = TestDbHelper.CreateInMemoryDbContext();

            for (int i = 1; i <= 5; i++)
            {
                context.Restaurants.Add(new Restaurant
                {
                    Name = $"Restaurant {i}",
                    Address = $"Address {i}"
                });
            }

            await context.SaveChangesAsync();

            var service = new RestaurantService(context);

            var result = await service.GetPagedAsync(0, 2);

            Assert.Equal(1, result.Pagination.CurrentPage);
            Assert.Equal(2, result.Items.Count());
        }

        [Fact]
        public async Task GetPagedAsync_ReturnsEmptyCollection_WhenSearchTermDoesNotMatch()
        {
            using var context = TestDbHelper.CreateInMemoryDbContext();

            context.Restaurants.Add(new Restaurant
            {
                Name = "Happy",
                Address = "Sofia"
            });

            await context.SaveChangesAsync();

            var service = new RestaurantService(context);

            var result = await service.GetPagedAsync(1, 10, "non-existing-search");

            Assert.Empty(result.Items);
        }

        [Fact]
        public async Task ExistsAsync_ReturnsFalse_WhenRestaurantDoesNotExist()
        {
            using var context = TestDbHelper.CreateInMemoryDbContext();

            var service = new RestaurantService(context);

            var exists = await service.ExistsAsync(999);

            Assert.False(exists);
        }
    }
}