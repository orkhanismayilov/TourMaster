using System.Web.Mvc;

namespace TourMaster.Areas.Manage
{
    public class ManageAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Manage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Manage_default",
                "manage/{controller}/{action}/{id}",
                new { controller = "home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}