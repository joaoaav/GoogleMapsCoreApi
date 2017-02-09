using GoogleMapsCoreApi;
using GoogleMapsCoreApi.Entities.Common;
using GoogleMapsCoreApi.Entities.Directions.Request;
using GoogleMapsCoreApi.Entities.Directions.Response;
using GoogleMapsCoreApi.Entities.DistanceMatrix.Request;
using GoogleMapsCoreApi.Entities.DistanceMatrix.Response;
using GoogleMapsCoreApi.Entities.Elevation.Request;
using GoogleMapsCoreApi.Entities.Geocoding.Request;
using GoogleMapsCoreApi.Entities.Geocoding.Response;
using GoogleMapsCoreApi.StaticMaps;
using GoogleMapsCoreApi.StaticMaps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace GoogleMapsCoreApiTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Driving directions
            var drivingDirectionRequest = new DirectionsRequest
            {
                Origin = "NYC, 5th and 39",
                Destination = "Philladephia, Chesnut and Wallnut"
            };

            DirectionsResponse drivingDirections = GoogleMaps.Directions.Query(drivingDirectionRequest);
            PrintDirections(drivingDirections);

            // Transit directions
            var transitDirectionRequest = new DirectionsRequest
            {
                Origin = "New York",
                Destination = "Queens",
                TravelMode = TravelMode.Transit,
                DepartureTime = DateTime.Now
            };

            DirectionsResponse transitDirections = GoogleMaps.Directions.Query(transitDirectionRequest);
            PrintDirections(transitDirections);

            // Distance 
            // The Distance API requires a valid ApiKey, otherwise it will return a invalid request
            var distanceRequest = new DistanceMatrixRequest
            {
                //ApiKey = "",
                Destinations = new[] { $"{"59.331045"},{"18.057507"}" },
                Origins = new[] { $"{"59.333962"},{"18.064685"}" },
                Alternatives = true,
                Mode = DistanceMatrixTravelModes.transit
            };

            var resultDistance = GoogleMaps.DistanceMatrix.Query(distanceRequest);

            var duration = GetDuration(resultDistance);

            Console.WriteLine("Duration: " + duration);

            var dep_time = DateTime.Today
                            .AddDays(1)
                            .AddHours(13);

            var request = new DirectionsRequest
            {
                Origin = "T-centralen, Stockholm, Sverige",
                Destination = "Kungsträdgården, Stockholm, Sverige",
                TravelMode = TravelMode.Transit,
                DepartureTime = dep_time,
                Language = "sv"
            };

            DirectionsResponse result = GoogleMaps.Directions.Query(request);
            PrintDirections(result);

            // Geocode
            //https://maps.googleapis.com/maps/api/geocode/json?address=Parque+Marechal+Mascarenhas+de+Morais&components=locality:Porto%20Aelgre|administrative_area:RS|country:BR
            var geocodeRequest = new GeocodingRequest
            {
                Address = "Parque Marechal Mascarenhas de Morais",
                Components = new GeocodingComponents()
                {
                    Locality = "Porto Alegre",
                    AdministrativeArea = "RS",
                    Country = "BR"
                }

            };

            GeocodingResponse geocode = GoogleMaps.Geocode.Query(geocodeRequest);
            Console.WriteLine(geocode);

            // Static maps API - get static map of with the path of the directions request
            var staticMapGenerator = new StaticMapsEngine();

            //Path from previous directions request
            IEnumerable<Step> steps = drivingDirections.Routes.First().Legs.First().Steps;
            // All start locations
            IList<ILocationString> path = steps.Select(step => step.StartLocation).ToList<ILocationString>();
            // also the end location of the last step
            path.Add(steps.Last().EndLocation);

            string url = staticMapGenerator.GenerateStaticMapURL(new StaticMapRequest(new Location(40.38742, -74.55366), 9, new ImageSize(800, 400))
            {
                Pathes = new List<Path> { new Path
                    {
                        Style = new PathStyle
                        {
                            Color = "red"
                        },
                        Locations = path
                    }}
            });

            Console.WriteLine("Map with path: " + url);

            // Async! (Elevation)
            var elevationRequest = new ElevationRequest
            {
                Locations = new[] { new Location(54, 78) },
            };

            var task = GoogleMaps.Elevation.QueryAsync(elevationRequest)
                .ContinueWith(t => Console.WriteLine("\n" + t.Result));

            Console.Write("Asynchronous query sent, waiting for a reply..");

            while (!task.IsCompleted)
            {
                Console.Write('.');
                Thread.Sleep(1000);
            }

            Console.WriteLine("Finished! Press any key to exit...");
            Console.ReadKey();
        }

        private static TimeSpan? GetDuration(DistanceMatrixResponse result)
        {
            var row = result.Rows.FirstOrDefault();

            var element = row?.Elements.FirstOrDefault();

            var duration = element?.Duration;

            return duration?.Value;
        }

        private static void PrintDirections(DirectionsResponse directions)
        {
            Route route = directions.Routes.First();
            Leg leg = route.Legs.First();

            foreach (Step step in leg.Steps)
            {
                Console.WriteLine(StripHTML(step.HtmlInstructions));

                var localIcon = step.TransitDetails?.Lines?.Vehicle?.LocalIcon;
                if (localIcon != null)
                    Console.WriteLine("Local sign: " + localIcon);
            }

            Console.WriteLine();
        }

        private static string StripHTML(string html)
        {
            return Regex.Replace(html, @"<(.|\n)*?>", string.Empty);
        }
    }
}
