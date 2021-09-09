using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace neigh.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        [Route]
        [Route("Index")]
        public ActionResult Index()
        {
            return View();
        }
    }
}