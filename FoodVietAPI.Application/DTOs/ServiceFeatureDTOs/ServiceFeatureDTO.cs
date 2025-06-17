namespace CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTOs
{
    public record ServiceFeatureDTO
    (
         Ulid ServiceFeatureId,
         string ServiceFeatureName,
         string? Description,
         int DefaultValue,
         string Action
    );
}

