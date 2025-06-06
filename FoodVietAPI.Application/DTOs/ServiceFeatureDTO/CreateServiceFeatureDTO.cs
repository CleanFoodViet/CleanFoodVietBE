namespace CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTO
{
    public record CreateServiceFeatureDTO
    (
         string ServiceFeatureName,
         string? Description,
         string DefaultValue
    );
}

