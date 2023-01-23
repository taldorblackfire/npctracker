using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MageNPCTracker.Models;
namespace MageNPCTracker.Controllers
{
    public class ArtifactController : Controller
    {
        public CofDContext _context = new CofDContext();
        private readonly List<ArtifactView> view = new List<ArtifactView>();

        public ArtifactController()
        {
            view = _context.ArtifactView.ToList();
        }

        public ActionResult AddAttainment()
        {
            ViewBag.ArtifactAttainments = new SelectList(_context.AttainmentTable.Where(x => x.ItemUsable).OrderBy(x => x.attainment_name), "Id", "attainment_name");

            return PartialView("_ArtifactAttainment", new ArtifactAttainment());
        }

        // GET: Artifact
        public ActionResult Index()
        {
            return View(view);
        }

        // GET: Artifact/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ArtifactViewModel vm = new ArtifactViewModel();

            var artifactTable = await _context.ArtifactTable.FirstOrDefaultAsync(m => m.Id == id);
            if (artifactTable == null)
            {
                return NotFound();
            }

            vm.ArtifactInfo = artifactTable;
            vm.Attainments = _context.ArtifactAttainment.Where(x => x.ArtifactId == id).Include(x => x.AttainmentTable).ToList();

            return View(vm);
        }

        // GET: Artifact/Create
        public IActionResult Create()
        {
            return View(new ArtifactViewModel());
        }

        // POST: Artifact/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArtifactViewModel artifactTable)
        {
            if (ModelState.IsValid)
            {
                int cost = 0;
                if (artifactTable.ArtifactInfo.ImperialSurcharge) cost += 4;
                if (artifactTable.ArtifactInfo.YantraBonus) cost++;

                cost += artifactTable.ArtifactInfo.Reach;

                for(int i = 0; i < artifactTable.Attainments.Count; i++)
                    if (artifactTable.Attainments[i].AttainmentId != 0) cost += 1;

                if (cost < 3) artifactTable.ArtifactInfo.Cost = 3;
                else artifactTable.ArtifactInfo.Cost = (short)cost;

                decimal gnosis = (decimal)(artifactTable.ArtifactInfo.Cost) / 2;

                if (gnosis < 6 && artifactTable.ArtifactInfo.ImperialSurcharge) artifactTable.ArtifactInfo.Gnosis = 6;
                else artifactTable.ArtifactInfo.Gnosis = (short)Math.Ceiling(gnosis);
                artifactTable.ArtifactInfo.Mana = (short)(artifactTable.ArtifactInfo.Cost * 2);

                _context.Add(artifactTable.ArtifactInfo);

                for (int i = 0; i < artifactTable.Attainments.Count; i++)
                {
                    if (artifactTable.Attainments[i].AttainmentId > 0)
                    {
                        artifactTable.Attainments[i].ArtifactId = artifactTable.ArtifactInfo.Id;
                        _context.ArtifactAttainment.Add(artifactTable.Attainments[i]);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(artifactTable);
        }

        // GET: Artifact/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ArtifactViewModel vm = new ArtifactViewModel();

            var artifactTable = await _context.ArtifactTable.FindAsync(id);
            if (artifactTable == null)
            {
                return NotFound();
            }

            vm.ArtifactInfo = artifactTable;
            vm.Attainments = _context.ArtifactAttainment.Where(x => x.ArtifactId == id).Include(x => x.AttainmentTable).ToList();
            ViewBag.ArtifactAttainments = new SelectList(_context.AttainmentTable.Where(x => x.ItemUsable).OrderBy(x => x.attainment_name), "Id", "attainment_name");

            return View(vm);
        }

        // POST: Artifact/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ArtifactViewModel artifactTable)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int cost = 0;
                    if (artifactTable.ArtifactInfo.ImperialSurcharge) cost += 4;
                    if (artifactTable.ArtifactInfo.YantraBonus) cost++;

                    cost += artifactTable.ArtifactInfo.Reach;

                    for (int i = 0; i < artifactTable.Attainments.Count; i++)
                    {
                        if (artifactTable.Attainments[i].AttainmentId == 0) _context.Remove(artifactTable.Attainments[i]);
                        else if (artifactTable.Attainments[i].Id == 0)
                        {
                            artifactTable.Attainments[i].ArtifactId = artifactTable.ArtifactInfo.Id;
                            _context.Add(artifactTable.Attainments[i]);
                            cost++;
                        }
                        else
                        {
                            cost++;
                            _context.Entry(artifactTable.Attainments[i]).State = EntityState.Modified;
                        }
                    }

                    if (cost < 3) artifactTable.ArtifactInfo.Cost = 3;
                    else artifactTable.ArtifactInfo.Cost = (short)cost;

                    decimal gnosis = (decimal)(artifactTable.ArtifactInfo.Cost) / 2;
                    if (gnosis < 6 && artifactTable.ArtifactInfo.ImperialSurcharge) artifactTable.ArtifactInfo.Gnosis = 6;
                    else artifactTable.ArtifactInfo.Gnosis = (short)Math.Ceiling(gnosis);
                    artifactTable.ArtifactInfo.Mana = (short)(artifactTable.ArtifactInfo.Cost * 2);

                    _context.Update(artifactTable.ArtifactInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtifactTableExists(artifactTable.ArtifactInfo.Id))
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

            ViewBag.ArtifactAttainments = new SelectList(_context.AttainmentTable.Where(x => x.ItemUsable).OrderBy(x => x.attainment_name), "Id", "attainment_name");
            return View(artifactTable);
        }

        // GET: Artifact/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artifactTable = await _context.ArtifactTable
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artifactTable == null)
            {
                return NotFound();
            }

            return View(artifactTable);
        }

        // POST: Artifact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artifactTable = await _context.ArtifactTable.FindAsync(id);
            _context.ArtifactTable.Remove(artifactTable);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtifactTableExists(int id)
        {
            return _context.ArtifactTable.Any(e => e.Id == id);
        }
    }
}
