using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Data.Exceptions
{
    public class UpdateRestrictedException : Exception
    {
        public UpdateRestrictedException()
        {
        }

        public UpdateRestrictedException(string? message) : base(message)
        {
        }
    }
}
