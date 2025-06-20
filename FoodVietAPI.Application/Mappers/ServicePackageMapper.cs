using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ServicePackageDTOs;
using CleanFoodVietAPI.Data.Entities;

namespace CleanFoodVietAPI.Application.Mappers
{
    public class ServicePackageMapper : Profile
    {
        public ServicePackageMapper()
        {
            // Mapping for reading
            CreateMap<ServicePackage, ServicePackageDTO>()
                .ForMember(dest => dest.Features,
                           opt => opt.MapFrom(src => src.ServicePackageFeatures.Select(psf => psf.ServiceFeature).ToList()));

            // Mapping for creating
            CreateMap<CreateServicePackageDTO, ServicePackage>()
                .ForMember(dest => dest.ServicePackageId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.ServicePackageFeatures, opt => opt.Ignore());

            // Mapping for updating (only PackageName and Description allowed)
            CreateMap<UpdateServicePackageDTO, ServicePackage>()
                .ForMember(dest => dest.Price, opt => opt.Ignore())
                .ForMember(dest => dest.Duration, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());
        }
    }
}
