using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs
{
    public record LocationDTO
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
