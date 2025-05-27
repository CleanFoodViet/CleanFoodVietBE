using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs
{
    public record ErrorDTO
    (
        int StatusCode,
        string Error,
        DateTime TimeStamp
    );
}
