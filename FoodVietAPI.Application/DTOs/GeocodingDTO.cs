using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
