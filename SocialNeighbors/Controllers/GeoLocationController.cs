using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi;
using System.Net;


namespace SocialNeighbors.Controllers
{
    public class GeoLocationController : Controller
    {

        private string myApiKey = "AIzaSyCxGw3Xlbycstt7WWQYhSiW6X34q7QvXsE";

        // GET: GeoLocation
        public ActionResult Index()
        {
            return View();
        }


        public Location GetLocation(string address)
        {
            var loc = new Location(0, 0);

            GeocodingRequest geocodingRequest = new GeocodingRequest();
            geocodingRequest.ApiKey = myApiKey;
            geocodingRequest.Address = address;

            GeocodingResponse responseGeoCode = GoogleMaps.Geocode.Query(geocodingRequest);

            foreach (var geoCode in responseGeoCode.Results)
            {
                loc = geoCode.Geometry.Location;
                break;
            }

            return loc;
        }

        [HttpGet]
        public ActionResult GetLocationCoordinates(string address)
        {
            var loc = GetLocation(address);
            if (loc.Latitude != 0 && loc.Longitude != 0)
            {
                return Json(loc, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
}