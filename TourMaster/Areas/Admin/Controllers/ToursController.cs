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

        [HttpGet]
        public JsonResult Approve(int? Id)
        {
            Tour tour = db.Tours.Find(Id);
            if (tour != null)
            {
                tour.Approved = 1;
                tour.Status = 1;
                string TourTitle = "";
                if (tour.FromId == tour.DestinationId)
                {
                    TourTitle = tour.City.CityName + " Tour";
                }
                else
                {
                    TourTitle = tour.City.CityName + " - " + tour.City1.CityName + " Tour";
                }

                Notification noti = new Notification
                {
                    UserId = tour.GuideId,
                    Text = TourTitle + " approved by administration",
                    Date = DateTime.Now,
                    NotificationTypeId = 6,
                    Link = "/manage/tours/details/" + tour.Id,
                    Status = 0
                };
                db.Notifications.Add(noti);
                db.SaveChanges();

                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Disapprove(int? Id) {
            Tour tour = db.Tours.Find(Id);
            if (tour != null)
            {
                tour.Approved = 0;
                tour.Status = 0;
                string TourTitle = "";
                if (tour.FromId == tour.DestinationId)
                {
                    TourTitle = tour.City.CityName + " Tour";
                }
                else
                {
                    TourTitle = tour.City.CityName + " - " + tour.City1.CityName + " Tour";
                }

                Notification noti = new Notification
                {
                    UserId = tour.GuideId,
                    Text = TourTitle + " disapproved by administration",
                    Date = DateTime.Now,
                    NotificationTypeId = 7,
                    Link = "/manage/tours/details/" + tour.Id,
                    Status = 0
                };
                db.Notifications.Add(noti);
                db.SaveChanges();

                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}