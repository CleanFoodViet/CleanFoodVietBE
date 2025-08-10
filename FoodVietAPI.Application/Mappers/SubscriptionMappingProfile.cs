using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.SubscriptionContractDTOs;
using CleanFoodVietAPI.Application.DTOs.SubscriptionContractBenefitDTOs;
using CleanFoodVietAPI.Application.DTOs.SubscriptionOrderDTOs;
using CleanFoodVietAPI.Application.DTOs.SubscriptionOrderPaymentDTOs;
using CleanFoodVietAPI.Data.Entities;

namespace CleanFoodVietAPI.Application.Mappers
{
    public class SubscriptionMappingProfile : Profile
    {
        public SubscriptionMappingProfile()
        {
            CreateMap<SubscriptionContractBenefit, SubscriptionContractBenefitDTO>()
                .ForMember(d => d.CreatedAt, opt => opt.MapFrom(s => s.CreatedAt))
                .ForMember(d => d.UpdatedAt, opt => opt.MapFrom(s => s.UpdatedAt));

            CreateMap<SubscriptionContract, SubscriptionContractDTO>()
                .ForMember(d => d.Benefits,
                           opt => opt.MapFrom(src => src.SubscriptionContractBenefits));

            CreateMap<ServicePackageOrder, ServicePackageOrderDTO>()
                .ForMember(d => d.Payments,
                           opt => opt.MapFrom(src => src.ServicePackageOrderPayments));

            CreateMap<ServicePackageOrderPayment, SubscriptionOrderPaymentDTO>();

            CreateMap<SubscriptionContractBenefit, CurrentSubscriptionContractBenefitDTO>();
        }
    }
}
