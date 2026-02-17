namespace OrderWebsiteASP.ViewModels.Restaurants
{
    public class RestaurantDeleteViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? ImageUrl { get; set; }
    }
}