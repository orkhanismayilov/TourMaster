using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourMaster.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string ProfileImage { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public List<TourModel> ToursList { get; set; }
        public int FeedbacksCount { get; set; }
        public int BookingsCount { get; set; }
        public int Rating { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string GooglePlus { get; set; }
        public string Twitter { get; set; }
    }
}