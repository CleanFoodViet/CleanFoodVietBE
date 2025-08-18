namespace CleanFoodVietAPI.Application.DTOs
{
    public class GeoapifyResponse
    {
        public List<Feature> Features { get; set; }
    }

    public class Feature
    {
        public LocationProperties Properties { get; set; }
    }

    public class LocationProperties
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
