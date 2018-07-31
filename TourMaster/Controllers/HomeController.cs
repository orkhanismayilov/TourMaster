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
                            CityId = signUpForm.CityId,
                            ProfileImage = "/public/images/profile_placeholder.png",
                            OverallRating = 0
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
            Session["LogInSuccess"] = null;
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

            List<Feedback> feedbacks = tour.Feedbacks.OrderByDescending(f => f.Id).Take(5).ToList();
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
                foreach (Tour tour in userRated.Tours.Where(t => t.Approved == 1))
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
                double b = totalUserRating / userRated.Tours.Where(t => t.Approved == 1).Count();
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

            Tour notiTour = db.Tours.Find(TourId);
            string TourTitle = "";
            if (notiTour.FromId == notiTour.DestinationId)
            {
                TourTitle = notiTour.City.CityName + " Tour";
            }
            else
            {
                TourTitle = notiTour.City.CityName + " - " + notiTour.City1.CityName + " Tour";
            }

            Notification noti = new Notification
            {
                UserId = userRated.Id,
                Text = "New feedback for " + TourTitle,
                Date = DateTime.Now,
                NotificationTypeId = 8,
                Link = "/manage/tours/details/" + TourId,
                Status = 0
            };
            db.Notifications.Add(noti);
            db.SaveChanges();


            return Json(feedbackModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadMoreFeedbacks(int FeedbackId, int FeedbacksCount)
        {
            Tour tour = db.Feedbacks.Find(FeedbackId).Tour;
            List<Feedback> feedbacks = tour.Feedbacks.OrderByDescending(f => f.Id).Skip(FeedbacksCount).Take(5).ToList();
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
                    Date = DateTime.Now,
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
            foreach (Tour tour in user.Tours.Where(t => t.Status == 1 && t.Approved == 1).OrderByDescending(t => t.PostedDate))
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

            int CompletedToursCount = 0;
            foreach (Tour allTours in user.Tours.Where(t => t.Approved == 1))
            {
                foreach (Booking booking in allTours.Bookings)
                {
                    if (booking.Status == 1)
                    {
                        CompletedToursCount++;
                    }
                }
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
                CompletedTours = CompletedToursCount,
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
                    Message = Msg,
                    Date = DateTime.Now
                };
                db.ContactRequests.Add(contact);
                db.SaveChanges();

                Notification noti = new Notification
                {
                    UserId = db.Users.FirstOrDefault(u => u.AccountType == 2).Id,
                    Text = "New contact request",
                    Link = "/admin/contacts/details/" + contact.Id,
                    Date = DateTime.Now,
                    Status = 0,
                    NotificationTypeId = 8
                };
                db.Notifications.Add(noti);
                db.SaveChanges();

                return Json(contact.Id, JsonRequestBehavior.AllowGet);
            }

            return Json("error", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BookingRequest(int TourId, int UserId, DateTime StartDate, DateTime EndDate, string Message)
        {
            if (!String.IsNullOrWhiteSpace(TourId.ToString()) && !String.IsNullOrWhiteSpace(UserId.ToString()) && !String.IsNullOrWhiteSpace(Message) && StartDate != null && EndDate != null)
            {
                BookingRequest bookingRequest = new BookingRequest
                {
                    TourId = TourId,
                    UserId = UserId,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    Message = Message,
                    Status = 0
                };
                db.BookingRequests.Add(bookingRequest);
                db.SaveChanges();

                Tour tour = db.Tours.Find(TourId);
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
                    Text = "New booking request for " + TourTitle,
                    Date = DateTime.Now,
                    NotificationTypeId = 8,
                    Link = "/manage/requests/index/" + tour.GuideId,
                    Status = 0
                };
                db.Notifications.Add(noti);
                db.SaveChanges();

                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult NotiSeen(int? Id)
        {
            User user = db.Users.Find(Id);
            foreach (Notification noti in user.Notifications.Where(n => n.Status == 0))
            {
                noti.Status = 1;
            }
            db.SaveChanges();
            Session["User"] = user;
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveSettings(int? Id, SignUpForm settings, string Facebook, string Instagram, string GooglePlus, string Twitter, HttpPostedFileBase profileImage)
        {
            User user = db.Users.Find(Id);
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    user.Fullname = settings.Name + " " + settings.Surname;
                    user.Email = settings.Email;
                    user.Birthday = settings.Birthday;
                    user.CityId = settings.CityId;
                    user.Phone = settings.Phone;
                    user.Facebook = Facebook;
                    user.Twitter = Twitter;
                    user.GooglePlus = GooglePlus;
                    user.Instagram = Instagram;
                }

                if (profileImage != null && profileImage.ContentLength > 0)
                {
                    string fileName = user.Id + DateTime.Now.ToString("yyyyMMddHHmss") + profileImage.FileName;
                    string path = System.IO.Path.Combine(Server.MapPath("~/uploads"), fileName);
                    profileImage.SaveAs(path);
                    string image = "/uploads/" + fileName;
                    string prevImage = user.ProfileImage;
                    if (prevImage.Contains("uploads"))
                    {
                        System.IO.File.Delete(Server.MapPath(prevImage));
                    }
                    user.ProfileImage = image;
                }

                db.SaveChanges();
                Session["User"] = user;
                Session["Settings"] = "ok";
            }

            return RedirectToAction("index");
        }

        [HttpPost]
        public JsonResult ChangePass(int? Id, string OldPass, string Pass, string PassConf)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.Find(Id);
                if (user != null && Pass == PassConf)
                {
                    if (Crypto.VerifyHashedPassword(user.Password, OldPass))
                    {
                        user.Password = Crypto.HashPassword(Pass);
                        db.SaveChanges();
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(0, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetChat(int? Id)
        {
            if (Session["User"] != null)
            {
                PrivateMessage thisMessage = db.PrivateMessages.Find(Id);
                User user = Session["User"] as User;
                User sender = null;
                if (thisMessage.User.Id == user.Id)
                {
                    sender = thisMessage.User1;
                }
                else
                {
                    sender = thisMessage.User;
                }

                List<PrivateMessage> pms = new List<PrivateMessage>();
                foreach (PrivateMessage pm in user.PrivateMessages.Where(pm => pm.Subject == thisMessage.Subject && (pm.SenderId == thisMessage.User.Id || pm.RecieverId == thisMessage.User.Id) && (pm.SenderId == thisMessage.User1.Id || pm.RecieverId == thisMessage.User1.Id)))
                {
                    pms.Add(pm);
                }

                foreach (PrivateMessage pm in user.PrivateMessages1.Where(pm => pm.Subject == thisMessage.Subject && (pm.SenderId == thisMessage.User.Id || pm.RecieverId == thisMessage.User.Id) && (pm.SenderId == thisMessage.User1.Id || pm.RecieverId == thisMessage.User1.Id)))
                {
                    pms.Add(pm);
                }

                List<MessageModel> messages = new List<MessageModel>();
                foreach (PrivateMessage pm in pms.OrderByDescending(pm => pm.Date))
                {
                    MessageModel message = new MessageModel
                    {
                        SenderId = pm.SenderId,
                        Message = pm.Message,
                        Date = pm.Date.ToString("HH:mm dd MMM yyyy"),
                        ReadStatus = pm.ReadStatus
                    };
                    messages.Add(message);
                    if (pm.ReadStatus == 0 && pm.User.Id != user.Id)
                    {
                        pm.ReadStatus = 1;
                        db.SaveChanges();
                    }
                }

                ChatModel chat = new ChatModel
                {
                    SenderId = sender.Id,
                    SenderFullname = sender.Fullname,
                    SenderImage = sender.ProfileImage,
                    Subject = thisMessage.Subject,
                    Messages = messages
                };

                return Json(chat, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Reply(PrivateMessage privateMessage)
        {
            if (privateMessage != null)
            {
                privateMessage.Date = DateTime.Now;
                privateMessage.ReadStatus = 0;
                db.PrivateMessages.Add(privateMessage);
                db.SaveChanges();
            }

            return Json(new { privateMessage.Message, Date = privateMessage.Date.ToString("HH:mm dd MMM yyyy") }, JsonRequestBehavior.AllowGet);
        }
    }
}