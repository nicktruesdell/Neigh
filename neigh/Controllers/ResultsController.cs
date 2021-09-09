using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using neigh.Datamodel;
using neigh.Models;
using Newtonsoft.Json;

namespace neigh.Controllers
{
    [System.Web.Mvc.RoutePrefix("Results")]
    public class ResultsController : Controller
    {
        [System.Web.Mvc.Route("{showid:int}")]
        // GET: Results/5
        public async Task<ActionResult> Index(int showid)
        {
            List<Result> lstResults;
            neighEntities context = new neighEntities();
            lstResults = await (from r in context.Results
                          where r.ShowId == showid
                          orderby r.SortOrder
                          select r).ToListAsync();
            ViewBag.ShowName = lstResults.First().Show.Name;
            ViewBag.ShowOverallPlacing = lstResults.First().Show.Level > 0;
            ViewBag.Judges = lstResults.First().Show.Show_Judges;
            return View(lstResults);
        }

        public async Task<ActionResult> Edit(int[] HorseId, int[] ClassId, int[] ShowId)
        {
            neighEntities context = new neighEntities();
            List<Result> lst = new List<Result>();
            List<ResultEditModel> lstModel = new List<ResultEditModel>();
            for (int i = 0; i < HorseId.Count(); i++)
            {
                int iHorseId = HorseId[i];
                int iClassId = ClassId[i];
                int iShowId = ShowId[i];
                lst.Add(await context.Results.SingleAsync(x => x.HorseId == iHorseId && x.ClassId == iClassId && x.ShowId == iShowId));
            }
            foreach (Result r in lst)
            {
                lstModel.Add(new ResultEditModel(r));
            }
            //String strJSON = JsonConvert.SerializeObject(lst, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Serialize, MaxDepth = 2 });
            String strJSON = JsonConvert.SerializeObject(lstModel, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, MaxDepth = 3 });
            ViewBag.ShowOverallPlacing = (context.Shows.Find(ShowId.First()).Level > 0 ? "true" : "false");
            ViewBag.JSONData = strJSON;
            ViewBag.ReturnURL = "/Results/" + ShowId[0].ToString();
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Edit([FromBody]Result[] results)
        {
            //Result[] results = JsonConvert.DeserializeObject<Result[]>(strJSON);
            neighEntities context = new neighEntities();
            foreach (Result r in results)
            {
                //context.Entry(r).State = EntityState.Modified;
                Result rToModify = await context.Results.SingleAsync(x => x.ShowId == r.ShowId && x.HorseId == r.HorseId && x.ClassId == r.ClassId);
                rToModify.HorsesInClass = r.HorsesInClass;
                rToModify.OverallPlacing = r.OverallPlacing;
                rToModify.ClassNumber = r.ClassNumber;

                foreach (Results_Show_Judges rsj in r.Results_Show_Judges)
                {
                    //context.Entry(rsj).State = EntityState.Modified;
                    Results_Show_Judges rsjToModify = rToModify.Results_Show_Judges.Single(x => x.Show_JudgeId == rsj.Show_JudgeId);
                    rsjToModify.Placing = rsj.Placing;
                }            
            }
            await context.SaveChangesAsync();
            return new HttpStatusCodeResult(200);
        }

        public async Task<ActionResult> Reorder(int ShowId)
        {
            neighEntities context = new neighEntities();
            var results = await (from r in context.Results
                                 where r.ShowId == ShowId
                                 orderby r.SortOrder
                                 select r).ToListAsync();

            List<ResultReorderModel> lstResults = new List<ResultReorderModel>();
            foreach (var result in results)
            {
                lstResults.Add(new ResultReorderModel()
                {
                    ClassName = result.Class.Name,
                    ClassId = result.ClassId,
                    ClassNumber = result.ClassNumber,
                    HorseId = result.HorseId,
                    HorseName = result.Horse.Nickname,
                    SortOrder = result.SortOrder != null ? result.SortOrder.ToString() : ""
                });
            }

            ViewBag.ShowName = results.First().Show.Name;
            ViewBag.ReturnURL = "/Results/" + results.First().ShowId.ToString();
            ViewBag.ResultsCount = results.Count();
            ViewBag.JSONData = Newtonsoft.Json.JsonConvert.SerializeObject(lstResults);

            return View();
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Reorder(int ShowId, [FromBody]ResultReorderModel[] results)
        {
            neighEntities context = new neighEntities();
            var dbResults = await (from r in context.Results
                                   where r.ShowId == ShowId
                                   select r).ToListAsync();

            foreach (var dbResult in dbResults)
            {
                var modelItem = results.FirstOrDefault(x => x.ClassId == dbResult.ClassId && x.HorseId == dbResult.HorseId);
                if (modelItem != null)
                {
                    int? iModelSort = int.Parse(modelItem.SortOrder);
                    if (dbResult.SortOrder != iModelSort)
                    {
                        dbResult.SortOrder = iModelSort;
                    }
                }
            }

            await context.SaveChangesAsync();

            return new HttpStatusCodeResult(200);
        }
    }
}