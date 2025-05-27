using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Data.Entities
{
    public class PackageServiceFeature
    {
        [Key]
        public Ulid PackageServiceFeatureId { get; set; }
        public Ulid ServicePackageId { get; set; }
        public Ulid ServiceFeatureId { get; set; }
        public string FeatureValue { get; set; } = null!;

        public virtual ServiceFeature ServiceFeature { get; set; } = null!;
        public virtual ServicePackage ServicePackage { get; set; } = null!;
    }
}
