namespace CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTOs
{
    public record UpdateServiceFeatureDTO(
         string? ServiceFeatureName,
         string? Description,
         string? DefaultValue,
         string? Status  // Optional value; if "DISABLE" then soft-delete (maps to INACTIVE)
    );
}

