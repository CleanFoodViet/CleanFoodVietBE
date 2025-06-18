using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ServicePackageDTOs;
using CleanFoodVietAPI.Data.Entities;

namespace CleanFoodVietAPI.Application.Mappers
{
    public class ServicePackageMapper : Profile
    {
        public ServicePackageMapper()
        {
            // Mapping for reading: map from ServicePackage to ServicePackageDTO (including features)
            CreateMap<ServicePackage, ServicePackageDTO>()
                .ForMember(dest => dest.Features,
                           opt => opt.MapFrom(src => src.ServicePackageFeatures.Select(psf => psf.ServiceFeature).ToList()));

            // Mapping for creation: exclude properties set in code (ID, CreatedAt, Status, and the join collection)
            CreateMap<CreateServicePackageDTO, ServicePackage>()
                .ForMember(dest => dest.ServicePackageId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.ServicePackageFeatures, opt => opt.Ignore());
        }
    }
}
