using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourMaster.Models
{
    public class SignUpForm
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public DateTime Birthday { get; set; }
        public string Phone { get; set; }
        public int CityId { get; set; }
        public int AccountTypeId { get; set; }
    }
}