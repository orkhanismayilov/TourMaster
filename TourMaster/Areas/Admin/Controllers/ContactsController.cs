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
    public class ContactsController : Controller
    {
        TourMasterEntities db = new TourMasterEntities();

        // GET: Admin/Contacts
        public ActionResult Index()
        {
            List<ContactRequest> requests = db.ContactRequests.ToList();

            return View(requests);
        }
    }
}