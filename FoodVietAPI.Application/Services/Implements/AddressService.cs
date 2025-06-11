using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.AddressDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
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

        public async Task<List<AddressDTO>> GetAccountAddressList(string accountId)
        {
            Ulid accId = Ulid.Parse(accountId);
            var accountAddresses = await _unitOfWork.GetRepository<Address>()
                .GetListAsync(predicate: a => a.AccountId == accId,
                              selector: a => new AddressDTO
                              {
                                  AddressId = a.AddressId,
                                  AddressLine1 = a.AddressLine1,
                                  AddressLine2 = a.AddressLine2,
                                  City = a.City,
                                  Country = a.Country,
                                  PostalCode = a.PostalCode,
                                  Province = a.Province
                              });

            return accountAddresses.ToList();
        }
    }
}
