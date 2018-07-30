using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TourMaster.Models;
using TourMaster.Filters;

namespace TourMaster.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        TourMasterEntities db = new TourMasterEntities();

        // GET: Admin/Home
        [AdminAuth]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            if (!String.IsNullOrWhiteSpace(Email) && !String.IsNullOrWhiteSpace(Password))
            {
                User admin = db.Users.Where(u => u.AccountType == 2).FirstOrDefault(u => u.Email == Email);
                if (admin != null)
                {
                    if (System.Web.Helpers.Crypto.VerifyHashedPassword(admin.Password, Password))
                    {
                        Session["LogInSuccess"] = true;
                        Session["LogInError"] = null;
                        Session["User"] = admin;

                        return RedirectToAction("index", new { controller = "home", area = "admin" });
                    }
                    else
                    {
                        Session["LogInError"] = true;
                        return RedirectToAction("login", new { controller = "home", area = "admin", error = 1 });
                    }
                }
                else
                {
                    Session["LogInError"] = true;
                    return RedirectToAction("login", new { controller = "home", area = "admin", error = 1 });
                }
            }
            else
            {
                Session["LogInError"] = true;
                return RedirectToAction("login", new { controller = "home", area = "admin", error = 1 });
            }
        }

        public ActionResult Logout()
        {
            Session["User"] = null;

            return RedirectToAction("login", new { controller = "home", area = "admin" });
        }
    }
}