using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Data.Exceptions
{
    public class DeletionRestrictedException : Exception
    {
        public DeletionRestrictedException(string message) : base(message)
        {
            
        }
    }
}
