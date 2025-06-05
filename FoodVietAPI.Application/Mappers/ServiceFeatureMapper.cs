using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTO;
using CleanFoodVietAPI.Data.Entities;

namespace CleanFoodVietAPI.Application.Mappers
{
    public class ServiceFeatureMapper : Profile
    {
        public ServiceFeatureMapper()
        {
            // For creating a new ServiceFeature
            CreateMap<CreateServiceFeatureDTO, ServiceFeature>()
                .ForMember(dest => dest.ServiceFeatureId,
                           opt => opt.MapFrom(src => Ulid.NewUlid()))
                // If our create DTO doesn't include a status, either ignore it or set the default.
                .ForMember(dest => dest.Status, opt => opt.Ignore());
            // (The entity already defaults to ACTIVE.)

            // Mapping from entity to DTO (assuming your ServiceFeatureDTO includes a property for Status)
            CreateMap<ServiceFeature, ServiceFeatureDTO>();
        }
    }
}
