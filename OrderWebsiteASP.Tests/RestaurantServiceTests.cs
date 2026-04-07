using OrderWebsiteASP.Data.Models;
using OrderWebsiteASP.Services.Core;
using OrderWebsiteASP.Tests.Helpers;

namespace OrderWebsiteASP.Tests
{
    public class RestaurantServiceTests
    {
        [Fact]
        public async Task GetPagedAsync_ReturnsCorrectNumberOfItemsForRequestedPage()
        {
            using var context = TestDbHelper.CreateInMemoryDbContext();

            for (int i = 1; i <= 10; i++)
            {
                context.Restaurants.Add(new Restaurant
                {
                    Name = $"Restaurant {i}",
                    Address = $"Address {i}"
                });
            }

            await context.SaveChangesAsync();

            var service = new RestaurantService(context);

            var result = await service.GetPagedAsync(2, 3);

            Assert.Equal(3, result.Items.Count());
            Assert.Equal(2, result.Pagination.CurrentPage);
            Assert.Equal(3, result.Pagination.PageSize);
            Assert.Equal(10, result.Pagination.TotalItems);
        }

        [Fact]
        public async Task GetPagedAsync_FiltersBySearchTermInNameAndAddress()
        {
            using var context = TestDbHelper.CreateInMemoryDbContext();

            context.Restaurants.AddRange(
                new Restaurant
                {
                    Name = "Happy Bar and Grill",
                    Address = "Sofia Center"
                },
                new Restaurant
                {
                    Name = "Pizza Place",
                    Address = "Plovdiv"
                },
                new Restaurant
                {
                    Name = "Burger House",
                    Address = "Happy Street"
                });

            await context.SaveChangesAsync();

            var service = new RestaurantService(context);

            var result = await service.GetPagedAsync(1, 10, "happy");

            Assert.Equal(2, result.Items.Count());
        }
    }
}