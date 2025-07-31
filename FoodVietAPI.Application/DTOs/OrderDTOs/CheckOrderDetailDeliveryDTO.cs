using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.OrderDTOs
{
    public class CheckOrderDetailDeliveryDTO
    {
        public Ulid OrderDetailId { get; set; }
        public int RemainDeliveryQuantity { get; set; }
    }
}
