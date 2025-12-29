using ForgeRealm.Data;
using ForgeRealm.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForgeRealm.Controllers
{
    public class PlayerController : Controller
    {
        private readonly AppDbContext _context;

        public PlayerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Player
        public async Task<IActionResult> Index()
        {
            var players = await _context.Players
                                        .Include(p => p.Resource)
                                        .Include(p => p.Buildings)
                                        .ToListAsync();
            return View(players);
        }

        // GET: /Player/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Player/Create
        [HttpPost]
        public async Task<IActionResult> Create(Player player)
        {
            if (ModelState.IsValid)
            {
                // Automatically assign default Resource values
                player.Resource = new Resource
                {
                    Gold = 100,
                    Wood = 100,
                    Stone = 0,
                    Food = 100
                };

                _context.Players.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(player);
        }
        // GET: /Player/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var player = await _context.Players
                                       .Include(p => p.Resource)
                                       .Include(p => p.Buildings)
                                       .FirstOrDefaultAsync(p => p.Id == id);

            if (player == null) return NotFound();

            return View(player);
        }

        // POST: /Player/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Player player)
        {
            if (id != player.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction("Index");
            }
            return View(player);
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
        // GET: /Player/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var player = await _context.Players
                                       .Include(p => p.Resource)
                                       .Include(p => p.Buildings)
                                       .FirstOrDefaultAsync(p => p.Id == id);

            if (player == null) return NotFound();

            return View(player);
        }

        // POST: /Player/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Players
                                       .Include(p => p.Resource)
                                       .FirstOrDefaultAsync(p => p.Id == id);

            if (player != null)
            {
                // Remove associated Resource first (if any)
                if (player.Resource != null)
                {
                    _context.Resources.Remove(player.Resource);
                }

                _context.Players.Remove(player);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

    }
}
