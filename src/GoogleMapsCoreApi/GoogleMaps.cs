using GoogleMapsCoreApi.Entities.Directions.Request;
using GoogleMapsCoreApi.Entities.Directions.Response;
using GoogleMapsCoreApi.Entities.DistanceMatrix.Request;
using GoogleMapsCoreApi.Entities.DistanceMatrix.Response;
using GoogleMapsCoreApi.Entities.Elevation.Request;
using GoogleMapsCoreApi.Entities.Elevation.Response;
using GoogleMapsCoreApi.Entities.Geocoding.Request;
using GoogleMapsCoreApi.Entities.Geocoding.Response;
using GoogleMapsCoreApi.Entities.PlaceAutocomplete.Request;
using GoogleMapsCoreApi.Entities.PlaceAutocomplete.Response;
using GoogleMapsCoreApi.Entities.Places.Request;
using GoogleMapsCoreApi.Entities.Places.Response;
using GoogleMapsCoreApi.Entities.PlacesDetails.Request;
using GoogleMapsCoreApi.Entities.PlacesDetails.Response;
using GoogleMapsCoreApi.Entities.PlacesNearBy.Request;
using GoogleMapsCoreApi.Entities.PlacesNearBy.Response;
using GoogleMapsCoreApi.Entities.PlacesRadar.Request;
using GoogleMapsCoreApi.Entities.PlacesRadar.Response;
using GoogleMapsCoreApi.Entities.PlacesText.Request;
using GoogleMapsCoreApi.Entities.PlacesText.Response;
using GoogleMapsCoreApi.Entities.TimeZone.Request;
using GoogleMapsCoreApi.Entities.TimeZone.Response;

namespace GoogleMapsCoreApi
{
    public class GoogleMaps
    {
        /// <summary>Perform geocoding operations.</summary>
        public static IEngineFacade<GeocodingRequest, GeocodingResponse> Geocode
        {
            get
            {
                return EngineFacade<GeocodingRequest, GeocodingResponse>.Instance;
            }
        }
        /// <summary>Perform directions operations.</summary>
        public static IEngineFacade<DirectionsRequest, DirectionsResponse> Directions
        {
            get
            {
                return EngineFacade<DirectionsRequest, DirectionsResponse>.Instance;
            }
        }
        /// <summary>Perform elevation operations.</summary>
        public static IEngineFacade<ElevationRequest, ElevationResponse> Elevation
        {
            get
            {
                return EngineFacade<ElevationRequest, ElevationResponse>.Instance;
            }
        }

        /// <summary>Perform places operations.</summary>
        public static IEngineFacade<PlacesRequest, PlacesResponse> Places
        {
            get
            {
                return EngineFacade<PlacesRequest, PlacesResponse>.Instance;
            }
        }

        /// <summary>Perform places text search operations.</summary>
        public static IEngineFacade<PlacesTextRequest, PlacesTextResponse> PlacesText
        {
            get
            {
                return EngineFacade<PlacesTextRequest, PlacesTextResponse>.Instance;
            }
        }

        /// <summary>Perform places radar search operations.</summary>
        public static IEngineFacade<PlacesRadarRequest, PlacesRadarResponse> PlacesRadar
        {
            get
            {
                return EngineFacade<PlacesRadarRequest, PlacesRadarResponse>.Instance;
            }
        }

        /// <summary>Perform places text search operations.</summary>
        public static IEngineFacade<TimeZoneRequest, TimeZoneResponse> TimeZone
        {
            get
            {
                return EngineFacade<TimeZoneRequest, TimeZoneResponse>.Instance;
            }
        }

        /// <summary>Perform places details  operations.</summary>
        public static IEngineFacade<PlacesDetailsRequest, PlacesDetailsResponse> PlacesDetails
        {
            get
            {
                return EngineFacade<PlacesDetailsRequest, PlacesDetailsResponse>.Instance;
            }
        }

        /// <summary>Perform place autocomplete operations.</summary>
        public static IEngineFacade<PlaceAutocompleteRequest, PlaceAutocompleteResponse> PlaceAutocomplete
        {
            get
            {
                return EngineFacade<PlaceAutocompleteRequest, PlaceAutocompleteResponse>.Instance;
            }
        }

        /// <summary>Perform near by places operations.</summary>
        public static IEngineFacade<PlacesNearByRequest, PlacesNearByResponse> PlacesNearBy
        {
            get
            {
                return EngineFacade<PlacesNearByRequest, PlacesNearByResponse>.Instance;
            }
        }
        /// <summary>Retrieve duration and distance values based on the recommended route between start and end points.</summary>
        public static IEngineFacade<DistanceMatrixRequest, DistanceMatrixResponse> DistanceMatrix
        {
            get
            {
                return EngineFacade<DistanceMatrixRequest, DistanceMatrixResponse>.Instance;
            }
        }

    }
}
