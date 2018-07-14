using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourMaster.Models
{
    public class FeedbackModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public string Date { get; set; }
        public int UserId { get; set; }
        public string UserFullname { get; set; }
        public string UserProfileImage { get; set; }
    }
}