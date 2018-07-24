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
                foreach (Tour tour in guide.Tours)
                {
                    foreach (Booking booking in tour.Bookings)
                    {
                        if ((booking.BookedStart >= br.StartDate && booking.BookedStart <= br.EndDate) || (booking.BookedEnd >= br.StartDate && booking.BookedEnd <= br.EndDate))
                        {
                            busy = 1;
                            return Json(0, JsonRequestBehavior.AllowGet); ;
                        }
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
                    br.Status = 1;
                    db.SaveChanges();
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(2, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult RejectBooking(int? Id) {
            BookingRequest br = db.BookingRequests.Find(Id);
            if (br != null)
            {
                br.Status = 2;
                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

    }
}