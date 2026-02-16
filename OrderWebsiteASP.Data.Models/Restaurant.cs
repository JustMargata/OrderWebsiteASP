using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public virtual ICollection<FoodItem> FoodItems { get; set; } = new HashSet<FoodItem>();
    }
}
