using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWebsiteASP.GCommon
{
    public class ValidationConstants
    {
        public static class Restaurant
        {
            public const int RestaurantNameMinLength = 2;
            public const int RestaurantNameMaxLength = 100;
            public const int RestaurantAddressMaxLength = 200;
        }

        public static class FoodItem
        {
            public const int FoodItemNameMaxLength = 100;
            public const decimal FoodItemMinPrice = 0.01m;
            public const decimal FoodItemMaxPrice = 1000;
        }

        public static class OrderItem
        {
            public const int OrderItemMinQuantity = 1;
            public const int OrderItemMaxQuantity = 50;
        }
    }
}
