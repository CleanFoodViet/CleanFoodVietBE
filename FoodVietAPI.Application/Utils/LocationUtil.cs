using CleanFoodVietAPI.Application.DTOs;
using GeoCoordinatePortable;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace CleanFoodVietAPI.Application.Utils
{
    public static class LocationUtil
    {
        public static async Task<LocationProperties> GetAddressLongLat(string address)
        {
            IConfiguration config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", true, true)
               .Build();

            var geoapifyApiKey = config["Geocoding:ApiKey"];
            var geoapifyGeocodingUrl = config["Geocoding:Url"];

            using (HttpClient client = new HttpClient())
            {
                string encodedAddress = Uri.EscapeDataString(address);
                string requestUrl = $"{geoapifyGeocodingUrl}?text={encodedAddress}&apiKey={geoapifyApiKey}";

                HttpResponseMessage response = await client.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response
                // Using JsonSerializer.Deserialize with a custom option to handle case-insensitive property names if needed
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Geoapify uses 'lat', 'lon', 'formatted' which match C# casing here, but good practice
                };
                GeoapifyResponse geoapifyData = JsonSerializer.Deserialize<GeoapifyResponse>(jsonResponse, options);

                if (geoapifyData?.Features != null && geoapifyData.Features.Count > 0)
                {
                    var firstResult = geoapifyData.Features[0].Properties;
                    return new LocationProperties
                    {
                        Lat = firstResult.Lat,
                        Lon = firstResult.Lon,
                    };
                }
                return null; // No results found
            }
        }

        //Get Post have address near input address in range (xx
        public static bool CheckAddressesInRange(double? range, double lon1, double lat1, double lon2, double lat2)
        {
            GeoCoordinate location1 = new GeoCoordinate(lat1, lon1);
            GeoCoordinate location2 = new GeoCoordinate(lat2, lon2);

            double distanceBetween = location1.GetDistanceTo(location2);

            return distanceBetween < range ? true : false;
        }
    }
}
