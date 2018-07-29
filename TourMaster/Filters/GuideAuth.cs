using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using TourMaster.Models;

namespace TourMaster.Filters
{
    public class GuideAuth : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext httpContext = HttpContext.Current;
            User user = httpContext.Session["User"] as User;
            if (user == null || user.AccountType != 1)
            {
                filterContext.Result = new RedirectResult("~/manage/login");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}