using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.AddressDTOs;
using CleanFoodVietAPI.Data.Entities;

namespace CleanFoodVietAPI.Application.Mappers
{
    public class AddressMapper : Profile
    {
        public AddressMapper()
        {
            CreateMap<Address, AddressDTO>();
        }
    }
}
