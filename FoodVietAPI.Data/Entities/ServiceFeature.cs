using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class ServiceFeature
    {
        [Key]
        public Ulid ServiceFeatureId { get; set; }
        public string ServiceFeatureName { get; set; } = null!;
        public string? Description { get; set; }
        public int DefaultValue { get; set; }
        public string Action { get; set; } = null!;
        public string Status { get; set; } = null!;

        public virtual ICollection<PackageServiceFeature> ServicePackageFeatures { get; set; } = new HashSet<PackageServiceFeature>();
    }
}
