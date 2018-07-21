using System;
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
                Tours = db.Tours.Where(t => t.Status == 1).ToList(),
                Categories = db.Categories.ToList(),
                MostPopular = db.Tours.Where(t => t.Status == 1 && t.Approved == 1).OrderByDescending(t => t.Bookings.Count).Take(3).ToList(),
                BestGuides = db.Users.Where(u => u.AccountType == 1).OrderByDescending(u => u.OverallRating).Take(3).ToList()
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

        [HttpPost]
        public ActionResult LogIn(LogInForm logInForm)
        {
            if (!String.IsNullOrWhiteSpace(logInForm.Email) && !String.IsNullOrWhiteSpace(logInForm.Password) && !String.IsNullOrWhiteSpace(logInForm.AccountTypeId.ToString()))
            {
                if (logInForm.AccountTypeId == 0)
                {
                    User traveler = db.Users.Where(u => u.AccountType == 0).FirstOrDefault(u => u.Email == logInForm.Email);
                    if (traveler != null)
                    {
                        if (Crypto.VerifyHashedPassword(traveler.Password, logInForm.Password))
                        {
                            Session["LogInSuccess"] = true;
                            Session["User"] = traveler;
                        }
                        else
                        {
                            Session["LogInError"] = true;
                            return RedirectToAction("index");
                        }
                    }
                    else
                    {
                        Session["LogInError"] = true;
                        return RedirectToAction("index");
                    }
                }

                if (logInForm.AccountTypeId == 1)
                {
                    User guide = db.Users.Where(u => u.AccountType == 1).FirstOrDefault(u => u.Email == logInForm.Email);
                    if (guide != null)
                    {
                        if (Crypto.VerifyHashedPassword(guide.Password, logInForm.Password))
                        {
                            Session["LogInSuccess"] = true;
                            Session["User"] = guide;
                        }
                        else
                        {
                            Session["LogInError"] = true;
                            return RedirectToAction("index");
                        }
                    }
                    else
                    {
                        Session["LogInError"] = true;
                        return RedirectToAction("index");
                    }
                }
            }
            return RedirectToAction("index");
        }

        public ActionResult LogOut()
        {
            Session["User"] = null;

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

            string TourTitle = "";
            if (tour.FromId == tour.DestinationId)
            {
                TourTitle = tour.City.CityName + " Tour";
            }
            else
            {
                TourTitle = tour.City.CityName + " - " + tour.City1.CityName + " Tour";
            }

            List<TourImage> timg = tour.TourImages.ToList();
            List<string> TourImagesUrl = new List<string>();
            foreach (TourImage img in timg)
            {
                TourImagesUrl.Add(img.ImageURL);
            }

            UserModel Guide = new UserModel
            {
                Id = tour.User.Id,
                Fullname = tour.User.Fullname,
                Email = tour.User.Email,
                Phone = tour.User.Phone,
                ProfileImage = tour.User.ProfileImage,
                Rating = tour.User.OverallRating
            };

            List<Feedback> feedbacks = tour.Feedbacks.OrderByDescending(f => f.Date).Take(5).ToList();
            List<FeedbackModel> feedbacksList = new List<FeedbackModel>();
            foreach (Feedback fdbck in feedbacks)
            {
                string date = fdbck.Date.ToString("HH:mm dd MMM yyyy");
                FeedbackModel feedback = new FeedbackModel
                {
                    Id = fdbck.Id,
                    Text = fdbck.Text,
                    Rating = fdbck.Rating,
                    Date = date,
                    UserId = fdbck.UserId,
                    UserFullname = fdbck.User.Fullname,
                    UserProfileImage = fdbck.User.ProfileImage
                };
                feedbacksList.Add(feedback);
            }

            string Price = tour.Price.ToString("#,##");

            TourInfoModel TourInfo = new TourInfoModel
            {
                Id = tour.Id,
                FromCity = tour.City.CityName,
                DestCity = tour.City1.CityName,
                TourTitle = TourTitle,
                TourImagesUrl = TourImagesUrl,
                Guide = Guide,
                FeedbacksList = feedbacksList,
                Desc = tour.Description,
                Categories = tour.Category,
                Duration = tour.Duration,
                DurationType = tour.DurationType.Type,
                Price = Price,
                Currency = tour.Currency.CurrencyName,
                Vehicle = tour.Vehicle,
                Accomodation = tour.Accomodation?.AccomodationName,
                AccomodationLvl = tour.AccomodationLevel?.Level
            };

            return Json(TourInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SubmitFeedback(int TourId, int UserId, string Text, int Rating)
        {
            Feedback feedback = new Feedback
            {
                TourId = TourId,
                UserId = UserId,
                Text = Text,
                Rating = Rating,
                Date = DateTime.Now
            };
            db.Feedbacks.Add(feedback);
            db.SaveChanges();

            User user = db.Users.Find(UserId);
            User userRated = db.Tours.Find(TourId).User;

            int rating = 0;
            int totalUserRating = 0;
            if (userRated.Tours.Count != 0)
            {
                foreach (Tour tour in userRated.Tours)
                {
                    if (tour.Feedbacks.Count != 0)
                    {
                        int totalTourRating = 0;
                        int overallTourRating = 0;
                        foreach (Feedback fdbck in tour.Feedbacks)
                        {
                            totalTourRating += fdbck.Rating;
                        }
                        double a = totalTourRating / tour.Feedbacks.Count;
                        overallTourRating = (int)Math.Ceiling(a);
                        totalUserRating += overallTourRating;
                    }
                }
                double b = totalUserRating / userRated.Tours.Count;
                rating = (int)Math.Ceiling(b);
            }

            userRated.OverallRating = rating;
            db.SaveChanges();

            FeedbackModel feedbackModel = new FeedbackModel
            {
                Id = feedback.Id,
                Text = feedback.Text,
                Rating = feedback.Rating,
                Date = feedback.Date.ToString("HH:mm dd MMM yyyy"),
                UserId = feedback.UserId,
                UserFullname = user.Fullname,
                UserProfileImage = user.ProfileImage
            };

            return Json(feedbackModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadMoreFeedbacks(int FeedbackId, int FeedbacksCount)
        {
            Tour tour = db.Feedbacks.Find(FeedbackId).Tour;
            List<Feedback> feedbacks = tour.Feedbacks.OrderByDescending(f => f.Date).Skip(FeedbacksCount).Take(5).ToList();
            List<FeedbackModel> feedbacksList = new List<FeedbackModel>();
            foreach (Feedback item in feedbacks)
            {
                FeedbackModel feedbackModel = new FeedbackModel
                {
                    Id = item.Id,
                    Text = item.Text,
                    Rating = item.Rating,
                    Date = item.Date.ToString("HH:mm dd MMM yyyy"),
                    UserId = item.UserId,
                    UserFullname = item.User.Fullname,
                    UserProfileImage = item.User.ProfileImage
                };
                feedbacksList.Add(feedbackModel);
            }

            return Json(feedbacksList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendPrivateMessage(int SenderId, int GuideId, string Subject, string Msg)
        {
            if (!String.IsNullOrWhiteSpace(SenderId.ToString()) && !String.IsNullOrWhiteSpace(GuideId.ToString()) && !String.IsNullOrWhiteSpace(Subject) && !String.IsNullOrWhiteSpace(Msg))
            {
                PrivateMessage pm = new PrivateMessage
                {
                    SenderId = SenderId,
                    RecieverId = GuideId,
                    Subject = Subject,
                    Message = Msg,
                    ReadStatus = 0
                };
                db.PrivateMessages.Add(pm);
                db.SaveChanges();

                bool sent = true;
                return Json(sent, JsonRequestBehavior.AllowGet);
            }
            else
            {
                bool sent = false;
                return Json(sent, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetUserInfo(int Id)
        {
            User user = db.Users.Find(Id);

            List<TourModel> toursList = new List<TourModel>();
            foreach (Tour tour in user.Tours.Where(t => t.Status == 1).OrderByDescending(t => t.PostedDate))
            {
                string title = "";
                if (tour.FromId == tour.DestinationId)
                {
                    title = tour.City.CityName;
                }
                else
                {
                    title = tour.City.CityName + " - " + tour.City1.CityName;
                }

                TourModel tourModel = new TourModel
                {
                    Id = tour.Id,
                    TourTitle = title,
                    TourImage = tour.TourImage.ImageURL,
                    Duration = tour.Duration,
                    DurationType = tour.DurationType.Type,
                    Price = tour.Price.ToString("#.##"),
                    Currency = tour.Currency.CurrencyName
                };
                toursList.Add(tourModel);
            }

            UserModel userModel = new UserModel
            {
                Id = user.Id,
                Fullname = user.Fullname,
                ProfileImage = user.ProfileImage,
                Country = user.City.Country.CountryName,
                CountryCode = user.City.Country.CountryCode,
                ToursList = toursList,
                FeedbacksCount = user.Tours.Sum(t => t.Feedbacks.Count),
                BookingsCount = user.Bookings.Where(b => b.Status == 1 && b.BookedEnd < DateTime.Now).Count(),
                Rating = user.OverallRating,
                Phone = user.Phone,
                Email = user.Email,
                Facebook = user.Facebook,
                Instagram = user.Instagram,
                GooglePlus = user.GooglePlus,
                Twitter = user.Twitter
            };


            return Json(userModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ContactFormSubmit(string Fullname, string Email, string Phone, string Subject, string Msg)
        {
            if (!String.IsNullOrWhiteSpace(Fullname) && !String.IsNullOrWhiteSpace(Email) && !String.IsNullOrWhiteSpace(Subject) && !String.IsNullOrWhiteSpace(Msg))
            {
                ContactRequest contact = new ContactRequest
                {
                    Fullname = Fullname,
                    Email = Email,
                    Phone = Phone,
                    Subject = Subject,
                    Message = Msg
                };
                db.ContactRequests.Add(contact);
                db.SaveChanges();

                return Json(contact.Id, JsonRequestBehavior.AllowGet);
            }

            return Json("error", JsonRequestBehavior.AllowGet);
        }


    }
}