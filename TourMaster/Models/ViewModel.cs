using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourMaster.Models
{
    public class ViewModel
    {
        public List<User> Users { get; set; }
        public List<Tour> Tours { get; set; }
        public List<Category> Categories { get; set; }
    }
}