using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourMaster.Models
{
    public class TourCreateEditModel
    {
        public int GuideId { get; set; }
        public int FromCity { get; set; }
        public int DestCity { get; set; }
        public double Price { get; set; }
        public int Currency { get; set; }
        public int Duration { get; set; }
        public int DurationType { get; set; }
        public int[] Categories { get; set; }
        public string Transport { get; set; }
        public int? Accomodation { get; set; }
        public int? AccomodationLvl { get; set; }
        public string Description { get; set; }
        public List<HttpPostedFileBase> Images { get; set; }
    }
}