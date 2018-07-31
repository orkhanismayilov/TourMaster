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

        [HttpGet]
        public ActionResult MainSlider()
        {
            List<MainSliderImage> images = db.Origins.FirstOrDefault().MainSliderImages.ToList();
            return View(images);
        }

        [HttpPost]
        public ActionResult Upload(List<HttpPostedFileBase> images)
        {
            if (images.Count > 0)
            {
                foreach (HttpPostedFileBase img in images)
                {
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + img.FileName;
                    string path = System.IO.Path.Combine(Server.MapPath("~/public/images/mainslider"), fileName);
                    img.SaveAs(path);
                    string url = "/public/images/mainslider/" + fileName;
                    MainSliderImage image = new MainSliderImage
                    {
                        ImageURL = url,
                        OriginId = 1
                    };
                    db.MainSliderImages.Add(image);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("mainslider");
        }

        [HttpPost]
        public JsonResult DeleteImage(int? Id)
        {
            MainSliderImage image = db.MainSliderImages.Find(Id);
            if (db.MainSliderImages.Count() == 1)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            string path = Server.MapPath(image.ImageURL);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            db.MainSliderImages.Remove(image);
            db.SaveChanges();

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveChanges(HttpPostedFileBase logo, Origin origin) {
            if (ModelState.IsValid)
            {
                if (logo != null)
                {
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + logo.FileName;
                    string path = System.IO.Path.Combine(Server.MapPath("~/public/images/logo"), fileName);
                    logo.SaveAs(path);
                    string logoURL = "/pulic/images/logo/" + fileName;
                    origin.Logo = logoURL;
                }
                Origin info = db.Origins.FirstOrDefault();
                info = origin;
                db.SaveChanges();
            }

            return RedirectToAction("index");
        }
    }
}