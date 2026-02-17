using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static OrderWebsiteASP.GCommon.ValidationConstants.Promotion;

namespace OrderWebsiteASP.Data.Models
{
    public class Promotion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(PromotionTitleMaxLength, MinimumLength = PromotionTitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(PromotionDescriptionMaxLength, MinimumLength = PromotionDescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required]
        [Range(PromotionMinDiscount, PromotionMaxDiscount)]
        public decimal DiscountPercent { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [StringLength(PromotionImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        [Required]
        [ForeignKey(nameof(Restaurant))]
        public int RestaurantId { get; set; }
        public virtual Restaurant? Restaurant { get; set; }
    }
}