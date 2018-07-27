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
    public class MessagesController : Controller
    {
        TourMasterEntities db = new TourMasterEntities();

        // GET: Manage/Meassages
        public ActionResult Index(int? Id)
        {
            User user = db.Users.Find(Id);
            
            return View(user);
        }

        [HttpGet]
        public ActionResult Details(int? Id) {

            PrivateMessage pm = db.PrivateMessages.Find(Id);
            pm.ReadStatus = 1;
            db.SaveChanges();

            return View(pm);
        }

        [HttpPost]
        public ActionResult Reply(PrivateMessage privateMessage) {
            if (privateMessage != null)
            {
                privateMessage.Date = DateTime.Now;
                privateMessage.ReadStatus = 0;
                privateMessage.Subject = "Re: " + privateMessage.Subject;
                db.PrivateMessages.Add(privateMessage);
                db.SaveChanges();
            }

            return RedirectToAction("index", new { id = privateMessage.SenderId });
        }
    }
}