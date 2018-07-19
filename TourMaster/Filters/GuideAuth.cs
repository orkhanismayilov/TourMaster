using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;

namespace TourMaster.Filters
{
    public class GuideAuth : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext httpContext = HttpContext.Current;
            if (httpContext.Session["User"] == null)
            {
                filterContext.Result = new RedirectResult("~/manage/login");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}