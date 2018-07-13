using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourMaster.Models
{
    public class LogInForm
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int AccountTypeId { get; set; }
    }
}