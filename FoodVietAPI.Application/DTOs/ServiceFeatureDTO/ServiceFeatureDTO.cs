using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTO
{
    public record ServiceFeatureDTO
    (
         Ulid ServiceFeatureId,
         string ServiceFeatureName,
         string? Description,
         string DefaultValue
    );
}

