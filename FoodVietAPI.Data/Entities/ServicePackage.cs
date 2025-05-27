using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Data.Entities
{
    public class ServicePackage
    {
        [Key]
        public Ulid ServicePackageId { get; set; }
        public string PackageName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<SubscriptionContract> ServicePackageContracts { get; set; } = new HashSet<SubscriptionContract>();
        public virtual ICollection<PackageServiceFeature> ServicePackageFeatures { get; set; } = new HashSet<PackageServiceFeature>();
        public virtual ICollection<ServicePackageOrder> ServicePackageOrders { get; set; } = new HashSet<ServicePackageOrder>();
    }
}
