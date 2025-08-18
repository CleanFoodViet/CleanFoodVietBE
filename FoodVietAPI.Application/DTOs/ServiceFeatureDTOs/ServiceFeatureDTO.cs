namespace CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTOs
{
    public class ServiceFeatureDTO
    {
        public Ulid ServiceFeatureId { get; set; }
        public string ServiceFeatureName { get; set; }
        public string? Description { get; set; }
        public int DefaultValue { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }

        public ServiceFeatureDTO(
            Ulid serviceFeatureId,
            string serviceFeatureName,
            string? description,
            int defaultValue,
            string action,
            string status)
        {
            ServiceFeatureId = serviceFeatureId;
            ServiceFeatureName = serviceFeatureName;
            Description = description;
            DefaultValue = defaultValue;
            Action = action;
            Status = status;
        }

        public ServiceFeatureDTO() { }
    }
}

