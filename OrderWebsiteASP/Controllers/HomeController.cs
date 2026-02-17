using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderWebsiteASP.Services.Core.Contracts;

namespace OrderWebsiteASP.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IRestaurantService _restaurantService;

        public HomeController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var restaurants = await _restaurantService.GetAllAsync();
            return View(restaurants);
        }
    }
}