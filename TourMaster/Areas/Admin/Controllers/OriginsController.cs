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
    public class OriginsController : Controller
    {
        TourMasterEntities db = new TourMasterEntities();

        // GET: Admin/Origins
        public ActionResult Index()
        {
            Origin origin = db.Origins.FirstOrDefault();

            return View(origin);
        }
    }
}