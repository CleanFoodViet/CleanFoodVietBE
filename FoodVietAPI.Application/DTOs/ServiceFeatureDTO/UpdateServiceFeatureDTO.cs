using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTO
{
    public record UpdateServiceFeatureDTO(
         string? ServiceFeatureName,
         string? Description,
         string? DefaultValue,
         string? Status  // Optional value; if "DISABLE" then soft-delete (maps to INACTIVE)
    );
}

