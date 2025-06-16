using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class PackageServiceFeature
    {
        [Key]
        public Ulid PackageServiceFeatureId { get; set; }
        public Ulid ServicePackageId { get; set; }
        public Ulid ServiceFeatureId { get; set; }

        public virtual ServiceFeature ServiceFeature { get; set; } = null!;
        public virtual ServicePackage ServicePackage { get; set; } = null!;
    }
}
