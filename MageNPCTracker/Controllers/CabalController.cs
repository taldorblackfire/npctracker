using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MageNPCTracker.Models;
using Microsoft.AspNetCore.Http;
namespace MageNPCTracker.Controllers
{
    public class CabalController : Controller
    {
        public CofDContext _context = new CofDContext();

        // GET: Cabal/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cabal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MageCabalid,CabalName,RightofEmeritus,RightofSanctuary,RightofCrossing,RightofNemesis,RightofHospitality,GameId")] MageCabal mageCabal)
        {
            if (ModelState.IsValid)
            {
                var id = HttpContext.Session.GetInt32("GameId");

                if (id == null)
                {
                    ModelState.AddModelError("", "You must have a game selected before you can create a cabal.");
                    return View(mageCabal);
                }
                else mageCabal.GameId = (int)id;

                _context.Add(mageCabal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mageCabal);
        }


        // GET: Cabal/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mageCabal = await _context.MageCabal
                .FirstOrDefaultAsync(m => m.MageCabalid == id);
            if (mageCabal == null)
            {
                return NotFound();
            }

            return View(mageCabal);
        }

        // POST: Cabal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mageCabal = await _context.MageCabal.FindAsync(id);
            _context.MageCabal.Remove(mageCabal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Cabal/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            CabalViewModel mageCabal = new CabalViewModel();
            mageCabal.CabalInfo = await _context.MageCabal.FirstOrDefaultAsync(m => m.MageCabalid == id);

            if (mageCabal.CabalInfo == null) return NotFound();
            mageCabal.CabalInfo.Game = await _context.Npcgame.FindAsync(mageCabal.CabalInfo.GameId);

            List<MageNpctable> mages = _context.MageNpctable.Where(x => x.Cabal == id).ToList();

            mageCabal.CabalMembers = await _context.Npctable.Where(x => mages.Select(y => y.Npcid).Contains(x.Npcid)).ToListAsync();

            for(int i = 0; i < mageCabal.CabalMembers.Count; i++)
            {
                int characterId = mageCabal.CabalMembers[i].Npcid;
                mageCabal.ImbuedItems.AddRange(await _context.Npcimbued.Where(x => x.Npcid == characterId).Include(y => y.Imbued).ToListAsync());
                mageCabal.Artifacts.AddRange(await _context.Npcartifact.Where(x => x.Npcid == characterId).Include(y => y.Artifact).ToListAsync());
            }

            return View(mageCabal);
        }

        // GET: Cabal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mageCabal = await _context.MageCabal.FindAsync(id);
            ViewBag.Games = new SelectList(_context.Npcgame.ToList(), "Id", "Name");
            if (mageCabal == null)
            {
                return NotFound();
            }
            return View(mageCabal);
        }

        // POST: Cabal/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MageCabalid,CabalName,RightofEmeritus,RightofSanctuary,RightofCrossing,RightofNemesis,RightofHospitality,GameId")] MageCabal mageCabal)
        {
            if (id != mageCabal.MageCabalid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mageCabal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MageCabalExists(mageCabal.MageCabalid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Games = new SelectList(_context.Npcgame.ToList(), "Id", "Name");
            return View(mageCabal);
        }

        // GET: Cabal
        public async Task<IActionResult> Index()
        {
            var id = HttpContext.Session.GetInt32("GameId");
            if(id == null) { id = 0; }


            return View(await _context.MageCabal.Where(x => x.GameId == id).ToListAsync());
        }

        private bool MageCabalExists(int id)
        {
            return _context.MageCabal.Any(e => e.MageCabalid == id);
        }
    }
}
