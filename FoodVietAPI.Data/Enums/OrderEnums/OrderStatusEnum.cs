using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Data.Enums.OrderEnums
{
    public enum OrderStatusEnum
    {
        PENDING,
        PREPARING,
        DELIVERING,
        DELIVERED,
        COMPLETED,
        CANCELLED
    }
}
