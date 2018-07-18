using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using TourMaster.Models;

namespace TourMaster.Areas.Manage.Controllers
{
    public class LoginController : Controller
    {
        TourMasterEntities db = new TourMasterEntities();

        // GET: Manage/Login
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            if (!String.IsNullOrWhiteSpace(Email) && !String.IsNullOrWhiteSpace(Password))
            {
                User guide = db.Users.Where(u => u.AccountType == 1).FirstOrDefault(u => u.Email == Email);
                if (guide != null)
                {
                    if (Crypto.VerifyHashedPassword(guide.Password, Password))
                    {
                        Session["LogInSuccess"] = true;
                        Session["User"] = guide;

                        return RedirectToAction("index", new { controller = "home", area = "manage" });
                    }
                    else
                    {
                        Session["LogInError"] = true;
                        return RedirectToAction("index", new { controller = "login", area = "manage" });
                    }
                }
                else
                {
                    Session["LogInError"] = true;
                    return RedirectToAction("index", new { controller = "login", area = "manage" });
                }
            }
            else
            {
                Session["LogInError"] = true;
                return RedirectToAction("index", new { controller = "login", area = "manage" });
            }
        }
    }
}