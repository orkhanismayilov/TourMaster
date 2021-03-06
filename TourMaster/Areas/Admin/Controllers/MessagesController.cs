﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TourMaster.Models;
using TourMaster.Filters;

namespace TourMaster.Areas.Admin.Controllers
{
    [AdminAuth]
    public class MessagesController : Controller
    {
        TourMasterEntities db = new TourMasterEntities();

        // GET: Admin/Messages
        public ActionResult Index(int? Id)
        {
            User user = db.Users.Find(Id);

            return View(user);
        }

        [HttpGet]
        public ActionResult Details(int? Id)
        {
            User user = Session["User"] as User;
            PrivateMessage pm = db.PrivateMessages.Find(Id);
            List<PrivateMessage> messages = db.PrivateMessages.Where(m => m.Subject == pm.Subject && (m.SenderId == pm.User.Id || m.RecieverId == pm.User.Id) && (m.SenderId == pm.User1.Id || m.RecieverId == pm.User1.Id)).ToList();
            foreach (PrivateMessage message in messages)
            {
                if (message.ReadStatus == 0 && message.User.Id != user.Id)
                {
                    message.ReadStatus = 1;
                    db.SaveChanges();
                }
            }
            Session["User"] = db.Users.Find(user.Id);

            return View(messages);
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