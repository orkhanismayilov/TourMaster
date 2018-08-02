using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TourMaster.Models;

namespace TourMaster.Areas.Admin.Models
{
    public class DashboardModel
    {
        public List<User> Users { get; set; }
    }
}