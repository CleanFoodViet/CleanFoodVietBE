using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.SubscriptionContractBenefitDTOs
{
    public class SubscriptionContractBenefitDTO
    {
        public Ulid SubscriptionContractBenefitId { get; set; }
        public string BenefitType { get; set; } = null!;
        public int DefaultValue { get; set; }
        public int RemainingValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }   
    }
}

