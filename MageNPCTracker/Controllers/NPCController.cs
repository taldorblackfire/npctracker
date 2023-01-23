using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MageNPCTracker.Models;
using System.Collections;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
namespace MageNPCTracker.Controllers
{
    public class NPCController : Controller
    {
        public CofDContext db = new CofDContext();
        public List<NPCView> view = new List<NPCView>();

        public NPCController()
        {
            view = db.NPCView.ToList();
        }

        public ActionResult AddNewArcana()
        {
            MageNpcarcana newArcana = new MageNpcarcana();

            ViewBag.ArcanaId = new SelectList(db.ArcanaTable.OrderBy(x => x.Arcana), "Id", "Arcana");

            return PartialView("_MageArcana", newArcana);
        }

        public ActionResult AddImbued()
        {
            ViewBag.ImbuedItem = new SelectList(db.ImbuedTable.OrderBy(x => x.Name).ToList(), "Id", "Name");

            return PartialView("_NPCImbued", new Npcimbued());
        }

        public ActionResult AddArtifact()
        {
            ViewBag.ArtifactList = new SelectList(db.ArtifactTable.OrderBy(x => x.Name).ToList(), "Id", "Name");

            return PartialView("_NPCArtifact", new Npcartifact());
        }

        public IActionResult Create()
        {
            NPCViewModel newNpc = new NPCViewModel();

            ViewBag.SupernaturalFaction = new SelectList(db.RefSupernaturalFaction.OrderBy(x => x.SupernaturalFactionId), "SupernaturalFactionId", "Name");

            return View(newNpc);
        }

        [HttpPost]
        public IActionResult Create(NPCViewModel newNpc)
        {
            var id = HttpContext.Session.GetInt32("GameId");

            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    ModelState.AddModelError("", "You must specify a game to create an NPC to associate it with a game");

                    ViewBag.SupernaturalFaction = new SelectList(db.RefSupernaturalFaction.OrderBy(x => x.SupernaturalFactionId), "SupernaturalFactionId", "Name");

                    return View(newNpc);
                }
                else
                {
                    newNpc.NpcInfo.GameId = (int)id;
                    db.Npctable.Add(newNpc.NpcInfo);
                    db.SaveChanges();

                    for (int i = 0; i < newNpc.ImbuedList.Count; i++)
                    {
                        if (newNpc.ImbuedList[i].ImbuedId > 0)
                        {
                            newNpc.ImbuedList[i].Npcid = newNpc.NpcInfo.Npcid;
                            db.Npcimbued.Add(newNpc.ImbuedList[i]);
                        }
                    }
                    db.SaveChanges();

                    for (int i = 0; i < newNpc.ArtifactList.Count; i++)
                    {
                        if (newNpc.ArtifactList[i].ArtifactId > 0)
                        {
                            newNpc.ArtifactList[i].Npcid = newNpc.NpcInfo.Npcid;
                            db.Npcartifact.Add(newNpc.ArtifactList[i]);
                        }
                    }
                    db.SaveChanges();
                }


                return RedirectToAction("Index");
            }

            ViewBag.SupernaturalFaction = new SelectList(db.RefSupernaturalFaction.OrderBy(x => x.SupernaturalFactionId), "SupernaturalFactionId", "Name");

            return View(newNpc);
        }

        public IActionResult CreateMage()
        {
            MageViewModel newMage = new MageViewModel();

            var gameId = HttpContext.Session.GetInt32("GameId");

            if (gameId == null) gameId = 0;

            var mages = db.MageNpctable.ToList();
            ViewBag.NpcidList = new SelectList(db.Npctable.Where(x => x.SupernaturalFaction == 2 && !mages.Select(y => y.Npcid).Contains(x.Npcid)).ToList(), "Npcid", "Alias");
            ViewBag.MentorList = new SelectList(db.Npctable.Where(x => x.SupernaturalFaction == 2 && x.GameId == gameId).OrderBy(x => x.Alias).ToList(), "Npcid", "Alias");
            ViewBag.OrderList = new SelectList(db.MageOrder.OrderBy(x => x.OrderName), "Id", "OrderName");
            ViewBag.PathList = new SelectList(db.MagePath.OrderBy(x => x.Name), "Id", "Name");
            ViewBag.LegacyList = new SelectList(db.RefLegacy.OrderBy(x => x.LegacyName).ToList(), "LegacyId", "LegacyName");
            ViewBag.OrderStatusList = new SelectList(db.MageCaucusInfo.OrderBy(x => x.MageOrderNavigation.OrderName), "MageCaucusId", "Title");
            ViewBag.ConsiliumStatusList = new SelectList(db.MageConsiliumTitles.OrderBy(x => x.Title), "ConsiliumTitleId", "Title");
            ViewBag.ArcanaId = new SelectList(db.ArcanaTable, "Id", "Arcana");
            ViewBag.CabalList = new SelectList(db.MageCabal.Where(x => x.GameId == gameId).OrderBy(x => x.CabalName).ToList(), "MageCabalid", "CabalName");

            newMage.MageArcana.Add(new MageNpcarcana { ArcanaId = 1, Level = 1 });

            return View(newMage);
        }

        [HttpPost]
        public IActionResult CreateMage(MageViewModel newMage)
        {
            var gameId = HttpContext.Session.GetInt32("GameId");

            if (gameId == null) gameId = 0;

            if (ModelState.IsValid)
            {
                db.MageNpctable.Add(newMage.MageInfo);
                db.SaveChanges();

                for (int i = 0; i < newMage.MageArcana.Count; i++)
                {
                    newMage.MageArcana[i].Npcid = newMage.MageInfo.Npcid;
                    db.MageNpcarcana.Add(newMage.MageArcana[i]);
                }
                db.SaveChanges();

                if (newMage.MageInfo.Mentor != null)
                {
                    MageApprenticeTable newApprentice = new MageApprenticeTable();
                    newApprentice.MentorId = (int)newMage.MageInfo.Mentor;
                    newApprentice.Npcid = newMage.MageInfo.Npcid;
                    db.MageApprenticeTable.Add(newApprentice);
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.NpcidList = new SelectList(db.Npctable.Where(x => x.SupernaturalFaction == 2).ToList(), "Npcid", "Alias");
            ViewBag.MentorList = new SelectList(db.Npctable.Where(x => x.SupernaturalFaction == 2 && x.GameId == gameId).OrderBy(x => x.Alias).ToList(), "Npcid", "Alias");
            ViewBag.OrderList = new SelectList(db.MageOrder.OrderBy(x => x.OrderName), "Id", "OrderName");
            ViewBag.PathList = new SelectList(db.MagePath.OrderBy(x => x.Name), "Id", "Name");
            ViewBag.LegacyList = new SelectList(db.RefLegacy.OrderBy(x => x.LegacyName).ToList(), "LegacyId", "LegacyName");
            ViewBag.OrderStatusList = new SelectList(db.MageCaucusInfo.OrderBy(x => x.MageOrderNavigation.OrderName), "MageCaucusId", "Title");
            ViewBag.ConsiliumStatusList = new SelectList(db.MageConsiliumTitles.OrderBy(x => x.Title), "ConsiliumTitleId", "Title");
            ViewBag.ArcanaId = new SelectList(db.ArcanaTable, "Id", "Arcana");
            ViewBag.CabalList = new SelectList(db.MageCabal.Where(x => x.GameId == gameId).OrderBy(x => x.CabalName).ToList(), "MageCabalid", "CabalName");

            return View(newMage);
        }

        public IActionResult Delete(int? id)
        {
            if(id == null) { return NotFound(); }

            var npc = view.Where(x => x.NPCId == id).FirstOrDefault();

            if (npc == null) { return NotFound(); }
            else return View(npc);
        }

        // POST: Artifact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var npc = await db.Npctable.FindAsync(id);
            db.Npctable.Remove(npc);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            if (id == null) return new StatusCodeResult(400);

            Npctable npc = db.Npctable.Include(x => x.SupernaturalFactionNavigation).FirstOrDefault(x => x.Npcid == id);

            if(npc.SupernaturalFaction == 2)
            {
                MageViewModel mageDetails = new MageViewModel();
                mageDetails.MageInfo = db.MageNpctable.FirstOrDefault(x => x.Npcid == id);
                mageDetails.MageArcana = db.MageNpcarcana.Include(x => x.Arcana).Where(x => x.Npcid == id).ToList();
                mageDetails.NpcInfo = npc;
                List<int> apprenticeIds = db.MageApprenticeTable.Where(x => x.MentorId == id).Select(x => x.Npcid).ToList();
                mageDetails.Apprentices = db.Npctable.Where(x => apprenticeIds.Contains(x.Npcid)).ToList();
                mageDetails.ImbuedList = db.Npcimbued.Where(x => x.Npcid == id).ToList();
                mageDetails.ArtifactList = db.Npcartifact.Where(x => x.Npcid == id).ToList();

                ViewBag.MentorList = new SelectList(db.Npctable.Where(x => x.SupernaturalFaction == 2).OrderBy(x => x.Alias).ToList(), "Npcid", "Alias");
                ViewBag.OrderList = new SelectList(db.MageOrder.OrderBy(x => x.OrderName), "Id", "OrderName");
                ViewBag.PathList = new SelectList(db.MagePath.OrderBy(x => x.Name), "Id", "Name");
                ViewBag.LegacyList = new SelectList(db.RefLegacy.OrderBy(x => x.LegacyName).ToList(), "LegacyId", "LegacyName");
                ViewBag.OrderStatusList = new SelectList(db.MageCaucusInfo.OrderBy(x => x.MageOrderNavigation.OrderName), "MageCaucusId", "Title");
                ViewBag.ConsiliumStatusList = new SelectList(db.MageConsiliumTitles.OrderBy(x => x.Title), "ConsiliumTitleId", "Title");
                ViewBag.ArcanaId = new SelectList(db.ArcanaTable.OrderBy(x => x.Arcana), "Id", "Arcana");
                ViewBag.CabalList = new SelectList(db.MageCabal.OrderBy(x => x.CabalName), "MageCabalid", "CabalName");
                ViewBag.ImbuedItem = new SelectList(db.ImbuedTable.OrderBy(x => x.Name).ToList(), "Id", "Name");
                ViewBag.ArtifactList = new SelectList(db.ArtifactTable.OrderBy(x => x.Name).ToList(), "Id", "Name");

                return View("MageDetails", mageDetails);
            }

            return View();
        }
        
        public IActionResult Edit(int? id)
        {
            if (id == null) return new StatusCodeResult(400);

            Npctable npc = db.Npctable.Include(x => x.SupernaturalFactionNavigation).FirstOrDefault(x => x.Npcid == id);

            NPCViewModel npcVM = new NPCViewModel();
            npcVM.NpcInfo = npc;
            npcVM.ImbuedList = db.Npcimbued.Where(x => x.Npcid == id).ToList();
            npcVM.ArtifactList = db.Npcartifact.Where(x => x.Npcid == id).ToList();

            ViewBag.Games = new SelectList(db.Npcgame.ToList(), "Id", "Name");
            ViewBag.SupernaturalFaction = new SelectList(db.RefSupernaturalFaction.OrderBy(x => x.SupernaturalFactionId), "SupernaturalFactionId", "Name");
            ViewBag.ImbuedItem = new SelectList(db.ImbuedTable.OrderBy(x => x.Name).ToList(), "Id", "Name");
            ViewBag.ArtifactList = new SelectList(db.ArtifactTable.OrderBy(x => x.Name).ToList(), "Id", "Name");

            return View(npcVM);
        }

        [HttpPost]
        public IActionResult Edit(NPCViewModel npc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(npc.NpcInfo).State = EntityState.Modified;
                db.SaveChanges();

                for (int i = 0; i < npc.ImbuedList.Count; i++)
                {
                    if (npc.ImbuedList[i].NpcimbuedId == 0)
                    {
                        npc.ImbuedList[i].Npcid = npc.NpcInfo.Npcid;
                        db.Npcimbued.Add(npc.ImbuedList[i]);
                    }
                    else
                    {
                        Npcimbued imbued = npc.ImbuedList[i];
                        db.Entry(imbued).State = EntityState.Modified;
                    }
                }

                for (int i = 0; i < npc.ArtifactList.Count; i++)
                {
                    if (npc.ArtifactList[i].NpcartifactId == 0)
                    {
                        npc.ArtifactList[i].Npcid = npc.NpcInfo.Npcid;
                        db.Npcartifact.Add(npc.ArtifactList[i]);
                    }
                    else
                    {
                        Npcartifact artifact = npc.ArtifactList[i];
                        db.Entry(artifact).State = EntityState.Modified;
                    }
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Games = new SelectList(db.Npcgame.ToList(), "Id", "Name");
            ViewBag.SupernaturalFaction = new SelectList(db.RefSupernaturalFaction.OrderBy(x => x.SupernaturalFactionId), "SupernaturalFactionId", "Name");
            ViewBag.ImbuedItem = new SelectList(db.ImbuedTable.OrderBy(x => x.Name).ToList(), "Id", "Name");
            ViewBag.ArtifactList = new SelectList(db.ArtifactTable.OrderBy(x => x.Name).ToList(), "Id", "Name");

            return View(npc);
        }

        public IActionResult EditMage(int? id)
        {
            if (id == null) return new StatusCodeResult(400);

            var gameId = HttpContext.Session.GetInt32("GameId");

            if (gameId == null) gameId = 0;

            Npctable npc = db.Npctable.Include(x => x.SupernaturalFactionNavigation).FirstOrDefault(x => x.Npcid == id);

            MageViewModel mageDetails = new MageViewModel();
            mageDetails.MageInfo = db.MageNpctable.FirstOrDefault(x => x.Npcid == id);
            mageDetails.MageArcana = db.MageNpcarcana.Where(x => x.Npcid == id).ToList();
            mageDetails.NpcInfo = npc;

            ViewBag.MentorList = new SelectList(db.Npctable.Where(x => x.SupernaturalFaction == 2 && x.GameId == gameId).OrderBy(x => x.Alias).ToList(), "Npcid", "Alias");
            ViewBag.OrderList = new SelectList(db.MageOrder.OrderBy(x => x.OrderName), "Id", "OrderName");
            ViewBag.PathList = new SelectList(db.MagePath.OrderBy(x => x.Name), "Id", "Name");
            ViewBag.LegacyList = new SelectList(db.RefLegacy.OrderBy(x => x.LegacyName).ToList(), "LegacyId", "LegacyName");
            ViewBag.OrderStatusList = new SelectList(db.MageCaucusInfo.OrderBy(x => x.MageOrderNavigation.OrderName), "MageCaucusId", "Title");
            ViewBag.ConsiliumStatusList = new SelectList(db.MageConsiliumTitles.OrderBy(x => x.Title), "ConsiliumTitleId", "Title");
            ViewBag.ArcanaId = new SelectList(db.ArcanaTable.OrderBy(x => x.Arcana), "Id", "Arcana");
            ViewBag.CabalList = new SelectList(db.MageCabal.Where(x => x.GameId == gameId).OrderBy(x => x.CabalName).ToList(), "MageCabalid", "CabalName");

            return View(mageDetails);
        }

        [HttpPost]
        public IActionResult EditMage(MageViewModel mage)
        {
            var gameId = HttpContext.Session.GetInt32("GameId");

            if (gameId == null) gameId = 0;

            if (ModelState.IsValid)
            {
                db.Entry(mage.MageInfo).State = EntityState.Modified;
                db.SaveChanges();

                for (int i = 0; i < mage.MageArcana.Count; i++)
                {
                    if (mage.MageArcana[i].MageNpcarcanaId > 0 && mage.MageArcana[i].Level > 0) db.Entry(mage.MageArcana[i]).State = EntityState.Modified;
                    else if(mage.MageArcana[i].MageNpcarcanaId > 0 && mage.MageArcana[i].Level == 0)
                    {
                        db.MageNpcarcana.Remove(mage.MageArcana[i]);
                    }
                    else
                    {
                        mage.MageArcana[i].Npcid = mage.MageInfo.Npcid;
                        db.MageNpcarcana.Add(mage.MageArcana[i]);
                    }
                }

                db.SaveChanges();

                MageApprenticeTable apprentice = new MageApprenticeTable();

                if (mage.MageInfo.Mentor != null)
                {
                    if (db.MageApprenticeTable.FirstOrDefault(x => x.Npcid == mage.MageInfo.Npcid) != null)
                    {
                        apprentice = db.MageApprenticeTable.FirstOrDefault(x => x.Npcid == mage.MageInfo.Npcid);
                        apprentice.MentorId = (int)mage.MageInfo.Mentor;
                    }
                    else
                    {
                        apprentice.MentorId = (int)mage.MageInfo.Mentor;
                        apprentice.Npcid = mage.MageInfo.Npcid;
                        db.MageApprenticeTable.Add(apprentice);
                    }
                    db.SaveChanges();
                }
                else
                {
                    if(db.MageApprenticeTable.FirstOrDefault(x => x.Npcid == mage.MageInfo.Npcid) != null)
                    {
                        apprentice = db.MageApprenticeTable.FirstOrDefault(x => x.Npcid == mage.MageInfo.Npcid);
                        db.MageApprenticeTable.Remove(apprentice);
                    }
                }

                return RedirectToAction("MageIndex");
            }

            ViewBag.MentorList = new SelectList(db.Npctable.Where(x => x.SupernaturalFaction == 2 && x.GameId == gameId).OrderBy(x => x.Alias).ToList(), "Npcid", "Alias");
            ViewBag.OrderList = new SelectList(db.MageOrder.OrderBy(x => x.OrderName), "Id", "OrderName");
            ViewBag.PathList = new SelectList(db.MagePath.OrderBy(x => x.Name), "Id", "Name");
            ViewBag.LegacyList = new SelectList(db.RefLegacy.OrderBy(x => x.LegacyName).ToList(), "LegacyId", "LegacyName");
            ViewBag.OrderStatusList = new SelectList(db.MageCaucusInfo.OrderBy(x => x.MageOrderNavigation.OrderName), "MageCaucusId", "Title");
            ViewBag.ConsiliumStatusList = new SelectList(db.MageConsiliumTitles.OrderBy(x => x.Title), "ConsiliumTitleId", "Title");
            ViewBag.ArcanaId = new SelectList(db.ArcanaTable.OrderBy(x => x.Arcana), "Id", "Arcana");
            ViewBag.CabalList = new SelectList(db.MageCabal.Where(x => x.GameId == gameId).OrderBy(x => x.CabalName).ToList(), "MageCabalid", "CabalName");

            return View(mage);
        }

        public async Task<IActionResult> Index()
        {
            var gameId = HttpContext.Session.GetInt32("GameId");

            if(gameId == null) gameId = 0;

            List<Npctable> npcs = await db.Npctable.Where(n => n.GameId == gameId).ToListAsync();

            if (npcs.Count > 0)
                npcs = db.Npctable.Where(n => n.GameId == gameId).Include(x => x.SupernaturalFactionNavigation).ToList();

            return View(npcs);
        }

        public IActionResult MageIndex()
        {
            MageIndexViewModel mageIndex = new MageIndexViewModel();

            var gameId = HttpContext.Session.GetInt32("GameId");

            if (gameId == null) gameId = 0;

            var npcs = view.Where(x => x.GameId == gameId).ToList();

            for(int i = 0; i < npcs.Count; i++)
            {
                int npcId = npcs[i].NPCId;

                if (!mageIndex.MageInfo.Select(x => x.NPCId).Contains(npcId))
                {
                    MageInfo info = new MageInfo();
                    info.NPCId = npcId;
                    info.Notes = npcs[i].Notes;
                    info.Legacy = npcs[i].Legacy;
                    info.Gnosis = npcs[i].Gnosis;
                    info.OrderTitle = npcs[i].OrderTitle;
                    info.ShadowName = npcs[i].ShadowName;
                    info.Cabal = npcs[i].Cabal;
                    info.ConsiliumTitle = npcs[i].ConsiliumTitle;
                    info.Alive = npcs[i].Alive;
                    info.CabalId = npcs[i].CabalId;
                    info.ConsiliumStatusId = npcs[i].ConsiliumStatusId;
                    info.OrderStatusId = npcs[i].OrderStatusId;
                    info.LegacyId = npcs[i].LegacyId;
                    info.Order = npcs[i].Order;

                    List<string> arcanaList = npcs.Where(x => x.NPCId == npcId).Select(x => x.Arcana).ToList();
                    List<int> levelList = npcs.Where(x => x.NPCId == npcId).Select(x => x.Level).ToList();

                    for(int j = 0; j < arcanaList.Count; j++) info.Arcana += arcanaList[j] + " " + levelList[j] + ", ";
                    info.Arcana = info.Arcana.Trim().TrimEnd(',');

                    mageIndex.MageInfo.Add(info);
                }
            }

            List<string> levels = new List<string>() { "1", "2", "3", "4", "5" };

            ViewBag.Levels = new SelectList(levels);
            ViewBag.Arcana = new SelectList(db.ArcanaTable, "Id", "Arcana");

            return View(mageIndex);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
