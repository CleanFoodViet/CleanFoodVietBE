using CleanFoodVietAPI.Application.DTOs.SubscriptionOrderPaymentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.SubscriptionOrderDTOs
{
    public class ServicePackageOrderDTO
    {
        public Ulid ServicePackageOrderId { get; set; }
        public Ulid GardenerId { get; set; }
        public Ulid ServicePackageId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public List<SubscriptionOrderPaymentDTO> Payments { get; set; } = new();
    }
}
