using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.CartDTOs
{
    public record CartDTO
    {
        //Cart Data Field
        public Ulid CartId { get; set; }
        public Ulid RetailerId { get; set; }
        public Ulid GardenerId { get; set; }
        public DateTime UpdatedAt { get; set; }

        //Cart Itens
        public List<CartItemDTO> cartItems { get; set; } = null!;
    }
}
