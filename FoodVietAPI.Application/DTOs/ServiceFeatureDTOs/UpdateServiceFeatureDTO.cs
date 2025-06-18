using CleanFoodVietAPI.Data.Enums.ServiceFeatureEnums;

namespace CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTOs
{
    public class UpdateServiceFeatureDTO
    {
        public string ServiceFeatureName { get; set; } = null!;
        public string? Description { get; set; }

        // Allow updating of the Action if needed. DefaultValue is intentionally omitted.
        public ServiceFeatureActionEnum Action { get; set; }

        // You might also allow updating the Status, e.g. for a soft-delete.
        public string? Status { get; set; }
    }
}

