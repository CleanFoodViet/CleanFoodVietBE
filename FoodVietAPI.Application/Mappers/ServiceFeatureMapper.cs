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
                           opt => opt.MapFrom(src => Ulid.NewUlid()));

            // Existing mapping from entity to DTO (if needed)
            CreateMap<ServiceFeature, ServiceFeatureDTO>();
        }
    }
}
