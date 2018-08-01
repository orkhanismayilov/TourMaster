using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TourMaster.Models;
using TourMaster.Filters;

namespace TourMaster.Areas.Manage.Controllers
{
    [GuideAuth]
    public class ToursController : Controller
    {
        private TourMasterEntities db = new TourMasterEntities();

        // GET: Manage/Tours
        public ActionResult Index(int? Id)
        {
            List<Tour> tours = db.Users.Find(Id).Tours.ToList();
            return View(tours);
        }

        // GET: Manage/Tours/Details/5
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

        // GET: Manage/Tours/Create
        public ActionResult Create()
        {
            ViewBag.AccomodationLevelId = new SelectList(db.AccomodationLevels, "Id", "Level");
            ViewBag.AccomodationId = new SelectList(db.Accomodations, "Id", "AccomodationName");
            ViewBag.FromId = new SelectList(db.Cities, "Id", "CityName");
            ViewBag.DestinationId = new SelectList(db.Cities, "Id", "CityName");
            ViewBag.CurrencyId = new SelectList(db.Currencies, "Id", "CurrencyName");
            ViewBag.DurationTypeId = new SelectList(db.DurationTypes, "Id", "Type");
            ViewBag.MainImageId = new SelectList(db.TourImages, "Id", "ImageURL");
            ViewBag.GuideId = new SelectList(db.Users, "Id", "Fullname");
            ViewBag.Countries = db.Countries.ToList();
            ViewBag.DurationTypes = db.DurationTypes.ToList();
            ViewBag.Currency = db.Currencies.ToList();
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Accomodations = db.Accomodations.ToList();
            ViewBag.AccomodationLvl = db.AccomodationLevels.ToList();
            return View();
        }

        // POST: Manage/Tours/Create
        [HttpPost]
        public ActionResult Create(TourCreateEditModel tour)
        {
            if (ModelState.IsValid)
            {
                List<string> Categories = new List<string>();
                foreach (int catId in tour.Categories)
                {
                    Categories.Add(db.Categories.Find(catId).CategoryName.ToLower());
                }

                Tour newTour = new Tour
                {
                    GuideId = tour.GuideId,
                    FromId = tour.FromCity,
                    DestinationId = tour.DestCity,
                    Price = (decimal)tour.Price,
                    CurrencyId = tour.Currency,
                    Category = String.Join(",", Categories.ToArray()),
                    Duration = tour.Duration,
                    DurationTypeId = tour.DurationType,
                    AccomodationId = tour.Accomodation != null ? tour.Accomodation : null,
                    AccomodationLevelId = tour.AccomodationLvl != null ? tour.AccomodationLvl : null,
                    Vehicle = tour.Transport,
                    Description = tour.Description,
                    PostedDate = DateTime.Now,
                    MainImageId = 1,
                    Status = 0,
                    Approved = 0
                };
                db.Tours.Add(newTour);
                db.SaveChanges();

                if (tour.Images.Count > 0)
                {
                    foreach (HttpPostedFileBase img in tour.Images)
                    {
                        string fileName = tour.GuideId + DateTime.Now.ToString("yyyyMMddHHmmss") + img.FileName;
                        string path = System.IO.Path.Combine(Server.MapPath("~/uploads"), fileName);
                        img.SaveAs(path);
                        string image = "/uploads/" + fileName;
                        TourImage tourImage = new TourImage
                        {
                            ImageURL = image,
                            TourId = newTour.Id,
                        };
                        db.TourImages.Add(tourImage);
                        db.SaveChanges();
                    }
                }

                newTour.MainImageId = db.TourImages.Where(ti => ti.TourId == newTour.Id).OrderBy(ti => ti.Id).FirstOrDefault().Id;
                db.SaveChanges();

                Notification noti = new Notification
                {
                    UserId = db.Users.FirstOrDefault(u => u.AccountType == 2).Id,
                    Text = "New tour from " + newTour.User.Fullname,
                    Date = DateTime.Now,
                    NotificationTypeId = 8,
                    Status = 0,
                    Link = "admin/tours/details/" + newTour.Id
                };
                db.Notifications.Add(noti);
                db.SaveChanges();
            };

            return RedirectToAction("index", new { controller = "tours", area = "manage", id = tour.GuideId });
        }

        // GET: Manage/Tours/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.AccomodationLevelId = new SelectList(db.AccomodationLevels, "Id", "Level", tour.AccomodationLevelId);
            ViewBag.AccomodationId = new SelectList(db.Accomodations, "Id", "AccomodationName", tour.AccomodationId);
            ViewBag.FromId = new SelectList(db.Cities, "Id", "CityName", tour.FromId);
            ViewBag.DestinationId = new SelectList(db.Cities, "Id", "CityName", tour.DestinationId);
            ViewBag.CurrencyId = new SelectList(db.Currencies, "Id", "CurrencyName", tour.CurrencyId);
            ViewBag.DurationTypeId = new SelectList(db.DurationTypes, "Id", "Type", tour.DurationTypeId);
            ViewBag.MainImageId = new SelectList(db.TourImages, "Id", "ImageURL", tour.MainImageId);
            ViewBag.GuideId = new SelectList(db.Users, "Id", "Fullname", tour.GuideId);
            ViewBag.Countries = db.Countries.ToList();
            ViewBag.DurationTypes = db.DurationTypes.ToList();
            ViewBag.Currency = db.Currencies.ToList();
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Accomodations = db.Accomodations.ToList();
            ViewBag.AccomodationLvl = db.AccomodationLevels.ToList();
            return View(tour);
        }

        // POST: Manage/Tours/Edit/5
        [HttpPost]
        public ActionResult Edit(TourCreateEditModel tour, int Id)
        {
            Tour thisTour = db.Tours.Find(Id);
            List<string> Categories = new List<string>();
            foreach (int catId in tour.Categories)
            {
                Categories.Add(db.Categories.Find(catId).CategoryName.ToLower());
            }

            if (ModelState.IsValid)
            {
                thisTour.FromId = tour.FromCity;
                thisTour.DestinationId = tour.DestCity;
                thisTour.Price = (decimal)tour.Price;
                thisTour.CurrencyId = tour.Currency;
                thisTour.Duration = tour.Duration;
                thisTour.DurationTypeId = tour.DurationType;
                thisTour.Category = String.Join(",", Categories.ToArray());
                thisTour.AccomodationId = tour.Accomodation != null ? tour.Accomodation : null;
                thisTour.AccomodationLevelId = tour.AccomodationLvl != null ? tour.AccomodationLvl : null;
                thisTour.Vehicle = tour.Transport;
                thisTour.Description = tour.Description;
                thisTour.Approved = 0;

                db.Entry(thisTour).State = EntityState.Modified;
                db.SaveChanges();
            }

            if (tour.Images.Count > 0)
            {
                foreach (HttpPostedFileBase img in tour.Images)
                {
                    string fileName = tour.GuideId + DateTime.Now.ToString("yyyyMMddHHmmss") + img.FileName;
                    string path = System.IO.Path.Combine(Server.MapPath("~/uploads"), fileName);
                    img.SaveAs(path);
                    string image = "/uploads/" + fileName;
                    TourImage tourImage = new TourImage
                    {
                        ImageURL = image,
                        TourId = thisTour.Id,
                    };
                    db.TourImages.Add(tourImage);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("edit", new { thisTour.Id });
        }

        [HttpPost]
        public JsonResult SetMainImage(int TourId, int ImageId)
        {
            Tour tour = db.Tours.Find(TourId);
            tour.MainImageId = ImageId;
            db.SaveChanges();

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteTourImage(int Id)
        {
            TourImage timg = db.TourImages.Find(Id);
            Tour tour = db.Tours.FirstOrDefault(t => t.MainImageId == Id);
            if (tour != null)
            {
                tour.MainImageId = 1;
            }
            timg.TourId = null;
            db.SaveChanges();
            string path = Server.MapPath(timg.ImageURL);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            db.TourImages.Remove(timg);
            db.SaveChanges();

            return Json(1, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult Disable(int? Id)
        {
            Tour tour = db.Tours.Find(Id);
            if (tour != null)
            {
                tour.Status = 0;
                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Activate(int? Id)
        {
            Tour tour = db.Tours.Find(Id);
            if (tour != null)
            {
                tour.Status = 1;
                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
