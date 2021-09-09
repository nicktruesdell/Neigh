using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using neigh.Datamodel;
using neigh.Models;

namespace neigh.Controllers
{
    [RoutePrefix("Admin/HorseInClass")]
    public class HorseInClassController : Controller
    {
        private neighEntities db = new neighEntities();

        [Route("Index/{ShowId:int}")]
        public async Task<ActionResult> Index(int ShowId)
        {
            HorseInClassDisplayModel model = new HorseInClassDisplayModel();
            int iCurrentYear = DateTime.Now.Year;
            var shows = await db.Shows.Where(x => x.StartDate.Year == iCurrentYear).OrderByDescending(x => x.StartDate).ToListAsync();
            foreach (var show in shows)
            {
                model.AllShows.Add(new SelectListItem()
                {
                    Text = show.Name,
                    Value = show.Id.ToString(),
                    Selected = show.Id == ShowId
                });
            }

            Show SelectedShow = shows.Single(x => x.Id == ShowId);

            var AllResults = await (from r in db.Results
                                        //where r.ShowId == shows.First().Id
                                    where r.ShowId == ShowId
                                    select new
                                    {
                                        r.HorseId,
                                        r.Horse.Nickname
                                    }).Distinct().ToListAsync();

            foreach (var result in AllResults)
            {
                model.Items.Add(new HorseInClassDisplayItemModel()
                {
                    HorseId = result.HorseId,
                    HorseName = result.Nickname,
                    ShowId = SelectedShow.Id,
                    ShowName = SelectedShow.Name
                });
            }
            return View(model);
        }

        [Route()]
        [Route("Index")]
        // GET: HorseInClass
        public async Task<ActionResult> Index()
        {
            HorseInClassDisplayModel model = new HorseInClassDisplayModel();
            int iCurrentYear = DateTime.Now.Year;
            var shows = await db.Shows.Where(x => x.StartDate.Year == iCurrentYear).OrderByDescending(x => x.StartDate).ToListAsync();
            foreach (var show in shows)
            {
                model.AllShows.Add(new SelectListItem()
                {
                    Text = show.Name,
                    Value = show.Id.ToString(),
                    Selected = false
                });
            }
            model.AllShows[0].Selected = true;
            int SelectedShowId = shows.First().Id;

            var AllResults = await (from r in db.Results
                                        //where r.ShowId == shows.First().Id
                                    where r.ShowId == SelectedShowId
                                    select new
                                    {
                                        r.HorseId,
                                        r.Horse.Nickname
                                    }).Distinct().ToListAsync();

            foreach (var result in AllResults)
            {
                model.Items.Add(new HorseInClassDisplayItemModel()
                {
                    HorseId = result.HorseId,
                    HorseName = result.Nickname,
                    ShowId = shows.First().Id,
                    ShowName = shows.First().Name
                });
            }
            return View(model);
        }

        [Route("AllShows/{ShowId:int}")]
        public async Task<ActionResult> AllShows(int ShowId)
        {
            HorseInClassDisplayModel model = new HorseInClassDisplayModel();
            var shows = await db.Shows.OrderByDescending(x => x.StartDate).ToListAsync();
            foreach (var show in shows)
            {
                model.AllShows.Add(new SelectListItem()
                {
                    Text = show.Name,
                    Value = show.Id.ToString(),
                    Selected = show.Id == ShowId
                });
            }

            Show SelectedShow = shows.Single(x => x.Id == ShowId);

            var AllResults = await (from r in db.Results
                                        //where r.ShowId == shows.First().Id
                                    where r.ShowId == ShowId
                                    select new
                                    {
                                        r.HorseId,
                                        r.Horse.Nickname
                                    }).Distinct().ToListAsync();

            foreach (var result in AllResults)
            {
                model.Items.Add(new HorseInClassDisplayItemModel()
                {
                    HorseId = result.HorseId,
                    HorseName = result.Nickname,
                    ShowId = SelectedShow.Id,
                    ShowName = SelectedShow.Name
                });
            }
            return View(model);
        }

        [Route("AllShows")]
        // GET: HorseInClass
        public async Task<ActionResult> AllShows()
        {
            HorseInClassDisplayModel model = new HorseInClassDisplayModel();
            var shows = await db.Shows.OrderByDescending(x => x.StartDate).ToListAsync();
            foreach (var show in shows)
            {
                model.AllShows.Add(new SelectListItem()
                {
                    Text = show.Name,
                    Value = show.Id.ToString(),
                    Selected = false
                });
            }
            model.AllShows[0].Selected = true;
            int SelectedShowId = shows.First().Id;

            var AllResults = await (from r in db.Results
                                        //where r.ShowId == shows.First().Id
                                    where r.ShowId == SelectedShowId
                                    select new
                                    {
                                        r.HorseId,
                                        r.Horse.Nickname
                                    }).Distinct().ToListAsync();

            foreach (var result in AllResults)
            {
                model.Items.Add(new HorseInClassDisplayItemModel()
                {
                    HorseId = result.HorseId,
                    HorseName = result.Nickname,
                    ShowId = shows.First().Id,
                    ShowName = shows.First().Name
                });
            }
            return View(model);
        }

        // GET: HorseInClass/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [Route("Create")]
        // GET: HorseInClass/Create
        public async Task<ActionResult> Create()
        {
            HorseInClassEditModel model = new HorseInClassEditModel();
            var AllShows = await db.Shows.ToListAsync();
            var AllHorses = await db.Horses.ToListAsync();
            foreach (var show in AllShows)
            {
                model.AllShows.Add(new SelectListItem()
                {
                    Selected = false,
                    Text = show.Name,
                    Value = show.Id.ToString()
                });
            }
            foreach (var horse in AllHorses)
            {
                model.AllHorses.Add(new SelectListItem()
                {
                    Selected = false,
                    Text = horse.Nickname,
                    Value = horse.Id.ToString()
                });
            }
            return View(model);
        }

        // POST: HorseInClass/Create
        [ValidateAntiForgeryToken]
        [Route("Create")]
        [HttpPost]
        public async Task<ActionResult> Create(HorseInClassEditModel model)
        {
            try
            {
                Show s = await db.Shows.SingleAsync(x => x.Id == model.SelectedShowId);
                foreach (int ClassId in model.SelectedClasses)
                {
                    db.Results.Add(new Result()
                    {
                        ClassId = ClassId,
                        HorseId = model.SelectedHorseId,
                        ShowId = model.SelectedShowId
                    });
                    foreach (Show_Judges sj in s.Show_Judges)
                    {
                        db.Results_Show_Judges.Add(new Results_Show_Judges()
                        {
                            ClassId = ClassId,
                            HorseId = model.SelectedHorseId,
                            ShowId = model.SelectedShowId,
                            Show_JudgeId = sj.Id
                        });
                    }
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: HorseInClass/Edit/5/5
        [Route("Edit/{ShowId:int}/{HorseId:int}")]
        [OutputCache(NoStore = true, Duration = 0)]
        public async Task<ActionResult> Edit(int ShowId, int HorseId)
        {
            HorseInClassEditModel model = new HorseInClassEditModel();
            var AllShows = await db.Shows.ToListAsync();
            var AllHorses = await db.Horses.ToListAsync();
            foreach (var show in AllShows)
            {
                model.AllShows.Add(new SelectListItem()
                {
                    Selected = false,
                    Text = show.Name,
                    Value = show.Id.ToString()
                });
            }
            foreach (var horse in AllHorses)
            {
                model.AllHorses.Add(new SelectListItem()
                {
                    Selected = false,
                    Text = horse.Nickname,
                    Value = horse.Id.ToString()
                });
            }
            model.SelectedShowId = ShowId;
            model.SelectedHorseId = HorseId;

            return View(model);
        }

        // POST: HorseInClass/Edit/5/5
        [ValidateAntiForgeryToken]
        [Route("Edit/{ShowId:int}/{HorseId:int}")]
        [HttpPost]
        public async Task<ActionResult> Edit(int ShowId, int HorseId, HorseInClassEditModel model)
        {
            try
            {
                Show s = await db.Shows.SingleAsync(x => x.Id == model.SelectedShowId);
                var results = await db.Results.Where(x => x.ShowId == ShowId && x.HorseId == HorseId).ToListAsync();
                var judgeresults = await db.Results_Show_Judges.Where(x => x.ShowId == ShowId && x.HorseId == HorseId).ToListAsync();
                // First delete any removed classes and apply any updates
                foreach (var result in results)
                {
                    if (!model.SelectedClasses.Contains(result.ClassId))
                    {
                        db.Entry(result).State = EntityState.Deleted;
                    }
                    else
                    {
                        result.ShowId = model.SelectedShowId;
                        result.HorseId = model.SelectedHorseId;
                    }
                }
                foreach (var judgeresult in judgeresults)
                {
                    if (!model.SelectedClasses.Contains(judgeresult.ClassId))
                    {
                        db.Entry(judgeresult).State = EntityState.Deleted;
                    }
                    else
                    {
                        judgeresult.ShowId = model.SelectedShowId;
                        judgeresult.HorseId = model.SelectedHorseId;
                    }
                }

                // Now add in any new ones
                var NewClassIds = model.SelectedClasses.Where(x => !results.Any(y => y.ClassId == x));
                foreach (int NewClassId in NewClassIds)
                {
                    int iAdded = 1;
                    db.Results.Add(new Result()
                    {
                        ClassId = NewClassId,
                        ShowId = model.SelectedShowId,
                        HorseId = model.SelectedHorseId,
                        SortOrder = results.Select(x => x.SortOrder).Max() + iAdded
                    });
                    iAdded++;

                    foreach (Show_Judges sj in s.Show_Judges)
                    {
                        db.Results_Show_Judges.Add(new Results_Show_Judges()
                        {
                            ClassId = NewClassId,
                            ShowId = model.SelectedShowId,
                            HorseId = model.SelectedHorseId,
                            Show_JudgeId = sj.Id
                        });
                    }
                }

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        // GET: HorseInClass/ForShow/5/6
        [Route("ForShow/{ShowId:int}/{HorseId:int}")]
        [OutputCache(NoStore = true, Duration = 0)]
        public async Task<ActionResult> ForShow(int ShowId, int HorseId)
        {
            Show s = await db.Shows.FindAsync(ShowId);
            ClassType ct = (ClassType)s.ShowType;
            var Classes = await db.Classes.Where(x => x.Type == ct && x.Archive == false).OrderBy(x => x.Name).ToListAsync();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var SingleClass in Classes)
            {
                lst.Add(new SelectListItem()
                {
                    Text = SingleClass.Name,
                    Value = SingleClass.Id.ToString(),
                    Selected = SingleClass.Results.Any(x => x.ShowId == ShowId && x.HorseId == HorseId)
                });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        // GET: HorseInClass/ForShow/5
        [Route("ForShow/{ShowId:int}")]
        public async Task<ActionResult> ForShow(int ShowId)
        {
            Show s = await db.Shows.FindAsync(ShowId);
            ClassType ct = (ClassType)s.ShowType;
            var Classes = await db.Classes.Where(x => x.Type == ct && x.Archive == false).OrderBy(x => x.Name).ToListAsync();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var SingleClass in Classes)
            {
                lst.Add(new SelectListItem()
                {
                    Text = SingleClass.Name,
                    Value = SingleClass.Id.ToString()
                });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        // GET: HorseInClass/ForShowType/5
        [Route("ForShowType/{ShowType:int}")]
        public async Task<ActionResult> ForShowType(int ShowType)
        {
            ClassType ct = (ClassType)ShowType;
            var Classes = await db.Classes.Where(x => x.Type == ct && x.Archive == false).OrderBy(x => x.Name).ToListAsync();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var SingleClass in Classes)
            {
                lst.Add(new SelectListItem()
                {
                    Text = SingleClass.Name,
                    Value = SingleClass.Id.ToString()
                });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        // GET: HorseInClass/ForShow
        public ActionResult ForShow()
        {
            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        // GET: HorseInClass/Delete/5/5
        [Route("Delete/{ShowId:int}/{HorseId:int}")]
        public async Task<ActionResult> Delete(int ShowId, int HorseId)
        {
            var Horse = await db.Horses.FindAsync(HorseId);
            var Show = await db.Shows.FindAsync(ShowId);
            var Results = await db.Results.Include("Class").Where(x => x.HorseId == HorseId && x.ShowId == ShowId).ToListAsync();
            String strClasses = "";

            foreach (var result in Results)
            {
                if (!String.IsNullOrEmpty(strClasses))
                {
                    strClasses += ", ";
                }
                strClasses += result.Class.Name;
            }
            HorseInClassDeleteModel model = new HorseInClassDeleteModel()
            {
                HorseName = Horse.Nickname,
                ShowName = Show.Name,
                Classes = strClasses
            };
            return View(model);
        }

        // POST: HorseInClass/Delete/5/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Delete/{HorseId:int}/{ShowId:int}")]
        public async Task<ActionResult> DeleteConfirmed(int HorseId, int ShowId)
        {
            try
            {
                var Results = await db.Results.Where(x => x.HorseId == HorseId && x.ShowId == ShowId).ToListAsync();
                db.Results.RemoveRange(Results);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
