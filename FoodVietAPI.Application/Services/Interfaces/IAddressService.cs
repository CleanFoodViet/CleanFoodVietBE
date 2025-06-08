using CleanFoodVietAPI.Application.DTOs.AddressDTOs;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IAddressService
    {
        Task<List<AddressDTO>> GetAccountAddresses(string accountId);
    }
}
