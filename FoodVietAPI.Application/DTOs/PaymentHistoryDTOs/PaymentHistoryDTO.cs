using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.PaymentHistoryDTOs
{
    public class PaymentHistoryDTO
    {
        public string OrderId { get; set; } = null!;
        public string PaymentId { get; set; } = null!;
        public decimal PaymentAmount { get; set; }
        public string Currency { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public decimal OrderTotalAmount { get; set; }
        public string OrderStatus { get; set; } = null!;
        public string PackageName { get; set; } = null!;
    }
}
