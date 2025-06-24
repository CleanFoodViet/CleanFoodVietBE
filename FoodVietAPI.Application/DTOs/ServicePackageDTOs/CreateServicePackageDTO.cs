using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.ServicePackageDTOs
{
    public class CreateServicePackageDTO
    {
        public string PackageName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }

        // List of feature IDs that the admin wants to include.
        public List<Ulid> FeatureIds { get; set; } = new();
    }
}

