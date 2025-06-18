using CleanFoodVietAPI.Data.Enums.ServiceFeatureEnums;

namespace CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTOs
{
    public class CreateServiceFeatureDTO
    {
        public string ServiceFeatureName { get; set; } = null!;
        public string? Description { get; set; }
        public string DefaultValue { get; set; } = null!;

        // Include the Action as part of the creation
        public ServiceFeatureActionEnum Action { get; set; }
    }
}

