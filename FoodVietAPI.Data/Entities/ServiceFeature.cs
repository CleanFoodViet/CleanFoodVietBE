using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Data.Entities
{
    public class ServiceFeature
    {
        [Key]
        public Ulid ServiceFeatureId { get; set; }
        public string ServiceFeatureName { get; set; } = null!;
        public string? Description { get; set; }
        public string DefaultValue { get; set; } = null!;

        public virtual ICollection<PackageServiceFeature> ServicePackageFeatures { get; set; } = new HashSet<PackageServiceFeature>();
    }
}
