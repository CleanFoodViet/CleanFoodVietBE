using CleanFoodVietAPI.Application.DTOs.AddressDTOs;
using CleanFoodVietAPI.Data.Entities;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IAddressService
    {
        Task<List<GetAddressDTO>> GetAccountAddressList(string accountId);
        Task<GetAddressDTO> GetAccountAddressDetail(string accountId, string addressId);
        Task CreateAddresss(AddressDTO createData, string accountId);
        Task UpdateAddress(AddressDTO updateData, string addressId, string accountId);
        Task DeletAddress(string addressId, string accountId);
    }
}
