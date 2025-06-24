using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.ServicePackageDTOs
{
    public class UpdateServicePackageDTO
    {
        // Only allow updating the package name and description.
        public string? PackageName { get; set; }
        public string? Description { get; set; }
    }
}

