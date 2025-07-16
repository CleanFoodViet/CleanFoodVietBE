using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.AuthDTOs;
using CleanFoodVietAPI.Application.DTOs.CertificateDTOs;
using CleanFoodVietAPI.Data.Entities;

namespace CleanFoodVietAPI.Application.Mappers
{
    public class CertificateMapper : Profile
    {
        public CertificateMapper()
        {
            CreateMap<Certificate, GetCertificateDTO>();
            CreateMap<Certificate, CertificateDTO>();

            CreateMap<GardenerRegisterDTO, Certificate>()
                .ForMember(des => des.CertificateId, opt => opt.MapFrom(src => Ulid.NewUlid()));

            CreateMap<CertificateDTO, Certificate>()
                .ForMember(des => des.CertificateId, opt => opt.MapFrom(src => Ulid.NewUlid()))
                .ForMember(des => des.GardenerId, opt => opt.Ignore());

            CreateMap<CreateCertificateDTO, Certificate>()
               .ForMember(des => des.CertificateId, opt => opt.MapFrom(src => Ulid.NewUlid()))
               .ForMember(des => des.GardenerId, opt => opt.Ignore());
        }
    }
}
