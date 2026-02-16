using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static OrderWebsiteASP.GCommon.ValidationConstants.OrderItem;

namespace OrderWebsiteASP.Data.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;

        [Required]
        public int FoodItemId { get; set; }
        public virtual FoodItem FoodItem { get; set; } = null!;

        [Required]
        [Range(OrderItemMinQuantity, OrderItemMaxQuantity)]
        public int Quantity { get; set; }
    }
}