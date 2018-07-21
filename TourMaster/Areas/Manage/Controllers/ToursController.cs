﻿using System;
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
            var tours = db.Users.Find(Id).Tours;
            return View(tours.ToList());
        }

        // GET: Manage/Tours/Details/5
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
        public ActionResult Create(TourCreateModel tour)
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
                    AccomodationId = tour.Accomodation != null?tour.Accomodation:null,
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GuideId,FromId,DestinationId,Price,CurrencyId,Category,Duration,DurationTypeId,AccomodationId,AccomodationLevelId,Vehicle,MainImageId,Description,PostedDate,Status")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tour).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccomodationLevelId = new SelectList(db.AccomodationLevels, "Id", "Level", tour.AccomodationLevelId);
            ViewBag.AccomodationId = new SelectList(db.Accomodations, "Id", "AccomodationName", tour.AccomodationId);
            ViewBag.FromId = new SelectList(db.Cities, "Id", "CityName", tour.FromId);
            ViewBag.DestinationId = new SelectList(db.Cities, "Id", "CityName", tour.DestinationId);
            ViewBag.CurrencyId = new SelectList(db.Currencies, "Id", "CurrencyName", tour.CurrencyId);
            ViewBag.DurationTypeId = new SelectList(db.DurationTypes, "Id", "Type", tour.DurationTypeId);
            ViewBag.MainImageId = new SelectList(db.TourImages, "Id", "ImageURL", tour.MainImageId);
            ViewBag.GuideId = new SelectList(db.Users, "Id", "Fullname", tour.GuideId);
            return View(tour);
        }

        [HttpPost]
        public JsonResult SetMainImage(int TourId, int ImageId) {
            Tour tour = db.Tours.Find(TourId);
            tour.MainImageId = ImageId;
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
