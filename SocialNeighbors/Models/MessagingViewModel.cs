using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNeighbors.Models
{
    public class MessagingViewModel
    {
        public ApplicationUser neighbor { get; set; }
        public ApplicationUser me { get; set; }

        public List<Message> messages { get; set; }
        public Message sentMessage { get; set; }
    }
}