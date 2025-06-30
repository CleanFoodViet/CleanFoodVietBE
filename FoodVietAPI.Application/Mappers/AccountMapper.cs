using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.AccountDTOs;
using CleanFoodVietAPI.Application.DTOs.AuthDTOs;
using CleanFoodVietAPI.Application.Utils;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.AccountEnums;

namespace CleanFoodVietAPI.Application.Mappers
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {
            CreateMap<RegisterDTO, Account>()
                .ForMember(des => des.AccountId, opt => opt.MapFrom(src => Ulid.NewUlid()))
                .ForMember(des => des.Password, opt => opt.MapFrom(src => HashUtil.PasswordHash(src.Password)))
                .ForMember(des => des.Gender, opt => opt.MapFrom(src => src.Gender ?? AccountGenderEnum.MALE.ToString()))
                .ForMember(des => des.Avatar, opt => opt.MapFrom(src => "None"))
                .ForMember(des => des.Status, opt => opt.MapFrom(src => AccountStatusEnum.ACTIVE.ToString()))
                .ForMember(des => des.IsVerified, opt => opt.MapFrom(src => false))
                .ForMember(des => des.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<GardenerRegisterDTO, Account>()
                .ForMember(des => des.AccountId, opt => opt.MapFrom(src => Ulid.NewUlid()))
                .ForMember(des => des.Password, opt => opt.MapFrom(src => HashUtil.PasswordHash(src.Password)))
                .ForMember(des => des.Gender, opt => opt.MapFrom(src => src.Gender ?? AccountGenderEnum.MALE.ToString()))
                .ForMember(des => des.Avatar, opt => opt.MapFrom(src => "None"))
                .ForMember(des => des.Status, opt => opt.MapFrom(src => AccountStatusEnum.ACTIVE.ToString()))
                .ForMember(des => des.IsVerified, opt => opt.MapFrom(src => false))
                .ForMember(des => des.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<UpdateAccountDTO, Account>()
                .ForMember(des => des.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForAllMembers(opt => opt.Condition((src, des, srcMember) => srcMember != null));

            CreateMap<Account, RegisterResponse>();
        }
    }
}
