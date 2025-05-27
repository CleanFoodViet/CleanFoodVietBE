using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Data.Entities
{
    public class SubscriptionContract
    {
        [Key]
        public Ulid SubscriptionId { get; set; }
        public Ulid GardenerId { get; set; }
        public Ulid ServicePackageId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = null!;
        public string SubscriptionType { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual ServicePackage ServicePackage { get; set; } = null!;
    }
}
