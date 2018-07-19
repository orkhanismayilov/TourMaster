using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TourMaster.Filters;

namespace TourMaster.Areas.Manage.Controllers
{
    [GuideAuth]
    public class HomeController : Controller
    {
        // GET: Manage/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["User"] = null;

            return RedirectToAction("index", new { controller = "login", area = "manage" });
        }
    }
}