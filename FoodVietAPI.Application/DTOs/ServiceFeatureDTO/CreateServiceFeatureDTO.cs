using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTO
{
    public record CreateServiceFeatureDTO
    (
         string ServiceFeatureName,
         string? Description,
         string DefaultValue
    );
}

