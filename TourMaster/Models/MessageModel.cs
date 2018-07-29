using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourMaster.Models
{
    public class MessageModel
    {
        public int SenderId { get; set; }
        public string Message { get; set; }
        public string Date { get; set; }
        public int ReadStatus { get; set; }
    }
}