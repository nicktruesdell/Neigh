using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using neigh.Datamodel;
using neigh.Models;

namespace neigh.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            DateTime dtStartDate = DateTime.Today.AddDays(-7);
            neighEntities context = new neighEntities();
            List<Show> lstUpComingShows = await (from s in context.Shows
                                                 where s.StartDate >= dtStartDate
                                                 orderby s.StartDate
                                                 select s).ToListAsync();
            return View(ConvertShowsToModelForView(lstUpComingShows));
        }

        public async Task<ActionResult> AllShows()
        {
            neighEntities context = new neighEntities();
            List<Show> lstUpComingShows = await (from s in context.Shows
                                                 orderby s.StartDate descending
                                                 select s).ToListAsync();
            return View(ConvertShowsToModelForView(lstUpComingShows));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Ping()
        {
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        private List<ShowModel> ConvertShowsToModelForView(List<Show>UpcomingShows)
        {
            List<ShowModel> lst = new List<ShowModel>();
            foreach (Show s in UpcomingShows)
            {
                lst.Add(new ShowModel()
                {
                    Name = s.Name,
                    Id = s.Id
                });
            }
            return lst;
        }
    }
}