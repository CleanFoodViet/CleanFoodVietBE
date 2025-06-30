using CleanFoodVietAPI.Application.DTOs.CartDTOs;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface ICartService
    {
        Task<List<CartDTO>> GetRetailerCarts(string retailerId);
        Task ModifyCart(string reatilerId, List<CartDTO> request);
        Task DeleteCart(string retailerId);
    }
}
