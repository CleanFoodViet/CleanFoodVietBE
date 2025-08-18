using CleanFoodVietAPI.Application.DTOs.AddressDTOs;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IAddressService
    {
        Task<List<GetAddressDTO>> GetAccountAddressList(string accountId);
        Task<GetAddressDTO> GetAddressDetail(string addressId);
        Task CreateAddresss(AddressDTO createData, string accountId);
        Task UpdateAddress(AddressDTO updateData, string addressId);
        Task DeletAddress(string addressId);
    }
}
