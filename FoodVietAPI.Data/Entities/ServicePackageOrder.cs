using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class ServicePackageOrder
    {
        [Key]
        public Ulid ServicePackageOrderId { get; set; }
        public Ulid GardenerId { get; set; }
        public Ulid ServicePackageId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public virtual ServicePackage ServicePackage { get; set; } = null!;
        public virtual Account Gardener { get; set; } = null!;
        public virtual ICollection<ServicePackageOrderPayment> ServicePackageOrderPayments { get; set; } = new HashSet<ServicePackageOrderPayment>();
    }
}
