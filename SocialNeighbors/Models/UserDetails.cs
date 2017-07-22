using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GoogleMapsApi.Entities.Common;

namespace SocialNeighbors.Models
{
    public class UserDetails
    {
        public int Id { get; set; }
        public string userID { get; set; }
        public double latestLat { get; set; }
        public double latestLong { get; set; }
        //public Location latestLoc { get; set; }

        public DateTime latestTime { get; set; }

        public UserDetails()
        {
            userID = "";
            latestLat = 0;
            latestLong = 0;
            latestTime = DateTime.Now;
        }
    }

  
}