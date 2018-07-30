using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TourMaster.Models;
using TourMaster.Filters;

namespace TourMaster.Areas.Admin.Controllers
{
    [AdminAuth]
    public class UsersController : Controller
    {
        TourMasterEntities db = new TourMasterEntities();

        // GET: Admin/Users
        public ActionResult Index()
        {
            List<User> users = db.Users.ToList();
            return View(users);
        }
    }
}