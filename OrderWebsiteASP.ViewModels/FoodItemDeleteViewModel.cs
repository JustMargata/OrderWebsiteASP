namespace OrderWebsiteASP.ViewModels.FoodItems
{
    public class FoodItemDeleteViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = null!;
    }
}