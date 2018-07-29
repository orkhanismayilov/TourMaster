using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TourMaster.Models;

namespace TourMaster.Models
{
    public class ChatModel
    {
        public int SenderId { get; set; }
        public string SenderFullname { get; set; }
        public string SenderImage { get; set; }
        public string Subject { get; set; }
        public List<MessageModel> Messages { get; set; }
    }
}