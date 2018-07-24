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
                    if (booking.BookedEnd < DateTime.Now && booking.Status != 2)
                    {
                        booking.Status = 2;
                    }
                    bookings.Add(booking);
                }
                db.SaveChanges();
            }

            return View(bookings);
        }
    }
}