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
    public class SettingsController : Controller
    {
        TourMasterEntities db = new TourMasterEntities();

        // GET: Manage/Settings
        public ActionResult Index(int? Id)
        {
            User user = db.Users.Find(Id);
            ViewBag.Countries = db.Countries.ToList();

            return View(user);
        }
    }
}