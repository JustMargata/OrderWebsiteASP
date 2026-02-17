using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderWebsiteASP.Data;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db)
    {
        _db = db;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        var restaurants = _db.Restaurants
            .Include(r => r.FoodItems)
            .ToList();

        return View(restaurants);
    }
}