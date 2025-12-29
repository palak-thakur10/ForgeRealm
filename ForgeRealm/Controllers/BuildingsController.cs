using ForgeRealm.Data;
using ForgeRealm.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.IO.Pipelines;
using System.Numerics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ForgeRealm.Controllers
{
    public class BuildingsController : Controller
    {
        private readonly AppDbContext _context;

        public BuildingsController(AppDbContext context)
        {
            _context = context;
        }

        // Predefined building names (Resource + Military/Defense)
        private List<string> GetBuildingNames()
        {
            return new List<string>
            {
                "Gold Mine", "Lumber Mill", "Stone Quarry", "Farm",
                "Barracks", "Archer Tower", "Siege Workshop", "Guard Post",
                "Training Grounds", "Iron Mine", "SimCity Series", "The Forge",
                "The Nexus", "The Workshop", "The Citadel", "The Hive"
            };
        }

        // GET: /Buildings
        public async Task<IActionResult> Index()
        {
            var buildings = await _context.Buildings
                                          .Include(b => b.Player)
                                          .OrderByDescending(b => b.Level)
                                          .ToListAsync();
            return View(buildings);
        }

        // GET: /Buildings/Create
        public IActionResult Create()
        {
            ViewData["Players"] = _context.Players.ToList();
            ViewData["BuildingNames"] = GetBuildingNames();
            return View();
        }

        // POST: /Buildings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Building building)
        {
            if (!GetBuildingNames().Contains(building.Name))
            {
                ModelState.AddModelError("Name", "Invalid building name. Please select from the list.");
            }

            if (ModelState.IsValid)
            {
                _context.Buildings.Add(building);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Players"] = _context.Players.ToList();
            ViewData["BuildingNames"] = GetBuildingNames();
            return View(building);
        }

        // GET: /Buildings/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var building = await _context.Buildings.FindAsync(id);
            if (building == null) return NotFound();

            ViewData["Players"] = _context.Players.ToList();
            ViewData["BuildingNames"] = GetBuildingNames();
            return View(building);
        }

        // POST: /Buildings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Building building)
        {
            if (!GetBuildingNames().Contains(building.Name))
            {
                ModelState.AddModelError("Name", "Invalid building name. Please select from the list.");
            }

            if (ModelState.IsValid)
            {
                _context.Update(building);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Players"] = _context.Players.ToList();
            ViewData["BuildingNames"] = GetBuildingNames();
            return View(building);
        }

        // GET: /Buildings/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var building = await _context.Buildings
                                         .Include(b => b.Player)
                                         .FirstOrDefaultAsync(b => b.Id == id);
            if (building == null) return NotFound();

            return View(building);
        }

        // POST: /Buildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var building = await _context.Buildings.FindAsync(id);
            if (building != null)
            {
                _context.Buildings.Remove(building);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
