using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNeighbors.Models
{
    public class PlayViewModel
    {
        public double lat { get; set; }
        public double lon { get; set; }

        public UserInformation currentUser { get; set; }
        //public List<UserInformation> userInfo { get; set; }
        public List<ApplicationUser> nearbyUsers { get; set; }

        public PlayViewModel()
        {
            lat = 0;
            lon = 0;
            currentUser = new UserInformation();
            //userInfo = new List<UserInformation>();
            nearbyUsers = new List<ApplicationUser>();
        }
    }

    public class UserInformation
    {
        public UserDetails userDetails { get; set; }
        public ApplicationUser user { get; set; }

        public UserInformation()
        {
            userDetails = new UserDetails();
            user = new ApplicationUser();
        }
    }
}