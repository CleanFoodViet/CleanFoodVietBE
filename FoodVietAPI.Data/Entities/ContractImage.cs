using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Data.Entities
{
    public class ContractImage
    {
        public Ulid ContractImageId { get; set; }
        public Ulid OrderId { get; set; }
        public string ImageUrl { get; set; } = null!;

        public virtual Order Order { get; set; } = null!;
    }
}
