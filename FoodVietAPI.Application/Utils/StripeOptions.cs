using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Utils
{
    public class StripeOptions
    {
        public string SecretKey { get; set; } = null!;
        public string PublishableKey { get; set; } = null!;
    }
}

