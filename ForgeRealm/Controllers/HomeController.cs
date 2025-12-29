using ForgeRealm.Data;
using ForgeRealm.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForgeRealm.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /
        public async Task<IActionResult> Index()
        {
            // Realm Stats
            var totalPlayers = await _context.Players.CountAsync();
            var totalBuildings = await _context.Buildings.CountAsync();
            var totalGold = await _context.Players.SumAsync(p => p.Resource.Gold);
            var totalWood = await _context.Players.SumAsync(p => p.Resource.Wood);
            var totalStone = await _context.Players.SumAsync(p => p.Resource.Stone);
            var totalFood = await _context.Players.SumAsync(p => p.Resource.Food);

            // Top 5 Players by Level then Gold
            var topPlayers = await _context.Players
                                           .Include(p => p.Resource)
                                           .OrderByDescending(p => p.Level)
                                           .ThenByDescending(p => p.Resource.Gold)
                                           .Take(5)
                                           .ToListAsync();

            // Prepare ViewModel
            var model = new HomeDashboardViewModel
            {
                TotalPlayers = totalPlayers,
                TotalBuildings = totalBuildings,
                TotalGold = totalGold,
                TotalWood = totalWood,
                TotalStone = totalStone,
                TotalFood = totalFood,
                TopPlayers = topPlayers
            };

            return View(model);
        }

        // GET: /Home/Privacy
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
