using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Data.Enums.ReportEnums
{
    public enum ReportStatusEnum
    {
        PENDING,
        INPROGRESS,
        ESCALATED,
        DUPLICATED,
        RESOLVED,
        REJECTED,
        CLOSED
    }
}
