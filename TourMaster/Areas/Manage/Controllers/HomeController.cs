using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TourMaster.Models;
using TourMaster.Filters;

namespace TourMaster.Areas.Manage.Controllers
{
    [GuideAuth]
    public class HomeController : Controller
    {
        // GET: Manage/Home
        public ActionResult Index()
        {
            User user = Session["User"] as User;
            return RedirectToAction("index", new { controller = "tours", area = "manage", user.Id });
        }

        public ActionResult Logout()
        {
            Session["User"] = null;

            return RedirectToAction("index", new { controller = "login", area = "manage" });
        }
    }
}