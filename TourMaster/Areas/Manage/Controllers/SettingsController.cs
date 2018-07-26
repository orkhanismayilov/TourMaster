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
    public class SettingsController : Controller
    {
        TourMasterEntities db = new TourMasterEntities();

        // GET: Manage/Settings
        public ActionResult Index(int? Id)
        {
            User user = db.Users.Find(Id);
            ViewBag.Countries = db.Countries.ToList();

            return View(user);
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
            }

            return RedirectToAction("index", new { user.Id });
        }
    }
}