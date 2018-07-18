﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TourMaster.Models;

namespace TourMaster.Areas.Manage.Controllers
{
    public class ToursController : Controller
    {
        private TourMasterEntities db = new TourMasterEntities();

        // GET: Manage/Tours
        public ActionResult Index()
        {
            var tours = db.Tours.Include(t => t.AccomodationLevel).Include(t => t.Accomodation).Include(t => t.City).Include(t => t.City1).Include(t => t.Currency).Include(t => t.DurationType).Include(t => t.TourImage).Include(t => t.User);
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
            return View();
        }

        // POST: Manage/Tours/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,GuideId,FromId,DestinationId,Price,CurrencyId,Category,Duration,DurationTypeId,AccomodationId,AccomodationLevelId,Vehicle,MainImageId,Description,PostedDate,Status")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                db.Tours.Add(tour);
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

        // GET: Manage/Tours/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Manage/Tours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tour tour = db.Tours.Find(id);
            db.Tours.Remove(tour);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}