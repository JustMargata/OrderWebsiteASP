using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderWebsiteASP.Data.Models;

namespace OrderWebsiteASP.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public virtual DbSet<Restaurant> Restaurants { get; set; } = null!;
        public virtual DbSet<FoodItem> FoodItems { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
        public virtual DbSet<Promotion> Promotions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FoodItem>()
                .HasOne(f => f.Restaurant)
                .WithMany(r => r.FoodItems)
                .HasForeignKey(f => f.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FoodItem>()
                .Property(f => f.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Entity<FoodItem>()
                .Property(f => f.Price)
                .HasPrecision(18, 2);

            builder.Entity<Restaurant>()
                .Property(r => r.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.FoodItem)
                .WithMany(f => f.OrderItems)
                .HasForeignKey(oi => oi.FoodItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Promotion>()
                .HasOne(p => p.Restaurant)
                .WithMany(r => r.Promotions)
                .HasForeignKey(p => p.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Promotion>()
                .Property(p => p.DiscountPercent)
                .HasPrecision(18, 2);

            var defaultUser = new IdentityUser
            {
                Id = "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                UserName = "admin@foodorder.com",
                Email = "admin@foodorder.com",
                EmailConfirmed = true,
                NormalizedUserName = "ADMIN@FOODORDER.COM",
                NormalizedEmail = "ADMIN@FOODORDER.COM",
                PasswordHash = new PasswordHasher<IdentityUser>()
                    .HashPassword(null, "Admin123!")
            };

            builder.Entity<IdentityUser>().HasData(defaultUser);

            builder.Entity<Restaurant>().HasData(
                new Restaurant { Id = 1, Name = "Marten's Pizzaria", Address = "123 Street Marten", ImageUrl = "https://images.unsplash.com/photo-1513104890138-7c749659a591?w=800" },
                new Restaurant { Id = 2, Name = "Happy Sofia", Address = "123 Happy Street Sofia", ImageUrl = "https://images.unsplash.com/photo-1579871494447-9811cf80d66c?w=800" },
                new Restaurant { Id = 3, Name = "Happy Plovdiv", Address = "123 Happy Street Plovdiv", ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=800" }
            );

            builder.Entity<FoodItem>().HasData(
                new FoodItem { Id = 1, Name = "Margherita Pizza", Price = 8.99m, RestaurantId = 1, ImageUrl = "https://images.unsplash.com/photo-1604068549290-dea0e4a305ca?w=400" },
                new FoodItem { Id = 2, Name = "Pepperoni Pizza", Price = 9.99m, RestaurantId = 1, ImageUrl = "https://images.unsplash.com/photo-1628840042765-356cda07504e?w=400" },
                new FoodItem { Id = 3, Name = "Four Cheese Pizza", Price = 10.50m, RestaurantId = 1, ImageUrl = "https://images.unsplash.com/photo-1513104890138-7c749659a591?w=400" },
                new FoodItem { Id = 4, Name = "Hawaiian Pizza", Price = 9.75m, RestaurantId = 1, ImageUrl = "https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?w=400" },
                new FoodItem { Id = 5, Name = "Garlic Bread", Price = 3.50m, RestaurantId = 1, ImageUrl = "https://i.pinimg.com/originals/9a/b3/9d/9ab39d2b5a5a22410d99800fa3064bed.png" },
                new FoodItem { Id = 6, Name = "California Roll", Price = 6.99m, RestaurantId = 2, ImageUrl = "https://www.waiter.com/blog/wp-content/uploads/2016/02/California-Sushi-Roll-2.jpg" },
                new FoodItem { Id = 7, Name = "Salmon Nigiri", Price = 5.50m, RestaurantId = 2, ImageUrl = "https://images.unsplash.com/photo-1559410545-0bdcd187e0a6?w=400" },
                new FoodItem { Id = 8, Name = "Tuna Sashimi", Price = 7.50m, RestaurantId = 2, ImageUrl = "https://images.unsplash.com/photo-1534482421-64566f976cfa?w=400" },
                new FoodItem { Id = 11, Name = "Cheeseburger", Price = 7.50m, RestaurantId = 3, ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=400" },
                new FoodItem { Id = 12, Name = "Bacon Burger", Price = 8.50m, RestaurantId = 3, ImageUrl = "https://images.unsplash.com/photo-1553979459-d2229ba7433b?w=400" },
                new FoodItem { Id = 13, Name = "Veggie Burger", Price = 7.00m, RestaurantId = 3, ImageUrl = "https://images.unsplash.com/photo-1520072959219-c595dc870360?w=400" }
            );
        }
    }
}