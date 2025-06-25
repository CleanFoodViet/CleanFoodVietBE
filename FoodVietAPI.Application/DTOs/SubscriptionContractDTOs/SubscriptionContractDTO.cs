using CleanFoodVietAPI.Application.DTOs.SubscriptionContractBenefitDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.SubscriptionContractDTOs
{
    public class SubscriptionContractDTO
    {
        public Ulid SubscriptionId { get; set; }
        public Ulid GardenerId { get; set; }
        public Ulid ServicePackageId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = null!;
        public string SubscriptionType { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public List<SubscriptionContractBenefitDTO> Benefits { get; set; } = new();
    }
}
