using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourMaster.Models
{
    public class TourInfoModel
    {
        public int Id { get; set; }
        public string FromCity { get; set; }
        public string DestCity { get; set; }
        public string TourTitle { get; set; }
        public List<string> TourImagesUrl { get; set; }
        public UserModel Guide { get; set; }
        public List<FeedbackModel> FeedbacksList { get; set; }
        public string Desc { get; set; }
        public string Categories { get; set; }
        public int Duration { get; set; }
        public string DurationType { get; set; }
        public string Price { get; set; }
        public string Currency { get; set; }
        public string Vehicle { get; set; }
        public string Accomodation { get; set; }
        public string AccomodationLvl { get; set; }
    }
}