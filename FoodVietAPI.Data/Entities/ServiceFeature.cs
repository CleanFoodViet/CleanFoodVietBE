using CleanFoodVietAPI.Data.Enums;
using CleanFoodVietAPI.Data.Enums.ServiceFeatureEnums;
using System;
using System.Collections.Generic;
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

        // New status property
        public ServiceFeatureStatusEnum Status { get; set; } = ServiceFeatureStatusEnum.ACTIVE;

        public virtual ICollection<PackageServiceFeature> ServicePackageFeatures { get; set; } = new HashSet<PackageServiceFeature>();
    }
}
