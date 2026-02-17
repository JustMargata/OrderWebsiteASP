using System.ComponentModel.DataAnnotations;
using static OrderWebsiteASP.GCommon.ValidationConstants.Restaurant;

namespace OrderWebsiteASP.ViewModels.Restaurants
{
    public class RestaurantEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(RestaurantNameMaxLength, MinimumLength = RestaurantNameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(RestaurantAddressMaxLength)]
        public string Address { get; set; } = null!;

        [StringLength(RestaurantImageUrlMaxLength)]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string? ImageUrl { get; set; }
    }
}