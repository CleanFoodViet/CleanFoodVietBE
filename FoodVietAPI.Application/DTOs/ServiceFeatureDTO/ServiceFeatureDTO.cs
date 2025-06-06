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

