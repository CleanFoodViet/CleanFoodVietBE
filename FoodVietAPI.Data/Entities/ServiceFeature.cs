using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class ServiceFeature
    {
        [Key]
        public Ulid ServiceFeatureId { get; set; }
        public string ServiceFeatureName { get; set; } = null!;
        public string? Description { get; set; }
        public string DefaultValue { get; set; } = null!;
        public string Status { get; set; } = null!;

        public virtual ICollection<PackageServiceFeature> ServicePackageFeatures { get; set; } = new HashSet<PackageServiceFeature>();
    }
}
