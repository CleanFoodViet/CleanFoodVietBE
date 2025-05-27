using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Data.Entities
{
    public class ServicePackageOrderPayment
    {
        [Key]
        public Ulid ServicePackageOrderPaymentId { get; set; }
        public Ulid ServicePackageOrderId { get; set; }
        public decimal PaymentAmount { get; set; }
        public string Currency { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = null!;

        public virtual ServicePackageOrder ServicePackageOrder { get; set; } = null!;
    }
}
