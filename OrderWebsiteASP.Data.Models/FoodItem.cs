using static OrderWebsiteASP.GCommon.ValidationConstants.FoodItem;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderWebsiteASP.Data.Models
{
    public class FoodItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(FoodItemNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [Range((double)FoodItemMinPrice, (double)FoodItemMaxPrice)]
        public decimal Price { get; set; }

        [StringLength(FoodItemImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        [Required]
        [ForeignKey(nameof(Restaurant))]
        public int RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; } = null!;

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }
}