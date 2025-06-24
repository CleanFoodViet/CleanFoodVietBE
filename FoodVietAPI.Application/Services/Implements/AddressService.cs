using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.AddressDTOs;
using CleanFoodVietAPI.Application.DTOs.AuthDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Application.Utils;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Principal;

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

        public async Task<GetAddressDTO> GetAccountAddressDetail(string accountId, string addressId)
        {
            Ulid accId = Ulid.Parse(accountId);
            Ulid addressID = Ulid.Parse(addressId);

            var account = await _unitOfWork.GetRepository<Account>()
                .GetAsync(predicate: acc => acc.AccountId == accId);
            if (account == null) throw new BadHttpRequestException("Account is not found");

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

            await _unitOfWork.GetRepository<Address>().InsertAsync(newAddress);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occurs when create address");

            
        }

        public async Task UpdateAddress(AddressDTO updateData, string addressId, string accountId)
        {
            Ulid accId = Ulid.Parse(accountId);
            var account = await _unitOfWork.GetRepository<Account>()
                .GetAsync(predicate: acc => acc.AccountId == accId);
            if (account == null) throw new BadHttpRequestException("Account is not found");

            Ulid addressID = Ulid.Parse(addressId);
            var address = await _unitOfWork.GetRepository<Address>()
                .GetAsync(predicate: address => address.AddressId == addressID);
            if (address == null) throw new BadHttpRequestException("Address is not found");

            _mapper.Map(updateData, address);
            _unitOfWork.GetRepository<Address>().UpdateAsync(address);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occurs when update address");
        }

        public async Task DeletAddress(string addressId, string accountId)
        {
            Ulid accId = Ulid.Parse(accountId);
            var account = await _unitOfWork.GetRepository<Account>()
                .GetAsync(predicate: acc => acc.AccountId == accId);
            if (account == null) throw new BadHttpRequestException("Account is not found");

            Ulid addressID = Ulid.Parse(addressId);
            var address = await _unitOfWork.GetRepository<Address>()
                .GetAsync(predicate: address => address.AddressId == addressID);
            if (address == null) throw new BadHttpRequestException("Address is not found");

            _unitOfWork.GetRepository<Address>().DeleteAsync(address);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occurs when delete address");
        }
    }
}
