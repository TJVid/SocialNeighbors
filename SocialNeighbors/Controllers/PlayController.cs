using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.DistanceMatrix.Request;
using GoogleMapsApi.Entities.PlacesNearBy.Request;
using GoogleMapsApi.Entities.PlacesNearBy.Response;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using Microsoft.Ajax.Utilities;
using SocialNeighbors.Models;
using System.Device.Location;

namespace SocialNeighbors.Controllers
{
    [Authorize]
    public class PlayController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUser currentUser;


        // GET: Play
        public ActionResult Index()
        {
            currentUser = db.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault();


            var playViewModel = new PlayViewModel();

            //Load Current User Details
            var x = db.Users.Where(u => u.Email == User.Identity.Name);
            playViewModel.currentUser.user = currentUser;
            playViewModel.currentUser.userDetails = db.UserDetails.FirstOrDefault(u => u.userID == User.Identity.Name);

            //Load the Nearby Users
            var userLocation = new Location(double.Parse(currentUser.Lat), double.Parse(currentUser.Lon));
            var neighbors = GetNeighbors(userLocation);
            playViewModel.nearbyUsers = neighbors;

            return View(playViewModel);
        }

        public List<ApplicationUser> GetNeighbors(Location currentLocation)
        {

            //var closest = db.Users.Select(u => new Location(double.Parse(u.Lat), double.Parse(u.Lon)))
            //          .GroupBy(x => Math.Pow((currentLocation.Latitude - x.Latitude), 2) + Math.Pow((currentLocation.Longitude - x.Longitude), 2))
            //          .OrderBy(x => x.Key)
            //          .Take(10); 

            //var closestNeighbors = db.Users
            //    .Select(x => 
            //    {
            //        new GeoCoordinate(double.Parse(x.Lat), double.Parse(x.Lon));
            //        x => x.Longitude;
            //    });

            var userCoordinates = new GeoCoordinate(currentLocation.Latitude, currentLocation.Longitude);
            currentUser = db.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault();


            var closestNeighbors = db.Users
                .Where(u => u.UserName != currentUser.Email)
                            .AsEnumerable()
                            .OrderBy(x => new GeoCoordinate(double.Parse(x.Lat), double.Parse(x.Lon)).GetDistanceTo(userCoordinates))
                            .Take(5)
                         ;

            return closestNeighbors.ToList();
        }

    }
}