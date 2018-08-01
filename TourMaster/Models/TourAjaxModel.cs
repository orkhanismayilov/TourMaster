using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourMaster.Models
{
    public class TourAjaxModel
    {
        public int Id { get; set; }
        public string TourTitle { get; set; }
        public string TourImage { get; set; }
        public int Duration { get; set; }
        public string DurationType { get; set; }
        public string Price { get; set; }
        public string Currency { get; set; }
        public string Categories { get; set; }
        public string GuideProfileImage { get; set; }
        public string GuideFullname { get; set; }
        public int GuideRating { get; set; }
    }
}