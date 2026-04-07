using Microsoft.EntityFrameworkCore;
using OrderWebsiteASP.Data;

namespace OrderWebsiteASP.Tests.Helpers
{
    public static class TestDbHelper
    {
        public static ApplicationDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}