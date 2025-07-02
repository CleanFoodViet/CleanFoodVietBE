using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.CartDTOs;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.OrderEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Mappers
{
    public class OrderMapper : Profile
    {
        public OrderMapper()
        {
            CreateMap<CartDTO, Order>()
                .ForMember(des => des.OrderId, opt => opt.MapFrom(src => Ulid.NewUlid()))
                .ForMember(des => des.Status, opt => opt.MapFrom(src => OrderStatusEnum.PENDING.ToString()))
                .ForMember(des => des.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<CartItemDTO, OrderDetail>()
                .ForMember(des => des.OrderDetailId, opt => opt.MapFrom(src => Ulid.NewUlid()))
                .ForMember(des => des.DeliveryStatus, opt => opt.MapFrom(src => OrderDetailStatusEnum.DELIVERING.ToString()))
                .ForMember(des => des.OrderId, opt => opt.MapFrom(
                        (src, des, member, context) => context.Items["OrderId"]
                    ));
        }
    }
}
