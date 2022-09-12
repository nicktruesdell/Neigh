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
    [RoutePrefix("Admin/Shows")]
    public class ShowsController : Controller
    {
        private neighEntities db = new neighEntities();

        [Route]
        [Route("Index")]
        // GET: Shows
        public async Task<ActionResult> Index()
        {
            var shows = await db.Shows.Include("Show_Judges").OrderByDescending(x => x.StartDate).ToListAsync();
            List<ShowModel> lst = new List<ShowModel>();
            foreach (var show in shows)
            {
                ShowModel sm = new ShowModel()
                {
                    Id = show.Id,
                    Type = (ShowType)show.ShowType,
                    Name = show.Name,
                    StartDate = show.StartDate,
                    EndDate = show.EndDate,
                    Location = show.Location,
                    Level = (ShowLevel)show.Level,
                    Judges = new List<string>()
                };
                foreach (var Show_Judge in show.Show_Judges)
                {
                    sm.Judges.Add(Show_Judge.Name);
                }
                lst.Add(sm);
            }
            return View(lst);
        }

        [Route("Create")]
        // GET: Shows/Create
        public ActionResult Create()
        {
            ShowCopyModel sm = new ShowCopyModel();
            sm.Judges = new List<string>();
            sm.Judges.Add("");
            return View(sm);
        }

        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ShowModel sm)
        {
            if (ModelState.IsValid)
            {
                Show s = new Show()
                {
                    Name = sm.Name,
                    Location = sm.Location,
                    StartDate = (DateTime)sm.StartDate,
                    EndDate = (DateTime)sm.EndDate,
                    ShowType = (int)sm.Type,
                    Level = (int)sm.Level
                };
                foreach (String strJudge in sm.Judges)
                {
                    s.Show_Judges.Add(new Show_Judges()
                    {
                        Name = strJudge
                    });
                }
                db.Shows.Add(s);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sm);
        }

        [Route("Delete/{id:int}")]
        // GET: Shows/Delete/id
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Show show = await db.Shows.FindAsync(id);
            if (show == null)
            {
                return HttpNotFound();
            }
            ShowModel sm = new ShowModel()
            {
                Name = show.Name,
                Location = show.Location,
                StartDate = show.StartDate,
                EndDate = show.EndDate
            };
            return View(sm);
        }

        [Route("Delete/{id:int}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            Show s = await db.Shows.FindAsync(id);
            foreach (Show_Judges sj in s.Show_Judges)
            {
                db.Results_Show_Judges.RemoveRange(sj.Results_Show_Judges);
            }
            db.Show_Judges.RemoveRange(s.Show_Judges);
            db.Results.RemoveRange(s.Results);
            db.Shows.Remove(s);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Route("Edit/{id:int}")]
        // GET: Shows/Edit/id
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Show show = await db.Shows.Include("Show_Judges").SingleOrDefaultAsync(x => x.Id == id);
            if (show == null)
            {
                return HttpNotFound();
            }
            ShowModel sm = new ShowModel()
            {
                Type = (ShowType)show.ShowType,
                Name = show.Name,
                Location = show.Location,
                StartDate = show.StartDate,
                EndDate = show.EndDate,
                Level = (ShowLevel)show.Level,
                Judges = new List<string>()
            };
            foreach (Show_Judges sj in show.Show_Judges)
            {
                sm.Judges.Add(sj.Name);
            }
            return View(sm);
        }

        [Route("Edit/{id:int}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ShowModel model)
        {
            if (ModelState.IsValid)
            {
                Show s = await db.Shows.Include("Show_Judges").SingleOrDefaultAsync(x => x.Id == model.Id);
                if (s == null)
                {
                    return HttpNotFound();
                }

                s.EndDate = (DateTime)model.EndDate;
                s.Location = model.Location;
                s.Name = model.Name;
                s.ShowType = (int)model.Type;
                s.StartDate = (DateTime)model.StartDate;
                s.Level = (int)model.Level;

                foreach (Show_Judges sj in s.Show_Judges.ToList())
                {
                    if (!model.Judges.Contains(sj.Name))
                    {
                        var ResultsToRemove = from r in sj.Results_Show_Judges
                                              where r.ShowId == s.Id
                                              select r;
                        foreach (var result in ResultsToRemove.ToList())
                        {
                            db.Entry(result).State = EntityState.Deleted;
                        }
                        db.Entry(sj).State = EntityState.Deleted;
                    }
                }

                foreach (String str in model.Judges)
                {
                    if (!s.Show_Judges.Any(x => x.Name == str))
                    {
                        Show_Judges sj = new Show_Judges()
                        {
                            Name = str
                        };
                        foreach (var result in s.Results)
                        {
                            sj.Results_Show_Judges.Add(new Results_Show_Judges()
                            {
                                ShowId = s.Id,
                                HorseId = result.HorseId,
                                ClassId = result.ClassId
                            });
                        }
                        //s.Show_Judges.Add(new Show_Judges()
                        //{
                        //    Name = str
                        //});
                        s.Show_Judges.Add(sj);
                    }
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        //[Route("Copy/{id:int}")]
        //[Route("Copy")]
        // GET: Shows/Copy/id
        public async Task<ActionResult> Copy(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Show show = await db.Shows.Include("Show_Judges").SingleOrDefaultAsync(x => x.Id == id);
            if (show == null)
            {
                return HttpNotFound();
            }
            ShowCopyModel sm = new ShowCopyModel()
            {
                Id = show.Id,
                Type = (ShowType)show.ShowType,
                //Name = show.Name,
                Location = show.Location,
                //StartDate = show.StartDate,
                //EndDate = show.EndDate,
                Level = (ShowLevel)show.Level,
                Judges = new List<string>(),
                Entries = new List<ResultReorderForCopyModel>()
            };
            foreach (Show_Judges sj in show.Show_Judges)
            {
                sm.Judges.Add(sj.Name);
            }
            foreach (Result res in show.Results)
            {
                sm.Entries.Add(new ResultReorderForCopyModel()
                {
                    HorseId = res.HorseId,
                    ClassId = res.ClassId.ToString(),
                    SortOrder = (res.SortOrder != null ? res.SortOrder.ToString() : ""),
                    ClassNumber = res.ClassNumber
                });
            }
            return View(sm);
        }

        [HttpPost]
        //[Route("Copy")]
        // POST: Shows/Copy
        public async Task<ActionResult> Copy([System.Web.Http.FromBody]ShowCopyModel model)
        {
            Show s = new Datamodel.Show();
            s.Name = model.Name;
            s.Location = model.Location;
            s.ShowType = (int)model.Type;
            s.Level = (int)model.Level;
            s.StartDate = (DateTime)model.StartDate;
            s.EndDate = (DateTime)model.EndDate;

            foreach (string str in model.Judges)
            {
                s.Show_Judges.Add(new Show_Judges()
                {
                    Name = str
                });
            }

            foreach (ResultReorderForCopyModel entry in model.Entries)
            {
                Result res = new Result()
                {
                    ClassId = int.Parse(entry.ClassId),
                    HorseId = entry.HorseId,
                    SortOrder = int.Parse(entry.SortOrder),
                    ClassNumber = entry.ClassNumber
                };
                s.Results.Add(res);
                   
            }
            int i = 1;
            foreach (Result res in s.Results.OrderBy(x => x.SortOrder))
            {
                res.SortOrder = i;
                i++;
            }

            db.Shows.Add(s);
            await db.SaveChangesAsync();

            foreach (Show_Judges sj in s.Show_Judges)
            {
                foreach (Result res in s.Results)
                {
                    db.Results_Show_Judges.Add(new Results_Show_Judges()
                    {
                        ShowId = s.Id,
                        HorseId = res.HorseId,
                        Show_JudgeId = sj.Id,
                        ClassId = res.ClassId
                    });
                }
            }
            await db.SaveChangesAsync();
            return new HttpStatusCodeResult(200);
        }

        public async Task<ActionResult> Copy_Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Show show = await db.Shows.Include("Show_Judges").Include("Results").SingleOrDefaultAsync(x => x.Id == id);
            if (show == null)
            {
                return HttpNotFound();
            }
            ShowCopyModel sm = new ShowCopyModel()
            {
                Id = show.Id,
                Type = (ShowType)show.ShowType,
                Name = show.Name,
                Location = show.Location,
                StartDate = show.StartDate,
                EndDate = show.EndDate,
                Level = (ShowLevel)show.Level,
                Judges = new List<string>(),
                Entries = new List<ResultReorderForCopyModel>()
            };
            
            foreach (Show_Judges sj in show.Show_Judges)
            {
                sm.Judges.Add(sj.Name);
            }
            foreach (Result res in show.Results.OrderBy(x => x.SortOrder))
            {
                sm.Entries.Add(new ResultReorderForCopyModel()
                {
                    HorseId = res.HorseId,
                    OldHorseId = res.HorseId,
                    ClassId = res.ClassId.ToString(),
                    OldClassId = res.ClassId,
                    SortOrder = (res.SortOrder != null ? res.SortOrder.ToString() : ""),
                    ClassNumber = res.ClassNumber
                });
            }
            return View(sm);
        }

        [HttpPost]
		public async Task<ActionResult> Copy_Edit(ShowCopyModel model)
		{
	        Show show = await db.Shows.Include("Show_Judges").Include("Results").Include("Results.Results_Show_Judges").SingleOrDefaultAsync(x => x.Id == model.Id);
			show.ShowType = (int)model.Type;
			show.Name = model.Name;
			show.Location = model.Location;
			show.StartDate = (DateTime)model.StartDate;
			show.EndDate = (DateTime)model.EndDate;
			show.Level = (int)model.Level;

			// Add in any new judges
			foreach (string name in model.Judges)
			{
				if (!show.Show_Judges.Any(judge => judge.Name == name))
				{
					show.Show_Judges.Add(new Show_Judges()
					{
						Name = name
					});
				}
			}

			// Remove any deleted judges
			foreach (Show_Judges judge in show.Show_Judges)
			{
				if (!model.Judges.Any(x => judge.Name == x))
				{
					db.Entry(judge).State = EntityState.Deleted;
				}
			}

			// Remove any deleted entries
			// Don't iterate over show.Results - throws InvalidOperationException if an item is removed
            for (int i = 0; i < show.Results.Count; i++)
			{
                var result = show.Results.ElementAt(i);
				if (!model.Entries.Any(e => e.OldClassId == result.ClassId && e.OldHorseId == result.HorseId))
				{
                    RemoveResultShowJudgeEntities(result);
                    db.Entry(result).State = EntityState.Deleted;
				}
			}

			// Update any changed entries
			foreach (var entry in model.Entries)
			{
				Result r = show.Results.FirstOrDefault(x => x.HorseId == entry.OldHorseId && x.ClassId == entry.OldClassId);
				if (r != null)
				{
					if (int.Parse(entry.ClassId) != r.ClassId || entry.HorseId != r.HorseId)
					{
                        RemoveResultShowJudgeEntities(r);
                        db.Entry(r).State = EntityState.Deleted;
                        show.Results.Add(new Result()
                        {
                            ClassId = int.Parse(entry.ClassId),
                            ClassNumber = entry.ClassNumber,
                            HorseId = entry.HorseId,
                            SortOrder = int.Parse(entry.SortOrder)
                        });
                    }
					else
					{
						// Can't change horse or class - PK violation - handled above
						r.ClassNumber = entry.ClassNumber;
						r.SortOrder = int.Parse(entry.SortOrder);
					}
				}
				else
				{
					show.Results.Add(new Result()
					{
						ClassId = int.Parse(entry.ClassId),
						ClassNumber = entry.ClassNumber,
						HorseId = entry.HorseId,
						SortOrder = int.Parse(entry.SortOrder)
					});
				}
			}

            try
            {
                await db.SaveChangesAsync();
            }
			catch (Exception e)
            {

            }

            // Need to re-sync results and judges
            foreach (Show_Judges sj in show.Show_Judges)
            {
                foreach (Result res in show.Results)
                {
                    if (!res.Results_Show_Judges.Any(x => x.Show_JudgeId == sj.Id))
                    {
                        db.Results_Show_Judges.Add(new Results_Show_Judges()
                        {
                            ShowId = show.Id,
                            HorseId = res.HorseId,
                            Show_JudgeId = sj.Id,
                            ClassId = res.ClassId
                        });
                    }
                }
            }
            await db.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
		}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void RemoveResultShowJudgeEntities(Result result)
        {
            foreach (Results_Show_Judges entity in result.Results_Show_Judges.ToList())
            {
                db.Entry(entity).State = EntityState.Deleted;
            }
        }
    }
}