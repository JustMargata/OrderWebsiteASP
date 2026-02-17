using System.ComponentModel.DataAnnotations;
using static OrderWebsiteASP.GCommon.ValidationConstants.FoodItem;

namespace OrderWebsiteASP.ViewModels.FoodItems
{
    public class FoodItemEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(FoodItemNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [Range((double)FoodItemMinPrice, (double)FoodItemMaxPrice)]
        public decimal Price { get; set; }

        [StringLength(FoodItemImageUrlMaxLength)]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string? ImageUrl { get; set; }

        public int RestaurantId { get; set; }
    }
}