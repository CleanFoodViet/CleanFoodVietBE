using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.OrderDeliveryDTOs;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.OrdeDeliveryEnums;

namespace CleanFoodVietAPI.Application.Mappers
{
    public class OrderDeliveryMapper : Profile
    {
        public OrderDeliveryMapper()
        {
            CreateMap<OrderDeliveryDetail, OrderDeliveryDetailDTO>()
                .ForMember(des => des.ProductId, opt => opt.MapFrom(src => src.OrderDetail.Product.ProductId))
                .ForMember(des => des.ProductName, opt => opt.MapFrom(src => src.OrderDetail.Product.ProductName));

            CreateMap<OrderDeliveryDTO, OrderDelivery>()
                .ForMember(des => des.OrderDeliveryId, opt => opt.MapFrom(src => Ulid.NewUlid()))
                .ForMember(des => des.DeliveryStatus, opt => opt.MapFrom(src => OrderDeliveryStatusEnum.DELIVERED.ToString()))
                .ForMember(des => des.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<CreateOrderDelieryDTO, OrderDelivery>()
                .ForMember(des => des.OrderDeliveryId, opt => opt.MapFrom(src => Ulid.NewUlid()))
                .ForMember(des => des.DeliveryStatus, opt => opt.MapFrom(src => OrderDeliveryStatusEnum.DELIVERING.ToString()))
                .ForMember(des => des.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<CreateOrderDelieryDetailsDTO, OrderDeliveryDetail>()
                .ForMember(des => des.OrderDeliveryDetailId, opt => opt.MapFrom(src => Ulid.NewUlid()))
                .ForMember(des => des.OrderDeliveryId, opt => opt.MapFrom(
                        (src, des, member, context) => context.Items["OrderDeliveryId"]
                    ));

        }
    }
}
