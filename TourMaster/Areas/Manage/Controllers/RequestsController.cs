using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TourMaster.Models;

namespace TourMaster.Areas.Manage.Controllers
{
    public class RequestsController : Controller
    {
        TourMasterEntities db = new TourMasterEntities();

        // GET: Manage/Requests
        public ActionResult Index(int? Id)
        {
            List<BookingRequest> bookingRequests = db.BookingRequests.Where(br => br.Tour.GuideId == Id).ToList();

            return View(bookingRequests);
        }
    }
}