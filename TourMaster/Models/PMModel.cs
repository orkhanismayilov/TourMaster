using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TourMaster.Models;

namespace TourMaster.Models
{
    public class PMModel
    {
        public string Subject { get; set; }
        public List<PrivateMessage> privateMessages { get; set; }
    }
}