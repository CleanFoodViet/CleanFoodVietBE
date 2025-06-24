using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ProductDTOs;
using CleanFoodVietAPI.Application.DTOs.ProductPriceDTOs;
using CleanFoodVietAPI.Data.Entities;

namespace CleanFoodVietAPI.Application.Mappers
{
    public class ProductPriceMapper : Profile
    {
        public ProductPriceMapper()
        {
            CreateMap<CreateProductDTO, ProductPrice>()
                .ForMember(src => src.ProductPriceId, opt => opt.MapFrom(src => Ulid.NewUlid()))
                .ForMember(src => src.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(src => src.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
