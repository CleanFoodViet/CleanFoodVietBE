using AutoMapper;
using CleanFoodVietAPI.Application.DTOs;
using CleanFoodVietAPI.Application.DTOs.AddressDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Application.Utils;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class AddressService : BaseService<AddressService>, IAddressService
    {
        public AddressService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<AddressService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<List<GetAddressDTO>> GetAccountAddressList(string accountId)
        {
            Ulid accId = Ulid.Parse(accountId);
            var account = await _unitOfWork.GetRepository<Account>()
               .GetAsync(predicate: acc => acc.AccountId == accId);
            if (account == null) throw new BadHttpRequestException("Account is not found");

            var accountAddresses = await _unitOfWork.GetRepository<Address>()
                .GetListAsync(predicate: a => a.AccountId == accId,
                              selector: a => new GetAddressDTO
                              {
                                  AddressId = a.AddressId,
                                  AddressLine = a.AddressLine,
                                  City = a.City,
                                  Country = a.Country,
                                  PostalCode = a.PostalCode,
                                  Province = a.Province
                              });

            return accountAddresses.ToList();
        }

        public async Task<GetAddressDTO> GetAddressDetail(string addressId)
        {
            Ulid addressID = Ulid.Parse(addressId);

            var address = await _unitOfWork.GetRepository<Address>()
                .GetAsync(
                    predicate: address => address.AddressId == addressID,
                    selector: address => new GetAddressDTO
                    {
                        AddressId = address.AddressId,
                        AddressLine = address.AddressLine,
                        City = address.City,
                        Country = address.Country,
                        PostalCode = address.PostalCode,
                        Province = address.Province
                    });
            if (address == null) throw new BadHttpRequestException("Address is not found");

            return address;
        }

        public async Task CreateAddresss(AddressDTO createData, string accountId)
        {
            Ulid accId = Ulid.Parse(accountId);
            var account = await _unitOfWork.GetRepository<Account>()
                .GetAsync(predicate: acc => acc.AccountId == accId);
            if (account == null) throw new BadHttpRequestException("Account is not found");

            Address newAddress = _mapper.Map<Address>(createData);
            newAddress.AccountId = accId;

            //Get longitude and latitude
            //LocationProperties location = await LocationUtil.GetAddressLongLat(newAddress.AddressLine);
            //if (location == null) throw new BadHttpRequestException("Cannot found the location");
            newAddress.Longitude = 0;
            newAddress.Latitude = 0;

            await _unitOfWork.GetRepository<Address>().InsertAsync(newAddress);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occurs when create address");
        }

        public async Task UpdateAddress(AddressDTO updateData, string addressId)
        {
            Ulid addressID = Ulid.Parse(addressId);
            var address = await _unitOfWork.GetRepository<Address>()
                .GetAsync(predicate: address => address.AddressId == addressID);
            if (address == null) throw new BadHttpRequestException("Address is not found");

            address.AddressLine = String.IsNullOrEmpty(updateData.AddressLine) ? address.AddressLine : updateData.AddressLine;
            address.City = String.IsNullOrEmpty(updateData.City) ? address.City : updateData.City;
            address.Province = String.IsNullOrEmpty(updateData.Province) ? address.Province : updateData.Province;
            address.PostalCode = String.IsNullOrEmpty(updateData.PostalCode) ? address.PostalCode : updateData.PostalCode;
            address.Country = String.IsNullOrEmpty(updateData.Country) ? address.Country : updateData.Country;

            //Get longitude and latitude
            if(updateData.AddressLine != null)
            {
                //LocationProperties location = await LocationUtil.GetAddressLongLat(updateData.AddressLine);
                //if (location == null) throw new BadHttpRequestException("Cannot found the location");
                address.Longitude = 0;
                address.Latitude = 0;
            }

            _unitOfWork.GetRepository<Address>().UpdateAsync(address);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occurs when update address (DB query error)");
        }

        public async Task DeletAddress(string addressId)
        {
            Ulid addressID = Ulid.Parse(addressId);
            var address = await _unitOfWork.GetRepository<Address>()
                .GetAsync(predicate: address => address.AddressId == addressID);
            if (address == null) throw new BadHttpRequestException("Address is not found");

            _unitOfWork.GetRepository<Address>().DeleteAsync(address);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occurs when delete address (DB query error)");
        }
    }
}
