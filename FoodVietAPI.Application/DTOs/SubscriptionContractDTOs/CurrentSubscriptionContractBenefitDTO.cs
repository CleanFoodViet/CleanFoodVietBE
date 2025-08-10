using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.SubscriptionContractDTOs
{
    public record CurrentSubscriptionContractBenefitDTO
    {
        public int DefaultValue { get; set; }
        public int RemainingValue { get; set; }
    }
}
