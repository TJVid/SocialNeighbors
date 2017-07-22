using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialNeighbors.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string fromUser { get; set; }
        public string toUser { get; set; }
        [Display(Name ="Message")]
        public string messageContent { get; set; }
        public DateTime timeStamp { get; set; }
        public bool isRead { get; set; }
    }
}