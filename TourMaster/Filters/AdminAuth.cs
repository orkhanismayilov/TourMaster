using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using TourMaster.Models;

namespace TourMaster.Filters
{
    public class AdminAuth : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext httpContext = HttpContext.Current;
            User user = httpContext.Session["User"] as User;
            if (user == null || user.AccountType != 2)
            {
                filterContext.Result = new RedirectResult("~/admin/home/login");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}