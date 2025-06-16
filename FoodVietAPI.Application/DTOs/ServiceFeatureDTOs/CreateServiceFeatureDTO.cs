namespace CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTOs
{
    public record CreateServiceFeatureDTO
    (
         string ServiceFeatureName,
         string? Description,
         string DefaultValue
    );
}

