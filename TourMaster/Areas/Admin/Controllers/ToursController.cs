using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using TourMaster.Models;
using TourMaster.Filters;

namespace TourMaster.Areas.Admin.Controllers
{
    [AdminAuth]
    public class ToursController : Controller
    {
        TourMasterEntities db = new TourMasterEntities();

        // GET: Admin/Tours
        public ActionResult Index()
        {
            List<Tour> tours = db.Tours.ToList();
            return View(tours);
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }
    }
}