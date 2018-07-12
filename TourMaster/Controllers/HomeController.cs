﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using TourMaster.Models;

namespace TourMaster.Controllers
{
    public class HomeController : Controller
    {
        TourMasterEntities db = new TourMasterEntities();

        public ActionResult Index()
        {
            ViewBag.Origin = db.Origins.FirstOrDefault();
            ViewBag.Countries = db.Countries.ToList();
            ViewModel model = new ViewModel
            {
                Users = db.Users.ToList(),
                Tours = db.Tours.ToList(),
                Categories = db.Categories.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult SignUp(SignUpForm signUpForm)
        {
            if (!String.IsNullOrWhiteSpace(signUpForm.Name)
                && !String.IsNullOrWhiteSpace(signUpForm.Surname)
                && !String.IsNullOrWhiteSpace(signUpForm.Email)
                && !String.IsNullOrWhiteSpace(signUpForm.Password)
                && !String.IsNullOrWhiteSpace(signUpForm.PasswordConfirmation)
                && !String.IsNullOrWhiteSpace(signUpForm.Phone)
                && signUpForm.Birthday != null
                && !String.IsNullOrWhiteSpace(signUpForm.CityId.ToString())
                && !String.IsNullOrWhiteSpace(signUpForm.AccountTypeId.ToString()))
            {
                if (db.Users.FirstOrDefault(u => u.Email == signUpForm.Email) == null)
                {
                    if (signUpForm.Password == signUpForm.PasswordConfirmation)
                    {
                        User newuser = new User
                        {
                            Fullname = signUpForm.Name + " " + signUpForm.Surname,
                            Email = signUpForm.Email,
                            Birthday = signUpForm.Birthday,
                            Password = Crypto.HashPassword(signUpForm.Password),
                            Phone = signUpForm.Phone,
                            AccountType = signUpForm.AccountTypeId,
                            CityId = signUpForm.CityId
                        };
                        db.Users.Add(newuser);
                        db.SaveChanges();
                        Session["SignedUpSuccess"] = true;
                    }
                    else
                    {
                        Session["SignedUpError"] = true;
                        Session["SignUpMsg"] = "Password and Password Confirmation are not the matching.";
                    }
                }
                else
                {
                    Session["SignedUpError"] = true;
                    Session["SignUpMsg"] = "User with this email already exists.";
                }
            }
            else
            {
                Session["SignedUpError"] = true;
                Session["SignUpMsg"] = "Please, fill all the inputs.";
            }

            return RedirectToAction("index");
        }

        [HttpGet]
        public JsonResult GetCities(int Id)
        {
            var cities = db.Cities.Where(c => c.CountryId == Id).Select(c => new
            {
                c.Id,
                c.CityName
            }).ToList();
            return Json(cities, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTourInfo(int Id)
        {
            Tour tour = db.Tours.Find(Id);

            List<TourImage> timg = tour.TourImages.ToList();
            List<string> timgurls = new List<string>();
            foreach (TourImage img in timg)
            {
                timgurls.Add(img.ImageURL);
            }

            string TourTitle = "";
            if (tour.FromId == tour.DestinationId)
            {
                TourTitle = tour.City.CityName + " Tour";
            }
            else
            {
                TourTitle = tour.City.CityName + " - " + tour.City1.CityName + " Tour";
            }

            List<Feedback> feedbacks = tour.Feedbacks.ToList();
            List<string> feedbacksList = new List<string>();
            foreach (Feedback fdbck in feedbacks)
            {
                string feedbackInfo = String.Join("/", new
                {
                    fdbck.Id,
                    fdbck.Text,
                    fdbck.Rating,
                    fdbck.Date,
                    fdbck.UserId,
                    fdbck.User.Fullname
                });
                feedbacksList.Add(feedbackInfo);
            };

            List<Booking> bookings = tour.Bookings.Where(b=>b.BookedStart > DateTime.Now).ToList();
            List<string> bookingsList = new List<string>();
            foreach (Booking bkng in bookings)
            {
                string bookingInfo = String.Join("/", new
                {
                    bkng.BookedStart,
                    bkng.BookedEnd,
                });
                bookingsList.Add(bookingInfo);
            }


            var TourInfo = db.Tours.Where(t => t.Id == Id).Select(t => new
            {
                t.Id,
                FromCity = t.City.CityName,
                DestCity = t.City1.CityName,
                TourTitle,
                TourImages = timgurls,
                Guide = new
                {
                    t.User.Id,
                    t.User.Fullname,
                    t.User.Email,
                    t.User.Phone,
                    t.User.ProfileImage
                },
                FeedbacksList = feedbacksList,
                Desc = t.Description,
                Categories = t.Category,
                t.Duration,
                DurationType = t.DurationType.Type,
                t.Price,
                Currency = t.Currency.CurrencyName,
                t.Vehicle,
                Accomodation = t.Accomodation.AccomodationName,
                t.AccomodationLevel,
                BookingsList = bookingsList
            });

            return Json(TourInfo, JsonRequestBehavior.AllowGet);
        }




    }
}