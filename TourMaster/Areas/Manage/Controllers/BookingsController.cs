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
    public class BookingsController : Controller
    {
        TourMasterEntities db = new TourMasterEntities();

        // GET: Manage/Bookings
        public ActionResult Index(int? Id)
        {
            List<Booking> bookings = db.Users.Find(Id).Bookings.ToList();

            return View(bookings);
        }
    }
}