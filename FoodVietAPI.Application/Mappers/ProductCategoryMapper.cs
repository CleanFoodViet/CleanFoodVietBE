using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ProductCategoryDTOs;
using CleanFoodVietAPI.Application.DTOs.ProductDTOs;
using CleanFoodVietAPI.Data.Entities;

namespace CleanFoodVietAPI.Application.Mappers
{
    public class ProductCategoryMapper : Profile
    {
        public ProductCategoryMapper()
        {
            CreateMap<CreateProductCategoryDTO, ProductCategory>()
                .ForMember(des => des.ProductCategoryId, opt => opt.MapFrom(src => Ulid.NewUlid()));

            CreateMap<UpdateProductCategoryDTO, ProductCategory>()
                .ForAllMembers(opt => opt.Condition((src, des, srcMember) => srcMember != null));

            CreateMap<ProductCategory, GetProductCategoryDTO>();

            CreateMap<CreateProductDTO, ProductCategory>()
                .ForMember(src => src.ProductCategoryId, opt => opt.MapFrom(src => Ulid.NewUlid())); ;
        }
    }
}
