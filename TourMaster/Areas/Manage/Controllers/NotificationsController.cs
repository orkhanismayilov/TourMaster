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
    public class NotificationsController : Controller
    {
        TourMasterEntities db = new TourMasterEntities();

        // GET: Manage/Notifications
        public ActionResult Index(int? Id)
        {
            List<Notification> notis = db.Users.Find(Id).Notifications.ToList();
            
            return View(notis);
        }
    }
}