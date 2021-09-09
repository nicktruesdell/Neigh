using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using neigh.Datamodel;
using neigh.Models;

namespace neigh.Controllers
{
    [RoutePrefix("Admin/Horses")]
    public class HorsesController : Controller
    {
        private neighEntities db = new neighEntities();

        [Route]
        [Route("Index")]
        // GET: Horses
        public async Task<ActionResult> Index()
        {
            return View(await db.Horses.OrderBy(x => x.Nickname).ToListAsync());
        }

        [Route("GetAll")]
        // GET: Horses/GetAll
        public async Task<ActionResult> GetAll()
        {
            var horses = await db.Horses.OrderBy(x => x.Nickname).ToListAsync();
            List<IntAndStringModel> lst = new List<IntAndStringModel>();

            foreach (var horse in horses)
            {
                lst.Add(new IntAndStringModel()
                {
                    Value = horse.Id,
                    Text = horse.Nickname
                });
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [Route("Details/{id:int}")]
        // GET: Horses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Horse horse = await db.Horses.FindAsync(id);
            if (horse == null)
            {
                return HttpNotFound();
            }
            return View(horse);
        }

        [Route("Create")]
        // GET: Horses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Horses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,FullName,Nickname,ARegNumber,RRegNumber")] Horse horse)
        {
            if (ModelState.IsValid)
            {
                db.Horses.Add(horse);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(horse);
        }

        [Route("Edit/{id:int}")]
        // GET: Horses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Horse horse = await db.Horses.FindAsync(id);
            if (horse == null)
            {
                return HttpNotFound();
            }
            return View(horse);
        }

        // POST: Horses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FullName,Nickname,ARegNumber,RRegNumber")] Horse horse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(horse).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(horse);
        }

        [Route("Delete/{id:int}")]
        // GET: Horses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Horse horse = await db.Horses.FindAsync(id);
            if (horse == null)
            {
                return HttpNotFound();
            }
            return View(horse);
        }

        // POST: Horses/Delete/5
        [Route("Delete/{id:int}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Horse horse = await db.Horses.FindAsync(id);
            db.Horses.Remove(horse);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
