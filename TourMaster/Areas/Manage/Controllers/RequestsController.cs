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
    public class RequestsController : Controller
    {
        TourMasterEntities db = new TourMasterEntities();

        // GET: Manage/Requests
        public ActionResult Index(int? Id)
        {
            List<BookingRequest> bookingRequests = db.BookingRequests.Where(br => br.Tour.GuideId == Id).ToList();

            return View(bookingRequests);
        }

        [HttpPost]
        public JsonResult ConfirmBooking(int? Id)
        {
            BookingRequest br = db.BookingRequests.Find(Id);
            if (br != null)
            {
                User guide = br.Tour.User;

                int busy = 0;
                foreach (Booking booking in guide.Bookings.Where(b => b.Status == 1))
                {
                    if ((booking.BookedStart >= br.StartDate && booking.BookedStart <= br.EndDate)||(booking.BookedEnd >= br.StartDate && booking.BookedEnd <= br.EndDate ))
                    {
                        busy = 1;
                        return Json(0, JsonRequestBehavior.AllowGet); ;
                    }
                }

                if (busy == 0)
                {
                    Booking newBooking = new Booking
                    {
                        TourId = br.TourId,
                        TravelerId = br.UserId,
                        BookingDate = DateTime.Now,
                        BookedStart = br.StartDate,
                        BookedEnd = br.EndDate,
                        BookedPrice = br.Tour.Price,
                        CurrencyId = br.Tour.CurrencyId,
                        Status = 1
                    };
                    db.Bookings.Add(newBooking);
                    db.SaveChanges();
                }
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RejectBooking(int? Id) {




            return Json(1, JsonRequestBehavior.AllowGet);
        }

    }
}