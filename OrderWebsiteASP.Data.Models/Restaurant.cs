using System.ComponentModel.DataAnnotations;
using static OrderWebsiteASP.GCommon.ValidationConstants.Restaurant;

namespace OrderWebsiteASP.Data.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(RestaurantNameMaxLength, MinimumLength = RestaurantNameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(RestaurantAddressMaxLength)]
        public string Address { get; set; } = null!;

        [StringLength(RestaurantImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        public virtual ICollection<FoodItem> FoodItems { get; set; } = new HashSet<FoodItem>();
        public virtual ICollection<Promotion> Promotions { get; set; } = new HashSet<Promotion>();
    }
}