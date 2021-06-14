using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MageNPCTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace MageNPCTracker.Controllers
{
    public class ImbuedController : Controller
    {
        public CofDContext _context = new CofDContext();

        public ActionResult AddItemEnhancement()
        {
            ViewBag.EnhancementId = new SelectList(_context.RefEnhancement, "Id", "Bonus");
            ViewBag.SpellId = new SelectList(_context.SpellTable.OrderBy(x => x.SpellName), "Id", "SpellName");

            return PartialView("_ItemEnhancement", new ItemEnhancement());
        }

        public ActionResult AddSpell()
        {
            ViewBag.Spells = new SelectList(_context.SpellTable.OrderBy(x => x.SpellName), "Id", "SpellName");
            return PartialView("_ItemSpell", new ImbuedItemSpell());
        }

        // GET: Imbued
        public IActionResult Index()
        {
            var ImbuedTable = _context.ImbuedTable;
            return View(ImbuedTable.ToList());
        }

        // GET: Imbued/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return BadRequest();
            ImbuedViewModel viewModel = new ImbuedViewModel();


            var imbuedTable = await _context.ImbuedTable.FindAsync(id);
            if (imbuedTable == null) return NotFound();

            viewModel.ImbuedInfo = imbuedTable;
            viewModel.ItemEnhancement = _context.ItemEnhancement.Where(x => x.ImbuedItemId == id).ToList();
            viewModel.Spells = _context.ImbuedItemSpell.Where(x => x.ImbuedItemId == id).Include(x => x.SpellTable).ToList();

            return View(viewModel);
        }

        // GET: Imbued/Create
        public IActionResult Create()
        {
            ImbuedViewModel item = new ImbuedViewModel();
            item.ImbuedInfo.Name = "";
            return View(item);
        }

        // POST: Imbued/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ImbuedViewModel imbuedTable)
        {
            if (ModelState.IsValid)
            {
                imbuedTable.ImbuedInfo.Mana = 1;
                imbuedTable.ImbuedInfo.BatteryDots = 0;

                List<ImbuedItemSpell> imbuedSpells = new List<ImbuedItemSpell>();

                int bonusDots = 0, spellDots = 0;

                if (imbuedTable.ItemEnhancement.Count > 0)
                {
                    List<ItemEnhancement> spellEnhancements = imbuedTable.ItemEnhancement.Where(x => x.SpellId > 0).ToList();
                    List<ItemEnhancement> bonusEnhancements = imbuedTable.ItemEnhancement.Where(x => x.EnhancementId > 0).ToList();

                    for (int i = 0; i < spellEnhancements.Count; i++)
                    {
                        int spellId = (int)spellEnhancements[i].SpellId;
                        List<byte> spellLevels = _context.SpellArcanaTable.Where(x => x.SpellTableId == spellId).Select(x => x.Level).ToList();
                        spellLevels.Sort();
                        int highestLevel = spellLevels[0];
                        if ((spellDots + highestLevel) <= 5) spellDots += highestLevel;
                        else
                        {
                            while (spellDots < 5) { spellDots++; highestLevel--; }
                            for (int j = 0; j < highestLevel; j++) spellDots += 2;
                        }
                    }

                    for (int i = 0; i < bonusEnhancements.Count; i++)
                    {
                        int enhancementId = (int)bonusEnhancements[i].EnhancementId;
                        RefEnhancement enhancement = _context.RefEnhancement.SingleOrDefault(x => x.Id == enhancementId);
                        bonusDots += enhancement.Cost;
                        if (enhancement.Bonus.Contains("Mana"))
                        {
                            imbuedTable.ImbuedInfo.Mana += 2;
                            imbuedTable.ImbuedInfo.BatteryDots += 1;
                        }
                    }
                }

                if (imbuedTable.Spells.Count > 0)
                {
                    List<int> spellLevels = new List<int>();
                    for (int i = 0; i < imbuedTable.Spells.Count; i++)
                    {
                        int spellid = imbuedTable.Spells[i].SpellId;
                        List<byte> currentLevels = _context.SpellArcanaTable.Where(x => x.SpellTableId == spellid).Select(x => x.Level).ToList();
                        currentLevels.Sort();
                        spellLevels.Add(currentLevels[0]);
                        ImbuedItemSpell imbuedItemSpell = new ImbuedItemSpell
                        {
                            ImbuedItemId = imbuedTable.ImbuedInfo.Id,
                            SpellId = spellid
                        };
                        imbuedSpells.Add(imbuedItemSpell);
                    }
                    spellLevels.Sort();
                    imbuedTable.ImbuedInfo.SpellLevel = (short)spellLevels.Last();
                }

                imbuedTable.ImbuedInfo.Cost = (short)(imbuedTable.ImbuedInfo.SpellLevel + bonusDots + spellDots);
                _context.ImbuedTable.Add(imbuedTable.ImbuedInfo);
                _context.SaveChanges();

                for (int i = 0; i < imbuedSpells.Count; i++)
                {
                    imbuedSpells[i].ImbuedItemId = imbuedTable.ImbuedInfo.Id;
                    _context.Add(imbuedSpells[i]);
                }

                for (int i = 0; i < imbuedTable.ItemEnhancement.Count; i++)
                {
                    imbuedTable.ItemEnhancement[i].ImbuedItemId = imbuedTable.ImbuedInfo.Id;
                    if(imbuedTable.ItemEnhancement[i].SpellId > 0)
                    {
                        int spellId = (int)imbuedTable.ItemEnhancement[i].SpellId;
                        List<byte> spellLevels = _context.SpellArcanaTable.Where(x => x.SpellTableId == spellId).Select(x => x.Level).ToList();
                        spellLevels.Sort();
                        SpellTable spell = _context.SpellTable.SingleOrDefault(x => x.Id == spellId);
                        imbuedTable.ItemEnhancement[i].Enhancement = spell.SpellName;
                        imbuedTable.ItemEnhancement[i].EnhancementCost = spellLevels[0];
                    }

                    if(imbuedTable.ItemEnhancement[i].EnhancementId > 0)
                    {
                        int enhancementId = (int)imbuedTable.ItemEnhancement[i].EnhancementId;
                        RefEnhancement enhancement = _context.RefEnhancement.SingleOrDefault(x => x.Id == enhancementId);

                        if (imbuedTable.ItemEnhancement[i].SpellId > 0)
                        {
                            int spellId = (int)imbuedTable.ItemEnhancement[i].SpellId;
                            SpellTable spell = _context.SpellTable.SingleOrDefault(x => x.Id == spellId);
                            imbuedTable.ItemEnhancement[i].Enhancement = enhancement.Bonus + " (Yantra Bonus Casting " + spell.SpellName + ")";
                        }
                        else imbuedTable.ItemEnhancement[i].Enhancement = enhancement.Bonus;

                        imbuedTable.ItemEnhancement[i].EnhancementCost = enhancement.Cost;
                    }

                    _context.ItemEnhancement.Add(imbuedTable.ItemEnhancement[i]);
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EnhancementId = new SelectList(_context.RefEnhancement, "Id", "Bonus");
            ViewBag.SpellId = new SelectList(_context.SpellTable.OrderBy(x => x.SpellName), "Id", "SpellName");
            ViewBag.Spells = new SelectList(_context.SpellTable.OrderBy(x => x.SpellName), "Id", "SpellName");
            return View(imbuedTable);
        }

        // GET: Imbued/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            ImbuedViewModel viewModel = new ImbuedViewModel();

            ImbuedTable imbuedTable = await _context.ImbuedTable.FindAsync(id);
            if (imbuedTable == null) return NotFound();

            viewModel.ImbuedInfo = imbuedTable;
            viewModel.ItemEnhancement = _context.ItemEnhancement.Where(x => x.ImbuedItemId == id).ToList();
            viewModel.Spells = _context.ImbuedItemSpell.Where(x => x.ImbuedItemId == id).ToList();

            ViewBag.EnhancementId = new SelectList(_context.RefEnhancement, "Id", "Bonus");
            ViewBag.SpellId = new SelectList(_context.SpellTable.OrderBy(x => x.SpellName), "Id", "SpellName");
            ViewBag.Spells = new SelectList(_context.SpellTable.OrderBy(x => x.SpellName), "Id", "SpellName");
            return View(viewModel);
        }

        // POST: Imbued/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ImbuedViewModel imbuedTable)
        {
            if (ModelState.IsValid)
            {
                imbuedTable.ImbuedInfo.Mana = 1;
                imbuedTable.ImbuedInfo.BatteryDots = 0;

                int bonusDots = 0, spellDots = 0;

                if (imbuedTable.ItemEnhancement.Count > 0)
                {
                    List<ItemEnhancement> spellEnhancements = imbuedTable.ItemEnhancement.Where(x => x.SpellId > 0).ToList();
                    List<ItemEnhancement> bonusEnhancements = imbuedTable.ItemEnhancement.Where(x => x.EnhancementId > 0).ToList();

                    for (int i = 0; i < spellEnhancements.Count; i++)
                    {
                        int spellId = (int)spellEnhancements[i].SpellId;
                        List<byte> spellLevels = _context.SpellArcanaTable.Where(x => x.SpellTableId == spellId).Select(x => x.Level).ToList();
                        spellLevels.Sort();
                        int highestLevel = spellLevels[0];
                        if ((spellDots + highestLevel) <= 5) spellDots += highestLevel;
                        else
                        {
                            while (spellDots < 5) { spellDots++; highestLevel--; }
                            for (int j = 0; j < highestLevel; j++) spellDots += 2;
                        }
                    }

                    for (int i = 0; i < bonusEnhancements.Count; i++)
                    {
                        int enhancementId = (int)bonusEnhancements[i].EnhancementId;
                        RefEnhancement enhancement = _context.RefEnhancement.SingleOrDefault(x => x.Id == enhancementId);
                        bonusDots += enhancement.Cost;
                        if (enhancement.Bonus.Contains("Mana"))
                        {
                            imbuedTable.ImbuedInfo.Mana += 2;
                            imbuedTable.ImbuedInfo.BatteryDots += 1;
                        }
                    }
                }

                if (imbuedTable.Spells.Count > 0)
                {
                    List<int> spellLevels = new List<int>();
                    for (int i = 0; i < imbuedTable.Spells.Count; i++)
                    {
                        int spellid = imbuedTable.Spells[i].SpellId;
                        if (spellid > 0)
                        {
                            List<byte> currentLevels = _context.SpellArcanaTable.Where(x => x.SpellTableId == spellid).Select(x => x.Level).ToList();
                            currentLevels.Sort();
                            spellLevels.Add(currentLevels[0]);
                            ImbuedItemSpell imbuedItemSpell = new ImbuedItemSpell
                            {
                                ImbuedItemId = imbuedTable.ImbuedInfo.Id,
                                SpellId = spellid
                            };

                            if (imbuedTable.Spells[i].ImbuedItemSpellId == 0) _context.Add(imbuedItemSpell);
                            else _context.Entry(imbuedTable.Spells[i]).State = EntityState.Modified;
                        }
                        else _context.Remove(imbuedTable.Spells[i]);
                    }
                    spellLevels.Sort();
                    imbuedTable.ImbuedInfo.SpellLevel = (short)spellLevels.Last();
                }

                for (int i = 0; i < imbuedTable.ItemEnhancement.Count; i++)
                {
                    int id = imbuedTable.ItemEnhancement[i].Id;
                    if(id == 0)
                    {
                        imbuedTable.ItemEnhancement[i].ImbuedItemId = imbuedTable.ImbuedInfo.Id;
                        if (imbuedTable.ItemEnhancement[i].SpellId > 0)
                        {
                            int spellId = (int)imbuedTable.ItemEnhancement[i].SpellId;
                            List<byte> spellLevels = _context.SpellArcanaTable.Where(x => x.SpellTableId == spellId).Select(x => x.Level).ToList();
                            spellLevels.Sort();
                            SpellTable spell = _context.SpellTable.SingleOrDefault(x => x.Id == spellId);
                            imbuedTable.ItemEnhancement[i].Enhancement = spell.SpellName;
                            imbuedTable.ItemEnhancement[i].EnhancementCost = spellLevels[0];
                        }

                        if (imbuedTable.ItemEnhancement[i].EnhancementId > 0)
                        {
                            int enhancementId = (int)imbuedTable.ItemEnhancement[i].EnhancementId;
                            RefEnhancement enhancement = _context.RefEnhancement.SingleOrDefault(x => x.Id == enhancementId);

                            if(imbuedTable.ItemEnhancement[i].SpellId > 0)
                            {
                                int spellId = (int)imbuedTable.ItemEnhancement[i].SpellId;
                                SpellTable spell = _context.SpellTable.SingleOrDefault(x => x.Id == spellId);
                                imbuedTable.ItemEnhancement[i].Enhancement = enhancement.Bonus + " (Yantra Bonus Casting " + spell.SpellName + ")";
                            }
                            else imbuedTable.ItemEnhancement[i].Enhancement = enhancement.Bonus;

                            imbuedTable.ItemEnhancement[i].EnhancementCost = enhancement.Cost;
                        }

                        _context.ItemEnhancement.Add(imbuedTable.ItemEnhancement[i]);
                    }
                    else if(id > 0)
                    {
                        if (imbuedTable.ItemEnhancement[i].SpellId > 0)
                        {
                            int spellId = (int)imbuedTable.ItemEnhancement[i].SpellId;
                            List<byte> spellLevels = _context.SpellArcanaTable.Where(x => x.SpellTableId == spellId).Select(x => x.Level).ToList();
                            spellLevels.Sort();
                            SpellTable spell = _context.SpellTable.SingleOrDefault(x => x.Id == spellId);
                            imbuedTable.ItemEnhancement[i].Enhancement = spell.SpellName;
                            imbuedTable.ItemEnhancement[i].EnhancementCost = spellLevels.Last();
                        }

                        if (imbuedTable.ItemEnhancement[i].EnhancementId > 0)
                        {
                            int enhancementId = (int)imbuedTable.ItemEnhancement[i].EnhancementId;
                            RefEnhancement enhancement = _context.RefEnhancement.SingleOrDefault(x => x.Id == enhancementId);

                            if (imbuedTable.ItemEnhancement[i].SpellId > 0)
                            {
                                int spellId = (int)imbuedTable.ItemEnhancement[i].SpellId;
                                SpellTable spell = _context.SpellTable.SingleOrDefault(x => x.Id == spellId);
                                imbuedTable.ItemEnhancement[i].Enhancement = enhancement.Bonus + " (Yantra Bonus Casting " + spell.SpellName + ")";
                            }
                            else imbuedTable.ItemEnhancement[i].Enhancement = enhancement.Bonus;

                            imbuedTable.ItemEnhancement[i].EnhancementCost = enhancement.Cost;
                        }
                        _context.Entry(imbuedTable.ItemEnhancement[i]).State = EntityState.Modified;

                        if (imbuedTable.ItemEnhancement[i].EnhancementId == null && imbuedTable.ItemEnhancement[i].SpellId == null) _context.Remove(imbuedTable.ItemEnhancement[i]);
                    }
                }

                imbuedTable.ImbuedInfo.Cost = (short)(imbuedTable.ImbuedInfo.SpellLevel + bonusDots + spellDots);
                _context.Entry(imbuedTable.ImbuedInfo).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EnhancementId = new SelectList(_context.RefEnhancement, "Id", "Bonus");
            ViewBag.SpellId = new SelectList(_context.SpellTable.OrderBy(x => x.SpellName), "Id", "SpellName");
            ViewBag.Spells = new SelectList(_context.SpellTable.OrderBy(x => x.SpellName), "Id", "SpellName");
            return View(imbuedTable);
        }

        // GET: Imbued/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var imbuedTable = await _context.ImbuedTable.FindAsync(id);
            if (imbuedTable == null) return NotFound();

            return View(imbuedTable);
        }

        // POST: Imbued/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imbuedTable = await _context.ImbuedTable.FindAsync(id);
            var itemEnhancement = _context.ItemEnhancement.Where(x => x.ImbuedItemId == id).ToList();

            _context.ItemEnhancement.RemoveRange(itemEnhancement);
            _context.ImbuedTable.Remove(imbuedTable);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}