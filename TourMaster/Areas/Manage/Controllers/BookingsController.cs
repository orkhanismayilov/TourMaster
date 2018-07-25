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
            List<Booking> bookings = new List<Booking>();
            foreach (Tour tour in db.Users.Find(Id).Tours)
            {
                foreach (Booking booking in tour.Bookings)
                {
                    if (booking.BookedEnd < DateTime.Now && booking.Status == 0)
                    {
                        booking.Status = 1;
                    }
                    bookings.Add(booking);
                }
                db.SaveChanges();
            }

            return View(bookings);
        }

        [HttpPost]
        public JsonResult CancelBooking(int? Id)
        {
            Booking booking = db.Bookings.Find(Id);
            if (booking != null)
            {
                booking.Status = 2;
                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }


    }
}