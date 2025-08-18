using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.CartDTOs;
using CleanFoodVietAPI.Data.Entities;

namespace CleanFoodVietAPI.Application.Mappers
{
    public class CartMapper : Profile
    {
        public CartMapper()
        {
            CreateMap<CartItem, CartItemDTO>()
                .ForMember(des => des.ProductName, opt => opt.MapFrom(src => src.Product.ProductName));

            CreateMap<CartDTO, Cart>()
                .ForMember(des => des.CartId, opt => opt.MapFrom(src => src.CartId ?? Ulid.NewUlid()));

            CreateMap<CartItemDTO, CartItem>()
                .ForMember(des => des.CartItemId, opt => opt.MapFrom(src => src.CartItemId ?? Ulid.NewUlid()));
        }
    }
}
