using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderWebsiteASP.Data.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        public virtual IdentityUser User { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Restaurant))]
        public int RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; } = null!;

        [Required]
        public DateTime OrderDate { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }
}